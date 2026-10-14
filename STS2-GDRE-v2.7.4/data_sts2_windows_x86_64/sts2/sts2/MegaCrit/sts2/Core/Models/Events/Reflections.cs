using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200084A RID: 2122
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Reflections : EventModel
	{
		// Token: 0x0600688E RID: 26766 RVA: 0x00278344 File Offset: 0x00276544
		public override void OnRoomEnter()
		{
			NEventRoom instance = NEventRoom.Instance;
			if (instance == null)
			{
				return;
			}
			Control vfxContainer = instance.VfxContainer;
			if (vfxContainer == null)
			{
				return;
			}
			vfxContainer.AddChildSafely(NMirrorVfx.Create());
		}

		// Token: 0x0600688F RID: 26767 RVA: 0x00278364 File Offset: 0x00276564
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.TouchAMirror), "REFLECTIONS.pages.INITIAL.options.TOUCH_A_MIRROR", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.Shatter), "REFLECTIONS.pages.INITIAL.options.SHATTER", HoverTipFactory.FromCardWithCardHoverTips<BadLuck>(false))
			});
		}

		// Token: 0x06006890 RID: 26768 RVA: 0x002783BC File Offset: 0x002765BC
		private async Task TouchAMirror()
		{
			List<CardModel> upgradedCards = base.Owner.Deck.Cards.Where<CardModel>((CardModel c) => c.IsUpgraded).ToList<CardModel>();
			int i = 0;
			while (i < 2 && upgradedCards.Count > 0)
			{
				CardModel cardModel = base.Rng.NextItem<CardModel>(upgradedCards);
				upgradedCards.Remove(cardModel);
				CardCmd.Downgrade(cardModel);
				CardCmd.Preview(cardModel, 1.2f, CardPreviewStyle.MessyLayout);
				await Cmd.CustomScaledWait(0.3f, 0.5f, false, default(CancellationToken));
				i++;
			}
			List<CardModel> upgradableCards = base.Owner.Deck.Cards.Where<CardModel>((CardModel c) => c.IsUpgradable).ToList<CardModel>();
			i = 0;
			while (i < 4 && upgradableCards.Count > 0)
			{
				CardModel cardModel2 = base.Rng.NextItem<CardModel>(upgradableCards);
				upgradableCards.Remove(cardModel2);
				CardCmd.Upgrade(cardModel2, CardPreviewStyle.MessyLayout);
				await Cmd.CustomScaledWait(0.3f, 0.5f, false, default(CancellationToken));
				i++;
			}
			await Cmd.CustomScaledWait(0.6f, 1.2f, false, default(CancellationToken));
			base.SetEventFinished(base.L10NLookup("REFLECTIONS.pages.TOUCH_A_MIRROR.description"));
		}

		// Token: 0x06006891 RID: 26769 RVA: 0x00278400 File Offset: 0x00276600
		private async Task Shatter()
		{
			int originalDeckSize = base.Owner.Deck.Cards.Count;
			for (int i = 0; i < originalDeckSize; i++)
			{
				CardModel cardModel = base.Owner.RunState.CloneCard(base.Owner.Deck.Cards[i]);
				CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
				CardPileAddResult cardPileAddResult2 = cardPileAddResult;
				CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.MessyLayout);
				await Cmd.CustomScaledWait(0.1f, 0.2f, false, default(CancellationToken));
			}
			await Cmd.CustomScaledWait(0.6f, 1.2f, false, default(CancellationToken));
			await CardPileCmd.AddCurseToDeck<BadLuck>(base.Owner);
			base.SetEventFinished(base.L10NLookup("REFLECTIONS.pages.SHATTER.description"));
		}
	}
}
