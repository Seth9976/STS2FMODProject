using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200096D RID: 2413
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Cruelty : CardModel
	{
		// Token: 0x06006FDF RID: 28639 RVA: 0x00288488 File Offset: 0x00286688
		public Cruelty()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001D2C RID: 7468
		// (get) Token: 0x06006FE0 RID: 28640 RVA: 0x00288495 File Offset: 0x00286695
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<CrueltyPower>(25m));
			}
		}

		// Token: 0x17001D2D RID: 7469
		// (get) Token: 0x06006FE1 RID: 28641 RVA: 0x002884A8 File Offset: 0x002866A8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x06006FE2 RID: 28642 RVA: 0x002884C8 File Offset: 0x002866C8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<CrueltyPower>(choiceContext, base.Owner.Creature, base.DynamicVars["CrueltyPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006FE3 RID: 28643 RVA: 0x00288513 File Offset: 0x00286713
		protected override void OnUpgrade()
		{
			base.DynamicVars["CrueltyPower"].UpgradeValueBy(25m);
		}
	}
}
