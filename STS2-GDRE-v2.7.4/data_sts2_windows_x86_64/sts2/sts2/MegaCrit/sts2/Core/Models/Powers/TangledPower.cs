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
using MegaCrit.Sts2.Core.Models.Afflictions;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000712 RID: 1810
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TangledPower : PowerModel
	{
		// Token: 0x17001404 RID: 5124
		// (get) Token: 0x06005BE3 RID: 23523 RVA: 0x002551F7 File Offset: 0x002533F7
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001405 RID: 5125
		// (get) Token: 0x06005BE4 RID: 23524 RVA: 0x002551FA File Offset: 0x002533FA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001406 RID: 5126
		// (get) Token: 0x06005BE5 RID: 23525 RVA: 0x002551FD File Offset: 0x002533FD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x06005BE6 RID: 23526 RVA: 0x0025520C File Offset: 0x0025340C
		[NullableContext(2)]
		[return: Nullable(1)]
		public override async Task AfterApplied(Creature applier, CardModel cardSource)
		{
			IEnumerable<CardModel> enumerable = base.Owner.Player.PlayerCombatState.AllCards.Where<CardModel>((CardModel c) => c.Type == CardType.Attack);
			foreach (CardModel cardModel in enumerable)
			{
				await CardCmd.Afflict<Entangled>(cardModel, 1m);
			}
			IEnumerator<CardModel> enumerator = null;
		}

		// Token: 0x06005BE7 RID: 23527 RVA: 0x00255250 File Offset: 0x00253450
		public override async Task AfterCardEnteredCombat(CardModel card)
		{
			if (card.Owner == base.Owner.Player)
			{
				if (card.Affliction == null)
				{
					if (card.Type == CardType.Attack)
					{
						await CardCmd.Afflict<Entangled>(card, 1m);
					}
				}
			}
		}

		// Token: 0x06005BE8 RID: 23528 RVA: 0x0025529C File Offset: 0x0025349C
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await PowerCmd.Remove(this);
			}
		}

		// Token: 0x06005BE9 RID: 23529 RVA: 0x002552E8 File Offset: 0x002534E8
		public override Task AfterRemoved(Creature oldOwner)
		{
			IEnumerable<CardModel> enumerable = oldOwner.Player.PlayerCombatState.AllCards.Where<CardModel>((CardModel c) => c.Affliction is Entangled);
			foreach (CardModel cardModel in enumerable)
			{
				CardCmd.ClearAffliction(cardModel);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005BEA RID: 23530 RVA: 0x0025536C File Offset: 0x0025356C
		public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			if (!(card.Affliction is Entangled) || card.Owner != base.Owner.Player)
			{
				modifiedCost = originalCost;
				return false;
			}
			modifiedCost = originalCost + base.Amount;
			return true;
		}
	}
}
