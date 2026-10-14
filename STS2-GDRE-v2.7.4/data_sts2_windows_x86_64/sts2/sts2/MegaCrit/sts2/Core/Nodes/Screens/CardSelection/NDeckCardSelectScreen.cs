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
	/// A generic select from the cards in the player's deck. Managing logic for the extra confirmation screen.
	/// Typically we use this screen when we are modifying the composition of a deck rather than a specific card.
	/// This is included but not limited to:
	/// - transforming cards
	/// - removing cards
	/// - cloning cards (Dolly's Mirror)
	/// Does not include actions that modify specific cards such as,
	/// upgrading (see NDeckSelectScreen) or applying enchantments (see NDeckEnchantSelectScreen) to cards, as those have special UI
	/// </summary>
	// Token: 0x0200034B RID: 843
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NDeckCardSelectScreen.cs")]
	public sealed class NDeckCardSelectScreen : NCardGridSelectionScreen
	{
		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x0600304C RID: 12364 RVA: 0x00162E40 File Offset: 0x00161040
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/card_selection/deck_card_select_screen");
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x0600304D RID: 12365 RVA: 0x00162E4C File Offset: 0x0016104C
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NDeckCardSelectScreen.ScenePath);
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x00162E58 File Offset: 0x00161058
		protected override IEnumerable<Control> PeekButtonTargets
		{
			get
			{
				return new <>z__ReadOnlyArray<Control>(new Control[] { this._previewContainer, this._closeButton, this._confirmButton });
			}
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x00162E80 File Offset: 0x00161080
		public override void _Ready()
		{
			this.ConnectSignalsAndInitGrid();
			this._previewContainer = base.GetNode<Control>("%PreviewContainer");
			this._previewCards = this._previewContainer.GetNode<Control>("%Cards");
			this._previewCancelButton = this._previewContainer.GetNode<NBackButton>("%PreviewCancel");
			this._previewConfirmButton = this._previewContainer.GetNode<NConfirmButton>("%PreviewConfirm");
			this._closeButton = base.GetNode<NBackButton>("%Close");
			this._confirmButton = base.GetNode<NConfirmButton>("%Confirm");
			this._infoLabel = base.GetNode<MegaRichTextLabel>("%BottomLabel");
			this._previewCancelButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
			this._previewConfirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
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
			this.RefreshConfirmButtonVisibility();
			this._previewContainer.Visible = false;
			this._previewContainer.MouseFilter = 2L;
			this._previewCancelButton.Disable();
			this._previewConfirmButton.Disable();
			this._infoLabel.Text = this._prefs.Prompt.GetFormattedText();
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x00163040 File Offset: 0x00161240
		public static NDeckCardSelectScreen Create(IReadOnlyList<CardModel> cards, CardSelectorPrefs prefs)
		{
			NDeckCardSelectScreen ndeckCardSelectScreen = PreloadManager.Cache.GetScene(NDeckCardSelectScreen.ScenePath).Instantiate<NDeckCardSelectScreen>(0L);
			ndeckCardSelectScreen.Name = "NDeckCardSelectScreen";
			ndeckCardSelectScreen._cards = cards;
			ndeckCardSelectScreen._prefs = prefs;
			return ndeckCardSelectScreen;
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x00163084 File Offset: 0x00161284
		private void RefreshConfirmButtonVisibility()
		{
			if (this._prefs.MinSelect != this._prefs.MaxSelect && this._selectedCards.Count >= this._prefs.MinSelect)
			{
				this._confirmButton.Enable();
				return;
			}
			this._confirmButton.Disable();
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x001630D8 File Offset: 0x001612D8
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

		// Token: 0x06003053 RID: 12371 RVA: 0x0016313E File Offset: 0x0016133E
		private void PreviewSelection(NButton _)
		{
			this.PreviewSelection();
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x00163148 File Offset: 0x00161348
		private void PreviewSelection()
		{
			this._grid.FocusBehaviorRecursive = 1L;
			base.GetViewport().GuiReleaseFocus();
			this._previewContainer.Visible = true;
			this._previewContainer.MouseFilter = 0L;
			this._closeButton.Disable();
			this._grid.SetCanScroll(false);
			this._previewCancelButton.Enable();
			this._previewConfirmButton.Enable();
			foreach (CardModel cardModel in this._selectedCards)
			{
				this._grid.UnhighlightCard(cardModel);
				NCard ncard = NCard.Create(cardModel, ModelVisibility.Visible);
				NPreviewCardHolder npreviewCardHolder = NPreviewCardHolder.Create(ncard, true, false);
				this._previewCards.AddChildSafely(npreviewCardHolder);
				ncard.UpdateVisuals(cardModel.Pile.Type, CardPreviewMode.Normal);
			}
			Callable.From(delegate
			{
				this._previewCards.PivotOffset = this._previewCards.Size / 2f;
				float num = 1f;
				if (this._selectedCards.Count > 6)
				{
					num = 0.55f;
				}
				else if (this._selectedCards.Count > 3)
				{
					num = 0.8f;
				}
				this._previewCards.Scale = Vector2.One * num;
			}).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x0016324C File Offset: 0x0016144C
		private void CloseSelection(NButton _)
		{
			this._completionSource.SetResult(Array.Empty<CardModel>());
			NOverlayStack.Instance.Remove(this);
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x0016326C File Offset: 0x0016146C
		private void CancelSelection(NButton _)
		{
			this._previewContainer.Visible = false;
			this._previewCancelButton.Disable();
			this._previewConfirmButton.Disable();
			this._grid.FocusBehaviorRecursive = 0L;
			this._grid.SetCanScroll(true);
			this._previewContainer.MouseFilter = 2L;
			for (int i = 0; i < this._previewCards.GetChildCount(false); i++)
			{
				this._previewCards.GetChild(i, false).QueueFreeSafely();
			}
			NGridCardHolder cardHolder = this._grid.GetCardHolder(this._selectedCards.Last<CardModel>());
			if (cardHolder != null)
			{
				cardHolder.TryGrabFocus();
			}
			this._selectedCards.Clear();
			if (this._prefs.Cancelable)
			{
				this._closeButton.Enable();
			}
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x0016332E File Offset: 0x0016152E
		private void ConfirmSelection(NButton _)
		{
			this.CheckIfSelectionComplete();
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x00163336 File Offset: 0x00161536
		private void CheckIfSelectionComplete()
		{
			if (this._selectedCards.Count < this._prefs.MinSelect)
			{
				return;
			}
			this._completionSource.SetResult(this._selectedCards);
			NOverlayStack.Instance.Remove(this);
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06003059 RID: 12377 RVA: 0x0016336D File Offset: 0x0016156D
		[Nullable(2)]
		public override Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._previewContainer.Visible)
				{
					return null;
				}
				return this._grid.DefaultFocusedControl;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x0600305A RID: 12378 RVA: 0x00163389 File Offset: 0x00161589
		[Nullable(2)]
		public override Control FocusedControlFromTopBar
		{
			[NullableContext(2)]
			get
			{
				if (this._previewContainer.Visible)
				{
					return null;
				}
				return this._grid.FocusedControlFromTopBar;
			}
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x001633A5 File Offset: 0x001615A5
		public override void AfterOverlayShown()
		{
			if (this._prefs.Cancelable)
			{
				this._closeButton.Enable();
			}
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x001633BF File Offset: 0x001615BF
		public override void AfterOverlayHidden()
		{
			this._closeButton.Disable();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600305D RID: 12381 RVA: 0x001633CC File Offset: 0x001615CC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NDeckCardSelectScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckCardSelectScreen.MethodName.RefreshConfirmButtonVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckCardSelectScreen.MethodName.PreviewSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckCardSelectScreen.MethodName.PreviewSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckCardSelectScreen.MethodName.CloseSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckCardSelectScreen.MethodName.CancelSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckCardSelectScreen.MethodName.ConfirmSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckCardSelectScreen.MethodName.CheckIfSelectionComplete, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckCardSelectScreen.MethodName.AfterOverlayShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckCardSelectScreen.MethodName.AfterOverlayHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600305E RID: 12382 RVA: 0x00163678 File Offset: 0x00161878
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDeckCardSelectScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckCardSelectScreen.MethodName.RefreshConfirmButtonVisibility && args.Count == 0)
			{
				this.RefreshConfirmButtonVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckCardSelectScreen.MethodName.PreviewSelection && args.Count == 1)
			{
				this.PreviewSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckCardSelectScreen.MethodName.PreviewSelection && args.Count == 0)
			{
				this.PreviewSelection();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckCardSelectScreen.MethodName.CloseSelection && args.Count == 1)
			{
				this.CloseSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckCardSelectScreen.MethodName.CancelSelection && args.Count == 1)
			{
				this.CancelSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckCardSelectScreen.MethodName.ConfirmSelection && args.Count == 1)
			{
				this.ConfirmSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckCardSelectScreen.MethodName.CheckIfSelectionComplete && args.Count == 0)
			{
				this.CheckIfSelectionComplete();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckCardSelectScreen.MethodName.AfterOverlayShown && args.Count == 0)
			{
				this.AfterOverlayShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckCardSelectScreen.MethodName.AfterOverlayHidden && args.Count == 0)
			{
				this.AfterOverlayHidden();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600305F RID: 12383 RVA: 0x00163838 File Offset: 0x00161A38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDeckCardSelectScreen.MethodName._Ready || (ref method) == NDeckCardSelectScreen.MethodName.RefreshConfirmButtonVisibility || (ref method) == NDeckCardSelectScreen.MethodName.PreviewSelection || (ref method) == NDeckCardSelectScreen.MethodName.CloseSelection || (ref method) == NDeckCardSelectScreen.MethodName.CancelSelection || (ref method) == NDeckCardSelectScreen.MethodName.ConfirmSelection || (ref method) == NDeckCardSelectScreen.MethodName.CheckIfSelectionComplete || (ref method) == NDeckCardSelectScreen.MethodName.AfterOverlayShown || (ref method) == NDeckCardSelectScreen.MethodName.AfterOverlayHidden || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003060 RID: 12384 RVA: 0x001638D4 File Offset: 0x00161AD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDeckCardSelectScreen.PropertyName._previewContainer)
			{
				this._previewContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._previewCards)
			{
				this._previewCards = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._previewCancelButton)
			{
				this._previewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._previewConfirmButton)
			{
				this._previewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._closeButton)
			{
				this._closeButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._infoLabel)
			{
				this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003061 RID: 12385 RVA: 0x001639A8 File Offset: 0x00161BA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDeckCardSelectScreen.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName.FocusedControlFromTopBar)
			{
				Control control = this.FocusedControlFromTopBar;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._previewContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._previewContainer);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._previewCards)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._previewCards);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._previewCancelButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._previewCancelButton);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._previewConfirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._previewConfirmButton);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._closeButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._closeButton);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NDeckCardSelectScreen.PropertyName._infoLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003062 RID: 12386 RVA: 0x00163AE4 File Offset: 0x00161CE4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDeckCardSelectScreen.PropertyName._previewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckCardSelectScreen.PropertyName._previewCards, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckCardSelectScreen.PropertyName._previewCancelButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckCardSelectScreen.PropertyName._previewConfirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckCardSelectScreen.PropertyName._closeButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckCardSelectScreen.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckCardSelectScreen.PropertyName._infoLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckCardSelectScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckCardSelectScreen.PropertyName.FocusedControlFromTopBar, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003063 RID: 12387 RVA: 0x00163C24 File Offset: 0x00161E24
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDeckCardSelectScreen.PropertyName._previewContainer, Variant.From<Control>(ref this._previewContainer));
			info.AddProperty(NDeckCardSelectScreen.PropertyName._previewCards, Variant.From<Control>(ref this._previewCards));
			info.AddProperty(NDeckCardSelectScreen.PropertyName._previewCancelButton, Variant.From<NBackButton>(ref this._previewCancelButton));
			info.AddProperty(NDeckCardSelectScreen.PropertyName._previewConfirmButton, Variant.From<NConfirmButton>(ref this._previewConfirmButton));
			info.AddProperty(NDeckCardSelectScreen.PropertyName._closeButton, Variant.From<NBackButton>(ref this._closeButton));
			info.AddProperty(NDeckCardSelectScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
			info.AddProperty(NDeckCardSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
		}

		/// <inheritdoc />
		// Token: 0x06003064 RID: 12388 RVA: 0x00163CD4 File Offset: 0x00161ED4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._previewContainer, ref variant))
			{
				this._previewContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._previewCards, ref variant2))
			{
				this._previewCards = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._previewCancelButton, ref variant3))
			{
				this._previewCancelButton = variant3.As<NBackButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._previewConfirmButton, ref variant4))
			{
				this._previewConfirmButton = variant4.As<NConfirmButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._closeButton, ref variant5))
			{
				this._closeButton = variant5.As<NBackButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._confirmButton, ref variant6))
			{
				this._confirmButton = variant6.As<NConfirmButton>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDeckCardSelectScreen.PropertyName._infoLabel, ref variant7))
			{
				this._infoLabel = variant7.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x040012A6 RID: 4774
		private readonly HashSet<CardModel> _selectedCards = new HashSet<CardModel>();

		// Token: 0x040012A7 RID: 4775
		private CardSelectorPrefs _prefs;

		// Token: 0x040012A8 RID: 4776
		private Control _previewContainer;

		// Token: 0x040012A9 RID: 4777
		private Control _previewCards;

		// Token: 0x040012AA RID: 4778
		private NBackButton _previewCancelButton;

		// Token: 0x040012AB RID: 4779
		private NConfirmButton _previewConfirmButton;

		// Token: 0x040012AC RID: 4780
		private NBackButton _closeButton;

		// Token: 0x040012AD RID: 4781
		private NConfirmButton _confirmButton;

		// Token: 0x040012AE RID: 4782
		private MegaRichTextLabel _infoLabel;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001613 RID: 5651
		[NullableContext(0)]
		public new class MethodName : NCardGridSelectionScreen.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040055A9 RID: 21929
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshConfirmButtonVisibility' method.
			/// </summary>
			// Token: 0x040055AA RID: 21930
			public static readonly StringName RefreshConfirmButtonVisibility = "RefreshConfirmButtonVisibility";

			/// <summary>
			/// Cached name for the 'PreviewSelection' method.
			/// </summary>
			// Token: 0x040055AB RID: 21931
			public static readonly StringName PreviewSelection = "PreviewSelection";

			/// <summary>
			/// Cached name for the 'CloseSelection' method.
			/// </summary>
			// Token: 0x040055AC RID: 21932
			public static readonly StringName CloseSelection = "CloseSelection";

			/// <summary>
			/// Cached name for the 'CancelSelection' method.
			/// </summary>
			// Token: 0x040055AD RID: 21933
			public static readonly StringName CancelSelection = "CancelSelection";

			/// <summary>
			/// Cached name for the 'ConfirmSelection' method.
			/// </summary>
			// Token: 0x040055AE RID: 21934
			public static readonly StringName ConfirmSelection = "ConfirmSelection";

			/// <summary>
			/// Cached name for the 'CheckIfSelectionComplete' method.
			/// </summary>
			// Token: 0x040055AF RID: 21935
			public static readonly StringName CheckIfSelectionComplete = "CheckIfSelectionComplete";

			/// <summary>
			/// Cached name for the 'AfterOverlayShown' method.
			/// </summary>
			// Token: 0x040055B0 RID: 21936
			public new static readonly StringName AfterOverlayShown = "AfterOverlayShown";

			/// <summary>
			/// Cached name for the 'AfterOverlayHidden' method.
			/// </summary>
			// Token: 0x040055B1 RID: 21937
			public new static readonly StringName AfterOverlayHidden = "AfterOverlayHidden";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001614 RID: 5652
		[NullableContext(0)]
		public new class PropertyName : NCardGridSelectionScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040055B2 RID: 21938
			public new static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the 'FocusedControlFromTopBar' property.
			/// </summary>
			// Token: 0x040055B3 RID: 21939
			public new static readonly StringName FocusedControlFromTopBar = "FocusedControlFromTopBar";

			/// <summary>
			/// Cached name for the '_previewContainer' field.
			/// </summary>
			// Token: 0x040055B4 RID: 21940
			public static readonly StringName _previewContainer = "_previewContainer";

			/// <summary>
			/// Cached name for the '_previewCards' field.
			/// </summary>
			// Token: 0x040055B5 RID: 21941
			public static readonly StringName _previewCards = "_previewCards";

			/// <summary>
			/// Cached name for the '_previewCancelButton' field.
			/// </summary>
			// Token: 0x040055B6 RID: 21942
			public static readonly StringName _previewCancelButton = "_previewCancelButton";

			/// <summary>
			/// Cached name for the '_previewConfirmButton' field.
			/// </summary>
			// Token: 0x040055B7 RID: 21943
			public static readonly StringName _previewConfirmButton = "_previewConfirmButton";

			/// <summary>
			/// Cached name for the '_closeButton' field.
			/// </summary>
			// Token: 0x040055B8 RID: 21944
			public static readonly StringName _closeButton = "_closeButton";

			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x040055B9 RID: 21945
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_infoLabel' field.
			/// </summary>
			// Token: 0x040055BA RID: 21946
			public static readonly StringName _infoLabel = "_infoLabel";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001615 RID: 5653
		[NullableContext(0)]
		public new class SignalName : NCardGridSelectionScreen.SignalName
		{
		}
	}
}
