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
using MegaCrit.Sts2.Core.Models.Characters;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000952 RID: 2386
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Cleanse : CardModel
	{
		// Token: 0x06006F52 RID: 28498 RVA: 0x002872D1 File Offset: 0x002854D1
		public Cleanse()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CEE RID: 7406
		// (get) Token: 0x06006F53 RID: 28499 RVA: 0x002872DE File Offset: 0x002854DE
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new SummonVar(3m));
			}
		}

		// Token: 0x17001CEF RID: 7407
		// (get) Token: 0x06006F54 RID: 28500 RVA: 0x002872F0 File Offset: 0x002854F0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }),
					HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
				});
			}
		}

		// Token: 0x06006F55 RID: 28501 RVA: 0x00287324 File Offset: 0x00285524
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(base.Owner.Character), Necrobinder.GetSummonDelayIfApplicable(base.Owner.Character));
			await OstyCmd.Summon(choiceContext, base.Owner, base.DynamicVars.Summon.BaseValue, this);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(base.Owner), base.Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1));
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				await CardCmd.Exhaust(choiceContext, cardModel, false, false);
			}
		}

		// Token: 0x06006F56 RID: 28502 RVA: 0x0028736F File Offset: 0x0028556F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Summon.UpgradeValueBy(2m);
		}
	}
}
