using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	// Token: 0x02000260 RID: 608
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NEpochSlot.cs")]
	public class NEpochSlot : NButton
	{
		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x000D458C File Offset: 0x000D278C
		protected override string ClickedSfx
		{
			get
			{
				return "event:/sfx/ui/timeline/ui_timeline_click";
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001FB5 RID: 8117 RVA: 0x000D4593 File Offset: 0x000D2793
		protected override string HoveredSfx
		{
			get
			{
				if (this.State != EpochSlotState.NotObtained)
				{
					return "event:/sfx/ui/timeline/ui_timeline_hover";
				}
				return "event:/sfx/ui/timeline/ui_timeline_hover_locked";
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x000D45A9 File Offset: 0x000D27A9
		// (set) Token: 0x06001FB7 RID: 8119 RVA: 0x000D45B1 File Offset: 0x000D27B1
		public EpochSlotState State { get; private set; }

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001FB8 RID: 8120 RVA: 0x000D45BA File Offset: 0x000D27BA
		// (set) Token: 0x06001FB9 RID: 8121 RVA: 0x000D45C2 File Offset: 0x000D27C2
		public bool HasSpawned { get; private set; }

		// Token: 0x06001FBA RID: 8122 RVA: 0x000D45CC File Offset: 0x000D27CC
		public override void _Ready()
		{
			this.ConnectSignals();
			this._slotImage = base.GetNode<TextureRect>("%SlotImage");
			this._portrait = base.GetNode<TextureRect>("%Portrait");
			this._chains = base.GetNode<TextureRect>("%Chains");
			this._hsv = (ShaderMaterial)this._portrait.GetMaterial();
			this._blurPortrait = base.GetNode<TextureRect>("%BlurPortrait");
			this._outline = base.GetNode<TextureRect>("%Outline");
			this._subViewportContainer = base.GetNode<SubViewportContainer>("%SubViewportContainer");
			this._subViewport = base.GetNode<SubViewport>("%SubViewport");
			this._blurShader = (ShaderMaterial)base.GetNode<Control>("%Blur").GetMaterial();
			this._selectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			if (!NGame.IsReleaseGame())
			{
				MegaLabel node = base.GetNode<MegaLabel>("%DebugLabel");
				node.Text = this.model.GetType().Name;
				node.Visible = true;
			}
			this.SetState(this.State);
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x000D4708 File Offset: 0x000D2908
		public static NEpochSlot Create(EpochSlotData data)
		{
			NEpochSlot nepochSlot = PreloadManager.Cache.GetScene("res://scenes/timeline_screen/epoch_slot.tscn").Instantiate<NEpochSlot>(0L);
			nepochSlot._era = data.Era;
			nepochSlot.State = data.State;
			nepochSlot.eraPosition = data.EraPosition;
			nepochSlot.model = data.Model;
			return nepochSlot;
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x000D4760 File Offset: 0x000D2960
		protected override void OnRelease()
		{
			if (!NGame.IsReleaseGame() && this.State == EpochSlotState.NotObtained && Input.IsKeyPressed(4194326L))
			{
				NHoverTipSet.Remove(this);
				this.State = EpochSlotState.Obtained;
			}
			base.OnRelease();
			if (this.State == EpochSlotState.Obtained)
			{
				NTimelineScreen.Instance.DisableInput();
				base.GetViewport().GuiReleaseFocus();
				this.RevealEpoch();
				this.SetState(EpochSlotState.Complete);
			}
			else if (this.State == EpochSlotState.Complete)
			{
				NTimelineScreen.Instance.DisableInput();
				NTimelineScreen.Instance.OpenInspectScreen(this, false);
			}
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One * 1.05f, 0.25).SetEase(1L).SetTrans(10L);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._s), 1f, 0.05);
		}

		/// <summary>
		/// Called when the player clicks on a glowing + locked slot!
		/// </summary>
		// Token: 0x06001FBD RID: 8125 RVA: 0x000D4890 File Offset: 0x000D2A90
		private void RevealEpoch()
		{
			this.State = EpochSlotState.Complete;
			this._portrait.Visible = true;
			this._portrait.Texture = this.model.Portrait;
			this.DisableHighlight();
			SaveManager.Instance.RevealEpoch(this.model.Id, false);
			this._slotImage.Modulate = Colors.White;
			this._slotImage.ClipChildren = 2L;
			this._chains.Visible = false;
			NTimelineScreen.Instance.OpenInspectScreen(this, true);
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x000D4918 File Offset: 0x000D2B18
		protected override void OnFocus()
		{
			base.OnFocus();
			this._isGlowPulsing = false;
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
			if (this.State != EpochSlotState.NotObtained)
			{
				this._hoverTween.TweenProperty(this, "scale", Vector2.One * 1.05f, 0.05);
				if (this.State == EpochSlotState.Complete)
				{
					this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NEpochSlot._s), 1.1f, 0.05);
					this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), 1.1f, 0.05);
					LocString unlockInfo = this.model.UnlockInfo;
					unlockInfo.Add("IsRevealed", true);
					this._hoverTip = new HoverTip(this.model.Title, unlockInfo, PreloadManager.Cache.GetTexture2D("res://images/packed/unlock_icon.png"));
				}
				else if (this.State == EpochSlotState.Obtained)
				{
					this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), 0.65f, 0.05);
					LocString unlockInfo2 = this.model.UnlockInfo;
					unlockInfo2.Add("IsRevealed", false);
					this._hoverTip = new HoverTip(this.model.Title, unlockInfo2, null);
				}
			}
			else
			{
				this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NEpochSlot._s), 0.25f, 0.05);
				this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), 1.2f, 0.05);
				LocString unlockInfo3 = this.model.UnlockInfo;
				unlockInfo3.Add("IsRevealed", false);
				this._hoverTip = new HoverTip(this.model.Title, unlockInfo3, null);
			}
			if (this._hoverTip != null)
			{
				NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
				float num = (base.Size.X * 0.5f + 6f) * base.GetGlobalTransform().Scale.X;
				float num2 = base.GlobalPosition.X + (base.Size.X * 0.5f + 6f) * base.GetGlobalTransform().Scale.X;
				if (base.GlobalPosition.X > NGame.Instance.GetViewportRect().Size.X * 0.7f)
				{
					if (nhoverTipSet != null)
					{
						nhoverTipSet.SetGlobalPosition(new Vector2(num2 - num - 360f, base.GlobalPosition.Y), false);
					}
				}
				else if (nhoverTipSet != null)
				{
					nhoverTipSet.SetGlobalPosition(new Vector2(num2 + num, base.GlobalPosition.Y), false);
				}
				if (nhoverTipSet != null)
				{
					nhoverTipSet.SetFollowOwner();
				}
			}
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x000D4CB8 File Offset: 0x000D2EB8
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this._selectionReticle.OnDeselect();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(7L);
			if (this.State != EpochSlotState.NotObtained)
			{
				if (this.State == EpochSlotState.Obtained)
				{
					this._isGlowPulsing = true;
					this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), 0.5f, 0.5).SetEase(1L).SetTrans(5L);
				}
				else if (this.State == EpochSlotState.Complete)
				{
					this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NEpochSlot._s), 1f, 0.5).SetEase(1L).SetTrans(5L);
					this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
				}
			}
			else
			{
				this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NEpochSlot._s), 0.25f, 0.5).SetEase(1L).SetTrans(5L);
				this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), 1.1f, 0.5).SetEase(1L).SetTrans(5L);
			}
			NHoverTipSet.Remove(this);
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x000D4EF0 File Offset: 0x000D30F0
		protected override void OnPress()
		{
			if (this.State == EpochSlotState.NotObtained)
			{
				return;
			}
			base.OnPress();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One * 0.95f, 0.25).SetEase(1L).SetTrans(5L);
			if (this.State == EpochSlotState.Complete)
			{
				this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NEpochSlot._s), 1f, 0.25).SetEase(1L).SetTrans(5L);
				this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), 1f, 0.25).SetEase(1L).SetTrans(5L);
				return;
			}
			if (this.State == EpochSlotState.Obtained)
			{
				this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NEpochSlot._v), 0.5f, 0.25).SetEase(1L).SetTrans(5L);
			}
		}

		/// <summary>
		/// Called when this slot is spawned in via Timeline Expansion OR your very first time opening the Timeline.
		/// Has a fun animation.
		/// </summary>
		// Token: 0x06001FC1 RID: 8129 RVA: 0x000D5074 File Offset: 0x000D3274
		public async Task SpawnSlot()
		{
			this._spawnTween = base.CreateTween().SetParallel(true);
			this._spawnTween.Chain();
			this._spawnTween.TweenInterval(Rng.Chaotic.NextDouble(0.0, 0.3));
			this._spawnTween.Chain();
			this._spawnTween.TweenProperty(this._slotImage, "position", Vector2.Zero, 0.5).SetEase(1L).SetTrans(10L)
				.From(new Vector2(0f, 64f));
			this._spawnTween.TweenProperty(this._outline, "position", Vector2.Zero, 0.5).SetEase(1L).SetTrans(10L)
				.From(new Vector2(0f, 64f));
			this._spawnTween.TweenProperty(this._slotImage, "self_modulate:a", 1f, 0.5).SetEase(1L).SetTrans(7L);
			TaskAwaiter<bool> taskAwaiter = this._spawnTween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this.HasSpawned = true;
				if (this.State == EpochSlotState.Obtained)
				{
					this.EnableHighlight();
				}
				else
				{
					this._outline.Modulate = NEpochSlot._defaultSlotOutlineColor;
				}
			}
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x000D50B8 File Offset: 0x000D32B8
		public override void _Process(double delta)
		{
			if (this._isGlowPulsing)
			{
				this._outline.Modulate = new Color(this._outline.Modulate.R, this._outline.Modulate.G, this._outline.Modulate.B, (Mathf.Sin(Time.GetTicksMsec() * 0.005f) + 2f) * 0.25f);
			}
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x000D512B File Offset: 0x000D332B
		private void DisableHighlight()
		{
			this._isGlowPulsing = false;
			this._outline.Modulate = Colors.Transparent;
			Control highlightVfx = this._highlightVfx;
			if (highlightVfx != null)
			{
				highlightVfx.QueueFreeSafely();
			}
			NEpochOffscreenVfx offscreenVfx = this._offscreenVfx;
			if (offscreenVfx == null)
			{
				return;
			}
			offscreenVfx.QueueFreeSafely();
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x000D5168 File Offset: 0x000D3368
		private void EnableHighlight()
		{
			this._isGlowPulsing = true;
			this._outline.Modulate = NEpochSlot._highlightSlotColor;
			this._outline.SelfModulate = StsColors.transparentWhite;
			Tween glowTween = this._glowTween;
			if (glowTween != null)
			{
				glowTween.Kill();
			}
			this._glowTween = base.CreateTween();
			this._glowTween.TweenProperty(this._outline, "self_modulate:a", 1f, 1.0);
			this._offscreenVfx = NEpochOffscreenVfx.Create(this);
			this._highlightVfx = NEpochHighlightVfx.Create();
			this.AddChildSafely(this._highlightVfx);
			NTimelineScreen.Instance.GetReminderVfxHolder().AddChildSafely(this._offscreenVfx);
			this.MoveChildSafely(this._highlightVfx, 0);
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x000D5230 File Offset: 0x000D3430
		public void SetState(EpochSlotState setState)
		{
			this.State = setState;
			if (this.State == EpochSlotState.None)
			{
				Log.Error("Slot State is invalid.", 2);
				return;
			}
			this._slotImage.Modulate = Colors.White;
			this._slotImage.ClipChildren = 2L;
			this._portrait.Visible = true;
			if (this.State == EpochSlotState.Complete)
			{
				this.DisableHighlight();
				this._portrait.Texture = this.model.Portrait;
				this.UpdateShaderS(1f);
				this.UpdateShaderV(1f);
			}
			else if (this.State == EpochSlotState.Obtained)
			{
				this._portrait.Texture = this.model.Portrait;
				this.UpdateShaderS(0f);
				this.UpdateShaderV(0.5f);
				this._chains.Visible = true;
			}
			else if (this.State == EpochSlotState.NotObtained)
			{
				this._blurShader.SetShaderParameter(NEpochSlot._lod, 2f);
				this.UpdateShaderS(0.25f);
				this.UpdateShaderV(1f);
				this._blurPortrait.Texture = this.model.Portrait;
				this._subViewportContainer.Visible = true;
				this._portrait.Texture = this._subViewport.GetTexture();
			}
			base.MouseDefaultCursorShape = ((this.State == EpochSlotState.Complete) ? 16L : 0L);
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000D538B File Offset: 0x000D358B
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NEpochSlot._s, value);
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x000D53A3 File Offset: 0x000D35A3
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NEpochSlot._v, value);
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x000D53BB File Offset: 0x000D35BB
		private void UpdateBlurLod(float value)
		{
			this._blurShader.SetShaderParameter(NEpochSlot._lod, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001FC9 RID: 8137 RVA: 0x000D53D4 File Offset: 0x000D35D4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(13)
			{
				new MethodInfo(NEpochSlot.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochSlot.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochSlot.MethodName.RevealEpoch, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochSlot.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochSlot.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochSlot.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochSlot.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEpochSlot.MethodName.DisableHighlight, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochSlot.MethodName.EnableHighlight, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochSlot.MethodName.SetState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "setState", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEpochSlot.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEpochSlot.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEpochSlot.MethodName.UpdateBlurLod, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001FCA RID: 8138 RVA: 0x000D5708 File Offset: 0x000D3908
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochSlot.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName.RevealEpoch && args.Count == 0)
			{
				this.RevealEpoch();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName.DisableHighlight && args.Count == 0)
			{
				this.DisableHighlight();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName.EnableHighlight && args.Count == 0)
			{
				this.EnableHighlight();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName.SetState && args.Count == 1)
			{
				this.SetState(VariantUtils.ConvertTo<EpochSlotState>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlot.MethodName.UpdateBlurLod && args.Count == 1)
			{
				this.UpdateBlurLod(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001FCB RID: 8139 RVA: 0x000D5948 File Offset: 0x000D3B48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEpochSlot.MethodName._Ready || (ref method) == NEpochSlot.MethodName.OnRelease || (ref method) == NEpochSlot.MethodName.RevealEpoch || (ref method) == NEpochSlot.MethodName.OnFocus || (ref method) == NEpochSlot.MethodName.OnUnfocus || (ref method) == NEpochSlot.MethodName.OnPress || (ref method) == NEpochSlot.MethodName._Process || (ref method) == NEpochSlot.MethodName.DisableHighlight || (ref method) == NEpochSlot.MethodName.EnableHighlight || (ref method) == NEpochSlot.MethodName.SetState || (ref method) == NEpochSlot.MethodName.UpdateShaderS || (ref method) == NEpochSlot.MethodName.UpdateShaderV || (ref method) == NEpochSlot.MethodName.UpdateBlurLod || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001FCC RID: 8140 RVA: 0x000D5A20 File Offset: 0x000D3C20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEpochSlot.PropertyName.State)
			{
				this.State = VariantUtils.ConvertTo<EpochSlotState>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName.HasSpawned)
			{
				this.HasSpawned = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._slotImage)
			{
				this._slotImage = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._portrait)
			{
				this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._chains)
			{
				this._chains = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._blurPortrait)
			{
				this._blurPortrait = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._blur)
			{
				this._blur = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._blurShader)
			{
				this._blurShader = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._offscreenVfx)
			{
				this._offscreenVfx = VariantUtils.ConvertTo<NEpochOffscreenVfx>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._highlightVfx)
			{
				this._highlightVfx = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._subViewportContainer)
			{
				this._subViewportContainer = VariantUtils.ConvertTo<SubViewportContainer>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._subViewport)
			{
				this._subViewport = VariantUtils.ConvertTo<SubViewport>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._isGlowPulsing)
			{
				this._isGlowPulsing = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._isComplete)
			{
				this._isComplete = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._isHovered)
			{
				this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._era)
			{
				this._era = VariantUtils.ConvertTo<EpochEra>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName.eraPosition)
			{
				this.eraPosition = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._glowTween)
			{
				this._glowTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._spawnTween)
			{
				this._spawnTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06001FCD RID: 8141 RVA: 0x000D5CA4 File Offset: 0x000D3EA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEpochSlot.PropertyName.ClickedSfx)
			{
				string text = this.ClickedSfx;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName.HoveredSfx)
			{
				string text = this.HoveredSfx;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName.State)
			{
				EpochSlotState state = this.State;
				value = VariantUtils.CreateFrom<EpochSlotState>(ref state);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName.HasSpawned)
			{
				bool hasSpawned = this.HasSpawned;
				value = VariantUtils.CreateFrom<bool>(ref hasSpawned);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._slotImage)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._slotImage);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._portrait)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._chains)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._chains);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._blurPortrait)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._blurPortrait);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._blur)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._blur);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._blurShader)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._blurShader);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._offscreenVfx)
			{
				value = VariantUtils.CreateFrom<NEpochOffscreenVfx>(ref this._offscreenVfx);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._highlightVfx)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._highlightVfx);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._subViewportContainer)
			{
				value = VariantUtils.CreateFrom<SubViewportContainer>(ref this._subViewportContainer);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._subViewport)
			{
				value = VariantUtils.CreateFrom<SubViewport>(ref this._subViewport);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._isGlowPulsing)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isGlowPulsing);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._isComplete)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isComplete);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._isHovered)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._era)
			{
				value = VariantUtils.CreateFrom<EpochEra>(ref this._era);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName.eraPosition)
			{
				value = VariantUtils.CreateFrom<int>(ref this.eraPosition);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._glowTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._glowTween);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._spawnTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._spawnTween);
				return true;
			}
			if ((ref name) == NEpochSlot.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001FCE RID: 8142 RVA: 0x000D5FE8 File Offset: 0x000D41E8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NEpochSlot.PropertyName.ClickedSfx, 0L, "", 4096L, false),
				new PropertyInfo(4L, NEpochSlot.PropertyName.HoveredSfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._slotImage, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._portrait, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._chains, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._blurPortrait, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._blur, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._blurShader, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._offscreenVfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._highlightVfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._subViewportContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._subViewport, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEpochSlot.PropertyName._isGlowPulsing, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEpochSlot.PropertyName._isComplete, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEpochSlot.PropertyName._isHovered, 0L, "", 4096L, false),
				new PropertyInfo(2L, NEpochSlot.PropertyName.State, 0L, "", 4096L, false),
				new PropertyInfo(2L, NEpochSlot.PropertyName._era, 0L, "", 4096L, false),
				new PropertyInfo(2L, NEpochSlot.PropertyName.eraPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._glowTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._spawnTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlot.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEpochSlot.PropertyName.HasSpawned, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001FCF RID: 8143 RVA: 0x000D632C File Offset: 0x000D452C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName state = NEpochSlot.PropertyName.State;
			EpochSlotState state2 = this.State;
			info.AddProperty(state, Variant.From<EpochSlotState>(ref state2));
			StringName hasSpawned = NEpochSlot.PropertyName.HasSpawned;
			bool hasSpawned2 = this.HasSpawned;
			info.AddProperty(hasSpawned, Variant.From<bool>(ref hasSpawned2));
			info.AddProperty(NEpochSlot.PropertyName._slotImage, Variant.From<TextureRect>(ref this._slotImage));
			info.AddProperty(NEpochSlot.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
			info.AddProperty(NEpochSlot.PropertyName._chains, Variant.From<TextureRect>(ref this._chains));
			info.AddProperty(NEpochSlot.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NEpochSlot.PropertyName._blurPortrait, Variant.From<TextureRect>(ref this._blurPortrait));
			info.AddProperty(NEpochSlot.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
			info.AddProperty(NEpochSlot.PropertyName._blur, Variant.From<Control>(ref this._blur));
			info.AddProperty(NEpochSlot.PropertyName._blurShader, Variant.From<ShaderMaterial>(ref this._blurShader));
			info.AddProperty(NEpochSlot.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NEpochSlot.PropertyName._offscreenVfx, Variant.From<NEpochOffscreenVfx>(ref this._offscreenVfx));
			info.AddProperty(NEpochSlot.PropertyName._highlightVfx, Variant.From<Control>(ref this._highlightVfx));
			info.AddProperty(NEpochSlot.PropertyName._subViewportContainer, Variant.From<SubViewportContainer>(ref this._subViewportContainer));
			info.AddProperty(NEpochSlot.PropertyName._subViewport, Variant.From<SubViewport>(ref this._subViewport));
			info.AddProperty(NEpochSlot.PropertyName._isGlowPulsing, Variant.From<bool>(ref this._isGlowPulsing));
			info.AddProperty(NEpochSlot.PropertyName._isComplete, Variant.From<bool>(ref this._isComplete));
			info.AddProperty(NEpochSlot.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
			info.AddProperty(NEpochSlot.PropertyName._era, Variant.From<EpochEra>(ref this._era));
			info.AddProperty(NEpochSlot.PropertyName.eraPosition, Variant.From<int>(ref this.eraPosition));
			info.AddProperty(NEpochSlot.PropertyName._glowTween, Variant.From<Tween>(ref this._glowTween));
			info.AddProperty(NEpochSlot.PropertyName._spawnTween, Variant.From<Tween>(ref this._spawnTween));
			info.AddProperty(NEpochSlot.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
		}

		/// <inheritdoc />
		// Token: 0x06001FD0 RID: 8144 RVA: 0x000D6540 File Offset: 0x000D4740
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEpochSlot.PropertyName.State, ref variant))
			{
				this.State = variant.As<EpochSlotState>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEpochSlot.PropertyName.HasSpawned, ref variant2))
			{
				this.HasSpawned = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEpochSlot.PropertyName._slotImage, ref variant3))
			{
				this._slotImage = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEpochSlot.PropertyName._portrait, ref variant4))
			{
				this._portrait = variant4.As<TextureRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NEpochSlot.PropertyName._chains, ref variant5))
			{
				this._chains = variant5.As<TextureRect>();
			}
			Variant variant6;
			if (info.TryGetProperty(NEpochSlot.PropertyName._hsv, ref variant6))
			{
				this._hsv = variant6.As<ShaderMaterial>();
			}
			Variant variant7;
			if (info.TryGetProperty(NEpochSlot.PropertyName._blurPortrait, ref variant7))
			{
				this._blurPortrait = variant7.As<TextureRect>();
			}
			Variant variant8;
			if (info.TryGetProperty(NEpochSlot.PropertyName._outline, ref variant8))
			{
				this._outline = variant8.As<TextureRect>();
			}
			Variant variant9;
			if (info.TryGetProperty(NEpochSlot.PropertyName._blur, ref variant9))
			{
				this._blur = variant9.As<Control>();
			}
			Variant variant10;
			if (info.TryGetProperty(NEpochSlot.PropertyName._blurShader, ref variant10))
			{
				this._blurShader = variant10.As<ShaderMaterial>();
			}
			Variant variant11;
			if (info.TryGetProperty(NEpochSlot.PropertyName._selectionReticle, ref variant11))
			{
				this._selectionReticle = variant11.As<NSelectionReticle>();
			}
			Variant variant12;
			if (info.TryGetProperty(NEpochSlot.PropertyName._offscreenVfx, ref variant12))
			{
				this._offscreenVfx = variant12.As<NEpochOffscreenVfx>();
			}
			Variant variant13;
			if (info.TryGetProperty(NEpochSlot.PropertyName._highlightVfx, ref variant13))
			{
				this._highlightVfx = variant13.As<Control>();
			}
			Variant variant14;
			if (info.TryGetProperty(NEpochSlot.PropertyName._subViewportContainer, ref variant14))
			{
				this._subViewportContainer = variant14.As<SubViewportContainer>();
			}
			Variant variant15;
			if (info.TryGetProperty(NEpochSlot.PropertyName._subViewport, ref variant15))
			{
				this._subViewport = variant15.As<SubViewport>();
			}
			Variant variant16;
			if (info.TryGetProperty(NEpochSlot.PropertyName._isGlowPulsing, ref variant16))
			{
				this._isGlowPulsing = variant16.As<bool>();
			}
			Variant variant17;
			if (info.TryGetProperty(NEpochSlot.PropertyName._isComplete, ref variant17))
			{
				this._isComplete = variant17.As<bool>();
			}
			Variant variant18;
			if (info.TryGetProperty(NEpochSlot.PropertyName._isHovered, ref variant18))
			{
				this._isHovered = variant18.As<bool>();
			}
			Variant variant19;
			if (info.TryGetProperty(NEpochSlot.PropertyName._era, ref variant19))
			{
				this._era = variant19.As<EpochEra>();
			}
			Variant variant20;
			if (info.TryGetProperty(NEpochSlot.PropertyName.eraPosition, ref variant20))
			{
				this.eraPosition = variant20.As<int>();
			}
			Variant variant21;
			if (info.TryGetProperty(NEpochSlot.PropertyName._glowTween, ref variant21))
			{
				this._glowTween = variant21.As<Tween>();
			}
			Variant variant22;
			if (info.TryGetProperty(NEpochSlot.PropertyName._spawnTween, ref variant22))
			{
				this._spawnTween = variant22.As<Tween>();
			}
			Variant variant23;
			if (info.TryGetProperty(NEpochSlot.PropertyName._hoverTween, ref variant23))
			{
				this._hoverTween = variant23.As<Tween>();
			}
		}

		// Token: 0x04000C4F RID: 3151
		private static readonly StringName _lod = new StringName("lod");

		// Token: 0x04000C50 RID: 3152
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000C51 RID: 3153
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04000C52 RID: 3154
		private const string _unlockIconPath = "res://images/packed/unlock_icon.png";

		// Token: 0x04000C53 RID: 3155
		private const string _scenePath = "res://scenes/timeline_screen/epoch_slot.tscn";

		// Token: 0x04000C54 RID: 3156
		public static readonly IEnumerable<string> assetPaths = new <>z__ReadOnlyArray<string>(new string[] { "res://scenes/timeline_screen/epoch_slot.tscn", "res://images/packed/unlock_icon.png" });

		// Token: 0x04000C55 RID: 3157
		private TextureRect _slotImage;

		// Token: 0x04000C56 RID: 3158
		private TextureRect _portrait;

		// Token: 0x04000C57 RID: 3159
		private TextureRect _chains;

		// Token: 0x04000C58 RID: 3160
		private ShaderMaterial _hsv;

		// Token: 0x04000C59 RID: 3161
		private TextureRect _blurPortrait;

		// Token: 0x04000C5A RID: 3162
		private TextureRect _outline;

		// Token: 0x04000C5B RID: 3163
		private Control _blur;

		// Token: 0x04000C5C RID: 3164
		private ShaderMaterial _blurShader;

		// Token: 0x04000C5D RID: 3165
		private NSelectionReticle _selectionReticle;

		// Token: 0x04000C5E RID: 3166
		[Nullable(2)]
		private NEpochOffscreenVfx _offscreenVfx;

		// Token: 0x04000C5F RID: 3167
		[Nullable(2)]
		private Control _highlightVfx;

		// Token: 0x04000C60 RID: 3168
		private SubViewportContainer _subViewportContainer;

		// Token: 0x04000C61 RID: 3169
		private SubViewport _subViewport;

		// Token: 0x04000C62 RID: 3170
		private bool _isGlowPulsing;

		// Token: 0x04000C63 RID: 3171
		public EpochModel model;

		// Token: 0x04000C64 RID: 3172
		private bool _isComplete;

		// Token: 0x04000C65 RID: 3173
		private bool _isHovered;

		// Token: 0x04000C67 RID: 3175
		private EpochEra _era;

		// Token: 0x04000C68 RID: 3176
		public int eraPosition;

		// Token: 0x04000C69 RID: 3177
		[Nullable(2)]
		private Tween _glowTween;

		// Token: 0x04000C6A RID: 3178
		[Nullable(2)]
		private Tween _spawnTween;

		// Token: 0x04000C6B RID: 3179
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x04000C6D RID: 3181
		private static readonly Color _highlightSlotColor = StsColors.purple;

		// Token: 0x04000C6E RID: 3182
		private static readonly Color _defaultSlotOutlineColor = new Color("70a0ff18");

		// Token: 0x04000C6F RID: 3183
		[Nullable(2)]
		private IHoverTip _hoverTip;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012AD RID: 4781
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400489B RID: 18587
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x0400489C RID: 18588
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'RevealEpoch' method.
			/// </summary>
			// Token: 0x0400489D RID: 18589
			public static readonly StringName RevealEpoch = "RevealEpoch";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400489E RID: 18590
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400489F RID: 18591
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x040048A0 RID: 18592
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040048A1 RID: 18593
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'DisableHighlight' method.
			/// </summary>
			// Token: 0x040048A2 RID: 18594
			public static readonly StringName DisableHighlight = "DisableHighlight";

			/// <summary>
			/// Cached name for the 'EnableHighlight' method.
			/// </summary>
			// Token: 0x040048A3 RID: 18595
			public static readonly StringName EnableHighlight = "EnableHighlight";

			/// <summary>
			/// Cached name for the 'SetState' method.
			/// </summary>
			// Token: 0x040048A4 RID: 18596
			public static readonly StringName SetState = "SetState";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x040048A5 RID: 18597
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x040048A6 RID: 18598
			public static readonly StringName UpdateShaderV = "UpdateShaderV";

			/// <summary>
			/// Cached name for the 'UpdateBlurLod' method.
			/// </summary>
			// Token: 0x040048A7 RID: 18599
			public static readonly StringName UpdateBlurLod = "UpdateBlurLod";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012AE RID: 4782
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ClickedSfx' property.
			/// </summary>
			// Token: 0x040048A8 RID: 18600
			public new static readonly StringName ClickedSfx = "ClickedSfx";

			/// <summary>
			/// Cached name for the 'HoveredSfx' property.
			/// </summary>
			// Token: 0x040048A9 RID: 18601
			public new static readonly StringName HoveredSfx = "HoveredSfx";

			/// <summary>
			/// Cached name for the 'State' property.
			/// </summary>
			// Token: 0x040048AA RID: 18602
			public static readonly StringName State = "State";

			/// <summary>
			/// Cached name for the 'HasSpawned' property.
			/// </summary>
			// Token: 0x040048AB RID: 18603
			public static readonly StringName HasSpawned = "HasSpawned";

			/// <summary>
			/// Cached name for the '_slotImage' field.
			/// </summary>
			// Token: 0x040048AC RID: 18604
			public static readonly StringName _slotImage = "_slotImage";

			/// <summary>
			/// Cached name for the '_portrait' field.
			/// </summary>
			// Token: 0x040048AD RID: 18605
			public static readonly StringName _portrait = "_portrait";

			/// <summary>
			/// Cached name for the '_chains' field.
			/// </summary>
			// Token: 0x040048AE RID: 18606
			public static readonly StringName _chains = "_chains";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x040048AF RID: 18607
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_blurPortrait' field.
			/// </summary>
			// Token: 0x040048B0 RID: 18608
			public static readonly StringName _blurPortrait = "_blurPortrait";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x040048B1 RID: 18609
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_blur' field.
			/// </summary>
			// Token: 0x040048B2 RID: 18610
			public static readonly StringName _blur = "_blur";

			/// <summary>
			/// Cached name for the '_blurShader' field.
			/// </summary>
			// Token: 0x040048B3 RID: 18611
			public static readonly StringName _blurShader = "_blurShader";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x040048B4 RID: 18612
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_offscreenVfx' field.
			/// </summary>
			// Token: 0x040048B5 RID: 18613
			public static readonly StringName _offscreenVfx = "_offscreenVfx";

			/// <summary>
			/// Cached name for the '_highlightVfx' field.
			/// </summary>
			// Token: 0x040048B6 RID: 18614
			public static readonly StringName _highlightVfx = "_highlightVfx";

			/// <summary>
			/// Cached name for the '_subViewportContainer' field.
			/// </summary>
			// Token: 0x040048B7 RID: 18615
			public static readonly StringName _subViewportContainer = "_subViewportContainer";

			/// <summary>
			/// Cached name for the '_subViewport' field.
			/// </summary>
			// Token: 0x040048B8 RID: 18616
			public static readonly StringName _subViewport = "_subViewport";

			/// <summary>
			/// Cached name for the '_isGlowPulsing' field.
			/// </summary>
			// Token: 0x040048B9 RID: 18617
			public static readonly StringName _isGlowPulsing = "_isGlowPulsing";

			/// <summary>
			/// Cached name for the '_isComplete' field.
			/// </summary>
			// Token: 0x040048BA RID: 18618
			public static readonly StringName _isComplete = "_isComplete";

			/// <summary>
			/// Cached name for the '_isHovered' field.
			/// </summary>
			// Token: 0x040048BB RID: 18619
			public new static readonly StringName _isHovered = "_isHovered";

			/// <summary>
			/// Cached name for the '_era' field.
			/// </summary>
			// Token: 0x040048BC RID: 18620
			public static readonly StringName _era = "_era";

			/// <summary>
			/// Cached name for the 'eraPosition' field.
			/// </summary>
			// Token: 0x040048BD RID: 18621
			public static readonly StringName eraPosition = "eraPosition";

			/// <summary>
			/// Cached name for the '_glowTween' field.
			/// </summary>
			// Token: 0x040048BE RID: 18622
			public static readonly StringName _glowTween = "_glowTween";

			/// <summary>
			/// Cached name for the '_spawnTween' field.
			/// </summary>
			// Token: 0x040048BF RID: 18623
			public static readonly StringName _spawnTween = "_spawnTween";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x040048C0 RID: 18624
			public static readonly StringName _hoverTween = "_hoverTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012AF RID: 4783
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
