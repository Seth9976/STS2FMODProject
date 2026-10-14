using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Rewards
{
	// Token: 0x0200014A RID: 330
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class Reward
	{
		/// <summary>
		/// The player that this reward is for.
		/// </summary>
		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06000FEB RID: 4075 RVA: 0x000606EE File Offset: 0x0005E8EE
		public Player Player { get; }

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06000FEC RID: 4076
		protected abstract RewardType RewardType { get; }

		/// <summary>
		/// The index to use for this reward when ordering the rewards displayed in a <see cref="T:MegaCrit.Sts2.Core.Rewards.RewardsSet" />.
		/// Lower indexes are displayed first.
		/// </summary>
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06000FED RID: 4077
		public abstract int RewardsSetIndex { get; }

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06000FEE RID: 4078
		public abstract LocString Description { get; }

		/// <summary>
		/// Whether this reward has been populated.
		/// </summary>
		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06000FEF RID: 4079
		public abstract bool IsPopulated { get; }

		/// <summary>
		/// Whether the reward has successfully been taken.
		/// </summary>
		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x000606F6 File Offset: 0x0005E8F6
		// (set) Token: 0x06000FF1 RID: 4081 RVA: 0x000606FE File Offset: 0x0005E8FE
		public bool SuccessfullySelected { get; private set; }

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00060707 File Offset: 0x0005E907
		[Nullable(2)]
		protected virtual string IconPath
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x0006070A File Offset: 0x0005E90A
		public virtual Vector2 IconPosition
		{
			get
			{
				return Vector2.Zero;
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x00060711 File Offset: 0x0005E911
		protected Reward(Player player)
		{
			this.Player = player;
		}

		/// <summary>
		/// Logic to run to populate this reward with options.
		/// For example, calling this on a CardReward would generate 3 cards for the player to choose from.
		/// </summary>
		// Token: 0x06000FF5 RID: 4085
		public abstract void Populate();

		/// <summary>
		/// Logic to run when this reward is selected.
		/// </summary>
		/// <returns>
		/// Whether the reward was received. Usually true, but false in certain cases (like if the player tries to
		/// take a potion when they have no more room for potions).
		/// </returns>
		// Token: 0x06000FF6 RID: 4086
		protected abstract Task<bool> OnSelect();

		/// <summary>
		/// Create an icon node representing this reward.
		/// Null if we're in test mode.
		/// </summary>
		// Token: 0x06000FF7 RID: 4087 RVA: 0x00060720 File Offset: 0x0005E920
		[NullableContext(2)]
		public virtual Control CreateIcon()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			TextureRect textureRect = new TextureRect();
			textureRect.Texture = PreloadManager.Cache.GetCompressedTexture2D(this.IconPath);
			textureRect.SetAnchorsPreset(15L, false);
			textureRect.ExpandMode = 1L;
			return textureRect;
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x00060765 File Offset: 0x0005E965
		protected virtual IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return Array.Empty<IHoverTip>();
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x0006076C File Offset: 0x0005E96C
		public virtual IEnumerable<IHoverTip> HoverTips
		{
			get
			{
				List<IHoverTip> list = this.ExtraHoverTips.ToList<IHoverTip>();
				if (this.ParentRewardSet != null)
				{
					list.Add(LinkedRewardSet.HoverTip);
				}
				return list;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x0006079E File Offset: 0x0005E99E
		// (set) Token: 0x06000FFB RID: 4091 RVA: 0x000607A6 File Offset: 0x0005E9A6
		[Nullable(2)]
		public LinkedRewardSet ParentRewardSet
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x000607AF File Offset: 0x0005E9AF
		public virtual void OnSkipped()
		{
		}

		/// <summary>
		/// YOU SHOULD MOST LIKELY NOT BE CALLING THIS!
		/// See <see cref="M:MegaCrit.Sts2.Core.Multiplayer.Game.RewardsSetSynchronizer.SelectLocalReward(MegaCrit.Sts2.Core.Rewards.Reward)" />.
		/// This only selects the reward on the local machine.
		/// </summary>
		// Token: 0x06000FFD RID: 4093 RVA: 0x000607B4 File Offset: 0x0005E9B4
		public async Task<bool> SelectUnsynchronized()
		{
			bool flag = await this.OnSelect();
			bool success = flag;
			if (success)
			{
				await Hook.AfterRewardTaken(this.Player.RunState, this.Player, this);
				this.SuccessfullySelected = true;
			}
			if (this.ParentRewardSet != null)
			{
				this.ParentRewardSet.RemoveReward(this);
				await this.ParentRewardSet.OnSelect();
			}
			return success;
		}

		// Token: 0x06000FFE RID: 4094
		public abstract void MarkContentAsSeen();

		// Token: 0x06000FFF RID: 4095 RVA: 0x000607F7 File Offset: 0x0005E9F7
		public virtual SerializableReward ToSerializable()
		{
			return new SerializableReward
			{
				RewardType = this.RewardType
			};
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0006080A File Offset: 0x0005EA0A
		public Reward SetRng(Rng rng)
		{
			this._rngOverride = rng;
			return this;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00060814 File Offset: 0x0005EA14
		public static Reward FromSerializable(SerializableReward save, Player player)
		{
			switch (save.RewardType)
			{
			case RewardType.Card:
			{
				IEnumerable<ModelId> cardPoolIds = save.CardPoolIds;
				Func<ModelId, CardPoolModel> func;
				if ((func = Reward.<>O.<0>__GetById) == null)
				{
					func = (Reward.<>O.<0>__GetById = new Func<ModelId, CardPoolModel>(ModelDb.GetById<CardPoolModel>));
				}
				CardCreationOptions cardCreationOptions = new CardCreationOptions(cardPoolIds.Select<ModelId, CardPoolModel>(func), save.Source, save.RarityOdds, null);
				return new CardReward(cardCreationOptions, save.OptionCount, player, null);
			}
			case RewardType.Gold:
				return new GoldReward(save.GoldAmount, player, save.WasGoldStolenBack);
			case RewardType.Potion:
				return new PotionReward(player);
			case RewardType.Relic:
				if (save.PredeterminedModelId != ModelId.none)
				{
					return new RelicReward(ModelDb.GetById<RelicModel>(save.PredeterminedModelId).ToMutable(), player);
				}
				return new RelicReward(player);
			case RewardType.RemoveCard:
				return new CardRemovalReward(player);
			case RewardType.SpecialCard:
			{
				CardModel cardModel = CardModel.FromSerializable(save.SpecialCard);
				player.RunState.AddCard(cardModel, player);
				SpecialCardReward specialCardReward = new SpecialCardReward(cardModel, player);
				if (save.CustomDescriptionEncounterSourceId != ModelId.none)
				{
					specialCardReward.SetCustomDescriptionEncounterSource(save.CustomDescriptionEncounterSourceId);
				}
				return specialCardReward;
			}
			default:
				throw new NotImplementedException("Serializing these types of rewards hasn't been implemented yet");
			}
		}

		/// <summary>
		/// If this is set, uses this rng rather than the default one used by the particular reward (Typically a player RNG)
		/// Used in the Crystal Sphere Items so that we can pass the event rng to be used and not hit state divergences in multiplayer
		/// since we don't sync the player rng sets in that case
		/// </summary>
		// Token: 0x040005A1 RID: 1441
		[Nullable(2)]
		protected Rng _rngOverride;

		// Token: 0x02000F34 RID: 3892
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04003C56 RID: 15446
			[Nullable(0)]
			public static Func<ModelId, CardPoolModel> <0>__GetById;
		}
	}
}
