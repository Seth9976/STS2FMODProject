using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006D6 RID: 1750
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReaperFormPower : PowerModel
	{
		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x06005A74 RID: 23156 RVA: 0x00252517 File Offset: 0x00250717
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x06005A75 RID: 23157 RVA: 0x0025251A File Offset: 0x0025071A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x06005A76 RID: 23158 RVA: 0x00252520 File Offset: 0x00250720
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x00252540 File Offset: 0x00250740
		public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, [Nullable(2)] Creature dealer, DamageResult result, ValueProp props, Creature target, [Nullable(2)] CardModel cardSource)
		{
			if (dealer != null)
			{
				if (dealer != base.Owner)
				{
					Player petOwner = dealer.PetOwner;
					if (((petOwner != null) ? petOwner.Creature : null) != base.Owner)
					{
						return;
					}
				}
				if (props.IsPoweredAttack())
				{
					if (result.TotalDamage > 0)
					{
						await PowerCmd.Apply<DoomPower>(choiceContext, target, result.TotalDamage * base.Amount, base.Owner, null, false);
					}
				}
			}
		}
	}
}
