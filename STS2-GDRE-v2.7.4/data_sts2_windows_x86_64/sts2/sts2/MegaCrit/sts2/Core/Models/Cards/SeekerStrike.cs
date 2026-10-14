using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AB6 RID: 2742
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SeekerStrike : CardModel
	{
		// Token: 0x060076D2 RID: 30418 RVA: 0x0029675C File Offset: 0x0029495C
		public SeekerStrike()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002022 RID: 8226
		// (get) Token: 0x060076D3 RID: 30419 RVA: 0x00296769 File Offset: 0x00294969
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x17002023 RID: 8227
		// (get) Token: 0x060076D4 RID: 30420 RVA: 0x00296778 File Offset: 0x00294978
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(9m, ValueProp.Move),
					new CardsVar(3)
				});
			}
		}

		// Token: 0x060076D5 RID: 30421 RVA: 0x002967A0 File Offset: 0x002949A0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			IEnumerable<CardModel> cardOptions = PileType.Draw.GetPile(base.Owner).Cards.ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.CombatCardSelection).Take<CardModel>(base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(base.Owner), base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, 1), (CardModel c) => cardOptions.Contains(c));
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				await CardPileCmd.Add(cardModel, PileType.Hand, CardPilePosition.Bottom, null, false);
			}
		}

		// Token: 0x060076D6 RID: 30422 RVA: 0x002967F3 File Offset: 0x002949F3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
