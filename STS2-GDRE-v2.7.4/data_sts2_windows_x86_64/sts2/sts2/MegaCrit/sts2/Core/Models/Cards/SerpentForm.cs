using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AB9 RID: 2745
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SerpentForm : CardModel
	{
		// Token: 0x060076E1 RID: 30433 RVA: 0x00296915 File Offset: 0x00294B15
		public SerpentForm()
			: base(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002028 RID: 8232
		// (get) Token: 0x060076E2 RID: 30434 RVA: 0x00296922 File Offset: 0x00294B22
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SerpentFormPower>(4m));
			}
		}

		// Token: 0x060076E3 RID: 30435 RVA: 0x00296934 File Offset: 0x00294B34
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<SerpentFormPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SerpentFormPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060076E4 RID: 30436 RVA: 0x0029697F File Offset: 0x00294B7F
		protected override void OnUpgrade()
		{
			base.DynamicVars["SerpentFormPower"].UpgradeValueBy(2m);
		}
	}
}
