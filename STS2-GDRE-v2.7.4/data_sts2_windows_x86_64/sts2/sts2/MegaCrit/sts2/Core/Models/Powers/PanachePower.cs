using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006C4 RID: 1732
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PanachePower : PowerModel
	{
		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x06005A06 RID: 23046 RVA: 0x00251783 File Offset: 0x0024F983
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x06005A07 RID: 23047 RVA: 0x00251786 File Offset: 0x0024F986
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x06005A08 RID: 23048 RVA: 0x00251789 File Offset: 0x0024F989
		public override int DisplayAmount
		{
			get
			{
				return base.DynamicVars["CardsLeft"].IntValue;
			}
		}

		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x06005A09 RID: 23049 RVA: 0x002517A0 File Offset: 0x0024F9A0
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x06005A0A RID: 23050 RVA: 0x002517A3 File Offset: 0x0024F9A3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("CardsLeft", 5m));
			}
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x002517BA File Offset: 0x0024F9BA
		protected override object InitInternalData()
		{
			return new PanachePower.Data();
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x002517C4 File Offset: 0x0024F9C4
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner.Player)
			{
				PanachePower.Data data = base.GetInternalData<PanachePower.Data>();
				if (data.alreadyApplied)
				{
					DynamicVar dynamicVar = base.DynamicVars["CardsLeft"];
					decimal baseValue = dynamicVar.BaseValue;
					dynamicVar.BaseValue = baseValue - 1m;
					base.InvokeDisplayAmountChanged();
					if (base.DynamicVars["CardsLeft"].IntValue <= 0)
					{
						await Cmd.Wait(0.5f, false);
						await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies, base.Amount, ValueProp.Unpowered, base.Owner);
						base.DynamicVars["CardsLeft"].BaseValue = 5m;
						base.InvokeDisplayAmountChanged();
					}
				}
				data.alreadyApplied = true;
			}
		}

		// Token: 0x06005A0D RID: 23053 RVA: 0x00251817 File Offset: 0x0024FA17
		public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			base.DynamicVars["CardsLeft"].BaseValue = 5m;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x0400250C RID: 9484
		private const int _baseCardsLeft = 5;

		// Token: 0x0400250D RID: 9485
		private const string _cardsLeftKey = "CardsLeft";

		// Token: 0x02001C22 RID: 7202
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// We track this so we don't count the Panache card towards itself.
			/// </summary>
			// Token: 0x04007073 RID: 28787
			public bool alreadyApplied;
		}
	}
}
