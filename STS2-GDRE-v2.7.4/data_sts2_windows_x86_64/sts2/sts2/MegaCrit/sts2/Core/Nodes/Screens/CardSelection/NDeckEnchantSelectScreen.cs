using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	/// <summary>
	/// NCardSelectionScreen specifically for enchanting cards in your deck.
	/// Managing logic for the extra confirmation screen.
	/// </summary>
	// Token: 0x0200034C RID: 844
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NDeckEnchantSelectScreen.cs")]
	public sealed class NDeckEnchantSelectScreen : NCardGridSelectionScreen
	{
		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06003067 RID: 12391 RVA: 0x00163E33 File Offset: 0x00162033
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/card_selection/deck_enchant_select_screen");
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06003068 RID: 12392 RVA: 0x00163E3F File Offset: 0x0016203F
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NDeckEnchantSelectScreen.ScenePath);
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06003069 RID: 12393 RVA: 0x00163E4B File Offset: 0x0016204B
		private bool UseSingleSelection
		{
			get
			{
				return this._prefs.MaxSelect == 1;
			}
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x00163E5C File Offset: 0x0016205C
		public override void _Ready()
		{
			this.ConnectSignalsAndInitGrid();
			this._confirmButton = base.GetNode<NConfirmButton>("Confirm");
			this._enchantSinglePreviewContainer = base.GetNode<Control>("%EnchantSinglePreviewContainer");
			this._singlePreview = this._enchantSinglePreviewContainer.GetNode<NEnchantPreview>("EnchantPreview");
			this._singlePreviewCancelButton = this._enchantSinglePreviewContainer.GetNode<NBackButton>("Cancel");
			this._singlePreviewConfirmButton = this._enchantSinglePreviewContainer.GetNode<NConfirmButton>("Confirm");
			this._enchantMultiPreviewContainer = base.GetNode<Control>("%EnchantMultiPreviewContainer");
			this._multiPreview = this._enchantMultiPreviewContainer.GetNode<Control>("Cards");
			this._multiPreviewCancelButton = this._enchantMultiPreviewContainer.GetNode<NBackButton>("Cancel");
			this._multiPreviewConfirmButton = this._enchantMultiPreviewContainer.GetNode<NConfirmButton>("Confirm");
			this._enchantmentDescriptionContainer = base.GetNode<Control>("%EnchantmentDescriptionContainer");
			this._enchantmentIcon = this._enchantmentDescriptionContainer.GetNode<TextureRect>("%EnchantmentIcon");
			this._enchantmentTitle = this._enchantmentDescriptionContainer.GetNode<MegaLabel>("%EnchantmentTitle");
			this._enchantmentDescription = this._enchantmentDescriptionContainer.GetNode<MegaRichTextLabel>("%EnchantmentDescription");
			this._closeButton = base.GetNode<NBackButton>("%Close");
			this._singlePreviewCancelButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
			this._singlePreviewConfirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
			this._multiPreviewCancelButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
			this._multiPreviewConfirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
			this._closeButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseSelection)), 0U);
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.PreviewSelection)), 0U);
			if (this._prefs.Cancelable)
			{
				this._closeButton.Enable();
			}
			else
			{
				this._closeButton.Disable();
			}
			EnchantmentModel enchantmentModel = this._enchantment.ToMutable();
			enchantmentModel.Amount = this._enchantmentAmount;
			enchantmentModel.RecalculateValues();
			this._enchantmentTitle.SetTextAutoSize(enchantmentModel.Title.GetFormattedText());
			this._enchantmentDescription.Text = enchantmentModel.DynamicDescription.GetFormattedText();
			this._enchantmentIcon.Texture = enchantmentModel.Icon;
			this._enchantSinglePreviewContainer.Visible = false;
			this._enchantSinglePreviewContainer.MouseFilter = 2L;
			this._enchantMultiPreviewContainer.Visible = false;
			this._enchantMultiPreviewContainer.MouseFilter = 2L;
			this.RefreshConfirmButtonVisibility();
			this._bottomTextContainer = base.GetNode<Control>("%BottomText");
			this._infoLabel = this._bottomTextContainer.GetNode<MegaRichTextLabel>("%BottomLabel");
			this._infoLabel.Text = "[center]" + this._prefs.Prompt.GetFormattedText() + "[/center]";
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x001641B8 File Offset: 0x001623B8
		public static NDeckEnchantSelectScreen ShowScreen(IReadOnlyList<CardModel> cards, EnchantmentModel enchantment, int amount, CardSelectorPrefs prefs)
		{
			NDeckEnchantSelectScreen ndeckEnchantSelectScreen = PreloadManager.Cache.GetScene(NDeckEnchantSelectScreen.ScenePath).Instantiate<NDeckEnchantSelectScreen>(0L);
			ndeckEnchantSelectScreen.Name = "NDeckEnchantSelectScreen";
			ndeckEnchantSelectScreen._cards = cards;
			ndeckEnchantSelectScreen._prefs = prefs;
			ndeckEnchantSelectScreen._enchantment = enchantment;
			ndeckEnchantSelectScreen._enchantmentAmount = amount;
			NOverlayStack.Instance.Push(ndeckEnchantSelectScreen);
			return ndeckEnchantSelectScreen;
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x0600306C RID: 12396 RVA: 0x00164214 File Offset: 0x00162414
		protected override IEnumerable<Control> PeekButtonTargets
		{
			get
			{
				return new <>z__ReadOnlyArray<Control>(new Control[] { this._enchantSinglePreviewContainer, this._enchantMultiPreviewContainer, this._enchantmentDescriptionContainer, this._closeButton, this._bottomTextContainer });
			}
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x00164250 File Offset: 0x00162450
		protected override void OnCardClicked(CardModel card)
		{
			if (this._selectedCards.Add(card))
			{
				this._grid.HighlightCard(card);
				if (this._prefs.MaxSelect == this._selectedCards.Count)
				{
					this.PreviewSelection();
				}
			}
			else
			{
				this._selectedCards.Remove(card);
				this._grid.UnhighlightCard(card);
			}
			this.RefreshConfirmButtonVisibility();
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x001642B8 File Offset: 0x001624B8
		private void RefreshConfirmButtonVisibility()
		{
			if (this._prefs.MinSelect != this._prefs.MaxSelect && this._selectedCards.Count >= this._prefs.MinSelect)
			{
				this._confirmButton.Enable();
				return;
			}
			this._confirmButton.Disable();
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x0016430C File Offset: 0x0016250C
		private void CloseSelection(NButton _)
		{
			this._completionSource.SetResult(Array.Empty<CardModel>());
			NOverlayStack.Instance.Remove(this);
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x0016432C File Offset: 0x0016252C
		private void CancelSelection(NButton _)
		{
			if (this.UseSingleSelection)
			{
				this._singlePreviewCancelButton.Disable();
				this._singlePreviewConfirmButton.Disable();
				this._enchantSinglePreviewContainer.Visible = false;
				this._enchantSinglePreviewContainer.MouseFilter = 2L;
			}
			else
			{
				this._multiPreviewCancelButton.Disable();
				this._multiPreviewConfirmButton.Disable();
				this._enchantMultiPreviewContainer.Visible = false;
				this._enchantMultiPreviewContainer.MouseFilter = 2L;
				for (int i = 0; i < this._multiPreview.GetChildCount(false); i++)
				{
					this._multiPreview.GetChild(i, false).QueueFreeSafely();
				}
			}
			this._grid.FocusBehaviorRecursive = 0L;
			this._grid.SetCanScroll(true);
			if (this._prefs.Cancelable)
			{
				this._closeButton.Enable();
			}
			foreach (CardModel cardModel in this._selectedCards)
			{
				this._grid.UnhighlightCard(cardModel);
			}
			NGridCardHolder cardHolder = this._grid.GetCardHolder(this._selectedCards.Last<CardModel>());
			if (cardHolder != null)
			{
				cardHolder.TryGrabFocus();
			}
			this._selectedCards.Clear();
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x00164474 File Offset: 0x00162674
		private void PreviewSelection(NButton _)
		{
			this.PreviewSelection();
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x0016447C File Offset: 0x0016267C
		private void PreviewSelection()
		{
			this._grid.FocusBehaviorRecursive = 1L;
			this._grid.SetCanScroll(false);
			this._closeButton.Disable();
			base.GetViewport().GuiReleaseFocus();
			if (this.UseSingleSelection)
			{
				this._enchantSinglePreviewContainer.Visible = true;
				this._enchantSinglePreviewContainer.MouseFilter = 0L;
				this._singlePreview.Init(this._selectedCards.First<CardModel>(), this._enchantment, this._enchantmentAmount);
				this._singlePreviewCancelButton.Enable();
				this._singlePreviewConfirmButton.Enable();
				return;
			}
			this._enchantMultiPreviewContainer.Visible = true;
			this._enchantMultiPreviewContainer.MouseFilter = 0L;
			this._multiPreviewCancelButton.Enable();
			this._multiPreviewConfirmButton.Enable();
			foreach (CardModel cardModel in this._selectedCards)
			{
				NCard ncard = NCard.Create(cardModel, ModelVisibility.Visible);
				this._multiPreview.AddChildSafely(NPreviewCardHolder.Create(ncard, true, false));
				ncard.UpdateVisuals(cardModel.Pile.Type, CardPreviewMode.Normal);
			}
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x001645B0 File Offset: 0x001627B0
		private void ConfirmSelection(NButton inputEvent)
		{
			if (this._selectedCards.Count == 0)
			{
				return;
			}
			this.CheckIfSelectionComplete();
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x001645C8 File Offset: 0x001627C8
		private void CheckIfSelectionComplete()
		{
			this._singlePreviewCancelButton.Enable();
			this._singlePreviewConfirmButton.Enable();
			if (this._selectedCards.Count >= this._prefs.MinSelect && this._selectedCards.Count <= this._prefs.MaxSelect)
			{
				this._completionSource.SetResult(this._selectedCards);
				NOverlayStack.Instance.Remove(this);
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x00164637 File Offset: 0x00162837
		[Nullable(2)]
		public override Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._enchantSinglePreviewContainer.Visible || this._enchantMultiPreviewContainer.Visible)
				{
					return null;
				}
				return this._grid.DefaultFocusedControl;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06003076 RID: 12406 RVA: 0x00164660 File Offset: 0x00162860
		[Nullable(2)]
		public override Control FocusedControlFromTopBar
		{
			[NullableContext(2)]
			get
			{
				if (this._enchantSinglePreviewContainer.Visible || this._enchantMultiPreviewContainer.Visible)
				{
					return null;
				}
				return this._grid.FocusedControlFromTopBar;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003077 RID: 12407 RVA: 0x0016468C File Offset: 0x0016288C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NDeckEnchantSelectScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckEnchantSelectScreen.MethodName.RefreshConfirmButtonVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckEnchantSelectScreen.MethodName.CloseSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckEnchantSelectScreen.MethodName.CancelSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckEnchantSelectScreen.MethodName.PreviewSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckEnchantSelectScreen.MethodName.PreviewSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckEnchantSelectScreen.MethodName.ConfirmSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckEnchantSelectScreen.MethodName.CheckIfSelectionComplete, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003078 RID: 12408 RVA: 0x001648DC File Offset: 0x00162ADC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDeckEnchantSelectScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckEnchantSelectScreen.MethodName.RefreshConfirmButtonVisibility && args.Count == 0)
			{
				this.RefreshConfirmButtonVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckEnchantSelectScreen.MethodName.CloseSelection && args.Count == 1)
			{
				this.CloseSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckEnchantSelectScreen.MethodName.CancelSelection && args.Count == 1)
			{
				this.CancelSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckEnchantSelectScreen.MethodName.PreviewSelection && args.Count == 1)
			{
				this.PreviewSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckEnchantSelectScreen.MethodName.PreviewSelection && args.Count == 0)
			{
				this.PreviewSelection();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckEnchantSelectScreen.MethodName.ConfirmSelection && args.Count == 1)
			{
				this.ConfirmSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckEnchantSelectScreen.MethodName.CheckIfSelectionComplete && args.Count == 0)
			{
				this.CheckIfSelectionComplete();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003079 RID: 12409 RVA: 0x00164A54 File Offset: 0x00162C54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDeckEnchantSelectScreen.MethodName._Ready || (ref method) == NDeckEnchantSelectScreen.MethodName.RefreshConfirmButtonVisibility || (ref method) == NDeckEnchantSelectScreen.MethodName.CloseSelection || (ref method) == NDeckEnchantSelectScreen.MethodName.CancelSelection || (ref method) == NDeckEnchantSelectScreen.MethodName.PreviewSelection || (ref method) == NDeckEnchantSelectScreen.MethodName.ConfirmSelection || (ref method) == NDeckEnchantSelectScreen.MethodName.CheckIfSelectionComplete || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600307A RID: 12410 RVA: 0x00164AD4 File Offset: 0x00162CD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantmentAmount)
			{
				this._enchantmentAmount = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantSinglePreviewContainer)
			{
				this._enchantSinglePreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._singlePreview)
			{
				this._singlePreview = VariantUtils.ConvertTo<NEnchantPreview>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._singlePreviewCancelButton)
			{
				this._singlePreviewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._singlePreviewConfirmButton)
			{
				this._singlePreviewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantMultiPreviewContainer)
			{
				this._enchantMultiPreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._multiPreview)
			{
				this._multiPreview = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._multiPreviewCancelButton)
			{
				this._multiPreviewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._multiPreviewConfirmButton)
			{
				this._multiPreviewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantmentDescriptionContainer)
			{
				this._enchantmentDescriptionContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantmentTitle)
			{
				this._enchantmentTitle = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantmentDescription)
			{
				this._enchantmentDescription = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantmentIcon)
			{
				this._enchantmentIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._bottomTextContainer)
			{
				this._bottomTextContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._infoLabel)
			{
				this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._closeButton)
			{
				this._closeButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600307B RID: 12411 RVA: 0x00164CB4 File Offset: 0x00162EB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName.UseSingleSelection)
			{
				bool useSingleSelection = this.UseSingleSelection;
				value = VariantUtils.CreateFrom<bool>(ref useSingleSelection);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName.FocusedControlFromTopBar)
			{
				Control control = this.FocusedControlFromTopBar;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantmentAmount)
			{
				value = VariantUtils.CreateFrom<int>(ref this._enchantmentAmount);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantSinglePreviewContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._enchantSinglePreviewContainer);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._singlePreview)
			{
				value = VariantUtils.CreateFrom<NEnchantPreview>(ref this._singlePreview);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._singlePreviewCancelButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._singlePreviewCancelButton);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._singlePreviewConfirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._singlePreviewConfirmButton);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantMultiPreviewContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._enchantMultiPreviewContainer);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._multiPreview)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._multiPreview);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._multiPreviewCancelButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._multiPreviewCancelButton);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._multiPreviewConfirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._multiPreviewConfirmButton);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantmentDescriptionContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._enchantmentDescriptionContainer);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantmentTitle)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._enchantmentTitle);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantmentDescription)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._enchantmentDescription);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._enchantmentIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._enchantmentIcon);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._bottomTextContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bottomTextContainer);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._infoLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
				return true;
			}
			if ((ref name) == NDeckEnchantSelectScreen.PropertyName._closeButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._closeButton);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600307C RID: 12412 RVA: 0x00164F54 File Offset: 0x00163154
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NDeckEnchantSelectScreen.PropertyName.UseSingleSelection, 0L, "", 4096L, false),
				new PropertyInfo(2L, NDeckEnchantSelectScreen.PropertyName._enchantmentAmount, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._enchantSinglePreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._singlePreview, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._singlePreviewCancelButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._singlePreviewConfirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._enchantMultiPreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._multiPreview, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._multiPreviewCancelButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._multiPreviewConfirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._enchantmentDescriptionContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._enchantmentTitle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._enchantmentDescription, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._enchantmentIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._bottomTextContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._infoLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName._closeButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckEnchantSelectScreen.PropertyName.FocusedControlFromTopBar, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600307D RID: 12413 RVA: 0x001651FC File Offset: 0x001633FC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentAmount, Variant.From<int>(ref this._enchantmentAmount));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantSinglePreviewContainer, Variant.From<Control>(ref this._enchantSinglePreviewContainer));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._singlePreview, Variant.From<NEnchantPreview>(ref this._singlePreview));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._singlePreviewCancelButton, Variant.From<NBackButton>(ref this._singlePreviewCancelButton));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._singlePreviewConfirmButton, Variant.From<NConfirmButton>(ref this._singlePreviewConfirmButton));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantMultiPreviewContainer, Variant.From<Control>(ref this._enchantMultiPreviewContainer));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._multiPreview, Variant.From<Control>(ref this._multiPreview));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._multiPreviewCancelButton, Variant.From<NBackButton>(ref this._multiPreviewCancelButton));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._multiPreviewConfirmButton, Variant.From<NConfirmButton>(ref this._multiPreviewConfirmButton));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentDescriptionContainer, Variant.From<Control>(ref this._enchantmentDescriptionContainer));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentTitle, Variant.From<MegaLabel>(ref this._enchantmentTitle));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentDescription, Variant.From<MegaRichTextLabel>(ref this._enchantmentDescription));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentIcon, Variant.From<TextureRect>(ref this._enchantmentIcon));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._bottomTextContainer, Variant.From<Control>(ref this._bottomTextContainer));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
			info.AddProperty(NDeckEnchantSelectScreen.PropertyName._closeButton, Variant.From<NBackButton>(ref this._closeButton));
		}

		/// <inheritdoc />
		// Token: 0x0600307E RID: 12414 RVA: 0x00165388 File Offset: 0x00163588
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentAmount, ref variant))
			{
				this._enchantmentAmount = variant.As<int>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantSinglePreviewContainer, ref variant2))
			{
				this._enchantSinglePreviewContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._singlePreview, ref variant3))
			{
				this._singlePreview = variant3.As<NEnchantPreview>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._singlePreviewCancelButton, ref variant4))
			{
				this._singlePreviewCancelButton = variant4.As<NBackButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._singlePreviewConfirmButton, ref variant5))
			{
				this._singlePreviewConfirmButton = variant5.As<NConfirmButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._confirmButton, ref variant6))
			{
				this._confirmButton = variant6.As<NConfirmButton>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantMultiPreviewContainer, ref variant7))
			{
				this._enchantMultiPreviewContainer = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._multiPreview, ref variant8))
			{
				this._multiPreview = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._multiPreviewCancelButton, ref variant9))
			{
				this._multiPreviewCancelButton = variant9.As<NBackButton>();
			}
			Variant variant10;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._multiPreviewConfirmButton, ref variant10))
			{
				this._multiPreviewConfirmButton = variant10.As<NConfirmButton>();
			}
			Variant variant11;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentDescriptionContainer, ref variant11))
			{
				this._enchantmentDescriptionContainer = variant11.As<Control>();
			}
			Variant variant12;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentTitle, ref variant12))
			{
				this._enchantmentTitle = variant12.As<MegaLabel>();
			}
			Variant variant13;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentDescription, ref variant13))
			{
				this._enchantmentDescription = variant13.As<MegaRichTextLabel>();
			}
			Variant variant14;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._enchantmentIcon, ref variant14))
			{
				this._enchantmentIcon = variant14.As<TextureRect>();
			}
			Variant variant15;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._bottomTextContainer, ref variant15))
			{
				this._bottomTextContainer = variant15.As<Control>();
			}
			Variant variant16;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._infoLabel, ref variant16))
			{
				this._infoLabel = variant16.As<MegaRichTextLabel>();
			}
			Variant variant17;
			if (info.TryGetProperty(NDeckEnchantSelectScreen.PropertyName._closeButton, ref variant17))
			{
				this._closeButton = variant17.As<NBackButton>();
			}
		}

		// Token: 0x040012AF RID: 4783
		private readonly HashSet<CardModel> _selectedCards = new HashSet<CardModel>();

		// Token: 0x040012B0 RID: 4784
		private CardSelectorPrefs _prefs;

		// Token: 0x040012B1 RID: 4785
		private EnchantmentModel _enchantment;

		// Token: 0x040012B2 RID: 4786
		private int _enchantmentAmount;

		// Token: 0x040012B3 RID: 4787
		private Control _enchantSinglePreviewContainer;

		// Token: 0x040012B4 RID: 4788
		private NEnchantPreview _singlePreview;

		// Token: 0x040012B5 RID: 4789
		private NBackButton _singlePreviewCancelButton;

		// Token: 0x040012B6 RID: 4790
		private NConfirmButton _singlePreviewConfirmButton;

		// Token: 0x040012B7 RID: 4791
		private NConfirmButton _confirmButton;

		// Token: 0x040012B8 RID: 4792
		private Control _enchantMultiPreviewContainer;

		// Token: 0x040012B9 RID: 4793
		private Control _multiPreview;

		// Token: 0x040012BA RID: 4794
		private NBackButton _multiPreviewCancelButton;

		// Token: 0x040012BB RID: 4795
		private NConfirmButton _multiPreviewConfirmButton;

		// Token: 0x040012BC RID: 4796
		private Control _enchantmentDescriptionContainer;

		// Token: 0x040012BD RID: 4797
		private MegaLabel _enchantmentTitle;

		// Token: 0x040012BE RID: 4798
		private MegaRichTextLabel _enchantmentDescription;

		// Token: 0x040012BF RID: 4799
		private TextureRect _enchantmentIcon;

		// Token: 0x040012C0 RID: 4800
		private Control _bottomTextContainer;

		// Token: 0x040012C1 RID: 4801
		private MegaRichTextLabel _infoLabel;

		// Token: 0x040012C2 RID: 4802
		private NBackButton _closeButton;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001616 RID: 5654
		[NullableContext(0)]
		public new class MethodName : NCardGridSelectionScreen.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040055BB RID: 21947
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshConfirmButtonVisibility' method.
			/// </summary>
			// Token: 0x040055BC RID: 21948
			public static readonly StringName RefreshConfirmButtonVisibility = "RefreshConfirmButtonVisibility";

			/// <summary>
			/// Cached name for the 'CloseSelection' method.
			/// </summary>
			// Token: 0x040055BD RID: 21949
			public static readonly StringName CloseSelection = "CloseSelection";

			/// <summary>
			/// Cached name for the 'CancelSelection' method.
			/// </summary>
			// Token: 0x040055BE RID: 21950
			public static readonly StringName CancelSelection = "CancelSelection";

			/// <summary>
			/// Cached name for the 'PreviewSelection' method.
			/// </summary>
			// Token: 0x040055BF RID: 21951
			public static readonly StringName PreviewSelection = "PreviewSelection";

			/// <summary>
			/// Cached name for the 'ConfirmSelection' method.
			/// </summary>
			// Token: 0x040055C0 RID: 21952
			public static readonly StringName ConfirmSelection = "ConfirmSelection";

			/// <summary>
			/// Cached name for the 'CheckIfSelectionComplete' method.
			/// </summary>
			// Token: 0x040055C1 RID: 21953
			public static readonly StringName CheckIfSelectionComplete = "CheckIfSelectionComplete";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001617 RID: 5655
		[NullableContext(0)]
		public new class PropertyName : NCardGridSelectionScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the 'UseSingleSelection' property.
			/// </summary>
			// Token: 0x040055C2 RID: 21954
			public static readonly StringName UseSingleSelection = "UseSingleSelection";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040055C3 RID: 21955
			public new static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the 'FocusedControlFromTopBar' property.
			/// </summary>
			// Token: 0x040055C4 RID: 21956
			public new static readonly StringName FocusedControlFromTopBar = "FocusedControlFromTopBar";

			/// <summary>
			/// Cached name for the '_enchantmentAmount' field.
			/// </summary>
			// Token: 0x040055C5 RID: 21957
			public static readonly StringName _enchantmentAmount = "_enchantmentAmount";

			/// <summary>
			/// Cached name for the '_enchantSinglePreviewContainer' field.
			/// </summary>
			// Token: 0x040055C6 RID: 21958
			public static readonly StringName _enchantSinglePreviewContainer = "_enchantSinglePreviewContainer";

			/// <summary>
			/// Cached name for the '_singlePreview' field.
			/// </summary>
			// Token: 0x040055C7 RID: 21959
			public static readonly StringName _singlePreview = "_singlePreview";

			/// <summary>
			/// Cached name for the '_singlePreviewCancelButton' field.
			/// </summary>
			// Token: 0x040055C8 RID: 21960
			public static readonly StringName _singlePreviewCancelButton = "_singlePreviewCancelButton";

			/// <summary>
			/// Cached name for the '_singlePreviewConfirmButton' field.
			/// </summary>
			// Token: 0x040055C9 RID: 21961
			public static readonly StringName _singlePreviewConfirmButton = "_singlePreviewConfirmButton";

			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x040055CA RID: 21962
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_enchantMultiPreviewContainer' field.
			/// </summary>
			// Token: 0x040055CB RID: 21963
			public static readonly StringName _enchantMultiPreviewContainer = "_enchantMultiPreviewContainer";

			/// <summary>
			/// Cached name for the '_multiPreview' field.
			/// </summary>
			// Token: 0x040055CC RID: 21964
			public static readonly StringName _multiPreview = "_multiPreview";

			/// <summary>
			/// Cached name for the '_multiPreviewCancelButton' field.
			/// </summary>
			// Token: 0x040055CD RID: 21965
			public static readonly StringName _multiPreviewCancelButton = "_multiPreviewCancelButton";

			/// <summary>
			/// Cached name for the '_multiPreviewConfirmButton' field.
			/// </summary>
			// Token: 0x040055CE RID: 21966
			public static readonly StringName _multiPreviewConfirmButton = "_multiPreviewConfirmButton";

			/// <summary>
			/// Cached name for the '_enchantmentDescriptionContainer' field.
			/// </summary>
			// Token: 0x040055CF RID: 21967
			public static readonly StringName _enchantmentDescriptionContainer = "_enchantmentDescriptionContainer";

			/// <summary>
			/// Cached name for the '_enchantmentTitle' field.
			/// </summary>
			// Token: 0x040055D0 RID: 21968
			public static readonly StringName _enchantmentTitle = "_enchantmentTitle";

			/// <summary>
			/// Cached name for the '_enchantmentDescription' field.
			/// </summary>
			// Token: 0x040055D1 RID: 21969
			public static readonly StringName _enchantmentDescription = "_enchantmentDescription";

			/// <summary>
			/// Cached name for the '_enchantmentIcon' field.
			/// </summary>
			// Token: 0x040055D2 RID: 21970
			public static readonly StringName _enchantmentIcon = "_enchantmentIcon";

			/// <summary>
			/// Cached name for the '_bottomTextContainer' field.
			/// </summary>
			// Token: 0x040055D3 RID: 21971
			public static readonly StringName _bottomTextContainer = "_bottomTextContainer";

			/// <summary>
			/// Cached name for the '_infoLabel' field.
			/// </summary>
			// Token: 0x040055D4 RID: 21972
			public static readonly StringName _infoLabel = "_infoLabel";

			/// <summary>
			/// Cached name for the '_closeButton' field.
			/// </summary>
			// Token: 0x040055D5 RID: 21973
			public static readonly StringName _closeButton = "_closeButton";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001618 RID: 5656
		[NullableContext(0)]
		public new class SignalName : NCardGridSelectionScreen.SignalName
		{
		}
	}
}
