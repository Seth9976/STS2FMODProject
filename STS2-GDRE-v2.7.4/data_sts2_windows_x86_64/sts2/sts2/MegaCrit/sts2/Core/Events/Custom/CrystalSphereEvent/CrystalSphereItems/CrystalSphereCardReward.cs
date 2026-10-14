using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems
{
	// Token: 0x02000C53 RID: 3155
	[NullableContext(1)]
	[Nullable(0)]
	public class CrystalSphereCardReward : CrystalSphereItem
	{
		// Token: 0x1700237F RID: 9087
		// (get) Token: 0x06008211 RID: 33297 RVA: 0x002C06F8 File Offset: 0x002BE8F8
		private string BannerMaterialPath
		{
			get
			{
				switch (this._rarity)
				{
				case CardRarity.Uncommon:
					return "res://materials/cards/banners/card_banner_uncommon_mat.tres";
				case CardRarity.Rare:
					return "res://materials/cards/banners/card_banner_rare_mat.tres";
				case CardRarity.Ancient:
					return "res://materials/cards/banners/card_banner_ancient_mat.tres";
				case CardRarity.Event:
					return "res://materials/cards/banners/card_banner_event_mat.tres";
				case CardRarity.Status:
					return "res://materials/cards/banners/card_banner_status_mat.tres";
				case CardRarity.Curse:
					return "res://materials/cards/banners/card_banner_curse_mat.tres";
				case CardRarity.Quest:
					return "res://materials/cards/banners/card_banner_quest_mat.tres";
				}
				return "res://materials/cards/banners/card_banner_common_mat.tres";
			}
		}

		// Token: 0x17002380 RID: 9088
		// (get) Token: 0x06008212 RID: 33298 RVA: 0x002C0775 File Offset: 0x002BE975
		public Material BannerMaterial
		{
			get
			{
				return PreloadManager.Cache.GetMaterial(this.BannerMaterialPath);
			}
		}

		// Token: 0x17002381 RID: 9089
		// (get) Token: 0x06008213 RID: 33299 RVA: 0x002C0787 File Offset: 0x002BE987
		public Material FrameMaterial
		{
			get
			{
				return this._owner.Character.CardPool.FrameMaterial;
			}
		}

		// Token: 0x17002382 RID: 9090
		// (get) Token: 0x06008214 RID: 33300 RVA: 0x002C079E File Offset: 0x002BE99E
		public override Vector2I Size
		{
			get
			{
				return new Vector2I(2, 2);
			}
		}

		// Token: 0x17002383 RID: 9091
		// (get) Token: 0x06008215 RID: 33301 RVA: 0x002C07A8 File Offset: 0x002BE9A8
		protected override string TexturePath
		{
			get
			{
				return ImageHelper.GetImagePath("events/crystal_sphere/crystal_sphere_" + this._rarity.ToString().ToLowerInvariant() + "_card_reward.png");
			}
		}

		// Token: 0x17002384 RID: 9092
		// (get) Token: 0x06008216 RID: 33302 RVA: 0x002C07E2 File Offset: 0x002BE9E2
		public override bool IsGood
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008217 RID: 33303 RVA: 0x002C07E5 File Offset: 0x002BE9E5
		public CrystalSphereCardReward(CardRarity rarity, Player owner)
		{
			this._rarity = rarity;
			this._owner = owner;
		}

		// Token: 0x06008218 RID: 33304 RVA: 0x002C07FC File Offset: 0x002BE9FC
		public override Reward ToReward(Player owner, Rng rng)
		{
			CardCreationOptions cardCreationOptions = new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(owner.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.Uniform, (CardModel c) => c.Rarity == this._rarity).WithRngOverride(rng);
			return new CardReward(cardCreationOptions, 3, owner, null);
		}

		// Token: 0x06008219 RID: 33305 RVA: 0x002C083C File Offset: 0x002BEA3C
		public override SerializableCrystalSphereItem ToSerializable()
		{
			return new SerializableCrystalSphereItem
			{
				type = CrystalSphereItemType.CardReward,
				cardRarity = this._rarity
			};
		}

		// Token: 0x04002BA0 RID: 11168
		private readonly Player _owner;

		// Token: 0x04002BA1 RID: 11169
		private readonly CardRarity _rarity;
	}
}
