using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005DB RID: 1499
	[NullableContext(1)]
	[Nullable(0)]
	public class SeaGlass : RelicModel
	{
		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x060054A9 RID: 21673 RVA: 0x002471D0 File Offset: 0x002453D0
		public override LocString Title
		{
			get
			{
				if (this.Character == null)
				{
					return new LocString("relics", base.Id.Entry + ".title");
				}
				return new LocString("relics", base.Id.Entry + "." + this.Character.Id.Entry + ".title");
			}
		}

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x060054AA RID: 21674 RVA: 0x00247239 File Offset: 0x00245439
		// (set) Token: 0x060054AB RID: 21675 RVA: 0x00247241 File Offset: 0x00245441
		[Nullable(2)]
		[SavedProperty]
		public ModelId CharacterId
		{
			[NullableContext(2)]
			get
			{
				return this._characterId;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._characterId = value;
				((StringVar)base.DynamicVars["Character"]).StringValue = this.Character.Title.GetFormattedText();
			}
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x060054AC RID: 21676 RVA: 0x0024727A File Offset: 0x0024547A
		[Nullable(2)]
		private CharacterModel Character
		{
			[NullableContext(2)]
			get
			{
				if (!(this.CharacterId != null))
				{
					return null;
				}
				return ModelDb.GetById<CharacterModel>(this.CharacterId);
			}
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x060054AD RID: 21677 RVA: 0x00247297 File Offset: 0x00245497
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x060054AE RID: 21678 RVA: 0x0024729A File Offset: 0x0024549A
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x060054AF RID: 21679 RVA: 0x0024729D File Offset: 0x0024549D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(15),
					new StringVar("Character", "")
				});
			}
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x002472C8 File Offset: 0x002454C8
		public override async Task AfterObtained()
		{
			if (this.CharacterId == null)
			{
				Log.Error("Sea Glass was obtained without a character ID assigned! This could be a bug, or the player could have used the console. Defaulting to Ironclad", 2);
				this.CharacterId = ModelDb.Character<Ironclad>().Id;
			}
			int num = base.DynamicVars.Cards.IntValue / 3;
			CardCreationOptions cardCreationOptions = CardCreationOptions.ForNonCombatWithUniformOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(this.Character.CardPool), (CardModel c) => c.Rarity == CardRarity.Common).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);
			CardCreationOptions cardCreationOptions2 = CardCreationOptions.ForNonCombatWithUniformOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(this.Character.CardPool), (CardModel c) => c.Rarity == CardRarity.Uncommon).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);
			CardCreationOptions cardCreationOptions3 = CardCreationOptions.ForNonCombatWithUniformOdds(new <>z__ReadOnlySingleElementList<CardPoolModel>(this.Character.CardPool), (CardModel c) => c.Rarity == CardRarity.Rare).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);
			List<CardCreationResult> list = CardFactory.CreateForReward(base.Owner, num, cardCreationOptions).ToList<CardCreationResult>();
			List<CardCreationResult> list2 = CardFactory.CreateForReward(base.Owner, num, cardCreationOptions2).ToList<CardCreationResult>();
			List<CardCreationResult> list3 = CardFactory.CreateForReward(base.Owner, num, cardCreationOptions3).ToList<CardCreationResult>();
			List<CardCreationResult> list4 = list.Concat<CardCreationResult>(list2).Concat<CardCreationResult>(list3).ToList<CardCreationResult>();
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(RelicModel.L10NLookup(base.Id.Entry + ".selectionScreenPrompt"), 0, list4.Count);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromSimpleGridForRewards(new BlockingPlayerChoiceContext(), list4, base.Owner, cardSelectorPrefs);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false), 1.2f, CardPreviewStyle.HorizontalLayout);
			}
			IEnumerator<CardModel> enumerator = null;
		}

		// Token: 0x040024B6 RID: 9398
		private const string _characterKey = "Character";

		// Token: 0x040024B7 RID: 9399
		[Nullable(2)]
		private ModelId _characterId;
	}
}
