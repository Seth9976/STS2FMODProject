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
	// Token: 0x020009B6 RID: 2486
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Fade : CardModel
	{
		// Token: 0x06007169 RID: 29033 RVA: 0x0028B2FE File Offset: 0x002894FE
		public Fade()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001DE1 RID: 7649
		// (get) Token: 0x0600716A RID: 29034 RVA: 0x0028B30B File Offset: 0x0028950B
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001DE2 RID: 7650
		// (get) Token: 0x0600716B RID: 29035 RVA: 0x0028B30E File Offset: 0x0028950E
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);
			}
		}

		// Token: 0x17001DE3 RID: 7651
		// (get) Token: 0x0600716C RID: 29036 RVA: 0x0028B316 File Offset: 0x00289516
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DexterityPower>(6m));
			}
		}

		// Token: 0x17001DE4 RID: 7652
		// (get) Token: 0x0600716D RID: 29037 RVA: 0x0028B328 File Offset: 0x00289528
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>(null));
			}
		}

		// Token: 0x0600716E RID: 29038 RVA: 0x0028B348 File Offset: 0x00289548
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await PowerCmd.Apply<FadePower>(choiceContext, cardPlay.Target, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600716F RID: 29039 RVA: 0x0028B39B File Offset: 0x0028959B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Dexterity.UpgradeValueBy(3m);
		}
	}
}
