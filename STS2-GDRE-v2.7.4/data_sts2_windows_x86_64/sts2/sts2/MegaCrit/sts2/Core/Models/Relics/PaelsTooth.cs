using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005AA RID: 1450
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaelsTooth : RelicModel
	{
		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06005369 RID: 21353 RVA: 0x00244B59 File Offset: 0x00242D59
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x0600536A RID: 21354 RVA: 0x00244B5C File Offset: 0x00242D5C
		public override bool ShowCounter
		{
			get
			{
				return base.IsMutable && this._serializableCards.Count > 0;
			}
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x0600536B RID: 21355 RVA: 0x00244B76 File Offset: 0x00242D76
		public override int DisplayAmount
		{
			get
			{
				if (!base.IsMutable)
				{
					return 0;
				}
				return this._serializableCards.Count;
			}
		}

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x0600536C RID: 21356 RVA: 0x00244B8D File Offset: 0x00242D8D
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x0600536D RID: 21357 RVA: 0x00244B90 File Offset: 0x00242D90
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(5),
					new StringVar("CardTitles", "")
				});
			}
		}

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x0600536E RID: 21358 RVA: 0x00244BB8 File Offset: 0x00242DB8
		// (set) Token: 0x0600536F RID: 21359 RVA: 0x00244BC0 File Offset: 0x00242DC0
		[SavedProperty]
		public List<SerializableCard> SerializableCards
		{
			get
			{
				return this._serializableCards;
			}
			private set
			{
				base.AssertMutable();
				this._serializableCards.Clear();
				this._serializableCards.AddRange(value);
				this.UpdateCardList();
			}
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x00244BE5 File Offset: 0x00242DE5
		protected override void AfterCloned()
		{
			base.AfterCloned();
			this._serializableCards = new List<SerializableCard>();
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x00244BF8 File Offset: 0x00242DF8
		public override async Task AfterObtained()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, base.DynamicVars.Cards.IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForRemoval(base.Owner, cardSelectorPrefs, (CardModel c) => c.IsUpgradable);
			IEnumerable<CardModel> enumerable2 = enumerable.OrderBy<CardModel, string>((CardModel c) => c.Id.Entry, StringComparer.Ordinal);
			foreach (CardModel cardModel in enumerable2)
			{
				CardModel cardModel2 = (CardModel)cardModel.MutableClone();
				this.SerializableCards.Add(cardModel2.ToSerializable());
				await CardPileCmd.RemoveFromDeck(cardModel, true);
			}
			IEnumerator<CardModel> enumerator = null;
			this.UpdateCardList();
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x00244C3C File Offset: 0x00242E3C
		public override async Task AfterCombatEnd(CombatRoom room)
		{
			if (!base.Owner.Creature.IsDead)
			{
				if (this.SerializableCards.Count != 0)
				{
					base.Flash();
					await Cmd.CustomScaledWait(0.1f, 1f, false, default(CancellationToken));
					SerializableCard serializableCard = base.Owner.PlayerRng.Rewards.NextItem<SerializableCard>(this.SerializableCards);
					CardModel cardModel = CardModel.FromSerializable(serializableCard);
					if (!base.Owner.RunState.ContainsCard(cardModel))
					{
						base.Owner.RunState.AddCard(cardModel, base.Owner);
					}
					if (cardModel.IsUpgradable)
					{
						CardCmd.Upgrade(cardModel, CardPreviewStyle.MessyLayout);
					}
					CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
					CardCmd.PreviewCardPileAdd(cardPileAddResult, 1.2f, CardPreviewStyle.HorizontalLayout);
					base.Status = ((this.SerializableCards.Count > 0) ? RelicStatus.Normal : RelicStatus.Disabled);
					this.SerializableCards.Remove(serializableCard);
					this.UpdateCardList();
				}
			}
		}

		// Token: 0x06005373 RID: 21363 RVA: 0x00244C80 File Offset: 0x00242E80
		private void UpdateCardList()
		{
			base.Status = ((this.SerializableCards.Count > 0) ? RelicStatus.Normal : RelicStatus.Disabled);
			StringVar stringVar = (StringVar)base.DynamicVars["CardTitles"];
			if (this.SerializableCards.Count == 0)
			{
				stringVar.StringValue = string.Empty;
			}
			else
			{
				stringVar.StringValue = string.Join<string>('\n', this.SerializableCards.Select<SerializableCard, string>((SerializableCard c) => "- " + SaveUtil.CardOrDeprecated(c.Id).Title));
			}
			base.InvokeDisplayAmountChanged();
		}

		/// <summary>
		/// DO NOT USE, this is only public for tests.
		/// </summary>
		// Token: 0x06005374 RID: 21364 RVA: 0x00244D12 File Offset: 0x00242F12
		public void DebugAddCard(SerializableCard card)
		{
			this.SerializableCards.Add(card);
		}

		// Token: 0x0400248F RID: 9359
		public const int cardsCount = 5;

		// Token: 0x04002490 RID: 9360
		private const string _cardTitlesKey = "CardTitles";

		// Token: 0x04002491 RID: 9361
		private List<SerializableCard> _serializableCards = new List<SerializableCard>();
	}
}
