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
	// Token: 0x0200075C RID: 1884
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FocusPotion : PotionModel
	{
		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x06005DC8 RID: 24008 RVA: 0x002582A3 File Offset: 0x002564A3
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Common;
			}
		}

		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x06005DC9 RID: 24009 RVA: 0x002582A6 File Offset: 0x002564A6
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x06005DCA RID: 24010 RVA: 0x002582A9 File Offset: 0x002564A9
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x06005DCB RID: 24011 RVA: 0x002582AC File Offset: 0x002564AC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<FocusPower>(2m));
			}
		}

		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x06005DCC RID: 24012 RVA: 0x002582C0 File Offset: 0x002564C0
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>(null));
			}
		}

		// Token: 0x06005DCD RID: 24013 RVA: 0x002582E0 File Offset: 0x002564E0
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<FocusPower>(choiceContext, target, base.DynamicVars["FocusPower"].BaseValue, target, null, false);
		}
	}
}
