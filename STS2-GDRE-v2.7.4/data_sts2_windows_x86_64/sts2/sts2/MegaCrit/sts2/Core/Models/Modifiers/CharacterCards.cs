using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x02000815 RID: 2069
	[NullableContext(1)]
	[Nullable(0)]
	public class CharacterCards : ModifierModel
	{
		// Token: 0x17001958 RID: 6488
		// (get) Token: 0x06006728 RID: 26408 RVA: 0x00271DC0 File Offset: 0x0026FFC0
		public override LocString Title
		{
			get
			{
				return ModelDb.GetById<CharacterModel>(this.CharacterModel).CardsModifierTitle;
			}
		}

		// Token: 0x17001959 RID: 6489
		// (get) Token: 0x06006729 RID: 26409 RVA: 0x00271DD2 File Offset: 0x0026FFD2
		public override LocString Description
		{
			get
			{
				return ModelDb.GetById<CharacterModel>(this.CharacterModel).CardsModifierDescription;
			}
		}

		// Token: 0x1700195A RID: 6490
		// (get) Token: 0x0600672A RID: 26410 RVA: 0x00271DE4 File Offset: 0x0026FFE4
		// (set) Token: 0x0600672B RID: 26411 RVA: 0x00271DFB File Offset: 0x0026FFFB
		[SavedProperty]
		public ModelId CharacterModel
		{
			get
			{
				ModelId characterModel = this._characterModel;
				if (characterModel == null)
				{
					throw new InvalidOperationException("CharacterCards modifier used without CharacterModel set!");
				}
				return characterModel;
			}
			set
			{
				base.AssertMutable();
				this._characterModel = value;
			}
		}

		// Token: 0x1700195B RID: 6491
		// (get) Token: 0x0600672C RID: 26412 RVA: 0x00271E0A File Offset: 0x0027000A
		private CardPoolModel CharacterCardPool
		{
			get
			{
				return ModelDb.GetById<CharacterModel>(this.CharacterModel).CardPool;
			}
		}

		// Token: 0x0600672D RID: 26413 RVA: 0x00271E1C File Offset: 0x0027001C
		public override IEnumerable<CardModel> ModifyMerchantCardPool(Player player, IEnumerable<CardModel> options)
		{
			CardPoolModel cardPool = player.Character.CardPool;
			CardModel[] array = options.ToArray<CardModel>();
			if (array.Any<CardModel>((CardModel c) => c.Pool != cardPool))
			{
				return array;
			}
			return array.Concat<CardModel>(this.CharacterCardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint));
		}

		// Token: 0x0600672E RID: 26414 RVA: 0x00271E80 File Offset: 0x00270080
		public override CardCreationOptions ModifyCardRewardCreationOptions(Player player, CardCreationOptions options)
		{
			if (options.Flags.HasFlag(CardCreationFlags.NoCardPoolModifications))
			{
				return options;
			}
			if (!options.Flags.HasFlag(CardCreationFlags.IsCardReward))
			{
				return options;
			}
			return options.WithCardPools(options.CardPools.Union<CardPoolModel>(new <>z__ReadOnlySingleElementList<CardPoolModel>(this.CharacterCardPool)));
		}

		// Token: 0x0600672F RID: 26415 RVA: 0x00271EE2 File Offset: 0x002700E2
		public override bool IsEquivalent(ModifierModel other)
		{
			return base.IsEquivalent(other) && ((CharacterCards)other)._characterModel == this._characterModel;
		}

		// Token: 0x0400274F RID: 10063
		[Nullable(2)]
		private ModelId _characterModel;
	}
}
