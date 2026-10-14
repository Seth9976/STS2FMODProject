using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000537 RID: 1335
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DustyTome : RelicModel
	{
		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06005054 RID: 20564 RVA: 0x0023EF00 File Offset: 0x0023D100
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06005055 RID: 20565 RVA: 0x0023EF03 File Offset: 0x0023D103
		// (set) Token: 0x06005056 RID: 20566 RVA: 0x0023EF0C File Offset: 0x0023D10C
		[Nullable(2)]
		[SavedProperty]
		public ModelId AncientCard
		{
			[NullableContext(2)]
			get
			{
				return this._ancientCard;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._ancientCard = value;
				if (this._ancientCard != null)
				{
					CardModel cardModel = SaveUtil.CardOrDeprecated(this._ancientCard);
					this._extraHoverTips = cardModel.HoverTips.Concat<IHoverTip>(new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard(cardModel, true)));
					((StringVar)base.DynamicVars["AncientCard"]).StringValue = cardModel.Title;
				}
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06005057 RID: 20567 RVA: 0x0023EF7D File Offset: 0x0023D17D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StringVar("AncientCard", ""));
			}
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x06005058 RID: 20568 RVA: 0x0023EF93 File Offset: 0x0023D193
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return this._extraHoverTips;
			}
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x0023EF9C File Offset: 0x0023D19C
		public void SetupForPlayer(Player player)
		{
			IEnumerable<CardModel> enumerable = from c in player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
				where c.Rarity == CardRarity.Ancient && !ArchaicTooth.TranscendenceCards.Contains(c)
				select c;
			this.AncientCard = player.PlayerRng.Rewards.NextItem<CardModel>(enumerable).Id;
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x0023F00C File Offset: 0x0023D20C
		public override async Task AfterObtained()
		{
			CardModel cardModel = base.Owner.RunState.CreateCard(ModelDb.GetById<CardModel>(this.AncientCard), base.Owner);
			CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(cardPileAddResult2, 2f, CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x0400243D RID: 9277
		private const string _ancientCardKey = "AncientCard";

		// Token: 0x0400243E RID: 9278
		[Nullable(2)]
		private ModelId _ancientCard;

		// Token: 0x0400243F RID: 9279
		private IEnumerable<IHoverTip> _extraHoverTips = Array.Empty<IHoverTip>();
	}
}
