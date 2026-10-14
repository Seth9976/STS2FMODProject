using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200063A RID: 1594
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BorrowedTimePower : PowerModel
	{
		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x060056F9 RID: 22265 RVA: 0x0024C05F File Offset: 0x0024A25F
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x060056FA RID: 22266 RVA: 0x0024C062 File Offset: 0x0024A262
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x060056FB RID: 22267 RVA: 0x0024C065 File Offset: 0x0024A265
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x060056FC RID: 22268 RVA: 0x0024C072 File Offset: 0x0024A272
		public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost;
			if (card.Owner.Creature != base.Owner)
			{
				return false;
			}
			modifiedCost = originalCost + base.Amount;
			return true;
		}

		// Token: 0x060056FD RID: 22269 RVA: 0x0024C0A8 File Offset: 0x0024A2A8
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
