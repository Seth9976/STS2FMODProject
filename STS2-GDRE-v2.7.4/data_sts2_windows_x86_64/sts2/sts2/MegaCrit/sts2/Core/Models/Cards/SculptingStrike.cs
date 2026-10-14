using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AB1 RID: 2737
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SculptingStrike : CardModel
	{
		// Token: 0x060076B7 RID: 30391 RVA: 0x0029646C File Offset: 0x0029466C
		public SculptingStrike()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002017 RID: 8215
		// (get) Token: 0x060076B8 RID: 30392 RVA: 0x00296479 File Offset: 0x00294679
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
			}
		}

		// Token: 0x17002018 RID: 8216
		// (get) Token: 0x060076B9 RID: 30393 RVA: 0x00296486 File Offset: 0x00294686
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x17002019 RID: 8217
		// (get) Token: 0x060076BA RID: 30394 RVA: 0x00296495 File Offset: 0x00294695
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(9m, ValueProp.Move));
			}
		}

		// Token: 0x060076BB RID: 30395 RVA: 0x002964AC File Offset: 0x002946AC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, 1), (CardModel c) => !c.GetKeywordsWithSources(KeywordSources.Local).Contains(CardKeyword.Ethereal), this);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.ApplyKeyword(cardModel, new CardKeyword[] { CardKeyword.Ethereal });
			}
		}

		// Token: 0x060076BC RID: 30396 RVA: 0x002964FF File Offset: 0x002946FF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
