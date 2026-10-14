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
	// Token: 0x02000A05 RID: 2565
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Hotfix : CardModel
	{
		// Token: 0x0600731B RID: 29467 RVA: 0x0028EB7A File Offset: 0x0028CD7A
		public Hotfix()
			: base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001E9B RID: 7835
		// (get) Token: 0x0600731C RID: 29468 RVA: 0x0028EB88 File Offset: 0x0028CD88
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>(null));
			}
		}

		// Token: 0x17001E9C RID: 7836
		// (get) Token: 0x0600731D RID: 29469 RVA: 0x0028EBA8 File Offset: 0x0028CDA8
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001E9D RID: 7837
		// (get) Token: 0x0600731E RID: 29470 RVA: 0x0028EBB0 File Offset: 0x0028CDB0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<FocusPower>(2m));
			}
		}

		// Token: 0x0600731F RID: 29471 RVA: 0x0028EBC4 File Offset: 0x0028CDC4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<HotfixPower>(choiceContext, base.Owner.Creature, base.DynamicVars["FocusPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007320 RID: 29472 RVA: 0x0028EC0F File Offset: 0x0028CE0F
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
