using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000651 RID: 1617
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CrabRagePower : PowerModel
	{
		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x0600577F RID: 22399 RVA: 0x0024CF87 File Offset: 0x0024B187
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x06005780 RID: 22400 RVA: 0x0024CF8A File Offset: 0x0024B18A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x06005781 RID: 22401 RVA: 0x0024CF90 File Offset: 0x0024B190
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x06005782 RID: 22402 RVA: 0x0024CFC7 File Offset: 0x0024B1C7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<StrengthPower>(6m),
					new BlockVar(99m, ValueProp.Unpowered)
				});
			}
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x0024CFF4 File Offset: 0x0024B1F4
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (creature != base.Owner)
			{
				if (creature.Side == base.Owner.Side)
				{
					base.Flash();
					await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, base.DynamicVars.Strength.IntValue, base.Owner, null, false);
					await CreatureCmd.GainBlock(base.Owner, base.DynamicVars.Block, null, false);
					await PowerCmd.Remove(this);
				}
			}
		}
	}
}
