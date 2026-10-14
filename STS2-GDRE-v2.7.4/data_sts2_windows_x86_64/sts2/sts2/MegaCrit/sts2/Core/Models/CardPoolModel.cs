using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004DD RID: 1245
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class CardPoolModel : AbstractModel, IPoolModel
	{
		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06004BC0 RID: 19392
		public abstract string Title { get; }

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06004BC1 RID: 19393
		public abstract string EnergyColorName { get; }

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06004BC2 RID: 19394
		public abstract string CardFrameMaterialPath { get; }

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06004BC3 RID: 19395 RVA: 0x0022A918 File Offset: 0x00228B18
		public string FrameMaterialPath
		{
			get
			{
				return "res://materials/cards/frames/" + this.CardFrameMaterialPath + "_mat.tres";
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06004BC4 RID: 19396 RVA: 0x0022A92F File Offset: 0x00228B2F
		public Material FrameMaterial
		{
			get
			{
				return PreloadManager.Cache.GetMaterial(this.FrameMaterialPath);
			}
		}

		/// <summary>
		/// The color of the card back when viewing a card from this pool in the Run History screen.
		/// </summary>
		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06004BC5 RID: 19397
		public abstract Color DeckEntryCardColor { get; }

		/// <summary>
		/// The color of the outline of the text in this card's Energy UI.
		/// It must blend with the energy icon of this card!
		/// </summary>
		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06004BC6 RID: 19398 RVA: 0x0022A941 File Offset: 0x00228B41
		public virtual Color EnergyOutlineColor
		{
			get
			{
				return new Color("5C5440");
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06004BC7 RID: 19399 RVA: 0x0022A94D File Offset: 0x00228B4D
		public string EnergyIconPath
		{
			get
			{
				return EnergyIconHelper.GetPath(this.EnergyColorName);
			}
		}

		/// <summary>
		/// Get every card in this pool (ignores Unlocks/Epoch state).
		/// </summary>
		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06004BC8 RID: 19400 RVA: 0x0022A95A File Offset: 0x00228B5A
		public virtual IEnumerable<CardModel> AllCards
		{
			get
			{
				if (this._allCards == null)
				{
					this._allCards = this.GenerateAllCards();
					this._allCards = ModHelper.ConcatModelsFromMods<CardModel>(this, this._allCards).ToArray<CardModel>();
				}
				return this._allCards;
			}
		}

		/// <summary>
		/// Get the IDs of every card in this pool (ignores Unlocks/Epoch state).
		/// </summary>
		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06004BC9 RID: 19401 RVA: 0x0022A990 File Offset: 0x00228B90
		public IEnumerable<ModelId> AllCardIds
		{
			get
			{
				HashSet<ModelId> hashSet;
				if ((hashSet = this._allCardIds) == null)
				{
					hashSet = (this._allCardIds = this.AllCards.Select<CardModel, ModelId>((CardModel c) => c.Id).ToHashSet<ModelId>());
				}
				return hashSet;
			}
		}

		/// <summary>
		/// Generates every card in this pool (ignores Unlocks/Epoch state).
		/// Overridden in subclasses, but should only be called once by <see cref="P:MegaCrit.Sts2.Core.Models.CardPoolModel.AllCards" /> so it can be cached.
		/// </summary>
		// Token: 0x06004BCA RID: 19402
		protected abstract CardModel[] GenerateAllCards();

		/// <summary>
		/// Clears the lazily-computed <see cref="P:MegaCrit.Sts2.Core.Models.CardPoolModel.AllCards" />/<see cref="P:MegaCrit.Sts2.Core.Models.CardPoolModel.AllCardIds" /> caches so they are regenerated on
		/// next access. Canonical pools are immutable, so they never need this, but mutable pools (like
		/// <see cref="T:MegaCrit.Sts2.Core.Models.CardPools.MockCardPool" />) can change their card list after construction and must invalidate the caches.
		/// </summary>
		// Token: 0x06004BCB RID: 19403 RVA: 0x0022A9DF File Offset: 0x00228BDF
		protected void InvalidateCardCache()
		{
			this._allCards = null;
			this._allCardIds = null;
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x0022A9EF File Offset: 0x00228BEF
		protected override void DeepCloneFields()
		{
			base.DeepCloneFields();
			this.InvalidateCardCache();
		}

		/// <summary>
		/// Returns every card in this pool that the player has unlocked.
		/// This excludes cards that haven't been unlocked via certain epochs. We also can filter cards based on whether
		/// they are allowed to appear during a singleplayer or multiplayer run.
		/// <param name="unlockState">Used to filter cards through unlocked epochs.</param>
		/// <param name="multiplayerConstraint">Used to filter cards based on if this is a singleplayer or multiplayer run.</param>
		/// </summary>
		// Token: 0x06004BCD RID: 19405 RVA: 0x0022AA00 File Offset: 0x00228C00
		public IEnumerable<CardModel> GetUnlockedCards(UnlockState unlockState, CardMultiplayerConstraint multiplayerConstraint)
		{
			List<CardModel> list = this.FilterThroughEpochs(unlockState, this.AllCards).ToList<CardModel>();
			if (multiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly)
			{
				if (multiplayerConstraint == CardMultiplayerConstraint.SingleplayerOnly)
				{
					list.RemoveAll((CardModel c) => c.MultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly);
				}
			}
			else
			{
				list.RemoveAll((CardModel c) => c.MultiplayerConstraint == CardMultiplayerConstraint.SingleplayerOnly);
			}
			return list;
		}

		/// <summary>
		/// Returns a list of cards but prunes out cards if the associated Epochs aren't revealed.
		/// </summary>
		// Token: 0x06004BCE RID: 19406 RVA: 0x0022AA79 File Offset: 0x00228C79
		protected virtual IEnumerable<CardModel> FilterThroughEpochs(UnlockState unlockState, IEnumerable<CardModel> cards)
		{
			return cards.ToList<CardModel>();
		}

		/// <summary>
		/// Is this a colorless card pool?
		/// False for card pools that are associated with a specific character (like <see cref="T:MegaCrit.Sts2.Core.Models.CardPools.IroncladCardPool" />), true
		/// for other card pools (like <see cref="T:MegaCrit.Sts2.Core.Models.CardPools.ColorlessCardPool" /> or <see cref="T:MegaCrit.Sts2.Core.Models.CardPools.EventCardPool" />).
		/// </summary>
		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06004BCF RID: 19407
		public abstract bool IsColorless { get; }

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06004BD0 RID: 19408 RVA: 0x0022AA81 File Offset: 0x00228C81
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x0022AA84 File Offset: 0x00228C84
		public CardPoolModel ToMutable()
		{
			base.AssertCanonical();
			return (CardPoolModel)base.MutableClone();
		}

		// Token: 0x04001D1F RID: 7455
		[Nullable(new byte[] { 2, 1 })]
		private CardModel[] _allCards;

		// Token: 0x04001D20 RID: 7456
		[Nullable(new byte[] { 2, 1 })]
		private HashSet<ModelId> _allCardIds;
	}
}
