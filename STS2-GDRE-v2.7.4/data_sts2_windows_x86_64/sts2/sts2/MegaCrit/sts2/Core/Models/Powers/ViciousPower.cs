using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000728 RID: 1832
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ViciousPower : PowerModel
	{
		// Token: 0x1700145B RID: 5211
		// (get) Token: 0x06005C79 RID: 23673 RVA: 0x002565EB File Offset: 0x002547EB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700145C RID: 5212
		// (get) Token: 0x06005C7A RID: 23674 RVA: 0x002565EE File Offset: 0x002547EE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700145D RID: 5213
		// (get) Token: 0x06005C7B RID: 23675 RVA: 0x002565F4 File Offset: 0x002547F4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x06005C7C RID: 23676 RVA: 0x00256614 File Offset: 0x00254814
		public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (!(amount <= 0m))
			{
				if (applier == base.Owner)
				{
					if (power is VulnerablePower)
					{
						base.Flash();
						await CardPileCmd.Draw(choiceContext, base.Amount, base.Owner.Player, false);
					}
				}
			}
		}
	}
}
