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

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000767 RID: 1895
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LiquidBronze : PotionModel
	{
		// Token: 0x1700154F RID: 5455
		// (get) Token: 0x06005E13 RID: 24083 RVA: 0x002589DB File Offset: 0x00256BDB
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x06005E14 RID: 24084 RVA: 0x002589DE File Offset: 0x00256BDE
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x06005E15 RID: 24085 RVA: 0x002589E1 File Offset: 0x00256BE1
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x06005E16 RID: 24086 RVA: 0x002589E4 File Offset: 0x00256BE4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<ThornsPower>(3m));
			}
		}

		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x06005E17 RID: 24087 RVA: 0x002589F8 File Offset: 0x00256BF8
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<ThornsPower>(null));
			}
		}

		// Token: 0x06005E18 RID: 24088 RVA: 0x00258A18 File Offset: 0x00256C18
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<ThornsPower>(choiceContext, target, base.DynamicVars["ThornsPower"].BaseValue, base.Owner.Creature, null, false);
		}
	}
}
