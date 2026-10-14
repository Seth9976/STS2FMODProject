using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Multiplayer.Replay
{
	// Token: 0x02000457 RID: 1111
	[NullableContext(1)]
	[Nullable(0)]
	public class CombatReplayWriter : IDisposable
	{
		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x0600455B RID: 17755 RVA: 0x00216531 File Offset: 0x00214731
		// (set) Token: 0x0600455C RID: 17756 RVA: 0x00216539 File Offset: 0x00214739
		public bool IsEnabled { get; set; } = true;

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x0600455D RID: 17757 RVA: 0x00216542 File Offset: 0x00214742
		public bool IsRecordingReplay
		{
			get
			{
				return this._replay != null;
			}
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x00216550 File Offset: 0x00214750
		public CombatReplayWriter(PlayerChoiceSynchronizer playerChoiceSynchronizer, RewardsSetSynchronizer rewardsSetSynchronizer, ActionQueueSet actionQueueSet, ActionQueueSynchronizer actionQueueSynchronizer, ChecksumTracker checksumTracker)
		{
			this._actionQueueSet = actionQueueSet;
			this._actionQueueSynchronizer = actionQueueSynchronizer;
			this._playerChoiceSynchronizer = playerChoiceSynchronizer;
			this._rewardsSetSynchronizer = rewardsSetSynchronizer;
			this._checksumTracker = checksumTracker;
			actionQueueSet.ActionEnqueued += this.RecordGameAction;
			actionQueueSet.ActionResumed += this.RecordActionResume;
			playerChoiceSynchronizer.PlayerChoiceReceived += this.RecordPlayerChoice;
			checksumTracker.ChecksumGenerated += this.RecordChecksum;
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x002165E4 File Offset: 0x002147E4
		public void Dispose()
		{
			this._actionQueueSet.ActionEnqueued -= this.RecordGameAction;
			this._actionQueueSet.ActionResumed -= this.RecordActionResume;
			this._playerChoiceSynchronizer.PlayerChoiceReceived -= this.RecordPlayerChoice;
			this._checksumTracker.ChecksumGenerated -= this.RecordChecksum;
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x00216650 File Offset: 0x00214850
		public void RecordInitialState(SerializableRun serializableRun)
		{
			if (!this.IsEnabled)
			{
				return;
			}
			CombatReplay combatReplay = new CombatReplay();
			ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
			combatReplay.version = ((releaseInfo != null) ? releaseInfo.Version : null) ?? "UNRELEASED";
			ReleaseInfo releaseInfo2 = ReleaseInfoManager.Instance.ReleaseInfo;
			string text;
			if ((text = ((releaseInfo2 != null) ? releaseInfo2.Commit : null)) == null)
			{
				text = GitHelper.ShortCommitId ?? "UNKNOWN";
			}
			combatReplay.gitCommit = text;
			combatReplay.modelIdHash = ModelIdSerializationCache.Hash;
			combatReplay.choiceIds = this._playerChoiceSynchronizer.ChoiceIds.ToList<uint>();
			combatReplay.rewardIds = this._rewardsSetSynchronizer.GetNextRewardIds().ToList<int>();
			combatReplay.nextActionId = this._actionQueueSet.NextActionId;
			combatReplay.nextChecksumId = this._checksumTracker.NextId;
			combatReplay.nextHookId = this._actionQueueSynchronizer.NextHookId;
			combatReplay.serializableRun = serializableRun;
			this._replay = combatReplay;
			this._replay.events.Clear();
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x00216748 File Offset: 0x00214948
		private void RecordGameAction(GameAction gameAction)
		{
			if (!this.IsEnabled)
			{
				return;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			if (this._replay == null)
			{
				throw new InvalidOperationException("RecordInitialState must be called first");
			}
			GenericHookGameAction genericHookGameAction = gameAction as GenericHookGameAction;
			if (genericHookGameAction != null)
			{
				this._replay.events.Add(new CombatReplayEvent
				{
					playerId = new ulong?(gameAction.OwnerId),
					eventType = CombatReplayEventType.HookAction,
					hookId = new uint?(genericHookGameAction.HookId),
					gameActionType = new GameActionType?(genericHookGameAction.ActionType)
				});
				return;
			}
			if (!gameAction.RecordableToReplay)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Found unrecordable game action: ");
				defaultInterpolatedStringHandler.AppendFormatted<GameAction>(gameAction);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._replay.events.Add(new CombatReplayEvent
			{
				playerId = new ulong?(gameAction.OwnerId),
				eventType = CombatReplayEventType.GameAction,
				action = gameAction.ToNetAction()
			});
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x00216854 File Offset: 0x00214A54
		private void RecordActionResume(uint actionId)
		{
			if (!this.IsEnabled)
			{
				return;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			if (this._replay == null)
			{
				throw new InvalidOperationException("RecordInitialState must be called first");
			}
			this._replay.events.Add(new CombatReplayEvent
			{
				eventType = CombatReplayEventType.ResumeAction,
				actionId = new uint?(actionId)
			});
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x002168B8 File Offset: 0x00214AB8
		private void RecordPlayerChoice(Player player, uint choiceId, NetPlayerChoiceResult result)
		{
			if (!this.IsEnabled)
			{
				return;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			if (this._replay == null)
			{
				throw new InvalidOperationException("RecordInitialState must be called first");
			}
			this._replay.events.Add(new CombatReplayEvent
			{
				eventType = CombatReplayEventType.PlayerChoice,
				playerId = new ulong?(player.NetId),
				choiceId = new uint?(choiceId),
				playerChoiceResult = new NetPlayerChoiceResult?(result)
			});
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x0021693C File Offset: 0x00214B3C
		private void RecordChecksum(NetChecksumData checksum, string context, NetFullCombatState fullCombatState)
		{
			if (!this.IsEnabled)
			{
				return;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return;
			}
			if (this._replay == null)
			{
				throw new InvalidOperationException("RecordInitialState must be called first");
			}
			this._replay.checksumData.Add(new ReplayChecksumData
			{
				checksumData = checksum,
				context = context,
				fullState = fullCombatState
			});
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x002169A4 File Offset: 0x00214BA4
		public void WriteReplay(string filePath, bool stopRecording)
		{
			if (!this.IsEnabled)
			{
				return;
			}
			if (this._replay == null)
			{
				throw new InvalidOperationException("RecordInitialState must be called first");
			}
			this._writer.Reset();
			this._writer.Write<CombatReplay>(this._replay.Anonymized());
			try
			{
				DirAccess.MakeDirRecursiveAbsolute(filePath.Substring(0, filePath.LastIndexOf('/')));
				using (FileAccessStream fileAccessStream = new FileAccessStream(filePath, 2L))
				{
					fileAccessStream.Write(this._writer.Buffer.AsSpan<byte>().Slice(0, this._writer.BytePosition));
				}
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Exception while writing replay: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			if (stopRecording)
			{
				this.StopRecording();
			}
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x00216A9C File Offset: 0x00214C9C
		public void StopRecording()
		{
			this._replay = null;
		}

		// Token: 0x04001B30 RID: 6960
		[Nullable(2)]
		private CombatReplay _replay;

		// Token: 0x04001B32 RID: 6962
		private readonly PacketWriter _writer = new PacketWriter();

		// Token: 0x04001B33 RID: 6963
		private readonly ActionQueueSet _actionQueueSet;

		// Token: 0x04001B34 RID: 6964
		private readonly ActionQueueSynchronizer _actionQueueSynchronizer;

		// Token: 0x04001B35 RID: 6965
		private readonly PlayerChoiceSynchronizer _playerChoiceSynchronizer;

		// Token: 0x04001B36 RID: 6966
		private readonly RewardsSetSynchronizer _rewardsSetSynchronizer;

		// Token: 0x04001B37 RID: 6967
		private readonly ChecksumTracker _checksumTracker;
	}
}
