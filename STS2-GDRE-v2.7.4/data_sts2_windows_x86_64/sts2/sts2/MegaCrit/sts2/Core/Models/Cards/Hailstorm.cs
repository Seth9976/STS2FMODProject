using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009EF RID: 2543
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Hailstorm : CardModel
	{
		// Token: 0x060072AA RID: 29354 RVA: 0x0028DC7E File Offset: 0x0028BE7E
		public Hailstorm()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E6D RID: 7789
		// (get) Token: 0x060072AB RID: 29355 RVA: 0x0028DC8B File Offset: 0x0028BE8B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromOrb<FrostOrb>());
			}
		}

		// Token: 0x17001E6E RID: 7790
		// (get) Token: 0x060072AC RID: 29356 RVA: 0x0028DC97 File Offset: 0x0028BE97
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<HailstormPower>(6m));
			}
		}

		// Token: 0x060072AD RID: 29357 RVA: 0x0028DCAC File Offset: 0x0028BEAC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<HailstormPower>(choiceContext, base.Owner.Creature, base.DynamicVars["HailstormPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060072AE RID: 29358 RVA: 0x0028DCF7 File Offset: 0x0028BEF7
		protected override void OnUpgrade()
		{
			base.DynamicVars["HailstormPower"].UpgradeValueBy(2m);
		}
	}
}
