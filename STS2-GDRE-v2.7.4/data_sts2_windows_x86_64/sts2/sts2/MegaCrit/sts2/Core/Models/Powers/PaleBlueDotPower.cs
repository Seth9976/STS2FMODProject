using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006C3 RID: 1731
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaleBlueDotPower : PowerModel
	{
		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x060059FC RID: 23036 RVA: 0x00251667 File Offset: 0x0024F867
		public override int DisplayAmount
		{
			get
			{
				return Math.Max(0, 5 - this.AttacksPlayedThisTurn);
			}
		}

		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x060059FD RID: 23037 RVA: 0x00251677 File Offset: 0x0024F877
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x060059FE RID: 23038 RVA: 0x0025167A File Offset: 0x0024F87A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x060059FF RID: 23039 RVA: 0x0025167D File Offset: 0x0024F87D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("CardPlay", 5m));
			}
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x00251694 File Offset: 0x0024F894
		protected override object InitInternalData()
		{
			return new PaleBlueDotPower.Data();
		}

		// Token: 0x06005A01 RID: 23041 RVA: 0x0025169C File Offset: 0x0024F89C
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner.Player)
			{
				PaleBlueDotPower.Data internalData = base.GetInternalData<PaleBlueDotPower.Data>();
				if (!internalData.alreadyActivatedThisTurn)
				{
					base.InvokeDisplayAmountChanged();
					if (this.AttacksPlayedThisTurn >= 5)
					{
						internalData.alreadyActivatedThisTurn = true;
						await Cmd.Wait(0.5f, false);
						await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, base.Owner, base.Amount, base.Owner, null, false);
					}
				}
			}
		}

		// Token: 0x06005A02 RID: 23042 RVA: 0x002516F0 File Offset: 0x0024F8F0
		public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			PaleBlueDotPower.Data internalData = base.GetInternalData<PaleBlueDotPower.Data>();
			internalData.alreadyActivatedThisTurn = false;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x06005A03 RID: 23043 RVA: 0x0025172A File Offset: 0x0024F92A
		private int AttacksPlayedThisTurn
		{
			get
			{
				return CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((CardPlayFinishedEntry c) => c.HappenedThisTurn(base.Owner.CombatState) && c.CardPlay.Player == base.Owner.Player);
			}
		}

		// Token: 0x0400250A RID: 9482
		public const string cardPlayThresholdKey = "CardPlay";

		// Token: 0x0400250B RID: 9483
		public const int cardPlayThresholdValue = 5;

		// Token: 0x02001C20 RID: 7200
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x0400706B RID: 28779
			public bool alreadyActivatedThisTurn;
		}
	}
}
