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
	// Token: 0x020009CF RID: 2511
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Footwork : CardModel
	{
		// Token: 0x060071F7 RID: 29175 RVA: 0x0028C67D File Offset: 0x0028A87D
		public Footwork()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E1F RID: 7711
		// (get) Token: 0x060071F8 RID: 29176 RVA: 0x0028C68A File Offset: 0x0028A88A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DexterityPower>(2m));
			}
		}

		// Token: 0x17001E20 RID: 7712
		// (get) Token: 0x060071F9 RID: 29177 RVA: 0x0028C69C File Offset: 0x0028A89C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>(null));
			}
		}

		// Token: 0x060071FA RID: 29178 RVA: 0x0028C6BC File Offset: 0x0028A8BC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060071FB RID: 29179 RVA: 0x0028C707 File Offset: 0x0028A907
		protected override void OnUpgrade()
		{
			base.DynamicVars.Dexterity.UpgradeValueBy(1m);
		}
	}
}
