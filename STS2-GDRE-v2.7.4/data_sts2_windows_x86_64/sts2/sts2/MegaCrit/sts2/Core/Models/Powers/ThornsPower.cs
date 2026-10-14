using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200071E RID: 1822
	public sealed class ThornsPower : PowerModel
	{
		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x06005C43 RID: 23619 RVA: 0x00255F87 File Offset: 0x00254187
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x06005C44 RID: 23620 RVA: 0x00255F8A File Offset: 0x0025418A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005C45 RID: 23621 RVA: 0x00255F90 File Offset: 0x00254190
		[NullableContext(1)]
		public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target == base.Owner)
			{
				if (dealer != null)
				{
					if (props.IsPoweredAttack() || cardSource is Omnislice)
					{
						base.Flash();
						await CreatureCmd.Damage(choiceContext, dealer, base.Amount, ValueProp.Unpowered | ValueProp.SkipHurtAnim, base.Owner, null, null);
					}
				}
			}
		}
	}
}
