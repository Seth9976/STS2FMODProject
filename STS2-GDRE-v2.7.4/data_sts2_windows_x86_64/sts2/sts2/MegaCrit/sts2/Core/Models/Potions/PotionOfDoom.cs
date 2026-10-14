using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200076F RID: 1903
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PotionOfDoom : PotionModel
	{
		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x06005E46 RID: 24134 RVA: 0x00258E43 File Offset: 0x00257043
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x06005E47 RID: 24135 RVA: 0x00258E46 File Offset: 0x00257046
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x06005E48 RID: 24136 RVA: 0x00258E49 File Offset: 0x00257049
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyEnemy;
			}
		}

		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x06005E49 RID: 24137 RVA: 0x00258E4C File Offset: 0x0025704C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DoomPower>(33m));
			}
		}

		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x06005E4A RID: 24138 RVA: 0x00258E60 File Offset: 0x00257060
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x06005E4B RID: 24139 RVA: 0x00258E80 File Offset: 0x00257080
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.PlaySplashVfx(target, Colors.Purple);
			}
			await PowerCmd.Apply<DoomPower>(choiceContext, target, base.DynamicVars.Doom.BaseValue, base.Owner.Creature, null, false);
		}
	}
}
