using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005F7 RID: 1527
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheAbacus : RelicModel
	{
		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x06005562 RID: 21858 RVA: 0x0024877A File Offset: 0x0024697A
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06005563 RID: 21859 RVA: 0x0024877D File Offset: 0x0024697D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(6m, ValueProp.Unpowered));
			}
		}

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06005564 RID: 21860 RVA: 0x00248790 File Offset: 0x00246990
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005565 RID: 21861 RVA: 0x002487A4 File Offset: 0x002469A4
		public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
		{
			if (shuffler == base.Owner)
			{
				base.Flash();
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
			}
		}
	}
}
