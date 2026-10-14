using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs
{
	// Token: 0x0200010E RID: 270
	[NullableContext(1)]
	[Nullable(0)]
	public static class RunHistoryUtilities
	{
		// Token: 0x06000C34 RID: 3124 RVA: 0x0004E198 File Offset: 0x0004C398
		public static void CreateRunHistoryEntry(SerializableRun run, bool victory, bool isAbandoned, PlatformType platformType)
		{
			ModelId modelId = ModelId.none;
			ModelId modelId2 = ModelId.none;
			List<MapPointHistoryEntry> list = run.MapPointHistory.LastOrDefault<List<MapPointHistoryEntry>>();
			MapPointHistoryEntry mapPointHistoryEntry = ((list != null) ? list.LastOrDefault<MapPointHistoryEntry>() : null);
			if (!victory && mapPointHistoryEntry != null)
			{
				MapPointRoomHistoryEntry mapPointRoomHistoryEntry = mapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>();
				RoomType roomType = mapPointRoomHistoryEntry.RoomType;
				if (roomType.IsCombatRoom())
				{
					modelId = mapPointRoomHistoryEntry.ModelId;
				}
				else if (roomType == RoomType.Event)
				{
					modelId2 = mapPointRoomHistoryEntry.ModelId;
				}
			}
			List<RunHistoryPlayer> list2 = new List<RunHistoryPlayer>();
			foreach (SerializablePlayer serializablePlayer in run.Players)
			{
				RunHistoryPlayer runHistoryPlayer = new RunHistoryPlayer
				{
					Id = serializablePlayer.NetId,
					Character = serializablePlayer.CharacterId,
					Deck = serializablePlayer.Deck,
					Relics = serializablePlayer.Relics,
					Potions = serializablePlayer.Potions,
					Badges = RunHistoryUtilities.GetBadgesForPlayer(run, serializablePlayer, victory, isAbandoned),
					MaxPotionSlotCount = serializablePlayer.MaxPotionSlotCount
				};
				list2.Add(runHistoryPlayer);
			}
			RunHistory runHistory = new RunHistory();
			ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
			runHistory.BuildId = ((releaseInfo != null) ? releaseInfo.Version : null) ?? "NON-RELEASE-VERSION";
			runHistory.PlatformType = platformType;
			runHistory.Players = list2;
			runHistory.GameMode = RunHistoryUtilities.GetGameMode(run);
			runHistory.Win = victory;
			runHistory.KilledByEncounter = modelId;
			runHistory.KilledByEvent = modelId2;
			runHistory.WasAbandoned = isAbandoned;
			runHistory.Seed = run.SerializableRng.Seed;
			runHistory.StartTime = run.StartTime;
			runHistory.RunTime = (float)((run.WinTime > 0L) ? run.WinTime : run.RunTime);
			runHistory.MapPointHistory = run.MapPointHistory;
			runHistory.Ascension = run.Ascension;
			runHistory.Acts = run.Acts.Select<SerializableActModel, ModelId>((SerializableActModel a) => a.Id).ToList<ModelId>();
			runHistory.Modifiers = run.Modifiers;
			RunHistory runHistory2 = runHistory;
			SaveManager.Instance.SaveRunHistory(runHistory2);
			Log.Info("Created Run History entry!", 2);
			if (RunManager.Instance.IsInProgress)
			{
				RunManager.Instance.History = runHistory2;
			}
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0004E3E0 File Offset: 0x0004C5E0
		private static IEnumerable<SerializableBadge> GetBadgesForPlayer(SerializableRun run, SerializablePlayer player, bool won, bool isAbandoned)
		{
			List<SerializableBadge> list = new List<SerializableBadge>();
			if (isAbandoned)
			{
				return list;
			}
			List<Badge> badges = ScoreUtility.GetBadges(run, player.NetId, won);
			foreach (Badge badge in badges)
			{
				list.Add(badge.ToSerializable());
			}
			return list;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x0004E450 File Offset: 0x0004C650
		private static GameMode GetGameMode(SerializableRun run)
		{
			return run.GameMode;
		}
	}
}
