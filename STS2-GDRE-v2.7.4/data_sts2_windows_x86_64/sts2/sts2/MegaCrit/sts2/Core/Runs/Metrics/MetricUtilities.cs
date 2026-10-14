using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Runs.Metrics
{
	// Token: 0x0200011A RID: 282
	[NullableContext(1)]
	[Nullable(0)]
	public static class MetricUtilities
	{
		/// <summary>
		/// Uploads run history to the metrics server.
		/// DO NOT UPLOAD WHEN:
		/// - Abandon
		/// - Not a release build
		/// - The run ended before floor 5 (these are invalid runs)
		/// For multiplayer runs, each individual player uploads their individual metrics.
		/// </summary>
		// Token: 0x06000DAC RID: 3500 RVA: 0x00059E98 File Offset: 0x00058098
		public static void UploadRunMetrics(SerializableRun run, bool isVictory, ulong localPlayerId)
		{
			try
			{
				MetricUtilities.UploadRunMetricsInternal(run, isVictory, localPlayerId);
			}
			catch (Exception ex)
			{
				Log.Error("Failed to upload run metrics: " + ex.Message + "\n" + ex.StackTrace, 2);
				SentryService.CaptureException(ex);
			}
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00059EEC File Offset: 0x000580EC
		private static bool ShouldUploadMetrics()
		{
			if (ReleaseInfoManager.Instance.ReleaseInfo == null)
			{
				Log.Info("Skipping metrics upload, this is a debug build.", 2);
				return false;
			}
			if (!SaveManager.Instance.PrefsSave.UploadData)
			{
				Log.Info("Skipping metrics upload, user has upload data unset in settings", 2);
				return false;
			}
			if (SaveManager.Instance.SettingsSave.FullConsole)
			{
				Log.Info("Skipping metrics upload, user has enabled full console", 2);
				return false;
			}
			if (OS.HasFeature("editor"))
			{
				Log.Info("Skipping metrics upload since we're in the editor.", 2);
				return false;
			}
			return true;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00059F68 File Offset: 0x00058168
		private static void UploadRunMetricsInternal(SerializableRun run, bool isVictory, ulong localPlayerId)
		{
			if (!MetricUtilities.ShouldUploadMetrics())
			{
				return;
			}
			if (RunManager.Instance.IsAbandoned)
			{
				Log.Info("Skipping metrics upload, run was abandoned.", 2);
				return;
			}
			if (SaveManager.Instance.Progress.NumberOfRuns <= 1)
			{
				Log.Info("Skipping metrics upload, this is our first run ever.", 2);
				return;
			}
			if (ModManager.IsRunningModded())
			{
				Log.Info("Skipping metrics upload, we're running modded.", 2);
				ModManager.CallMetricsHooks(run, isVictory, localPlayerId);
				return;
			}
			if (run.GameMode != GameMode.Standard)
			{
				return;
			}
			List<MapPointHistoryEntry> list = run.MapPointHistory.SelectMany<List<MapPointHistoryEntry>, MapPointHistoryEntry>((List<MapPointHistoryEntry> logs) => logs).ToList<MapPointHistoryEntry>();
			if (list.Count < 5)
			{
				return;
			}
			LocManager.Instance.StartOverridingLanguageAsEnglish();
			try
			{
				ModelId modelId = ModelId.none;
				List<MapPointHistoryEntry> list2 = run.MapPointHistory.LastOrDefault<List<MapPointHistoryEntry>>();
				MapPointHistoryEntry mapPointHistoryEntry = ((list2 != null) ? list2.LastOrDefault<MapPointHistoryEntry>() : null);
				if (!isVictory && mapPointHistoryEntry != null && mapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().RoomType.IsCombatRoom())
				{
					modelId = mapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().ModelId;
				}
				SerializablePlayer localPlayer = run.Players.First<SerializablePlayer>((SerializablePlayer p) => p.NetId == localPlayerId);
				List<EncounterMetric> list3 = (from e in list
					where e.Rooms.Last<MapPointRoomHistoryEntry>().RoomType.IsCombatRoom()
					select new EncounterMetric(e.Rooms.Last<MapPointRoomHistoryEntry>().ModelId.Entry, int.Min(e.GetEntry(localPlayerId).DamageTaken, localPlayer.MaxHp), e.Rooms.Last<MapPointRoomHistoryEntry>().TurnsTaken + 1)).ToList<EncounterMetric>();
				List<CardChoiceMetric> list4 = (from e in list
					where e.GetEntry(localPlayerId).CardChoices.Count > 0
					select new CardChoiceMetric(e.GetEntry(localPlayerId).CardChoices)).ToList<CardChoiceMetric>();
				List<AncientMetric> list5 = (from e in list
					where e.MapPointType == MapPointType.Ancient
					where e.GetEntry(localPlayerId).AncientChoices.Count > 0
					select new AncientMetric(e, e.GetEntry(localPlayerId))).ToList<AncientMetric>();
				List<ActWinMetric> list6 = new List<ActWinMetric>();
				List<EventChoiceMetric> list7 = new List<EventChoiceMetric>();
				for (int i = 0; i < run.MapPointHistory.Count; i++)
				{
					List<MapPointHistoryEntry> list8 = run.MapPointHistory[i];
					foreach (MapPointHistoryEntry mapPointHistoryEntry2 in list8)
					{
						if (mapPointHistoryEntry2.Rooms.First<MapPointRoomHistoryEntry>().RoomType == RoomType.Event && mapPointHistoryEntry2.GetEntry(localPlayerId).EventChoices.Count != 0 && mapPointHistoryEntry2.MapPointType != MapPointType.Ancient)
						{
							list7.Add(new EventChoiceMetric(mapPointHistoryEntry2, localPlayerId, run.Acts[i]));
						}
					}
					bool flag = i < run.MapPointHistory.Count - 1 || isVictory;
					list6.Add(new ActWinMetric(run.Acts[i].Id.Entry, flag));
				}
				ProgressState progress = SaveManager.Instance.Progress;
				RunMetrics runMetrics = new RunMetrics();
				runMetrics.Ascension = run.Ascension;
				runMetrics.TotalPlaytime = (float)progress.TotalPlaytime;
				runMetrics.TotalWinRate = (float)progress.Wins / (float)progress.NumberOfRuns;
				runMetrics.NumReloads = run.NumReloads;
				ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
				runMetrics.BuildId = ((releaseInfo != null) ? releaseInfo.Version : null) ?? "NON-RELEASE-VERSION";
				runMetrics.BuildType = PlatformUtil.GetPlatformBranch().ToName();
				runMetrics.PlayerId = progress.UniqueId;
				runMetrics.Character = localPlayer.CharacterId;
				runMetrics.NumPlayers = run.Players.Count;
				List<ModelId> list9;
				if (run.Players.Count <= 1)
				{
					list9 = new List<ModelId>();
				}
				else
				{
					list9 = run.Players.Select<SerializablePlayer, ModelId>((SerializablePlayer p) => p.CharacterId).ToList<ModelId>();
				}
				runMetrics.Team = list9;
				runMetrics.Win = isVictory;
				runMetrics.FloorReached = list.Count;
				runMetrics.KilledByEncounter = modelId;
				runMetrics.Deck = localPlayer.Deck.Select<SerializableCard, ModelId>((SerializableCard c) => c.Id);
				runMetrics.Relics = localPlayer.Relics.Select<SerializableRelic, ModelId>((SerializableRelic r) => r.Id);
				runMetrics.RunPlaytime = (float)((run.WinTime > 0L) ? run.WinTime : run.RunTime);
				runMetrics.Encounters = list3;
				runMetrics.CardChoices = list4;
				runMetrics.EventChoices = list7;
				runMetrics.AncientChoices = list5;
				runMetrics.ActWins = list6;
				runMetrics.CampfireUpgrades = (from c in list.Where<MapPointHistoryEntry>((MapPointHistoryEntry e) => e.MapPointType == MapPointType.RestSite).SelectMany<MapPointHistoryEntry, ModelId>((MapPointHistoryEntry e) => e.GetEntry(localPlayerId).UpgradedCards)
					select c.Entry).ToList<string>();
				runMetrics.RelicBuys = (from r in list.SelectMany<MapPointHistoryEntry, ModelId>((MapPointHistoryEntry e) => e.GetEntry(localPlayerId).BoughtRelics)
					select r.Entry).ToList<string>();
				runMetrics.PotionBuys = (from p in list.SelectMany<MapPointHistoryEntry, ModelId>((MapPointHistoryEntry e) => e.GetEntry(localPlayerId).BoughtPotions)
					select p.Entry).ToList<string>();
				runMetrics.ColorlessBuys = (from c in list.SelectMany<MapPointHistoryEntry, ModelId>((MapPointHistoryEntry e) => e.GetEntry(localPlayerId).BoughtColorless)
					select c.Entry).ToList<string>();
				runMetrics.PotionDiscards = (from p in list.SelectMany<MapPointHistoryEntry, ModelId>((MapPointHistoryEntry e) => e.GetEntry(localPlayerId).PotionDiscarded)
					select p.Entry).ToList<string>();
				RunMetrics runMetrics2 = runMetrics;
				MetricUtilities.UploadRunMetrics(runMetrics2);
			}
			finally
			{
				LocManager.Instance.StopOverridingLanguageAsEnglish();
			}
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0005A610 File Offset: 0x00058810
		private static void UploadRunMetrics(RunMetrics run)
		{
			Log.Info("Uploading run metrics...", 2);
			string text = JsonSerializer.Serialize<RunMetrics>(run, MetricsSerializerContext.Default.RunMetrics);
			TaskHelper.RunSafely(MetricUtilities.PutRequest("https://sts2-metric-upload.megacrit.com/record_data/", text, "Run metrics"));
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0005A650 File Offset: 0x00058850
		public static void UploadAchievementMetric(Achievement achievement)
		{
			if (!MetricUtilities.ShouldUploadMetrics())
			{
				return;
			}
			AchievementMetric achievementMetric = new AchievementMetric();
			ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
			achievementMetric.BuildId = ((releaseInfo != null) ? releaseInfo.Version : null) ?? "NON-RELEASE-VERSION";
			achievementMetric.Achievement = achievement.ToString();
			achievementMetric.TotalRuns = SaveManager.Instance.Progress.NumberOfRuns;
			achievementMetric.TotalPlaytime = SaveManager.Instance.Progress.TotalPlaytime;
			achievementMetric.TotalAchievements = SaveManager.Instance.Progress.UnlockedAchievements.Count;
			AchievementMetric achievementMetric2 = achievementMetric;
			string text = JsonSerializer.Serialize<AchievementMetric>(achievementMetric2, MetricsSerializerContext.Default.AchievementMetric);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Uploading achievement metric for achievement ");
			defaultInterpolatedStringHandler.AppendFormatted<Achievement>(achievement);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			string text2 = "https://sts2-metric-upload.megacrit.com/record_achievement/";
			string text3 = text;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Achievement ");
			defaultInterpolatedStringHandler.AppendFormatted<Achievement>(achievement);
			TaskHelper.RunSafely(MetricUtilities.PutRequest(text2, text3, defaultInterpolatedStringHandler.ToStringAndClear()));
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0005A75C File Offset: 0x0005895C
		public static void UploadEpochMetric(string epochId)
		{
			if (!MetricUtilities.ShouldUploadMetrics())
			{
				return;
			}
			EpochMetric epochMetric = new EpochMetric();
			ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
			epochMetric.BuildId = ((releaseInfo != null) ? releaseInfo.Version : null) ?? "NON-RELEASE-VERSION";
			epochMetric.Epoch = epochId.Replace("_EPOCH", "");
			epochMetric.TotalRuns = SaveManager.Instance.Progress.NumberOfRuns;
			epochMetric.TotalPlaytime = SaveManager.Instance.Progress.TotalPlaytime;
			epochMetric.TotalEpochs = SaveManager.Instance.Progress.Epochs.Count;
			EpochMetric epochMetric2 = epochMetric;
			string text = JsonSerializer.Serialize<EpochMetric>(epochMetric2, MetricsSerializerContext.Default.EpochMetric);
			Log.Info("Uploading epoch metric for epoch " + epochId, 2);
			TaskHelper.RunSafely(MetricUtilities.PutRequest("https://sts2-metric-upload.megacrit.com/record_epoch/", text, "Epoch " + epochId));
		}

		/// <summary>
		/// Uploads a player's Settings preferences and Graphics info.
		/// This is only uploaded a single time, at the end of their 5th run. This allows
		/// players to have a chance to play and figure out which settings they want to use.
		/// This info is used to figure out which options and display settings are frequently used.
		/// </summary>
		// Token: 0x06000DB2 RID: 3506 RVA: 0x0005A834 File Offset: 0x00058A34
		public static void UploadSettingsMetric()
		{
			if (!MetricUtilities.ShouldUploadMetrics())
			{
				return;
			}
			long num = (long)OS.GetMemoryInfo()["physical"];
			SettingsDataMetric settingsDataMetric = default(SettingsDataMetric);
			ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
			settingsDataMetric.BuildId = ((releaseInfo != null) ? releaseInfo.Version : null) ?? "NON-RELEASE-VERSION";
			settingsDataMetric.Os = OS.GetName();
			settingsDataMetric.Platform = "Steam";
			settingsDataMetric.SystemRam = ((num == -1L) ? (-1) : ((int)Math.Round((double)num / 1073741824.0)));
			settingsDataMetric.LanguageCode = SaveManager.Instance.SettingsSave.Language;
			settingsDataMetric.FastModeType = SaveManager.Instance.PrefsSave.FastMode;
			settingsDataMetric.Screenshake = SaveManager.Instance.PrefsSave.ScreenShakeOptionIndex;
			settingsDataMetric.ShowRunTimer = SaveManager.Instance.PrefsSave.ShowRunTimer;
			settingsDataMetric.PhobiaMode = SaveManager.Instance.PrefsSave.PhobiaMode;
			settingsDataMetric.ShowCardIndices = SaveManager.Instance.PrefsSave.ShowCardIndices;
			settingsDataMetric.DisplayCount = DisplayServer.GetScreenCount();
			settingsDataMetric.DisplayResolution = SaveManager.Instance.SettingsSave.WindowSize;
			settingsDataMetric.Fullscreen = SaveManager.Instance.SettingsSave.Fullscreen;
			settingsDataMetric.AspectRatio = SaveManager.Instance.SettingsSave.AspectRatioSetting;
			settingsDataMetric.ResizeWindows = SaveManager.Instance.SettingsSave.ResizeWindows;
			settingsDataMetric.VSync = SaveManager.Instance.SettingsSave.VSync;
			settingsDataMetric.FpsLimit = SaveManager.Instance.SettingsSave.FpsLimit;
			settingsDataMetric.Msaa = SaveManager.Instance.SettingsSave.Msaa;
			SettingsDataMetric settingsDataMetric2 = settingsDataMetric;
			string text = JsonSerializer.Serialize<SettingsDataMetric>(settingsDataMetric2, MetricsSerializerContext.Default.SettingsDataMetric);
			Log.Info("Completed 5 runs! Uploading settings metrics", 2);
			TaskHelper.RunSafely(MetricUtilities.PutRequest("https://sts2-metric-upload.megacrit.com/record_settings/", text, "Settings data metrics"));
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0005AA28 File Offset: 0x00058C28
		private static async Task PutRequest(string url, string json, string context)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(json);
			ByteArrayContent byteArrayContent = new ByteArrayContent(bytes);
			byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
			HttpResponseMessage response;
			try
			{
				HttpResponseMessage httpResponseMessage = await MetricUtilities._httpClient.PutAsync(url, byteArrayContent);
				response = httpResponseMessage;
			}
			catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
			{
				Log.Warn("Metrics upload for '" + context + "' failed due to network error: " + ex.Message, 2);
				return;
			}
			if (response.IsSuccessStatusCode)
			{
				Log.Info("Metric for '" + context + "' successfully uploaded!", 2);
			}
			else
			{
				string text = await response.Content.ReadAsStringAsync();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Metrics upload request for '");
				defaultInterpolatedStringHandler.AppendFormatted(context);
				defaultInterpolatedStringHandler.AppendLiteral("' failed with status code: ");
				defaultInterpolatedStringHandler.AppendFormatted<HttpStatusCode>(response.StatusCode);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				Log.Info("Response body: " + text, 2);
			}
		}

		// Token: 0x040004EE RID: 1262
		private const string _metricsRunDataEndpoint = "https://sts2-metric-upload.megacrit.com/record_data/";

		// Token: 0x040004EF RID: 1263
		private const string _metricsAchievementDataEndpoint = "https://sts2-metric-upload.megacrit.com/record_achievement/";

		// Token: 0x040004F0 RID: 1264
		private const string _metricsEpochDataEndpoint = "https://sts2-metric-upload.megacrit.com/record_epoch/";

		// Token: 0x040004F1 RID: 1265
		private const string _metricsSettingsDataEndpoint = "https://sts2-metric-upload.megacrit.com/record_settings/";

		// Token: 0x040004F2 RID: 1266
		private const int _runLengthThreshold = 5;

		// Token: 0x040004F3 RID: 1267
		private static readonly HttpClient _httpClient = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(15L)
		};
	}
}
