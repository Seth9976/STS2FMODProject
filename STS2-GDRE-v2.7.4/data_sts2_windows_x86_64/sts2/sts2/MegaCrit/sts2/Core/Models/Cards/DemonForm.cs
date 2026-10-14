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
	// Token: 0x0200098C RID: 2444
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DemonForm : CardModel
	{
		// Token: 0x06007085 RID: 28805 RVA: 0x0028986D File Offset: 0x00287A6D
		public DemonForm()
			: base(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001D79 RID: 7545
		// (get) Token: 0x06007086 RID: 28806 RVA: 0x0028987C File Offset: 0x00287A7C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x17001D7A RID: 7546
		// (get) Token: 0x06007087 RID: 28807 RVA: 0x0028989C File Offset: 0x00287A9C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(3m));
			}
		}

		// Token: 0x06007088 RID: 28808 RVA: 0x002898B0 File Offset: 0x00287AB0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<DemonFormPower>(choiceContext, base.Owner.Creature, base.DynamicVars["StrengthPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007089 RID: 28809 RVA: 0x002898FB File Offset: 0x00287AFB
		protected override void OnUpgrade()
		{
			base.DynamicVars["StrengthPower"].UpgradeValueBy(1m);
		}
	}
}
