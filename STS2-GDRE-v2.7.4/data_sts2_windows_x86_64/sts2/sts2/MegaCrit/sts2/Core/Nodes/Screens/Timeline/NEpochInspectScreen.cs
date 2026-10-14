using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	/// <summary>
	/// The screen which opens when you click on an Epoch to view the bigger image + flavor text.
	/// Note that there are two UX for this screen. One for first time open (plays the animation of
	/// the text crawling in) and the second time open, which skips this animation.
	/// </summary>
	// Token: 0x0200025D RID: 605
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NEpochInspectScreen.cs")]
	public class NEpochInspectScreen : NClickableControl, IScreenContext
	{
		// Token: 0x06001F81 RID: 8065 RVA: 0x000D21E8 File Offset: 0x000D03E8
		public override void _Ready()
		{
			this._storyLabel = base.GetNode<MegaLabel>("%StoryLabel");
			this._chapterLabel = base.GetNode<MegaLabel>("%ChapterLabel");
			this._placeholderLabel = base.GetNode<MegaLabel>("%PlaceholderLabel");
			this._portrait = base.GetNode<TextureRect>("%Portrait");
			this._portraitFlash = base.GetNode<TextureRect>("%PortraitFlash");
			this._mask = base.GetNode<TextureRect>("%Mask");
			this._maskOffsetY = this._mask.OffsetTop;
			this._maskOffsetX = this._mask.OffsetLeft;
			this._chains = base.GetNode<NEpochChains>("%Chains");
			this._portraitHsv = (ShaderMaterial)this._portrait.Material;
			this._closeButton = base.GetNode<NButton>("%CloseButton");
			this._fancyText = base.GetNode<MegaRichTextLabel>("%FancyText");
			this._closeLabel = base.GetNode<MegaLabel>("%CloseLabel");
			this._chapterLoc = new LocString("timeline", "EPOCH_INSPECT.chapterFormat");
			this._unlockInfo = base.GetNode<NUnlockInfo>("%UnlockInfo");
			this._nextChapterButton = base.GetNode<NEpochPaginateButton>("%NextChapterButton");
			this._prevChapterButton = base.GetNode<NEpochPaginateButton>("%PrevChapterButton");
			this._prevChapterButtonOffsetX = this._prevChapterButton.OffsetLeft;
			this._nextChapterButtonOffsetX = this._nextChapterButton.OffsetLeft;
			this._nextChapterButton.Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>(delegate(InputEvent _)
			{
				this.NextChapter();
			}), 0U);
			this._prevChapterButton.Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>(delegate(InputEvent _)
			{
				this.PrevChapter();
			}), 0U);
			base.Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>(new Action<InputEvent>(this.OnMouseReleased)), 0U);
			this._closeButton.Disable();
		}

		/// <summary>
		/// If the Epoch was revealed, we go through a slightly different UX flow (more dramatic opening of screen, queue up
		/// unlocks, etc)
		/// </summary>
		// Token: 0x06001F82 RID: 8066 RVA: 0x000D23F4 File Offset: 0x000D05F4
		public async Task Open(NEpochSlot slot, EpochModel epoch, bool wasRevealed)
		{
			Tween buttonTween = this._buttonTween;
			if (buttonTween != null)
			{
				buttonTween.FastForwardToCompletion();
			}
			this._wasRevealed = wasRevealed;
			this._epoch = epoch;
			if (!this._epoch.HasRealPortrait)
			{
				this._placeholderLabel.Visible = true;
				this._placeholderLabel.Text = NEpochInspectScreen._placeholderLoc.GetRawText();
			}
			else
			{
				this._placeholderLabel.Visible = false;
			}
			base.Modulate = Colors.White;
			this._portrait.Texture = epoch.RealPortrait;
			this._fancyText.Modulate = StsColors.transparentWhite;
			this._fancyText.Text = epoch.Description;
			this._hasStory = epoch.StoryTitle != null;
			SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_open_epoch", 1f);
			if (this._hasStory)
			{
				this._storyLabel.SetTextAutoSize(epoch.StoryTitle ?? string.Empty);
				this._chapterLoc.Add("ChapterIndex", epoch.ChapterIndex);
				this._chapterLoc.Add("ChapterName", epoch.Title);
				this._chapterLabel.SetTextAutoSize(this._chapterLoc.GetFormattedText());
				this._chapterLabel.VerticalAlignment = 1L;
				this._nextChapterButton.Enable();
				this._prevChapterButton.Enable();
			}
			else
			{
				this._storyLabel.SetTextAutoSize(string.Empty);
				this._chapterLabel.SetTextAutoSize(epoch.Title.GetFormattedText());
				this._chapterLabel.VerticalAlignment = 2L;
				this._nextChapterButton.Disable();
				this._prevChapterButton.Disable();
			}
			this._closeButton.MouseFilter = 0L;
			this._closeButton.Scale = Vector2.One;
			this._closeButtonY = this._closeButton.Position.Y + 180f;
			this._closeButton.Position = new Vector2(this._closeButton.Position.X, this._closeButtonY);
			this._closeButton.Modulate = StsColors.transparentWhite;
			this._closeButton.Disable();
			NTimelineScreen.Instance.ShowBackstopAndHideUi();
			base.Visible = true;
			Vector2 size = this._mask.Size;
			this._mask.GlobalPosition = slot.GlobalPosition;
			this._mask.SetDeferred(Control.PropertyName.Size, slot.Size * slot.GetGlobalTransform().Scale);
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._mask, "offset_left", this._maskOffsetX, 0.4).SetTrans(7L).SetEase(1L);
			this._tween.TweenProperty(this._mask, "offset_top", this._maskOffsetY, 0.4).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._mask, "size", size, 0.4).SetTrans(7L).SetEase(1L);
			this._storyLabel.Modulate = StsColors.transparentWhite;
			this._chapterLabel.Modulate = StsColors.transparentWhite;
			this._nextChapterButton.Modulate = StsColors.transparentWhite;
			this._prevChapterButton.Modulate = StsColors.transparentWhite;
			this._tween.TweenProperty(this._storyLabel, "modulate:a", 1f, 0.5).SetDelay(0.4);
			this._tween.TweenProperty(this._chapterLabel, "modulate:a", 1f, 0.5).SetDelay(0.2);
			this._tween.TweenProperty(this._prevChapterButton, "offset_left", this._prevChapterButtonOffsetX, 0.25).SetEase(1L).SetTrans(10L)
				.From(this._prevChapterButtonOffsetX + 100f)
				.SetDelay(0.25);
			this._tween.TweenProperty(this._prevChapterButton, "modulate:a", 1f, 0.25).SetDelay(0.25);
			this._tween.TweenProperty(this._nextChapterButton, "offset_left", this._nextChapterButtonOffsetX, 0.25).SetEase(1L).SetTrans(10L)
				.From(this._nextChapterButtonOffsetX - 100f)
				.SetDelay(0.25);
			this._tween.TweenProperty(this._nextChapterButton, "modulate:a", 1f, 0.25).SetDelay(0.25);
			if (wasRevealed)
			{
				await TaskHelper.RunSafely(this.UnlockAnimation(epoch));
			}
			else
			{
				this._closeLabel.SetTextAutoSize(new LocString("timeline", "EPOCH_INSPECT.closeButton").GetRawText());
				this._fancyText.Text = epoch.Description;
				Tween textTween = this._textTween;
				if (textTween != null)
				{
					textTween.Kill();
				}
				this._textTween = base.CreateTween().SetParallel(true);
				this._textTween.TweenProperty(this._fancyText, "modulate:a", 1f, 0.5).SetDelay(0.1);
				this._fancyText.VisibleRatio = 1f;
				Tween buttonTween2 = this._buttonTween;
				if (buttonTween2 != null)
				{
					buttonTween2.Kill();
				}
				this._buttonTween = base.CreateTween().SetParallel(true);
				this._buttonTween.TweenProperty(this._closeButton, "modulate:a", 1f, 0.3).SetEase(1L).SetTrans(7L)
					.SetDelay(0.1);
				this._buttonTween.TweenProperty(this._closeButton, "position:y", this._closeButtonY - 180f, 0.3).SetEase(1L).SetTrans(10L)
					.SetDelay(0.1);
				this._buttonTween.TweenCallback(Callable.From(new Action(this._closeButton.Enable)));
				this.RefreshChapterPaginators();
				this._unlockInfo.AnimIn(epoch.UnlockText);
			}
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x000D244F File Offset: 0x000D064F
		private void HidePaginators()
		{
			this._hasStory = false;
			this._nextChapterButton.Disable();
			this._prevChapterButton.Disable();
		}

		/// <summary>
		/// The Epoch Inspect Screen was navigated to via the Chapter Navigator button.
		/// </summary>
		// Token: 0x06001F84 RID: 8068 RVA: 0x000D2470 File Offset: 0x000D0670
		private void OpenViaPaginator(EpochModel epoch)
		{
			this._epoch = epoch;
			base.Modulate = Colors.White;
			this._fancyText.Text = epoch.Description;
			this._portrait.Texture = epoch.RealPortrait;
			this._hasStory = epoch.StoryTitle != null;
			this._storyLabel.Modulate = Colors.White;
			this._chapterLabel.Modulate = Colors.White;
			if (!this._epoch.HasRealPortrait)
			{
				this._placeholderLabel.Visible = true;
				this._placeholderLabel.Text = NEpochInspectScreen._placeholderLoc.GetRawText();
			}
			else
			{
				this._placeholderLabel.Visible = false;
			}
			if (this._hasStory)
			{
				this._storyLabel.SetTextAutoSize(epoch.StoryTitle ?? string.Empty);
				this._chapterLoc.Add("ChapterIndex", epoch.ChapterIndex);
				this._chapterLoc.Add("ChapterName", epoch.Title);
				this._chapterLabel.SetTextAutoSize(this._chapterLoc.GetFormattedText());
				this._chapterLabel.VerticalAlignment = 1L;
				this._nextChapterButton.Enable();
				this._prevChapterButton.Enable();
			}
			else
			{
				this._storyLabel.SetTextAutoSize(string.Empty);
				this._chapterLabel.SetTextAutoSize(epoch.Title.GetFormattedText());
				this._chapterLabel.VerticalAlignment = 2L;
				this._nextChapterButton.Disable();
				this._prevChapterButton.Disable();
			}
			this._fancyText.Modulate = StsColors.transparentWhite;
			NTimelineScreen.Instance.ShowBackstopAndHideUi();
			base.Visible = true;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			Tween textTween = this._textTween;
			if (textTween != null)
			{
				textTween.Kill();
			}
			this._textTween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._mask, "offset_top", this._maskOffsetY, 0.25).SetEase(1L).SetTrans(5L);
			this._textTween.TweenProperty(this._fancyText, "modulate:a", 1f, 0.5).SetDelay(0.1);
			this._fancyText.VisibleRatio = 1f;
			TaskHelper.RunSafely(this._unlockInfo.AnimInViaPaginator(epoch.UnlockText));
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x000D2700 File Offset: 0x000D0900
		public void Close()
		{
			if (NTimelineScreen.Instance.IsScreenQueued())
			{
				NTimelineScreen.Instance.OpenQueuedScreen();
			}
			else
			{
				NTimelineScreen.Instance.EnableInput();
				NTimelineScreen.Instance.HideBackstopAndShowUi(true);
			}
			base.FocusMode = 0L;
			Tween buttonTween = this._buttonTween;
			if (buttonTween != null)
			{
				buttonTween.FastForwardToCompletion();
			}
			Tween unlockTween = this._unlockTween;
			if (unlockTween != null)
			{
				unlockTween.FastForwardToCompletion();
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.FastForwardToCompletion();
			}
			Tween textTween = this._textTween;
			if (textTween != null)
			{
				textTween.FastForwardToCompletion();
			}
			this._buttonTween = base.CreateTween().SetParallel(true);
			this._buttonTween.TweenProperty(this._closeButton, "scale", new Vector2(3f, 0.1f), 0.3).SetEase(1L).SetTrans(5L);
			this._buttonTween.TweenProperty(this._closeButton, "modulate:a", 0f, 0.25).SetEase(1L).SetTrans(7L);
			this._buttonTween.TweenProperty(this, "modulate", new Color(0f, 0f, 0f, 0f), 0.5);
			this._buttonTween.TweenCallback(Callable.From(delegate
			{
				base.Visible = false;
				this._closeButton.Disable();
				if (this._wasRevealed)
				{
					AchievementsHelper.CheckTimelineComplete();
				}
			}));
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.right, new Action(this.NextChapter));
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.left, new Action(this.PrevChapter));
		}

		/// <summary>
		/// Is called when the player opens the Epoch inspect screen by clicking on a locked node in the Timeline.
		/// </summary>
		/// <param name="epoch"></param>
		// Token: 0x06001F86 RID: 8070 RVA: 0x000D28B8 File Offset: 0x000D0AB8
		public async Task UnlockAnimation(EpochModel epoch)
		{
			this.HidePaginators();
			epoch.QueueUnlocks();
			SaveManager.Instance.SaveProgressFile();
			this._unlockInfo.HideImmediately();
			this._closeLabel.SetTextAutoSize(new LocString("timeline", "EPOCH_INSPECT.continueButton").GetRawText());
			this._fancyText.VisibleRatio = 0f;
			this._fancyText.Modulate = StsColors.transparentWhite;
			this._portraitHsv.SetShaderParameter(NEpochInspectScreen._s, 0f);
			this._portraitHsv.SetShaderParameter(NEpochInspectScreen._v, 0.75f);
			this._chains.Texture = PreloadManager.Cache.GetTexture2D(NEpochInspectScreen.lockedImagePath);
			this._chains.Visible = true;
			this._chains.Modulate = Colors.White;
			this._chains.SelfModulate = Colors.White;
			this._portraitFlash.Modulate = new Color(1f, 1f, 1f, 0f);
			Tween unlockTween = this._unlockTween;
			if (unlockTween != null)
			{
				unlockTween.Kill();
			}
			this._unlockTween = base.CreateTween().SetParallel(true);
			this._unlockTween.TweenProperty(this._chains, "scale", Vector2.One * 0.98f, 0.5).SetEase(1L).SetTrans(5L)
				.SetDelay(0.5);
			TaskAwaiter<bool> taskAwaiter = this._unlockTween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this._chains.Unlock();
				await this._chains.AwaitSignal(NEpochChains.SignalName.OnAnimationFinished, this);
				this._portraitFlash.Modulate = Colors.White;
				this._unlockTween = base.CreateTween().SetParallel(true);
				this._unlockTween.TweenProperty(this._portraitFlash, "modulate:a", 0f, 0.5);
				this._unlockTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._portraitHsv.GetShaderParameter(NEpochInspectScreen._s), 1f, 1.0).SetEase(1L).SetTrans(5L);
				this._unlockTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._portraitHsv.GetShaderParameter(NEpochInspectScreen._v), 1f, 1.0).SetEase(1L).SetTrans(5L);
				Tween textTween = this._textTween;
				if (textTween != null)
				{
					textTween.Kill();
				}
				this._textTween = base.CreateTween().SetParallel(true);
				this._textTween.TweenProperty(this._fancyText, "modulate:a", 1f, 2.0).SetDelay(0.25);
				this._textTween.TweenProperty(this._fancyText, "visible_ratio", 1f, (double)this._fancyText.GetTotalCharacterCount() * 0.015).SetDelay(0.5);
				Tween buttonTween = this._buttonTween;
				if (buttonTween != null)
				{
					buttonTween.Kill();
				}
				this._buttonTween = base.CreateTween().SetParallel(true);
				this._buttonTween.TweenProperty(this._closeButton, "modulate:a", 1f, 0.3).SetEase(1L).SetTrans(7L)
					.SetDelay(1.0);
				this._buttonTween.TweenProperty(this._closeButton, "position:y", this._closeButtonY - 180f, 0.3).SetEase(1L).SetTrans(10L)
					.SetDelay(1.0);
				this._buttonTween.TweenCallback(Callable.From(new Action(this._closeButton.Enable)));
				bool flag = await this._unlockTween.AwaitFinished(this);
			}
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x000D2903 File Offset: 0x000D0B03
		private void UpdateShaderS(float value)
		{
			this._portraitHsv.SetShaderParameter(NEpochInspectScreen._s, value);
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x000D291B File Offset: 0x000D0B1B
		private void UpdateShaderV(float value)
		{
			this._portraitHsv.SetShaderParameter(NEpochInspectScreen._v, value);
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x000D2933 File Offset: 0x000D0B33
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionPressed(MegaInput.select, false, false) || inputEvent.IsActionPressed(MegaInput.accept, false, false))
			{
				this.SpeedUpTextAnimation();
			}
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x000D2959 File Offset: 0x000D0B59
		private void OnMouseReleased(InputEvent obj)
		{
			this.SpeedUpTextAnimation();
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x000D2964 File Offset: 0x000D0B64
		private void SpeedUpTextAnimation()
		{
			if (this._textTween == null)
			{
				return;
			}
			if (this._textTween.IsRunning())
			{
				this._textTween.Kill();
				this._fancyText.Modulate = Colors.White;
				this._fancyText.VisibleRatio = 1f;
			}
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x000D29B2 File Offset: 0x000D0BB2
		private void NextChapter()
		{
			this.OpenViaPaginator(this._nextChapterEpoch);
			this.RefreshChapterPaginators();
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x000D29C6 File Offset: 0x000D0BC6
		private void PrevChapter()
		{
			this.OpenViaPaginator(this._prevChapterEpoch);
			this.RefreshChapterPaginators();
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x000D29DC File Offset: 0x000D0BDC
		private void RefreshChapterPaginators()
		{
			if (!this._hasStory)
			{
				return;
			}
			this._nextChapterEpoch = StoryModel.NextChapter(this._epoch);
			this._prevChapterEpoch = StoryModel.PrevChapter(this._epoch);
			if (this._nextChapterEpoch != null)
			{
				NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.right, new Action(this.NextChapter));
				this._nextChapterButton.Visible = true;
			}
			else
			{
				this._nextChapterButton.Visible = false;
			}
			if (this._prevChapterEpoch != null)
			{
				NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.left, new Action(this.PrevChapter));
				this._prevChapterButton.Visible = true;
				return;
			}
			this._prevChapterButton.Visible = false;
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x000D2A97 File Offset: 0x000D0C97
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F90 RID: 8080 RVA: 0x000D2A9C File Offset: 0x000D0C9C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NEpochInspectScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochInspectScreen.MethodName.HidePaginators, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochInspectScreen.MethodName.Close, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochInspectScreen.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEpochInspectScreen.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEpochInspectScreen.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NEpochInspectScreen.MethodName.OnMouseReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "obj", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NEpochInspectScreen.MethodName.SpeedUpTextAnimation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochInspectScreen.MethodName.NextChapter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochInspectScreen.MethodName.PrevChapter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochInspectScreen.MethodName.RefreshChapterPaginators, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F91 RID: 8081 RVA: 0x000D2D64 File Offset: 0x000D0F64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochInspectScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochInspectScreen.MethodName.HidePaginators && args.Count == 0)
			{
				this.HidePaginators();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochInspectScreen.MethodName.Close && args.Count == 0)
			{
				this.Close();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochInspectScreen.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochInspectScreen.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochInspectScreen.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochInspectScreen.MethodName.OnMouseReleased && args.Count == 1)
			{
				this.OnMouseReleased(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochInspectScreen.MethodName.SpeedUpTextAnimation && args.Count == 0)
			{
				this.SpeedUpTextAnimation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochInspectScreen.MethodName.NextChapter && args.Count == 0)
			{
				this.NextChapter();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochInspectScreen.MethodName.PrevChapter && args.Count == 0)
			{
				this.PrevChapter();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochInspectScreen.MethodName.RefreshChapterPaginators && args.Count == 0)
			{
				this.RefreshChapterPaginators();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001F92 RID: 8082 RVA: 0x000D2F4C File Offset: 0x000D114C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEpochInspectScreen.MethodName._Ready || (ref method) == NEpochInspectScreen.MethodName.HidePaginators || (ref method) == NEpochInspectScreen.MethodName.Close || (ref method) == NEpochInspectScreen.MethodName.UpdateShaderS || (ref method) == NEpochInspectScreen.MethodName.UpdateShaderV || (ref method) == NEpochInspectScreen.MethodName._Input || (ref method) == NEpochInspectScreen.MethodName.OnMouseReleased || (ref method) == NEpochInspectScreen.MethodName.SpeedUpTextAnimation || (ref method) == NEpochInspectScreen.MethodName.NextChapter || (ref method) == NEpochInspectScreen.MethodName.PrevChapter || (ref method) == NEpochInspectScreen.MethodName.RefreshChapterPaginators || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001F93 RID: 8083 RVA: 0x000D3008 File Offset: 0x000D1208
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEpochInspectScreen.PropertyName._closeButton)
			{
				this._closeButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._portrait)
			{
				this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._portraitFlash)
			{
				this._portraitFlash = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._mask)
			{
				this._mask = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._chains)
			{
				this._chains = VariantUtils.ConvertTo<NEpochChains>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._portraitHsv)
			{
				this._portraitHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._fancyText)
			{
				this._fancyText = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._storyLabel)
			{
				this._storyLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._chapterLabel)
			{
				this._chapterLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._closeLabel)
			{
				this._closeLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._placeholderLabel)
			{
				this._placeholderLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._nextChapterButton)
			{
				this._nextChapterButton = VariantUtils.ConvertTo<NEpochPaginateButton>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._prevChapterButton)
			{
				this._prevChapterButton = VariantUtils.ConvertTo<NEpochPaginateButton>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._unlockInfo)
			{
				this._unlockInfo = VariantUtils.ConvertTo<NUnlockInfo>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._hasStory)
			{
				this._hasStory = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._wasRevealed)
			{
				this._wasRevealed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._prevChapterButtonOffsetX)
			{
				this._prevChapterButtonOffsetX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._nextChapterButtonOffsetX)
			{
				this._nextChapterButtonOffsetX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._maskOffsetX)
			{
				this._maskOffsetX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._maskOffsetY)
			{
				this._maskOffsetY = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._closeButtonY)
			{
				this._closeButtonY = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._unlockTween)
			{
				this._unlockTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._buttonTween)
			{
				this._buttonTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._textTween)
			{
				this._textTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06001F94 RID: 8084 RVA: 0x000D32C0 File Offset: 0x000D14C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEpochInspectScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._closeButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._closeButton);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._portrait)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._portraitFlash)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._portraitFlash);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._mask)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._mask);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._chains)
			{
				value = VariantUtils.CreateFrom<NEpochChains>(ref this._chains);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._portraitHsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._portraitHsv);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._fancyText)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._fancyText);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._storyLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._storyLabel);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._chapterLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._chapterLabel);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._closeLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._closeLabel);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._placeholderLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._placeholderLabel);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._nextChapterButton)
			{
				value = VariantUtils.CreateFrom<NEpochPaginateButton>(ref this._nextChapterButton);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._prevChapterButton)
			{
				value = VariantUtils.CreateFrom<NEpochPaginateButton>(ref this._prevChapterButton);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._unlockInfo)
			{
				value = VariantUtils.CreateFrom<NUnlockInfo>(ref this._unlockInfo);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._hasStory)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._hasStory);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._wasRevealed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._wasRevealed);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._prevChapterButtonOffsetX)
			{
				value = VariantUtils.CreateFrom<float>(ref this._prevChapterButtonOffsetX);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._nextChapterButtonOffsetX)
			{
				value = VariantUtils.CreateFrom<float>(ref this._nextChapterButtonOffsetX);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._maskOffsetX)
			{
				value = VariantUtils.CreateFrom<float>(ref this._maskOffsetX);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._maskOffsetY)
			{
				value = VariantUtils.CreateFrom<float>(ref this._maskOffsetY);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._closeButtonY)
			{
				value = VariantUtils.CreateFrom<float>(ref this._closeButtonY);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._unlockTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._unlockTween);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._buttonTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._buttonTween);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NEpochInspectScreen.PropertyName._textTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._textTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001F95 RID: 8085 RVA: 0x000D3618 File Offset: 0x000D1818
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._closeButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._portrait, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._portraitFlash, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._mask, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._chains, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._portraitHsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._fancyText, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._storyLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._chapterLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._closeLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._placeholderLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._nextChapterButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._prevChapterButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._unlockInfo, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEpochInspectScreen.PropertyName._hasStory, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEpochInspectScreen.PropertyName._wasRevealed, 0L, "", 4096L, false),
				new PropertyInfo(3L, NEpochInspectScreen.PropertyName._prevChapterButtonOffsetX, 0L, "", 4096L, false),
				new PropertyInfo(3L, NEpochInspectScreen.PropertyName._nextChapterButtonOffsetX, 0L, "", 4096L, false),
				new PropertyInfo(3L, NEpochInspectScreen.PropertyName._maskOffsetX, 0L, "", 4096L, false),
				new PropertyInfo(3L, NEpochInspectScreen.PropertyName._maskOffsetY, 0L, "", 4096L, false),
				new PropertyInfo(3L, NEpochInspectScreen.PropertyName._closeButtonY, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._unlockTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._buttonTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName._textTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochInspectScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001F96 RID: 8086 RVA: 0x000D3980 File Offset: 0x000D1B80
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEpochInspectScreen.PropertyName._closeButton, Variant.From<NButton>(ref this._closeButton));
			info.AddProperty(NEpochInspectScreen.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
			info.AddProperty(NEpochInspectScreen.PropertyName._portraitFlash, Variant.From<TextureRect>(ref this._portraitFlash));
			info.AddProperty(NEpochInspectScreen.PropertyName._mask, Variant.From<TextureRect>(ref this._mask));
			info.AddProperty(NEpochInspectScreen.PropertyName._chains, Variant.From<NEpochChains>(ref this._chains));
			info.AddProperty(NEpochInspectScreen.PropertyName._portraitHsv, Variant.From<ShaderMaterial>(ref this._portraitHsv));
			info.AddProperty(NEpochInspectScreen.PropertyName._fancyText, Variant.From<MegaRichTextLabel>(ref this._fancyText));
			info.AddProperty(NEpochInspectScreen.PropertyName._storyLabel, Variant.From<MegaLabel>(ref this._storyLabel));
			info.AddProperty(NEpochInspectScreen.PropertyName._chapterLabel, Variant.From<MegaLabel>(ref this._chapterLabel));
			info.AddProperty(NEpochInspectScreen.PropertyName._closeLabel, Variant.From<MegaLabel>(ref this._closeLabel));
			info.AddProperty(NEpochInspectScreen.PropertyName._placeholderLabel, Variant.From<MegaLabel>(ref this._placeholderLabel));
			info.AddProperty(NEpochInspectScreen.PropertyName._nextChapterButton, Variant.From<NEpochPaginateButton>(ref this._nextChapterButton));
			info.AddProperty(NEpochInspectScreen.PropertyName._prevChapterButton, Variant.From<NEpochPaginateButton>(ref this._prevChapterButton));
			info.AddProperty(NEpochInspectScreen.PropertyName._unlockInfo, Variant.From<NUnlockInfo>(ref this._unlockInfo));
			info.AddProperty(NEpochInspectScreen.PropertyName._hasStory, Variant.From<bool>(ref this._hasStory));
			info.AddProperty(NEpochInspectScreen.PropertyName._wasRevealed, Variant.From<bool>(ref this._wasRevealed));
			info.AddProperty(NEpochInspectScreen.PropertyName._prevChapterButtonOffsetX, Variant.From<float>(ref this._prevChapterButtonOffsetX));
			info.AddProperty(NEpochInspectScreen.PropertyName._nextChapterButtonOffsetX, Variant.From<float>(ref this._nextChapterButtonOffsetX));
			info.AddProperty(NEpochInspectScreen.PropertyName._maskOffsetX, Variant.From<float>(ref this._maskOffsetX));
			info.AddProperty(NEpochInspectScreen.PropertyName._maskOffsetY, Variant.From<float>(ref this._maskOffsetY));
			info.AddProperty(NEpochInspectScreen.PropertyName._closeButtonY, Variant.From<float>(ref this._closeButtonY));
			info.AddProperty(NEpochInspectScreen.PropertyName._unlockTween, Variant.From<Tween>(ref this._unlockTween));
			info.AddProperty(NEpochInspectScreen.PropertyName._buttonTween, Variant.From<Tween>(ref this._buttonTween));
			info.AddProperty(NEpochInspectScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NEpochInspectScreen.PropertyName._textTween, Variant.From<Tween>(ref this._textTween));
		}

		/// <inheritdoc />
		// Token: 0x06001F97 RID: 8087 RVA: 0x000D3BBC File Offset: 0x000D1DBC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._closeButton, ref variant))
			{
				this._closeButton = variant.As<NButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._portrait, ref variant2))
			{
				this._portrait = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._portraitFlash, ref variant3))
			{
				this._portraitFlash = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._mask, ref variant4))
			{
				this._mask = variant4.As<TextureRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._chains, ref variant5))
			{
				this._chains = variant5.As<NEpochChains>();
			}
			Variant variant6;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._portraitHsv, ref variant6))
			{
				this._portraitHsv = variant6.As<ShaderMaterial>();
			}
			Variant variant7;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._fancyText, ref variant7))
			{
				this._fancyText = variant7.As<MegaRichTextLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._storyLabel, ref variant8))
			{
				this._storyLabel = variant8.As<MegaLabel>();
			}
			Variant variant9;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._chapterLabel, ref variant9))
			{
				this._chapterLabel = variant9.As<MegaLabel>();
			}
			Variant variant10;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._closeLabel, ref variant10))
			{
				this._closeLabel = variant10.As<MegaLabel>();
			}
			Variant variant11;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._placeholderLabel, ref variant11))
			{
				this._placeholderLabel = variant11.As<MegaLabel>();
			}
			Variant variant12;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._nextChapterButton, ref variant12))
			{
				this._nextChapterButton = variant12.As<NEpochPaginateButton>();
			}
			Variant variant13;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._prevChapterButton, ref variant13))
			{
				this._prevChapterButton = variant13.As<NEpochPaginateButton>();
			}
			Variant variant14;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._unlockInfo, ref variant14))
			{
				this._unlockInfo = variant14.As<NUnlockInfo>();
			}
			Variant variant15;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._hasStory, ref variant15))
			{
				this._hasStory = variant15.As<bool>();
			}
			Variant variant16;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._wasRevealed, ref variant16))
			{
				this._wasRevealed = variant16.As<bool>();
			}
			Variant variant17;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._prevChapterButtonOffsetX, ref variant17))
			{
				this._prevChapterButtonOffsetX = variant17.As<float>();
			}
			Variant variant18;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._nextChapterButtonOffsetX, ref variant18))
			{
				this._nextChapterButtonOffsetX = variant18.As<float>();
			}
			Variant variant19;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._maskOffsetX, ref variant19))
			{
				this._maskOffsetX = variant19.As<float>();
			}
			Variant variant20;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._maskOffsetY, ref variant20))
			{
				this._maskOffsetY = variant20.As<float>();
			}
			Variant variant21;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._closeButtonY, ref variant21))
			{
				this._closeButtonY = variant21.As<float>();
			}
			Variant variant22;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._unlockTween, ref variant22))
			{
				this._unlockTween = variant22.As<Tween>();
			}
			Variant variant23;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._buttonTween, ref variant23))
			{
				this._buttonTween = variant23.As<Tween>();
			}
			Variant variant24;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._tween, ref variant24))
			{
				this._tween = variant24.As<Tween>();
			}
			Variant variant25;
			if (info.TryGetProperty(NEpochInspectScreen.PropertyName._textTween, ref variant25))
			{
				this._textTween = variant25.As<Tween>();
			}
		}

		// Token: 0x04000C29 RID: 3113
		private static readonly LocString _placeholderLoc = new LocString("timeline", "PLACEHOLDER_PORTRAIT");

		// Token: 0x04000C2A RID: 3114
		public static readonly string lockedImagePath = ImageHelper.GetImagePath("packed/timeline/epoch_slot_locked.png");

		// Token: 0x04000C2B RID: 3115
		private NButton _closeButton;

		// Token: 0x04000C2C RID: 3116
		private TextureRect _portrait;

		// Token: 0x04000C2D RID: 3117
		private TextureRect _portraitFlash;

		// Token: 0x04000C2E RID: 3118
		private TextureRect _mask;

		// Token: 0x04000C2F RID: 3119
		private NEpochChains _chains;

		// Token: 0x04000C30 RID: 3120
		private ShaderMaterial _portraitHsv;

		// Token: 0x04000C31 RID: 3121
		private MegaRichTextLabel _fancyText;

		// Token: 0x04000C32 RID: 3122
		private MegaLabel _storyLabel;

		// Token: 0x04000C33 RID: 3123
		private MegaLabel _chapterLabel;

		// Token: 0x04000C34 RID: 3124
		private MegaLabel _closeLabel;

		// Token: 0x04000C35 RID: 3125
		private MegaLabel _placeholderLabel;

		// Token: 0x04000C36 RID: 3126
		private NEpochPaginateButton _nextChapterButton;

		// Token: 0x04000C37 RID: 3127
		private NEpochPaginateButton _prevChapterButton;

		// Token: 0x04000C38 RID: 3128
		private NUnlockInfo _unlockInfo;

		// Token: 0x04000C39 RID: 3129
		private List<SerializableEpoch> _allEpochs;

		// Token: 0x04000C3A RID: 3130
		private EpochModel _epoch;

		// Token: 0x04000C3B RID: 3131
		[Nullable(2)]
		private EpochModel _prevChapterEpoch;

		// Token: 0x04000C3C RID: 3132
		[Nullable(2)]
		private EpochModel _nextChapterEpoch;

		// Token: 0x04000C3D RID: 3133
		private LocString _chapterLoc;

		// Token: 0x04000C3E RID: 3134
		private bool _hasStory;

		// Token: 0x04000C3F RID: 3135
		private bool _wasRevealed;

		// Token: 0x04000C40 RID: 3136
		private float _prevChapterButtonOffsetX;

		// Token: 0x04000C41 RID: 3137
		private float _nextChapterButtonOffsetX;

		// Token: 0x04000C42 RID: 3138
		private float _maskOffsetX;

		// Token: 0x04000C43 RID: 3139
		private float _maskOffsetY;

		// Token: 0x04000C44 RID: 3140
		private float _closeButtonY;

		// Token: 0x04000C45 RID: 3141
		[Nullable(2)]
		private Tween _unlockTween;

		// Token: 0x04000C46 RID: 3142
		[Nullable(2)]
		private Tween _buttonTween;

		// Token: 0x04000C47 RID: 3143
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000C48 RID: 3144
		[Nullable(2)]
		private Tween _textTween;

		// Token: 0x04000C49 RID: 3145
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000C4A RID: 3146
		private static readonly StringName _s = new StringName("s");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012A2 RID: 4770
		[NullableContext(0)]
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004860 RID: 18528
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'HidePaginators' method.
			/// </summary>
			// Token: 0x04004861 RID: 18529
			public static readonly StringName HidePaginators = "HidePaginators";

			/// <summary>
			/// Cached name for the 'Close' method.
			/// </summary>
			// Token: 0x04004862 RID: 18530
			public static readonly StringName Close = "Close";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x04004863 RID: 18531
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04004864 RID: 18532
			public static readonly StringName UpdateShaderV = "UpdateShaderV";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04004865 RID: 18533
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'OnMouseReleased' method.
			/// </summary>
			// Token: 0x04004866 RID: 18534
			public static readonly StringName OnMouseReleased = "OnMouseReleased";

			/// <summary>
			/// Cached name for the 'SpeedUpTextAnimation' method.
			/// </summary>
			// Token: 0x04004867 RID: 18535
			public static readonly StringName SpeedUpTextAnimation = "SpeedUpTextAnimation";

			/// <summary>
			/// Cached name for the 'NextChapter' method.
			/// </summary>
			// Token: 0x04004868 RID: 18536
			public static readonly StringName NextChapter = "NextChapter";

			/// <summary>
			/// Cached name for the 'PrevChapter' method.
			/// </summary>
			// Token: 0x04004869 RID: 18537
			public static readonly StringName PrevChapter = "PrevChapter";

			/// <summary>
			/// Cached name for the 'RefreshChapterPaginators' method.
			/// </summary>
			// Token: 0x0400486A RID: 18538
			public static readonly StringName RefreshChapterPaginators = "RefreshChapterPaginators";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012A3 RID: 4771
		[NullableContext(0)]
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x0400486B RID: 18539
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_closeButton' field.
			/// </summary>
			// Token: 0x0400486C RID: 18540
			public static readonly StringName _closeButton = "_closeButton";

			/// <summary>
			/// Cached name for the '_portrait' field.
			/// </summary>
			// Token: 0x0400486D RID: 18541
			public static readonly StringName _portrait = "_portrait";

			/// <summary>
			/// Cached name for the '_portraitFlash' field.
			/// </summary>
			// Token: 0x0400486E RID: 18542
			public static readonly StringName _portraitFlash = "_portraitFlash";

			/// <summary>
			/// Cached name for the '_mask' field.
			/// </summary>
			// Token: 0x0400486F RID: 18543
			public static readonly StringName _mask = "_mask";

			/// <summary>
			/// Cached name for the '_chains' field.
			/// </summary>
			// Token: 0x04004870 RID: 18544
			public static readonly StringName _chains = "_chains";

			/// <summary>
			/// Cached name for the '_portraitHsv' field.
			/// </summary>
			// Token: 0x04004871 RID: 18545
			public static readonly StringName _portraitHsv = "_portraitHsv";

			/// <summary>
			/// Cached name for the '_fancyText' field.
			/// </summary>
			// Token: 0x04004872 RID: 18546
			public static readonly StringName _fancyText = "_fancyText";

			/// <summary>
			/// Cached name for the '_storyLabel' field.
			/// </summary>
			// Token: 0x04004873 RID: 18547
			public static readonly StringName _storyLabel = "_storyLabel";

			/// <summary>
			/// Cached name for the '_chapterLabel' field.
			/// </summary>
			// Token: 0x04004874 RID: 18548
			public static readonly StringName _chapterLabel = "_chapterLabel";

			/// <summary>
			/// Cached name for the '_closeLabel' field.
			/// </summary>
			// Token: 0x04004875 RID: 18549
			public static readonly StringName _closeLabel = "_closeLabel";

			/// <summary>
			/// Cached name for the '_placeholderLabel' field.
			/// </summary>
			// Token: 0x04004876 RID: 18550
			public static readonly StringName _placeholderLabel = "_placeholderLabel";

			/// <summary>
			/// Cached name for the '_nextChapterButton' field.
			/// </summary>
			// Token: 0x04004877 RID: 18551
			public static readonly StringName _nextChapterButton = "_nextChapterButton";

			/// <summary>
			/// Cached name for the '_prevChapterButton' field.
			/// </summary>
			// Token: 0x04004878 RID: 18552
			public static readonly StringName _prevChapterButton = "_prevChapterButton";

			/// <summary>
			/// Cached name for the '_unlockInfo' field.
			/// </summary>
			// Token: 0x04004879 RID: 18553
			public static readonly StringName _unlockInfo = "_unlockInfo";

			/// <summary>
			/// Cached name for the '_hasStory' field.
			/// </summary>
			// Token: 0x0400487A RID: 18554
			public static readonly StringName _hasStory = "_hasStory";

			/// <summary>
			/// Cached name for the '_wasRevealed' field.
			/// </summary>
			// Token: 0x0400487B RID: 18555
			public static readonly StringName _wasRevealed = "_wasRevealed";

			/// <summary>
			/// Cached name for the '_prevChapterButtonOffsetX' field.
			/// </summary>
			// Token: 0x0400487C RID: 18556
			public static readonly StringName _prevChapterButtonOffsetX = "_prevChapterButtonOffsetX";

			/// <summary>
			/// Cached name for the '_nextChapterButtonOffsetX' field.
			/// </summary>
			// Token: 0x0400487D RID: 18557
			public static readonly StringName _nextChapterButtonOffsetX = "_nextChapterButtonOffsetX";

			/// <summary>
			/// Cached name for the '_maskOffsetX' field.
			/// </summary>
			// Token: 0x0400487E RID: 18558
			public static readonly StringName _maskOffsetX = "_maskOffsetX";

			/// <summary>
			/// Cached name for the '_maskOffsetY' field.
			/// </summary>
			// Token: 0x0400487F RID: 18559
			public static readonly StringName _maskOffsetY = "_maskOffsetY";

			/// <summary>
			/// Cached name for the '_closeButtonY' field.
			/// </summary>
			// Token: 0x04004880 RID: 18560
			public static readonly StringName _closeButtonY = "_closeButtonY";

			/// <summary>
			/// Cached name for the '_unlockTween' field.
			/// </summary>
			// Token: 0x04004881 RID: 18561
			public static readonly StringName _unlockTween = "_unlockTween";

			/// <summary>
			/// Cached name for the '_buttonTween' field.
			/// </summary>
			// Token: 0x04004882 RID: 18562
			public static readonly StringName _buttonTween = "_buttonTween";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004883 RID: 18563
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_textTween' field.
			/// </summary>
			// Token: 0x04004884 RID: 18564
			public static readonly StringName _textTween = "_textTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012A4 RID: 4772
		[NullableContext(0)]
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
