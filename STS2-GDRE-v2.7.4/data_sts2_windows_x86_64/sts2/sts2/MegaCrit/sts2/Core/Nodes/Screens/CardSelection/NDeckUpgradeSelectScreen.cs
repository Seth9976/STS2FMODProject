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
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	/// <summary>
	/// CardSelectionScreen specifically for upgrading cards in your deck.
	/// Managing logic for the extra confirmation screen.
	/// </summary>
	// Token: 0x0200034E RID: 846
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NDeckUpgradeSelectScreen.cs")]
	public sealed class NDeckUpgradeSelectScreen : NCardGridSelectionScreen
	{
		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06003099 RID: 12441 RVA: 0x00166743 File Offset: 0x00164943
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/card_selection/deck_upgrade_select_screen");
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x0600309A RID: 12442 RVA: 0x0016674F File Offset: 0x0016494F
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NDeckUpgradeSelectScreen.ScenePath);
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x0600309B RID: 12443 RVA: 0x0016675B File Offset: 0x0016495B
		private bool UseSingleSelection
		{
			get
			{
				return this._prefs.MaxSelect == 1;
			}
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x0016676C File Offset: 0x0016496C
		public override void _Ready()
		{
			this.ConnectSignalsAndInitGrid();
			this._upgradeSinglePreviewContainer = base.GetNode<Control>("%UpgradeSinglePreviewContainer");
			this._singlePreview = this._upgradeSinglePreviewContainer.GetNode<NUpgradePreview>("UpgradePreview");
			this._singlePreviewCancelButton = this._upgradeSinglePreviewContainer.GetNode<NBackButton>("Cancel");
			this._singlePreviewConfirmButton = this._upgradeSinglePreviewContainer.GetNode<NConfirmButton>("Confirm");
			this._upgradeMultiPreviewContainer = base.GetNode<Control>("%UpgradeMultiPreviewContainer");
			this._multiPreview = this._upgradeMultiPreviewContainer.GetNode<Control>("Cards");
			this._multiPreviewCancelButton = this._upgradeMultiPreviewContainer.GetNode<NBackButton>("Cancel");
			this._multiPreviewConfirmButton = this._upgradeMultiPreviewContainer.GetNode<NConfirmButton>("Confirm");
			this._closeButton = base.GetNode<NBackButton>("%Close");
			this._bottomTextContainer = base.GetNode<Control>("%BottomText");
			this._infoLabel = this._bottomTextContainer.GetNode<MegaRichTextLabel>("%BottomLabel");
			this._infoLabel.Text = this._prefs.Prompt.GetFormattedText();
			this._singlePreviewCancelButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
			this._singlePreviewConfirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
			this._multiPreviewCancelButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CancelSelection)), 0U);
			this._multiPreviewConfirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ConfirmSelection)), 0U);
			this._closeButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseSelection)), 0U);
			if (this._prefs.Cancelable)
			{
				this._closeButton.Enable();
			}
			else
			{
				this._closeButton.Disable();
			}
			this._upgradeSinglePreviewContainer.Visible = false;
			this._upgradeSinglePreviewContainer.MouseFilter = 2L;
			this._upgradeMultiPreviewContainer.Visible = false;
			this._upgradeMultiPreviewContainer.MouseFilter = 2L;
			this._singlePreviewCancelButton.Disable();
			this._singlePreviewConfirmButton.Disable();
			this._multiPreviewCancelButton.Disable();
			this._multiPreviewConfirmButton.Disable();
			this._viewUpgrades = base.GetNode<NTickbox>("%Upgrades");
			this._viewUpgrades.IsTicked = false;
			this._viewUpgrades.Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowUpgrades)), 0U);
			this.OnControllerStateUpdated();
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
			base.GetNode<MegaLabel>("%ViewUpgradesLabel").SetTextAutoSize(new LocString("card_selection", "VIEW_UPGRADES").GetFormattedText());
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x00166ABC File Offset: 0x00164CBC
		public static NDeckUpgradeSelectScreen ShowScreen(IReadOnlyList<CardModel> cards, CardSelectorPrefs prefs, IRunState runState)
		{
			NDeckUpgradeSelectScreen ndeckUpgradeSelectScreen = PreloadManager.Cache.GetScene(NDeckUpgradeSelectScreen.ScenePath).Instantiate<NDeckUpgradeSelectScreen>(0L);
			ndeckUpgradeSelectScreen.Name = "NDeckUpgradeSelectScreen";
			ndeckUpgradeSelectScreen._cards = cards;
			ndeckUpgradeSelectScreen._prefs = prefs;
			ndeckUpgradeSelectScreen._runState = runState;
			NOverlayStack.Instance.Push(ndeckUpgradeSelectScreen);
			return ndeckUpgradeSelectScreen;
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x00166B11 File Offset: 0x00164D11
		protected override IEnumerable<Control> PeekButtonTargets
		{
			get
			{
				return new <>z__ReadOnlyArray<Control>(new Control[] { this._upgradeSinglePreviewContainer, this._upgradeMultiPreviewContainer, this._closeButton });
			}
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x00166B3C File Offset: 0x00164D3C
		protected override void OnCardClicked(CardModel card)
		{
			if (this._selectedCards.Add(card))
			{
				this._grid.HighlightCard(card);
				if (this.UseSingleSelection)
				{
					base.GetViewport().GuiReleaseFocus();
					this._grid.FocusBehaviorRecursive = 1L;
					this._upgradeSinglePreviewContainer.Visible = true;
					this._upgradeSinglePreviewContainer.MouseFilter = 0L;
					this._singlePreview.Card = card;
					this._singlePreviewCancelButton.Enable();
					this._singlePreviewConfirmButton.Enable();
					this._grid.SetCanScroll(false);
					this._closeButton.Disable();
					return;
				}
				if (this._prefs.MaxSelect != this._selectedCards.Count)
				{
					return;
				}
				base.GetViewport().GuiReleaseFocus();
				this._grid.FocusBehaviorRecursive = 1L;
				this._upgradeMultiPreviewContainer.Visible = true;
				this._upgradeMultiPreviewContainer.MouseFilter = 0L;
				this._multiPreviewCancelButton.Enable();
				this._multiPreviewConfirmButton.Enable();
				using (HashSet<CardModel>.Enumerator enumerator = this._selectedCards.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CardModel cardModel = enumerator.Current;
						this._grid.UnhighlightCard(cardModel);
						CardModel cardModel2 = this._runState.CloneCard(cardModel);
						cardModel2.UpgradeInternal();
						cardModel2.UpgradePreviewType = CardUpgradePreviewType.Deck;
						NCard ncard = NCard.Create(cardModel2, ModelVisibility.Visible);
						this._multiPreview.AddChildSafely(NPreviewCardHolder.Create(ncard, true, false));
						ncard.ShowUpgradePreview();
						this._grid.SetCanScroll(false);
						this._closeButton.Disable();
					}
					return;
				}
			}
			this._selectedCards.Remove(card);
			this._grid.UnhighlightCard(card);
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x00166CF4 File Offset: 0x00164EF4
		private void CloseSelection(NButton _)
		{
			this._completionSource.SetResult(Array.Empty<CardModel>());
			this._singlePreviewCancelButton.Disable();
			this._singlePreviewConfirmButton.Disable();
			this._multiPreviewCancelButton.Disable();
			this._multiPreviewConfirmButton.Disable();
			NOverlayStack.Instance.Remove(this);
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x00166D48 File Offset: 0x00164F48
		private void CancelSelection(NButton _)
		{
			if (this.UseSingleSelection)
			{
				this._upgradeSinglePreviewContainer.Visible = false;
				this._upgradeSinglePreviewContainer.MouseFilter = 2L;
				this._singlePreviewCancelButton.Disable();
				this._singlePreviewConfirmButton.Disable();
			}
			else
			{
				this._upgradeMultiPreviewContainer.Visible = false;
				this._upgradeMultiPreviewContainer.MouseFilter = 2L;
				for (int i = 0; i < this._multiPreview.GetChildCount(false); i++)
				{
					this._multiPreview.GetChild(i, false).QueueFreeSafely();
				}
				this._multiPreviewCancelButton.Disable();
				this._multiPreviewConfirmButton.Disable();
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

		// Token: 0x060030A2 RID: 12450 RVA: 0x00166E90 File Offset: 0x00165090
		private void ConfirmSelection(NButton _)
		{
			if (this._selectedCards.Count == 0)
			{
				return;
			}
			this.CheckIfSelectionComplete();
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x00166EA8 File Offset: 0x001650A8
		private void CheckIfSelectionComplete()
		{
			this._singlePreviewCancelButton.Enable();
			this._singlePreviewConfirmButton.Enable();
			if (this._selectedCards.Count >= this._prefs.MaxSelect)
			{
				this._completionSource.SetResult(this._selectedCards);
				NOverlayStack.Instance.Remove(this);
			}
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x00166EFF File Offset: 0x001650FF
		private void ToggleShowUpgrades(NTickbox tickbox)
		{
			this._grid.IsShowingUpgrades = tickbox.IsTicked;
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x00166F12 File Offset: 0x00165112
		private void OnControllerStateUpdated()
		{
			this._viewUpgrades.Visible = !NControllerManager.Instance.IsUsingController;
			if (NControllerManager.Instance.IsUsingController)
			{
				this._viewUpgrades.IsTicked = false;
				this.ToggleShowUpgrades(this._viewUpgrades);
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060030A6 RID: 12454 RVA: 0x00166F50 File Offset: 0x00165150
		[Nullable(2)]
		public override Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._upgradeSinglePreviewContainer.Visible || this._upgradeMultiPreviewContainer.Visible)
				{
					return null;
				}
				return this._grid.DefaultFocusedControl;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x00166F79 File Offset: 0x00165179
		[Nullable(2)]
		public override Control FocusedControlFromTopBar
		{
			[NullableContext(2)]
			get
			{
				if (this._upgradeSinglePreviewContainer.Visible || this._upgradeMultiPreviewContainer.Visible)
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
		// Token: 0x060030A8 RID: 12456 RVA: 0x00166FA4 File Offset: 0x001651A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NDeckUpgradeSelectScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckUpgradeSelectScreen.MethodName.CloseSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckUpgradeSelectScreen.MethodName.CancelSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckUpgradeSelectScreen.MethodName.ConfirmSelection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckUpgradeSelectScreen.MethodName.CheckIfSelectionComplete, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckUpgradeSelectScreen.MethodName.ToggleShowUpgrades, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckUpgradeSelectScreen.MethodName.OnControllerStateUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060030A9 RID: 12457 RVA: 0x001671C4 File Offset: 0x001653C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDeckUpgradeSelectScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckUpgradeSelectScreen.MethodName.CloseSelection && args.Count == 1)
			{
				this.CloseSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckUpgradeSelectScreen.MethodName.CancelSelection && args.Count == 1)
			{
				this.CancelSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckUpgradeSelectScreen.MethodName.ConfirmSelection && args.Count == 1)
			{
				this.ConfirmSelection(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckUpgradeSelectScreen.MethodName.CheckIfSelectionComplete && args.Count == 0)
			{
				this.CheckIfSelectionComplete();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckUpgradeSelectScreen.MethodName.ToggleShowUpgrades && args.Count == 1)
			{
				this.ToggleShowUpgrades(VariantUtils.ConvertTo<NTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckUpgradeSelectScreen.MethodName.OnControllerStateUpdated && args.Count == 0)
			{
				this.OnControllerStateUpdated();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060030AA RID: 12458 RVA: 0x00167318 File Offset: 0x00165518
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDeckUpgradeSelectScreen.MethodName._Ready || (ref method) == NDeckUpgradeSelectScreen.MethodName.CloseSelection || (ref method) == NDeckUpgradeSelectScreen.MethodName.CancelSelection || (ref method) == NDeckUpgradeSelectScreen.MethodName.ConfirmSelection || (ref method) == NDeckUpgradeSelectScreen.MethodName.CheckIfSelectionComplete || (ref method) == NDeckUpgradeSelectScreen.MethodName.ToggleShowUpgrades || (ref method) == NDeckUpgradeSelectScreen.MethodName.OnControllerStateUpdated || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060030AB RID: 12459 RVA: 0x00167398 File Offset: 0x00165598
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._upgradeSinglePreviewContainer)
			{
				this._upgradeSinglePreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._singlePreview)
			{
				this._singlePreview = VariantUtils.ConvertTo<NUpgradePreview>(ref value);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._singlePreviewCancelButton)
			{
				this._singlePreviewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._singlePreviewConfirmButton)
			{
				this._singlePreviewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._viewUpgrades)
			{
				this._viewUpgrades = VariantUtils.ConvertTo<NTickbox>(ref value);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._bottomTextContainer)
			{
				this._bottomTextContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._infoLabel)
			{
				this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._upgradeMultiPreviewContainer)
			{
				this._upgradeMultiPreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._multiPreview)
			{
				this._multiPreview = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._multiPreviewCancelButton)
			{
				this._multiPreviewCancelButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._multiPreviewConfirmButton)
			{
				this._multiPreviewConfirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._closeButton)
			{
				this._closeButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060030AC RID: 12460 RVA: 0x001674F4 File Offset: 0x001656F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName.UseSingleSelection)
			{
				bool useSingleSelection = this.UseSingleSelection;
				value = VariantUtils.CreateFrom<bool>(ref useSingleSelection);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName.FocusedControlFromTopBar)
			{
				Control control = this.FocusedControlFromTopBar;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._upgradeSinglePreviewContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._upgradeSinglePreviewContainer);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._singlePreview)
			{
				value = VariantUtils.CreateFrom<NUpgradePreview>(ref this._singlePreview);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._singlePreviewCancelButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._singlePreviewCancelButton);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._singlePreviewConfirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._singlePreviewConfirmButton);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._viewUpgrades)
			{
				value = VariantUtils.CreateFrom<NTickbox>(ref this._viewUpgrades);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._bottomTextContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bottomTextContainer);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._infoLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._upgradeMultiPreviewContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._upgradeMultiPreviewContainer);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._multiPreview)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._multiPreview);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._multiPreviewCancelButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._multiPreviewCancelButton);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._multiPreviewConfirmButton)
			{
				value = VariantUtils.CreateFrom<NConfirmButton>(ref this._multiPreviewConfirmButton);
				return true;
			}
			if ((ref name) == NDeckUpgradeSelectScreen.PropertyName._closeButton)
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
		// Token: 0x060030AD RID: 12461 RVA: 0x001676F4 File Offset: 0x001658F4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NDeckUpgradeSelectScreen.PropertyName.UseSingleSelection, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._upgradeSinglePreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._singlePreview, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._singlePreviewCancelButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._singlePreviewConfirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._viewUpgrades, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._bottomTextContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._infoLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._upgradeMultiPreviewContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._multiPreview, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._multiPreviewCancelButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._multiPreviewConfirmButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName._closeButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckUpgradeSelectScreen.PropertyName.FocusedControlFromTopBar, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060030AE RID: 12462 RVA: 0x001678F8 File Offset: 0x00165AF8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._upgradeSinglePreviewContainer, Variant.From<Control>(ref this._upgradeSinglePreviewContainer));
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreview, Variant.From<NUpgradePreview>(ref this._singlePreview));
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreviewCancelButton, Variant.From<NBackButton>(ref this._singlePreviewCancelButton));
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreviewConfirmButton, Variant.From<NConfirmButton>(ref this._singlePreviewConfirmButton));
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._viewUpgrades, Variant.From<NTickbox>(ref this._viewUpgrades));
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._bottomTextContainer, Variant.From<Control>(ref this._bottomTextContainer));
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._upgradeMultiPreviewContainer, Variant.From<Control>(ref this._upgradeMultiPreviewContainer));
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreview, Variant.From<Control>(ref this._multiPreview));
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreviewCancelButton, Variant.From<NBackButton>(ref this._multiPreviewCancelButton));
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreviewConfirmButton, Variant.From<NConfirmButton>(ref this._multiPreviewConfirmButton));
			info.AddProperty(NDeckUpgradeSelectScreen.PropertyName._closeButton, Variant.From<NBackButton>(ref this._closeButton));
		}

		/// <inheritdoc />
		// Token: 0x060030AF RID: 12463 RVA: 0x00167A14 File Offset: 0x00165C14
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._upgradeSinglePreviewContainer, ref variant))
			{
				this._upgradeSinglePreviewContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreview, ref variant2))
			{
				this._singlePreview = variant2.As<NUpgradePreview>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreviewCancelButton, ref variant3))
			{
				this._singlePreviewCancelButton = variant3.As<NBackButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._singlePreviewConfirmButton, ref variant4))
			{
				this._singlePreviewConfirmButton = variant4.As<NConfirmButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._viewUpgrades, ref variant5))
			{
				this._viewUpgrades = variant5.As<NTickbox>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._bottomTextContainer, ref variant6))
			{
				this._bottomTextContainer = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._infoLabel, ref variant7))
			{
				this._infoLabel = variant7.As<MegaRichTextLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._upgradeMultiPreviewContainer, ref variant8))
			{
				this._upgradeMultiPreviewContainer = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreview, ref variant9))
			{
				this._multiPreview = variant9.As<Control>();
			}
			Variant variant10;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreviewCancelButton, ref variant10))
			{
				this._multiPreviewCancelButton = variant10.As<NBackButton>();
			}
			Variant variant11;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._multiPreviewConfirmButton, ref variant11))
			{
				this._multiPreviewConfirmButton = variant11.As<NConfirmButton>();
			}
			Variant variant12;
			if (info.TryGetProperty(NDeckUpgradeSelectScreen.PropertyName._closeButton, ref variant12))
			{
				this._closeButton = variant12.As<NBackButton>();
			}
		}

		// Token: 0x040012CF RID: 4815
		private readonly HashSet<CardModel> _selectedCards = new HashSet<CardModel>();

		// Token: 0x040012D0 RID: 4816
		private CardSelectorPrefs _prefs;

		// Token: 0x040012D1 RID: 4817
		private IRunState _runState;

		// Token: 0x040012D2 RID: 4818
		private Control _upgradeSinglePreviewContainer;

		// Token: 0x040012D3 RID: 4819
		private NUpgradePreview _singlePreview;

		// Token: 0x040012D4 RID: 4820
		private NBackButton _singlePreviewCancelButton;

		// Token: 0x040012D5 RID: 4821
		private NConfirmButton _singlePreviewConfirmButton;

		// Token: 0x040012D6 RID: 4822
		private NTickbox _viewUpgrades;

		// Token: 0x040012D7 RID: 4823
		private Control _bottomTextContainer;

		// Token: 0x040012D8 RID: 4824
		private MegaRichTextLabel _infoLabel;

		// Token: 0x040012D9 RID: 4825
		private Control _upgradeMultiPreviewContainer;

		// Token: 0x040012DA RID: 4826
		private Control _multiPreview;

		// Token: 0x040012DB RID: 4827
		private NBackButton _multiPreviewCancelButton;

		// Token: 0x040012DC RID: 4828
		private NConfirmButton _multiPreviewConfirmButton;

		// Token: 0x040012DD RID: 4829
		private NBackButton _closeButton;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200161C RID: 5660
		[NullableContext(0)]
		public new class MethodName : NCardGridSelectionScreen.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040055EA RID: 21994
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'CloseSelection' method.
			/// </summary>
			// Token: 0x040055EB RID: 21995
			public static readonly StringName CloseSelection = "CloseSelection";

			/// <summary>
			/// Cached name for the 'CancelSelection' method.
			/// </summary>
			// Token: 0x040055EC RID: 21996
			public static readonly StringName CancelSelection = "CancelSelection";

			/// <summary>
			/// Cached name for the 'ConfirmSelection' method.
			/// </summary>
			// Token: 0x040055ED RID: 21997
			public static readonly StringName ConfirmSelection = "ConfirmSelection";

			/// <summary>
			/// Cached name for the 'CheckIfSelectionComplete' method.
			/// </summary>
			// Token: 0x040055EE RID: 21998
			public static readonly StringName CheckIfSelectionComplete = "CheckIfSelectionComplete";

			/// <summary>
			/// Cached name for the 'ToggleShowUpgrades' method.
			/// </summary>
			// Token: 0x040055EF RID: 21999
			public static readonly StringName ToggleShowUpgrades = "ToggleShowUpgrades";

			/// <summary>
			/// Cached name for the 'OnControllerStateUpdated' method.
			/// </summary>
			// Token: 0x040055F0 RID: 22000
			public static readonly StringName OnControllerStateUpdated = "OnControllerStateUpdated";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200161D RID: 5661
		[NullableContext(0)]
		public new class PropertyName : NCardGridSelectionScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the 'UseSingleSelection' property.
			/// </summary>
			// Token: 0x040055F1 RID: 22001
			public static readonly StringName UseSingleSelection = "UseSingleSelection";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040055F2 RID: 22002
			public new static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the 'FocusedControlFromTopBar' property.
			/// </summary>
			// Token: 0x040055F3 RID: 22003
			public new static readonly StringName FocusedControlFromTopBar = "FocusedControlFromTopBar";

			/// <summary>
			/// Cached name for the '_upgradeSinglePreviewContainer' field.
			/// </summary>
			// Token: 0x040055F4 RID: 22004
			public static readonly StringName _upgradeSinglePreviewContainer = "_upgradeSinglePreviewContainer";

			/// <summary>
			/// Cached name for the '_singlePreview' field.
			/// </summary>
			// Token: 0x040055F5 RID: 22005
			public static readonly StringName _singlePreview = "_singlePreview";

			/// <summary>
			/// Cached name for the '_singlePreviewCancelButton' field.
			/// </summary>
			// Token: 0x040055F6 RID: 22006
			public static readonly StringName _singlePreviewCancelButton = "_singlePreviewCancelButton";

			/// <summary>
			/// Cached name for the '_singlePreviewConfirmButton' field.
			/// </summary>
			// Token: 0x040055F7 RID: 22007
			public static readonly StringName _singlePreviewConfirmButton = "_singlePreviewConfirmButton";

			/// <summary>
			/// Cached name for the '_viewUpgrades' field.
			/// </summary>
			// Token: 0x040055F8 RID: 22008
			public static readonly StringName _viewUpgrades = "_viewUpgrades";

			/// <summary>
			/// Cached name for the '_bottomTextContainer' field.
			/// </summary>
			// Token: 0x040055F9 RID: 22009
			public static readonly StringName _bottomTextContainer = "_bottomTextContainer";

			/// <summary>
			/// Cached name for the '_infoLabel' field.
			/// </summary>
			// Token: 0x040055FA RID: 22010
			public static readonly StringName _infoLabel = "_infoLabel";

			/// <summary>
			/// Cached name for the '_upgradeMultiPreviewContainer' field.
			/// </summary>
			// Token: 0x040055FB RID: 22011
			public static readonly StringName _upgradeMultiPreviewContainer = "_upgradeMultiPreviewContainer";

			/// <summary>
			/// Cached name for the '_multiPreview' field.
			/// </summary>
			// Token: 0x040055FC RID: 22012
			public static readonly StringName _multiPreview = "_multiPreview";

			/// <summary>
			/// Cached name for the '_multiPreviewCancelButton' field.
			/// </summary>
			// Token: 0x040055FD RID: 22013
			public static readonly StringName _multiPreviewCancelButton = "_multiPreviewCancelButton";

			/// <summary>
			/// Cached name for the '_multiPreviewConfirmButton' field.
			/// </summary>
			// Token: 0x040055FE RID: 22014
			public static readonly StringName _multiPreviewConfirmButton = "_multiPreviewConfirmButton";

			/// <summary>
			/// Cached name for the '_closeButton' field.
			/// </summary>
			// Token: 0x040055FF RID: 22015
			public static readonly StringName _closeButton = "_closeButton";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200161E RID: 5662
		[NullableContext(0)]
		public new class SignalName : NCardGridSelectionScreen.SignalName
		{
		}
	}
}
