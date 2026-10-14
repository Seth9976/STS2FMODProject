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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	/// <summary>
	/// CardSelectionScreen specifically for transforming cards to random other cards.
	/// For transforming cards into specific other cards, you probably don't need to use this.
	/// </summary>
	// Token: 0x0200034D RID: 845
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NDeckTransformSelectScreen.cs")]
	public sealed class NDeckTransformSelectScreen : NCardGridSelectionScreen
	{
		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x0016558B File Offset: 0x0016378B
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/card_selection/deck_transform_select_screen");
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06003081 RID: 12417 RVA: 0x00165597 File Offset: 0x00163797
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NDeckTransformSelectScreen.ScenePath);
			}
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x001655A4 File Offset: 0x001637A4
		public override void _Ready()
		{
			this.ConnectSignalsAndInitGrid();
			this._confirmButton = base.GetNode<NConfirmButton>("Confirm");
			this._previewContainer = base.GetNode<Control>("%PreviewContainer");
			this._transformPreview = this._previewContainer.GetNode<NTransformPreview>("TransformPreview");
			this._previewCancelButton = this._previewContainer.GetNode<NBackButton>("Cancel");
			this._previewConfirmButton = this._previewContainer.GetNode<NConfirmButton>("Confirm");
			this._closeButton = base.GetNode<NBackButton>("%Close");
			this._previewCancelButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
			this._previewConfirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CompleteSelection)), 0U);
			this._closeButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseSelection)), 0U);
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
			if (this._prefs.Cancelable)
			{
				this._closeButton.Enable();
			}
			else
			{
				this._closeButton.Disable();
			}
			this.RefreshConfirmButtonVisibility();
			this._previewCancelButton.Disable();
			this._previewConfirmButton.Disable();
			this._bottomTextContainer = base.GetNode<Control>("%BottomText");
			this._infoLabel = this._bottomTextContainer.GetNode<MegaRichTextLabel>("%BottomLabel");
			this._infoLabel.Text = this._prefs.Prompt.GetFormattedText();
			this._viewUpgrades = base.GetNode<NTickbox>("%Upgrades");
			this._viewUpgrades.IsTicked = false;
			this._viewUpgrades.Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowUpgrades)), 0U);
			this.OnControllerStateUpdated();
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
			base.GetNode<MegaLabel>("%ViewUpgradesLabel").SetTextAutoSize(new LocString("card_selection", "VIEW_UPGRADES").GetFormattedText());
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x00165840 File Offset: 0x00163A40
		public static NDeckTransformSelectScreen ShowScreen(IReadOnlyList<CardModel> cards, Func<CardModel, CardTransformation> cardToTransformation, CardSelectorPrefs prefs)
		{
			NDeckTransformSelectScreen ndeckTransformSelectScreen = PreloadManager.Cache.GetScene(NDeckTransformSelectScreen.ScenePath).Instantiate<NDeckTransformSelectScreen>(0L);
			ndeckTransformSelectScreen.Name = "NDeckTransformSelectScreen";
			ndeckTransformSelectScreen._cards = cards;
			ndeckTransformSelectScreen._cardToTransformation = cardToTransformation;
			ndeckTransformSelectScreen._prefs = prefs;
			NOverlayStack.Instance.Push(ndeckTransformSelectScreen);
			return ndeckTransformSelectScreen;
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x00165895 File Offset: 0x00163A95
		protected override IEnumerable<Control> PeekButtonTargets
		{
			get
			{
				return new <>z__ReadOnlyArray<Control>(new Control[] { this._previewContainer, this._closeButton, this._bottomTextContainer });
			}
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x001658C0 File Offset: 0x00163AC0
		private void RefreshConfirmButtonVisibility()
		{
			if (this._prefs.MinSelect != this._prefs.MaxSelect && this._selectedCards.Count >= this._prefs.MinSelect)
			{
				this._confirmButton.Enable();
				return;
			}
			this._confirmButton.Disable();
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x00165914 File Offset: 0x00163B14
		protected override void OnCardClicked(CardModel card)
		{
			if (this._selectedCards.Add(card))
			{
				this._grid.HighlightCard(card);
				if (this._prefs.MaxSelect == this._selectedCards.Count)
				{
					this.OpenPreviewScreen();
				}
			}
			else
			{
				this._selectedCards.Remove(card);
				this._grid.UnhighlightCard(card);
			}
			this.RefreshConfirmButtonVisibility();
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x0016597A File Offset: 0x00163B7A
		private void CloseSelection(NButton _)
		{
			this._completionSource.SetResult(Array.Empty<CardModel>());
			this._previewCancelButton.Disable();
			this._previewConfirmButton.Disable();
			NOverlayStack.Instance.Remove(this);
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x001659B0 File Offset: 0x00163BB0
		private void CancelSelection(NButton _)
		{
			this._previewContainer.Visible = false;
			this._grid.FocusBehaviorRecursive = 0L;
			this._previewContainer.MouseFilter = 2L;
			this._transformPreview.Uninitialize();
			this._previewCancelButton.Disable();
			this._previewConfirmButton.Disable();
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

		// Token: 0x06003089 RID: 12425 RVA: 0x00165AA0 File Offset: 0x00163CA0
		private void ConfirmSelection(NButton _)
		{
			if (this._selectedCards.Count >= this._prefs.MinSelect)
			{
				if (this._prefs.RequireManualConfirmation)
				{
					this.OpenPreviewScreen();
					return;
				}
				this.CompleteSelection(_);
			}
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x00165AD8 File Offset: 0x00163CD8
		private void OpenPreviewScreen()
		{
			base.GetViewport().GuiReleaseFocus();
			this._grid.FocusBehaviorRecursive = 1L;
			this._previewContainer.Visible = true;
			this._previewContainer.MouseFilter = 0L;
			this._previewCancelButton.Enable();
			this._previewConfirmButton.Enable();
			foreach (CardModel cardModel in this._selectedCards)
			{
				this._grid.UnhighlightCard(cardModel);
			}
			this._transformPreview.Initialize(this._selectedCards.Select<CardModel, CardTransformation>(this._cardToTransformation));
			this._closeButton.Disable();
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x00165BA0 File Offset: 0x00163DA0
		private void CompleteSelection(NButton _)
		{
			this._completionSource.SetResult(this._selectedCards);
			NOverlayStack.Instance.Remove(this);
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x00165BBE File Offset: 0x00163DBE
		private void ToggleShowUpgrades(NTickbox tickbox)
		{
			this._grid.IsShowingUpgrades = tickbox.IsTicked;
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x00165BD1 File Offset: 0x00163DD1
		private void OnControllerStateUpdated()
		{
			this._viewUpgrades.Visible = !NControllerManager.Instance.IsUsingController;
			if (NControllerManager.Instance.IsUsingController)
			{
				this._viewUpgrades.IsTicked = false;
				this.ToggleShowUpgrades(this._viewUpgrades);
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x00165C0F File Offset: 0x00163E0F
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

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x0600308F RID: 12431 RVA: 0x00165C2B File Offset: 0x00163E2B
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

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003090 RID: 12432 RVA: 0x00165C48 File Offset: 0x00163E48
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NDeckTransformSelectScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckTransformSelectScreen.MethodName.RefreshConfirmButtonVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckTransformSelectScreen.MethodName.CloseSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckTransformSelectScreen.MethodName.CancelSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckTransformSelectScreen.MethodName.ConfirmSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckTransformSelectScreen.MethodName.OpenPreviewScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckTransformSelectScreen.MethodName.CompleteSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckTransformSelectScreen.MethodName.ToggleShowUpgrades, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckTransformSelectScreen.MethodName.OnControllerStateUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003091 RID: 12433 RVA: 0x00165EF8 File Offset: 0x001640F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDeckTransformSelectScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckTransformSelectScreen.MethodName.RefreshConfirmButtonVisibility && args.Count == 0)
			{
				this.RefreshConfirmButtonVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckTransformSelectScreen.MethodName.CloseSelection && args.Count == 1)
			{
				this.CloseSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckTransformSelectScreen.MethodName.CancelSelection && args.Count == 1)
			{
				this.CancelSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckTransformSelectScreen.MethodName.ConfirmSelection && args.Count == 1)
			{
				this.ConfirmSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckTransformSelectScreen.MethodName.OpenPreviewScreen && args.Count == 0)
			{
				this.OpenPreviewScreen();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckTransformSelectScreen.MethodName.CompleteSelection && args.Count == 1)
			{
				this.CompleteSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckTransformSelectScreen.MethodName.ToggleShowUpgrades && args.Count == 1)
			{
				this.ToggleShowUpgrades(VariantUtils.ConvertTo<NTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckTransformSelectScreen.MethodName.OnControllerStateUpdated && args.Count == 0)
			{
				this.OnControllerStateUpdated();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003092 RID: 12434 RVA: 0x001660A4 File Offset: 0x001642A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDeckTransformSelectScreen.MethodName._Ready || (ref method) == NDeckTransformSelectScreen.MethodName.RefreshConfirmButtonVisibility || (ref method) == NDeckTransformSelectScreen.MethodName.CloseSelection || (ref method) == NDeckTransformSelectScreen.MethodName.CancelSelection || (ref method) == NDeckTransformSelectScreen.MethodName.ConfirmSelection || (ref method) == NDeckTransformSelectScreen.MethodName.OpenPreviewScreen || (ref method) == NDeckTransformSelectScreen.MethodName.CompleteSelection || (ref method) == NDeckTransformSelectScreen.MethodName.ToggleShowUpgrades || (ref method) == NDeckTransformSelectScreen.MethodName.OnControllerStateUpdated || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003093 RID: 12435 RVA: 0x00166140 File Offset: 0x00164340
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._previewContainer)
			{
				this._previewContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._transformPreview)
			{
				this._transformPreview = VariantUtils.ConvertTo<NTransformPreview>(ref value);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._previewCancelButton)
			{
				this._previewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._previewConfirmButton)
			{
				this._previewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._bottomTextContainer)
			{
				this._bottomTextContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._infoLabel)
			{
				this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._viewUpgrades)
			{
				this._viewUpgrades = VariantUtils.ConvertTo<NTickbox>(ref value);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._closeButton)
			{
				this._closeButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003094 RID: 12436 RVA: 0x00166248 File Offset: 0x00164448
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDeckTransformSelectScreen.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName.FocusedControlFromTopBar)
			{
				Control control = this.FocusedControlFromTopBar;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._previewContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._previewContainer);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._transformPreview)
			{
				value = VariantUtils.CreateFrom<NTransformPreview>(ref this._transformPreview);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._previewCancelButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._previewCancelButton);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._previewConfirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._previewConfirmButton);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._bottomTextContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bottomTextContainer);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._infoLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._viewUpgrades)
			{
				value = VariantUtils.CreateFrom<NTickbox>(ref this._viewUpgrades);
				return true;
			}
			if ((ref name) == NDeckTransformSelectScreen.PropertyName._closeButton)
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
		// Token: 0x06003095 RID: 12437 RVA: 0x001663C4 File Offset: 0x001645C4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDeckTransformSelectScreen.PropertyName._previewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckTransformSelectScreen.PropertyName._transformPreview, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckTransformSelectScreen.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckTransformSelectScreen.PropertyName._previewCancelButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckTransformSelectScreen.PropertyName._previewConfirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckTransformSelectScreen.PropertyName._bottomTextContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckTransformSelectScreen.PropertyName._infoLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckTransformSelectScreen.PropertyName._viewUpgrades, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckTransformSelectScreen.PropertyName._closeButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckTransformSelectScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckTransformSelectScreen.PropertyName.FocusedControlFromTopBar, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003096 RID: 12438 RVA: 0x00166544 File Offset: 0x00164744
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDeckTransformSelectScreen.PropertyName._previewContainer, Variant.From<Control>(ref this._previewContainer));
			info.AddProperty(NDeckTransformSelectScreen.PropertyName._transformPreview, Variant.From<NTransformPreview>(ref this._transformPreview));
			info.AddProperty(NDeckTransformSelectScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
			info.AddProperty(NDeckTransformSelectScreen.PropertyName._previewCancelButton, Variant.From<NBackButton>(ref this._previewCancelButton));
			info.AddProperty(NDeckTransformSelectScreen.PropertyName._previewConfirmButton, Variant.From<NConfirmButton>(ref this._previewConfirmButton));
			info.AddProperty(NDeckTransformSelectScreen.PropertyName._bottomTextContainer, Variant.From<Control>(ref this._bottomTextContainer));
			info.AddProperty(NDeckTransformSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
			info.AddProperty(NDeckTransformSelectScreen.PropertyName._viewUpgrades, Variant.From<NTickbox>(ref this._viewUpgrades));
			info.AddProperty(NDeckTransformSelectScreen.PropertyName._closeButton, Variant.From<NBackButton>(ref this._closeButton));
		}

		/// <inheritdoc />
		// Token: 0x06003097 RID: 12439 RVA: 0x00166620 File Offset: 0x00164820
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._previewContainer, ref variant))
			{
				this._previewContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._transformPreview, ref variant2))
			{
				this._transformPreview = variant2.As<NTransformPreview>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._confirmButton, ref variant3))
			{
				this._confirmButton = variant3.As<NConfirmButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._previewCancelButton, ref variant4))
			{
				this._previewCancelButton = variant4.As<NBackButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._previewConfirmButton, ref variant5))
			{
				this._previewConfirmButton = variant5.As<NConfirmButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._bottomTextContainer, ref variant6))
			{
				this._bottomTextContainer = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._infoLabel, ref variant7))
			{
				this._infoLabel = variant7.As<MegaRichTextLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._viewUpgrades, ref variant8))
			{
				this._viewUpgrades = variant8.As<NTickbox>();
			}
			Variant variant9;
			if (info.TryGetProperty(NDeckTransformSelectScreen.PropertyName._closeButton, ref variant9))
			{
				this._closeButton = variant9.As<NBackButton>();
			}
		}

		// Token: 0x040012C3 RID: 4803
		private readonly HashSet<CardModel> _selectedCards = new HashSet<CardModel>();

		// Token: 0x040012C4 RID: 4804
		private Func<CardModel, CardTransformation> _cardToTransformation;

		// Token: 0x040012C5 RID: 4805
		private CardSelectorPrefs _prefs;

		// Token: 0x040012C6 RID: 4806
		private Control _previewContainer;

		// Token: 0x040012C7 RID: 4807
		private NTransformPreview _transformPreview;

		// Token: 0x040012C8 RID: 4808
		private NConfirmButton _confirmButton;

		// Token: 0x040012C9 RID: 4809
		private NBackButton _previewCancelButton;

		// Token: 0x040012CA RID: 4810
		private NConfirmButton _previewConfirmButton;

		// Token: 0x040012CB RID: 4811
		private Control _bottomTextContainer;

		// Token: 0x040012CC RID: 4812
		private MegaRichTextLabel _infoLabel;

		// Token: 0x040012CD RID: 4813
		private NTickbox _viewUpgrades;

		// Token: 0x040012CE RID: 4814
		private NBackButton _closeButton;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001619 RID: 5657
		[NullableContext(0)]
		public new class MethodName : NCardGridSelectionScreen.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040055D6 RID: 21974
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshConfirmButtonVisibility' method.
			/// </summary>
			// Token: 0x040055D7 RID: 21975
			public static readonly StringName RefreshConfirmButtonVisibility = "RefreshConfirmButtonVisibility";

			/// <summary>
			/// Cached name for the 'CloseSelection' method.
			/// </summary>
			// Token: 0x040055D8 RID: 21976
			public static readonly StringName CloseSelection = "CloseSelection";

			/// <summary>
			/// Cached name for the 'CancelSelection' method.
			/// </summary>
			// Token: 0x040055D9 RID: 21977
			public static readonly StringName CancelSelection = "CancelSelection";

			/// <summary>
			/// Cached name for the 'ConfirmSelection' method.
			/// </summary>
			// Token: 0x040055DA RID: 21978
			public static readonly StringName ConfirmSelection = "ConfirmSelection";

			/// <summary>
			/// Cached name for the 'OpenPreviewScreen' method.
			/// </summary>
			// Token: 0x040055DB RID: 21979
			public static readonly StringName OpenPreviewScreen = "OpenPreviewScreen";

			/// <summary>
			/// Cached name for the 'CompleteSelection' method.
			/// </summary>
			// Token: 0x040055DC RID: 21980
			public static readonly StringName CompleteSelection = "CompleteSelection";

			/// <summary>
			/// Cached name for the 'ToggleShowUpgrades' method.
			/// </summary>
			// Token: 0x040055DD RID: 21981
			public static readonly StringName ToggleShowUpgrades = "ToggleShowUpgrades";

			/// <summary>
			/// Cached name for the 'OnControllerStateUpdated' method.
			/// </summary>
			// Token: 0x040055DE RID: 21982
			public static readonly StringName OnControllerStateUpdated = "OnControllerStateUpdated";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200161A RID: 5658
		[NullableContext(0)]
		public new class PropertyName : NCardGridSelectionScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040055DF RID: 21983
			public new static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the 'FocusedControlFromTopBar' property.
			/// </summary>
			// Token: 0x040055E0 RID: 21984
			public new static readonly StringName FocusedControlFromTopBar = "FocusedControlFromTopBar";

			/// <summary>
			/// Cached name for the '_previewContainer' field.
			/// </summary>
			// Token: 0x040055E1 RID: 21985
			public static readonly StringName _previewContainer = "_previewContainer";

			/// <summary>
			/// Cached name for the '_transformPreview' field.
			/// </summary>
			// Token: 0x040055E2 RID: 21986
			public static readonly StringName _transformPreview = "_transformPreview";

			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x040055E3 RID: 21987
			public static readonly StringName _confirmButton = "_confirmButton";

			/// <summary>
			/// Cached name for the '_previewCancelButton' field.
			/// </summary>
			// Token: 0x040055E4 RID: 21988
			public static readonly StringName _previewCancelButton = "_previewCancelButton";

			/// <summary>
			/// Cached name for the '_previewConfirmButton' field.
			/// </summary>
			// Token: 0x040055E5 RID: 21989
			public static readonly StringName _previewConfirmButton = "_previewConfirmButton";

			/// <summary>
			/// Cached name for the '_bottomTextContainer' field.
			/// </summary>
			// Token: 0x040055E6 RID: 21990
			public static readonly StringName _bottomTextContainer = "_bottomTextContainer";

			/// <summary>
			/// Cached name for the '_infoLabel' field.
			/// </summary>
			// Token: 0x040055E7 RID: 21991
			public static readonly StringName _infoLabel = "_infoLabel";

			/// <summary>
			/// Cached name for the '_viewUpgrades' field.
			/// </summary>
			// Token: 0x040055E8 RID: 21992
			public static readonly StringName _viewUpgrades = "_viewUpgrades";

			/// <summary>
			/// Cached name for the '_closeButton' field.
			/// </summary>
			// Token: 0x040055E9 RID: 21993
			public static readonly StringName _closeButton = "_closeButton";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200161B RID: 5659
		[NullableContext(0)]
		public new class SignalName : NCardGridSelectionScreen.SignalName
		{
		}
	}
}
