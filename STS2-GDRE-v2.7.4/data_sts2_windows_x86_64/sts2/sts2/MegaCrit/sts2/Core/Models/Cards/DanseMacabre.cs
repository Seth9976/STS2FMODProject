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
	// Token: 0x02000972 RID: 2418
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DanseMacabre : CardModel
	{
		// Token: 0x06006FF8 RID: 28664 RVA: 0x00288810 File Offset: 0x00286A10
		public DanseMacabre()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001D35 RID: 7477
		// (get) Token: 0x06006FF9 RID: 28665 RVA: 0x0028881D File Offset: 0x00286A1D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<DanseMacabrePower>(4m),
					new EnergyVar(2)
				});
			}
		}

		// Token: 0x17001D36 RID: 7478
		// (get) Token: 0x06006FFA RID: 28666 RVA: 0x00288841 File Offset: 0x00286A41
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06006FFB RID: 28667 RVA: 0x00288850 File Offset: 0x00286A50
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<DanseMacabrePower>(choiceContext, base.Owner.Creature, base.DynamicVars["DanseMacabrePower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006FFC RID: 28668 RVA: 0x0028889B File Offset: 0x00286A9B
		protected override void OnUpgrade()
		{
			base.DynamicVars["DanseMacabrePower"].UpgradeValueBy(2m);
		}
	}
}
