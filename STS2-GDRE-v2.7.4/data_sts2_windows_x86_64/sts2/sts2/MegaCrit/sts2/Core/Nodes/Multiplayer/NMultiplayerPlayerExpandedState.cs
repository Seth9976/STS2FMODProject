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
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	/// <summary>
	/// Displays the player's relics and deck.
	/// Shown when a player clicks a remote player.
	/// </summary>
	// Token: 0x0200038E RID: 910
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerPlayerExpandedState.cs")]
	public class NMultiplayerPlayerExpandedState : Control, ICapstoneScreen, IScreenContext
	{
		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x0600361B RID: 13851 RVA: 0x00193404 File Offset: 0x00191604
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x0600361C RID: 13852 RVA: 0x00193407 File Offset: 0x00191607
		public NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.RemotePlayerExpandedState;
			}
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x0019340C File Offset: 0x0019160C
		public static NMultiplayerPlayerExpandedState Create(Player player)
		{
			NMultiplayerPlayerExpandedState nmultiplayerPlayerExpandedState = PreloadManager.Cache.GetScene(NMultiplayerPlayerExpandedState._scenePath).Instantiate<NMultiplayerPlayerExpandedState>(0L);
			nmultiplayerPlayerExpandedState._player = player;
			return nmultiplayerPlayerExpandedState;
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x00193438 File Offset: 0x00191638
		public void AfterCapstoneOpened()
		{
			this._backButton.Enable();
			NGlobalUi globalUi = NRun.Instance.GlobalUi;
			globalUi.TopBar.AnimHide();
			globalUi.RelicInventory.AnimHide();
			globalUi.MultiplayerPlayerContainer.AnimHide();
			globalUi.MoveChildSafely(globalUi.AboveTopBarVfxContainer, globalUi.CapstoneContainer.GetIndex(false));
			globalUi.MoveChildSafely(globalUi.CardPreviewContainer, globalUi.CapstoneContainer.GetIndex(false));
			globalUi.MoveChildSafely(globalUi.MessyCardPreviewContainer, globalUi.CapstoneContainer.GetIndex(false));
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x001934C4 File Offset: 0x001916C4
		public void AfterCapstoneClosed()
		{
			NGlobalUi globalUi = NRun.Instance.GlobalUi;
			globalUi.TopBar.AnimShow();
			globalUi.RelicInventory.AnimShow();
			globalUi.MultiplayerPlayerContainer.AnimShow();
			globalUi.MoveChildSafely(globalUi.AboveTopBarVfxContainer, globalUi.TopBar.GetIndex(false) + 1);
			globalUi.MoveChildSafely(globalUi.CardPreviewContainer, globalUi.TopBar.GetIndex(false) + 1);
			globalUi.MoveChildSafely(globalUi.MessyCardPreviewContainer, globalUi.TopBar.GetIndex(false) + 1);
			this.QueueFreeSafely();
			this._backButton.Disable();
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x0019355C File Offset: 0x0019175C
		public override void _Ready()
		{
			this._playerNameLabel = base.GetNode<MegaRichTextLabel>("%PlayerNameLabel");
			this._cardsHeader = base.GetNode<MegaRichTextLabel>("%CardsHeader");
			this._cardContainer = base.GetNode<Control>("%CardContainer");
			this._relicsHeader = base.GetNode<MegaRichTextLabel>("%RelicsHeader");
			this._relicContainer = base.GetNode<Control>("%RelicContainer");
			this._potionsHeader = base.GetNode<MegaRichTextLabel>("%PotionsHeader");
			this._potionContainer = base.GetNode<Control>("%PotionContainer");
			this._backButton = base.GetNode<NBackButton>("%BackButton");
			LocString locString = new LocString("gameplay_ui", "MULTIPLAYER_EXPANDED_STATE.title");
			locString.Add("PlayerName", PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, this._player.NetId));
			locString.Add("Character", this._player.Character.Title);
			this._playerNameLabel.Text = locString.GetFormattedText();
			LocString locString2 = new LocString("gameplay_ui", "MULTIPLAYER_EXPANDED_STATE.relicHeader");
			this._relicsHeader.Text = locString2.GetFormattedText();
			LocString locString3 = new LocString("gameplay_ui", "MULTIPLAYER_EXPANDED_STATE.cardHeader");
			this._cardsHeader.Text = locString3.GetFormattedText();
			LocString locString4 = new LocString("gameplay_ui", "MULTIPLAYER_EXPANDED_STATE.potionHeader");
			this._potionsHeader.Text = locString4.GetFormattedText();
			foreach (RelicModel relicModel in this._player.Relics)
			{
				NRelicBasicHolder holder = NRelicBasicHolder.Create(relicModel);
				this._relicContainer.AddChildSafely(holder);
				holder.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
				{
					this.OnRelicClicked(holder.Relic);
				}), 0U);
				holder.MouseDefaultCursorShape = 16L;
			}
			foreach (PotionModel potionModel in this._player.Potions)
			{
				NPotionHolder npotionHolder = NPotionHolder.Create(false);
				NPotion npotion = NPotion.Create(potionModel);
				this._potionContainer.AddChildSafely(npotionHolder);
				npotionHolder.AddPotion(npotion);
				npotion.Position = Vector2.Zero;
			}
			this._cards.Clear();
			this._cards.AddRange(this._player.Deck.Cards);
			foreach (IGrouping<NMultiplayerPlayerExpandedState.CardGroupKey, CardModel> grouping in from x in this._player.Deck.Cards
				group x by new NMultiplayerPlayerExpandedState.CardGroupKey(x))
			{
				NDeckHistoryEntry ndeckHistoryEntry = NDeckHistoryEntry.Create(grouping.First<CardModel>(), grouping.Count<CardModel>());
				ndeckHistoryEntry.Connect(NDeckHistoryEntry.SignalName.Clicked, Callable.From<NDeckHistoryEntry>(new Action<NDeckHistoryEntry>(this.ShowEntry)), 0U);
				this._cardContainer.AddChildSafely(ndeckHistoryEntry);
			}
			this._backButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.BackButtonPressed)), 0U);
			this.UpdateNavigation();
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x001938F8 File Offset: 0x00191AF8
		public override void _Input(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (NDevConsole.IsConsoleVisible)
			{
				return;
			}
			if (!NControllerManager.Instance.IsUsingController)
			{
				return;
			}
			Control control = base.GetViewport().GuiGetFocusOwner();
			bool flag = control is TextEdit || control is LineEdit;
			if (flag)
			{
				return;
			}
			if (!ActiveScreenContext.Instance.IsCurrent(this))
			{
				return;
			}
			Control control2 = base.GetViewport().GuiGetFocusOwner();
			if (control2 != null && base.IsAncestorOf(control2))
			{
				return;
			}
			if (inputEvent.IsActionPressed(MegaInput.left, false, false) || inputEvent.IsActionPressed(MegaInput.right, false, false) || inputEvent.IsActionPressed(MegaInput.up, false, false) || inputEvent.IsActionPressed(MegaInput.down, false, false) || inputEvent.IsActionPressed(MegaInput.select, false, false))
			{
				this._relicContainer.GetChild<NRelicBasicHolder>(0, false).TryGrabFocus();
				Viewport viewport = base.GetViewport();
				if (viewport == null)
				{
					return;
				}
				viewport.SetInputAsHandled();
			}
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x001939DD File Offset: 0x00191BDD
		private void ShowEntry(NDeckHistoryEntry entry)
		{
			NGame.Instance.GetInspectCardScreen().Open(this._cards, this._cards.IndexOf(entry.Card), false);
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x00193A06 File Offset: 0x00191C06
		private void BackButtonPressed(NButton _)
		{
			NCapstoneContainer.Instance.Close();
		}

		/// <summary>
		/// Opens the Inspect Relic screen and allows the player to paginate through the rest of the top bar relics.
		/// </summary>
		/// <param name="node"></param>
		// Token: 0x06003624 RID: 13860 RVA: 0x00193A14 File Offset: 0x00191C14
		private void OnRelicClicked(NRelic node)
		{
			List<RelicModel> list = new List<RelicModel>();
			foreach (NRelicBasicHolder nrelicBasicHolder in this._relicContainer.GetChildren(false).OfType<NRelicBasicHolder>())
			{
				list.Add(nrelicBasicHolder.Relic.Model);
			}
			NGame.Instance.GetInspectRelicScreen().Open(list, node.Model);
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06003625 RID: 13861 RVA: 0x00193A94 File Offset: 0x00191C94
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x00193A98 File Offset: 0x00191C98
		private void UpdateNavigation()
		{
			for (int i = 0; i < this._relicContainer.GetChildCount(false); i++)
			{
				NRelicBasicHolder child = this._relicContainer.GetChild<NRelicBasicHolder>(i, false);
				child.FocusNeighborLeft = ((i > 0) ? this._relicContainer.GetChild<NRelicBasicHolder>(i - 1, false).GetPath() : this._relicContainer.GetChild<NRelicBasicHolder>(i, false).GetPath());
				child.FocusNeighborRight = ((i < this._relicContainer.GetChildCount(false) - 1) ? this._relicContainer.GetChild<NRelicBasicHolder>(i + 1, false).GetPath() : this._relicContainer.GetChild<NRelicBasicHolder>(i, false).GetPath());
				child.FocusNeighborTop = child.GetPath();
				if (this._potionContainer.GetChildCount(false) > 0)
				{
					Control control = child;
					Control child2 = this._potionContainer.GetChild<Control>(Mathf.Min(i, this._potionContainer.GetChildCount(false) - 1), false);
					control.FocusNeighborBottom = ((child2 != null) ? child2.GetPath() : null);
				}
				else if (this._cardContainer.GetChildCount(false) > 0)
				{
					Control control2 = child;
					Control child3 = this._cardContainer.GetChild<Control>(Mathf.Min(i, this._cardContainer.GetChildCount(false) - 1), false);
					control2.FocusNeighborBottom = ((child3 != null) ? child3.GetPath() : null);
				}
				else
				{
					child.FocusNeighborBottom = child.GetPath();
				}
			}
			for (int j = 0; j < this._potionContainer.GetChildCount(false); j++)
			{
				NPotionHolder child4 = this._potionContainer.GetChild<NPotionHolder>(j, false);
				child4.FocusNeighborLeft = ((j > 0) ? this._potionContainer.GetChild<NPotionHolder>(j - 1, false).GetPath() : this._potionContainer.GetChild<NPotionHolder>(j, false).GetPath());
				child4.FocusNeighborRight = ((j < this._potionContainer.GetChildCount(false) - 1) ? this._potionContainer.GetChild<NPotionHolder>(j + 1, false).GetPath() : this._potionContainer.GetChild<NPotionHolder>(j, false).GetPath());
				if (this._relicContainer.GetChildCount(false) > 0)
				{
					Control control3 = child4;
					Control child5 = this._relicContainer.GetChild<Control>(Mathf.Min(j, this._relicContainer.GetChildCount(false) - 1), false);
					control3.FocusNeighborTop = ((child5 != null) ? child5.GetPath() : null);
				}
				else
				{
					child4.FocusNeighborTop = child4.GetPath();
				}
				if (this._cardContainer.GetChildCount(false) > 0)
				{
					Control control4 = child4;
					Control child6 = this._cardContainer.GetChild<Control>(Mathf.Min(j, this._cardContainer.GetChildCount(false) - 1), false);
					control4.FocusNeighborBottom = ((child6 != null) ? child6.GetPath() : null);
				}
				else
				{
					child4.FocusNeighborBottom = child4.GetPath();
				}
			}
			for (int k = 0; k < this._cardContainer.GetChildCount(false); k++)
			{
				NDeckHistoryEntry child7 = this._cardContainer.GetChild<NDeckHistoryEntry>(k, false);
				child7.FocusNeighborLeft = ((k > 0) ? this._cardContainer.GetChild<NDeckHistoryEntry>(k - 1, false).GetPath() : this._cardContainer.GetChild<NDeckHistoryEntry>(k, false).GetPath());
				child7.FocusNeighborRight = ((k < this._cardContainer.GetChildCount(false) - 1) ? this._cardContainer.GetChild<NDeckHistoryEntry>(k + 1, false).GetPath() : this._cardContainer.GetChild<NDeckHistoryEntry>(k, false).GetPath());
				if (this._potionContainer.GetChildCount(false) > 0)
				{
					Control control5 = child7;
					Control child8 = this._potionContainer.GetChild<Control>(Mathf.Min(k, this._potionContainer.GetChildCount(false) - 1), false);
					control5.FocusNeighborTop = ((child8 != null) ? child8.GetPath() : null);
				}
				else if (this._relicContainer.GetChildCount(false) > 0)
				{
					Control control6 = child7;
					Control child9 = this._relicContainer.GetChild<Control>(Mathf.Min(k, this._relicContainer.GetChildCount(false) - 1), false);
					control6.FocusNeighborTop = ((child9 != null) ? child9.GetPath() : null);
				}
				else
				{
					child7.FocusNeighborTop = child7.GetPath();
				}
				child7.FocusNeighborBottom = child7.GetPath();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003627 RID: 13863 RVA: 0x00193E6C File Offset: 0x0019206C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerExpandedState.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerPlayerExpandedState.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.ShowEntry, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "entry", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.BackButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.OnRelicClicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMultiplayerPlayerExpandedState.MethodName.UpdateNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003628 RID: 13864 RVA: 0x001940BC File Offset: 0x001922BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneOpened && args.Count == 0)
			{
				this.AfterCapstoneOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneClosed && args.Count == 0)
			{
				this.AfterCapstoneClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerExpandedState.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerExpandedState.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerExpandedState.MethodName.ShowEntry && args.Count == 1)
			{
				this.ShowEntry(VariantUtils.ConvertTo<NDeckHistoryEntry>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerExpandedState.MethodName.BackButtonPressed && args.Count == 1)
			{
				this.BackButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerExpandedState.MethodName.OnRelicClicked && args.Count == 1)
			{
				this.OnRelicClicked(VariantUtils.ConvertTo<NRelic>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerPlayerExpandedState.MethodName.UpdateNavigation && args.Count == 0)
			{
				this.UpdateNavigation();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003629 RID: 13865 RVA: 0x00194234 File Offset: 0x00192434
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneOpened || (ref method) == NMultiplayerPlayerExpandedState.MethodName.AfterCapstoneClosed || (ref method) == NMultiplayerPlayerExpandedState.MethodName._Ready || (ref method) == NMultiplayerPlayerExpandedState.MethodName._Input || (ref method) == NMultiplayerPlayerExpandedState.MethodName.ShowEntry || (ref method) == NMultiplayerPlayerExpandedState.MethodName.BackButtonPressed || (ref method) == NMultiplayerPlayerExpandedState.MethodName.OnRelicClicked || (ref method) == NMultiplayerPlayerExpandedState.MethodName.UpdateNavigation || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600362A RID: 13866 RVA: 0x001942C0 File Offset: 0x001924C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._playerNameLabel)
			{
				this._playerNameLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._cardsHeader)
			{
				this._cardsHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._cardContainer)
			{
				this._cardContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._potionsHeader)
			{
				this._potionsHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._potionContainer)
			{
				this._potionContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._relicsHeader)
			{
				this._relicsHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._relicContainer)
			{
				this._relicContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600362B RID: 13867 RVA: 0x001943B0 File Offset: 0x001925B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._playerNameLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._playerNameLabel);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._cardsHeader)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._cardsHeader);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._cardContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardContainer);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._potionsHeader)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._potionsHeader);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._potionContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._potionContainer);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._relicsHeader)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._relicsHeader);
				return true;
			}
			if ((ref name) == NMultiplayerPlayerExpandedState.PropertyName._relicContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._relicContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600362C RID: 13868 RVA: 0x00194530 File Offset: 0x00192730
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMultiplayerPlayerExpandedState.PropertyName._playerNameLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerExpandedState.PropertyName._cardsHeader, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerExpandedState.PropertyName._cardContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerExpandedState.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerExpandedState.PropertyName._potionsHeader, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerExpandedState.PropertyName._potionContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerExpandedState.PropertyName._relicsHeader, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerExpandedState.PropertyName._relicContainer, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMultiplayerPlayerExpandedState.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(2L, NMultiplayerPlayerExpandedState.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMultiplayerPlayerExpandedState.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600362D RID: 13869 RVA: 0x001946B0 File Offset: 0x001928B0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._playerNameLabel, Variant.From<MegaRichTextLabel>(ref this._playerNameLabel));
			info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._cardsHeader, Variant.From<MegaRichTextLabel>(ref this._cardsHeader));
			info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._cardContainer, Variant.From<Control>(ref this._cardContainer));
			info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._potionsHeader, Variant.From<MegaRichTextLabel>(ref this._potionsHeader));
			info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._potionContainer, Variant.From<Control>(ref this._potionContainer));
			info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._relicsHeader, Variant.From<MegaRichTextLabel>(ref this._relicsHeader));
			info.AddProperty(NMultiplayerPlayerExpandedState.PropertyName._relicContainer, Variant.From<Control>(ref this._relicContainer));
		}

		/// <inheritdoc />
		// Token: 0x0600362E RID: 13870 RVA: 0x00194774 File Offset: 0x00192974
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._playerNameLabel, ref variant))
			{
				this._playerNameLabel = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._cardsHeader, ref variant2))
			{
				this._cardsHeader = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._cardContainer, ref variant3))
			{
				this._cardContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._backButton, ref variant4))
			{
				this._backButton = variant4.As<NBackButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._potionsHeader, ref variant5))
			{
				this._potionsHeader = variant5.As<MegaRichTextLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._potionContainer, ref variant6))
			{
				this._potionContainer = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._relicsHeader, ref variant7))
			{
				this._relicsHeader = variant7.As<MegaRichTextLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMultiplayerPlayerExpandedState.PropertyName._relicContainer, ref variant8))
			{
				this._relicContainer = variant8.As<Control>();
			}
		}

		// Token: 0x040014E9 RID: 5353
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/multiplayer_player_expanded_state");

		// Token: 0x040014EA RID: 5354
		private MegaRichTextLabel _playerNameLabel;

		// Token: 0x040014EB RID: 5355
		private MegaRichTextLabel _cardsHeader;

		// Token: 0x040014EC RID: 5356
		private Control _cardContainer;

		// Token: 0x040014ED RID: 5357
		private NBackButton _backButton;

		// Token: 0x040014EE RID: 5358
		private MegaRichTextLabel _potionsHeader;

		// Token: 0x040014EF RID: 5359
		private Control _potionContainer;

		// Token: 0x040014F0 RID: 5360
		private MegaRichTextLabel _relicsHeader;

		// Token: 0x040014F1 RID: 5361
		private Control _relicContainer;

		// Token: 0x040014F2 RID: 5362
		private Player _player;

		// Token: 0x040014F3 RID: 5363
		private List<CardModel> _cards = new List<CardModel>();

		// Token: 0x0200171C RID: 5916
		[Nullable(0)]
		private class CardGroupKey
		{
			// Token: 0x0600A617 RID: 42519 RVA: 0x0034B166 File Offset: 0x00349366
			public CardGroupKey(CardModel card)
			{
				this._card = card;
			}

			// Token: 0x0600A618 RID: 42520 RVA: 0x0034B178 File Offset: 0x00349378
			[NullableContext(2)]
			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (obj.GetType() != base.GetType())
				{
					return false;
				}
				NMultiplayerPlayerExpandedState.CardGroupKey cardGroupKey = (NMultiplayerPlayerExpandedState.CardGroupKey)obj;
				if (this._card.Id.Equals(cardGroupKey._card.Id) && this._card.CurrentUpgradeLevel == cardGroupKey._card.CurrentUpgradeLevel)
				{
					EnchantmentModel enchantment = this._card.Enchantment;
					ModelId modelId = ((enchantment != null) ? enchantment.Id : null);
					EnchantmentModel enchantment2 = cardGroupKey._card.Enchantment;
					if (modelId == ((enchantment2 != null) ? enchantment2.Id : null))
					{
						EnchantmentModel enchantment3 = this._card.Enchantment;
						int? num = ((enchantment3 != null) ? new int?(enchantment3.Amount) : null);
						EnchantmentModel enchantment4 = cardGroupKey._card.Enchantment;
						int? num2 = ((enchantment4 != null) ? new int?(enchantment4.Amount) : null);
						return (num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null));
					}
				}
				return false;
			}

			// Token: 0x0600A619 RID: 42521 RVA: 0x0034B284 File Offset: 0x00349484
			public override int GetHashCode()
			{
				ModelId id = this._card.Id;
				int currentUpgradeLevel = this._card.CurrentUpgradeLevel;
				EnchantmentModel enchantment = this._card.Enchantment;
				ModelId modelId = ((enchantment != null) ? enchantment.Id : null);
				EnchantmentModel enchantment2 = this._card.Enchantment;
				return HashCode.Combine<ModelId, int, ModelId, int?>(id, currentUpgradeLevel, modelId, (enchantment2 != null) ? new int?(enchantment2.Amount) : null);
			}

			// Token: 0x040059FE RID: 23038
			private readonly CardModel _card;
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200171D RID: 5917
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'AfterCapstoneOpened' method.
			/// </summary>
			// Token: 0x040059FF RID: 23039
			public static readonly StringName AfterCapstoneOpened = "AfterCapstoneOpened";

			/// <summary>
			/// Cached name for the 'AfterCapstoneClosed' method.
			/// </summary>
			// Token: 0x04005A00 RID: 23040
			public static readonly StringName AfterCapstoneClosed = "AfterCapstoneClosed";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005A01 RID: 23041
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005A02 RID: 23042
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'ShowEntry' method.
			/// </summary>
			// Token: 0x04005A03 RID: 23043
			public static readonly StringName ShowEntry = "ShowEntry";

			/// <summary>
			/// Cached name for the 'BackButtonPressed' method.
			/// </summary>
			// Token: 0x04005A04 RID: 23044
			public static readonly StringName BackButtonPressed = "BackButtonPressed";

			/// <summary>
			/// Cached name for the 'OnRelicClicked' method.
			/// </summary>
			// Token: 0x04005A05 RID: 23045
			public static readonly StringName OnRelicClicked = "OnRelicClicked";

			/// <summary>
			/// Cached name for the 'UpdateNavigation' method.
			/// </summary>
			// Token: 0x04005A06 RID: 23046
			public static readonly StringName UpdateNavigation = "UpdateNavigation";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200171E RID: 5918
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04005A07 RID: 23047
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x04005A08 RID: 23048
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005A09 RID: 23049
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_playerNameLabel' field.
			/// </summary>
			// Token: 0x04005A0A RID: 23050
			public static readonly StringName _playerNameLabel = "_playerNameLabel";

			/// <summary>
			/// Cached name for the '_cardsHeader' field.
			/// </summary>
			// Token: 0x04005A0B RID: 23051
			public static readonly StringName _cardsHeader = "_cardsHeader";

			/// <summary>
			/// Cached name for the '_cardContainer' field.
			/// </summary>
			// Token: 0x04005A0C RID: 23052
			public static readonly StringName _cardContainer = "_cardContainer";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04005A0D RID: 23053
			public static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_potionsHeader' field.
			/// </summary>
			// Token: 0x04005A0E RID: 23054
			public static readonly StringName _potionsHeader = "_potionsHeader";

			/// <summary>
			/// Cached name for the '_potionContainer' field.
			/// </summary>
			// Token: 0x04005A0F RID: 23055
			public static readonly StringName _potionContainer = "_potionContainer";

			/// <summary>
			/// Cached name for the '_relicsHeader' field.
			/// </summary>
			// Token: 0x04005A10 RID: 23056
			public static readonly StringName _relicsHeader = "_relicsHeader";

			/// <summary>
			/// Cached name for the '_relicContainer' field.
			/// </summary>
			// Token: 0x04005A11 RID: 23057
			public static readonly StringName _relicContainer = "_relicContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200171F RID: 5919
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
