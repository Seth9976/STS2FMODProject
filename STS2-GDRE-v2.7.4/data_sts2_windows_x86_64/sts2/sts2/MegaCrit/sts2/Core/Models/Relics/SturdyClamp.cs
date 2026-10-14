using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005F1 RID: 1521
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SturdyClamp : RelicModel
	{
		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x0600553D RID: 21821 RVA: 0x002483C3 File Offset: 0x002465C3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x0600553E RID: 21822 RVA: 0x002483C6 File Offset: 0x002465C6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(10m, ValueProp.Unpowered));
			}
		}

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x0600553F RID: 21823 RVA: 0x002483DA File Offset: 0x002465DA
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005540 RID: 21824 RVA: 0x002483EC File Offset: 0x002465EC
		public override bool ShouldClearBlock(Creature creature)
		{
			return creature != base.Owner.Creature;
		}

		// Token: 0x06005541 RID: 21825 RVA: 0x00248400 File Offset: 0x00246600
		public override async Task AfterPreventingBlockClear(AbstractModel preventer, Creature creature)
		{
			if (this == preventer)
			{
				if (creature == base.Owner.Creature)
				{
					int block = creature.Block;
					if (block != 0)
					{
						if (block > 10)
						{
							await CreatureCmd.LoseBlock(new BlockingPlayerChoiceContext(), creature, block - 10, null);
						}
						base.Flash();
					}
				}
			}
		}

		// Token: 0x040024C4 RID: 9412
		private const int _maxRetainedBlock = 10;
	}
}
