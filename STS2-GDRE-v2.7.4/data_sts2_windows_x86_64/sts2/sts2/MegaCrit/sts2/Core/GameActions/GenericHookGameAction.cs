using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C22 RID: 3106
	[NullableContext(1)]
	[Nullable(0)]
	public class GenericHookGameAction : GameAction
	{
		// Token: 0x1700231B RID: 8987
		// (get) Token: 0x0600804E RID: 32846 RVA: 0x002B8626 File Offset: 0x002B6826
		public override bool RecordableToReplay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700231C RID: 8988
		// (get) Token: 0x0600804F RID: 32847 RVA: 0x002B8629 File Offset: 0x002B6829
		// (set) Token: 0x06008050 RID: 32848 RVA: 0x002B8631 File Offset: 0x002B6831
		[Nullable(2)]
		public HookPlayerChoiceContext ChoiceContext
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x1700231D RID: 8989
		// (get) Token: 0x06008051 RID: 32849 RVA: 0x002B863A File Offset: 0x002B683A
		public override ulong OwnerId { get; }

		// Token: 0x1700231E RID: 8990
		// (get) Token: 0x06008052 RID: 32850 RVA: 0x002B8642 File Offset: 0x002B6842
		public override GameActionType ActionType
		{
			get
			{
				return this._gameActionType;
			}
		}

		// Token: 0x1700231F RID: 8991
		// (get) Token: 0x06008053 RID: 32851 RVA: 0x002B864A File Offset: 0x002B684A
		public uint HookId { get; }

		// Token: 0x17002320 RID: 8992
		// (get) Token: 0x06008054 RID: 32852 RVA: 0x002B8652 File Offset: 0x002B6852
		public Task ExecutionStartedTask
		{
			get
			{
				return this._executionStartedSource.Task;
			}
		}

		// Token: 0x06008055 RID: 32853 RVA: 0x002B8660 File Offset: 0x002B6860
		public GenericHookGameAction(uint hookId, ulong ownerId, GameActionType gameActionType)
		{
			this._gameActionType = gameActionType;
			this.HookId = hookId;
			this.OwnerId = ownerId;
			if (this._gameActionType != GameActionType.Combat && this._gameActionType != GameActionType.CombatPlayPhaseOnly)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unexpected GameActionType ");
				defaultInterpolatedStringHandler.AppendFormatted<GameActionType>(this._gameActionType);
				defaultInterpolatedStringHandler.AppendLiteral(" received for GenericHookGameAction!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06008056 RID: 32854 RVA: 0x002B86EC File Offset: 0x002B68EC
		public void SetChoiceContext(HookPlayerChoiceContext choiceContext)
		{
			Player owner = choiceContext.Owner;
			ulong? num = ((owner != null) ? new ulong?(owner.NetId) : null);
			ulong ownerId = this.OwnerId;
			if (!((num.GetValueOrDefault() == ownerId) & (num != null)))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Assigned choice context with owner ");
				Player owner2 = choiceContext.Owner;
				defaultInterpolatedStringHandler.AppendFormatted<ulong?>((owner2 != null) ? new ulong?(owner2.NetId) : null);
				defaultInterpolatedStringHandler.AppendLiteral(" to GenericHookGameAction with owner ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.OwnerId);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.ChoiceContext = choiceContext;
			this._choiceContextSetSource.SetResult();
		}

		// Token: 0x06008057 RID: 32855 RVA: 0x002B87B4 File Offset: 0x002B69B4
		protected override async Task ExecuteAction()
		{
			await this._choiceContextSetSource.Task;
			this._executionStartedSource.SetResult();
			await this.ChoiceContext.Task;
			if (this.debugArtificialDelayAfterTask != null)
			{
				await this.debugArtificialDelayAfterTask;
			}
		}

		// Token: 0x06008058 RID: 32856 RVA: 0x002B87F7 File Offset: 0x002B69F7
		public override INetAction ToNetAction()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06008059 RID: 32857 RVA: 0x002B8800 File Offset: 0x002B6A00
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 5);
			defaultInterpolatedStringHandler.AppendFormatted("GenericHookGameAction");
			defaultInterpolatedStringHandler.AppendLiteral(" id ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.HookId);
			defaultInterpolatedStringHandler.AppendLiteral(" owner ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.OwnerId);
			defaultInterpolatedStringHandler.AppendLiteral(" source ");
			HookPlayerChoiceContext choiceContext = this.ChoiceContext;
			defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>((choiceContext != null) ? choiceContext.Source : null);
			defaultInterpolatedStringHandler.AppendLiteral(" last involved ");
			HookPlayerChoiceContext choiceContext2 = this.ChoiceContext;
			defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>((choiceContext2 != null) ? choiceContext2.LastInvolvedModel : null);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002AEE RID: 10990
		private readonly TaskCompletionSource _executionStartedSource = new TaskCompletionSource();

		// Token: 0x04002AEF RID: 10991
		private readonly TaskCompletionSource _choiceContextSetSource = new TaskCompletionSource();

		// Token: 0x04002AF0 RID: 10992
		private readonly GameActionType _gameActionType;

		/// <summary>
		/// See CombatManagerMultiplayerTest.
		/// </summary>
		// Token: 0x04002AF4 RID: 10996
		[Nullable(2)]
		public Task debugArtificialDelayAfterTask;
	}
}
