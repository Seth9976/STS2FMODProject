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
	// Token: 0x02000774 RID: 1908
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RadiantTincture : PotionModel
	{
		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x06005E65 RID: 24165 RVA: 0x002590AB File Offset: 0x002572AB
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x06005E66 RID: 24166 RVA: 0x002590AE File Offset: 0x002572AE
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x06005E67 RID: 24167 RVA: 0x002590B1 File Offset: 0x002572B1
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x06005E68 RID: 24168 RVA: 0x002590B4 File Offset: 0x002572B4
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x06005E69 RID: 24169 RVA: 0x002590C1 File Offset: 0x002572C1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(1),
					new PowerVar<RadiancePower>(3m)
				});
			}
		}

		// Token: 0x06005E6A RID: 24170 RVA: 0x002590E8 File Offset: 0x002572E8
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, target.Player);
			await PowerCmd.Apply<RadiancePower>(choiceContext, target, base.DynamicVars["RadiancePower"].BaseValue, base.Owner.Creature, null, false);
		}
	}
}
