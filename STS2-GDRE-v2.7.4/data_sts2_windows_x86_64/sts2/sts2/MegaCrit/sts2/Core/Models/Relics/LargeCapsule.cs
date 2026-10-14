using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000573 RID: 1395
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LargeCapsule : RelicModel
	{
		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x060051F1 RID: 20977 RVA: 0x00241DAF File Offset: 0x0023FFAF
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x060051F2 RID: 20978 RVA: 0x00241DB2 File Offset: 0x0023FFB2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new IntVar("Relics", 2m));
			}
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x00241DCC File Offset: 0x0023FFCC
		public override async Task AfterObtained()
		{
			for (int i = 0; i < base.DynamicVars["Relics"].IntValue; i++)
			{
				RelicModel relicModel = RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable();
				await RelicCmd.Obtain(relicModel, base.Owner, -1);
			}
			List<CardPileAddResult> list = new List<CardPileAddResult>(2);
			List<CardPileAddResult> list2 = list;
			list2.Add(await CardPileCmd.Add(base.Owner.RunState.CreateCard(LargeCapsule.GetStrikeForCharacter(base.Owner.Character), base.Owner), PileType.Deck, CardPilePosition.Bottom, null, false));
			List<CardPileAddResult> list3 = list;
			list3.Add(await CardPileCmd.Add(base.Owner.RunState.CreateCard(LargeCapsule.GetDefendForCharacter(base.Owner.Character), base.Owner), PileType.Deck, CardPilePosition.Bottom, null, false));
			List<CardPileAddResult> list4 = list;
			list2 = null;
			list3 = null;
			list = null;
			CardCmd.PreviewCardPileAdd(list4, 2f, CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x00241E10 File Offset: 0x00240010
		private static CardModel GetStrikeForCharacter(CharacterModel character)
		{
			if (TestMode.IsOn && character is Deprived)
			{
				return ModelDb.Card<StrikeIronclad>();
			}
			return character.CardPool.AllCards.First<CardModel>((CardModel c) => c.Rarity == CardRarity.Basic && c.Tags.Contains(CardTag.Strike));
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x00241E64 File Offset: 0x00240064
		private static CardModel GetDefendForCharacter(CharacterModel character)
		{
			if (TestMode.IsOn && character is Deprived)
			{
				return ModelDb.Card<DefendIronclad>();
			}
			return character.CardPool.AllCards.First<CardModel>((CardModel c) => c.Rarity == CardRarity.Basic && c.Tags.Contains(CardTag.Defend));
		}

		// Token: 0x0400246A RID: 9322
		private const string _relicKey = "Relics";
	}
}
