using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006FD RID: 1789
	public sealed class SpeedsterPower : PowerModel
	{
		// Token: 0x170013CD RID: 5069
		// (get) Token: 0x06005B68 RID: 23400 RVA: 0x002542C6 File Offset: 0x002524C6
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013CE RID: 5070
		// (get) Token: 0x06005B69 RID: 23401 RVA: 0x002542C9 File Offset: 0x002524C9
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005B6A RID: 23402 RVA: 0x002542CC File Offset: 0x002524CC
		[NullableContext(1)]
		public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (!fromHandDraw)
			{
				if (card.Owner.Creature == base.Owner)
				{
					if (card.Owner.Creature.CombatState.CurrentSide == card.Owner.Creature.Side)
					{
						VfxCmd.PlayOnCreatureCenters(base.CombatState.HittableEnemies, "vfx/vfx_attack_slash");
						SfxCmd.Play("slash_attack.mp3", 1f);
						await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies, base.Amount, ValueProp.Unpowered, base.Owner);
					}
				}
			}
		}
	}
}
