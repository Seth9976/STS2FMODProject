using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000914 RID: 2324
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BeatDown : CardModel
	{
		// Token: 0x06006E19 RID: 28185 RVA: 0x00284ACF File Offset: 0x00282CCF
		public BeatDown()
			: base(3, CardType.Skill, CardRarity.Rare, TargetType.RandomEnemy, true)
		{
		}

		// Token: 0x17001C6F RID: 7279
		// (get) Token: 0x06006E1A RID: 28186 RVA: 0x00284ADC File Offset: 0x00282CDC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x06006E1B RID: 28187 RVA: 0x00284AEC File Offset: 0x00282CEC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			CardPile pile = PileType.Discard.GetPile(base.Owner);
			IEnumerable<CardModel> enumerable = pile.Cards.Where<CardModel>((CardModel c) => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.Shuffle)
				.Take<CardModel>(base.DynamicVars.Cards.IntValue);
			foreach (CardModel cardModel in enumerable)
			{
				if (CombatManager.Instance.IsOverOrEnding)
				{
					break;
				}
				if (cardModel.TargetType == TargetType.AnyEnemy)
				{
					Creature creature = base.Owner.RunState.Rng.CombatTargets.NextItem<Creature>(base.CombatState.HittableEnemies);
					await CardCmd.AutoPlay(choiceContext, cardModel, creature, AutoPlayType.Default, false, false);
				}
				else
				{
					await CardCmd.AutoPlay(choiceContext, cardModel, null, AutoPlayType.Default, false, false);
				}
			}
			IEnumerator<CardModel> enumerator = null;
		}

		// Token: 0x06006E1C RID: 28188 RVA: 0x00284B37 File Offset: 0x00282D37
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
