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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200075D RID: 1885
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Fortifier : PotionModel
	{
		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x06005DCF RID: 24015 RVA: 0x0025833B File Offset: 0x0025653B
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x06005DD0 RID: 24016 RVA: 0x0025833E File Offset: 0x0025653E
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x06005DD1 RID: 24017 RVA: 0x00258341 File Offset: 0x00256541
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x06005DD2 RID: 24018 RVA: 0x00258344 File Offset: 0x00256544
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005DD3 RID: 24019 RVA: 0x00258358 File Offset: 0x00256558
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await CreatureCmd.GainBlock(target, target.Block * 2, ValueProp.Unpowered, null, false);
		}
	}
}
