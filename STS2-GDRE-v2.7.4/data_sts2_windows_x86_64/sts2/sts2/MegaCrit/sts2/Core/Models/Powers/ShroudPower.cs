using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006ED RID: 1773
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShroudPower : PowerModel
	{
		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x06005B07 RID: 23303 RVA: 0x0025387A File Offset: 0x00251A7A
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013A1 RID: 5025
		// (get) Token: 0x06005B08 RID: 23304 RVA: 0x0025387D File Offset: 0x00251A7D
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013A2 RID: 5026
		// (get) Token: 0x06005B09 RID: 23305 RVA: 0x00253880 File Offset: 0x00251A80
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x06005B0A RID: 23306 RVA: 0x002538A0 File Offset: 0x00251AA0
		public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (applier == base.Owner)
			{
				if (power is DoomPower)
				{
					await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null, false);
				}
			}
		}
	}
}
