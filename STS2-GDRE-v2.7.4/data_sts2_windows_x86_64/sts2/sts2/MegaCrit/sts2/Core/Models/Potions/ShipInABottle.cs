using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000777 RID: 1911
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShipInABottle : PotionModel
	{
		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x06005E7B RID: 24187 RVA: 0x0025926F File Offset: 0x0025746F
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x06005E7C RID: 24188 RVA: 0x00259272 File Offset: 0x00257472
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x06005E7D RID: 24189 RVA: 0x00259275 File Offset: 0x00257475
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x06005E7E RID: 24190 RVA: 0x00259278 File Offset: 0x00257478
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(10m, ValueProp.Unpowered));
			}
		}

		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x06005E7F RID: 24191 RVA: 0x0025928C File Offset: 0x0025748C
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005E80 RID: 24192 RVA: 0x002592A0 File Offset: 0x002574A0
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await CreatureCmd.GainBlock(target, base.DynamicVars.Block, null, false);
			await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, target, base.DynamicVars.Block.IntValue, base.Owner.Creature, null, false);
		}
	}
}
