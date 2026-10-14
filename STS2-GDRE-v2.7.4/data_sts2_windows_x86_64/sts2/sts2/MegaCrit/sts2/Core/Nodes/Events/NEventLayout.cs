using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Events
{
	// Token: 0x020003BD RID: 957
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Events/NEventLayout.cs")]
	public class NEventLayout : Control
	{
		/// <summary>
		/// Container for event VFX nodes (ambient particles, etc.).
		/// Can be null in some subclasses.
		/// </summary>
		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06003996 RID: 14742 RVA: 0x001B47E3 File Offset: 0x001B29E3
		// (set) Token: 0x06003997 RID: 14743 RVA: 0x001B47EB File Offset: 0x001B29EB
		[Nullable(2)]
		public Control VfxContainer
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06003998 RID: 14744 RVA: 0x001B47F4 File Offset: 0x001B29F4
		public IEnumerable<NEventOptionButton> OptionButtons
		{
			get
			{
				return this._optionsContainer.GetChildren(false).OfType<NEventOptionButton>();
			}
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x001B4808 File Offset: 0x001B2A08
		public override void _Ready()
		{
			this._portrait = base.GetNodeOrNull<TextureRect>("%Portrait");
			this._title = base.GetNodeOrNull<MegaLabel>("%Title");
			this._description = base.GetNodeOrNull<MegaRichTextLabel>("%EventDescription");
			this.VfxContainer = base.GetNodeOrNull<Control>("%VfxContainer");
			this._sharedEventLabel = base.GetNodeOrNull<MegaLabel>("%SharedEventLabel");
			MegaLabel sharedEventLabel = this._sharedEventLabel;
			if (sharedEventLabel != null)
			{
				sharedEventLabel.SetTextAutoSize(NEventLayout._sharedEventLoc.GetFormattedText());
			}
			this._optionsContainer = base.GetNode<VBoxContainer>("%OptionsContainer");
			MegaRichTextLabel description = this._description;
			if (description != null)
			{
				description.SetText(string.Empty);
			}
			this.ApplyDebugUiVisibility();
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x001B48D0 File Offset: 0x001B2AD0
		public override void _EnterTree()
		{
			RunManager.Instance.EventSynchronizer.PlayerVoteChanged += this.OnPlayerVoteChanged;
			NGame instance = NGame.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Connect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)), 0U);
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x001B4920 File Offset: 0x001B2B20
		public override void _ExitTree()
		{
			RunManager.Instance.EventSynchronizer.PlayerVoteChanged -= this.OnPlayerVoteChanged;
			NGame instance = NGame.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Disconnect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)));
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x001B496D File Offset: 0x001B2B6D
		public virtual void SetEvent(EventModel eventModel)
		{
			this._event = eventModel;
			this.InitializeVisuals();
			this._event.OnRoomEnter();
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x001B4988 File Offset: 0x001B2B88
		protected virtual void InitializeVisuals()
		{
			if (this._event.HasPhobiaModePortrait)
			{
				this.SetPortrait(this._event.CreateInitialPortrait(), this._event.CreateInitialPhobiaModePortrait());
			}
			else
			{
				this.SetPortrait(this._event.CreateInitialPortrait(), null);
			}
			if (!this._event.HasVfx)
			{
				return;
			}
			Node2D node2D = this._event.CreateVfx();
			NEventRoom.Instance.Layout.AddVfxAnchoredToPortrait(node2D);
			node2D.Position = EventModel.VfxOffset;
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x001B4A08 File Offset: 0x001B2C08
		private void UpdatePhobiaMode()
		{
			if (this._currentPhobiaPortraitTex == null)
			{
				return;
			}
			if (this._portrait == null)
			{
				throw new InvalidOperationException("Trying to set a portrait in an event layout that doesn't have one.");
			}
			if (SaveManager.Instance.PrefsSave.PhobiaMode)
			{
				this._portrait.Texture = this._currentPhobiaPortraitTex;
				return;
			}
			this._portrait.Texture = this._currentPortraitTex;
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x001B4A68 File Offset: 0x001B2C68
		public void SetPortrait(Texture2D portrait, [Nullable(2)] Texture2D phobiaModePortrait = null)
		{
			if (this._portrait == null)
			{
				throw new InvalidOperationException("Trying to set a portrait in an event layout that doesn't have one.");
			}
			this._currentPortraitTex = portrait;
			this._currentPhobiaPortraitTex = phobiaModePortrait;
			this._portrait.Texture = ((SaveManager.Instance.PrefsSave.PhobiaMode && this._currentPhobiaPortraitTex != null) ? this._currentPhobiaPortraitTex : this._currentPortraitTex);
		}

		/// <summary>
		/// Adds a child node that's anchored to the portrait. Usually you'd want to add it to VfxContainer, but you can use
		/// this method instead if your VFX's position is dependent on the portrait (usually due to resolution stuff)
		/// </summary>
		/// <param name="vfx">VFX to add</param>
		// Token: 0x060039A0 RID: 14752 RVA: 0x001B4AC8 File Offset: 0x001B2CC8
		[NullableContext(2)]
		public void AddVfxAnchoredToPortrait(Node vfx)
		{
			this._portrait.AddChildSafely(vfx);
		}

		/// <summary>
		/// Removes ALL child nodes from the Portrait, vfx or not.
		/// Useful for swapping out vfx if an event has multiple portraits.
		/// </summary>
		// Token: 0x060039A1 RID: 14753 RVA: 0x001B4AD8 File Offset: 0x001B2CD8
		public void RemoveNodesOnPortrait()
		{
			foreach (Node node in this._portrait.GetChildren(false))
			{
				this._portrait.RemoveChildSafely(node);
			}
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x001B4B30 File Offset: 0x001B2D30
		public void SetTitle(string title)
		{
			if (this._title == null)
			{
				return;
			}
			this._title.Text = title;
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x001B4B47 File Offset: 0x001B2D47
		public void SetDescription(string description)
		{
			if (this._description == null)
			{
				return;
			}
			this._description.SetTextAutoSize(description);
			this.AnimateIn();
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x001B4B64 File Offset: 0x001B2D64
		protected virtual void AnimateIn()
		{
			if (this._sharedEventLabel != null)
			{
				this._sharedEventLabel.Modulate = StsColors.transparentWhite;
			}
			if (this._description != null)
			{
				this._description.Modulate = StsColors.transparentWhite;
				bool flag = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast;
				Tween descriptionTween = this._descriptionTween;
				if (descriptionTween != null)
				{
					descriptionTween.Kill();
				}
				this._descriptionTween = base.CreateTween().SetParallel(true);
				this._descriptionTween.TweenInterval(flag ? 0.2 : 0.5);
				this._descriptionTween.Chain();
				if (this._title != null)
				{
					this._descriptionTween.TweenProperty(this._title, "modulate", Colors.White, flag ? 0.25 : 0.5);
				}
				this._descriptionTween.TweenProperty(this._description, "modulate", Colors.White, flag ? 0.5 : 1.0).SetDelay(0.25);
				this._descriptionTween.TweenProperty(this._description, "visible_ratio", 1f, flag ? 0.5 : 1.0).SetDelay(0.25).From(0f)
					.SetEase(1L)
					.SetTrans(1L);
				if (this._sharedEventLabel != null)
				{
					this._descriptionTween.TweenProperty(this._sharedEventLabel, "modulate", Colors.White, flag ? 0.25 : 0.5).SetDelay(0.25);
				}
			}
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x001B4D54 File Offset: 0x001B2F54
		public void ClearOptions()
		{
			foreach (Node node in this._optionsContainer.GetChildren(false).ToList<Node>())
			{
				this._optionsContainer.RemoveChildSafely(node);
				node.QueueFreeSafely();
			}
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x001B4DC0 File Offset: 0x001B2FC0
		public void AddOptions(IEnumerable<EventOption> options)
		{
			if (this._sharedEventLabel != null)
			{
				CanvasItem sharedEventLabel = this._sharedEventLabel;
				EventModel @event = this._event;
				sharedEventLabel.Visible = @event != null && @event.IsShared && !@event.IsFinished && this._event.Owner.RunState.Players.Count > 1;
			}
			foreach (EventOption eventOption in options)
			{
				NEventOptionButton neventOptionButton = NEventOptionButton.Create(this._event, eventOption, this._optionsContainer.GetChildCount(false));
				this._optionsContainer.AddChildSafely(neventOptionButton);
				neventOptionButton.RefreshVotes();
			}
			int childCount = this._optionsContainer.GetChildCount(false);
			if (childCount == 0)
			{
				return;
			}
			NodePath path = this._optionsContainer.GetChild<Control>(0, false).GetPath();
			NodePath path2 = this._optionsContainer.GetChild<Control>(childCount - 1, false).GetPath();
			for (int i = 0; i < childCount; i++)
			{
				Control child = this._optionsContainer.GetChild<Control>(i, false);
				NodePath path3 = child.GetPath();
				child.FocusNeighborLeft = path3;
				child.FocusNeighborRight = path3;
				child.FocusNeighborTop = ((i > 0) ? this._optionsContainer.GetChild<Control>(i - 1, false).GetPath() : path2);
				child.FocusNeighborBottom = ((i < childCount - 1) ? this._optionsContainer.GetChild<Control>(i + 1, false).GetPath() : path);
			}
			this.AnimateButtonsIn();
		}

		/// <summary>
		/// Called when this layout is finished being set up.
		/// At this point, all the buttons for the initial page should be populated.
		/// </summary>
		// Token: 0x060039A7 RID: 14759 RVA: 0x001B4F48 File Offset: 0x001B3148
		public virtual void OnSetupComplete()
		{
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x001B4F4C File Offset: 0x001B314C
		protected virtual void AnimateButtonsIn()
		{
			using (IEnumerator<NEventOptionButton> enumerator = this.OptionButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NEventOptionButton button = enumerator.Current;
					Callable.From(delegate
					{
						button.AnimateIn();
					}).CallDeferred(Array.Empty<Variant>());
				}
			}
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x001B4FBC File Offset: 0x001B31BC
		public async Task BeforeSharedOptionChosen(EventOption option)
		{
			NEventOptionButton chosenButton = null;
			foreach (NEventOptionButton neventOptionButton in this.OptionButtons)
			{
				neventOptionButton.Disable();
				if (neventOptionButton.Option == option)
				{
					chosenButton = neventOptionButton;
				}
			}
			if (chosenButton != null)
			{
				EventSplitVoteAnimation eventSplitVoteAnimation = new EventSplitVoteAnimation(this, this._event.Owner.RunState);
				await eventSplitVoteAnimation.TryPlay(chosenButton);
				foreach (NEventOptionButton neventOptionButton2 in this.OptionButtons)
				{
					if (neventOptionButton2.Option != option)
					{
						neventOptionButton2.GrayOut();
					}
				}
				await chosenButton.FlashConfirmation();
			}
		}

		/// <summary>
		/// Called during a shared event when a player changes the option they voted on.
		/// </summary>
		// Token: 0x060039AA RID: 14762 RVA: 0x001B5008 File Offset: 0x001B3208
		private void OnPlayerVoteChanged(Player player)
		{
			foreach (NEventOptionButton neventOptionButton in this.OptionButtons)
			{
				neventOptionButton.RefreshVotes();
			}
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x001B5054 File Offset: 0x001B3254
		public void DisableEventOptions()
		{
			foreach (NEventOptionButton neventOptionButton in this.OptionButtons)
			{
				neventOptionButton.Disable();
			}
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x001B50A0 File Offset: 0x001B32A0
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionReleased(DebugHotkey.hideEventUi, false))
			{
				NEventLayout._isDebugUiVisible = !NEventLayout._isDebugUiVisible;
				this.ApplyDebugUiVisibility();
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NEventLayout._isDebugUiVisible ? "Hide Event UI" : "Show Event UI"));
			}
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x001B50F0 File Offset: 0x001B32F0
		private void ApplyDebugUiVisibility()
		{
			if (NEventLayout._isDebugUiVisible)
			{
				this._optionsContainer.Visible = false;
				if (this._title != null)
				{
					this._title.Modulate = Colors.Transparent;
				}
				if (this._description != null)
				{
					this._description.Visible = false;
					return;
				}
			}
			else
			{
				this._optionsContainer.Visible = true;
				if (this._description != null)
				{
					this._description.Visible = true;
				}
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x060039AE RID: 14766 RVA: 0x001B515D File Offset: 0x001B335D
		[Nullable(2)]
		public virtual Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this.OptionButtons.FirstOrDefault<NEventOptionButton>();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060039AF RID: 14767 RVA: 0x001B516C File Offset: 0x001B336C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(17)
			{
				new MethodInfo(NEventLayout.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventLayout.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventLayout.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventLayout.MethodName.InitializeVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventLayout.MethodName.UpdatePhobiaMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventLayout.MethodName.SetPortrait, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "portrait", 0L, "", 6L, new StringName("Texture2D"), false),
					new PropertyInfo(24L, "phobiaModePortrait", 0L, "", 6L, new StringName("Texture2D"), false)
				}, null),
				new MethodInfo(NEventLayout.MethodName.AddVfxAnchoredToPortrait, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "vfx", 0L, "", 6L, new StringName("Node"), false)
				}, null),
				new MethodInfo(NEventLayout.MethodName.RemoveNodesOnPortrait, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventLayout.MethodName.SetTitle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "title", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEventLayout.MethodName.SetDescription, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "description", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEventLayout.MethodName.AnimateIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventLayout.MethodName.ClearOptions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventLayout.MethodName.OnSetupComplete, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventLayout.MethodName.AnimateButtonsIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventLayout.MethodName.DisableEventOptions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventLayout.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NEventLayout.MethodName.ApplyDebugUiVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060039B0 RID: 14768 RVA: 0x001B55A8 File Offset: 0x001B37A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEventLayout.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.InitializeVisuals && args.Count == 0)
			{
				this.InitializeVisuals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.UpdatePhobiaMode && args.Count == 0)
			{
				this.UpdatePhobiaMode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.SetPortrait && args.Count == 2)
			{
				this.SetPortrait(VariantUtils.ConvertTo<Texture2D>(args[0]), VariantUtils.ConvertTo<Texture2D>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.AddVfxAnchoredToPortrait && args.Count == 1)
			{
				this.AddVfxAnchoredToPortrait(VariantUtils.ConvertTo<Node>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.RemoveNodesOnPortrait && args.Count == 0)
			{
				this.RemoveNodesOnPortrait();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.SetTitle && args.Count == 1)
			{
				this.SetTitle(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.SetDescription && args.Count == 1)
			{
				this.SetDescription(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.AnimateIn && args.Count == 0)
			{
				this.AnimateIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.ClearOptions && args.Count == 0)
			{
				this.ClearOptions();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.OnSetupComplete && args.Count == 0)
			{
				this.OnSetupComplete();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.AnimateButtonsIn && args.Count == 0)
			{
				this.AnimateButtonsIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.DisableEventOptions && args.Count == 0)
			{
				this.DisableEventOptions();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventLayout.MethodName.ApplyDebugUiVisibility && args.Count == 0)
			{
				this.ApplyDebugUiVisibility();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060039B1 RID: 14769 RVA: 0x001B5888 File Offset: 0x001B3A88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEventLayout.MethodName._Ready || (ref method) == NEventLayout.MethodName._EnterTree || (ref method) == NEventLayout.MethodName._ExitTree || (ref method) == NEventLayout.MethodName.InitializeVisuals || (ref method) == NEventLayout.MethodName.UpdatePhobiaMode || (ref method) == NEventLayout.MethodName.SetPortrait || (ref method) == NEventLayout.MethodName.AddVfxAnchoredToPortrait || (ref method) == NEventLayout.MethodName.RemoveNodesOnPortrait || (ref method) == NEventLayout.MethodName.SetTitle || (ref method) == NEventLayout.MethodName.SetDescription || (ref method) == NEventLayout.MethodName.AnimateIn || (ref method) == NEventLayout.MethodName.ClearOptions || (ref method) == NEventLayout.MethodName.OnSetupComplete || (ref method) == NEventLayout.MethodName.AnimateButtonsIn || (ref method) == NEventLayout.MethodName.DisableEventOptions || (ref method) == NEventLayout.MethodName._Input || (ref method) == NEventLayout.MethodName.ApplyDebugUiVisibility || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060039B2 RID: 14770 RVA: 0x001B599C File Offset: 0x001B3B9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEventLayout.PropertyName.VfxContainer)
			{
				this.VfxContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._descriptionTween)
			{
				this._descriptionTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._optionsContainer)
			{
				this._optionsContainer = VariantUtils.ConvertTo<VBoxContainer>(ref value);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._portrait)
			{
				this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._currentPortraitTex)
			{
				this._currentPortraitTex = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._currentPhobiaPortraitTex)
			{
				this._currentPhobiaPortraitTex = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._title)
			{
				this._title = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._sharedEventLabel)
			{
				this._sharedEventLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060039B3 RID: 14771 RVA: 0x001B5AA4 File Offset: 0x001B3CA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEventLayout.PropertyName.VfxContainer)
			{
				Control control = this.VfxContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._descriptionTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._descriptionTween);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._optionsContainer)
			{
				value = VariantUtils.CreateFrom<VBoxContainer>(ref this._optionsContainer);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._portrait)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._currentPortraitTex)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._currentPortraitTex);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._currentPhobiaPortraitTex)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._currentPhobiaPortraitTex);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._title)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._title);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._sharedEventLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._sharedEventLabel);
				return true;
			}
			if ((ref name) == NEventLayout.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060039B4 RID: 14772 RVA: 0x001B5C00 File Offset: 0x001B3E00
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEventLayout.PropertyName.VfxContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventLayout.PropertyName._descriptionTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventLayout.PropertyName._optionsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventLayout.PropertyName._portrait, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventLayout.PropertyName._currentPortraitTex, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventLayout.PropertyName._currentPhobiaPortraitTex, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventLayout.PropertyName._title, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventLayout.PropertyName._sharedEventLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventLayout.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventLayout.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060039B5 RID: 14773 RVA: 0x001B5D60 File Offset: 0x001B3F60
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName vfxContainer = NEventLayout.PropertyName.VfxContainer;
			Control vfxContainer2 = this.VfxContainer;
			info.AddProperty(vfxContainer, Variant.From<Control>(ref vfxContainer2));
			info.AddProperty(NEventLayout.PropertyName._descriptionTween, Variant.From<Tween>(ref this._descriptionTween));
			info.AddProperty(NEventLayout.PropertyName._optionsContainer, Variant.From<VBoxContainer>(ref this._optionsContainer));
			info.AddProperty(NEventLayout.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
			info.AddProperty(NEventLayout.PropertyName._currentPortraitTex, Variant.From<Texture2D>(ref this._currentPortraitTex));
			info.AddProperty(NEventLayout.PropertyName._currentPhobiaPortraitTex, Variant.From<Texture2D>(ref this._currentPhobiaPortraitTex));
			info.AddProperty(NEventLayout.PropertyName._title, Variant.From<MegaLabel>(ref this._title));
			info.AddProperty(NEventLayout.PropertyName._sharedEventLabel, Variant.From<MegaLabel>(ref this._sharedEventLabel));
			info.AddProperty(NEventLayout.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
		}

		/// <inheritdoc />
		// Token: 0x060039B6 RID: 14774 RVA: 0x001B5E40 File Offset: 0x001B4040
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEventLayout.PropertyName.VfxContainer, ref variant))
			{
				this.VfxContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEventLayout.PropertyName._descriptionTween, ref variant2))
			{
				this._descriptionTween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEventLayout.PropertyName._optionsContainer, ref variant3))
			{
				this._optionsContainer = variant3.As<VBoxContainer>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEventLayout.PropertyName._portrait, ref variant4))
			{
				this._portrait = variant4.As<TextureRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NEventLayout.PropertyName._currentPortraitTex, ref variant5))
			{
				this._currentPortraitTex = variant5.As<Texture2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NEventLayout.PropertyName._currentPhobiaPortraitTex, ref variant6))
			{
				this._currentPhobiaPortraitTex = variant6.As<Texture2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NEventLayout.PropertyName._title, ref variant7))
			{
				this._title = variant7.As<MegaLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NEventLayout.PropertyName._sharedEventLabel, ref variant8))
			{
				this._sharedEventLabel = variant8.As<MegaLabel>();
			}
			Variant variant9;
			if (info.TryGetProperty(NEventLayout.PropertyName._description, ref variant9))
			{
				this._description = variant9.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x04001666 RID: 5734
		public const string defaultScenePath = "res://scenes/events/default_event_layout.tscn";

		// Token: 0x04001668 RID: 5736
		[Nullable(2)]
		protected Tween _descriptionTween;

		// Token: 0x04001669 RID: 5737
		protected VBoxContainer _optionsContainer;

		// Token: 0x0400166A RID: 5738
		[Nullable(2)]
		private TextureRect _portrait;

		// Token: 0x0400166B RID: 5739
		[Nullable(2)]
		private Texture2D _currentPortraitTex;

		// Token: 0x0400166C RID: 5740
		[Nullable(2)]
		private Texture2D _currentPhobiaPortraitTex;

		// Token: 0x0400166D RID: 5741
		[Nullable(2)]
		private MegaLabel _title;

		// Token: 0x0400166E RID: 5742
		protected EventModel _event;

		/// <summary>
		/// The label to show when an event is "shared" (see <see cref="P:MegaCrit.Sts2.Core.Models.EventModel.IsShared" /> for details).
		/// Can be null for event types that can't be shared (like <see cref="T:MegaCrit.Sts2.Core.Models.AncientEventModel" />).
		/// </summary>
		// Token: 0x0400166F RID: 5743
		[Nullable(2)]
		protected MegaLabel _sharedEventLabel;

		// Token: 0x04001670 RID: 5744
		private static readonly LocString _sharedEventLoc = new LocString("events", "SHARED_EVENT_INFO");

		/// <summary>
		/// The event description label. Some events (like Ancients) have no descriptions, in which case this will be null.
		/// </summary>
		// Token: 0x04001671 RID: 5745
		[Nullable(2)]
		protected MegaRichTextLabel _description;

		// Token: 0x04001672 RID: 5746
		private static bool _isDebugUiVisible;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017D1 RID: 6097
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005CD2 RID: 23762
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005CD3 RID: 23763
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005CD4 RID: 23764
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'InitializeVisuals' method.
			/// </summary>
			// Token: 0x04005CD5 RID: 23765
			public static readonly StringName InitializeVisuals = "InitializeVisuals";

			/// <summary>
			/// Cached name for the 'UpdatePhobiaMode' method.
			/// </summary>
			// Token: 0x04005CD6 RID: 23766
			public static readonly StringName UpdatePhobiaMode = "UpdatePhobiaMode";

			/// <summary>
			/// Cached name for the 'SetPortrait' method.
			/// </summary>
			// Token: 0x04005CD7 RID: 23767
			public static readonly StringName SetPortrait = "SetPortrait";

			/// <summary>
			/// Cached name for the 'AddVfxAnchoredToPortrait' method.
			/// </summary>
			// Token: 0x04005CD8 RID: 23768
			public static readonly StringName AddVfxAnchoredToPortrait = "AddVfxAnchoredToPortrait";

			/// <summary>
			/// Cached name for the 'RemoveNodesOnPortrait' method.
			/// </summary>
			// Token: 0x04005CD9 RID: 23769
			public static readonly StringName RemoveNodesOnPortrait = "RemoveNodesOnPortrait";

			/// <summary>
			/// Cached name for the 'SetTitle' method.
			/// </summary>
			// Token: 0x04005CDA RID: 23770
			public static readonly StringName SetTitle = "SetTitle";

			/// <summary>
			/// Cached name for the 'SetDescription' method.
			/// </summary>
			// Token: 0x04005CDB RID: 23771
			public static readonly StringName SetDescription = "SetDescription";

			/// <summary>
			/// Cached name for the 'AnimateIn' method.
			/// </summary>
			// Token: 0x04005CDC RID: 23772
			public static readonly StringName AnimateIn = "AnimateIn";

			/// <summary>
			/// Cached name for the 'ClearOptions' method.
			/// </summary>
			// Token: 0x04005CDD RID: 23773
			public static readonly StringName ClearOptions = "ClearOptions";

			/// <summary>
			/// Cached name for the 'OnSetupComplete' method.
			/// </summary>
			// Token: 0x04005CDE RID: 23774
			public static readonly StringName OnSetupComplete = "OnSetupComplete";

			/// <summary>
			/// Cached name for the 'AnimateButtonsIn' method.
			/// </summary>
			// Token: 0x04005CDF RID: 23775
			public static readonly StringName AnimateButtonsIn = "AnimateButtonsIn";

			/// <summary>
			/// Cached name for the 'DisableEventOptions' method.
			/// </summary>
			// Token: 0x04005CE0 RID: 23776
			public static readonly StringName DisableEventOptions = "DisableEventOptions";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005CE1 RID: 23777
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'ApplyDebugUiVisibility' method.
			/// </summary>
			// Token: 0x04005CE2 RID: 23778
			public static readonly StringName ApplyDebugUiVisibility = "ApplyDebugUiVisibility";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017D2 RID: 6098
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'VfxContainer' property.
			/// </summary>
			// Token: 0x04005CE3 RID: 23779
			public static readonly StringName VfxContainer = "VfxContainer";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005CE4 RID: 23780
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_descriptionTween' field.
			/// </summary>
			// Token: 0x04005CE5 RID: 23781
			public static readonly StringName _descriptionTween = "_descriptionTween";

			/// <summary>
			/// Cached name for the '_optionsContainer' field.
			/// </summary>
			// Token: 0x04005CE6 RID: 23782
			public static readonly StringName _optionsContainer = "_optionsContainer";

			/// <summary>
			/// Cached name for the '_portrait' field.
			/// </summary>
			// Token: 0x04005CE7 RID: 23783
			public static readonly StringName _portrait = "_portrait";

			/// <summary>
			/// Cached name for the '_currentPortraitTex' field.
			/// </summary>
			// Token: 0x04005CE8 RID: 23784
			public static readonly StringName _currentPortraitTex = "_currentPortraitTex";

			/// <summary>
			/// Cached name for the '_currentPhobiaPortraitTex' field.
			/// </summary>
			// Token: 0x04005CE9 RID: 23785
			public static readonly StringName _currentPhobiaPortraitTex = "_currentPhobiaPortraitTex";

			/// <summary>
			/// Cached name for the '_title' field.
			/// </summary>
			// Token: 0x04005CEA RID: 23786
			public static readonly StringName _title = "_title";

			/// <summary>
			/// Cached name for the '_sharedEventLabel' field.
			/// </summary>
			// Token: 0x04005CEB RID: 23787
			public static readonly StringName _sharedEventLabel = "_sharedEventLabel";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005CEC RID: 23788
			public static readonly StringName _description = "_description";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017D3 RID: 6099
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
