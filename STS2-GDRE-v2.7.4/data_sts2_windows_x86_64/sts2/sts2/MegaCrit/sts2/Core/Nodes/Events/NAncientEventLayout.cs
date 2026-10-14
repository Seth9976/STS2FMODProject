using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Events
{
	// Token: 0x020003BA RID: 954
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Events/NAncientEventLayout.cs")]
	public class NAncientEventLayout : NEventLayout
	{
		// Token: 0x06003951 RID: 14673 RVA: 0x001B1A8C File Offset: 0x001AFC8C
		public override void _Ready()
		{
			base._Ready();
			this._ancientBgContainer = base.GetNode<NAncientBgContainer>("%AncientBgContainer");
			this._contentContainer = base.GetNode<Control>("%ContentContainer");
			this._content = base.GetNode<VBoxContainer>("%Content");
			this._dialogueContainer = base.GetNode<VBoxContainer>("%DialogueContainer");
			this._dialogueHitbox = base.GetNode<NAncientDialogueHitbox>("%DialogueHitbox");
			this._dialogueHitbox.Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnDialogueHitboxClicked)), 0U);
			this._dialogueHitbox.Visible = false;
			this._dialogueHitbox.Disable();
			this._fakeNextButtonContainer = base.GetNode<Control>("%FakeNextButtonContainer");
			this._fakeNextButton = this._fakeNextButtonContainer.GetNode<Control>("FakeNextButton");
			this._fakeNextButtonLabel = this._fakeNextButton.GetNode<MegaLabel>("Label");
			this._fakeNextButtonControllerIcon = this._fakeNextButton.GetNode<TextureRect>("ControllerIcon");
			this._originalContentContainerHeight = this._contentContainer.Size.Y;
			this._contentContainer.Size = new Vector2(this._contentContainer.Size.X, this._fakeNextButtonContainer.GlobalPosition.Y - this._contentContainer.GlobalPosition.Y);
			this.UpdateHotkeyDisplay();
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateHotkeyDisplay)), 0U);
		}

		// Token: 0x06003952 RID: 14674 RVA: 0x001B1C71 File Offset: 0x001AFE71
		public override void _EnterTree()
		{
			base._EnterTree();
			this._cts = new CancellationTokenSource();
			ActiveScreenContext.Instance.Updated += this.UpdateBannerVisibility;
		}

		// Token: 0x06003953 RID: 14675 RVA: 0x001B1C9C File Offset: 0x001AFE9C
		public override void _ExitTree()
		{
			base._ExitTree();
			this._cts.Cancel();
			if (this._ancientEvent.HasAmbientBgm)
			{
				SfxCmd.StopLoop(this._ancientEvent.AmbientBgm);
			}
			ActiveScreenContext.Instance.Updated -= this.UpdateBannerVisibility;
			NControllerManager.Instance.Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)));
			NControllerManager.Instance.Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateHotkeyDisplay)));
			NInputManager.Instance.Disconnect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateHotkeyDisplay)));
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x001B1D50 File Offset: 0x001AFF50
		protected override void InitializeVisuals()
		{
			this._ancientEvent = (AncientEventModel)this._event;
			this._ancientNameBanner = NAncientNameBanner.Create(this._ancientEvent);
			this.AddChildSafely(this._ancientNameBanner);
			this.UpdateBannerVisibility();
			AncientEventModel ancientEvent = this._ancientEvent;
			if (ancientEvent != null && ancientEvent.Owner != null && ancientEvent.HealedAmount > 0)
			{
				TaskHelper.RunSafely(this.PlayHealVfxAfterFadeIn(this._ancientEvent.Owner, this._ancientEvent.HealedAmount));
			}
			foreach (Node node in this._ancientBgContainer.GetChildren(false))
			{
				this._ancientBgContainer.RemoveChildSafely(node);
			}
			this._ancientBgContainer.AddChildSafely(this._ancientEvent.CreateBackgroundScene().Instantiate<Control>(0L));
			if (this._ancientEvent.HasAmbientBgm)
			{
				SfxCmd.PlayLoop(this._ancientEvent.AmbientBgm, true);
			}
		}

		// Token: 0x06003955 RID: 14677 RVA: 0x001B1E5C File Offset: 0x001B005C
		protected override void AnimateIn()
		{
			if (this._description == null)
			{
				return;
			}
			this._description.Modulate = Colors.Transparent;
			Tween descriptionTween = this._descriptionTween;
			if (descriptionTween != null)
			{
				descriptionTween.Kill();
			}
			this._descriptionTween = base.CreateTween().SetParallel(true);
			if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast)
			{
				this._descriptionTween.TweenInterval(0.2);
			}
			else
			{
				this._descriptionTween.TweenInterval(0.5);
			}
			this._descriptionTween.Chain();
			this._descriptionTween.TweenProperty(this._description, "modulate", Colors.White, 1.0);
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x001B1F20 File Offset: 0x001B0120
		public void SetDialogue(IReadOnlyList<AncientDialogueLine> lines)
		{
			this._dialogue.Clear();
			this._dialogue.AddRange(lines);
			this._currentDialogueLine = 0;
			foreach (AncientDialogueLine ancientDialogueLine in lines)
			{
				NAncientDialogueLine nancientDialogueLine = NAncientDialogueLine.Create(ancientDialogueLine, this._ancientEvent, this._ancientEvent.Owner.Character);
				nancientDialogueLine.Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnDialogueLineFocused)), 0U);
				nancientDialogueLine.Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnDialogueLineUnfocused)), 0U);
				this._dialogueContainer.AddChildSafely(nancientDialogueLine);
			}
		}

		/// <summary>
		/// Makes the stale dialogue lines all visible at once.
		/// </summary>
		// Token: 0x06003957 RID: 14679 RVA: 0x001B1FE4 File Offset: 0x001B01E4
		private void OnDialogueLineFocused(NClickableControl dialogueLine)
		{
			if (this._dialogueContainer.GetChild<NAncientDialogueLine>(this._currentDialogueLine, false) == dialogueLine)
			{
				return;
			}
			for (int i = 0; i < this._currentDialogueLine; i++)
			{
				NAncientDialogueLine child = this._dialogueContainer.GetChild<NAncientDialogueLine>(i, false);
				child.FadeInStaleDialogue();
			}
		}

		/// <summary>
		/// Returns the stale dialogues to their original opacity.
		/// </summary>
		// Token: 0x06003958 RID: 14680 RVA: 0x001B202C File Offset: 0x001B022C
		private void OnDialogueLineUnfocused(NClickableControl dialogueLine)
		{
			if (this._dialogueContainer.GetChild<NAncientDialogueLine>(this._currentDialogueLine, false).Equals(dialogueLine))
			{
				return;
			}
			for (int i = 0; i < this._currentDialogueLine; i++)
			{
				NAncientDialogueLine child = this._dialogueContainer.GetChild<NAncientDialogueLine>(i, false);
				child.FadeOutStaleDialogue();
			}
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x001B207C File Offset: 0x001B027C
		public void ClearDialogue()
		{
			this._dialogue.Clear();
			this._currentDialogueLine = 0;
			foreach (Node node in this._dialogueContainer.GetChildren(false).ToList<Node>())
			{
				this._dialogueContainer.RemoveChildSafely(node);
				node.QueueFreeSafely();
			}
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x001B20F8 File Offset: 0x001B02F8
		public override void OnSetupComplete()
		{
			this._dialogueContainer.ResetSize();
			this._optionsContainer.ResetSize();
			this._content.ResetSize();
			this._content.Position = new Vector2(this._content.Position.X, this._contentContainer.Size.Y);
			this.SetDialogueLineAndAnimate(0);
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x001B2160 File Offset: 0x001B0360
		protected override void AnimateButtonsIn()
		{
			foreach (NEventOptionButton neventOptionButton in base.OptionButtons)
			{
				neventOptionButton.Modulate = Colors.White;
				neventOptionButton.EnableButton();
			}
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x001B21B8 File Offset: 0x001B03B8
		private async Task PlayHealVfxAfterFadeIn(Player player, decimal healAmount)
		{
			await Cmd.Wait(0.2f, this._cts.Token, false);
			PlayerFullscreenHealVfx.Play(player, healAmount, base.VfxContainer);
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x001B220B File Offset: 0x001B040B
		private void OnDialogueHitboxClicked(NClickableControl _)
		{
			this.SetDialogueLineAndAnimate(this._currentDialogueLine + 1);
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x001B221C File Offset: 0x001B041C
		private void SetDialogueLineAndAnimate(int lineIndex)
		{
			this._currentDialogueLine = lineIndex;
			if (this._contentTween != null)
			{
				this._contentTween.Pause();
				this._contentTween.CustomStep(1.0);
				this._contentTween.Kill();
				this._contentTween = null;
			}
			this.UpdateFakeNextButton();
			NAncientDialogueLine childOrNull = this._dialogueContainer.GetChildOrNull<NAncientDialogueLine>(this._currentDialogueLine, false);
			float num = 0f;
			if (childOrNull != null)
			{
				childOrNull.OnAnimInSetVisible();
				childOrNull.PlaySfx();
				num = childOrNull.Position.Y + childOrNull.Size.Y;
			}
			if (this.IsDialogueOnLastLine)
			{
				this._fakeNextButtonContainer.Visible = false;
				this._contentContainer.Size = new Vector2(this._contentContainer.Size.X, this._originalContentContainerHeight);
				this._dialogueHitbox.Visible = false;
				this._dialogueHitbox.Disable();
				foreach (NEventOptionButton neventOptionButton in base.OptionButtons)
				{
					neventOptionButton.EnableButton();
				}
				num += this._optionsContainer.Size.Y + 10f;
			}
			else
			{
				this._fakeNextButtonContainer.Visible = true;
				this._dialogueHitbox.Visible = true;
				this._dialogueHitbox.Enable();
			}
			if (this._dialogueContainer.GetChildCount(false) > this._currentDialogueLine)
			{
				this._dialogueContainer.GetChild<NAncientDialogueLine>(this._currentDialogueLine, false).SetSpeakerIconVisible();
			}
			foreach (NEventOptionButton neventOptionButton2 in base.OptionButtons)
			{
				neventOptionButton2.FocusMode = 0L;
			}
			this._contentTween = base.CreateTween();
			this._contentTween.TweenProperty(this._content, "position", new Vector2(this._content.Position.X, this._contentContainer.Size.Y - num), 1.0).SetEase(1L).SetTrans(5L);
			if (this.IsDialogueOnLastLine)
			{
				this._contentTween.Parallel().TweenCallback(Callable.From(delegate
				{
					foreach (NEventOptionButton neventOptionButton3 in base.OptionButtons)
					{
						neventOptionButton3.FocusMode = 2L;
					}
					Control defaultFocusedControl = this.DefaultFocusedControl;
					if (defaultFocusedControl == null)
					{
						return;
					}
					defaultFocusedControl.TryGrabFocus();
				})).SetDelay(0.8);
			}
			for (int i = 0; i < this._currentDialogueLine; i++)
			{
				NAncientDialogueLine child = this._dialogueContainer.GetChild<NAncientDialogueLine>(i, false);
				child.SetTransparency((i != this._currentDialogueLine) ? 0.25f : 1f);
			}
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x001B24D8 File Offset: 0x001B06D8
		private void UpdateFakeNextButton()
		{
			LocString locString = ((this._dialogue.Count > this._currentDialogueLine) ? this._dialogue[this._currentDialogueLine].NextButtonText : null);
			if (locString != null)
			{
				this._fakeNextButtonLabel.SetTextAutoSize(locString.GetFormattedText() ?? "");
				return;
			}
			this._fakeNextButtonLabel.Visible = false;
		}

		/// <summary>
		/// Is the player on the last line of dialogue?
		/// </summary>
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06003960 RID: 14688 RVA: 0x001B253C File Offset: 0x001B073C
		private bool IsDialogueOnLastLine
		{
			get
			{
				return this._currentDialogueLine >= this._dialogue.Count - 1;
			}
		}

		/// <summary>
		/// Used to hide the banner when you open a screen above this (like the map)
		/// </summary>
		// Token: 0x06003961 RID: 14689 RVA: 0x001B2558 File Offset: 0x001B0758
		private void HideNameBanner()
		{
			if (this._ancientNameBanner == null)
			{
				return;
			}
			Tween bannerTween = this._bannerTween;
			if (bannerTween != null)
			{
				bannerTween.Kill();
			}
			this._bannerTween = base.CreateTween();
			this._bannerTween.TweenProperty(this._ancientNameBanner, "modulate:a", 0f, 0.25);
		}

		/// <summary>
		/// Used to show the banner when you return focus to the event room
		/// </summary>
		// Token: 0x06003962 RID: 14690 RVA: 0x001B25BC File Offset: 0x001B07BC
		private void ShowNameBanner()
		{
			if (this._ancientNameBanner == null)
			{
				return;
			}
			Tween bannerTween = this._bannerTween;
			if (bannerTween != null)
			{
				bannerTween.Kill();
			}
			this._bannerTween = base.CreateTween();
			this._bannerTween.TweenProperty(this._ancientNameBanner, "modulate:a", 1f, 0.5);
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x001B261E File Offset: 0x001B081E
		private void UpdateBannerVisibility()
		{
			if (NEventRoom.Instance == null)
			{
				return;
			}
			if (ActiveScreenContext.Instance.IsCurrent(NEventRoom.Instance))
			{
				this.ShowNameBanner();
				return;
			}
			this.HideNameBanner();
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06003964 RID: 14692 RVA: 0x001B2646 File Offset: 0x001B0846
		[Nullable(2)]
		public override Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (!this.IsDialogueOnLastLine)
				{
					return null;
				}
				return base.OptionButtons.FirstOrDefault<NEventOptionButton>();
			}
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x001B2660 File Offset: 0x001B0860
		private void UpdateHotkeyDisplay()
		{
			this._fakeNextButtonControllerIcon.Visible = NControllerManager.Instance.IsUsingController;
			string hotkey = this._dialogueHitbox.GetHotkey();
			if (hotkey != null)
			{
				this._fakeNextButtonControllerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(hotkey);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003966 RID: 14694 RVA: 0x001B26A8 File Offset: 0x001B08A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(17)
			{
				new MethodInfo(NAncientEventLayout.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName.InitializeVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName.AnimateIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName.OnDialogueLineFocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "dialogueLine", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NAncientEventLayout.MethodName.OnDialogueLineUnfocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "dialogueLine", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NAncientEventLayout.MethodName.ClearDialogue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName.OnSetupComplete, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName.AnimateButtonsIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName.OnDialogueHitboxClicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NAncientEventLayout.MethodName.SetDialogueLineAndAnimate, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "lineIndex", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAncientEventLayout.MethodName.UpdateFakeNextButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName.HideNameBanner, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName.ShowNameBanner, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName.UpdateBannerVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientEventLayout.MethodName.UpdateHotkeyDisplay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003967 RID: 14695 RVA: 0x001B2A94 File Offset: 0x001B0C94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAncientEventLayout.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.InitializeVisuals && args.Count == 0)
			{
				this.InitializeVisuals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.AnimateIn && args.Count == 0)
			{
				this.AnimateIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.OnDialogueLineFocused && args.Count == 1)
			{
				this.OnDialogueLineFocused(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.OnDialogueLineUnfocused && args.Count == 1)
			{
				this.OnDialogueLineUnfocused(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.ClearDialogue && args.Count == 0)
			{
				this.ClearDialogue();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.OnSetupComplete && args.Count == 0)
			{
				this.OnSetupComplete();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.AnimateButtonsIn && args.Count == 0)
			{
				this.AnimateButtonsIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.OnDialogueHitboxClicked && args.Count == 1)
			{
				this.OnDialogueHitboxClicked(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.SetDialogueLineAndAnimate && args.Count == 1)
			{
				this.SetDialogueLineAndAnimate(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.UpdateFakeNextButton && args.Count == 0)
			{
				this.UpdateFakeNextButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.HideNameBanner && args.Count == 0)
			{
				this.HideNameBanner();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.ShowNameBanner && args.Count == 0)
			{
				this.ShowNameBanner();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.UpdateBannerVisibility && args.Count == 0)
			{
				this.UpdateBannerVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientEventLayout.MethodName.UpdateHotkeyDisplay && args.Count == 0)
			{
				this.UpdateHotkeyDisplay();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003968 RID: 14696 RVA: 0x001B2D58 File Offset: 0x001B0F58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAncientEventLayout.MethodName._Ready || (ref method) == NAncientEventLayout.MethodName._EnterTree || (ref method) == NAncientEventLayout.MethodName._ExitTree || (ref method) == NAncientEventLayout.MethodName.InitializeVisuals || (ref method) == NAncientEventLayout.MethodName.AnimateIn || (ref method) == NAncientEventLayout.MethodName.OnDialogueLineFocused || (ref method) == NAncientEventLayout.MethodName.OnDialogueLineUnfocused || (ref method) == NAncientEventLayout.MethodName.ClearDialogue || (ref method) == NAncientEventLayout.MethodName.OnSetupComplete || (ref method) == NAncientEventLayout.MethodName.AnimateButtonsIn || (ref method) == NAncientEventLayout.MethodName.OnDialogueHitboxClicked || (ref method) == NAncientEventLayout.MethodName.SetDialogueLineAndAnimate || (ref method) == NAncientEventLayout.MethodName.UpdateFakeNextButton || (ref method) == NAncientEventLayout.MethodName.HideNameBanner || (ref method) == NAncientEventLayout.MethodName.ShowNameBanner || (ref method) == NAncientEventLayout.MethodName.UpdateBannerVisibility || (ref method) == NAncientEventLayout.MethodName.UpdateHotkeyDisplay || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003969 RID: 14697 RVA: 0x001B2E6C File Offset: 0x001B106C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAncientEventLayout.PropertyName._currentDialogueLine)
			{
				this._currentDialogueLine = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._ancientBgContainer)
			{
				this._ancientBgContainer = VariantUtils.ConvertTo<NAncientBgContainer>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._ancientNameBanner)
			{
				this._ancientNameBanner = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._bannerTween)
			{
				this._bannerTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._contentContainer)
			{
				this._contentContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._originalContentContainerHeight)
			{
				this._originalContentContainerHeight = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._content)
			{
				this._content = VariantUtils.ConvertTo<VBoxContainer>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._dialogueContainer)
			{
				this._dialogueContainer = VariantUtils.ConvertTo<VBoxContainer>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._dialogueHitbox)
			{
				this._dialogueHitbox = VariantUtils.ConvertTo<NAncientDialogueHitbox>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._fakeNextButtonContainer)
			{
				this._fakeNextButtonContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._fakeNextButton)
			{
				this._fakeNextButton = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._fakeNextButtonControllerIcon)
			{
				this._fakeNextButtonControllerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._fakeNextButtonLabel)
			{
				this._fakeNextButtonLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._contentTween)
			{
				this._contentTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600396A RID: 14698 RVA: 0x001B2FFC File Offset: 0x001B11FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAncientEventLayout.PropertyName.IsDialogueOnLastLine)
			{
				bool isDialogueOnLastLine = this.IsDialogueOnLastLine;
				value = VariantUtils.CreateFrom<bool>(ref isDialogueOnLastLine);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._currentDialogueLine)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentDialogueLine);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._ancientBgContainer)
			{
				value = VariantUtils.CreateFrom<NAncientBgContainer>(ref this._ancientBgContainer);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._ancientNameBanner)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._ancientNameBanner);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._bannerTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._bannerTween);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._contentContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._contentContainer);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._originalContentContainerHeight)
			{
				value = VariantUtils.CreateFrom<float>(ref this._originalContentContainerHeight);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._content)
			{
				value = VariantUtils.CreateFrom<VBoxContainer>(ref this._content);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._dialogueContainer)
			{
				value = VariantUtils.CreateFrom<VBoxContainer>(ref this._dialogueContainer);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._dialogueHitbox)
			{
				value = VariantUtils.CreateFrom<NAncientDialogueHitbox>(ref this._dialogueHitbox);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._fakeNextButtonContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._fakeNextButtonContainer);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._fakeNextButton)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._fakeNextButton);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._fakeNextButtonControllerIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._fakeNextButtonControllerIcon);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._fakeNextButtonLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._fakeNextButtonLabel);
				return true;
			}
			if ((ref name) == NAncientEventLayout.PropertyName._contentTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._contentTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600396B RID: 14699 RVA: 0x001B3218 File Offset: 0x001B1418
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NAncientEventLayout.PropertyName._currentDialogueLine, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._ancientBgContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._ancientNameBanner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._bannerTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._contentContainer, 0L, "", 4096L, false),
				new PropertyInfo(3L, NAncientEventLayout.PropertyName._originalContentContainerHeight, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._content, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._dialogueContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._dialogueHitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._fakeNextButtonContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._fakeNextButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._fakeNextButtonControllerIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._fakeNextButtonLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName._contentTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NAncientEventLayout.PropertyName.IsDialogueOnLastLine, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientEventLayout.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600396C RID: 14700 RVA: 0x001B343C File Offset: 0x001B163C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAncientEventLayout.PropertyName._currentDialogueLine, Variant.From<int>(ref this._currentDialogueLine));
			info.AddProperty(NAncientEventLayout.PropertyName._ancientBgContainer, Variant.From<NAncientBgContainer>(ref this._ancientBgContainer));
			info.AddProperty(NAncientEventLayout.PropertyName._ancientNameBanner, Variant.From<Control>(ref this._ancientNameBanner));
			info.AddProperty(NAncientEventLayout.PropertyName._bannerTween, Variant.From<Tween>(ref this._bannerTween));
			info.AddProperty(NAncientEventLayout.PropertyName._contentContainer, Variant.From<Control>(ref this._contentContainer));
			info.AddProperty(NAncientEventLayout.PropertyName._originalContentContainerHeight, Variant.From<float>(ref this._originalContentContainerHeight));
			info.AddProperty(NAncientEventLayout.PropertyName._content, Variant.From<VBoxContainer>(ref this._content));
			info.AddProperty(NAncientEventLayout.PropertyName._dialogueContainer, Variant.From<VBoxContainer>(ref this._dialogueContainer));
			info.AddProperty(NAncientEventLayout.PropertyName._dialogueHitbox, Variant.From<NAncientDialogueHitbox>(ref this._dialogueHitbox));
			info.AddProperty(NAncientEventLayout.PropertyName._fakeNextButtonContainer, Variant.From<Control>(ref this._fakeNextButtonContainer));
			info.AddProperty(NAncientEventLayout.PropertyName._fakeNextButton, Variant.From<Control>(ref this._fakeNextButton));
			info.AddProperty(NAncientEventLayout.PropertyName._fakeNextButtonControllerIcon, Variant.From<TextureRect>(ref this._fakeNextButtonControllerIcon));
			info.AddProperty(NAncientEventLayout.PropertyName._fakeNextButtonLabel, Variant.From<MegaLabel>(ref this._fakeNextButtonLabel));
			info.AddProperty(NAncientEventLayout.PropertyName._contentTween, Variant.From<Tween>(ref this._contentTween));
		}

		/// <inheritdoc />
		// Token: 0x0600396D RID: 14701 RVA: 0x001B3584 File Offset: 0x001B1784
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._currentDialogueLine, ref variant))
			{
				this._currentDialogueLine = variant.As<int>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._ancientBgContainer, ref variant2))
			{
				this._ancientBgContainer = variant2.As<NAncientBgContainer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._ancientNameBanner, ref variant3))
			{
				this._ancientNameBanner = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._bannerTween, ref variant4))
			{
				this._bannerTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._contentContainer, ref variant5))
			{
				this._contentContainer = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._originalContentContainerHeight, ref variant6))
			{
				this._originalContentContainerHeight = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._content, ref variant7))
			{
				this._content = variant7.As<VBoxContainer>();
			}
			Variant variant8;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._dialogueContainer, ref variant8))
			{
				this._dialogueContainer = variant8.As<VBoxContainer>();
			}
			Variant variant9;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._dialogueHitbox, ref variant9))
			{
				this._dialogueHitbox = variant9.As<NAncientDialogueHitbox>();
			}
			Variant variant10;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._fakeNextButtonContainer, ref variant10))
			{
				this._fakeNextButtonContainer = variant10.As<Control>();
			}
			Variant variant11;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._fakeNextButton, ref variant11))
			{
				this._fakeNextButton = variant11.As<Control>();
			}
			Variant variant12;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._fakeNextButtonControllerIcon, ref variant12))
			{
				this._fakeNextButtonControllerIcon = variant12.As<TextureRect>();
			}
			Variant variant13;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._fakeNextButtonLabel, ref variant13))
			{
				this._fakeNextButtonLabel = variant13.As<MegaLabel>();
			}
			Variant variant14;
			if (info.TryGetProperty(NAncientEventLayout.PropertyName._contentTween, ref variant14))
			{
				this._contentTween = variant14.As<Tween>();
			}
		}

		// Token: 0x04001647 RID: 5703
		public const string ancientScenePath = "res://scenes/events/ancient_event_layout.tscn";

		// Token: 0x04001648 RID: 5704
		private const double _contentTweenDuration = 1.0;

		// Token: 0x04001649 RID: 5705
		private AncientEventModel _ancientEvent;

		// Token: 0x0400164A RID: 5706
		private readonly List<AncientDialogueLine> _dialogue = new List<AncientDialogueLine>();

		// Token: 0x0400164B RID: 5707
		private int _currentDialogueLine;

		// Token: 0x0400164C RID: 5708
		private NAncientBgContainer _ancientBgContainer;

		// Token: 0x0400164D RID: 5709
		[Nullable(2)]
		private Control _ancientNameBanner;

		// Token: 0x0400164E RID: 5710
		[Nullable(2)]
		private Tween _bannerTween;

		/// <summary>
		/// Parent node for the content container.
		/// We need this because we want to auto-scroll parts of the content container into view as the player progresses
		/// the dialogue.
		/// </summary>
		// Token: 0x0400164F RID: 5711
		private Control _contentContainer;

		/// <summary>
		/// We temporarily resize this container based on some other things happening in the layout, but we eventually want
		/// to restore it to its original height, so we set this during creation.
		/// </summary>
		// Token: 0x04001650 RID: 5712
		private float _originalContentContainerHeight;

		/// <summary>
		/// Contains the dialogue and the option buttons.
		/// </summary>
		// Token: 0x04001651 RID: 5713
		private VBoxContainer _content;

		// Token: 0x04001652 RID: 5714
		private VBoxContainer _dialogueContainer;

		/// <summary>
		/// The invisible hitbox that the player needs to click inside to advance the dialogue.
		/// </summary>
		// Token: 0x04001653 RID: 5715
		private NAncientDialogueHitbox _dialogueHitbox;

		/// <summary>
		/// The container for the fake "next button".
		/// We use this to size the bottom margin of the content container while the button is showing.
		/// </summary>
		// Token: 0x04001654 RID: 5716
		private Control _fakeNextButtonContainer;

		/// <summary>
		/// The "button" that lets the player advance the dialogue.
		/// This is fake because <see cref="F:MegaCrit.Sts2.Core.Nodes.Events.NAncientEventLayout._dialogueHitbox" /> is what's really listening for these clicks. The button is
		/// just to let the player know that they can continue, and to show some flavorful text for continuing.
		/// </summary>
		// Token: 0x04001655 RID: 5717
		private Control _fakeNextButton;

		// Token: 0x04001656 RID: 5718
		private TextureRect _fakeNextButtonControllerIcon;

		// Token: 0x04001657 RID: 5719
		private MegaLabel _fakeNextButtonLabel;

		// Token: 0x04001658 RID: 5720
		[Nullable(2)]
		private Tween _contentTween;

		// Token: 0x04001659 RID: 5721
		private CancellationTokenSource _cts = new CancellationTokenSource();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017C7 RID: 6087
		[NullableContext(0)]
		public new class MethodName : NEventLayout.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C95 RID: 23701
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005C96 RID: 23702
			public new static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005C97 RID: 23703
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'InitializeVisuals' method.
			/// </summary>
			// Token: 0x04005C98 RID: 23704
			public new static readonly StringName InitializeVisuals = "InitializeVisuals";

			/// <summary>
			/// Cached name for the 'AnimateIn' method.
			/// </summary>
			// Token: 0x04005C99 RID: 23705
			public new static readonly StringName AnimateIn = "AnimateIn";

			/// <summary>
			/// Cached name for the 'OnDialogueLineFocused' method.
			/// </summary>
			// Token: 0x04005C9A RID: 23706
			public static readonly StringName OnDialogueLineFocused = "OnDialogueLineFocused";

			/// <summary>
			/// Cached name for the 'OnDialogueLineUnfocused' method.
			/// </summary>
			// Token: 0x04005C9B RID: 23707
			public static readonly StringName OnDialogueLineUnfocused = "OnDialogueLineUnfocused";

			/// <summary>
			/// Cached name for the 'ClearDialogue' method.
			/// </summary>
			// Token: 0x04005C9C RID: 23708
			public static readonly StringName ClearDialogue = "ClearDialogue";

			/// <summary>
			/// Cached name for the 'OnSetupComplete' method.
			/// </summary>
			// Token: 0x04005C9D RID: 23709
			public new static readonly StringName OnSetupComplete = "OnSetupComplete";

			/// <summary>
			/// Cached name for the 'AnimateButtonsIn' method.
			/// </summary>
			// Token: 0x04005C9E RID: 23710
			public new static readonly StringName AnimateButtonsIn = "AnimateButtonsIn";

			/// <summary>
			/// Cached name for the 'OnDialogueHitboxClicked' method.
			/// </summary>
			// Token: 0x04005C9F RID: 23711
			public static readonly StringName OnDialogueHitboxClicked = "OnDialogueHitboxClicked";

			/// <summary>
			/// Cached name for the 'SetDialogueLineAndAnimate' method.
			/// </summary>
			// Token: 0x04005CA0 RID: 23712
			public static readonly StringName SetDialogueLineAndAnimate = "SetDialogueLineAndAnimate";

			/// <summary>
			/// Cached name for the 'UpdateFakeNextButton' method.
			/// </summary>
			// Token: 0x04005CA1 RID: 23713
			public static readonly StringName UpdateFakeNextButton = "UpdateFakeNextButton";

			/// <summary>
			/// Cached name for the 'HideNameBanner' method.
			/// </summary>
			// Token: 0x04005CA2 RID: 23714
			public static readonly StringName HideNameBanner = "HideNameBanner";

			/// <summary>
			/// Cached name for the 'ShowNameBanner' method.
			/// </summary>
			// Token: 0x04005CA3 RID: 23715
			public static readonly StringName ShowNameBanner = "ShowNameBanner";

			/// <summary>
			/// Cached name for the 'UpdateBannerVisibility' method.
			/// </summary>
			// Token: 0x04005CA4 RID: 23716
			public static readonly StringName UpdateBannerVisibility = "UpdateBannerVisibility";

			/// <summary>
			/// Cached name for the 'UpdateHotkeyDisplay' method.
			/// </summary>
			// Token: 0x04005CA5 RID: 23717
			public static readonly StringName UpdateHotkeyDisplay = "UpdateHotkeyDisplay";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017C8 RID: 6088
		[NullableContext(0)]
		public new class PropertyName : NEventLayout.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsDialogueOnLastLine' property.
			/// </summary>
			// Token: 0x04005CA6 RID: 23718
			public static readonly StringName IsDialogueOnLastLine = "IsDialogueOnLastLine";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005CA7 RID: 23719
			public new static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_currentDialogueLine' field.
			/// </summary>
			// Token: 0x04005CA8 RID: 23720
			public static readonly StringName _currentDialogueLine = "_currentDialogueLine";

			/// <summary>
			/// Cached name for the '_ancientBgContainer' field.
			/// </summary>
			// Token: 0x04005CA9 RID: 23721
			public static readonly StringName _ancientBgContainer = "_ancientBgContainer";

			/// <summary>
			/// Cached name for the '_ancientNameBanner' field.
			/// </summary>
			// Token: 0x04005CAA RID: 23722
			public static readonly StringName _ancientNameBanner = "_ancientNameBanner";

			/// <summary>
			/// Cached name for the '_bannerTween' field.
			/// </summary>
			// Token: 0x04005CAB RID: 23723
			public static readonly StringName _bannerTween = "_bannerTween";

			/// <summary>
			/// Cached name for the '_contentContainer' field.
			/// </summary>
			// Token: 0x04005CAC RID: 23724
			public static readonly StringName _contentContainer = "_contentContainer";

			/// <summary>
			/// Cached name for the '_originalContentContainerHeight' field.
			/// </summary>
			// Token: 0x04005CAD RID: 23725
			public static readonly StringName _originalContentContainerHeight = "_originalContentContainerHeight";

			/// <summary>
			/// Cached name for the '_content' field.
			/// </summary>
			// Token: 0x04005CAE RID: 23726
			public static readonly StringName _content = "_content";

			/// <summary>
			/// Cached name for the '_dialogueContainer' field.
			/// </summary>
			// Token: 0x04005CAF RID: 23727
			public static readonly StringName _dialogueContainer = "_dialogueContainer";

			/// <summary>
			/// Cached name for the '_dialogueHitbox' field.
			/// </summary>
			// Token: 0x04005CB0 RID: 23728
			public static readonly StringName _dialogueHitbox = "_dialogueHitbox";

			/// <summary>
			/// Cached name for the '_fakeNextButtonContainer' field.
			/// </summary>
			// Token: 0x04005CB1 RID: 23729
			public static readonly StringName _fakeNextButtonContainer = "_fakeNextButtonContainer";

			/// <summary>
			/// Cached name for the '_fakeNextButton' field.
			/// </summary>
			// Token: 0x04005CB2 RID: 23730
			public static readonly StringName _fakeNextButton = "_fakeNextButton";

			/// <summary>
			/// Cached name for the '_fakeNextButtonControllerIcon' field.
			/// </summary>
			// Token: 0x04005CB3 RID: 23731
			public static readonly StringName _fakeNextButtonControllerIcon = "_fakeNextButtonControllerIcon";

			/// <summary>
			/// Cached name for the '_fakeNextButtonLabel' field.
			/// </summary>
			// Token: 0x04005CB4 RID: 23732
			public static readonly StringName _fakeNextButtonLabel = "_fakeNextButtonLabel";

			/// <summary>
			/// Cached name for the '_contentTween' field.
			/// </summary>
			// Token: 0x04005CB5 RID: 23733
			public static readonly StringName _contentTween = "_contentTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017C9 RID: 6089
		[NullableContext(0)]
		public new class SignalName : NEventLayout.SignalName
		{
		}
	}
}
