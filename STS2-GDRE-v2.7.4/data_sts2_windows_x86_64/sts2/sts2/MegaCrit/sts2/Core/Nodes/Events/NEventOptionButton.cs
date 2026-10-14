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
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Events
{
	/// <summary>
	/// The nice big buttons that show up in event rooms.
	/// NOTE: The Ancient Event Button also uses this same script.
	/// </summary>
	// Token: 0x020003BE RID: 958
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Events/NEventOptionButton.cs")]
	public class NEventOptionButton : NButton
	{
		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x060039B9 RID: 14777 RVA: 0x001B5F6E File Offset: 0x001B416E
		// (set) Token: 0x060039BA RID: 14778 RVA: 0x001B5F76 File Offset: 0x001B4176
		public EventModel Event { get; private set; }

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x060039BB RID: 14779 RVA: 0x001B5F7F File Offset: 0x001B417F
		// (set) Token: 0x060039BC RID: 14780 RVA: 0x001B5F87 File Offset: 0x001B4187
		public EventOption Option { get; private set; }

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x060039BD RID: 14781 RVA: 0x001B5F90 File Offset: 0x001B4190
		// (set) Token: 0x060039BE RID: 14782 RVA: 0x001B5F98 File Offset: 0x001B4198
		private int Index { get; set; }

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060039BF RID: 14783 RVA: 0x001B5FA1 File Offset: 0x001B41A1
		public NMultiplayerVoteContainer VoteContainer
		{
			get
			{
				return this._playerVoteContainer;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x001B5FA9 File Offset: 0x001B41A9
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("events/event_option_button");
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x060039C1 RID: 14785 RVA: 0x001B5FB5 File Offset: 0x001B41B5
		private static string AncientScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("events/ancient_event_option_button");
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x060039C2 RID: 14786 RVA: 0x001B5FC1 File Offset: 0x001B41C1
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NEventOptionButton.ScenePath,
					NEventOptionButton.AncientScenePath,
					NEventOptionButton._voteIconPath
				});
			}
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x001B5FE8 File Offset: 0x001B41E8
		public static NEventOptionButton Create(EventModel eventModel, EventOption option, int index)
		{
			NEventOptionButton neventOptionButton = ((eventModel is AncientEventModel) ? PreloadManager.Cache.GetScene(NEventOptionButton.AncientScenePath).Instantiate<NEventOptionButton>(0L) : PreloadManager.Cache.GetScene(NEventOptionButton.ScenePath).Instantiate<NEventOptionButton>(0L));
			neventOptionButton.Event = eventModel;
			neventOptionButton.Option = option;
			neventOptionButton.Index = index;
			neventOptionButton._buttonColor = eventModel.ButtonColor;
			return neventOptionButton;
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x001B6050 File Offset: 0x001B4250
		public override void _Ready()
		{
			this.ConnectSignals();
			this.Event.DynamicVars.AddTo(this.Option.Description);
			this.Event.DynamicVars.AddTo(this.Option.Title);
			string formattedText = this.Option.Title.GetFormattedText();
			string formattedText2 = this.Option.Description.GetFormattedText();
			this._label = base.GetNode<MegaRichTextLabel>("%Text");
			if (string.IsNullOrEmpty(formattedText))
			{
				this._label.Text = formattedText2;
			}
			else
			{
				string text = (this.Option.IsLocked ? "red" : "gold");
				MegaRichTextLabel label = this._label;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 4);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral("][b]");
				defaultInterpolatedStringHandler.AppendFormatted(formattedText);
				defaultInterpolatedStringHandler.AppendLiteral("[/b][/");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral("]\n");
				defaultInterpolatedStringHandler.AppendFormatted(formattedText2);
				label.Text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			this._image = base.GetNode<NinePatchRect>("Image");
			this._image.Modulate = this._buttonColor;
			this._hsv = (ShaderMaterial)this._image.Material;
			this._outline = base.GetNode<NinePatchRect>("Outline");
			this._killGlow = base.GetNode<NinePatchRect>("RedFlash");
			this._confirmFlash = base.GetNode<NinePatchRect>("BlueFlash");
			this._playerVoteContainer = base.GetNode<NMultiplayerVoteContainer>("PlayerVoteContainer");
			this._playerVoteContainer.Initialize(new NMultiplayerVoteContainer.PlayerVotedDelegate(this.ShouldDisplayPlayerVote), this.Event.Owner.RunState.Players);
			if (this.Event is AncientEventModel && this.Option.Relic != null)
			{
				TextureRect node = base.GetNode<TextureRect>("%RelicIcon");
				node.SetTexture(this.Option.Relic.Icon);
				node.GetNode<TextureRect>("%Outline").SetTexture(this.Option.Relic.IconOutline);
				node.Visible = true;
			}
			if (this.Option.IsLocked)
			{
				this.SetVisuallyLocked();
				return;
			}
			if (this.WillKillPlayer())
			{
				this.ShowPersistentKillGlow();
			}
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x001B62C4 File Offset: 0x001B44C4
		private void SetVisuallyLocked()
		{
			this._label.Modulate = new Color(this._label.Modulate.R, this._label.Modulate.G, this._label.Modulate.B, 0.7f);
			ShaderMaterial hsv = this._hsv;
			if (hsv != null)
			{
				hsv.SetShaderParameter(NEventOptionButton._h, 0.48f);
			}
			ShaderMaterial hsv2 = this._hsv;
			if (hsv2 != null)
			{
				hsv2.SetShaderParameter(NEventOptionButton._s, 0.2f);
			}
			ShaderMaterial hsv3 = this._hsv;
			if (hsv3 == null)
			{
				return;
			}
			hsv3.SetShaderParameter(NEventOptionButton._v, 0.65f);
		}

		// Token: 0x060039C6 RID: 14790 RVA: 0x001B6378 File Offset: 0x001B4578
		public void AnimateIn()
		{
			if (!GodotObject.IsInstanceValid(this))
			{
				return;
			}
			if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
			{
				this.EnableButton();
				return;
			}
			base.Modulate = StsColors.transparentWhite;
			bool flag = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast;
			this._animInTween = base.CreateTween().SetParallel(true);
			this._animInTween.TweenInterval(flag ? 0.25 : 0.5);
			this._animInTween.Chain();
			this._animInTween.TweenInterval((flag ? 0.1 : 0.2) * (double)this.Index);
			this._animInTween.Chain();
			this._animInTween.TweenProperty(this, "position", base.Position, flag ? 0.25 : 0.5).SetEase(1L).SetTrans(10L)
				.From(base.Position + new Vector2(-60f, 0f));
			this._animInTween.TweenProperty(this, "modulate", Colors.White, flag ? 0.25 : 0.5);
			this._animInTween.Finished += this.EnableButton;
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x001B64F8 File Offset: 0x001B46F8
		public void EnableButton()
		{
			base.MouseFilter = 0L;
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x001B6504 File Offset: 0x001B4704
		protected override void OnRelease()
		{
			if (this.Option.IsLocked)
			{
				return;
			}
			if (this.WillKillPlayer())
			{
				if (this.Event.Owner.RunState.Players.Count > 1)
				{
					if (this._deathPreventionVfx == null)
					{
						LocString eventDeathPreventionLine = this.Event.Owner.Character.EventDeathPreventionLine;
						this._deathPreventionVfx = NThoughtBubbleVfx.Create(eventDeathPreventionLine.GetFormattedText(), DialogueSide.Right, null);
						NEventRoom instance = NEventRoom.Instance;
						if (instance != null)
						{
							Control vfxContainer = instance.VfxContainer;
							if (vfxContainer != null)
							{
								vfxContainer.AddChildSafely(this._deathPreventionVfx);
							}
						}
						this._deathPreventionVfxPosition = base.GlobalPosition + new Vector2(-50f, -15f);
						this._deathPreventionVfx.GlobalPosition = this._deathPreventionVfxPosition;
					}
					else
					{
						CancellationTokenSource deathPreventionCancellation = this._deathPreventionCancellation;
						if (deathPreventionCancellation != null)
						{
							deathPreventionCancellation.Cancel();
						}
					}
					TaskHelper.RunSafely(this.RumbleDeathVfx());
					TaskHelper.RunSafely(this.ExpireDeathPreventionVfx());
					return;
				}
				this.ShowPersistentKillGlow();
			}
			NEventRoom.Instance.OptionButtonClicked(this.Option, this.Index);
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x001B6620 File Offset: 0x001B4820
		private async Task RumbleDeathVfx()
		{
			ScreenRumbleInstance rumble = new ScreenRumbleInstance(20f, 0.30000001192092896, 10f, RumbleStyle.Rumble);
			while (!rumble.IsDone)
			{
				float num = await this.AwaitProcessFrame(this._cancelToken.Token);
				float num2 = num;
				Vector2 vector = rumble.Update((double)num2);
				this._deathPreventionVfx.GlobalPosition = this._deathPreventionVfxPosition + vector;
			}
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x001B6664 File Offset: 0x001B4864
		private async Task ExpireDeathPreventionVfx()
		{
			this._deathPreventionCancellation = new CancellationTokenSource();
			await Cmd.Wait(2.5f, this._deathPreventionCancellation.Token, true);
			if (!this._deathPreventionCancellation.IsCancellationRequested)
			{
				if (this._deathPreventionVfx != null)
				{
					TaskHelper.RunSafely(this._deathPreventionVfx.GoAway());
				}
				this._deathPreventionVfx = null;
			}
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x001B66A8 File Offset: 0x001B48A8
		protected override void OnPress()
		{
			if (this.Option.IsLocked)
			{
				return;
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", NEventOptionButton._pressScale, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), 1.2f, 0.9f, 0.5).SetEase(1L).SetTrans(5L);
			base.OnPress();
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x001B676C File Offset: 0x001B496C
		protected override void OnFocus()
		{
			if (this.Option.IsLocked)
			{
				return;
			}
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", NEventOptionButton._hoverScale, 0.05);
			this._tween.TweenProperty(this._outline, "modulate", StsColors.blueGlow, 0.05);
			ShaderMaterial hsv = this._hsv;
			if (hsv != null)
			{
				hsv.SetShaderParameter(NEventOptionButton._v, 1.2f);
			}
			NHoverTipSet.CreateAndShow(this, this.Option.HoverTips, (this.Event.LayoutType == EventLayoutType.Combat) ? HoverTipAlignment.Right : HoverTipAlignment.Left);
			if (this.WillKillPlayer())
			{
				this.PulseKillGlow();
			}
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x001B685C File Offset: 0x001B4A5C
		protected override void OnUnfocus()
		{
			if (this.Option.IsLocked)
			{
				return;
			}
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), 1.2f, 0.9f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._outline, "modulate:a", 0f, 0.3);
			NHoverTipSet.Remove(this);
			if (this.WillKillPlayer())
			{
				this.ShowPersistentKillGlow();
			}
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x001B6964 File Offset: 0x001B4B64
		public async Task FlashConfirmation()
		{
			Tween flashTween = this._flashTween;
			if (flashTween != null)
			{
				flashTween.Kill();
			}
			CanvasItem confirmFlash = this._confirmFlash;
			Color modulate = this._confirmFlash.Modulate;
			modulate.A = 0f;
			confirmFlash.Modulate = modulate;
			this._flashTween = base.CreateTween();
			this._flashTween.TweenProperty(this._confirmFlash, "modulate:a", 0.8f, 0.5).SetEase(1L).SetTrans(5L);
			this._flashTween.Parallel().TweenProperty(this._confirmFlash, "scale", new Vector2(1.03f, 1.1f), 0.25).SetEase(1L)
				.SetTrans(5L);
			this._flashTween.TweenProperty(this._confirmFlash, "scale", Vector2.One, 0.25).SetEase(1L).SetTrans(5L);
			this._flashTween.TweenProperty(this._confirmFlash, "modulate:a", 0f, 0.3);
			TaskAwaiter<bool> taskAwaiter = this._flashTween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				await Cmd.Wait(0.5f, this._cancelToken.Token, false);
			}
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x001B69A8 File Offset: 0x001B4BA8
		public void GrayOut()
		{
			Tween flashTween = this._flashTween;
			if (flashTween != null)
			{
				flashTween.Kill();
			}
			this._flashTween = base.CreateTween();
			Tween flashTween2 = this._flashTween;
			NodePath nodePath = "modulate";
			Color lightGray = StsColors.lightGray;
			lightGray.A = 0.5f;
			flashTween2.TweenProperty(this, nodePath, lightGray, 0.3).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x001B6A18 File Offset: 0x001B4C18
		private void UpdateShaderParam(float newV)
		{
			ShaderMaterial hsv = this._hsv;
			if (hsv == null)
			{
				return;
			}
			hsv.SetShaderParameter(NEventOptionButton._v, newV);
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x001B6A38 File Offset: 0x001B4C38
		private bool ShouldDisplayPlayerVote(Player player)
		{
			uint? playerVote = RunManager.Instance.EventSynchronizer.GetPlayerVote(player);
			long? num = ((playerVote != null) ? new long?((long)((ulong)playerVote.GetValueOrDefault())) : null);
			long num2 = (long)this.Index;
			return (num.GetValueOrDefault() == num2) & (num != null);
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x001B6A92 File Offset: 0x001B4C92
		public void RefreshVotes()
		{
			this._playerVoteContainer.RefreshPlayerVotes(true);
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x001B6AA0 File Offset: 0x001B4CA0
		public override void _ExitTree()
		{
			base._ExitTree();
			this._cancelToken.Cancel();
			if (this._animInTween != null)
			{
				this._animInTween.Finished -= this.EnableButton;
			}
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x001B6AD2 File Offset: 0x001B4CD2
		private bool WillKillPlayer()
		{
			if (this.Event.Owner != null)
			{
				Func<Player, bool> willKillPlayer = this.Option.WillKillPlayer;
				return willKillPlayer != null && willKillPlayer(this.Event.Owner);
			}
			return false;
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x001B6B04 File Offset: 0x001B4D04
		private void ShowPersistentKillGlow()
		{
			Tween killGlowTween = this._killGlowTween;
			if (killGlowTween != null)
			{
				killGlowTween.Kill();
			}
			CanvasItem killGlow = this._killGlow;
			Color modulate = this._killGlow.Modulate;
			modulate.A = 0.5f;
			killGlow.Modulate = modulate;
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x001B6B48 File Offset: 0x001B4D48
		private void PulseKillGlow()
		{
			Tween killGlowTween = this._killGlowTween;
			if (killGlowTween != null)
			{
				killGlowTween.Kill();
			}
			CanvasItem killGlow = this._killGlow;
			Color modulate = this._killGlow.Modulate;
			modulate.A = 0.25f;
			killGlow.Modulate = modulate;
			this._killGlowTween = base.CreateTween();
			this._killGlowTween.TweenProperty(this._killGlow, "modulate:a", 1f, 0.2).SetEase(1L).SetTrans(5L);
			this._killGlowTween.TweenProperty(this._killGlow, "modulate:a", 0.25f, 0.8);
			this._killGlowTween.SetLoops(0);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060039D7 RID: 14807 RVA: 0x001B6C10 File Offset: 0x001B4E10
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(15)
			{
				new MethodInfo(NEventOptionButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.SetVisuallyLocked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.AnimateIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.EnableButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.GrayOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "newV", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEventOptionButton.MethodName.RefreshVotes, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.WillKillPlayer, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.ShowPersistentKillGlow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEventOptionButton.MethodName.PulseKillGlow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060039D8 RID: 14808 RVA: 0x001B6F0C File Offset: 0x001B510C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEventOptionButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.SetVisuallyLocked && args.Count == 0)
			{
				this.SetVisuallyLocked();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.AnimateIn && args.Count == 0)
			{
				this.AnimateIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.EnableButton && args.Count == 0)
			{
				this.EnableButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.GrayOut && args.Count == 0)
			{
				this.GrayOut();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.RefreshVotes && args.Count == 0)
			{
				this.RefreshVotes();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.WillKillPlayer && args.Count == 0)
			{
				bool flag = this.WillKillPlayer();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.ShowPersistentKillGlow && args.Count == 0)
			{
				this.ShowPersistentKillGlow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEventOptionButton.MethodName.PulseKillGlow && args.Count == 0)
			{
				this.PulseKillGlow();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060039D9 RID: 14809 RVA: 0x001B7164 File Offset: 0x001B5364
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEventOptionButton.MethodName._Ready || (ref method) == NEventOptionButton.MethodName.SetVisuallyLocked || (ref method) == NEventOptionButton.MethodName.AnimateIn || (ref method) == NEventOptionButton.MethodName.EnableButton || (ref method) == NEventOptionButton.MethodName.OnRelease || (ref method) == NEventOptionButton.MethodName.OnPress || (ref method) == NEventOptionButton.MethodName.OnFocus || (ref method) == NEventOptionButton.MethodName.OnUnfocus || (ref method) == NEventOptionButton.MethodName.GrayOut || (ref method) == NEventOptionButton.MethodName.UpdateShaderParam || (ref method) == NEventOptionButton.MethodName.RefreshVotes || (ref method) == NEventOptionButton.MethodName._ExitTree || (ref method) == NEventOptionButton.MethodName.WillKillPlayer || (ref method) == NEventOptionButton.MethodName.ShowPersistentKillGlow || (ref method) == NEventOptionButton.MethodName.PulseKillGlow || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060039DA RID: 14810 RVA: 0x001B725C File Offset: 0x001B545C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEventOptionButton.PropertyName.Index)
			{
				this.Index = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<NinePatchRect>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<NinePatchRect>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._killGlow)
			{
				this._killGlow = VariantUtils.ConvertTo<NinePatchRect>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._confirmFlash)
			{
				this._confirmFlash = VariantUtils.ConvertTo<NinePatchRect>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._playerVoteContainer)
			{
				this._playerVoteContainer = VariantUtils.ConvertTo<NMultiplayerVoteContainer>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._animInTween)
			{
				this._animInTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._flashTween)
			{
				this._flashTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._killGlowTween)
			{
				this._killGlowTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._buttonColor)
			{
				this._buttonColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._deathPreventionVfx)
			{
				this._deathPreventionVfx = VariantUtils.ConvertTo<NThoughtBubbleVfx>(ref value);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._deathPreventionVfxPosition)
			{
				this._deathPreventionVfxPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060039DB RID: 14811 RVA: 0x001B7408 File Offset: 0x001B5608
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEventOptionButton.PropertyName.Index)
			{
				int index = this.Index;
				value = VariantUtils.CreateFrom<int>(ref index);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName.VoteContainer)
			{
				NMultiplayerVoteContainer voteContainer = this.VoteContainer;
				value = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref voteContainer);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<NinePatchRect>(ref this._image);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<NinePatchRect>(ref this._outline);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._killGlow)
			{
				value = VariantUtils.CreateFrom<NinePatchRect>(ref this._killGlow);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._confirmFlash)
			{
				value = VariantUtils.CreateFrom<NinePatchRect>(ref this._confirmFlash);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._playerVoteContainer)
			{
				value = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref this._playerVoteContainer);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._animInTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animInTween);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._flashTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._flashTween);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._killGlowTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._killGlowTween);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._buttonColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._buttonColor);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._deathPreventionVfx)
			{
				value = VariantUtils.CreateFrom<NThoughtBubbleVfx>(ref this._deathPreventionVfx);
				return true;
			}
			if ((ref name) == NEventOptionButton.PropertyName._deathPreventionVfxPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._deathPreventionVfxPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060039DC RID: 14812 RVA: 0x001B7624 File Offset: 0x001B5824
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEventOptionButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName._killGlow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName._confirmFlash, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName._playerVoteContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName._animInTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName._flashTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName._killGlowTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NEventOptionButton.PropertyName.Index, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName.VoteContainer, 0L, "", 4096L, false),
				new PropertyInfo(20L, NEventOptionButton.PropertyName._buttonColor, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEventOptionButton.PropertyName._deathPreventionVfx, 0L, "", 4096L, false),
				new PropertyInfo(5L, NEventOptionButton.PropertyName._deathPreventionVfxPosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060039DD RID: 14813 RVA: 0x001B7848 File Offset: 0x001B5A48
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName index = NEventOptionButton.PropertyName.Index;
			int index2 = this.Index;
			info.AddProperty(index, Variant.From<int>(ref index2));
			info.AddProperty(NEventOptionButton.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NEventOptionButton.PropertyName._image, Variant.From<NinePatchRect>(ref this._image));
			info.AddProperty(NEventOptionButton.PropertyName._outline, Variant.From<NinePatchRect>(ref this._outline));
			info.AddProperty(NEventOptionButton.PropertyName._killGlow, Variant.From<NinePatchRect>(ref this._killGlow));
			info.AddProperty(NEventOptionButton.PropertyName._confirmFlash, Variant.From<NinePatchRect>(ref this._confirmFlash));
			info.AddProperty(NEventOptionButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NEventOptionButton.PropertyName._playerVoteContainer, Variant.From<NMultiplayerVoteContainer>(ref this._playerVoteContainer));
			info.AddProperty(NEventOptionButton.PropertyName._animInTween, Variant.From<Tween>(ref this._animInTween));
			info.AddProperty(NEventOptionButton.PropertyName._flashTween, Variant.From<Tween>(ref this._flashTween));
			info.AddProperty(NEventOptionButton.PropertyName._killGlowTween, Variant.From<Tween>(ref this._killGlowTween));
			info.AddProperty(NEventOptionButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NEventOptionButton.PropertyName._buttonColor, Variant.From<Color>(ref this._buttonColor));
			info.AddProperty(NEventOptionButton.PropertyName._deathPreventionVfx, Variant.From<NThoughtBubbleVfx>(ref this._deathPreventionVfx));
			info.AddProperty(NEventOptionButton.PropertyName._deathPreventionVfxPosition, Variant.From<Vector2>(ref this._deathPreventionVfxPosition));
		}

		/// <inheritdoc />
		// Token: 0x060039DE RID: 14814 RVA: 0x001B79AC File Offset: 0x001B5BAC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEventOptionButton.PropertyName.Index, ref variant))
			{
				this.Index = variant.As<int>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._image, ref variant3))
			{
				this._image = variant3.As<NinePatchRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._outline, ref variant4))
			{
				this._outline = variant4.As<NinePatchRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._killGlow, ref variant5))
			{
				this._killGlow = variant5.As<NinePatchRect>();
			}
			Variant variant6;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._confirmFlash, ref variant6))
			{
				this._confirmFlash = variant6.As<NinePatchRect>();
			}
			Variant variant7;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._hsv, ref variant7))
			{
				this._hsv = variant7.As<ShaderMaterial>();
			}
			Variant variant8;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._playerVoteContainer, ref variant8))
			{
				this._playerVoteContainer = variant8.As<NMultiplayerVoteContainer>();
			}
			Variant variant9;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._animInTween, ref variant9))
			{
				this._animInTween = variant9.As<Tween>();
			}
			Variant variant10;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._flashTween, ref variant10))
			{
				this._flashTween = variant10.As<Tween>();
			}
			Variant variant11;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._killGlowTween, ref variant11))
			{
				this._killGlowTween = variant11.As<Tween>();
			}
			Variant variant12;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._tween, ref variant12))
			{
				this._tween = variant12.As<Tween>();
			}
			Variant variant13;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._buttonColor, ref variant13))
			{
				this._buttonColor = variant13.As<Color>();
			}
			Variant variant14;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._deathPreventionVfx, ref variant14))
			{
				this._deathPreventionVfx = variant14.As<NThoughtBubbleVfx>();
			}
			Variant variant15;
			if (info.TryGetProperty(NEventOptionButton.PropertyName._deathPreventionVfxPosition, ref variant15))
			{
				this._deathPreventionVfxPosition = variant15.As<Vector2>();
			}
		}

		// Token: 0x04001673 RID: 5747
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001674 RID: 5748
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04001675 RID: 5749
		private static readonly StringName _h = new StringName("h");

		// Token: 0x04001676 RID: 5750
		private static readonly string _voteIconPath = SceneHelper.GetScenePath("ui/multiplayer_vote_icon");

		// Token: 0x04001677 RID: 5751
		private MegaRichTextLabel _label;

		// Token: 0x04001678 RID: 5752
		private NinePatchRect _image;

		// Token: 0x04001679 RID: 5753
		private NinePatchRect _outline;

		// Token: 0x0400167A RID: 5754
		private NinePatchRect _killGlow;

		// Token: 0x0400167B RID: 5755
		private NinePatchRect _confirmFlash;

		// Token: 0x0400167C RID: 5756
		[Nullable(2)]
		private ShaderMaterial _hsv;

		// Token: 0x0400167D RID: 5757
		private NMultiplayerVoteContainer _playerVoteContainer;

		// Token: 0x0400167E RID: 5758
		[Nullable(2)]
		private Tween _animInTween;

		// Token: 0x0400167F RID: 5759
		[Nullable(2)]
		private Tween _flashTween;

		// Token: 0x04001680 RID: 5760
		[Nullable(2)]
		private Tween _killGlowTween;

		// Token: 0x04001681 RID: 5761
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001682 RID: 5762
		private static readonly Vector2 _hoverScale = Vector2.One * 1.01f;

		// Token: 0x04001683 RID: 5763
		private static readonly Vector2 _pressScale = Vector2.One * 0.99f;

		// Token: 0x04001684 RID: 5764
		private const float _defaultV = 0.9f;

		// Token: 0x04001685 RID: 5765
		private const float _hoverV = 1.2f;

		// Token: 0x04001686 RID: 5766
		private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

		// Token: 0x0400168A RID: 5770
		private Color _buttonColor;

		// Token: 0x0400168B RID: 5771
		[Nullable(2)]
		private NThoughtBubbleVfx _deathPreventionVfx;

		// Token: 0x0400168C RID: 5772
		[Nullable(2)]
		private CancellationTokenSource _deathPreventionCancellation;

		// Token: 0x0400168D RID: 5773
		private Vector2 _deathPreventionVfxPosition;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017D6 RID: 6102
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005CF4 RID: 23796
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetVisuallyLocked' method.
			/// </summary>
			// Token: 0x04005CF5 RID: 23797
			public static readonly StringName SetVisuallyLocked = "SetVisuallyLocked";

			/// <summary>
			/// Cached name for the 'AnimateIn' method.
			/// </summary>
			// Token: 0x04005CF6 RID: 23798
			public static readonly StringName AnimateIn = "AnimateIn";

			/// <summary>
			/// Cached name for the 'EnableButton' method.
			/// </summary>
			// Token: 0x04005CF7 RID: 23799
			public static readonly StringName EnableButton = "EnableButton";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005CF8 RID: 23800
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005CF9 RID: 23801
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005CFA RID: 23802
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005CFB RID: 23803
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'GrayOut' method.
			/// </summary>
			// Token: 0x04005CFC RID: 23804
			public static readonly StringName GrayOut = "GrayOut";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x04005CFD RID: 23805
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";

			/// <summary>
			/// Cached name for the 'RefreshVotes' method.
			/// </summary>
			// Token: 0x04005CFE RID: 23806
			public static readonly StringName RefreshVotes = "RefreshVotes";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005CFF RID: 23807
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'WillKillPlayer' method.
			/// </summary>
			// Token: 0x04005D00 RID: 23808
			public static readonly StringName WillKillPlayer = "WillKillPlayer";

			/// <summary>
			/// Cached name for the 'ShowPersistentKillGlow' method.
			/// </summary>
			// Token: 0x04005D01 RID: 23809
			public static readonly StringName ShowPersistentKillGlow = "ShowPersistentKillGlow";

			/// <summary>
			/// Cached name for the 'PulseKillGlow' method.
			/// </summary>
			// Token: 0x04005D02 RID: 23810
			public static readonly StringName PulseKillGlow = "PulseKillGlow";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017D7 RID: 6103
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Index' property.
			/// </summary>
			// Token: 0x04005D03 RID: 23811
			public static readonly StringName Index = "Index";

			/// <summary>
			/// Cached name for the 'VoteContainer' property.
			/// </summary>
			// Token: 0x04005D04 RID: 23812
			public static readonly StringName VoteContainer = "VoteContainer";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005D05 RID: 23813
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04005D06 RID: 23814
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04005D07 RID: 23815
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_killGlow' field.
			/// </summary>
			// Token: 0x04005D08 RID: 23816
			public static readonly StringName _killGlow = "_killGlow";

			/// <summary>
			/// Cached name for the '_confirmFlash' field.
			/// </summary>
			// Token: 0x04005D09 RID: 23817
			public static readonly StringName _confirmFlash = "_confirmFlash";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005D0A RID: 23818
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_playerVoteContainer' field.
			/// </summary>
			// Token: 0x04005D0B RID: 23819
			public static readonly StringName _playerVoteContainer = "_playerVoteContainer";

			/// <summary>
			/// Cached name for the '_animInTween' field.
			/// </summary>
			// Token: 0x04005D0C RID: 23820
			public static readonly StringName _animInTween = "_animInTween";

			/// <summary>
			/// Cached name for the '_flashTween' field.
			/// </summary>
			// Token: 0x04005D0D RID: 23821
			public static readonly StringName _flashTween = "_flashTween";

			/// <summary>
			/// Cached name for the '_killGlowTween' field.
			/// </summary>
			// Token: 0x04005D0E RID: 23822
			public static readonly StringName _killGlowTween = "_killGlowTween";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005D0F RID: 23823
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_buttonColor' field.
			/// </summary>
			// Token: 0x04005D10 RID: 23824
			public static readonly StringName _buttonColor = "_buttonColor";

			/// <summary>
			/// Cached name for the '_deathPreventionVfx' field.
			/// </summary>
			// Token: 0x04005D11 RID: 23825
			public static readonly StringName _deathPreventionVfx = "_deathPreventionVfx";

			/// <summary>
			/// Cached name for the '_deathPreventionVfxPosition' field.
			/// </summary>
			// Token: 0x04005D12 RID: 23826
			public static readonly StringName _deathPreventionVfxPosition = "_deathPreventionVfxPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017D8 RID: 6104
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
