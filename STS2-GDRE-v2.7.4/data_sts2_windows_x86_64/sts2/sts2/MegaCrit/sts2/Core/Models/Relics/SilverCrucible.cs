using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005E3 RID: 1507
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SilverCrucible : RelicModel
	{
		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x060054E2 RID: 21730 RVA: 0x00247890 File Offset: 0x00245A90
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x060054E3 RID: 21731 RVA: 0x00247893 File Offset: 0x00245A93
		public override bool IsUsedUp
		{
			get
			{
				return this.TimesUsed >= base.DynamicVars.Cards.IntValue && this.TreasureRoomsEntered > 0;
			}
		}

		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x060054E4 RID: 21732 RVA: 0x002478B8 File Offset: 0x00245AB8
		public override bool ShowCounter
		{
			get
			{
				return base.DynamicVars.Cards.IntValue > 0 && this.TimesUsed < base.DynamicVars.Cards.IntValue;
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x060054E5 RID: 21733 RVA: 0x002478E7 File Offset: 0x00245AE7
		public override int DisplayAmount
		{
			get
			{
				return base.DynamicVars.Cards.IntValue - this.TimesUsed;
			}
		}

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x060054E6 RID: 21734 RVA: 0x00247900 File Offset: 0x00245B00
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x060054E7 RID: 21735 RVA: 0x0024790D File Offset: 0x00245B0D
		// (set) Token: 0x060054E8 RID: 21736 RVA: 0x00247915 File Offset: 0x00245B15
		[SavedProperty]
		public int TimesUsed
		{
			get
			{
				return this._timesUsed;
			}
			set
			{
				base.AssertMutable();
				this._timesUsed = value;
				base.InvokeDisplayAmountChanged();
				this.CheckIfUsedUp();
			}
		}

		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x060054E9 RID: 21737 RVA: 0x00247930 File Offset: 0x00245B30
		// (set) Token: 0x060054EA RID: 21738 RVA: 0x00247938 File Offset: 0x00245B38
		[SavedProperty]
		public int TreasureRoomsEntered
		{
			get
			{
				return this._treasureRoomsEntered;
			}
			set
			{
				base.AssertMutable();
				this._treasureRoomsEntered = value;
				this.CheckIfUsedUp();
			}
		}

		// Token: 0x060054EB RID: 21739 RVA: 0x0024794D File Offset: 0x00245B4D
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.Count == 1;
		}

		// Token: 0x060054EC RID: 21740 RVA: 0x0024795D File Offset: 0x00245B5D
		private void CheckIfUsedUp()
		{
			if (this.IsUsedUp)
			{
				base.Status = RelicStatus.Disabled;
			}
		}

		// Token: 0x060054ED RID: 21741 RVA: 0x00247970 File Offset: 0x00245B70
		public override bool TryModifyCardRewardOptionsLate(Player player, List<CardCreationResult> cardRewards, CardCreationOptions options)
		{
			if (player != base.Owner)
			{
				return false;
			}
			if (this.TimesUsed >= base.DynamicVars.Cards.IntValue)
			{
				return false;
			}
			if (!options.Flags.HasFlag(CardCreationFlags.IsCardReward))
			{
				return false;
			}
			foreach (CardCreationResult cardCreationResult in cardRewards)
			{
				CardModel card = cardCreationResult.Card;
				if (card.IsUpgradable)
				{
					CardModel cardModel = base.Owner.RunState.CloneCard(card);
					CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
					cardCreationResult.ModifyCard(cardModel, this);
				}
			}
			return true;
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x00247A2C File Offset: 0x00245C2C
		public override Task AfterModifyingCardRewardOptions()
		{
			if (this.TimesUsed >= base.DynamicVars.Cards.IntValue)
			{
				return Task.CompletedTask;
			}
			int timesUsed = this.TimesUsed;
			this.TimesUsed = timesUsed + 1;
			return Task.CompletedTask;
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x00247A6C File Offset: 0x00245C6C
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (room is TreasureRoom)
			{
				int treasureRoomsEntered = this.TreasureRoomsEntered;
				this.TreasureRoomsEntered = treasureRoomsEntered + 1;
			}
			return Task.CompletedTask;
		}

		// Token: 0x060054F0 RID: 21744 RVA: 0x00247A96 File Offset: 0x00245C96
		public override bool ShouldGenerateTreasure(Player player)
		{
			return player != base.Owner || this.TreasureRoomsEntered > 1;
		}

		// Token: 0x040024BE RID: 9406
		private int _timesUsed;

		// Token: 0x040024BF RID: 9407
		private int _treasureRoomsEntered;
	}
}
