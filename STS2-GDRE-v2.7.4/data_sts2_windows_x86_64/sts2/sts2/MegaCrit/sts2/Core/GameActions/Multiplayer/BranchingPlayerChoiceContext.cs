using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	/// <summary>
	/// A player choice context used when there is an existing player choice context, but we might begin a player choice for
	/// a different player.
	///
	/// If there is a GameAction in progress, and we're executing within it, then a choice for the non-owning player will
	/// block the OWNING player's queue by default. For example, if I throw a Skill Potion at another player, then it will
	/// block MY actions, and not the other player's. This is undesirable for several reasons:
	///  - I can't play cards while you're making a choice.
	///  - I might open a new choice screen for you while you're already looking at one.
	///
	/// The BranchingPlayerChoiceContext exists to solve these problems. If a new choice begins, then:
	///  - If the choice is for the owning player, we continue with the existing choice context.
	///  - If the choice is for someone else, we create a new GameAction (via the HookPlayerChoiceContext) and use that new
	///    GameAction for execution of the player choice.
	///
	/// It is up to the caller to specify what the boundaries of execution for the new GameAction are, using
	/// AssignTaskAndWaitForPauseOrCompletion. For example, take Tutor:
	///  - We want everything after the player choice within OnPlay to execute on the CHOOSING player's queue...
	///  - But we want Hook.AfterCardPlayed to execute without waiting for that choice on the OWNING player's queue.
	/// Imagine if the owning player has Unceasing Top; we want that draw to take place before the choosing player makes
	/// their choice, so that the owning player is not awkwardly waiting for the choosing player. This is why the
	/// BranchingPlayerChoiceContext is passed the Task for CardModel.OnPlay, and not CardModel.OnPlayWrapper.
	///
	/// For a contrasting example, see Huddle Up. If its draws trigger an autoplay:
	///  - We want that specific set of draws to delay on the autoplay on the CHOOSING player's queue...
	///  - But we do not want that set of draws to delay other player's draws.
	///  - And if no draw triggers an autoplay, we want the entire group of draws to execute in one GameAction.
	/// It uses BranchingPlayerChoiceContext directly within its OnPlay to achieve this effect. Each CardPileCmd.Draw call
	/// creates its own BranchingPlayerChoiceContext, which surrounds only the Draw Task and not the entire CardModel.OnPlay.
	///
	/// With all that said, this API is very experimental and needs proving out in real testing. I think it's likely I have
	/// not thought through every single scenario.
	/// </summary>
	// Token: 0x02000C38 RID: 3128
	[NullableContext(1)]
	[Nullable(0)]
	public class BranchingPlayerChoiceContext : PlayerChoiceContext
	{
		// Token: 0x140000AA RID: 170
		// (add) Token: 0x0600811C RID: 33052 RVA: 0x002BD054 File Offset: 0x002BB254
		// (remove) Token: 0x0600811D RID: 33053 RVA: 0x002BD08C File Offset: 0x002BB28C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<HookPlayerChoiceContext> AfterBranched;

		// Token: 0x17002345 RID: 9029
		// (get) Token: 0x0600811E RID: 33054 RVA: 0x002BD0C1 File Offset: 0x002BB2C1
		public override ulong? OwnerId
		{
			get
			{
				return this._originalContext.OwnerId;
			}
		}

		// Token: 0x0600811F RID: 33055 RVA: 0x002BD0CE File Offset: 0x002BB2CE
		public BranchingPlayerChoiceContext(ulong localPlayerId, GameActionType gameActionType, PlayerChoiceContext existing)
		{
			this._originalContext = existing;
			this._gameActionType = gameActionType;
			this._localPlayerId = localPlayerId;
		}

		// Token: 0x06008120 RID: 33056 RVA: 0x002BD0F8 File Offset: 0x002BB2F8
		public override async Task SignalPlayerChoiceBegun(Player chooser, PlayerChoiceOptions options)
		{
			PlayerChoiceContext playerChoiceContext;
			if (this._originalContext.OwnerId != null)
			{
				ulong netId = chooser.NetId;
				ulong? ownerId = this._originalContext.OwnerId;
				if (!((netId == ownerId.GetValueOrDefault()) & (ownerId != null)))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					if (this._createdContext != null)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(127, 4);
						defaultInterpolatedStringHandler.AppendFormatted("BranchingPlayerChoiceContext");
						defaultInterpolatedStringHandler.AppendLiteral(" has been used twice! ");
						defaultInterpolatedStringHandler.AppendLiteral("We switched owners from ");
						defaultInterpolatedStringHandler.AppendFormatted<ulong?>(this._originalContext.OwnerId);
						defaultInterpolatedStringHandler.AppendLiteral(" to ");
						defaultInterpolatedStringHandler.AppendFormatted<ulong?>(this._createdContext.OwnerId);
						defaultInterpolatedStringHandler.AppendLiteral(", ");
						defaultInterpolatedStringHandler.AppendLiteral("and now we are trying to switch to ");
						defaultInterpolatedStringHandler.AppendFormatted<ulong>(chooser.NetId);
						defaultInterpolatedStringHandler.AppendLiteral(". Re-using the existing created context.");
						Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						playerChoiceContext = this._createdContext;
						goto IL_0247;
					}
					LogLevel logLevel = LogLevel.Debug;
					LogType logType = LogType.GameSync;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Branching context began choice for ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(chooser.NetId);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendLiteral("who is not the owner (");
					defaultInterpolatedStringHandler.AppendFormatted<ulong?>(this._originalContext.OwnerId);
					defaultInterpolatedStringHandler.AppendLiteral("). Using new ");
					defaultInterpolatedStringHandler.AppendFormatted("HookPlayerChoiceContext");
					Log.LogMessage(logLevel, logType, defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this._createdContext = new HookPlayerChoiceContext(chooser, this._localPlayerId, this._gameActionType);
					Action<HookPlayerChoiceContext> afterBranched = this.AfterBranched;
					if (afterBranched != null)
					{
						afterBranched(this._createdContext);
					}
					this._pausedCompletionSource.SetResult();
					playerChoiceContext = this._createdContext;
					goto IL_0247;
				}
			}
			if (this._originalContext.OwnerId == null)
			{
				LogLevel logLevel2 = LogLevel.Debug;
				LogType logType2 = LogType.GameSync;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Branching context began choice for ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(chooser.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendLiteral("and there is no owner. Using existing ");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerChoiceContext>(this._originalContext);
				Log.LogMessage(logLevel2, logType2, defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			playerChoiceContext = this._originalContext;
			IL_0247:
			await playerChoiceContext.SignalPlayerChoiceBegun(chooser, options);
		}

		// Token: 0x06008121 RID: 33057 RVA: 0x002BD14C File Offset: 0x002BB34C
		public async Task AssignTaskAndWaitForPauseOrCompletion(Task task)
		{
			await TaskHelper.WhenAny(new Task[]
			{
				task,
				this._pausedCompletionSource.Task
			});
			if (this._pausedCompletionSource.Task.IsCompleted)
			{
				await this._createdContext.AssignTaskAndWaitForPauseOrCompletion(task);
			}
		}

		// Token: 0x06008122 RID: 33058 RVA: 0x002BD197 File Offset: 0x002BB397
		public override Task SignalPlayerChoiceEnded()
		{
			return (this._createdContext ?? this._originalContext).SignalPlayerChoiceEnded();
		}

		// Token: 0x04002B37 RID: 11063
		private PlayerChoiceContext _originalContext;

		// Token: 0x04002B38 RID: 11064
		private readonly GameActionType _gameActionType;

		// Token: 0x04002B39 RID: 11065
		private readonly ulong _localPlayerId;

		// Token: 0x04002B3A RID: 11066
		[Nullable(2)]
		private HookPlayerChoiceContext _createdContext;

		// Token: 0x04002B3B RID: 11067
		private TaskCompletionSource _pausedCompletionSource = new TaskCompletionSource();
	}
}
