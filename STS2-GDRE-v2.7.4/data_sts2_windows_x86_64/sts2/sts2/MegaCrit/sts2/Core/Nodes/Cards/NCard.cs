using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Helpers.Models;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Pooling;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Cards
{
	// Token: 0x02000413 RID: 1043
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Cards/NCard.cs")]
	public class NCard : Control, IPoolable
	{
		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x060041BE RID: 16830 RVA: 0x001FC6EE File Offset: 0x001FA8EE
		// (set) Token: 0x060041BF RID: 16831 RVA: 0x001FC6F6 File Offset: 0x001FA8F6
		public NCardHighlight CardHighlight { get; private set; }

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x060041C0 RID: 16832 RVA: 0x001FC6FF File Offset: 0x001FA8FF
		// (set) Token: 0x060041C1 RID: 16833 RVA: 0x001FC707 File Offset: 0x001FA907
		public Control Body { get; private set; }

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x060041C2 RID: 16834 RVA: 0x001FC710 File Offset: 0x001FA910
		// (set) Token: 0x060041C3 RID: 16835 RVA: 0x001FC718 File Offset: 0x001FA918
		public ModelVisibility Visibility
		{
			get
			{
				return this._visibility;
			}
			set
			{
				if (this._visibility != value)
				{
					this._visibility = value;
					this.Reload();
				}
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x060041C4 RID: 16836 RVA: 0x001FC730 File Offset: 0x001FA930
		// (set) Token: 0x060041C5 RID: 16837 RVA: 0x001FC738 File Offset: 0x001FA938
		[Nullable(2)]
		public Tween PlayPileTween
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x001FC741 File Offset: 0x001FA941
		// (set) Token: 0x060041C7 RID: 16839 RVA: 0x001FC749 File Offset: 0x001FA949
		[Nullable(2)]
		private Tween RandomizeCostTween
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x060041C8 RID: 16840 RVA: 0x001FC752 File Offset: 0x001FA952
		// (set) Token: 0x060041C9 RID: 16841 RVA: 0x001FC75A File Offset: 0x001FA95A
		public PileType DisplayingPile { get; private set; }

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x060041CA RID: 16842 RVA: 0x001FC763 File Offset: 0x001FA963
		public Control EnchantmentTab
		{
			get
			{
				return this._enchantmentTab;
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x060041CB RID: 16843 RVA: 0x001FC76B File Offset: 0x001FA96B
		public TextureRect EnchantmentVfxOverride
		{
			get
			{
				return this._enchantmentVfxOverride;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x060041CC RID: 16844 RVA: 0x001FC773 File Offset: 0x001FA973
		public Node OverlayContainer
		{
			get
			{
				return this._overlayContainer;
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x001FC77B File Offset: 0x001FA97B
		public Node CardVfxContainer
		{
			get
			{
				return this._cardVfxContainer;
			}
		}

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x060041CE RID: 16846 RVA: 0x001FC784 File Offset: 0x001FA984
		// (remove) Token: 0x060041CF RID: 16847 RVA: 0x001FC7BC File Offset: 0x001FA9BC
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<CardModel> ModelChanged;

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x060041D0 RID: 16848 RVA: 0x001FC7F1 File Offset: 0x001FA9F1
		// (set) Token: 0x060041D1 RID: 16849 RVA: 0x001FC7FC File Offset: 0x001FA9FC
		[Nullable(2)]
		public CardModel Model
		{
			[NullableContext(2)]
			get
			{
				return this._model;
			}
			[NullableContext(2)]
			set
			{
				if (this._model == value)
				{
					return;
				}
				CardModel model = this._model;
				this.UnsubscribeFromModel(model);
				this._model = value;
				this.Reload();
				this.SubscribeToModel(this._model);
				Action<CardModel> modelChanged = this.ModelChanged;
				if (modelChanged != null)
				{
					modelChanged(model);
				}
				if (this._model != null && (this._model.RunState != null || this._model.CombatState != null) && LocalContext.IsMine(this._model))
				{
					SaveManager.Instance.MarkCardAsSeen(this._model);
				}
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x060041D2 RID: 16850 RVA: 0x001FC88B File Offset: 0x001FAA8B
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "res://scenes/cards/card.tscn", "res://scenes/cards/card_portrait_blur_material.tres", "res://scenes/cards/card_canvas_group_blur_material.tres", "res://scenes/cards/card_canvas_group_mask_blur_material.tres", "res://scenes/cards/card_canvas_group_mask_material.tres" });
			}
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x001FC8C0 File Offset: 0x001FAAC0
		public void OnInstantiated()
		{
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x001FC8C4 File Offset: 0x001FAAC4
		public override void _Ready()
		{
			this._titleLabel = base.GetNode<MegaLabel>("%TitleLabel");
			this._descriptionLabel = base.GetNode<MegaRichTextLabel>("%DescriptionLabel");
			this._frame = base.GetNode<TextureRect>("%Frame");
			this._ancientBorderGlassOverlay = base.GetNode<TextureRect>("%AncientBorderGlassOverlay");
			this._ancientBorder = base.GetNode<TextureRect>("%AncientBorder");
			this._ancientTextBg = base.GetNode<TextureRect>("%AncientTextBg");
			this._ancientBanner = base.GetNode<Control>("%AncientBanner");
			this._portrait = base.GetNode<TextureRect>("%Portrait");
			this._ancientPortrait = base.GetNode<TextureRect>("%AncientPortrait");
			this._typeLabel = base.GetNode<MegaLabel>("%TypeLabel");
			this._portraitBorder = base.GetNode<TextureRect>("%PortraitBorder");
			this._portraitCanvasGroup = base.GetNode<CanvasGroup>("%PortraitCanvasGroup");
			this._energyLabel = base.GetNode<MegaLabel>("%EnergyLabel");
			this._energyIcon = base.GetNode<TextureRect>("%EnergyIcon");
			this._starLabel = base.GetNode<MegaLabel>("%StarLabel");
			this._starIcon = base.GetNode<TextureRect>("%StarIcon");
			this._banner = base.GetNode<TextureRect>("%TitleBanner");
			this._lock = base.GetNode<TextureRect>("%Lock");
			this._typePlaque = base.GetNode<NinePatchRect>("%TypePlaque");
			this._unplayableEnergyIcon = base.GetNode<TextureRect>("%UnplayableEnergyIcon");
			this._unplayableStarIcon = base.GetNode<TextureRect>("%UnplayableStarIcon");
			this._enchantmentIcon = base.GetNode<TextureRect>("%Enchantment/Icon");
			this._enchantmentLabel = base.GetNode<MegaLabel>("%Enchantment/Label");
			this._sparkles = base.GetNode<GpuParticles2D>("CardContainer/CardSparkles");
			this._enchantmentTab = base.GetNode<Control>("%Enchantment");
			this._enchantmentTab.Visible = false;
			this._enchantmentVfxOverride = base.GetNode<TextureRect>("%EnchantmentVfxOverride");
			this.CardHighlight = base.GetNode<NCardHighlight>("%Highlight");
			this.Body = base.GetNode<Control>("%CardContainer");
			this._overlayContainer = base.GetNode("%OverlayContainer");
			this._cardVfxContainer = base.GetNode("%CardVfxContainer");
			this._defaultEnchantmentPosition = this._enchantmentTab.Position;
			this.Reload();
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x001FCB88 File Offset: 0x001FAD88
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
			this.SubscribeToModel(this.Model);
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x001FCBA1 File Offset: 0x001FADA1
		public override void _ExitTree()
		{
			this._cts.Cancel();
			Tween randomizeCostTween = this.RandomizeCostTween;
			if (randomizeCostTween != null)
			{
				randomizeCostTween.Kill();
			}
			this.RandomizeCostTween = null;
			this.UnsubscribeFromModel(this.Model);
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x001FCBD2 File Offset: 0x001FADD2
		[NullableContext(2)]
		private void SubscribeToModel(CardModel model)
		{
			if (model == null || !base.IsInsideTree())
			{
				return;
			}
			model.AfflictionChanged += this.OnAfflictionChanged;
			model.EnchantmentChanged += this.OnEnchantmentChanged;
			this.SubscribeToEnchantment(model.Enchantment);
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x001FCC10 File Offset: 0x001FAE10
		[NullableContext(2)]
		private void UnsubscribeFromModel(CardModel model)
		{
			if (model == null)
			{
				return;
			}
			model.AfflictionChanged -= this.OnAfflictionChanged;
			model.EnchantmentChanged -= this.OnEnchantmentChanged;
			this.UnsubscribeFromEnchantment(model.Enchantment);
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x001FCC48 File Offset: 0x001FAE48
		[NullableContext(2)]
		private void SubscribeToEnchantment(EnchantmentModel model)
		{
			if (model == null || !base.IsInsideTree())
			{
				return;
			}
			if (this._subscribedEnchantment != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to subscribe to enchantment ");
				defaultInterpolatedStringHandler.AppendFormatted<EnchantmentModel>(model);
				defaultInterpolatedStringHandler.AppendLiteral(", but ");
				defaultInterpolatedStringHandler.AppendFormatted<NCard>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" is already subscribed to ");
				defaultInterpolatedStringHandler.AppendFormatted<EnchantmentModel>(this._subscribedEnchantment);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._subscribedEnchantment = model;
			this._subscribedEnchantment.StatusChanged += this.OnEnchantmentStatusChanged;
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x001FCCEB File Offset: 0x001FAEEB
		[NullableContext(2)]
		private void UnsubscribeFromEnchantment(EnchantmentModel model)
		{
			if (model == null)
			{
				return;
			}
			if (model != this._subscribedEnchantment)
			{
				return;
			}
			this._subscribedEnchantment.StatusChanged -= this.OnEnchantmentStatusChanged;
			this._subscribedEnchantment = null;
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x001FCD19 File Offset: 0x001FAF19
		public static void InitPool()
		{
			NodePool.Init<NCard>("res://scenes/cards/card.tscn", 30);
		}

		// Token: 0x060041DC RID: 16860 RVA: 0x001FCD28 File Offset: 0x001FAF28
		[return: Nullable(2)]
		public static NCard Create(CardModel card, ModelVisibility visibility = ModelVisibility.Visible)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCard ncard = NodePool.Get<NCard>();
			ncard.Model = card;
			ncard.Visibility = visibility;
			return ncard;
		}

		/// <summary>
		/// Try to find a card's node if it is in the Play or Hand pile.
		/// The "Table" here refers to the Play or Hand Pile, where there would be
		/// card nodes.
		/// </summary>
		/// <param name="card">The card to find.</param>
		/// <param name="overridePile">The pile to try to find the card in. If null, the card's pile will be used.</param>
		/// <returns></returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
		// Token: 0x060041DD RID: 16861 RVA: 0x001FCD54 File Offset: 0x001FAF54
		[return: Nullable(2)]
		public static NCard FindOnTable(CardModel card, PileType? overridePile = null)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (!CombatManager.Instance.IsInProgress)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCombatUi ncombatUi = ((instance != null) ? instance.Ui : null);
			if (ncombatUi == null)
			{
				return null;
			}
			CardPile pile = card.Pile;
			PileType? pileType = ((pile != null) ? new PileType?(pile.Type) : overridePile);
			NCard ncard;
			if (pileType != null)
			{
				switch (pileType.GetValueOrDefault())
				{
				case PileType.None:
					ncard = null;
					break;
				case PileType.Draw:
					ncard = null;
					break;
				case PileType.Hand:
				{
					NCard ncard2;
					if ((ncard2 = ncombatUi.Hand.GetCard(card)) == null)
					{
						ncard2 = ncombatUi.PlayQueue.GetCardNode(card) ?? ncombatUi.GetCardFromPlayContainer(card);
					}
					ncard = ncard2;
					break;
				}
				case PileType.Discard:
					ncard = null;
					break;
				case PileType.Exhaust:
					ncard = null;
					break;
				case PileType.Play:
					ncard = ncombatUi.GetCardFromPlayContainer(card);
					break;
				case PileType.Deck:
					ncard = null;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				ncard = null;
			}
			return ncard;
		}

		/// <summary>
		/// WARNING: You usually want to use the Size property on <see cref="T:MegaCrit.Sts2.Core.Nodes.Cards.Holders.NCardHolder" />.
		/// This is just for if you need the size of an NCard that's not in a holder.
		///
		/// Get the current size of this card. Takes scale into account.
		/// </summary>
		// Token: 0x060041DE RID: 16862 RVA: 0x001FCE2F File Offset: 0x001FB02F
		public Vector2 GetCurrentSize()
		{
			return NCard.defaultSize * base.Scale;
		}

		/// <summary>
		/// Sets the creature that the card is targeting.
		/// Necessary to do things like update damage amounts when the target has Vulnerable.
		/// </summary>
		/// <param name="creature">
		/// Creature being targeted by the card.
		/// </param>
		// Token: 0x060041DF RID: 16863 RVA: 0x001FCE41 File Offset: 0x001FB041
		[NullableContext(2)]
		public void SetPreviewTarget(Creature creature)
		{
			if (this._previewTarget == creature)
			{
				return;
			}
			this._previewTarget = creature;
			this.UpdateVisuals(this.DisplayingPile, CardPreviewMode.Normal);
		}

		/// <summary>
		/// Refreshes this card to show the correct text given the context.
		/// </summary>
		/// <param name="pileType">
		/// The pile that the card is being viewed in.
		/// We can't just use Model.Pile here, because the visual context might not match up.
		/// Examples: When a card is being played, it might transition from the play pile to the discard pile while being
		/// displayed. If the text to changed in response to this, it would be visually jarring.
		/// </param>
		/// <param name="previewMode">
		/// The type of preview to show in the card's visuals. See <see cref="T:MegaCrit.Sts2.Core.Entities.Cards.CardPreviewMode" /> for details.
		/// </param>
		// Token: 0x060041E0 RID: 16864 RVA: 0x001FCE64 File Offset: 0x001FB064
		public void UpdateVisuals(PileType pileType, CardPreviewMode previewMode)
		{
			if (!base.IsNodeReady())
			{
				return;
			}
			if (this.Model == null)
			{
				throw new InvalidOperationException("Cannot update text with no model.");
			}
			this.DisplayingPile = pileType;
			Creature creature = this._previewTarget ?? this.Model.CurrentTarget;
			this.UpdatePortrait();
			this.UpdateTitleLabel();
			this.UpdateEnergyCostVisuals(pileType);
			this.UpdateStarCostVisuals(pileType);
			this.UpdateEnchantmentVisuals();
			this.Model.DynamicVars.ClearPreview();
			if (!this._forceUnpoweredPreview)
			{
				this.Model.UpdateDynamicVarPreview(previewMode, creature, this.Model.DynamicVars);
				if (this.Model.Enchantment != null)
				{
					this.Model.Enchantment.DynamicVars.ClearPreview();
					this.Model.UpdateDynamicVarPreview(previewMode, creature, this.Model.Enchantment.DynamicVars);
				}
			}
			string text;
			if (previewMode == CardPreviewMode.Upgrade)
			{
				text = this.Model.GetDescriptionForUpgradePreview();
			}
			else
			{
				text = this.Model.GetDescriptionForPile(pileType, creature);
			}
			switch (this.Visibility)
			{
			case ModelVisibility.Visible:
				this._descriptionLabel.SetTextAutoSize("[center]" + text + "[/center]");
				return;
			case ModelVisibility.NotSeen:
				this._descriptionLabel.SetTextAutoSize("[center][font_size=40]" + this._unknownDescription.GetFormattedText() + "[/font_size][/center]");
				return;
			case ModelVisibility.Locked:
				this._descriptionLabel.SetTextAutoSize("[center][font_size=40]" + this._lockedDescription.GetFormattedText() + "[/font_size][/center]");
				return;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x001FCFE3 File Offset: 0x001FB1E3
		public void ShowUpgradePreview()
		{
			this.UpdateVisuals(this.DisplayingPile, CardPreviewMode.Upgrade);
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x001FCFF4 File Offset: 0x001FB1F4
		private void UpdateEnchantmentVisuals()
		{
			if (this.Model == null)
			{
				throw new InvalidOperationException("Cannot show enchantment with no model.");
			}
			EnchantmentModel enchantment = this.Model.Enchantment;
			if (enchantment != null)
			{
				this._enchantmentTab.Visible = true;
				this._enchantmentIcon.Texture = enchantment.Icon;
				this._enchantmentLabel.SetTextAutoSize(enchantment.DisplayAmount.ToString());
				this._enchantmentLabel.Visible = enchantment.ShowAmount;
				this.SetEnchantmentStatus(enchantment.Status);
			}
			else
			{
				this._enchantmentTab.Visible = false;
			}
			if (this.Model.HasStarCostX || this.Model.CurrentStarCost >= 0)
			{
				this._enchantmentTab.Position = this._defaultEnchantmentPosition;
				return;
			}
			this._enchantmentTab.Position = this._defaultEnchantmentPosition + Vector2.Up * 45f;
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x001FD0D8 File Offset: 0x001FB2D8
		private void OnEnchantmentStatusChanged()
		{
			CardModel model = this.Model;
			EnchantmentStatus? enchantmentStatus;
			if (model == null)
			{
				enchantmentStatus = null;
			}
			else
			{
				EnchantmentModel enchantment = model.Enchantment;
				enchantmentStatus = ((enchantment != null) ? new EnchantmentStatus?(enchantment.Status) : null);
			}
			EnchantmentStatus? enchantmentStatus2 = enchantmentStatus;
			this.SetEnchantmentStatus(enchantmentStatus2.GetValueOrDefault(EnchantmentStatus.Disabled));
		}

		/// <summary>
		/// Used when an Enchantment is temporarily disabled on a card. This change is purely visual.
		/// For instance, an Enchantment which only triggers once per turn or requires a condition to activate.
		/// </summary>
		// Token: 0x060041E4 RID: 16868 RVA: 0x001FD128 File Offset: 0x001FB328
		private void SetEnchantmentStatus(EnchantmentStatus status)
		{
			if (status == EnchantmentStatus.Disabled)
			{
				this._enchantmentTab.Modulate = new Color(1f, 1f, 1f, 0.9f);
				ShaderMaterial shaderMaterial = (ShaderMaterial)this._enchantmentTab.Material;
				shaderMaterial.SetShaderParameter(NCard._h, 0.25);
				shaderMaterial.SetShaderParameter(NCard._s, 0.1);
				shaderMaterial.SetShaderParameter(NCard._v, 0.6);
				this._enchantmentIcon.UseParentMaterial = true;
				this._enchantmentLabel.SelfModulate = StsColors.gray;
				return;
			}
			this._enchantmentTab.Modulate = Colors.White;
			ShaderMaterial shaderMaterial2 = (ShaderMaterial)this._enchantmentTab.Material;
			shaderMaterial2.SetShaderParameter(NCard._h, 0.25);
			shaderMaterial2.SetShaderParameter(NCard._s, 0.4);
			shaderMaterial2.SetShaderParameter(NCard._v, 0.6);
			this._enchantmentIcon.UseParentMaterial = false;
			this._enchantmentLabel.SelfModulate = Colors.White;
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x001FD264 File Offset: 0x001FB464
		private void UpdateEnergyCostVisuals(PileType pileType)
		{
			if (this.Visibility != ModelVisibility.Visible)
			{
				this._energyLabel.SetTextAutoSize("?");
				this._energyIcon.Visible = true;
				this._energyLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, StsColors.cream);
				this._energyLabel.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, this.Model.Pool.EnergyOutlineColor);
				return;
			}
			if (this.Model.EnergyCost.CostsX)
			{
				this._energyLabel.SetTextAutoSize("X");
				this._energyIcon.Visible = true;
			}
			else
			{
				int withModifiers = this.Model.EnergyCost.GetWithModifiers(CostModifiers.All);
				this._energyLabel.SetTextAutoSize(withModifiers.ToString());
				this._energyIcon.Visible = withModifiers >= 0;
			}
			this.UpdateEnergyCostColor(pileType);
			UnplayableReason unplayableReason;
			AbstractModel abstractModel;
			if (pileType == PileType.Hand && !this.Model.CanPlay(out unplayableReason, out abstractModel))
			{
				this._unplayableEnergyIcon.Visible = !unplayableReason.HasResourceCostReason();
				return;
			}
			this._unplayableEnergyIcon.Visible = false;
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x001FD36F File Offset: 0x001FB56F
		public void SetPretendCardCanBePlayed(bool pretendCardCanBePlayed)
		{
			this._pretendCardCanBePlayed = pretendCardCanBePlayed;
			this.UpdateEnergyCostVisuals(this.DisplayingPile);
			this.UpdateStarCostVisuals(this.DisplayingPile);
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x001FD390 File Offset: 0x001FB590
		public void SetForceUnpoweredPreview(bool forceUnpoweredPreview)
		{
			this._forceUnpoweredPreview = forceUnpoweredPreview;
		}

		/// <summary>
		/// Update the color of the card's energy cost.
		/// See <see cref="M:MegaCrit.Sts2.Core.Helpers.Models.CardCostHelper.GetEnergyCostColor(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Combat.ICombatState)" /> for details on the rules when the card is in the player's
		/// hand.
		/// WARNING: If you make a change to this method, you should probably make a similar change to
		/// <see cref="M:MegaCrit.Sts2.Core.Nodes.Cards.NCard.UpdateStarCostColor(MegaCrit.Sts2.Core.Entities.Cards.PileType)" />, or write a comment explaining why the two methods are different.
		/// </summary>
		/// <param name="pileType">The pile that the card is being viewed in. See UpdateText() for why we need this.</param>
		// Token: 0x060041E8 RID: 16872 RVA: 0x001FD39C File Offset: 0x001FB59C
		private void UpdateEnergyCostColor(PileType pileType)
		{
			Color color = StsColors.cream;
			Color color2 = this.Model.Pool.EnergyOutlineColor;
			CardEnergyCost energyCost = this.Model.EnergyCost;
			if (energyCost != null && !energyCost.CostsX && energyCost.WasJustUpgraded)
			{
				color = StsColors.green;
				color2 = StsColors.energyGreenOutline;
			}
			else if (pileType == PileType.Hand)
			{
				CardCostColor energyCostColor = CardCostHelper.GetEnergyCostColor(this.Model, this.Model.CombatState);
				color = NCard.GetCostTextColorInHand(energyCostColor, this._pretendCardCanBePlayed, color);
				color2 = NCard.GetCostOutlineColorInHand(energyCostColor, this._pretendCardCanBePlayed, color2);
			}
			this._energyLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, color);
			this._energyLabel.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, color2);
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x001FD448 File Offset: 0x001FB648
		private void UpdateStarCostVisuals(PileType pileType)
		{
			if (this.Visibility != ModelVisibility.Visible)
			{
				this._starLabel.SetTextAutoSize(string.Empty);
				this._starIcon.Visible = false;
				this._starLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, StsColors.cream);
				this._starLabel.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, this.Model.Pool.EnergyOutlineColor);
				return;
			}
			if (this.Model.HasStarCostX)
			{
				this._starLabel.SetTextAutoSize("X");
				this._starIcon.Visible = true;
			}
			else
			{
				this._starLabel.SetTextAutoSize(this.Model.GetStarCostWithModifiers().ToString());
				this._starIcon.Visible = this.Model.GetStarCostWithModifiers() >= 0;
			}
			this.UpdateStarCostColor(pileType);
			UnplayableReason unplayableReason;
			AbstractModel abstractModel;
			if (pileType == PileType.Hand && !this.Model.CanPlay(out unplayableReason, out abstractModel))
			{
				this._unplayableStarIcon.Visible = !unplayableReason.HasResourceCostReason();
				return;
			}
			this._unplayableStarIcon.Visible = false;
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x001FD554 File Offset: 0x001FB754
		private void UpdateStarCostText(int cost)
		{
			if (this.Model.HasStarCostX)
			{
				this._starLabel.SetTextAutoSize("X");
				this._starIcon.Visible = true;
				return;
			}
			if (cost >= 0)
			{
				this._starLabel.SetTextAutoSize(cost.ToString());
				this._starIcon.Visible = true;
				return;
			}
			this._starIcon.Visible = false;
		}

		/// <summary>
		/// Update the color of the card's star cost.
		/// See <see cref="M:MegaCrit.Sts2.Core.Helpers.Models.CardCostHelper.GetStarCostColor(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Combat.ICombatState)" /> for details on the rules when the card is in the player's
		/// hand.
		/// WARNING: If you make a change to this method, you should probably make a similar change to
		/// <see cref="M:MegaCrit.Sts2.Core.Nodes.Cards.NCard.UpdateStarCostColor(MegaCrit.Sts2.Core.Entities.Cards.PileType)" />, or write a comment explaining why the two methods are different.
		/// </summary>
		/// <param name="pileType">The pile that the card is being viewed in. See UpdateText() for why we need this.</param>
		// Token: 0x060041EB RID: 16875 RVA: 0x001FD5BC File Offset: 0x001FB7BC
		private void UpdateStarCostColor(PileType pileType)
		{
			Color color = StsColors.cream;
			Color color2 = StsColors.defaultStarCostOutline;
			if (!this.Model.HasStarCostX && this.Model.WasStarCostJustUpgraded)
			{
				color = StsColors.green;
				color2 = StsColors.energyGreenOutline;
			}
			else if (pileType == PileType.Hand)
			{
				CardCostColor starCostColor = CardCostHelper.GetStarCostColor(this.Model, this.Model.CombatState);
				color = NCard.GetCostTextColorInHand(starCostColor, this._pretendCardCanBePlayed, color);
				color2 = NCard.GetCostOutlineColorInHand(starCostColor, this._pretendCardCanBePlayed, color2);
			}
			this._starLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, color);
			this._starLabel.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, color2);
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x001FD658 File Offset: 0x001FB858
		private static Color GetCostTextColorInHand(CardCostColor costColor, bool pretendCardCanBePlayed, Color defaultColor)
		{
			Color color;
			switch (costColor)
			{
			case CardCostColor.Unmodified:
				color = defaultColor;
				break;
			case CardCostColor.Increased:
				color = StsColors.energyBlue;
				break;
			case CardCostColor.Decreased:
				color = StsColors.green;
				break;
			case CardCostColor.InsufficientResources:
				color = (pretendCardCanBePlayed ? defaultColor : StsColors.red);
				break;
			default:
				throw new ArgumentOutOfRangeException("costColor", costColor, null);
			}
			return color;
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x001FD6B4 File Offset: 0x001FB8B4
		private static Color GetCostOutlineColorInHand(CardCostColor costColor, bool pretendCardCanBePlayed, Color defaultColor)
		{
			Color color;
			switch (costColor)
			{
			case CardCostColor.Unmodified:
				color = defaultColor;
				break;
			case CardCostColor.Increased:
				color = StsColors.energyBlueOutline;
				break;
			case CardCostColor.Decreased:
				color = StsColors.energyGreenOutline;
				break;
			case CardCostColor.InsufficientResources:
				color = (pretendCardCanBePlayed ? defaultColor : StsColors.unplayableEnergyCostOutline);
				break;
			default:
				throw new ArgumentOutOfRangeException("costColor", costColor, null);
			}
			return color;
		}

		/// <summary>
		/// Animates the card cost texts between random costs until settling on the real cost.
		/// Used for content that randomizes card costs (ie snecko eye)
		/// </summary>
		// Token: 0x060041EE RID: 16878 RVA: 0x001FD710 File Offset: 0x001FB910
		public void PlayRandomizeCostAnim()
		{
			Tween randomizeCostTween = this.RandomizeCostTween;
			if (randomizeCostTween != null)
			{
				randomizeCostTween.Kill();
			}
			this.RandomizeCostTween = base.CreateTween();
			float offset = Rng.Chaotic.NextFloat(10f);
			this.RandomizeCostTween.TweenMethod(Callable.From<float>(delegate(float t)
			{
				int num = (int)(offset + t) % 8;
				if (num > 3)
				{
					this._energyLabel.SetTextAutoSize("?");
					return;
				}
				this._energyLabel.SetTextAutoSize((t % 4f).ToString());
			}), 0, 50, (double)Rng.Chaotic.NextFloat(0.4f, 0.6f)).SetEase(1L).SetTrans(1L);
			this.RandomizeCostTween.Connect(Tween.SignalName.Finished, Callable.From(delegate
			{
				if (this.Model != null)
				{
					this.UpdateEnergyCostVisuals(this.DisplayingPile);
				}
			}), 4U);
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x001FD7CC File Offset: 0x001FB9CC
		private void Reload()
		{
			if (!base.IsNodeReady())
			{
				return;
			}
			if (this.Model == null)
			{
				return;
			}
			if (OS.HasFeature("editor"))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(NCard));
				defaultInterpolatedStringHandler.AppendLiteral("-");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this.Model.Id);
				base.Name = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			this._energyIcon.Texture = this.Model.EnergyIcon;
			this.UpdateTypePlaque();
			bool flag = this.Model.Rarity == CardRarity.Ancient;
			this._portraitBorder.Visible = !flag;
			this._portrait.Visible = !flag;
			this._frame.Visible = !flag;
			this._ancientPortrait.Visible = flag;
			this._ancientBorderGlassOverlay.Visible = flag;
			this._ancientBorder.Visible = flag;
			this._ancientTextBg.Visible = flag;
			this._ancientBanner.Visible = flag;
			this._banner.Visible = !flag;
			this._lock.Visible = this.Visibility == ModelVisibility.Locked;
			if (this.Visibility != ModelVisibility.Visible)
			{
				if (this._portraitBlurMaterial == null)
				{
					this._portraitBlurMaterial = PreloadManager.Cache.GetMaterial("res://scenes/cards/card_portrait_blur_material.tres");
				}
				if (flag)
				{
					if (this._canvasGroupMaskBlurMaterial == null)
					{
						this._canvasGroupMaskBlurMaterial = PreloadManager.Cache.GetMaterial("res://scenes/cards/card_canvas_group_mask_blur_material.tres");
					}
					this._portraitCanvasGroup.Material = this._canvasGroupMaskBlurMaterial;
				}
				else
				{
					if (this._canvasGroupBlurMaterial == null)
					{
						this._canvasGroupBlurMaterial = PreloadManager.Cache.GetMaterial("res://scenes/cards/card_canvas_group_blur_material.tres");
					}
					this._portraitCanvasGroup.Material = this._canvasGroupBlurMaterial;
				}
				this._portrait.Material = this._portraitBlurMaterial;
				this._ancientPortrait.Material = this._portraitBlurMaterial;
			}
			else
			{
				if (flag)
				{
					if (this._canvasGroupMaskMaterial == null)
					{
						this._canvasGroupMaskMaterial = PreloadManager.Cache.GetMaterial("res://scenes/cards/card_canvas_group_mask_material.tres");
					}
					this._portraitCanvasGroup.Material = this._canvasGroupMaskMaterial;
				}
				else
				{
					this._portraitCanvasGroup.Material = null;
				}
				this._portrait.Material = null;
				this._ancientPortrait.Material = null;
			}
			this.UpdatePortrait();
			this._frame.Material = this.Model.FrameMaterial;
			this.ReloadOverlay();
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x001FDA20 File Offset: 0x001FBC20
		private void UpdatePortrait()
		{
			if (this.Model == null)
			{
				return;
			}
			Texture2D portrait = this.Model.Portrait;
			if (this.Model.Rarity != CardRarity.Ancient)
			{
				this._portrait.Texture = portrait;
				this._portraitBorder.Texture = this.Model.PortraitBorder;
				this._portraitBorder.Material = this.Model.BannerMaterial;
				this._frame.Texture = this.Model.Frame;
				this._banner.Material = this.Model.BannerMaterial;
				this._banner.Texture = this.Model.BannerTexture;
				return;
			}
			this._ancientBorder.Texture = this.Model.AncientBorder;
			this._ancientTextBg.Texture = this.Model.AncientTextBg;
			this._ancientPortrait.Texture = portrait;
			this._banner.Material = null;
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x001FDB10 File Offset: 0x001FBD10
		private void UpdateTypePlaque()
		{
			this._typeLabel.SetTextAutoSize(this.Model.Type.ToLocString().GetFormattedText());
			Material bannerMaterial = this.Model.BannerMaterial;
			if (this._typePlaque.Material != bannerMaterial)
			{
				this._typePlaque.Material = this.Model.BannerMaterial;
			}
			Callable.From(new Action(this.UpdateTypePlaqueSizeAndPosition)).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x001FDB8C File Offset: 0x001FBD8C
		private void UpdateTypePlaqueSizeAndPosition()
		{
			float num = this._typePlaque.Position.X + this._typePlaque.Size.X * 0.5f;
			Control typePlaque = this._typePlaque;
			Vector2 vector = this._typePlaque.Size;
			vector.X = Mathf.Max(this._typeLabel.Size.X + 17f, 61f);
			typePlaque.Size = vector;
			Control typePlaque2 = this._typePlaque;
			vector = this._typePlaque.Position;
			vector.X = num - this._typePlaque.Size.X * 0.5f;
			typePlaque2.Position = vector;
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x001FDC38 File Offset: 0x001FBE38
		private void UpdateTitleLabel()
		{
			string text;
			Color color;
			Color color2;
			if (this.Visibility == ModelVisibility.NotSeen)
			{
				text = this._unknownTitle.GetFormattedText();
				color = StsColors.cream;
				color2 = this.GetTitleLabelOutlineColor();
			}
			else if (this.Visibility == ModelVisibility.Locked)
			{
				text = this._lockedTitle.GetFormattedText();
				color = StsColors.cream;
				color2 = this.GetTitleLabelOutlineColor();
			}
			else if (this.Model.CurrentUpgradeLevel == 0)
			{
				text = this.Model.Title;
				color = StsColors.cream;
				color2 = this.GetTitleLabelOutlineColor();
			}
			else
			{
				text = this.Model.Title;
				color = StsColors.green;
				color2 = StsColors.cardTitleOutlineSpecial;
			}
			this._titleLabel.SetTextAutoSize(text);
			this._titleLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, color);
			this._titleLabel.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, color2);
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x001FDCFC File Offset: 0x001FBEFC
		private Color GetTitleLabelOutlineColor()
		{
			Color color;
			switch (this._model.Rarity)
			{
			case CardRarity.None:
			case CardRarity.Basic:
			case CardRarity.Common:
			case CardRarity.Token:
				color = StsColors.cardTitleOutlineCommon;
				break;
			case CardRarity.Uncommon:
				color = StsColors.cardTitleOutlineUncommon;
				break;
			case CardRarity.Rare:
				color = StsColors.cardTitleOutlineRare;
				break;
			case CardRarity.Ancient:
				color = StsColors.cardTitleOutlineCommon;
				break;
			case CardRarity.Event:
				color = StsColors.cardTitleOutlineSpecial;
				break;
			case CardRarity.Status:
				color = StsColors.cardTitleOutlineStatus;
				break;
			case CardRarity.Curse:
				color = StsColors.cardTitleOutlineCurse;
				break;
			case CardRarity.Quest:
				color = StsColors.cardTitleOutlineQuest;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return color;
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x001FDD90 File Offset: 0x001FBF90
		private void ReloadOverlay()
		{
			if (this._cardOverlay != null)
			{
				this.OverlayContainer.RemoveChildSafely(this._cardOverlay);
				this._cardOverlay.QueueFreeSafely();
				this._cardOverlay = null;
			}
			if (this.Model == null)
			{
				return;
			}
			if (this.Model.Rarity == CardRarity.Ancient)
			{
				this._frame.Visible = false;
				this._ancientBorder.Visible = true;
			}
			AfflictionModel affliction = this.Model.Affliction;
			if (affliction != null && affliction.HasOverlay)
			{
				this._cardOverlay = this.Model.Affliction.CreateOverlay();
			}
			else if (this.Model.HasBuiltInOverlay)
			{
				this._cardOverlay = this.Model.CreateOverlay();
			}
			if (this._cardOverlay != null)
			{
				this.OverlayContainer.AddChildSafely(this._cardOverlay);
			}
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x001FDE5D File Offset: 0x001FC05D
		private void OnAfflictionChanged()
		{
			this.ReloadOverlay();
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x001FDE65 File Offset: 0x001FC065
		private void OnEnchantmentChanged()
		{
			this.UnsubscribeFromEnchantment(this._subscribedEnchantment);
			CardModel model = this.Model;
			this.SubscribeToEnchantment((model != null) ? model.Enchantment : null);
			this.UpdateEnchantmentVisuals();
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x001FDE91 File Offset: 0x001FC091
		private string GetTitleText()
		{
			return this._titleLabel.Text;
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x001FDEA0 File Offset: 0x001FC0A0
		public void ActivateRewardScreenGlow()
		{
			if (this._model.Rarity == CardRarity.Rare)
			{
				this._sparkles.Visible = true;
				this._rareGlow = NCardRareGlow.Create();
				if (this._rareGlow != null)
				{
					this.Body.AddChildSafely(this._rareGlow);
					this.Body.MoveChildSafely(this._rareGlow, 1);
				}
				this.CardHighlight.Modulate = NCardHighlight.gold;
				return;
			}
			if (this._model.Rarity == CardRarity.Uncommon)
			{
				this._uncommonGlow = NCardUncommonGlow.Create();
				if (this._uncommonGlow != null)
				{
					this.Body.AddChildSafely(this._uncommonGlow);
					this.Body.MoveChildSafely(this._uncommonGlow, 1);
				}
				this.CardHighlight.Modulate = NCardHighlight.playableColor;
			}
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x001FDF64 File Offset: 0x001FC164
		public void FlashRelicOnCard(RelicModel relic)
		{
			NRelicFlashVfx nrelicFlashVfx = NRelicFlashVfx.Create(relic);
			this.Body.AddChildSafely(nrelicFlashVfx);
			nrelicFlashVfx.Scale = Vector2.One * 2f;
			nrelicFlashVfx.Position = base.Size * 0.5f;
			this._flashVfx.Add(nrelicFlashVfx);
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x001FDFBB File Offset: 0x001FC1BB
		public void KillRarityGlow()
		{
			NCardRareGlow rareGlow = this._rareGlow;
			if (rareGlow != null)
			{
				rareGlow.Kill();
			}
			NCardUncommonGlow uncommonGlow = this._uncommonGlow;
			if (uncommonGlow == null)
			{
				return;
			}
			uncommonGlow.Kill();
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x001FDFE0 File Offset: 0x001FC1E0
		public async Task AnimMultiCardPlay()
		{
			if (GodotObject.IsInstanceValid(this))
			{
				Vector2 scale = base.Scale;
				float y = base.Position.Y;
				Tween playPileTween = this.PlayPileTween;
				if (playPileTween != null)
				{
					playPileTween.FastForwardToCompletion();
				}
				this.PlayPileTween = base.CreateTween().SetParallel(true);
				this.PlayPileTween.TweenProperty(this, "modulate", StsColors.transparentBlack, 0.2);
				this.PlayPileTween.Chain();
				this.PlayPileTween.TweenInterval((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 0.1 : 0.2);
				this.PlayPileTween.TweenCallback(Callable.From(delegate
				{
					this.UpdateVisuals(PileType.Play, CardPreviewMode.Normal);
				}));
				this.PlayPileTween.Chain();
				this.PlayPileTween.TweenProperty(this, "modulate", Colors.White, 0.2).SetEase(1L).SetTrans(7L);
				this.PlayPileTween.TweenProperty(this, "scale", scale, 0.25).SetEase(1L).SetTrans(7L)
					.From(scale * 0.5f);
				this.PlayPileTween.TweenProperty(this, "position:y", y, 0.25).SetEase(1L).SetTrans(10L)
					.From(y + 250f);
				await Cmd.CustomScaledWait(0.4f, 0.5f, false, this._cts.Token);
			}
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x001FE024 File Offset: 0x001FC224
		public void AnimCardToPlayPile()
		{
			Vector2 targetPosition = PileType.Play.GetTargetPosition(this);
			Tween playPileTween = this.PlayPileTween;
			if (playPileTween != null)
			{
				playPileTween.FastForwardToCompletion();
			}
			this.PlayPileTween = base.CreateTween().SetParallel(true);
			this.PlayPileTween.TweenProperty(this, "position", targetPosition, 0.2).SetEase(1L).SetTrans(7L);
			this.PlayPileTween.TweenProperty(this, "scale", Vector2.One * 0.8f, 0.2).SetEase(1L).SetTrans(7L);
		}

		// Token: 0x060041FE RID: 16894 RVA: 0x001FE0D4 File Offset: 0x001FC2D4
		public void OnReturnedFromPool()
		{
			if (base.IsNodeReady())
			{
				base.Position = Vector2.Zero;
				base.Rotation = 0f;
				base.Scale = Vector2.One;
				base.Modulate = Colors.White;
				base.Visible = true;
				this.Body.Visible = true;
				this.Body.Modulate = Colors.White;
				this.Body.Scale = Vector2.One;
				this._visibility = ModelVisibility.Visible;
				this.CardHighlight.Modulate = NCardHighlight.playableColor;
				this.CardHighlight.AnimHideInstantly();
				this._sparkles.Visible = false;
				this._enchantmentTab.Visible = false;
				this._enchantmentVfxOverride.Visible = false;
				this._model = null;
				this._previewTarget = null;
				this._pretendCardCanBePlayed = false;
				this._forceUnpoweredPreview = false;
				this._portrait.Material = null;
				this._ancientPortrait.Material = null;
				this._portraitCanvasGroup.Material = null;
				this.DisplayingPile = PileType.None;
				this.ModelChanged = null;
			}
		}

		// Token: 0x060041FF RID: 16895 RVA: 0x001FE1E0 File Offset: 0x001FC3E0
		public void OnFreedToPool()
		{
			NCardRareGlow rareGlow = this._rareGlow;
			if (rareGlow != null)
			{
				rareGlow.QueueFreeSafely();
			}
			this._rareGlow = null;
			NCardUncommonGlow uncommonGlow = this._uncommonGlow;
			if (uncommonGlow != null)
			{
				uncommonGlow.QueueFreeSafely();
			}
			this._uncommonGlow = null;
			Control cardOverlay = this._cardOverlay;
			if (cardOverlay != null)
			{
				cardOverlay.QueueFreeSafely();
			}
			this._cardOverlay = null;
			this._portrait.Texture = null;
			this._enchantmentVfxOverride.Texture = null;
			foreach (NRelicFlashVfx nrelicFlashVfx in this._flashVfx)
			{
				if (nrelicFlashVfx.IsValid())
				{
					nrelicFlashVfx.QueueFreeSafely();
				}
			}
			this._flashVfx.Clear();
			Tween playPileTween = this.PlayPileTween;
			if (playPileTween != null)
			{
				playPileTween.Kill();
			}
			this.PlayPileTween = null;
			Tween randomizeCostTween = this.RandomizeCostTween;
			if (randomizeCostTween != null)
			{
				randomizeCostTween.Kill();
			}
			this.RandomizeCostTween = null;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004200 RID: 16896 RVA: 0x001FE2D8 File Offset: 0x001FC4D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(36)
			{
				new MethodInfo(NCard.MethodName.OnInstantiated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.InitPool, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NCard.MethodName.GetCurrentSize, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.UpdateVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "pileType", 0L, "", 6L, false),
					new PropertyInfo(2L, "previewMode", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCard.MethodName.ShowUpgradePreview, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.UpdateEnchantmentVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.OnEnchantmentStatusChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.SetEnchantmentStatus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "status", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCard.MethodName.UpdateEnergyCostVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "pileType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCard.MethodName.SetPretendCardCanBePlayed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "pretendCardCanBePlayed", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCard.MethodName.SetForceUnpoweredPreview, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "forceUnpoweredPreview", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCard.MethodName.UpdateEnergyCostColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "pileType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCard.MethodName.UpdateStarCostVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "pileType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCard.MethodName.UpdateStarCostText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "cost", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCard.MethodName.UpdateStarCostColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "pileType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCard.MethodName.GetCostTextColorInHand, new PropertyInfo(20L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "costColor", 0L, "", 6L, false),
					new PropertyInfo(1L, "pretendCardCanBePlayed", 0L, "", 6L, false),
					new PropertyInfo(20L, "defaultColor", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCard.MethodName.GetCostOutlineColorInHand, new PropertyInfo(20L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "costColor", 0L, "", 6L, false),
					new PropertyInfo(1L, "pretendCardCanBePlayed", 0L, "", 6L, false),
					new PropertyInfo(20L, "defaultColor", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCard.MethodName.PlayRandomizeCostAnim, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.Reload, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.UpdatePortrait, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.UpdateTypePlaque, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.UpdateTypePlaqueSizeAndPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.UpdateTitleLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.GetTitleLabelOutlineColor, new PropertyInfo(20L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.ReloadOverlay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.OnAfflictionChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.OnEnchantmentChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.GetTitleText, new PropertyInfo(4L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.ActivateRewardScreenGlow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.KillRarityGlow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.AnimCardToPlayPile, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.OnReturnedFromPool, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCard.MethodName.OnFreedToPool, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004201 RID: 16897 RVA: 0x001FEBD0 File Offset: 0x001FCDD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCard.MethodName.OnInstantiated && args.Count == 0)
			{
				this.OnInstantiated();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.InitPool && args.Count == 0)
			{
				NCard.InitPool();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.GetCurrentSize && args.Count == 0)
			{
				Vector2 currentSize = this.GetCurrentSize();
				ret = VariantUtils.CreateFrom<Vector2>(ref currentSize);
				return true;
			}
			if ((ref method) == NCard.MethodName.UpdateVisuals && args.Count == 2)
			{
				this.UpdateVisuals(VariantUtils.ConvertTo<PileType>(args[0]), VariantUtils.ConvertTo<CardPreviewMode>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.ShowUpgradePreview && args.Count == 0)
			{
				this.ShowUpgradePreview();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.UpdateEnchantmentVisuals && args.Count == 0)
			{
				this.UpdateEnchantmentVisuals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.OnEnchantmentStatusChanged && args.Count == 0)
			{
				this.OnEnchantmentStatusChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.SetEnchantmentStatus && args.Count == 1)
			{
				this.SetEnchantmentStatus(VariantUtils.ConvertTo<EnchantmentStatus>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.UpdateEnergyCostVisuals && args.Count == 1)
			{
				this.UpdateEnergyCostVisuals(VariantUtils.ConvertTo<PileType>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.SetPretendCardCanBePlayed && args.Count == 1)
			{
				this.SetPretendCardCanBePlayed(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.SetForceUnpoweredPreview && args.Count == 1)
			{
				this.SetForceUnpoweredPreview(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.UpdateEnergyCostColor && args.Count == 1)
			{
				this.UpdateEnergyCostColor(VariantUtils.ConvertTo<PileType>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.UpdateStarCostVisuals && args.Count == 1)
			{
				this.UpdateStarCostVisuals(VariantUtils.ConvertTo<PileType>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.UpdateStarCostText && args.Count == 1)
			{
				this.UpdateStarCostText(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.UpdateStarCostColor && args.Count == 1)
			{
				this.UpdateStarCostColor(VariantUtils.ConvertTo<PileType>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.GetCostTextColorInHand && args.Count == 3)
			{
				Color costTextColorInHand = NCard.GetCostTextColorInHand(VariantUtils.ConvertTo<CardCostColor>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<Color>(ref costTextColorInHand);
				return true;
			}
			if ((ref method) == NCard.MethodName.GetCostOutlineColorInHand && args.Count == 3)
			{
				Color costOutlineColorInHand = NCard.GetCostOutlineColorInHand(VariantUtils.ConvertTo<CardCostColor>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<Color>(ref costOutlineColorInHand);
				return true;
			}
			if ((ref method) == NCard.MethodName.PlayRandomizeCostAnim && args.Count == 0)
			{
				this.PlayRandomizeCostAnim();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.Reload && args.Count == 0)
			{
				this.Reload();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.UpdatePortrait && args.Count == 0)
			{
				this.UpdatePortrait();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.UpdateTypePlaque && args.Count == 0)
			{
				this.UpdateTypePlaque();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.UpdateTypePlaqueSizeAndPosition && args.Count == 0)
			{
				this.UpdateTypePlaqueSizeAndPosition();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.UpdateTitleLabel && args.Count == 0)
			{
				this.UpdateTitleLabel();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.GetTitleLabelOutlineColor && args.Count == 0)
			{
				Color titleLabelOutlineColor = this.GetTitleLabelOutlineColor();
				ret = VariantUtils.CreateFrom<Color>(ref titleLabelOutlineColor);
				return true;
			}
			if ((ref method) == NCard.MethodName.ReloadOverlay && args.Count == 0)
			{
				this.ReloadOverlay();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.OnAfflictionChanged && args.Count == 0)
			{
				this.OnAfflictionChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.OnEnchantmentChanged && args.Count == 0)
			{
				this.OnEnchantmentChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.GetTitleText && args.Count == 0)
			{
				string titleText = this.GetTitleText();
				ret = VariantUtils.CreateFrom<string>(ref titleText);
				return true;
			}
			if ((ref method) == NCard.MethodName.ActivateRewardScreenGlow && args.Count == 0)
			{
				this.ActivateRewardScreenGlow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.KillRarityGlow && args.Count == 0)
			{
				this.KillRarityGlow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.AnimCardToPlayPile && args.Count == 0)
			{
				this.AnimCardToPlayPile();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.OnReturnedFromPool && args.Count == 0)
			{
				this.OnReturnedFromPool();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.OnFreedToPool && args.Count == 0)
			{
				this.OnFreedToPool();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x001FF218 File Offset: 0x001FD418
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCard.MethodName.InitPool && args.Count == 0)
			{
				NCard.InitPool();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCard.MethodName.GetCostTextColorInHand && args.Count == 3)
			{
				Color costTextColorInHand = NCard.GetCostTextColorInHand(VariantUtils.ConvertTo<CardCostColor>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<Color>(ref costTextColorInHand);
				return true;
			}
			if ((ref method) == NCard.MethodName.GetCostOutlineColorInHand && args.Count == 3)
			{
				Color costOutlineColorInHand = NCard.GetCostOutlineColorInHand(VariantUtils.ConvertTo<CardCostColor>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<Color>(ref costOutlineColorInHand);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06004203 RID: 16899 RVA: 0x001FF2F8 File Offset: 0x001FD4F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCard.MethodName.OnInstantiated || (ref method) == NCard.MethodName._Ready || (ref method) == NCard.MethodName._EnterTree || (ref method) == NCard.MethodName._ExitTree || (ref method) == NCard.MethodName.InitPool || (ref method) == NCard.MethodName.GetCurrentSize || (ref method) == NCard.MethodName.UpdateVisuals || (ref method) == NCard.MethodName.ShowUpgradePreview || (ref method) == NCard.MethodName.UpdateEnchantmentVisuals || (ref method) == NCard.MethodName.OnEnchantmentStatusChanged || (ref method) == NCard.MethodName.SetEnchantmentStatus || (ref method) == NCard.MethodName.UpdateEnergyCostVisuals || (ref method) == NCard.MethodName.SetPretendCardCanBePlayed || (ref method) == NCard.MethodName.SetForceUnpoweredPreview || (ref method) == NCard.MethodName.UpdateEnergyCostColor || (ref method) == NCard.MethodName.UpdateStarCostVisuals || (ref method) == NCard.MethodName.UpdateStarCostText || (ref method) == NCard.MethodName.UpdateStarCostColor || (ref method) == NCard.MethodName.GetCostTextColorInHand || (ref method) == NCard.MethodName.GetCostOutlineColorInHand || (ref method) == NCard.MethodName.PlayRandomizeCostAnim || (ref method) == NCard.MethodName.Reload || (ref method) == NCard.MethodName.UpdatePortrait || (ref method) == NCard.MethodName.UpdateTypePlaque || (ref method) == NCard.MethodName.UpdateTypePlaqueSizeAndPosition || (ref method) == NCard.MethodName.UpdateTitleLabel || (ref method) == NCard.MethodName.GetTitleLabelOutlineColor || (ref method) == NCard.MethodName.ReloadOverlay || (ref method) == NCard.MethodName.OnAfflictionChanged || (ref method) == NCard.MethodName.OnEnchantmentChanged || (ref method) == NCard.MethodName.GetTitleText || (ref method) == NCard.MethodName.ActivateRewardScreenGlow || (ref method) == NCard.MethodName.KillRarityGlow || (ref method) == NCard.MethodName.AnimCardToPlayPile || (ref method) == NCard.MethodName.OnReturnedFromPool || (ref method) == NCard.MethodName.OnFreedToPool || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06004204 RID: 16900 RVA: 0x001FF528 File Offset: 0x001FD728
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCard.PropertyName.CardHighlight)
			{
				this.CardHighlight = VariantUtils.ConvertTo<NCardHighlight>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName.Body)
			{
				this.Body = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName.Visibility)
			{
				this.Visibility = VariantUtils.ConvertTo<ModelVisibility>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName.PlayPileTween)
			{
				this.PlayPileTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName.RandomizeCostTween)
			{
				this.RandomizeCostTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName.DisplayingPile)
			{
				this.DisplayingPile = VariantUtils.ConvertTo<PileType>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._titleLabel)
			{
				this._titleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._descriptionLabel)
			{
				this._descriptionLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._ancientPortrait)
			{
				this._ancientPortrait = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._portrait)
			{
				this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._frame)
			{
				this._frame = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._ancientBorderGlassOverlay)
			{
				this._ancientBorderGlassOverlay = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._ancientBorder)
			{
				this._ancientBorder = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._ancientBanner)
			{
				this._ancientBanner = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._ancientTextBg)
			{
				this._ancientTextBg = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._portraitBorder)
			{
				this._portraitBorder = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._banner)
			{
				this._banner = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._lock)
			{
				this._lock = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._typePlaque)
			{
				this._typePlaque = VariantUtils.ConvertTo<NinePatchRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._typeLabel)
			{
				this._typeLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._portraitCanvasGroup)
			{
				this._portraitCanvasGroup = VariantUtils.ConvertTo<CanvasGroup>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._rareGlow)
			{
				this._rareGlow = VariantUtils.ConvertTo<NCardRareGlow>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._uncommonGlow)
			{
				this._uncommonGlow = VariantUtils.ConvertTo<NCardUncommonGlow>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._sparkles)
			{
				this._sparkles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._energyIcon)
			{
				this._energyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._energyLabel)
			{
				this._energyLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._unplayableEnergyIcon)
			{
				this._unplayableEnergyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._starIcon)
			{
				this._starIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._starLabel)
			{
				this._starLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._unplayableStarIcon)
			{
				this._unplayableStarIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._overlayContainer)
			{
				this._overlayContainer = VariantUtils.ConvertTo<Node>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._cardOverlay)
			{
				this._cardOverlay = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._cardVfxContainer)
			{
				this._cardVfxContainer = VariantUtils.ConvertTo<Node>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._enchantmentTab)
			{
				this._enchantmentTab = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._enchantmentVfxOverride)
			{
				this._enchantmentVfxOverride = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._enchantmentIcon)
			{
				this._enchantmentIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._enchantmentLabel)
			{
				this._enchantmentLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._defaultEnchantmentPosition)
			{
				this._defaultEnchantmentPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._pretendCardCanBePlayed)
			{
				this._pretendCardCanBePlayed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._forceUnpoweredPreview)
			{
				this._forceUnpoweredPreview = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._portraitBlurMaterial)
			{
				this._portraitBlurMaterial = VariantUtils.ConvertTo<Material>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._canvasGroupMaskBlurMaterial)
			{
				this._canvasGroupMaskBlurMaterial = VariantUtils.ConvertTo<Material>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._canvasGroupBlurMaterial)
			{
				this._canvasGroupBlurMaterial = VariantUtils.ConvertTo<Material>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._canvasGroupMaskMaterial)
			{
				this._canvasGroupMaskMaterial = VariantUtils.ConvertTo<Material>(ref value);
				return true;
			}
			if ((ref name) == NCard.PropertyName._visibility)
			{
				this._visibility = VariantUtils.ConvertTo<ModelVisibility>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06004205 RID: 16901 RVA: 0x001FF9FC File Offset: 0x001FDBFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCard.PropertyName.CardHighlight)
			{
				NCardHighlight cardHighlight = this.CardHighlight;
				value = VariantUtils.CreateFrom<NCardHighlight>(ref cardHighlight);
				return true;
			}
			if ((ref name) == NCard.PropertyName.Body)
			{
				Control control = this.Body;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCard.PropertyName.Visibility)
			{
				ModelVisibility visibility = this.Visibility;
				value = VariantUtils.CreateFrom<ModelVisibility>(ref visibility);
				return true;
			}
			if ((ref name) == NCard.PropertyName.PlayPileTween)
			{
				Tween tween = this.PlayPileTween;
				value = VariantUtils.CreateFrom<Tween>(ref tween);
				return true;
			}
			if ((ref name) == NCard.PropertyName.RandomizeCostTween)
			{
				Tween tween = this.RandomizeCostTween;
				value = VariantUtils.CreateFrom<Tween>(ref tween);
				return true;
			}
			if ((ref name) == NCard.PropertyName.DisplayingPile)
			{
				PileType displayingPile = this.DisplayingPile;
				value = VariantUtils.CreateFrom<PileType>(ref displayingPile);
				return true;
			}
			if ((ref name) == NCard.PropertyName.EnchantmentTab)
			{
				Control control = this.EnchantmentTab;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCard.PropertyName.EnchantmentVfxOverride)
			{
				TextureRect enchantmentVfxOverride = this.EnchantmentVfxOverride;
				value = VariantUtils.CreateFrom<TextureRect>(ref enchantmentVfxOverride);
				return true;
			}
			if ((ref name) == NCard.PropertyName.OverlayContainer)
			{
				Node node = this.OverlayContainer;
				value = VariantUtils.CreateFrom<Node>(ref node);
				return true;
			}
			if ((ref name) == NCard.PropertyName.CardVfxContainer)
			{
				Node node = this.CardVfxContainer;
				value = VariantUtils.CreateFrom<Node>(ref node);
				return true;
			}
			if ((ref name) == NCard.PropertyName._titleLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._titleLabel);
				return true;
			}
			if ((ref name) == NCard.PropertyName._descriptionLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._descriptionLabel);
				return true;
			}
			if ((ref name) == NCard.PropertyName._ancientPortrait)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._ancientPortrait);
				return true;
			}
			if ((ref name) == NCard.PropertyName._portrait)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
				return true;
			}
			if ((ref name) == NCard.PropertyName._frame)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._frame);
				return true;
			}
			if ((ref name) == NCard.PropertyName._ancientBorderGlassOverlay)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._ancientBorderGlassOverlay);
				return true;
			}
			if ((ref name) == NCard.PropertyName._ancientBorder)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._ancientBorder);
				return true;
			}
			if ((ref name) == NCard.PropertyName._ancientBanner)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._ancientBanner);
				return true;
			}
			if ((ref name) == NCard.PropertyName._ancientTextBg)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._ancientTextBg);
				return true;
			}
			if ((ref name) == NCard.PropertyName._portraitBorder)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._portraitBorder);
				return true;
			}
			if ((ref name) == NCard.PropertyName._banner)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._banner);
				return true;
			}
			if ((ref name) == NCard.PropertyName._lock)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._lock);
				return true;
			}
			if ((ref name) == NCard.PropertyName._typePlaque)
			{
				value = VariantUtils.CreateFrom<NinePatchRect>(ref this._typePlaque);
				return true;
			}
			if ((ref name) == NCard.PropertyName._typeLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._typeLabel);
				return true;
			}
			if ((ref name) == NCard.PropertyName._portraitCanvasGroup)
			{
				value = VariantUtils.CreateFrom<CanvasGroup>(ref this._portraitCanvasGroup);
				return true;
			}
			if ((ref name) == NCard.PropertyName._rareGlow)
			{
				value = VariantUtils.CreateFrom<NCardRareGlow>(ref this._rareGlow);
				return true;
			}
			if ((ref name) == NCard.PropertyName._uncommonGlow)
			{
				value = VariantUtils.CreateFrom<NCardUncommonGlow>(ref this._uncommonGlow);
				return true;
			}
			if ((ref name) == NCard.PropertyName._sparkles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sparkles);
				return true;
			}
			if ((ref name) == NCard.PropertyName._energyIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._energyIcon);
				return true;
			}
			if ((ref name) == NCard.PropertyName._energyLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._energyLabel);
				return true;
			}
			if ((ref name) == NCard.PropertyName._unplayableEnergyIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._unplayableEnergyIcon);
				return true;
			}
			if ((ref name) == NCard.PropertyName._starIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._starIcon);
				return true;
			}
			if ((ref name) == NCard.PropertyName._starLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._starLabel);
				return true;
			}
			if ((ref name) == NCard.PropertyName._unplayableStarIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._unplayableStarIcon);
				return true;
			}
			if ((ref name) == NCard.PropertyName._overlayContainer)
			{
				value = VariantUtils.CreateFrom<Node>(ref this._overlayContainer);
				return true;
			}
			if ((ref name) == NCard.PropertyName._cardOverlay)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardOverlay);
				return true;
			}
			if ((ref name) == NCard.PropertyName._cardVfxContainer)
			{
				value = VariantUtils.CreateFrom<Node>(ref this._cardVfxContainer);
				return true;
			}
			if ((ref name) == NCard.PropertyName._enchantmentTab)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._enchantmentTab);
				return true;
			}
			if ((ref name) == NCard.PropertyName._enchantmentVfxOverride)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._enchantmentVfxOverride);
				return true;
			}
			if ((ref name) == NCard.PropertyName._enchantmentIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._enchantmentIcon);
				return true;
			}
			if ((ref name) == NCard.PropertyName._enchantmentLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._enchantmentLabel);
				return true;
			}
			if ((ref name) == NCard.PropertyName._defaultEnchantmentPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._defaultEnchantmentPosition);
				return true;
			}
			if ((ref name) == NCard.PropertyName._pretendCardCanBePlayed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._pretendCardCanBePlayed);
				return true;
			}
			if ((ref name) == NCard.PropertyName._forceUnpoweredPreview)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._forceUnpoweredPreview);
				return true;
			}
			if ((ref name) == NCard.PropertyName._portraitBlurMaterial)
			{
				value = VariantUtils.CreateFrom<Material>(ref this._portraitBlurMaterial);
				return true;
			}
			if ((ref name) == NCard.PropertyName._canvasGroupMaskBlurMaterial)
			{
				value = VariantUtils.CreateFrom<Material>(ref this._canvasGroupMaskBlurMaterial);
				return true;
			}
			if ((ref name) == NCard.PropertyName._canvasGroupBlurMaterial)
			{
				value = VariantUtils.CreateFrom<Material>(ref this._canvasGroupBlurMaterial);
				return true;
			}
			if ((ref name) == NCard.PropertyName._canvasGroupMaskMaterial)
			{
				value = VariantUtils.CreateFrom<Material>(ref this._canvasGroupMaskMaterial);
				return true;
			}
			if ((ref name) == NCard.PropertyName._visibility)
			{
				value = VariantUtils.CreateFrom<ModelVisibility>(ref this._visibility);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004206 RID: 16902 RVA: 0x00200054 File Offset: 0x001FE254
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCard.PropertyName._titleLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._descriptionLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._ancientPortrait, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._portrait, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._frame, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._ancientBorderGlassOverlay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._ancientBorder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._ancientBanner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._ancientTextBg, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._portraitBorder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._banner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._lock, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._typePlaque, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._typeLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._portraitCanvasGroup, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._rareGlow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._uncommonGlow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._sparkles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._energyIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._energyLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._unplayableEnergyIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._starIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._starLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._unplayableStarIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._overlayContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._cardOverlay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._cardVfxContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._enchantmentTab, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._enchantmentVfxOverride, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._enchantmentIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._enchantmentLabel, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCard.PropertyName._defaultEnchantmentPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName.CardHighlight, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName.Body, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCard.PropertyName._pretendCardCanBePlayed, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCard.PropertyName._forceUnpoweredPreview, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._portraitBlurMaterial, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._canvasGroupMaskBlurMaterial, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._canvasGroupBlurMaterial, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName._canvasGroupMaskMaterial, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCard.PropertyName._visibility, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCard.PropertyName.Visibility, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName.PlayPileTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName.RandomizeCostTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCard.PropertyName.DisplayingPile, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName.EnchantmentTab, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName.EnchantmentVfxOverride, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName.OverlayContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCard.PropertyName.CardVfxContainer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004207 RID: 16903 RVA: 0x002006B4 File Offset: 0x001FE8B4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName cardHighlight = NCard.PropertyName.CardHighlight;
			NCardHighlight cardHighlight2 = this.CardHighlight;
			info.AddProperty(cardHighlight, Variant.From<NCardHighlight>(ref cardHighlight2));
			StringName body = NCard.PropertyName.Body;
			Control body2 = this.Body;
			info.AddProperty(body, Variant.From<Control>(ref body2));
			StringName visibility = NCard.PropertyName.Visibility;
			ModelVisibility visibility2 = this.Visibility;
			info.AddProperty(visibility, Variant.From<ModelVisibility>(ref visibility2));
			StringName playPileTween = NCard.PropertyName.PlayPileTween;
			Tween tween = this.PlayPileTween;
			info.AddProperty(playPileTween, Variant.From<Tween>(ref tween));
			StringName randomizeCostTween = NCard.PropertyName.RandomizeCostTween;
			tween = this.RandomizeCostTween;
			info.AddProperty(randomizeCostTween, Variant.From<Tween>(ref tween));
			StringName displayingPile = NCard.PropertyName.DisplayingPile;
			PileType displayingPile2 = this.DisplayingPile;
			info.AddProperty(displayingPile, Variant.From<PileType>(ref displayingPile2));
			info.AddProperty(NCard.PropertyName._titleLabel, Variant.From<MegaLabel>(ref this._titleLabel));
			info.AddProperty(NCard.PropertyName._descriptionLabel, Variant.From<MegaRichTextLabel>(ref this._descriptionLabel));
			info.AddProperty(NCard.PropertyName._ancientPortrait, Variant.From<TextureRect>(ref this._ancientPortrait));
			info.AddProperty(NCard.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
			info.AddProperty(NCard.PropertyName._frame, Variant.From<TextureRect>(ref this._frame));
			info.AddProperty(NCard.PropertyName._ancientBorderGlassOverlay, Variant.From<TextureRect>(ref this._ancientBorderGlassOverlay));
			info.AddProperty(NCard.PropertyName._ancientBorder, Variant.From<TextureRect>(ref this._ancientBorder));
			info.AddProperty(NCard.PropertyName._ancientBanner, Variant.From<Control>(ref this._ancientBanner));
			info.AddProperty(NCard.PropertyName._ancientTextBg, Variant.From<TextureRect>(ref this._ancientTextBg));
			info.AddProperty(NCard.PropertyName._portraitBorder, Variant.From<TextureRect>(ref this._portraitBorder));
			info.AddProperty(NCard.PropertyName._banner, Variant.From<TextureRect>(ref this._banner));
			info.AddProperty(NCard.PropertyName._lock, Variant.From<TextureRect>(ref this._lock));
			info.AddProperty(NCard.PropertyName._typePlaque, Variant.From<NinePatchRect>(ref this._typePlaque));
			info.AddProperty(NCard.PropertyName._typeLabel, Variant.From<MegaLabel>(ref this._typeLabel));
			info.AddProperty(NCard.PropertyName._portraitCanvasGroup, Variant.From<CanvasGroup>(ref this._portraitCanvasGroup));
			info.AddProperty(NCard.PropertyName._rareGlow, Variant.From<NCardRareGlow>(ref this._rareGlow));
			info.AddProperty(NCard.PropertyName._uncommonGlow, Variant.From<NCardUncommonGlow>(ref this._uncommonGlow));
			info.AddProperty(NCard.PropertyName._sparkles, Variant.From<GpuParticles2D>(ref this._sparkles));
			info.AddProperty(NCard.PropertyName._energyIcon, Variant.From<TextureRect>(ref this._energyIcon));
			info.AddProperty(NCard.PropertyName._energyLabel, Variant.From<MegaLabel>(ref this._energyLabel));
			info.AddProperty(NCard.PropertyName._unplayableEnergyIcon, Variant.From<TextureRect>(ref this._unplayableEnergyIcon));
			info.AddProperty(NCard.PropertyName._starIcon, Variant.From<TextureRect>(ref this._starIcon));
			info.AddProperty(NCard.PropertyName._starLabel, Variant.From<MegaLabel>(ref this._starLabel));
			info.AddProperty(NCard.PropertyName._unplayableStarIcon, Variant.From<TextureRect>(ref this._unplayableStarIcon));
			info.AddProperty(NCard.PropertyName._overlayContainer, Variant.From<Node>(ref this._overlayContainer));
			info.AddProperty(NCard.PropertyName._cardOverlay, Variant.From<Control>(ref this._cardOverlay));
			info.AddProperty(NCard.PropertyName._cardVfxContainer, Variant.From<Node>(ref this._cardVfxContainer));
			info.AddProperty(NCard.PropertyName._enchantmentTab, Variant.From<Control>(ref this._enchantmentTab));
			info.AddProperty(NCard.PropertyName._enchantmentVfxOverride, Variant.From<TextureRect>(ref this._enchantmentVfxOverride));
			info.AddProperty(NCard.PropertyName._enchantmentIcon, Variant.From<TextureRect>(ref this._enchantmentIcon));
			info.AddProperty(NCard.PropertyName._enchantmentLabel, Variant.From<MegaLabel>(ref this._enchantmentLabel));
			info.AddProperty(NCard.PropertyName._defaultEnchantmentPosition, Variant.From<Vector2>(ref this._defaultEnchantmentPosition));
			info.AddProperty(NCard.PropertyName._pretendCardCanBePlayed, Variant.From<bool>(ref this._pretendCardCanBePlayed));
			info.AddProperty(NCard.PropertyName._forceUnpoweredPreview, Variant.From<bool>(ref this._forceUnpoweredPreview));
			info.AddProperty(NCard.PropertyName._portraitBlurMaterial, Variant.From<Material>(ref this._portraitBlurMaterial));
			info.AddProperty(NCard.PropertyName._canvasGroupMaskBlurMaterial, Variant.From<Material>(ref this._canvasGroupMaskBlurMaterial));
			info.AddProperty(NCard.PropertyName._canvasGroupBlurMaterial, Variant.From<Material>(ref this._canvasGroupBlurMaterial));
			info.AddProperty(NCard.PropertyName._canvasGroupMaskMaterial, Variant.From<Material>(ref this._canvasGroupMaskMaterial));
			info.AddProperty(NCard.PropertyName._visibility, Variant.From<ModelVisibility>(ref this._visibility));
		}

		/// <inheritdoc />
		// Token: 0x06004208 RID: 16904 RVA: 0x00200ABC File Offset: 0x001FECBC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCard.PropertyName.CardHighlight, ref variant))
			{
				this.CardHighlight = variant.As<NCardHighlight>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCard.PropertyName.Body, ref variant2))
			{
				this.Body = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCard.PropertyName.Visibility, ref variant3))
			{
				this.Visibility = variant3.As<ModelVisibility>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCard.PropertyName.PlayPileTween, ref variant4))
			{
				this.PlayPileTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCard.PropertyName.RandomizeCostTween, ref variant5))
			{
				this.RandomizeCostTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCard.PropertyName.DisplayingPile, ref variant6))
			{
				this.DisplayingPile = variant6.As<PileType>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCard.PropertyName._titleLabel, ref variant7))
			{
				this._titleLabel = variant7.As<MegaLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCard.PropertyName._descriptionLabel, ref variant8))
			{
				this._descriptionLabel = variant8.As<MegaRichTextLabel>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCard.PropertyName._ancientPortrait, ref variant9))
			{
				this._ancientPortrait = variant9.As<TextureRect>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCard.PropertyName._portrait, ref variant10))
			{
				this._portrait = variant10.As<TextureRect>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCard.PropertyName._frame, ref variant11))
			{
				this._frame = variant11.As<TextureRect>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCard.PropertyName._ancientBorderGlassOverlay, ref variant12))
			{
				this._ancientBorderGlassOverlay = variant12.As<TextureRect>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCard.PropertyName._ancientBorder, ref variant13))
			{
				this._ancientBorder = variant13.As<TextureRect>();
			}
			Variant variant14;
			if (info.TryGetProperty(NCard.PropertyName._ancientBanner, ref variant14))
			{
				this._ancientBanner = variant14.As<Control>();
			}
			Variant variant15;
			if (info.TryGetProperty(NCard.PropertyName._ancientTextBg, ref variant15))
			{
				this._ancientTextBg = variant15.As<TextureRect>();
			}
			Variant variant16;
			if (info.TryGetProperty(NCard.PropertyName._portraitBorder, ref variant16))
			{
				this._portraitBorder = variant16.As<TextureRect>();
			}
			Variant variant17;
			if (info.TryGetProperty(NCard.PropertyName._banner, ref variant17))
			{
				this._banner = variant17.As<TextureRect>();
			}
			Variant variant18;
			if (info.TryGetProperty(NCard.PropertyName._lock, ref variant18))
			{
				this._lock = variant18.As<TextureRect>();
			}
			Variant variant19;
			if (info.TryGetProperty(NCard.PropertyName._typePlaque, ref variant19))
			{
				this._typePlaque = variant19.As<NinePatchRect>();
			}
			Variant variant20;
			if (info.TryGetProperty(NCard.PropertyName._typeLabel, ref variant20))
			{
				this._typeLabel = variant20.As<MegaLabel>();
			}
			Variant variant21;
			if (info.TryGetProperty(NCard.PropertyName._portraitCanvasGroup, ref variant21))
			{
				this._portraitCanvasGroup = variant21.As<CanvasGroup>();
			}
			Variant variant22;
			if (info.TryGetProperty(NCard.PropertyName._rareGlow, ref variant22))
			{
				this._rareGlow = variant22.As<NCardRareGlow>();
			}
			Variant variant23;
			if (info.TryGetProperty(NCard.PropertyName._uncommonGlow, ref variant23))
			{
				this._uncommonGlow = variant23.As<NCardUncommonGlow>();
			}
			Variant variant24;
			if (info.TryGetProperty(NCard.PropertyName._sparkles, ref variant24))
			{
				this._sparkles = variant24.As<GpuParticles2D>();
			}
			Variant variant25;
			if (info.TryGetProperty(NCard.PropertyName._energyIcon, ref variant25))
			{
				this._energyIcon = variant25.As<TextureRect>();
			}
			Variant variant26;
			if (info.TryGetProperty(NCard.PropertyName._energyLabel, ref variant26))
			{
				this._energyLabel = variant26.As<MegaLabel>();
			}
			Variant variant27;
			if (info.TryGetProperty(NCard.PropertyName._unplayableEnergyIcon, ref variant27))
			{
				this._unplayableEnergyIcon = variant27.As<TextureRect>();
			}
			Variant variant28;
			if (info.TryGetProperty(NCard.PropertyName._starIcon, ref variant28))
			{
				this._starIcon = variant28.As<TextureRect>();
			}
			Variant variant29;
			if (info.TryGetProperty(NCard.PropertyName._starLabel, ref variant29))
			{
				this._starLabel = variant29.As<MegaLabel>();
			}
			Variant variant30;
			if (info.TryGetProperty(NCard.PropertyName._unplayableStarIcon, ref variant30))
			{
				this._unplayableStarIcon = variant30.As<TextureRect>();
			}
			Variant variant31;
			if (info.TryGetProperty(NCard.PropertyName._overlayContainer, ref variant31))
			{
				this._overlayContainer = variant31.As<Node>();
			}
			Variant variant32;
			if (info.TryGetProperty(NCard.PropertyName._cardOverlay, ref variant32))
			{
				this._cardOverlay = variant32.As<Control>();
			}
			Variant variant33;
			if (info.TryGetProperty(NCard.PropertyName._cardVfxContainer, ref variant33))
			{
				this._cardVfxContainer = variant33.As<Node>();
			}
			Variant variant34;
			if (info.TryGetProperty(NCard.PropertyName._enchantmentTab, ref variant34))
			{
				this._enchantmentTab = variant34.As<Control>();
			}
			Variant variant35;
			if (info.TryGetProperty(NCard.PropertyName._enchantmentVfxOverride, ref variant35))
			{
				this._enchantmentVfxOverride = variant35.As<TextureRect>();
			}
			Variant variant36;
			if (info.TryGetProperty(NCard.PropertyName._enchantmentIcon, ref variant36))
			{
				this._enchantmentIcon = variant36.As<TextureRect>();
			}
			Variant variant37;
			if (info.TryGetProperty(NCard.PropertyName._enchantmentLabel, ref variant37))
			{
				this._enchantmentLabel = variant37.As<MegaLabel>();
			}
			Variant variant38;
			if (info.TryGetProperty(NCard.PropertyName._defaultEnchantmentPosition, ref variant38))
			{
				this._defaultEnchantmentPosition = variant38.As<Vector2>();
			}
			Variant variant39;
			if (info.TryGetProperty(NCard.PropertyName._pretendCardCanBePlayed, ref variant39))
			{
				this._pretendCardCanBePlayed = variant39.As<bool>();
			}
			Variant variant40;
			if (info.TryGetProperty(NCard.PropertyName._forceUnpoweredPreview, ref variant40))
			{
				this._forceUnpoweredPreview = variant40.As<bool>();
			}
			Variant variant41;
			if (info.TryGetProperty(NCard.PropertyName._portraitBlurMaterial, ref variant41))
			{
				this._portraitBlurMaterial = variant41.As<Material>();
			}
			Variant variant42;
			if (info.TryGetProperty(NCard.PropertyName._canvasGroupMaskBlurMaterial, ref variant42))
			{
				this._canvasGroupMaskBlurMaterial = variant42.As<Material>();
			}
			Variant variant43;
			if (info.TryGetProperty(NCard.PropertyName._canvasGroupBlurMaterial, ref variant43))
			{
				this._canvasGroupBlurMaterial = variant43.As<Material>();
			}
			Variant variant44;
			if (info.TryGetProperty(NCard.PropertyName._canvasGroupMaskMaterial, ref variant44))
			{
				this._canvasGroupMaskMaterial = variant44.As<Material>();
			}
			Variant variant45;
			if (info.TryGetProperty(NCard.PropertyName._visibility, ref variant45))
			{
				this._visibility = variant45.As<ModelVisibility>();
			}
		}

		// Token: 0x0400198F RID: 6543
		private const string _scenePath = "res://scenes/cards/card.tscn";

		// Token: 0x04001990 RID: 6544
		private const string _portraitBlurMaterialPath = "res://scenes/cards/card_portrait_blur_material.tres";

		// Token: 0x04001991 RID: 6545
		private const string _canvasGroupMaskMaterialPath = "res://scenes/cards/card_canvas_group_mask_material.tres";

		// Token: 0x04001992 RID: 6546
		private const string _canvasGroupBlurMaterialPath = "res://scenes/cards/card_canvas_group_blur_material.tres";

		// Token: 0x04001993 RID: 6547
		private const string _canvasGroupMaskBlurMaterialPath = "res://scenes/cards/card_canvas_group_mask_blur_material.tres";

		// Token: 0x04001994 RID: 6548
		private const float _typePlaqueXMargin = 17f;

		// Token: 0x04001995 RID: 6549
		private const float _typePlaqueMinXSize = 61f;

		// Token: 0x04001996 RID: 6550
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001997 RID: 6551
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04001998 RID: 6552
		private static readonly StringName _h = new StringName("h");

		// Token: 0x04001999 RID: 6553
		public static readonly Vector2 defaultSize = new Vector2(300f, 422f);

		// Token: 0x0400199A RID: 6554
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x0400199B RID: 6555
		[Nullable(2)]
		private CardModel _model;

		// Token: 0x0400199C RID: 6556
		private MegaLabel _titleLabel;

		// Token: 0x0400199D RID: 6557
		private MegaRichTextLabel _descriptionLabel;

		// Token: 0x0400199E RID: 6558
		private TextureRect _ancientPortrait;

		// Token: 0x0400199F RID: 6559
		private TextureRect _portrait;

		// Token: 0x040019A0 RID: 6560
		private TextureRect _frame;

		// Token: 0x040019A1 RID: 6561
		private TextureRect _ancientBorderGlassOverlay;

		// Token: 0x040019A2 RID: 6562
		private TextureRect _ancientBorder;

		// Token: 0x040019A3 RID: 6563
		private Control _ancientBanner;

		// Token: 0x040019A4 RID: 6564
		private TextureRect _ancientTextBg;

		// Token: 0x040019A5 RID: 6565
		private TextureRect _portraitBorder;

		// Token: 0x040019A6 RID: 6566
		private TextureRect _banner;

		// Token: 0x040019A7 RID: 6567
		private TextureRect _lock;

		// Token: 0x040019A8 RID: 6568
		private NinePatchRect _typePlaque;

		// Token: 0x040019A9 RID: 6569
		private MegaLabel _typeLabel;

		// Token: 0x040019AA RID: 6570
		private CanvasGroup _portraitCanvasGroup;

		// Token: 0x040019AB RID: 6571
		[Nullable(2)]
		private NCardRareGlow _rareGlow;

		// Token: 0x040019AC RID: 6572
		[Nullable(2)]
		private NCardUncommonGlow _uncommonGlow;

		// Token: 0x040019AD RID: 6573
		private GpuParticles2D _sparkles;

		// Token: 0x040019AE RID: 6574
		private readonly List<NRelicFlashVfx> _flashVfx = new List<NRelicFlashVfx>();

		// Token: 0x040019AF RID: 6575
		private TextureRect _energyIcon;

		// Token: 0x040019B0 RID: 6576
		private MegaLabel _energyLabel;

		// Token: 0x040019B1 RID: 6577
		private TextureRect _unplayableEnergyIcon;

		// Token: 0x040019B2 RID: 6578
		private TextureRect _starIcon;

		// Token: 0x040019B3 RID: 6579
		private MegaLabel _starLabel;

		// Token: 0x040019B4 RID: 6580
		private TextureRect _unplayableStarIcon;

		// Token: 0x040019B5 RID: 6581
		private Node _overlayContainer;

		// Token: 0x040019B6 RID: 6582
		[Nullable(2)]
		private Control _cardOverlay;

		// Token: 0x040019B7 RID: 6583
		private Node _cardVfxContainer;

		// Token: 0x040019B8 RID: 6584
		[Nullable(2)]
		private Creature _previewTarget;

		// Token: 0x040019B9 RID: 6585
		private Control _enchantmentTab;

		// Token: 0x040019BA RID: 6586
		private TextureRect _enchantmentVfxOverride;

		// Token: 0x040019BB RID: 6587
		private TextureRect _enchantmentIcon;

		// Token: 0x040019BC RID: 6588
		private MegaLabel _enchantmentLabel;

		// Token: 0x040019BD RID: 6589
		private Vector2 _defaultEnchantmentPosition;

		// Token: 0x040019BE RID: 6590
		private const int _enchantmentTabStarLabelOffset = 45;

		// Token: 0x040019C1 RID: 6593
		[Nullable(2)]
		private EnchantmentModel _subscribedEnchantment;

		// Token: 0x040019C2 RID: 6594
		private bool _pretendCardCanBePlayed;

		// Token: 0x040019C3 RID: 6595
		private bool _forceUnpoweredPreview;

		// Token: 0x040019C4 RID: 6596
		[Nullable(2)]
		private Material _portraitBlurMaterial;

		// Token: 0x040019C5 RID: 6597
		[Nullable(2)]
		private Material _canvasGroupMaskBlurMaterial;

		// Token: 0x040019C6 RID: 6598
		[Nullable(2)]
		private Material _canvasGroupBlurMaterial;

		// Token: 0x040019C7 RID: 6599
		[Nullable(2)]
		private Material _canvasGroupMaskMaterial;

		/// <summary>
		/// The visibility of the model.
		/// In all cases during a run, this should be ModelVisibility.Visible. When we are showing the card as part of the
		/// compendium or other places, the card might show up as unknown (if the player has not seen it) or locked (if the
		/// card is locked in the timeline.
		/// </summary>
		// Token: 0x040019C8 RID: 6600
		private ModelVisibility _visibility = ModelVisibility.Visible;

		// Token: 0x040019C9 RID: 6601
		private readonly LocString _unknownDescription = new LocString("card_library", "UNKNOWN.description");

		// Token: 0x040019CA RID: 6602
		private readonly LocString _unknownTitle = new LocString("card_library", "UNKNOWN.title");

		// Token: 0x040019CB RID: 6603
		private readonly LocString _lockedDescription = new LocString("card_library", "LOCKED.description");

		// Token: 0x040019CC RID: 6604
		private readonly LocString _lockedTitle = new LocString("card_library", "LOCKED.title");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001937 RID: 6455
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'OnInstantiated' method.
			/// </summary>
			// Token: 0x04006333 RID: 25395
			public static readonly StringName OnInstantiated = "OnInstantiated";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006334 RID: 25396
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04006335 RID: 25397
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006336 RID: 25398
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'InitPool' method.
			/// </summary>
			// Token: 0x04006337 RID: 25399
			public static readonly StringName InitPool = "InitPool";

			/// <summary>
			/// Cached name for the 'GetCurrentSize' method.
			/// </summary>
			// Token: 0x04006338 RID: 25400
			public static readonly StringName GetCurrentSize = "GetCurrentSize";

			/// <summary>
			/// Cached name for the 'UpdateVisuals' method.
			/// </summary>
			// Token: 0x04006339 RID: 25401
			public static readonly StringName UpdateVisuals = "UpdateVisuals";

			/// <summary>
			/// Cached name for the 'ShowUpgradePreview' method.
			/// </summary>
			// Token: 0x0400633A RID: 25402
			public static readonly StringName ShowUpgradePreview = "ShowUpgradePreview";

			/// <summary>
			/// Cached name for the 'UpdateEnchantmentVisuals' method.
			/// </summary>
			// Token: 0x0400633B RID: 25403
			public static readonly StringName UpdateEnchantmentVisuals = "UpdateEnchantmentVisuals";

			/// <summary>
			/// Cached name for the 'OnEnchantmentStatusChanged' method.
			/// </summary>
			// Token: 0x0400633C RID: 25404
			public static readonly StringName OnEnchantmentStatusChanged = "OnEnchantmentStatusChanged";

			/// <summary>
			/// Cached name for the 'SetEnchantmentStatus' method.
			/// </summary>
			// Token: 0x0400633D RID: 25405
			public static readonly StringName SetEnchantmentStatus = "SetEnchantmentStatus";

			/// <summary>
			/// Cached name for the 'UpdateEnergyCostVisuals' method.
			/// </summary>
			// Token: 0x0400633E RID: 25406
			public static readonly StringName UpdateEnergyCostVisuals = "UpdateEnergyCostVisuals";

			/// <summary>
			/// Cached name for the 'SetPretendCardCanBePlayed' method.
			/// </summary>
			// Token: 0x0400633F RID: 25407
			public static readonly StringName SetPretendCardCanBePlayed = "SetPretendCardCanBePlayed";

			/// <summary>
			/// Cached name for the 'SetForceUnpoweredPreview' method.
			/// </summary>
			// Token: 0x04006340 RID: 25408
			public static readonly StringName SetForceUnpoweredPreview = "SetForceUnpoweredPreview";

			/// <summary>
			/// Cached name for the 'UpdateEnergyCostColor' method.
			/// </summary>
			// Token: 0x04006341 RID: 25409
			public static readonly StringName UpdateEnergyCostColor = "UpdateEnergyCostColor";

			/// <summary>
			/// Cached name for the 'UpdateStarCostVisuals' method.
			/// </summary>
			// Token: 0x04006342 RID: 25410
			public static readonly StringName UpdateStarCostVisuals = "UpdateStarCostVisuals";

			/// <summary>
			/// Cached name for the 'UpdateStarCostText' method.
			/// </summary>
			// Token: 0x04006343 RID: 25411
			public static readonly StringName UpdateStarCostText = "UpdateStarCostText";

			/// <summary>
			/// Cached name for the 'UpdateStarCostColor' method.
			/// </summary>
			// Token: 0x04006344 RID: 25412
			public static readonly StringName UpdateStarCostColor = "UpdateStarCostColor";

			/// <summary>
			/// Cached name for the 'GetCostTextColorInHand' method.
			/// </summary>
			// Token: 0x04006345 RID: 25413
			public static readonly StringName GetCostTextColorInHand = "GetCostTextColorInHand";

			/// <summary>
			/// Cached name for the 'GetCostOutlineColorInHand' method.
			/// </summary>
			// Token: 0x04006346 RID: 25414
			public static readonly StringName GetCostOutlineColorInHand = "GetCostOutlineColorInHand";

			/// <summary>
			/// Cached name for the 'PlayRandomizeCostAnim' method.
			/// </summary>
			// Token: 0x04006347 RID: 25415
			public static readonly StringName PlayRandomizeCostAnim = "PlayRandomizeCostAnim";

			/// <summary>
			/// Cached name for the 'Reload' method.
			/// </summary>
			// Token: 0x04006348 RID: 25416
			public static readonly StringName Reload = "Reload";

			/// <summary>
			/// Cached name for the 'UpdatePortrait' method.
			/// </summary>
			// Token: 0x04006349 RID: 25417
			public static readonly StringName UpdatePortrait = "UpdatePortrait";

			/// <summary>
			/// Cached name for the 'UpdateTypePlaque' method.
			/// </summary>
			// Token: 0x0400634A RID: 25418
			public static readonly StringName UpdateTypePlaque = "UpdateTypePlaque";

			/// <summary>
			/// Cached name for the 'UpdateTypePlaqueSizeAndPosition' method.
			/// </summary>
			// Token: 0x0400634B RID: 25419
			public static readonly StringName UpdateTypePlaqueSizeAndPosition = "UpdateTypePlaqueSizeAndPosition";

			/// <summary>
			/// Cached name for the 'UpdateTitleLabel' method.
			/// </summary>
			// Token: 0x0400634C RID: 25420
			public static readonly StringName UpdateTitleLabel = "UpdateTitleLabel";

			/// <summary>
			/// Cached name for the 'GetTitleLabelOutlineColor' method.
			/// </summary>
			// Token: 0x0400634D RID: 25421
			public static readonly StringName GetTitleLabelOutlineColor = "GetTitleLabelOutlineColor";

			/// <summary>
			/// Cached name for the 'ReloadOverlay' method.
			/// </summary>
			// Token: 0x0400634E RID: 25422
			public static readonly StringName ReloadOverlay = "ReloadOverlay";

			/// <summary>
			/// Cached name for the 'OnAfflictionChanged' method.
			/// </summary>
			// Token: 0x0400634F RID: 25423
			public static readonly StringName OnAfflictionChanged = "OnAfflictionChanged";

			/// <summary>
			/// Cached name for the 'OnEnchantmentChanged' method.
			/// </summary>
			// Token: 0x04006350 RID: 25424
			public static readonly StringName OnEnchantmentChanged = "OnEnchantmentChanged";

			/// <summary>
			/// Cached name for the 'GetTitleText' method.
			/// </summary>
			// Token: 0x04006351 RID: 25425
			public static readonly StringName GetTitleText = "GetTitleText";

			/// <summary>
			/// Cached name for the 'ActivateRewardScreenGlow' method.
			/// </summary>
			// Token: 0x04006352 RID: 25426
			public static readonly StringName ActivateRewardScreenGlow = "ActivateRewardScreenGlow";

			/// <summary>
			/// Cached name for the 'KillRarityGlow' method.
			/// </summary>
			// Token: 0x04006353 RID: 25427
			public static readonly StringName KillRarityGlow = "KillRarityGlow";

			/// <summary>
			/// Cached name for the 'AnimCardToPlayPile' method.
			/// </summary>
			// Token: 0x04006354 RID: 25428
			public static readonly StringName AnimCardToPlayPile = "AnimCardToPlayPile";

			/// <summary>
			/// Cached name for the 'OnReturnedFromPool' method.
			/// </summary>
			// Token: 0x04006355 RID: 25429
			public static readonly StringName OnReturnedFromPool = "OnReturnedFromPool";

			/// <summary>
			/// Cached name for the 'OnFreedToPool' method.
			/// </summary>
			// Token: 0x04006356 RID: 25430
			public static readonly StringName OnFreedToPool = "OnFreedToPool";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001938 RID: 6456
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CardHighlight' property.
			/// </summary>
			// Token: 0x04006357 RID: 25431
			public static readonly StringName CardHighlight = "CardHighlight";

			/// <summary>
			/// Cached name for the 'Body' property.
			/// </summary>
			// Token: 0x04006358 RID: 25432
			public static readonly StringName Body = "Body";

			/// <summary>
			/// Cached name for the 'Visibility' property.
			/// </summary>
			// Token: 0x04006359 RID: 25433
			public static readonly StringName Visibility = "Visibility";

			/// <summary>
			/// Cached name for the 'PlayPileTween' property.
			/// </summary>
			// Token: 0x0400635A RID: 25434
			public static readonly StringName PlayPileTween = "PlayPileTween";

			/// <summary>
			/// Cached name for the 'RandomizeCostTween' property.
			/// </summary>
			// Token: 0x0400635B RID: 25435
			public static readonly StringName RandomizeCostTween = "RandomizeCostTween";

			/// <summary>
			/// Cached name for the 'DisplayingPile' property.
			/// </summary>
			// Token: 0x0400635C RID: 25436
			public static readonly StringName DisplayingPile = "DisplayingPile";

			/// <summary>
			/// Cached name for the 'EnchantmentTab' property.
			/// </summary>
			// Token: 0x0400635D RID: 25437
			public static readonly StringName EnchantmentTab = "EnchantmentTab";

			/// <summary>
			/// Cached name for the 'EnchantmentVfxOverride' property.
			/// </summary>
			// Token: 0x0400635E RID: 25438
			public static readonly StringName EnchantmentVfxOverride = "EnchantmentVfxOverride";

			/// <summary>
			/// Cached name for the 'OverlayContainer' property.
			/// </summary>
			// Token: 0x0400635F RID: 25439
			public static readonly StringName OverlayContainer = "OverlayContainer";

			/// <summary>
			/// Cached name for the 'CardVfxContainer' property.
			/// </summary>
			// Token: 0x04006360 RID: 25440
			public static readonly StringName CardVfxContainer = "CardVfxContainer";

			/// <summary>
			/// Cached name for the '_titleLabel' field.
			/// </summary>
			// Token: 0x04006361 RID: 25441
			public static readonly StringName _titleLabel = "_titleLabel";

			/// <summary>
			/// Cached name for the '_descriptionLabel' field.
			/// </summary>
			// Token: 0x04006362 RID: 25442
			public static readonly StringName _descriptionLabel = "_descriptionLabel";

			/// <summary>
			/// Cached name for the '_ancientPortrait' field.
			/// </summary>
			// Token: 0x04006363 RID: 25443
			public static readonly StringName _ancientPortrait = "_ancientPortrait";

			/// <summary>
			/// Cached name for the '_portrait' field.
			/// </summary>
			// Token: 0x04006364 RID: 25444
			public static readonly StringName _portrait = "_portrait";

			/// <summary>
			/// Cached name for the '_frame' field.
			/// </summary>
			// Token: 0x04006365 RID: 25445
			public static readonly StringName _frame = "_frame";

			/// <summary>
			/// Cached name for the '_ancientBorderGlassOverlay' field.
			/// </summary>
			// Token: 0x04006366 RID: 25446
			public static readonly StringName _ancientBorderGlassOverlay = "_ancientBorderGlassOverlay";

			/// <summary>
			/// Cached name for the '_ancientBorder' field.
			/// </summary>
			// Token: 0x04006367 RID: 25447
			public static readonly StringName _ancientBorder = "_ancientBorder";

			/// <summary>
			/// Cached name for the '_ancientBanner' field.
			/// </summary>
			// Token: 0x04006368 RID: 25448
			public static readonly StringName _ancientBanner = "_ancientBanner";

			/// <summary>
			/// Cached name for the '_ancientTextBg' field.
			/// </summary>
			// Token: 0x04006369 RID: 25449
			public static readonly StringName _ancientTextBg = "_ancientTextBg";

			/// <summary>
			/// Cached name for the '_portraitBorder' field.
			/// </summary>
			// Token: 0x0400636A RID: 25450
			public static readonly StringName _portraitBorder = "_portraitBorder";

			/// <summary>
			/// Cached name for the '_banner' field.
			/// </summary>
			// Token: 0x0400636B RID: 25451
			public static readonly StringName _banner = "_banner";

			/// <summary>
			/// Cached name for the '_lock' field.
			/// </summary>
			// Token: 0x0400636C RID: 25452
			public static readonly StringName _lock = "_lock";

			/// <summary>
			/// Cached name for the '_typePlaque' field.
			/// </summary>
			// Token: 0x0400636D RID: 25453
			public static readonly StringName _typePlaque = "_typePlaque";

			/// <summary>
			/// Cached name for the '_typeLabel' field.
			/// </summary>
			// Token: 0x0400636E RID: 25454
			public static readonly StringName _typeLabel = "_typeLabel";

			/// <summary>
			/// Cached name for the '_portraitCanvasGroup' field.
			/// </summary>
			// Token: 0x0400636F RID: 25455
			public static readonly StringName _portraitCanvasGroup = "_portraitCanvasGroup";

			/// <summary>
			/// Cached name for the '_rareGlow' field.
			/// </summary>
			// Token: 0x04006370 RID: 25456
			public static readonly StringName _rareGlow = "_rareGlow";

			/// <summary>
			/// Cached name for the '_uncommonGlow' field.
			/// </summary>
			// Token: 0x04006371 RID: 25457
			public static readonly StringName _uncommonGlow = "_uncommonGlow";

			/// <summary>
			/// Cached name for the '_sparkles' field.
			/// </summary>
			// Token: 0x04006372 RID: 25458
			public static readonly StringName _sparkles = "_sparkles";

			/// <summary>
			/// Cached name for the '_energyIcon' field.
			/// </summary>
			// Token: 0x04006373 RID: 25459
			public static readonly StringName _energyIcon = "_energyIcon";

			/// <summary>
			/// Cached name for the '_energyLabel' field.
			/// </summary>
			// Token: 0x04006374 RID: 25460
			public static readonly StringName _energyLabel = "_energyLabel";

			/// <summary>
			/// Cached name for the '_unplayableEnergyIcon' field.
			/// </summary>
			// Token: 0x04006375 RID: 25461
			public static readonly StringName _unplayableEnergyIcon = "_unplayableEnergyIcon";

			/// <summary>
			/// Cached name for the '_starIcon' field.
			/// </summary>
			// Token: 0x04006376 RID: 25462
			public static readonly StringName _starIcon = "_starIcon";

			/// <summary>
			/// Cached name for the '_starLabel' field.
			/// </summary>
			// Token: 0x04006377 RID: 25463
			public static readonly StringName _starLabel = "_starLabel";

			/// <summary>
			/// Cached name for the '_unplayableStarIcon' field.
			/// </summary>
			// Token: 0x04006378 RID: 25464
			public static readonly StringName _unplayableStarIcon = "_unplayableStarIcon";

			/// <summary>
			/// Cached name for the '_overlayContainer' field.
			/// </summary>
			// Token: 0x04006379 RID: 25465
			public static readonly StringName _overlayContainer = "_overlayContainer";

			/// <summary>
			/// Cached name for the '_cardOverlay' field.
			/// </summary>
			// Token: 0x0400637A RID: 25466
			public static readonly StringName _cardOverlay = "_cardOverlay";

			/// <summary>
			/// Cached name for the '_cardVfxContainer' field.
			/// </summary>
			// Token: 0x0400637B RID: 25467
			public static readonly StringName _cardVfxContainer = "_cardVfxContainer";

			/// <summary>
			/// Cached name for the '_enchantmentTab' field.
			/// </summary>
			// Token: 0x0400637C RID: 25468
			public static readonly StringName _enchantmentTab = "_enchantmentTab";

			/// <summary>
			/// Cached name for the '_enchantmentVfxOverride' field.
			/// </summary>
			// Token: 0x0400637D RID: 25469
			public static readonly StringName _enchantmentVfxOverride = "_enchantmentVfxOverride";

			/// <summary>
			/// Cached name for the '_enchantmentIcon' field.
			/// </summary>
			// Token: 0x0400637E RID: 25470
			public static readonly StringName _enchantmentIcon = "_enchantmentIcon";

			/// <summary>
			/// Cached name for the '_enchantmentLabel' field.
			/// </summary>
			// Token: 0x0400637F RID: 25471
			public static readonly StringName _enchantmentLabel = "_enchantmentLabel";

			/// <summary>
			/// Cached name for the '_defaultEnchantmentPosition' field.
			/// </summary>
			// Token: 0x04006380 RID: 25472
			public static readonly StringName _defaultEnchantmentPosition = "_defaultEnchantmentPosition";

			/// <summary>
			/// Cached name for the '_pretendCardCanBePlayed' field.
			/// </summary>
			// Token: 0x04006381 RID: 25473
			public static readonly StringName _pretendCardCanBePlayed = "_pretendCardCanBePlayed";

			/// <summary>
			/// Cached name for the '_forceUnpoweredPreview' field.
			/// </summary>
			// Token: 0x04006382 RID: 25474
			public static readonly StringName _forceUnpoweredPreview = "_forceUnpoweredPreview";

			/// <summary>
			/// Cached name for the '_portraitBlurMaterial' field.
			/// </summary>
			// Token: 0x04006383 RID: 25475
			public static readonly StringName _portraitBlurMaterial = "_portraitBlurMaterial";

			/// <summary>
			/// Cached name for the '_canvasGroupMaskBlurMaterial' field.
			/// </summary>
			// Token: 0x04006384 RID: 25476
			public static readonly StringName _canvasGroupMaskBlurMaterial = "_canvasGroupMaskBlurMaterial";

			/// <summary>
			/// Cached name for the '_canvasGroupBlurMaterial' field.
			/// </summary>
			// Token: 0x04006385 RID: 25477
			public static readonly StringName _canvasGroupBlurMaterial = "_canvasGroupBlurMaterial";

			/// <summary>
			/// Cached name for the '_canvasGroupMaskMaterial' field.
			/// </summary>
			// Token: 0x04006386 RID: 25478
			public static readonly StringName _canvasGroupMaskMaterial = "_canvasGroupMaskMaterial";

			/// <summary>
			/// Cached name for the '_visibility' field.
			/// </summary>
			// Token: 0x04006387 RID: 25479
			public static readonly StringName _visibility = "_visibility";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001939 RID: 6457
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
