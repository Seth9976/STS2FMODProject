using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000574 RID: 1396
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LastingCandy : RelicModel
	{
		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x060051F7 RID: 20983 RVA: 0x00241EBD File Offset: 0x002400BD
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x00241EC0 File Offset: 0x002400C0
		public override bool IsAllowed(IRunState runState)
		{
			return !runState.Players.Any<Player>(delegate(Player p)
			{
				if (p != null && p.Character is Ironclad)
				{
					UnlockState unlockState = p.UnlockState;
					if (unlockState != null)
					{
						return unlockState.NumberOfRuns == 0;
					}
				}
				return false;
			}) && RelicModel.IsBeforeAct3TreasureChest(runState);
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x060051F9 RID: 20985 RVA: 0x00241EF6 File Offset: 0x002400F6
		public override bool ShowCounter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x060051FA RID: 20986 RVA: 0x00241EF9 File Offset: 0x002400F9
		public override int DisplayAmount
		{
			get
			{
				if (!this.IsActivating)
				{
					return this.CombatRewardsSeen % 2;
				}
				return 2;
			}
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x060051FB RID: 20987 RVA: 0x00241F0D File Offset: 0x0024010D
		// (set) Token: 0x060051FC RID: 20988 RVA: 0x00241F15 File Offset: 0x00240115
		private bool IsActivating
		{
			get
			{
				return this._isActivating;
			}
			set
			{
				base.AssertMutable();
				this._isActivating = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x060051FD RID: 20989 RVA: 0x00241F2A File Offset: 0x0024012A
		// (set) Token: 0x060051FE RID: 20990 RVA: 0x00241F32 File Offset: 0x00240132
		[SavedProperty]
		public int CombatRewardsSeen
		{
			get
			{
				return this._combatRewardsSeen;
			}
			set
			{
				base.AssertMutable();
				this._combatRewardsSeen = value;
			}
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x00241F44 File Offset: 0x00240144
		public override bool TryModifyCardRewardOptions(Player player, List<CardCreationResult> rewardOptions, CardCreationOptions creationOptions)
		{
			if (base.Owner != player)
			{
				return false;
			}
			if (creationOptions.Source != CardCreationSource.Encounter)
			{
				return false;
			}
			if (!this.IsInTriggeringCombat)
			{
				return false;
			}
			if (!creationOptions.Flags.HasFlag(CardCreationFlags.IsCardReward))
			{
				return false;
			}
			if (!creationOptions.Flags.HasFlag(CardCreationFlags.IsFromCombat))
			{
				return false;
			}
			bool allowDupes = false;
			List<CardModel> list = creationOptions.GetPossibleCards(player).ToList<CardModel>();
			IEnumerable<CardModel> enumerable = list.Where<CardModel>((CardModel c) => LastingCandy.CardPoolFilter(c, rewardOptions, false));
			if (!enumerable.Any<CardModel>())
			{
				allowDupes = true;
				enumerable = list.Where<CardModel>((CardModel c) => LastingCandy.CardPoolFilter(c, rewardOptions, true));
			}
			if (!enumerable.Any<CardModel>())
			{
				return false;
			}
			CardCreationOptions cardCreationOptions = new CardCreationOptions(creationOptions.CardPools, CardCreationSource.Other, creationOptions.RarityOdds, delegate(CardModel c)
			{
				Func<CardModel, bool> cardPoolFilter = creationOptions.CardPoolFilter;
				return (cardPoolFilter == null || cardPoolFilter(c)) && LastingCandy.CardPoolFilter(c, rewardOptions, allowDupes);
			}).WithFlags(CardCreationFlags.NoModifyHooks | CardCreationFlags.NoCardPoolModifications);
			CardCreationResult cardCreationResult = CardFactory.CreateForReward(base.Owner, 1, cardCreationOptions).FirstOrDefault<CardCreationResult>();
			CardModel cardModel = ((cardCreationResult != null) ? cardCreationResult.Card : null);
			if (cardModel != null)
			{
				CardCreationResult cardCreationResult2 = new CardCreationResult(cardModel);
				cardCreationResult2.ModifyCard(cardModel, this);
				rewardOptions.Add(cardCreationResult2);
			}
			return cardModel != null;
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x002420A4 File Offset: 0x002402A4
		public override Task BeforeCombatRewardOffered(RewardsSet rewards, CombatRoom room)
		{
			if (rewards.Player != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (rewards.Rewards.All<Reward>((Reward r) => !(r is CardReward)))
			{
				return Task.CompletedTask;
			}
			if (this.IsInTriggeringCombat)
			{
				TaskHelper.RunSafely(this.DoActivateVisuals());
			}
			int combatRewardsSeen = this.CombatRewardsSeen;
			this.CombatRewardsSeen = combatRewardsSeen + 1;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x06005201 RID: 20993 RVA: 0x00242128 File Offset: 0x00240328
		private async Task DoActivateVisuals()
		{
			this.IsActivating = true;
			base.Flash();
			await Cmd.Wait(1f, false);
			this.IsActivating = false;
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06005202 RID: 20994 RVA: 0x0024216B File Offset: 0x0024036B
		private bool IsInTriggeringCombat
		{
			get
			{
				return this.CombatRewardsSeen > 0 && this.CombatRewardsSeen % 2 == 1;
			}
		}

		// Token: 0x06005203 RID: 20995 RVA: 0x00242184 File Offset: 0x00240384
		private static bool CardPoolFilter(CardModel card, List<CardCreationResult> rewardOptions, bool allowDupes)
		{
			return card.Type == CardType.Power && (allowDupes || rewardOptions.TrueForAll((CardCreationResult o) => o.originalCard.Id != card.Id));
		}

		// Token: 0x0400246B RID: 9323
		private bool _isActivating;

		// Token: 0x0400246C RID: 9324
		private int _combatRewardsSeen;
	}
}
