using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006F7 RID: 1783
	public sealed class SmokestackPower : PowerModel
	{
		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x06005B4D RID: 23373 RVA: 0x002540A1 File Offset: 0x002522A1
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x06005B4E RID: 23374 RVA: 0x002540A4 File Offset: 0x002522A4
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005B4F RID: 23375 RVA: 0x002540A8 File Offset: 0x002522A8
		[NullableContext(1)]
		public override async Task AfterCardGeneratedForCombat(CardModel card, [Nullable(2)] Player creator)
		{
			if (card.Type == CardType.Status)
			{
				if (creator != null && creator.Creature == base.Owner)
				{
					base.Flash();
					await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.Amount, ValueProp.Unpowered, base.Owner);
				}
			}
		}
	}
}
