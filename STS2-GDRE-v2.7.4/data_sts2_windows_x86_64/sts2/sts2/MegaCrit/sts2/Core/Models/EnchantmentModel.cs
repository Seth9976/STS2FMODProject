using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004DF RID: 1247
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class EnchantmentModel : AbstractModel
	{
		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x06004C24 RID: 19492 RVA: 0x0022B4BB File Offset: 0x002296BB
		public LocString Title
		{
			get
			{
				return new LocString("enchantments", base.Id.Entry + ".title");
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x06004C25 RID: 19493 RVA: 0x0022B4DC File Offset: 0x002296DC
		private LocString Description
		{
			get
			{
				return new LocString("enchantments", base.Id.Entry + ".description");
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06004C26 RID: 19494 RVA: 0x0022B4FD File Offset: 0x002296FD
		private LocString ExtraCardText
		{
			get
			{
				return new LocString("enchantments", base.Id.Entry + ".extraCardText");
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06004C27 RID: 19495 RVA: 0x0022B51E File Offset: 0x0022971E
		public virtual bool HasExtraCardText
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06004C28 RID: 19496 RVA: 0x0022B524 File Offset: 0x00229724
		public LocString DynamicDescription
		{
			get
			{
				LocString description = this.Description;
				description.Add("Amount", this.Amount);
				DynamicVarSet dynamicVarSet = this.DynamicVars.Clone(this);
				dynamicVarSet.ClearPreview();
				CardModel card = this._card;
				if (card != null)
				{
					card.UpdateDynamicVarPreview(CardPreviewMode.None, null, dynamicVarSet);
				}
				description.Add("energyPrefix", EnergyIconHelper.GetPrefix(this));
				dynamicVarSet.AddTo(description);
				return description;
			}
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x06004C29 RID: 19497 RVA: 0x0022B590 File Offset: 0x00229790
		[Nullable(2)]
		public LocString DynamicExtraCardText
		{
			[NullableContext(2)]
			get
			{
				if (!this.HasExtraCardText || this.Status == EnchantmentStatus.Disabled)
				{
					return null;
				}
				LocString extraCardText = this.ExtraCardText;
				extraCardText.Add("Amount", this.Amount);
				if (base.IsCanonical)
				{
					extraCardText.Add("TargetType", "None");
				}
				else
				{
					extraCardText.Add("TargetType", this.Card.TargetType.ToString());
				}
				this.DynamicVars.AddTo(extraCardText);
				return extraCardText;
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x06004C2A RID: 19498 RVA: 0x0022B618 File Offset: 0x00229818
		public static string MissingIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("enchantments/missing_enchantment.png");
			}
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06004C2B RID: 19499 RVA: 0x0022B624 File Offset: 0x00229824
		public string IntendedIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("enchantments/" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06004C2C RID: 19500 RVA: 0x0022B64A File Offset: 0x0022984A
		private string BetaIconPath
		{
			get
			{
				return ImageHelper.GetImagePath("enchantments/beta/" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06004C2D RID: 19501 RVA: 0x0022B670 File Offset: 0x00229870
		public string IconPath
		{
			get
			{
				if (this._iconPath == null)
				{
					if (ResourceLoader.Exists(this.IntendedIconPath, ""))
					{
						this._iconPath = this.IntendedIconPath;
					}
					else if (ResourceLoader.Exists(this.BetaIconPath, ""))
					{
						this._iconPath = this.BetaIconPath;
					}
					else
					{
						this._iconPath = EnchantmentModel.MissingIconPath;
					}
				}
				return this._iconPath;
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06004C2E RID: 19502 RVA: 0x0022B6D6 File Offset: 0x002298D6
		public CompressedTexture2D Icon
		{
			get
			{
				return PreloadManager.Cache.GetCompressedTexture2D(this.IconPath);
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06004C2F RID: 19503 RVA: 0x0022B6E8 File Offset: 0x002298E8
		public virtual bool ShowAmount
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x06004C30 RID: 19504 RVA: 0x0022B6EB File Offset: 0x002298EB
		public virtual int DisplayAmount
		{
			get
			{
				return this.Amount;
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06004C31 RID: 19505 RVA: 0x0022B6F3 File Offset: 0x002298F3
		public override bool PreviewOutsideOfCombat
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06004C32 RID: 19506 RVA: 0x0022B6F8 File Offset: 0x002298F8
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				CardModel card = this.Card;
				return card != null && card.ShouldReceiveCombatHooks;
			}
		}

		/// <summary>
		/// Set this to true to sort the card to the bottom of the draw pile at the beginning of combat.
		/// </summary>
		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06004C33 RID: 19507 RVA: 0x0022B717 File Offset: 0x00229917
		public virtual bool ShouldStartAtBottomOfDrawPile
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Get the card that this is enchanting.
		/// This is almost never null, so we leave it as non-nullable to make it easier to use. If you really need to check
		/// for null, use <see cref="P:MegaCrit.Sts2.Core.Models.EnchantmentModel.HasCard" />.
		/// </summary>
		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06004C34 RID: 19508 RVA: 0x0022B71A File Offset: 0x0022991A
		// (set) Token: 0x06004C35 RID: 19509 RVA: 0x0022B728 File Offset: 0x00229928
		public CardModel Card
		{
			get
			{
				base.AssertMutable();
				return this._card;
			}
			set
			{
				base.AssertMutable();
				value.AssertMutable();
				if (this._card != null)
				{
					throw new InvalidOperationException("Enchantments cannot be moved from one card to another.");
				}
				this._card = value;
			}
		}

		/// <summary>
		/// Does this enchantment have a card?
		/// </summary>
		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06004C36 RID: 19510 RVA: 0x0022B750 File Offset: 0x00229950
		public bool HasCard
		{
			get
			{
				return this._card != null;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06004C37 RID: 19511 RVA: 0x0022B75B File Offset: 0x0022995B
		// (set) Token: 0x06004C38 RID: 19512 RVA: 0x0022B763 File Offset: 0x00229963
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				base.AssertMutable();
				this._amount = value;
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06004C39 RID: 19513 RVA: 0x0022B772 File Offset: 0x00229972
		// (set) Token: 0x06004C3A RID: 19514 RVA: 0x0022B77A File Offset: 0x0022997A
		[Nullable(2)]
		[JsonPropertyName("props")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public SavedProperties Props
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x0022B783 File Offset: 0x00229983
		public virtual bool CanEnchantCardType(CardType cardType)
		{
			return true;
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x06004C3C RID: 19516 RVA: 0x0022B786 File Offset: 0x00229986
		public virtual bool IsStackable
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Checks whether the specified card can be enchanted with this enchantment. For example, Sharp can only
		/// enchant attacks, so this will return true if an Attack is passed, but false if a Skill is passed.
		///
		/// Note: Do not override this method to REMOVE restrictions, just to ADD them. When you override it, make sure to
		/// call `base.CanEnchant`, and then add your own restrictions afterwards. You can also override some other methods
		/// to add specific types of restrictions (check EnchantmentModel.cs for details).
		/// </summary>
		/// <param name="card">Card to check validity of.</param>
		/// <returns>Whether or not the specified card is valid to enchant with this.</returns>
		// Token: 0x06004C3D RID: 19517 RVA: 0x0022B78C File Offset: 0x0022998C
		public virtual bool CanEnchant(CardModel card)
		{
			CardType type = card.Type;
			bool flag = type - CardType.Status <= 2;
			if (flag)
			{
				return false;
			}
			if (!this.CanEnchantCardType(card.Type))
			{
				return false;
			}
			CardPile pile = card.Pile;
			return (pile == null || pile.Type != PileType.Deck || !card.Keywords.Contains(CardKeyword.Unplayable)) && (card.Enchantment == null || (this.IsStackable && !(card.Enchantment.GetType() != base.GetType())));
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x0022B813 File Offset: 0x00229A13
		public virtual Task OnPlay(PlayerChoiceContext choiceContext, [Nullable(2)] CardPlay cardPlay)
		{
			return Task.CompletedTask;
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x06004C3F RID: 19519 RVA: 0x0022B81A File Offset: 0x00229A1A
		public DynamicVarSet DynamicVars
		{
			get
			{
				if (this._dynamicVars != null)
				{
					return this._dynamicVars;
				}
				this._dynamicVars = new DynamicVarSet(this.CanonicalVars);
				this._dynamicVars.InitializeWithOwner(this);
				return this._dynamicVars;
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06004C40 RID: 19520 RVA: 0x0022B84E File Offset: 0x00229A4E
		protected virtual IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return Array.Empty<DynamicVar>();
			}
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06004C41 RID: 19521 RVA: 0x0022B855 File Offset: 0x00229A55
		// (set) Token: 0x06004C42 RID: 19522 RVA: 0x0022B85D File Offset: 0x00229A5D
		public EnchantmentStatus Status
		{
			get
			{
				return this._status;
			}
			set
			{
				base.AssertMutable();
				if (this._status == value)
				{
					return;
				}
				this._status = value;
				Action statusChanged = this.StatusChanged;
				if (statusChanged == null)
				{
					return;
				}
				statusChanged();
			}
		}

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06004C43 RID: 19523 RVA: 0x0022B888 File Offset: 0x00229A88
		// (remove) Token: 0x06004C44 RID: 19524 RVA: 0x0022B8C0 File Offset: 0x00229AC0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action StatusChanged;

		/// <summary>
		/// Override this property to add conditions to check to determine whether to show a gold glow on this card.
		/// </summary>
		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06004C45 RID: 19525 RVA: 0x0022B8F5 File Offset: 0x00229AF5
		public virtual bool ShouldGlowGold
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Override this property to add conditions to check to determine whether to show a red glow on this card.
		/// For example, Corrupted causes the card to glow red if it will kill you.
		/// </summary>
		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06004C46 RID: 19526 RVA: 0x0022B8F8 File Offset: 0x00229AF8
		public virtual bool ShouldGlowRed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06004C47 RID: 19527 RVA: 0x0022B8FB File Offset: 0x00229AFB
		// (set) Token: 0x06004C48 RID: 19528 RVA: 0x0022B90D File Offset: 0x00229B0D
		public EnchantmentModel CanonicalInstance
		{
			get
			{
				if (!base.IsMutable)
				{
					return this;
				}
				return this._canonicalInstance;
			}
			private set
			{
				base.AssertMutable();
				this._canonicalInstance = value;
			}
		}

		// Token: 0x06004C49 RID: 19529 RVA: 0x0022B91C File Offset: 0x00229B1C
		public EnchantmentModel ToMutable()
		{
			base.AssertCanonical();
			EnchantmentModel enchantmentModel = (EnchantmentModel)base.MutableClone();
			enchantmentModel.CanonicalInstance = this;
			return enchantmentModel;
		}

		// Token: 0x06004C4A RID: 19530 RVA: 0x0022B943 File Offset: 0x00229B43
		protected override void DeepCloneFields()
		{
			this._card = null;
			this.StatusChanged = null;
			this._dynamicVars = this.DynamicVars.Clone(this);
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06004C4B RID: 19531 RVA: 0x0022B965 File Offset: 0x00229B65
		public HoverTip HoverTip
		{
			get
			{
				return new HoverTip(this.Title, this.DynamicDescription, this.Icon);
			}
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06004C4C RID: 19532 RVA: 0x0022B97E File Offset: 0x00229B7E
		protected virtual IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return Array.Empty<IHoverTip>();
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06004C4D RID: 19533 RVA: 0x0022B988 File Offset: 0x00229B88
		public unsafe IEnumerable<IHoverTip> HoverTips
		{
			get
			{
				int num = 1;
				List<IHoverTip> list = new List<IHoverTip>(num);
				CollectionsMarshal.SetCount<IHoverTip>(list, num);
				Span<IHoverTip> span = CollectionsMarshal.AsSpan<IHoverTip>(list);
				int num2 = 0;
				*span[num2] = this.HoverTip;
				List<IHoverTip> list2 = list;
				list2.AddRange(this.ExtraHoverTips);
				return list2;
			}
		}

		/// <summary>
		/// WARNING: If you're thinking of calling this from inside a model, you probably want <see cref="M:MegaCrit.Sts2.Core.Commands.CardCmd.Enchant(MegaCrit.Sts2.Core.Models.EnchantmentModel,MegaCrit.Sts2.Core.Models.CardModel,System.Decimal)" />
		/// instead.
		///
		/// Apply this enchantment to the specified card. This does not run any hooks.
		/// </summary>
		/// <param name="card">Card to enchant.</param>
		/// <param name="amount">Amount for the enchantment.</param>
		// Token: 0x06004C4E RID: 19534 RVA: 0x0022B9CF File Offset: 0x00229BCF
		public void ApplyInternal(CardModel card, decimal amount)
		{
			if (this.Card != null)
			{
				throw new InvalidOperationException("Can't apply an enchantment to a card when it's already been applied to a different card.");
			}
			base.AssertMutable();
			card.AssertMutable();
			this.Amount = (int)amount;
			this.Card = card;
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x0022BA03 File Offset: 0x00229C03
		public void ClearInternal()
		{
			base.AssertMutable();
			this._card = null;
		}

		/// <summary>
		/// Run this enchantment's modification logic on the enchanted card. You usually want to call
		/// <see cref="M:MegaCrit.Sts2.Core.Models.EnchantmentModel.ApplyInternal(MegaCrit.Sts2.Core.Models.CardModel,System.Decimal)" /> first.
		///
		/// This is called after a card is first enchanted, and after it is deserialized.
		///
		/// It is also called when an enchantment's modification logic needs to be refreshed. For example, after a card is
		/// downgraded, its DamageVars will be reset to their original values. If the card is enchanted with Sharp, we need
		/// to re-execute its modification logic so the card's damage will still be increased.
		///
		/// It is NOT called when a card is cloned, because the enchantments effects will already be reflected in the card's
		/// values.
		/// </summary>
		// Token: 0x06004C50 RID: 19536 RVA: 0x0022BA12 File Offset: 0x00229C12
		public void ModifyCard()
		{
			if (this.Card == null)
			{
				throw new InvalidOperationException("Card must be set at this point.");
			}
			this.OnEnchant();
			this.RecalculateValues();
			this.Card.DynamicVars.RecalculateForUpgradeOrEnchant();
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x0022BA43 File Offset: 0x00229C43
		public virtual void RecalculateValues()
		{
		}

		// Token: 0x06004C52 RID: 19538 RVA: 0x0022BA45 File Offset: 0x00229C45
		public SerializableEnchantment ToSerializable()
		{
			base.AssertMutable();
			return new SerializableEnchantment
			{
				Id = base.Id,
				Props = SavedProperties.From(this),
				Amount = this.Amount
			};
		}

		// Token: 0x06004C53 RID: 19539 RVA: 0x0022BA78 File Offset: 0x00229C78
		public static EnchantmentModel FromSerializable(SerializableEnchantment save)
		{
			EnchantmentModel enchantmentModel = SaveUtil.EnchantmentOrDeprecated(save.Id).ToMutable();
			SavedProperties props = save.Props;
			if (props != null)
			{
				props.Fill(enchantmentModel);
			}
			enchantmentModel.Amount = save.Amount;
			return enchantmentModel;
		}

		/// <summary>
		/// Modify the card that this is enchanting.
		/// Use this for enchantments that do things similar to upgrades, like change the values of a card's DynamicVars
		/// (Sharp), change a card's keywords (Royally Approved), change a card's energy cost (Tezcatara's Ember), etc.
		/// To modify a card's energy cost, use <see cref="M:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost.UpgradeBy(System.Int32)" />.
		/// </summary>
		// Token: 0x06004C54 RID: 19540 RVA: 0x0022BAB5 File Offset: 0x00229CB5
		protected virtual void OnEnchant()
		{
		}

		/// <summary>
		/// Add to the amount of block that this enchantment's card gains.
		/// This hook runs BEFORE all other block modification hooks.
		/// Enchantments MUST use this hook instead of <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyBlockAdditive(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" />.
		/// </summary>
		/// <param name="originalBlock">The original amount of block that would be gained.</param>
		/// <returns>The amount to add to the block gain.</returns>
		// Token: 0x06004C55 RID: 19541 RVA: 0x0022BAB7 File Offset: 0x00229CB7
		public virtual decimal EnchantBlockAdditive(decimal originalBlock)
		{
			return 0m;
		}

		/// <summary>
		/// Modify the amount of block that this enchantment's card gains.
		/// This hook runs BEFORE all other block modification hooks.
		/// Enchantments MUST use this hook instead of <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyBlockMultiplicative(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" />.
		/// </summary>
		/// <param name="originalBlock">The original amount of block that would be gained.</param>
		/// <returns>The amount to multiply the block gain by.</returns>
		// Token: 0x06004C56 RID: 19542 RVA: 0x0022BABE File Offset: 0x00229CBE
		public virtual decimal EnchantBlockMultiplicative(decimal originalBlock)
		{
			return 1m;
		}

		/// <summary>
		/// Add to the amount of damage that this enchantment's card does.
		/// This hook runs BEFORE all other damage modification hooks.
		/// Enchantments MUST use this hook instead of <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyDamageAdditive(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" />.
		/// </summary>
		/// <param name="originalDamage">The amount of damage that would be dealt.</param>
		/// <param name="props">ValueProp for damage.</param>
		/// <returns>Amount of damage to be added.</returns>
		// Token: 0x06004C57 RID: 19543 RVA: 0x0022BAC5 File Offset: 0x00229CC5
		public virtual decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
		{
			return 0m;
		}

		/// <summary>
		/// Multiply the amount of damage that this enchantment's card does.
		/// This hook runs BEFORE all other damage modification hooks.
		/// Enchantments MUST use this hook instead of <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyDamageMultiplicative(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Decimal,MegaCrit.Sts2.Core.ValueProps.ValueProp,MegaCrit.Sts2.Core.Entities.Creatures.Creature,MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" />.
		/// </summary>
		/// <param name="originalDamage">The amount of damage that would be dealt.</param>
		/// <param name="props">ValueProp for damage.</param>
		/// <returns>Amount that the damage should be multiplied by.</returns>
		// Token: 0x06004C58 RID: 19544 RVA: 0x0022BACC File Offset: 0x00229CCC
		public virtual decimal EnchantDamageMultiplicative(decimal originalDamage, ValueProp props)
		{
			return 1m;
		}

		/// <summary>
		/// Modify the number of times this enchantment's card is played.
		/// This hook runs BEFORE all other card play count modification hooks.
		/// Enchantments MUST use this hook instead of <see cref="M:MegaCrit.Sts2.Core.Models.AbstractModel.ModifyCardPlayCount(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Int32)" />.
		/// </summary>
		/// <param name="originalPlayCount">The original number of times this card would be played.</param>
		/// <returns>The new number of times this card should be played.</returns>
		// Token: 0x06004C59 RID: 19545 RVA: 0x0022BAD3 File Offset: 0x00229CD3
		public virtual int EnchantPlayCount(int originalPlayCount)
		{
			return originalPlayCount;
		}

		// Token: 0x04001D27 RID: 7463
		public const string locTable = "enchantments";

		// Token: 0x04001D28 RID: 7464
		[Nullable(2)]
		private string _iconPath;

		// Token: 0x04001D29 RID: 7465
		[Nullable(2)]
		private CardModel _card;

		// Token: 0x04001D2A RID: 7466
		private int _amount;

		// Token: 0x04001D2C RID: 7468
		[Nullable(2)]
		private DynamicVarSet _dynamicVars;

		// Token: 0x04001D2D RID: 7469
		private EnchantmentStatus _status;

		// Token: 0x04001D2F RID: 7471
		private EnchantmentModel _canonicalInstance;
	}
}
