using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Rewards
{
	/// <summary>
	/// A reward that adds a specific card to the player's deck.
	/// Good for events like <see cref="T:MegaCrit.Sts2.Core.Models.Events.TheLanternKey" /> that give specific quest cards, and for other miscellaneous spots
	/// that offer specific cards as rewards (like <see cref="T:MegaCrit.Sts2.Core.Models.Monsters.ThievingHopper" /> giving you your stolen card back as a reward).
	/// </summary>
	// Token: 0x0200014E RID: 334
	[NullableContext(1)]
	[Nullable(0)]
	public class SpecialCardReward : Reward
	{
		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x0600101A RID: 4122 RVA: 0x00061406 File Offset: 0x0005F606
		private static string RewardIcon
		{
			get
			{
				return ImageHelper.GetImagePath("ui/reward_screen/reward_icon_special_card.png");
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x00061412 File Offset: 0x0005F612
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(SpecialCardReward.RewardIcon);
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x0600101C RID: 4124 RVA: 0x0006141E File Offset: 0x0005F61E
		protected override RewardType RewardType
		{
			get
			{
				return RewardType.SpecialCard;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x0600101D RID: 4125 RVA: 0x00061421 File Offset: 0x0005F621
		public override int RewardsSetIndex
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x00061424 File Offset: 0x0005F624
		protected override string IconPath
		{
			get
			{
				return SpecialCardReward.RewardIcon;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x0600101F RID: 4127 RVA: 0x0006142C File Offset: 0x0005F62C
		public override LocString Description
		{
			get
			{
				LocString locString = null;
				if (this._customDescriptionEncounterSourceId != ModelId.none)
				{
					locString = ModelDb.GetById<EncounterModel>(this._customDescriptionEncounterSourceId).CustomRewardDescription;
				}
				if (locString == null)
				{
					locString = new LocString("gameplay_ui", "COMBAT_REWARD_ADD_SPECIAL_CARD");
				}
				locString.Add("Card", this._card.Title);
				return locString;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x00061488 File Offset: 0x0005F688
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard(this._card, false));
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0006149B File Offset: 0x0005F69B
		public SpecialCardReward(CardModel card, Player player)
			: base(player)
		{
			card.AssertMutable();
			this._card = card;
		}

		/// <summary>
		/// Set an encounter to use for this reward's description.
		/// If this is not set, the default description will be used.
		/// </summary>
		// Token: 0x06001022 RID: 4130 RVA: 0x000614BC File Offset: 0x0005F6BC
		public void SetCustomDescriptionEncounterSource(ModelId encounterId)
		{
			if (ModelDb.GetByIdOrNull<EncounterModel>(encounterId) == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Encounter ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(encounterId);
				defaultInterpolatedStringHandler.AppendLiteral(" does not exist!");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._customDescriptionEncounterSourceId = encounterId;
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x0006150F File Offset: 0x0005F70F
		public override bool IsPopulated
		{
			get
			{
				return this._card != null;
			}
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0006151A File Offset: 0x0005F71A
		public override void Populate()
		{
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0006151C File Offset: 0x0005F71C
		protected override async Task<bool> OnSelect()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(base.Player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" obtained ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this._card.Id);
			defaultInterpolatedStringHandler.AppendLiteral(" from special card reward");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(this._card, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			if (cardPileAddResult2.success)
			{
				CardCmd.PreviewCardPileAdd(cardPileAddResult2, 2f, CardPreviewStyle.HorizontalLayout);
			}
			this._wasTaken = true;
			return true;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00061560 File Offset: 0x0005F760
		public override void OnSkipped()
		{
			if (this._wasTaken)
			{
				return;
			}
			base.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(base.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(this._card, false));
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x000615AC File Offset: 0x0005F7AC
		public override SerializableReward ToSerializable()
		{
			return new SerializableReward
			{
				RewardType = this.RewardType,
				SpecialCard = this._card.ToSerializable(),
				CustomDescriptionEncounterSourceId = this._customDescriptionEncounterSourceId
			};
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x000615DC File Offset: 0x0005F7DC
		public override void MarkContentAsSeen()
		{
		}

		// Token: 0x040005B5 RID: 1461
		private bool _wasTaken;

		// Token: 0x040005B6 RID: 1462
		[Nullable(2)]
		private readonly CardModel _card;

		// Token: 0x040005B7 RID: 1463
		private ModelId _customDescriptionEncounterSourceId = ModelId.none;
	}
}
