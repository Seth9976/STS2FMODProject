using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen
{
	// Token: 0x0200031B RID: 795
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NGameOverScreen.cs")]
	public class NGameOverScreen : NClickableControl, IOverlayScreen, IScreenContext
	{
		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002C39 RID: 11321 RVA: 0x00139E48 File Offset: 0x00138048
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/game_over_screen");
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002C3A RID: 11322 RVA: 0x00139E54 File Offset: 0x00138054
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NGameOverScreen.ScenePath);
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002C3B RID: 11323 RVA: 0x00139E60 File Offset: 0x00138060
		public NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.GameOver;
			}
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x00139E64 File Offset: 0x00138064
		public override void _Ready()
		{
			AbstractRoom currentRoom = this._runState.CurrentRoom;
			bool flag = currentRoom != null && currentRoom.IsVictoryRoom;
			RunHistory runHistory;
			if ((runHistory = RunManager.Instance.History) == null)
			{
				(runHistory = new RunHistory()).Win = flag;
			}
			this._history = runHistory;
			this._score = ScoreUtility.CalculateScore(this._serializableRun, this._history.Win);
			this._uiNode = base.GetNode<Control>("%Ui");
			this._continueButton = base.GetNode<NGameOverContinueButton>("%ContinueButton");
			this._continueButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenSummaryScreen)), 0U);
			this._continueButton.Disable();
			this._viewRunButton = base.GetNode<NViewRunButton>("%ViewRunButton");
			this._viewRunButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenRunHistoryScreen)), 0U);
			this._mainMenuButton = base.GetNode<NReturnToMainMenuButton>("%MainMenuButton");
			this._mainMenuButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnMainMenuButtonPressed)), 0U);
			this._scoreLineContainer = base.GetNode<GridContainer>("%ScoreLineContainer");
			this._badgeContainer = base.GetNode<Control>("%BadgeContainer");
			this._scoreBar = base.GetNode<Control>("%ScoreBar");
			this._scoreFg = base.GetNode<Control>("%ScoreFg");
			this._scoreProgress = base.GetNode<MegaLabel>("%ScoreProgress");
			this._unlocksRemaining = base.GetNode<MegaLabel>("%UnlocksRemaining");
			this._screenshakeContainer = base.GetNode<Control>("%ScreenshakeContainer");
			this._leaderboardButton = base.GetNode<NGameOverContinueButton>("%LeaderboardButton");
			this._leaderboardButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ShowLeaderboard)), 0U);
			this._creatureContainer = base.GetNode<Control>("%CreatureContainer");
			this._summaryContainer = base.GetNode<NRunSummary>("%RunSummaryContainer");
			this._backstop = base.GetNode<ColorRect>("%Backstop");
			this._fullBlackBackstop = base.GetNode<ColorRect>("%FullBlackBackstop");
			this._backstopMaterial = (ShaderMaterial)this._backstop.Material;
			this._summaryBackstop = base.GetNode<ColorRect>("%SummaryBackstop");
			this._leaderboard = base.GetNode<NDailyRunLeaderboard>("%DailyRunLeaderboard");
			this._banner = base.GetNode<NCommonBanner>("%Banner");
			this._victoryDamageLabel = base.GetNode<MegaRichTextLabel>("%VictoryDamageLabel");
			this._discoveryLabel = base.GetNode<MegaLabel>("%DiscoveryLabel");
			this._discoveryLabel.SetTextAutoSize(new LocString("game_over_screen", "DISCOVERY_HEADER").GetFormattedText());
			this._deathQuote = base.GetNode<MegaRichTextLabel>("%DeathQuoteLabel");
			this.InitializeBannerAndQuote();
			this._leaderboardButton.Disable();
			this._viewRunButton.Disable();
			this._mainMenuButton.Disable();
			this._leaderboard.Visible = false;
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x0013A1A8 File Offset: 0x001383A8
		public override void _ExitTree()
		{
			this._cts.Cancel();
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x0013A1B5 File Offset: 0x001383B5
		private bool DiscoveredAnyEpochs()
		{
			return this._localPlayer.DiscoveredEpochs.Count > 0;
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x0013A1CC File Offset: 0x001383CC
		private void InitializeBannerAndQuote()
		{
			ModelId id = this._localPlayer.Character.Id;
			if (this._history.Win)
			{
				this._banner.label.SetTextAutoSize(new LocString("game_over_screen", "BANNER.falseWin").GetRawText());
				this._deathQuote.Text = string.Empty;
				long personalArchitectDamage = StatsManager.GetPersonalArchitectDamage();
				long? globalArchitectDamage = StatsManager.GetGlobalArchitectDamage();
				StringBuilder stringBuilder = new StringBuilder();
				LocString locString;
				if (globalArchitectDamage != null)
				{
					locString = new LocString("game_over_screen", "VICTORY_DAMAGE");
					locString.Add("TotalDamage", globalArchitectDamage.Value);
				}
				else
				{
					locString = new LocString("game_over_screen", "VICTORY_DAMAGE_LOCAL");
				}
				locString.Add("PlayerDamage", this._score);
				locString.Add("PersonalDamage", personalArchitectDamage);
				stringBuilder.Append(locString.GetFormattedText());
				int ascensionLevel = this._runState.AscensionLevel;
				if (ascensionLevel < 10 && ascensionLevel > 0 && this._runState.AscensionLevel >= this._localPlayer.MaxAscensionWhenRunStarted && this._runState.GameMode == GameMode.Standard)
				{
					stringBuilder.Append("\n\n");
					LocString locString2 = new LocString("game_over_screen", "VICTORY_UNLOCKED_ASCENSION");
					locString2.Add("AscensionLevel", this._runState.AscensionLevel + 1);
					stringBuilder.Append(locString2.GetFormattedText());
				}
				this._victoryDamageLabel.Text = stringBuilder.ToString();
			}
			else
			{
				LocTable table = LocManager.Instance.GetTable("game_over_screen");
				IReadOnlyList<LocString> locStringsWithPrefix = table.GetLocStringsWithPrefix("BANNER.lose");
				this._banner.label.SetTextAutoSize(Rng.Chaotic.NextItem<LocString>(locStringsWithPrefix).GetRawText());
				IReadOnlyList<LocString> locStringsWithPrefix2 = table.GetLocStringsWithPrefix("QUOTES");
				this._deathQuote.Text = Rng.Chaotic.NextItem<LocString>(locStringsWithPrefix2).GetFormattedText();
			}
			this._encounterQuote = NRunHistory.GetDeathQuote(this._history, id, NRunHistory.GetGameOverType(this._history));
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x0013A3E0 File Offset: 0x001385E0
		private async Task AnimateInQuote()
		{
			if (this._deathQuote.Modulate.A != 0f)
			{
				Tween quoteTween = this._quoteTween;
				if (quoteTween != null)
				{
					quoteTween.Kill();
				}
				this._quoteTween = base.CreateTween();
				this._quoteTween.TweenProperty(this._deathQuote, "modulate:a", 0f, 0.25);
				TaskAwaiter<bool> taskAwaiter = this._quoteTween.AwaitFinished(this).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					return;
				}
				this._deathQuote.Text = this._encounterQuote;
				this._quoteTween.Kill();
				await Cmd.Wait(1f, this._cts.Token, false);
			}
			if (this.IsValid())
			{
				Tween quoteTween2 = this._quoteTween;
				if (quoteTween2 != null)
				{
					quoteTween2.Kill();
				}
				this._quoteTween = base.CreateTween().SetParallel(true);
				if (this._history.Win)
				{
					this._quoteTween.TweenProperty(this._victoryDamageLabel, "visible_ratio", 1f, 2.0).SetEase(1L).SetTrans(1L);
					this._quoteTween.TweenProperty(this._victoryDamageLabel, "modulate:a", 1f, 2.0);
					bool flag = await this._quoteTween.AwaitFinished(this);
				}
				else
				{
					this._quoteTween.TweenProperty(this._deathQuote, "position:y", 156f, 2.0).SetEase(1L).SetTrans(5L)
						.From(90f);
					this._quoteTween.TweenProperty(this._deathQuote, "modulate:a", 1f, 1.5);
				}
			}
		}

		/// <summary>
		/// Create an instance of this screen.
		/// Null if we're in test mode.
		/// </summary>
		// Token: 0x06002C41 RID: 11329 RVA: 0x0013A424 File Offset: 0x00138624
		[return: Nullable(2)]
		public static NGameOverScreen Create(RunState runState, SerializableRun serializableRun)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NGameOverScreen ngameOverScreen = PreloadManager.Cache.GetScene(NGameOverScreen.ScenePath).Instantiate<NGameOverScreen>(0L);
			ngameOverScreen._runState = runState;
			ngameOverScreen._serializableRun = serializableRun;
			ngameOverScreen._localPlayer = LocalContext.GetMe(runState);
			return ngameOverScreen;
		}

		/// <summary>
		/// Called when the player continues to the score/badge breakdown.
		/// </summary>
		/// <param name="_"></param>
		// Token: 0x06002C42 RID: 11330 RVA: 0x0013A46C File Offset: 0x0013866C
		private void OpenSummaryScreen(NButton _)
		{
			this._isAnimatingSummary = true;
			this._continueButton.Disable();
			this._victoryDamageLabel.Visible = false;
			Tween tween = base.CreateTween();
			tween.TweenProperty(this._summaryBackstop, "modulate", Colors.White, 0.5);
			TaskHelper.RunSafely(this.AnimateInQuote());
			TaskHelper.RunSafely(this.AnimateRunSummary());
		}

		/// <summary>
		/// Animates the second page of our Game Over screen
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException"></exception>
		// Token: 0x06002C43 RID: 11331 RVA: 0x0013A4E0 File Offset: 0x001386E0
		private async Task AnimateRunSummary()
		{
			Tween tween = base.CreateTween();
			tween.TweenProperty(this._banner, "position:y", this._banner.Position.Y - 32f, 0.5).SetEase(1L).SetTrans(7L);
			this._summaryContainer.Visible = true;
			await this.AnimateScoreLines();
			await this.AnimateBadges();
			await this.AnimateScoreBar();
			await this.AnimateDiscoveries();
			for (int i = 0; i < this._badgeContainer.GetChildCount(false); i++)
			{
				this._badgeContainer.GetChild<NBadge>(i, false).FocusNeighborLeft = ((i > 0) ? this._badgeContainer.GetChild<NBadge>(i - 1, false).GetPath() : this._badgeContainer.GetChild<NBadge>(this._badgeContainer.GetChildCount(false) - 1, false).GetPath());
				this._badgeContainer.GetChild<NBadge>(i, false).FocusNeighborRight = ((i < this._badgeContainer.GetChildCount(false) - 1) ? this._badgeContainer.GetChild<NBadge>(i + 1, false).GetPath() : this._badgeContainer.GetChild<NBadge>(0, false).GetPath());
				this._badgeContainer.GetChild<NBadge>(i, false).FocusNeighborTop = this._badgeContainer.GetChild<NBadge>(i, false).GetPath();
				this._badgeContainer.GetChild<NBadge>(i, false).FocusNeighborBottom = ((this._summaryContainer.DefaultFocusedControl != null) ? this._summaryContainer.DefaultFocusedControl.GetPath() : this._badgeContainer.GetChild<NBadge>(i, false).GetPath());
			}
			this._summaryContainer.SetControllerNav((this._badgeContainer.GetChildCount(false) > 0) ? this._badgeContainer.GetChild<Control>(0, false) : null);
			if (this._badgeContainer.GetChildCount(false) > 0)
			{
				base.FocusNeighborBottom = this._badgeContainer.GetChild<NBadge>(0, false).GetPath();
				base.FocusNeighborTop = this._badgeContainer.GetChild<NBadge>(0, false).GetPath();
				base.FocusNeighborLeft = this._badgeContainer.GetChild<NBadge>(0, false).GetPath();
				base.FocusNeighborRight = this._badgeContainer.GetChild<NBadge>(0, false).GetPath();
			}
			else if (this._summaryContainer.DefaultFocusedControl != null)
			{
				base.FocusNeighborBottom = this._summaryContainer.DefaultFocusedControl.GetPath();
				base.FocusNeighborTop = this._summaryContainer.DefaultFocusedControl.GetPath();
				base.FocusNeighborLeft = this._summaryContainer.DefaultFocusedControl.GetPath();
				base.FocusNeighborRight = this._summaryContainer.DefaultFocusedControl.GetPath();
			}
			if (this._history.GameMode == GameMode.Daily)
			{
				this._leaderboard.Initialize(RunManager.Instance.DailyTime.Value, this._runState.Players.Select<Player, ulong>((Player p) => p.NetId), false);
				this._leaderboardButton.Visible = true;
				this._leaderboardButton.Enable();
			}
			else
			{
				if (this.DiscoveredAnyEpochs())
				{
					this._mainMenuButton.SetLabelForUnlock();
				}
				this._mainMenuButton.Visible = true;
				this._mainMenuButton.Enable();
			}
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x0013A524 File Offset: 0x00138724
		private async Task AnimateScoreLines()
		{
			this._scoreLines.Clear();
			string text = "SCORE_LINE.floorsClimbed";
			string text2 = "FloorCount";
			int totalFloor = this._runState.TotalFloor;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler.AppendLiteral("+");
			defaultInterpolatedStringHandler.AppendFormatted<int>(ScoreUtility.GetScoreForFloor(this._serializableRun.MapPointHistory));
			this.AddScoreLine(text, text2, totalFloor, defaultInterpolatedStringHandler.ToStringAndClear(), "res://images/ui/game_over_screen/score_floor.png");
			int num = this._serializableRun.MapPointHistory.SelectMany<List<MapPointHistoryEntry>, MapPointHistoryEntry>((List<MapPointHistoryEntry> actEntries) => actEntries).Sum<MapPointHistoryEntry>((MapPointHistoryEntry e) => e.GetEntry(this._localPlayer.NetId).GoldGained);
			string text3 = "SCORE_LINE.goldGained";
			string text4 = "GoldAmount";
			int num2 = num;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler.AppendLiteral("+");
			defaultInterpolatedStringHandler.AppendFormatted<int>(ScoreUtility.GetScoreForGoldGained(this._serializableRun.MapPointHistory, this._serializableRun.Players.Count));
			this.AddScoreLine(text3, text4, num2, defaultInterpolatedStringHandler.ToStringAndClear(), "res://images/ui/game_over_screen/score_gold.png");
			int elitesKilledCount = ScoreUtility.GetElitesKilledCount(this._serializableRun.MapPointHistory);
			if (elitesKilledCount > 0)
			{
				string text5 = "SCORE_LINE.elitesKilled";
				string text6 = "EliteCount";
				int num3 = elitesKilledCount;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
				defaultInterpolatedStringHandler.AppendLiteral("+");
				defaultInterpolatedStringHandler.AppendFormatted<int>(ScoreUtility.GetScoreForElitesKilled(elitesKilledCount));
				this.AddScoreLine(text5, text6, num3, defaultInterpolatedStringHandler.ToStringAndClear(), "res://images/ui/game_over_screen/score_elite.png");
			}
			int bossesSlainCount = ScoreUtility.GetBossesSlainCount(this._serializableRun.MapPointHistory, this._history.Win);
			if (bossesSlainCount > 0)
			{
				string text7 = "SCORE_LINE.bossesSlain";
				string text8 = "BossCount";
				int num4 = bossesSlainCount;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
				defaultInterpolatedStringHandler.AppendLiteral("+");
				defaultInterpolatedStringHandler.AppendFormatted<int>(ScoreUtility.GetScoreForBossesSlain(bossesSlainCount));
				this.AddScoreLine(text7, text8, num4, defaultInterpolatedStringHandler.ToStringAndClear(), "res://images/ui/game_over_screen/score_boss.png");
			}
			int ascension = this._history.Ascension;
			if (ascension > 0)
			{
				this.AddScoreLine("SCORE_LINE.ascension", "AscensionLevel", ascension, "x" + this.GetAscensionMulti(ascension), "res://images/ui/game_over_screen/score_ascension.png");
			}
			foreach (NScoreLine nscoreLine in this._scoreLines)
			{
				await nscoreLine.AnimateIn();
			}
			List<NScoreLine>.Enumerator enumerator = default(List<NScoreLine>.Enumerator);
			await Cmd.Wait(0.5f, this._cts.Token, false);
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x0013A568 File Offset: 0x00138768
		private async Task AnimateBadges()
		{
			List<Badge> badges = ScoreUtility.GetBadges(this._serializableRun, this._localPlayer.NetId, this._history.Win);
			foreach (Badge badge in badges)
			{
				this._badgeContainer.AddChildSafely(NBadge.Create(badge));
			}
			if (!this._serializableRun.GameMode.AreAchievementsAndEpochsLocked())
			{
				this.SaveBadgesToProgress(badges);
			}
			await Cmd.Wait(0.25f, this._cts.Token, false);
			foreach (NBadge nbadge in this._badgeContainer.GetChildren(false).OfType<NBadge>())
			{
				await nbadge.AnimateIn();
			}
			IEnumerator<NBadge> enumerator2 = null;
			await Cmd.Wait(0.5f, this._cts.Token, false);
		}

		/// <summary>
		/// Based on the badges you get, save them to the progress file.
		/// </summary>
		// Token: 0x06002C46 RID: 11334 RVA: 0x0013A5AC File Offset: 0x001387AC
		private void SaveBadgesToProgress(List<Badge> badgesToSave)
		{
			CharacterStats characterStats = SaveManager.Instance.Progress.CharacterStats[this._localPlayer.Character.Id];
			using (List<Badge>.Enumerator enumerator = badgesToSave.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Badge badge = enumerator.Current;
					BadgeStats badgeStats = characterStats.Badges.FirstOrDefault<BadgeStats>((BadgeStats b) => b.Id.Equals(badge.Id) && b.Rarity == badge.Rarity);
					if (badgeStats == null)
					{
						BadgeStats badgeStats2 = new BadgeStats
						{
							Id = badge.Id,
							Count = 1,
							Rarity = badge.Rarity
						};
						characterStats.Badges.Add(badgeStats2);
						Log.Info("You got a new badge: " + badge.Id, 2);
					}
					else
					{
						BadgeStats badgeStats3 = badgeStats;
						int count = badgeStats3.Count;
						badgeStats3.Count = count + 1;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
						defaultInterpolatedStringHandler.AppendLiteral("You got badge: ");
						defaultInterpolatedStringHandler.AppendFormatted(badge.Id);
						defaultInterpolatedStringHandler.AppendLiteral(" again. Now you have ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(badgeStats.Count);
						defaultInterpolatedStringHandler.AppendLiteral("!");
						Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
				}
			}
		}

		/// <summary>
		/// Helper function to create score lines, add child, and place it in a list to bulk animate later.
		/// </summary>
		// Token: 0x06002C47 RID: 11335 RVA: 0x0013A718 File Offset: 0x00138918
		private void AddScoreLine(string locEntryKey, [Nullable(2)] string locAmountKey = null, int amount = 0, string scoreLabel = "ERROR", [Nullable(2)] string iconPath = null)
		{
			LocString locString = new LocString("game_over_screen", locEntryKey);
			if (locAmountKey != null)
			{
				locString.Add(locAmountKey, amount);
			}
			Texture2D texture2D = ((iconPath == null) ? null : PreloadManager.Cache.GetTexture2D(iconPath));
			NScoreLine nscoreLine = NScoreLine.Create(locString.GetFormattedText(), scoreLabel, texture2D);
			this._scoreLineContainer.AddChildSafely(nscoreLine);
			this._scoreLines.Add(nscoreLine);
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x0013A77C File Offset: 0x0013897C
		private async Task AnimateScoreBar()
		{
			int unlocksRemaining = SaveManager.Instance.GetUnlocksRemaining();
			LocString locString = new LocString("game_over_screen", "SCORE.unlocksRemaining");
			locString.Add("UnlockCount", unlocksRemaining);
			this._unlocksRemaining.SetTextAutoSize(locString.GetFormattedText());
			if (unlocksRemaining > 0)
			{
				int currentScore = SaveManager.Instance.GetCurrentScore();
				this._scoreThreshold = this.GetScoreThreshold(unlocksRemaining);
				MegaLabel scoreProgress = this._scoreProgress;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(currentScore);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._scoreThreshold);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				scoreProgress.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
				this._scoreFg.Scale = new Vector2((float)currentScore / (float)this._scoreThreshold, 1f);
				Tween scoreTween = base.CreateTween();
				scoreTween.TweenProperty(this._scoreBar, "modulate:a", 1f, 0.3);
				TaskAwaiter<bool> taskAwaiter = scoreTween.AwaitFinished(this).GetAwaiter();
				TaskAwaiter<bool> taskAwaiter2;
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					if (currentScore + this._score >= this._scoreThreshold)
					{
						Log.Info("New Unlock, yay!", 2);
						MegaLabel node = base.GetNode<MegaLabel>("%UnlockText");
						this._scoreUnlockedEpochId = SaveManager.Instance.IncrementUnlock();
						currentScore -= this._scoreThreshold;
						int newThreshold = this.GetScoreThreshold(unlocksRemaining - 1);
						node.SetTextAutoSize(new LocString("game_over_screen", (newThreshold == 0) ? "SCORE.unlockedAllMessage" : "SCORE.unlockedEpochMessage").GetFormattedText());
						scoreTween = base.CreateTween().SetParallel(true);
						scoreTween.TweenInterval(1.0);
						scoreTween.Chain();
						scoreTween.TweenMethod(Callable.From<int>(new Action<int>(this.TweenScore)), currentScore + this._scoreThreshold, this._scoreThreshold, 1.0).SetEase(1L).SetTrans(7L);
						scoreTween.TweenProperty(this._scoreFg, "scale:x", 1f, 1.0).SetEase(1L).SetTrans(7L);
						scoreTween.Chain();
						scoreTween.TweenCallback(Callable.From(new Action(this.PlayUnlockSfx)));
						scoreTween.TweenProperty(node, "modulate:a", 1f, 0.25);
						scoreTween.TweenProperty(node, "position:y", -60f, 0.25).SetEase(1L).SetTrans(11L);
						TaskAwaiter<bool> taskAwaiter3 = scoreTween.AwaitFinished(this).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							await taskAwaiter3;
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter3.GetResult())
						{
							return;
						}
						if (this._scoreUnlockedEpochId != null && !SaveManager.Instance.IsEpochRevealed(this._scoreUnlockedEpochId))
						{
							EpochModel epochModel = EpochModel.Get(this._scoreUnlockedEpochId);
							SaveManager.Instance.ObtainEpoch(this._scoreUnlockedEpochId);
							NGame.Instance.AddChildSafely(NGainEpochVfx.Create(epochModel));
							this._localPlayer.DiscoveredEpochs.Add(epochModel.Id);
							LocalContext.GetMe(this._serializableRun).DiscoveredEpochs.Add(epochModel.Id);
						}
						LocString locString2 = new LocString("game_over_screen", "SCORE.unlocksRemaining");
						locString2.Add("UnlockCount", unlocksRemaining - 1);
						this._unlocksRemaining.SetTextAutoSize(locString2.GetFormattedText());
						this._scoreThreshold = newThreshold;
						currentScore += this._score;
						if (newThreshold == 0 || currentScore == 0)
						{
							Log.Info("Player has gotten all unlocks or they've overflowed exactly 0", 2);
							SaveManager.Instance.Progress.CurrentScore = 0;
						}
						else if (currentScore >= newThreshold)
						{
							Log.Info("Score is too awesome. Disallow double unlock.", 2);
							scoreTween.Kill();
							scoreTween = base.CreateTween().SetParallel(true);
							scoreTween.TweenInterval(0.5);
							scoreTween.Chain();
							scoreTween.TweenMethod(Callable.From<int>(new Action<int>(this.TweenScore)), 0, newThreshold * 99 / 100, 1.0).SetEase(1L).SetTrans(7L);
							scoreTween.TweenProperty(this._scoreFg, "scale:x", 1f, 1.0).SetEase(1L).SetTrans(7L)
								.From(0f);
							TaskAwaiter<bool> taskAwaiter4 = scoreTween.AwaitFinished(this).GetAwaiter();
							if (!taskAwaiter4.IsCompleted)
							{
								await taskAwaiter4;
								taskAwaiter4 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (!taskAwaiter4.GetResult())
							{
								return;
							}
							SaveManager.Instance.Progress.CurrentScore = newThreshold - 1;
						}
						else
						{
							Log.Info("Animate overflow score.", 2);
							scoreTween.Kill();
							scoreTween = base.CreateTween().SetParallel(true);
							scoreTween.Chain();
							scoreTween.TweenInterval(0.5);
							scoreTween.Chain();
							scoreTween.TweenMethod(Callable.From<int>(new Action<int>(this.TweenScore)), 0, currentScore, 1.0).SetEase(1L).SetTrans(7L);
							scoreTween.TweenProperty(this._scoreFg, "scale:x", (float)currentScore / (float)newThreshold, 1.0).SetEase(1L).SetTrans(7L)
								.From(0f);
							TaskAwaiter<bool> taskAwaiter5 = scoreTween.AwaitFinished(this).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								await taskAwaiter5;
								taskAwaiter5 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (!taskAwaiter5.GetResult())
							{
								return;
							}
							SaveManager.Instance.Progress.CurrentScore = currentScore;
						}
					}
					else
					{
						Log.Info("Not enough score to level up", 2);
						scoreTween = base.CreateTween().SetParallel(true);
						scoreTween.TweenInterval(0.5);
						scoreTween.TweenMethod(Callable.From<int>(new Action<int>(this.TweenScore)), currentScore, currentScore + this._score, 1.0);
						scoreTween.TweenProperty(this._scoreFg, "scale:x", (float)(currentScore + this._score) / (float)this._scoreThreshold, 1.0);
						SaveManager.Instance.Progress.CurrentScore += this._score;
					}
					SaveManager.Instance.SaveProgressFile();
					scoreTween = null;
				}
			}
			else
			{
				Log.Info("This player has all unlocks. No action", 2);
			}
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x0013A7BF File Offset: 0x001389BF
		private void PlayUnlockSfx()
		{
			Log.Info("TODO: Play the ding unlock sfx here pls", 2);
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x0013A7CC File Offset: 0x001389CC
		private void TweenScore(int value)
		{
			MegaLabel scoreProgress = this._scoreProgress;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
			defaultInterpolatedStringHandler.AppendLiteral("[");
			defaultInterpolatedStringHandler.AppendFormatted<int>(value);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._scoreThreshold);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			scoreProgress.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		/// <summary>
		/// Lookup table of score thresholds based on the total number of unlocks the player has completed.
		/// Must match: <see cref="M:MegaCrit.Sts2.Core.Saves.SaveManager.GetEpochIdForUnlock" />.
		/// </summary>
		/// <param name="unlocksRemaining"></param>
		/// <returns></returns>
		// Token: 0x06002C4B RID: 11339 RVA: 0x0013A830 File Offset: 0x00138A30
		private int GetScoreThreshold(int unlocksRemaining)
		{
			int num;
			switch (18 - unlocksRemaining)
			{
			case 0:
				num = 200;
				break;
			case 1:
				num = 500;
				break;
			case 2:
				num = 750;
				break;
			case 3:
				num = 1000;
				break;
			case 4:
				num = 1250;
				break;
			case 5:
				num = 1500;
				break;
			case 6:
				num = 1600;
				break;
			case 7:
				num = 1700;
				break;
			case 8:
				num = 1800;
				break;
			case 9:
				num = 1900;
				break;
			case 10:
				num = 2000;
				break;
			case 11:
				num = 2100;
				break;
			case 12:
				num = 2200;
				break;
			case 13:
				num = 2300;
				break;
			case 14:
				num = 2400;
				break;
			case 15:
				num = 2500;
				break;
			case 16:
				num = 2500;
				break;
			case 17:
				num = 2500;
				break;
			default:
				num = 0;
				break;
			}
			return num;
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x0013A930 File Offset: 0x00138B30
		private void ShowLeaderboard(NButton _)
		{
			this._banner.ChangeText(new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.title").GetRawText());
			Tween tween = base.CreateTween().SetParallel(true);
			CanvasItem leaderboard = this._leaderboard;
			Color modulate = this._leaderboard.Modulate;
			modulate.A = 0f;
			leaderboard.Modulate = modulate;
			tween.TweenProperty(this._leaderboard, "modulate:a", 1f, 0.5);
			tween.TweenProperty(this._summaryContainer, "modulate:a", 0f, 0.5);
			tween.TweenProperty(this._deathQuote, "modulate:a", 0f, 0.5);
			tween.Chain().TweenCallback(Callable.From(new Action(this.HideSummary)));
			this._leaderboard.Visible = true;
			this._leaderboardButton.Disable();
			if (this.DiscoveredAnyEpochs())
			{
				this._mainMenuButton.SetLabelForUnlock();
			}
			this._mainMenuButton.Visible = true;
			this._mainMenuButton.Enable();
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x0013AA69 File Offset: 0x00138C69
		private void HideSummary()
		{
			this._summaryContainer.Visible = false;
			this._deathQuote.Visible = false;
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x0013AA84 File Offset: 0x00138C84
		private async Task AnimateDiscoveries()
		{
			await this._summaryContainer.AnimateInDiscoveries(this._runState, this._cts.Token);
			this._isAnimatingSummary = false;
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x0013AAC8 File Offset: 0x00138CC8
		private void OpenRunHistoryScreen(NButton _)
		{
			Control control = ResourceLoader.Load<PackedScene>("res://scenes/screens/run_history_screen/run_history_screen_via_game_over_screen.tscn", null, 1L).Instantiate<Control>(0L);
			this.AddChildSafely(control);
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x0013AAF4 File Offset: 0x00138CF4
		private void OnMainMenuButtonPressed(NButton _)
		{
			if (RunManager.Instance.NetService.Type == NetGameType.Host)
			{
				RunManager.Instance.NetService.Disconnect(NetError.QuitGameOver, false);
			}
			this._mainMenuButton.Disable();
			if (this.DiscoveredAnyEpochs())
			{
				this.OpenTimeline();
				return;
			}
			this.ReturnToMainMenu();
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x0013AB44 File Offset: 0x00138D44
		private void OpenTimeline()
		{
			TaskHelper.RunSafely(this.TransitionOutToTimeline());
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x0013AB52 File Offset: 0x00138D52
		private void ReturnToMainMenu()
		{
			TaskHelper.RunSafely(this.TransitionOutToMainMenu());
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x0013AB60 File Offset: 0x00138D60
		private async Task TransitionOutToTimeline()
		{
			await NGame.Instance.GoToTimelineAfterRun();
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x0013AB9C File Offset: 0x00138D9C
		private async Task TransitionOutToMainMenu()
		{
			await NGame.Instance.ReturnToMainMenuAfterRun();
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x0013ABD7 File Offset: 0x00138DD7
		public void AfterOverlayOpened()
		{
			this.MoveCreaturesToDifferentLayerAndDisableUi();
			TaskHelper.RunSafely(this.AnimateIn());
		}

		/// <summary>
		/// We move the Creature nodes so they render above the game over screen backstop!
		/// Drama!
		/// </summary>
		// Token: 0x06002C56 RID: 11350 RVA: 0x0013ABEC File Offset: 0x00138DEC
		private void MoveCreaturesToDifferentLayerAndDisableUi()
		{
			List<NCreatureVisuals> list = new List<NCreatureVisuals>();
			List<NCreature> list2;
			if (NCombatRoom.Instance != null)
			{
				if (NCombatRoom.Instance.Mode == CombatRoomMode.ActiveCombat)
				{
					NCombatRoom.Instance.Ui.AnimOut();
				}
				list2 = NCombatRoom.Instance.CreatureNodes.ToList<NCreature>();
				list = list2.Select<NCreature, NCreatureVisuals>((NCreature c) => c.Visuals).ToList<NCreatureVisuals>();
			}
			else
			{
				if (NMerchantRoom.Instance != null)
				{
					list2 = new List<NCreature>();
					using (IEnumerator<NMerchantCharacter> enumerator = NMerchantRoom.Instance.PlayerVisuals.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							NMerchantCharacter nmerchantCharacter = enumerator.Current;
							nmerchantCharacter.PlayAnimation("die", false);
							nmerchantCharacter.Reparent(this._creatureContainer, true);
						}
						goto IL_02E6;
					}
				}
				if (NRestSiteRoom.Instance != null)
				{
					list2 = new List<NCreature>();
					list = new List<NCreatureVisuals>();
					using (IEnumerator<Player> enumerator2 = this._runState.Players.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Player player = enumerator2.Current;
							NCreatureVisuals ncreatureVisuals = player.Creature.CreateVisuals();
							list.Add(ncreatureVisuals);
							this._creatureContainer.AddChildSafely(ncreatureVisuals);
							ncreatureVisuals.SpineAnimation.SetAnimation("die", false, 0);
							NRestSiteCharacter characterForPlayer = NRestSiteRoom.Instance.GetCharacterForPlayer(player);
							ncreatureVisuals.GlobalPosition = characterForPlayer.GlobalPosition;
							ncreatureVisuals.Scale = characterForPlayer.Scale;
							characterForPlayer.Visible = false;
							Vector2 vector;
							vector..ctor(100f, 100f);
							ncreatureVisuals.Position += vector * new Vector2((float)Math.Sign(ncreatureVisuals.Scale.X), (float)Math.Sign(ncreatureVisuals.Scale.Y));
						}
						goto IL_02E6;
					}
				}
				list2 = new List<NCreature>();
				list = new List<NCreatureVisuals>();
				foreach (Player player2 in this._runState.Players)
				{
					NCreatureVisuals ncreatureVisuals2 = player2.Creature.CreateVisuals();
					list.Add(ncreatureVisuals2);
					this._creatureContainer.AddChildSafely(ncreatureVisuals2);
					ncreatureVisuals2.SpineAnimation.SetAnimation("die", false, 0);
				}
				float num = Math.Min(250f, (base.Size.X - 200f) / (float)(list.Count - 1));
				float num2 = (float)(list.Count - 1) * -num * 0.5f;
				foreach (NCreatureVisuals ncreatureVisuals3 in list)
				{
					ncreatureVisuals3.Position = this._creatureContainer.Size * 0.5f + new Vector2(num2, 200f);
					num2 += num;
				}
			}
			IL_02E6:
			list2.Sort((NCreature c1, NCreature c2) => c1.GetIndex(false).CompareTo(c2.GetIndex(false)));
			foreach (NCreature ncreature in list2)
			{
				ncreature.AnimHideIntent(0.0);
				ncreature.AnimDisableUi();
			}
			foreach (NCreatureVisuals ncreatureVisuals4 in list)
			{
				ncreatureVisuals4.Reparent(this._creatureContainer, true);
			}
		}

		/// <summary>
		/// Called when the Game Over screen appears (immediately upon Death/Victory)
		/// </summary>
		// Token: 0x06002C57 RID: 11351 RVA: 0x0013AFD0 File Offset: 0x001391D0
		private async Task AnimateIn()
		{
			Tween backstopTween = base.CreateTween();
			this._uiNode.Modulate = StsColors.transparentWhite;
			if (NEventRoom.Instance != null)
			{
				CanvasItem fullBlackBackstop = this._fullBlackBackstop;
				Color color = this._fullBlackBackstop.Modulate;
				color.A = 0f;
				fullBlackBackstop.Modulate = color;
				this._fullBlackBackstop.Visible = true;
				backstopTween.TweenProperty(this._fullBlackBackstop, "modulate:a", 1f, 0.2);
				foreach (NCreatureVisuals ncreatureVisuals in this._creatureContainer.GetChildren(false).OfType<NCreatureVisuals>())
				{
					CanvasItem canvasItem = ncreatureVisuals;
					color = ncreatureVisuals.Modulate;
					color.A = 0f;
					canvasItem.Modulate = color;
					backstopTween.Parallel().TweenProperty(ncreatureVisuals, "modulate:a", 1f, 0.2);
				}
			}
			Variant shaderParameter = this._backstopMaterial.GetShaderParameter(NGameOverScreen._threshold);
			backstopTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateBackstopMaterial)), shaderParameter, 1f, 1.5).SetEase(2L).SetTrans(1L);
			TaskAwaiter<bool> taskAwaiter = backstopTween.AwaitFinished(this).GetAwaiter();
			TaskAwaiter<bool> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this._banner.AnimateIn();
				backstopTween.Kill();
				Tween tween = base.CreateTween();
				tween.TweenProperty(this._uiNode, "modulate:a", 1f, 0.25);
				TaskAwaiter<bool> taskAwaiter3 = tween.AwaitFinished(this).GetAwaiter();
				if (!taskAwaiter3.IsCompleted)
				{
					await taskAwaiter3;
					taskAwaiter3 = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter3.GetResult())
				{
					TaskHelper.RunSafely(this.AnimateInQuote());
					this._continueButton.Enable();
				}
			}
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x0013B013 File Offset: 0x00139213
		private void UpdateBackstopMaterial(float value)
		{
			this._backstopMaterial.SetShaderParameter(NGameOverScreen._threshold, value);
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x0013B02B File Offset: 0x0013922B
		public void AfterOverlayClosed()
		{
			this.QueueFreeSafely();
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x0013B033 File Offset: 0x00139233
		public void AfterOverlayShown()
		{
			NGame.Instance.SetScreenShakeTarget(this._screenshakeContainer);
			base.Visible = true;
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x0013B04C File Offset: 0x0013924C
		public void AfterOverlayHidden()
		{
			base.Visible = false;
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002C5C RID: 11356 RVA: 0x0013B055 File Offset: 0x00139255
		public bool UseSharedBackstop
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002C5D RID: 11357 RVA: 0x0013B058 File Offset: 0x00139258
		[Nullable(2)]
		public Control FocusedControlFromTopBar
		{
			[NullableContext(2)]
			get
			{
				if (this._badgeContainer.GetChildCount(false) > 0)
				{
					return this._badgeContainer.GetChild<NBadge>(0, false);
				}
				if (this._summaryContainer.DefaultFocusedControl != null)
				{
					return this._summaryContainer.DefaultFocusedControl;
				}
				return this;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002C5E RID: 11358 RVA: 0x0013B091 File Offset: 0x00139291
		public Control DefaultFocusedControl
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// Little helper function that creates the x1.1 sort of string for the Score Line.
		/// </summary>
		/// <param name="ascension"></param>
		/// <returns></returns>
		// Token: 0x06002C5F RID: 11359 RVA: 0x0013B094 File Offset: 0x00139294
		private string GetAscensionMulti(int ascension)
		{
			int num = ascension / 10 + 1;
			int num2 = ascension % 10;
			if (num2 != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return num.ToString();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002C60 RID: 11360 RVA: 0x0013B0E4 File Offset: 0x001392E4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(22)
			{
				new MethodInfo(NGameOverScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.DiscoveredAnyEpochs, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.InitializeBannerAndQuote, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.OpenSummaryScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NGameOverScreen.MethodName.AddScoreLine, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "locEntryKey", 0L, "", 6L, false),
					new PropertyInfo(4L, "locAmountKey", 0L, "", 6L, false),
					new PropertyInfo(2L, "amount", 0L, "", 6L, false),
					new PropertyInfo(4L, "scoreLabel", 0L, "", 6L, false),
					new PropertyInfo(4L, "iconPath", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGameOverScreen.MethodName.PlayUnlockSfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.TweenScore, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGameOverScreen.MethodName.GetScoreThreshold, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "unlocksRemaining", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGameOverScreen.MethodName.ShowLeaderboard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NGameOverScreen.MethodName.HideSummary, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.OpenRunHistoryScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NGameOverScreen.MethodName.OnMainMenuButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NGameOverScreen.MethodName.OpenTimeline, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.ReturnToMainMenu, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.AfterOverlayOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.MoveCreaturesToDifferentLayerAndDisableUi, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.UpdateBackstopMaterial, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGameOverScreen.MethodName.AfterOverlayClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.AfterOverlayShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.AfterOverlayHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGameOverScreen.MethodName.GetAscensionMulti, new PropertyInfo(4L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "ascension", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C61 RID: 11361 RVA: 0x0013B704 File Offset: 0x00139904
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGameOverScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.DiscoveredAnyEpochs && args.Count == 0)
			{
				bool flag = this.DiscoveredAnyEpochs();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.InitializeBannerAndQuote && args.Count == 0)
			{
				this.InitializeBannerAndQuote();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.OpenSummaryScreen && args.Count == 1)
			{
				this.OpenSummaryScreen(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.AddScoreLine && args.Count == 5)
			{
				this.AddScoreLine(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]), VariantUtils.ConvertTo<int>(args[2]), VariantUtils.ConvertTo<string>(args[3]), VariantUtils.ConvertTo<string>(args[4]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.PlayUnlockSfx && args.Count == 0)
			{
				this.PlayUnlockSfx();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.TweenScore && args.Count == 1)
			{
				this.TweenScore(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.GetScoreThreshold && args.Count == 1)
			{
				int scoreThreshold = this.GetScoreThreshold(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<int>(ref scoreThreshold);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.ShowLeaderboard && args.Count == 1)
			{
				this.ShowLeaderboard(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.HideSummary && args.Count == 0)
			{
				this.HideSummary();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.OpenRunHistoryScreen && args.Count == 1)
			{
				this.OpenRunHistoryScreen(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.OnMainMenuButtonPressed && args.Count == 1)
			{
				this.OnMainMenuButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.OpenTimeline && args.Count == 0)
			{
				this.OpenTimeline();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.ReturnToMainMenu && args.Count == 0)
			{
				this.ReturnToMainMenu();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.AfterOverlayOpened && args.Count == 0)
			{
				this.AfterOverlayOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.MoveCreaturesToDifferentLayerAndDisableUi && args.Count == 0)
			{
				this.MoveCreaturesToDifferentLayerAndDisableUi();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.UpdateBackstopMaterial && args.Count == 1)
			{
				this.UpdateBackstopMaterial(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.AfterOverlayClosed && args.Count == 0)
			{
				this.AfterOverlayClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.AfterOverlayShown && args.Count == 0)
			{
				this.AfterOverlayShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.AfterOverlayHidden && args.Count == 0)
			{
				this.AfterOverlayHidden();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGameOverScreen.MethodName.GetAscensionMulti && args.Count == 1)
			{
				string ascensionMulti = this.GetAscensionMulti(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref ascensionMulti);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002C62 RID: 11362 RVA: 0x0013BB10 File Offset: 0x00139D10
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGameOverScreen.MethodName._Ready || (ref method) == NGameOverScreen.MethodName._ExitTree || (ref method) == NGameOverScreen.MethodName.DiscoveredAnyEpochs || (ref method) == NGameOverScreen.MethodName.InitializeBannerAndQuote || (ref method) == NGameOverScreen.MethodName.OpenSummaryScreen || (ref method) == NGameOverScreen.MethodName.AddScoreLine || (ref method) == NGameOverScreen.MethodName.PlayUnlockSfx || (ref method) == NGameOverScreen.MethodName.TweenScore || (ref method) == NGameOverScreen.MethodName.GetScoreThreshold || (ref method) == NGameOverScreen.MethodName.ShowLeaderboard || (ref method) == NGameOverScreen.MethodName.HideSummary || (ref method) == NGameOverScreen.MethodName.OpenRunHistoryScreen || (ref method) == NGameOverScreen.MethodName.OnMainMenuButtonPressed || (ref method) == NGameOverScreen.MethodName.OpenTimeline || (ref method) == NGameOverScreen.MethodName.ReturnToMainMenu || (ref method) == NGameOverScreen.MethodName.AfterOverlayOpened || (ref method) == NGameOverScreen.MethodName.MoveCreaturesToDifferentLayerAndDisableUi || (ref method) == NGameOverScreen.MethodName.UpdateBackstopMaterial || (ref method) == NGameOverScreen.MethodName.AfterOverlayClosed || (ref method) == NGameOverScreen.MethodName.AfterOverlayShown || (ref method) == NGameOverScreen.MethodName.AfterOverlayHidden || (ref method) == NGameOverScreen.MethodName.GetAscensionMulti || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002C63 RID: 11363 RVA: 0x0013BC70 File Offset: 0x00139E70
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGameOverScreen.PropertyName._continueButton)
			{
				this._continueButton = VariantUtils.ConvertTo<NGameOverContinueButton>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._viewRunButton)
			{
				this._viewRunButton = VariantUtils.ConvertTo<NViewRunButton>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._mainMenuButton)
			{
				this._mainMenuButton = VariantUtils.ConvertTo<NReturnToMainMenuButton>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._leaderboardButton)
			{
				this._leaderboardButton = VariantUtils.ConvertTo<NGameOverContinueButton>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._badgeContainer)
			{
				this._badgeContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreLineContainer)
			{
				this._scoreLineContainer = VariantUtils.ConvertTo<GridContainer>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreBar)
			{
				this._scoreBar = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreFg)
			{
				this._scoreFg = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreProgress)
			{
				this._scoreProgress = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._unlocksRemaining)
			{
				this._unlocksRemaining = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._score)
			{
				this._score = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreThreshold)
			{
				this._scoreThreshold = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreUnlockedEpochId)
			{
				this._scoreUnlockedEpochId = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._leaderboard)
			{
				this._leaderboard = VariantUtils.ConvertTo<NDailyRunLeaderboard>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._creatureContainer)
			{
				this._creatureContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._summaryContainer)
			{
				this._summaryContainer = VariantUtils.ConvertTo<NRunSummary>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._fullBlackBackstop)
			{
				this._fullBlackBackstop = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._summaryBackstop)
			{
				this._summaryBackstop = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._backstop)
			{
				this._backstop = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._banner)
			{
				this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._deathQuote)
			{
				this._deathQuote = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._victoryDamageLabel)
			{
				this._victoryDamageLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._uiNode)
			{
				this._uiNode = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._screenshakeContainer)
			{
				this._screenshakeContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._discoveryLabel)
			{
				this._discoveryLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._encounterQuote)
			{
				this._encounterQuote = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._isAnimatingSummary)
			{
				this._isAnimatingSummary = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._backstopMaterial)
			{
				this._backstopMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._quoteTween)
			{
				this._quoteTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002C64 RID: 11364 RVA: 0x0013BF94 File Offset: 0x0013A194
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGameOverScreen.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName.FocusedControlFromTopBar)
			{
				Control control = this.FocusedControlFromTopBar;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._continueButton)
			{
				value = VariantUtils.CreateFrom<NGameOverContinueButton>(ref this._continueButton);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._viewRunButton)
			{
				value = VariantUtils.CreateFrom<NViewRunButton>(ref this._viewRunButton);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._mainMenuButton)
			{
				value = VariantUtils.CreateFrom<NReturnToMainMenuButton>(ref this._mainMenuButton);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._leaderboardButton)
			{
				value = VariantUtils.CreateFrom<NGameOverContinueButton>(ref this._leaderboardButton);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._badgeContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._badgeContainer);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreLineContainer)
			{
				value = VariantUtils.CreateFrom<GridContainer>(ref this._scoreLineContainer);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreBar)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._scoreBar);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreFg)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._scoreFg);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreProgress)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._scoreProgress);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._unlocksRemaining)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._unlocksRemaining);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._score)
			{
				value = VariantUtils.CreateFrom<int>(ref this._score);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreThreshold)
			{
				value = VariantUtils.CreateFrom<int>(ref this._scoreThreshold);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._scoreUnlockedEpochId)
			{
				value = VariantUtils.CreateFrom<string>(ref this._scoreUnlockedEpochId);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._leaderboard)
			{
				value = VariantUtils.CreateFrom<NDailyRunLeaderboard>(ref this._leaderboard);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._creatureContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._creatureContainer);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._summaryContainer)
			{
				value = VariantUtils.CreateFrom<NRunSummary>(ref this._summaryContainer);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._fullBlackBackstop)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._fullBlackBackstop);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._summaryBackstop)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._summaryBackstop);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._backstop)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._backstop);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._banner)
			{
				value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._deathQuote)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._deathQuote);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._victoryDamageLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._victoryDamageLabel);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._uiNode)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._uiNode);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._screenshakeContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._screenshakeContainer);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._discoveryLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._discoveryLabel);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._encounterQuote)
			{
				value = VariantUtils.CreateFrom<string>(ref this._encounterQuote);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._isAnimatingSummary)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isAnimatingSummary);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._backstopMaterial)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._backstopMaterial);
				return true;
			}
			if ((ref name) == NGameOverScreen.PropertyName._quoteTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._quoteTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002C65 RID: 11365 RVA: 0x0013C3D8 File Offset: 0x0013A5D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGameOverScreen.PropertyName._continueButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._viewRunButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._mainMenuButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._leaderboardButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._badgeContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._scoreLineContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._scoreBar, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._scoreFg, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._scoreProgress, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._unlocksRemaining, 0L, "", 4096L, false),
				new PropertyInfo(2L, NGameOverScreen.PropertyName._score, 0L, "", 4096L, false),
				new PropertyInfo(2L, NGameOverScreen.PropertyName._scoreThreshold, 0L, "", 4096L, false),
				new PropertyInfo(4L, NGameOverScreen.PropertyName._scoreUnlockedEpochId, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._leaderboard, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._creatureContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._summaryContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._fullBlackBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._summaryBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._backstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._banner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._deathQuote, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._victoryDamageLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._uiNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._screenshakeContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._discoveryLabel, 0L, "", 4096L, false),
				new PropertyInfo(4L, NGameOverScreen.PropertyName._encounterQuote, 0L, "", 4096L, false),
				new PropertyInfo(1L, NGameOverScreen.PropertyName._isAnimatingSummary, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._backstopMaterial, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName._quoteTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NGameOverScreen.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(1L, NGameOverScreen.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName.FocusedControlFromTopBar, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGameOverScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C66 RID: 11366 RVA: 0x0013C828 File Offset: 0x0013AA28
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGameOverScreen.PropertyName._continueButton, Variant.From<NGameOverContinueButton>(ref this._continueButton));
			info.AddProperty(NGameOverScreen.PropertyName._viewRunButton, Variant.From<NViewRunButton>(ref this._viewRunButton));
			info.AddProperty(NGameOverScreen.PropertyName._mainMenuButton, Variant.From<NReturnToMainMenuButton>(ref this._mainMenuButton));
			info.AddProperty(NGameOverScreen.PropertyName._leaderboardButton, Variant.From<NGameOverContinueButton>(ref this._leaderboardButton));
			info.AddProperty(NGameOverScreen.PropertyName._badgeContainer, Variant.From<Control>(ref this._badgeContainer));
			info.AddProperty(NGameOverScreen.PropertyName._scoreLineContainer, Variant.From<GridContainer>(ref this._scoreLineContainer));
			info.AddProperty(NGameOverScreen.PropertyName._scoreBar, Variant.From<Control>(ref this._scoreBar));
			info.AddProperty(NGameOverScreen.PropertyName._scoreFg, Variant.From<Control>(ref this._scoreFg));
			info.AddProperty(NGameOverScreen.PropertyName._scoreProgress, Variant.From<MegaLabel>(ref this._scoreProgress));
			info.AddProperty(NGameOverScreen.PropertyName._unlocksRemaining, Variant.From<MegaLabel>(ref this._unlocksRemaining));
			info.AddProperty(NGameOverScreen.PropertyName._score, Variant.From<int>(ref this._score));
			info.AddProperty(NGameOverScreen.PropertyName._scoreThreshold, Variant.From<int>(ref this._scoreThreshold));
			info.AddProperty(NGameOverScreen.PropertyName._scoreUnlockedEpochId, Variant.From<string>(ref this._scoreUnlockedEpochId));
			info.AddProperty(NGameOverScreen.PropertyName._leaderboard, Variant.From<NDailyRunLeaderboard>(ref this._leaderboard));
			info.AddProperty(NGameOverScreen.PropertyName._creatureContainer, Variant.From<Control>(ref this._creatureContainer));
			info.AddProperty(NGameOverScreen.PropertyName._summaryContainer, Variant.From<NRunSummary>(ref this._summaryContainer));
			info.AddProperty(NGameOverScreen.PropertyName._fullBlackBackstop, Variant.From<ColorRect>(ref this._fullBlackBackstop));
			info.AddProperty(NGameOverScreen.PropertyName._summaryBackstop, Variant.From<ColorRect>(ref this._summaryBackstop));
			info.AddProperty(NGameOverScreen.PropertyName._backstop, Variant.From<ColorRect>(ref this._backstop));
			info.AddProperty(NGameOverScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
			info.AddProperty(NGameOverScreen.PropertyName._deathQuote, Variant.From<MegaRichTextLabel>(ref this._deathQuote));
			info.AddProperty(NGameOverScreen.PropertyName._victoryDamageLabel, Variant.From<MegaRichTextLabel>(ref this._victoryDamageLabel));
			info.AddProperty(NGameOverScreen.PropertyName._uiNode, Variant.From<Control>(ref this._uiNode));
			info.AddProperty(NGameOverScreen.PropertyName._screenshakeContainer, Variant.From<Control>(ref this._screenshakeContainer));
			info.AddProperty(NGameOverScreen.PropertyName._discoveryLabel, Variant.From<MegaLabel>(ref this._discoveryLabel));
			info.AddProperty(NGameOverScreen.PropertyName._encounterQuote, Variant.From<string>(ref this._encounterQuote));
			info.AddProperty(NGameOverScreen.PropertyName._isAnimatingSummary, Variant.From<bool>(ref this._isAnimatingSummary));
			info.AddProperty(NGameOverScreen.PropertyName._backstopMaterial, Variant.From<ShaderMaterial>(ref this._backstopMaterial));
			info.AddProperty(NGameOverScreen.PropertyName._quoteTween, Variant.From<Tween>(ref this._quoteTween));
		}

		/// <inheritdoc />
		// Token: 0x06002C67 RID: 11367 RVA: 0x0013CABC File Offset: 0x0013ACBC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._continueButton, ref variant))
			{
				this._continueButton = variant.As<NGameOverContinueButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._viewRunButton, ref variant2))
			{
				this._viewRunButton = variant2.As<NViewRunButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._mainMenuButton, ref variant3))
			{
				this._mainMenuButton = variant3.As<NReturnToMainMenuButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._leaderboardButton, ref variant4))
			{
				this._leaderboardButton = variant4.As<NGameOverContinueButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._badgeContainer, ref variant5))
			{
				this._badgeContainer = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreLineContainer, ref variant6))
			{
				this._scoreLineContainer = variant6.As<GridContainer>();
			}
			Variant variant7;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreBar, ref variant7))
			{
				this._scoreBar = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreFg, ref variant8))
			{
				this._scoreFg = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreProgress, ref variant9))
			{
				this._scoreProgress = variant9.As<MegaLabel>();
			}
			Variant variant10;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._unlocksRemaining, ref variant10))
			{
				this._unlocksRemaining = variant10.As<MegaLabel>();
			}
			Variant variant11;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._score, ref variant11))
			{
				this._score = variant11.As<int>();
			}
			Variant variant12;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreThreshold, ref variant12))
			{
				this._scoreThreshold = variant12.As<int>();
			}
			Variant variant13;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreUnlockedEpochId, ref variant13))
			{
				this._scoreUnlockedEpochId = variant13.As<string>();
			}
			Variant variant14;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._leaderboard, ref variant14))
			{
				this._leaderboard = variant14.As<NDailyRunLeaderboard>();
			}
			Variant variant15;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._creatureContainer, ref variant15))
			{
				this._creatureContainer = variant15.As<Control>();
			}
			Variant variant16;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._summaryContainer, ref variant16))
			{
				this._summaryContainer = variant16.As<NRunSummary>();
			}
			Variant variant17;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._fullBlackBackstop, ref variant17))
			{
				this._fullBlackBackstop = variant17.As<ColorRect>();
			}
			Variant variant18;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._summaryBackstop, ref variant18))
			{
				this._summaryBackstop = variant18.As<ColorRect>();
			}
			Variant variant19;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._backstop, ref variant19))
			{
				this._backstop = variant19.As<ColorRect>();
			}
			Variant variant20;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._banner, ref variant20))
			{
				this._banner = variant20.As<NCommonBanner>();
			}
			Variant variant21;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._deathQuote, ref variant21))
			{
				this._deathQuote = variant21.As<MegaRichTextLabel>();
			}
			Variant variant22;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._victoryDamageLabel, ref variant22))
			{
				this._victoryDamageLabel = variant22.As<MegaRichTextLabel>();
			}
			Variant variant23;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._uiNode, ref variant23))
			{
				this._uiNode = variant23.As<Control>();
			}
			Variant variant24;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._screenshakeContainer, ref variant24))
			{
				this._screenshakeContainer = variant24.As<Control>();
			}
			Variant variant25;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._discoveryLabel, ref variant25))
			{
				this._discoveryLabel = variant25.As<MegaLabel>();
			}
			Variant variant26;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._encounterQuote, ref variant26))
			{
				this._encounterQuote = variant26.As<string>();
			}
			Variant variant27;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._isAnimatingSummary, ref variant27))
			{
				this._isAnimatingSummary = variant27.As<bool>();
			}
			Variant variant28;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._backstopMaterial, ref variant28))
			{
				this._backstopMaterial = variant28.As<ShaderMaterial>();
			}
			Variant variant29;
			if (info.TryGetProperty(NGameOverScreen.PropertyName._quoteTween, ref variant29))
			{
				this._quoteTween = variant29.As<Tween>();
			}
		}

		// Token: 0x040010A9 RID: 4265
		private static readonly StringName _threshold = new StringName("threshold");

		// Token: 0x040010AA RID: 4266
		private RunState _runState;

		// Token: 0x040010AB RID: 4267
		private SerializableRun _serializableRun;

		// Token: 0x040010AC RID: 4268
		private RunHistory _history;

		// Token: 0x040010AD RID: 4269
		private Player _localPlayer;

		// Token: 0x040010AE RID: 4270
		private NGameOverContinueButton _continueButton;

		// Token: 0x040010AF RID: 4271
		private NViewRunButton _viewRunButton;

		// Token: 0x040010B0 RID: 4272
		private NReturnToMainMenuButton _mainMenuButton;

		// Token: 0x040010B1 RID: 4273
		private NGameOverContinueButton _leaderboardButton;

		// Token: 0x040010B2 RID: 4274
		private Control _badgeContainer;

		// Token: 0x040010B3 RID: 4275
		private GridContainer _scoreLineContainer;

		// Token: 0x040010B4 RID: 4276
		private readonly List<NScoreLine> _scoreLines = new List<NScoreLine>();

		// Token: 0x040010B5 RID: 4277
		private Control _scoreBar;

		// Token: 0x040010B6 RID: 4278
		private Control _scoreFg;

		// Token: 0x040010B7 RID: 4279
		private MegaLabel _scoreProgress;

		// Token: 0x040010B8 RID: 4280
		private MegaLabel _unlocksRemaining;

		// Token: 0x040010B9 RID: 4281
		private int _score;

		// Token: 0x040010BA RID: 4282
		private int _scoreThreshold;

		// Token: 0x040010BB RID: 4283
		[Nullable(2)]
		private string _scoreUnlockedEpochId;

		// Token: 0x040010BC RID: 4284
		private NDailyRunLeaderboard _leaderboard;

		// Token: 0x040010BD RID: 4285
		private Control _creatureContainer;

		// Token: 0x040010BE RID: 4286
		private NRunSummary _summaryContainer;

		// Token: 0x040010BF RID: 4287
		private ColorRect _fullBlackBackstop;

		// Token: 0x040010C0 RID: 4288
		private ColorRect _summaryBackstop;

		// Token: 0x040010C1 RID: 4289
		private ColorRect _backstop;

		// Token: 0x040010C2 RID: 4290
		private NCommonBanner _banner;

		// Token: 0x040010C3 RID: 4291
		private MegaRichTextLabel _deathQuote;

		// Token: 0x040010C4 RID: 4292
		private MegaRichTextLabel _victoryDamageLabel;

		// Token: 0x040010C5 RID: 4293
		private Control _uiNode;

		// Token: 0x040010C6 RID: 4294
		private Control _screenshakeContainer;

		// Token: 0x040010C7 RID: 4295
		private MegaLabel _discoveryLabel;

		// Token: 0x040010C8 RID: 4296
		private string _encounterQuote;

		// Token: 0x040010C9 RID: 4297
		private bool _isAnimatingSummary;

		// Token: 0x040010CA RID: 4298
		private ShaderMaterial _backstopMaterial;

		// Token: 0x040010CB RID: 4299
		[Nullable(2)]
		private Tween _quoteTween;

		// Token: 0x040010CC RID: 4300
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200154D RID: 5453
		[NullableContext(0)]
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005175 RID: 20853
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005176 RID: 20854
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'DiscoveredAnyEpochs' method.
			/// </summary>
			// Token: 0x04005177 RID: 20855
			public static readonly StringName DiscoveredAnyEpochs = "DiscoveredAnyEpochs";

			/// <summary>
			/// Cached name for the 'InitializeBannerAndQuote' method.
			/// </summary>
			// Token: 0x04005178 RID: 20856
			public static readonly StringName InitializeBannerAndQuote = "InitializeBannerAndQuote";

			/// <summary>
			/// Cached name for the 'OpenSummaryScreen' method.
			/// </summary>
			// Token: 0x04005179 RID: 20857
			public static readonly StringName OpenSummaryScreen = "OpenSummaryScreen";

			/// <summary>
			/// Cached name for the 'AddScoreLine' method.
			/// </summary>
			// Token: 0x0400517A RID: 20858
			public static readonly StringName AddScoreLine = "AddScoreLine";

			/// <summary>
			/// Cached name for the 'PlayUnlockSfx' method.
			/// </summary>
			// Token: 0x0400517B RID: 20859
			public static readonly StringName PlayUnlockSfx = "PlayUnlockSfx";

			/// <summary>
			/// Cached name for the 'TweenScore' method.
			/// </summary>
			// Token: 0x0400517C RID: 20860
			public static readonly StringName TweenScore = "TweenScore";

			/// <summary>
			/// Cached name for the 'GetScoreThreshold' method.
			/// </summary>
			// Token: 0x0400517D RID: 20861
			public static readonly StringName GetScoreThreshold = "GetScoreThreshold";

			/// <summary>
			/// Cached name for the 'ShowLeaderboard' method.
			/// </summary>
			// Token: 0x0400517E RID: 20862
			public static readonly StringName ShowLeaderboard = "ShowLeaderboard";

			/// <summary>
			/// Cached name for the 'HideSummary' method.
			/// </summary>
			// Token: 0x0400517F RID: 20863
			public static readonly StringName HideSummary = "HideSummary";

			/// <summary>
			/// Cached name for the 'OpenRunHistoryScreen' method.
			/// </summary>
			// Token: 0x04005180 RID: 20864
			public static readonly StringName OpenRunHistoryScreen = "OpenRunHistoryScreen";

			/// <summary>
			/// Cached name for the 'OnMainMenuButtonPressed' method.
			/// </summary>
			// Token: 0x04005181 RID: 20865
			public static readonly StringName OnMainMenuButtonPressed = "OnMainMenuButtonPressed";

			/// <summary>
			/// Cached name for the 'OpenTimeline' method.
			/// </summary>
			// Token: 0x04005182 RID: 20866
			public static readonly StringName OpenTimeline = "OpenTimeline";

			/// <summary>
			/// Cached name for the 'ReturnToMainMenu' method.
			/// </summary>
			// Token: 0x04005183 RID: 20867
			public static readonly StringName ReturnToMainMenu = "ReturnToMainMenu";

			/// <summary>
			/// Cached name for the 'AfterOverlayOpened' method.
			/// </summary>
			// Token: 0x04005184 RID: 20868
			public static readonly StringName AfterOverlayOpened = "AfterOverlayOpened";

			/// <summary>
			/// Cached name for the 'MoveCreaturesToDifferentLayerAndDisableUi' method.
			/// </summary>
			// Token: 0x04005185 RID: 20869
			public static readonly StringName MoveCreaturesToDifferentLayerAndDisableUi = "MoveCreaturesToDifferentLayerAndDisableUi";

			/// <summary>
			/// Cached name for the 'UpdateBackstopMaterial' method.
			/// </summary>
			// Token: 0x04005186 RID: 20870
			public static readonly StringName UpdateBackstopMaterial = "UpdateBackstopMaterial";

			/// <summary>
			/// Cached name for the 'AfterOverlayClosed' method.
			/// </summary>
			// Token: 0x04005187 RID: 20871
			public static readonly StringName AfterOverlayClosed = "AfterOverlayClosed";

			/// <summary>
			/// Cached name for the 'AfterOverlayShown' method.
			/// </summary>
			// Token: 0x04005188 RID: 20872
			public static readonly StringName AfterOverlayShown = "AfterOverlayShown";

			/// <summary>
			/// Cached name for the 'AfterOverlayHidden' method.
			/// </summary>
			// Token: 0x04005189 RID: 20873
			public static readonly StringName AfterOverlayHidden = "AfterOverlayHidden";

			/// <summary>
			/// Cached name for the 'GetAscensionMulti' method.
			/// </summary>
			// Token: 0x0400518A RID: 20874
			public static readonly StringName GetAscensionMulti = "GetAscensionMulti";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200154E RID: 5454
		[NullableContext(0)]
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x0400518B RID: 20875
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x0400518C RID: 20876
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'FocusedControlFromTopBar' property.
			/// </summary>
			// Token: 0x0400518D RID: 20877
			public static readonly StringName FocusedControlFromTopBar = "FocusedControlFromTopBar";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x0400518E RID: 20878
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_continueButton' field.
			/// </summary>
			// Token: 0x0400518F RID: 20879
			public static readonly StringName _continueButton = "_continueButton";

			/// <summary>
			/// Cached name for the '_viewRunButton' field.
			/// </summary>
			// Token: 0x04005190 RID: 20880
			public static readonly StringName _viewRunButton = "_viewRunButton";

			/// <summary>
			/// Cached name for the '_mainMenuButton' field.
			/// </summary>
			// Token: 0x04005191 RID: 20881
			public static readonly StringName _mainMenuButton = "_mainMenuButton";

			/// <summary>
			/// Cached name for the '_leaderboardButton' field.
			/// </summary>
			// Token: 0x04005192 RID: 20882
			public static readonly StringName _leaderboardButton = "_leaderboardButton";

			/// <summary>
			/// Cached name for the '_badgeContainer' field.
			/// </summary>
			// Token: 0x04005193 RID: 20883
			public static readonly StringName _badgeContainer = "_badgeContainer";

			/// <summary>
			/// Cached name for the '_scoreLineContainer' field.
			/// </summary>
			// Token: 0x04005194 RID: 20884
			public static readonly StringName _scoreLineContainer = "_scoreLineContainer";

			/// <summary>
			/// Cached name for the '_scoreBar' field.
			/// </summary>
			// Token: 0x04005195 RID: 20885
			public static readonly StringName _scoreBar = "_scoreBar";

			/// <summary>
			/// Cached name for the '_scoreFg' field.
			/// </summary>
			// Token: 0x04005196 RID: 20886
			public static readonly StringName _scoreFg = "_scoreFg";

			/// <summary>
			/// Cached name for the '_scoreProgress' field.
			/// </summary>
			// Token: 0x04005197 RID: 20887
			public static readonly StringName _scoreProgress = "_scoreProgress";

			/// <summary>
			/// Cached name for the '_unlocksRemaining' field.
			/// </summary>
			// Token: 0x04005198 RID: 20888
			public static readonly StringName _unlocksRemaining = "_unlocksRemaining";

			/// <summary>
			/// Cached name for the '_score' field.
			/// </summary>
			// Token: 0x04005199 RID: 20889
			public static readonly StringName _score = "_score";

			/// <summary>
			/// Cached name for the '_scoreThreshold' field.
			/// </summary>
			// Token: 0x0400519A RID: 20890
			public static readonly StringName _scoreThreshold = "_scoreThreshold";

			/// <summary>
			/// Cached name for the '_scoreUnlockedEpochId' field.
			/// </summary>
			// Token: 0x0400519B RID: 20891
			public static readonly StringName _scoreUnlockedEpochId = "_scoreUnlockedEpochId";

			/// <summary>
			/// Cached name for the '_leaderboard' field.
			/// </summary>
			// Token: 0x0400519C RID: 20892
			public static readonly StringName _leaderboard = "_leaderboard";

			/// <summary>
			/// Cached name for the '_creatureContainer' field.
			/// </summary>
			// Token: 0x0400519D RID: 20893
			public static readonly StringName _creatureContainer = "_creatureContainer";

			/// <summary>
			/// Cached name for the '_summaryContainer' field.
			/// </summary>
			// Token: 0x0400519E RID: 20894
			public static readonly StringName _summaryContainer = "_summaryContainer";

			/// <summary>
			/// Cached name for the '_fullBlackBackstop' field.
			/// </summary>
			// Token: 0x0400519F RID: 20895
			public static readonly StringName _fullBlackBackstop = "_fullBlackBackstop";

			/// <summary>
			/// Cached name for the '_summaryBackstop' field.
			/// </summary>
			// Token: 0x040051A0 RID: 20896
			public static readonly StringName _summaryBackstop = "_summaryBackstop";

			/// <summary>
			/// Cached name for the '_backstop' field.
			/// </summary>
			// Token: 0x040051A1 RID: 20897
			public static readonly StringName _backstop = "_backstop";

			/// <summary>
			/// Cached name for the '_banner' field.
			/// </summary>
			// Token: 0x040051A2 RID: 20898
			public static readonly StringName _banner = "_banner";

			/// <summary>
			/// Cached name for the '_deathQuote' field.
			/// </summary>
			// Token: 0x040051A3 RID: 20899
			public static readonly StringName _deathQuote = "_deathQuote";

			/// <summary>
			/// Cached name for the '_victoryDamageLabel' field.
			/// </summary>
			// Token: 0x040051A4 RID: 20900
			public static readonly StringName _victoryDamageLabel = "_victoryDamageLabel";

			/// <summary>
			/// Cached name for the '_uiNode' field.
			/// </summary>
			// Token: 0x040051A5 RID: 20901
			public static readonly StringName _uiNode = "_uiNode";

			/// <summary>
			/// Cached name for the '_screenshakeContainer' field.
			/// </summary>
			// Token: 0x040051A6 RID: 20902
			public static readonly StringName _screenshakeContainer = "_screenshakeContainer";

			/// <summary>
			/// Cached name for the '_discoveryLabel' field.
			/// </summary>
			// Token: 0x040051A7 RID: 20903
			public static readonly StringName _discoveryLabel = "_discoveryLabel";

			/// <summary>
			/// Cached name for the '_encounterQuote' field.
			/// </summary>
			// Token: 0x040051A8 RID: 20904
			public static readonly StringName _encounterQuote = "_encounterQuote";

			/// <summary>
			/// Cached name for the '_isAnimatingSummary' field.
			/// </summary>
			// Token: 0x040051A9 RID: 20905
			public static readonly StringName _isAnimatingSummary = "_isAnimatingSummary";

			/// <summary>
			/// Cached name for the '_backstopMaterial' field.
			/// </summary>
			// Token: 0x040051AA RID: 20906
			public static readonly StringName _backstopMaterial = "_backstopMaterial";

			/// <summary>
			/// Cached name for the '_quoteTween' field.
			/// </summary>
			// Token: 0x040051AB RID: 20907
			public static readonly StringName _quoteTween = "_quoteTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200154F RID: 5455
		[NullableContext(0)]
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
