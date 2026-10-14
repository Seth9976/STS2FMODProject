using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Proceed Button. The Man. The Myth. The Legend. A Button.
	/// </summary>
	// Token: 0x020003E9 RID: 1001
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NProceedButton.cs")]
	public class NProceedButton : NButton
	{
		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06003D01 RID: 15617 RVA: 0x001D3327 File Offset: 0x001D1527
		public static LocString ProceedLoc
		{
			get
			{
				return new LocString("gameplay_ui", "PROCEED_BUTTON");
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06003D02 RID: 15618 RVA: 0x001D3338 File Offset: 0x001D1538
		public static LocString SkipLoc
		{
			get
			{
				return new LocString("gameplay_ui", "CHOOSE_CARD_SKIP_BUTTON");
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06003D03 RID: 15619 RVA: 0x001D3349 File Offset: 0x001D1549
		// (set) Token: 0x06003D04 RID: 15620 RVA: 0x001D3351 File Offset: 0x001D1551
		public bool IsSkip { get; private set; }

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06003D05 RID: 15621 RVA: 0x001D335A File Offset: 0x001D155A
		protected override string[] Hotkeys
		{
			get
			{
				return new string[] { MegaInput.accept };
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06003D06 RID: 15622 RVA: 0x001D3370 File Offset: 0x001D1570
		private Vector2 ShowPos
		{
			get
			{
				return NProceedButton._showPosRatio * this._viewport.GetVisibleRect().Size;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06003D07 RID: 15623 RVA: 0x001D339C File Offset: 0x001D159C
		private Vector2 HidePos
		{
			get
			{
				return NProceedButton._hidePosRatio * this._viewport.GetVisibleRect().Size;
			}
		}

		// Token: 0x06003D08 RID: 15624 RVA: 0x001D33C8 File Offset: 0x001D15C8
		public override void _Ready()
		{
			this.ConnectSignals();
			this._outline = base.GetNode<Control>("%Outline");
			this._buttonImage = base.GetNode<Control>("%Image");
			this._hsv = (ShaderMaterial)this._buttonImage.GetMaterial();
			this._label = base.GetNode<MegaLabel>("%Label");
			this._viewport = base.GetViewport();
			base.Position = this.HidePos;
			base.Disable();
			NGame.Instance.DebugToggleProceedButton += this.DebugToggleVisibility;
		}

		// Token: 0x06003D09 RID: 15625 RVA: 0x001D3467 File Offset: 0x001D1667
		public override void _ExitTree()
		{
			base._ExitTree();
			NGame.Instance.DebugToggleProceedButton -= this.DebugToggleVisibility;
		}

		/// <summary>
		/// Call when we want this button to animate in.
		/// We refresh a lot of values here as the screen may not have been disposed and we reuse the button.
		/// May need to separate this logic into a Refresh() call for organization.
		/// </summary>
		// Token: 0x06003D0A RID: 15626 RVA: 0x001D3488 File Offset: 0x001D1688
		protected override void OnEnable()
		{
			base.OnEnable();
			if (NGame.IsDebugHidingProceedButton)
			{
				base.Modulate = Colors.Transparent;
			}
			base.Scale = Vector2.One;
			this._buttonImage.SelfModulate = StsColors.gray;
			this.UpdateShaderS(1f);
			this.UpdateShaderV(1f);
			Tween animTween = this._animTween;
			if (animTween != null)
			{
				animTween.Kill();
			}
			this._animTween = base.CreateTween().SetParallel(true);
			this._animTween.TweenProperty(this, "position", this.ShowPos, 0.5).SetEase(1L).SetTrans(5L);
			this._animTween.TweenProperty(this._buttonImage, "self_modulate", Colors.White, 0.5);
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this._outline, "modulate", Colors.White, 0.5);
			if (this._shouldPulse)
			{
				this.StartGlowTween();
			}
		}

		/// <summary>
		/// Call when we want this button to hide this button (Disables clickability/hotkeys)
		/// </summary>
		// Token: 0x06003D0B RID: 15627 RVA: 0x001D35CC File Offset: 0x001D17CC
		protected override void OnDisable()
		{
			base.OnDisable();
			Tween animTween = this._animTween;
			if (animTween != null)
			{
				animTween.Kill();
			}
			this._animTween = base.CreateTween().SetParallel(true);
			this._animTween.TweenProperty(this, "position", this.HidePos, 0.8).SetEase(1L).SetTrans(5L);
			this._animTween.TweenProperty(this._buttonImage, "self_modulate", StsColors.gray, 0.8);
			this._animTween.TweenProperty(this._outline, "modulate", StsColors.transparentWhite, 0.8);
			Tween glowTween = this._glowTween;
			if (glowTween == null)
			{
				return;
			}
			glowTween.Kill();
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x001D36AC File Offset: 0x001D18AC
		protected override void OnRelease()
		{
			if (!this._isEnabled)
			{
				return;
			}
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._buttonImage, "modulate", Colors.White, 0.5);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProceedButton._v), 1f, 0.05);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NProceedButton._s), 1f, 0.05);
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x001D37CC File Offset: 0x001D19CC
		protected override void OnFocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", NProceedButton._hoverScale, 0.05);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProceedButton._v), 1.4f, 0.05);
			Tween glowTween = this._glowTween;
			if (glowTween != null)
			{
				glowTween.Kill();
			}
			this._glowTween = base.CreateTween();
			this._glowTween.TweenProperty(this._outline, "self_modulate:a", 1f, 0.05);
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x001D38B4 File Offset: 0x001D1AB4
		protected override void OnUnfocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._buttonImage, "modulate", Colors.White, 0.5);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProceedButton._v), 1f, 0.05);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NProceedButton._s), 1f, 0.05);
			if (this._shouldPulse)
			{
				this.StartGlowTween();
				return;
			}
			this.StopGlowTween();
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x001D39E0 File Offset: 0x001D1BE0
		protected override void OnPress()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", Vector2.One * 0.95f, 0.5).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._buttonImage, "modulate", StsColors.gray, 0.5);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProceedButton._v), 1f, 0.25);
			this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NProceedButton._s), 0.8f, 0.25);
			this.StopGlowTween();
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x001D3B07 File Offset: 0x001D1D07
		public void UpdateText(LocString loc)
		{
			this._label.SetTextAutoSize(loc.GetFormattedText());
			this.IsSkip = loc.LocEntryKey == NProceedButton.SkipLoc.LocEntryKey;
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x001D3B35 File Offset: 0x001D1D35
		private void DebugToggleVisibility()
		{
			base.Modulate = (NGame.IsDebugHidingProceedButton ? Colors.Transparent : Colors.White);
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x001D3B50 File Offset: 0x001D1D50
		public void SetPulseState(bool isPulsing)
		{
			this._shouldPulse = isPulsing;
			if (isPulsing)
			{
				this.StartGlowTween();
				return;
			}
			this.StopGlowTween();
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x001D3B6C File Offset: 0x001D1D6C
		private void StartGlowTween()
		{
			Tween glowTween = this._glowTween;
			if (glowTween != null)
			{
				glowTween.Kill();
			}
			this._glowTween = base.CreateTween().SetLoops(0);
			this._glowTween.TweenProperty(this._outline, "self_modulate:a", 0.25f, 0.5);
			this._glowTween.TweenProperty(this._outline, "self_modulate:a", 0.75f, 0.5);
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x001D3BFC File Offset: 0x001D1DFC
		private void StopGlowTween()
		{
			Tween glowTween = this._glowTween;
			if (glowTween != null)
			{
				glowTween.Kill();
			}
			this._glowTween = base.CreateTween();
			this._glowTween.TweenProperty(this._outline, "self_modulate:a", 0f, 0.5);
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x001D3C55 File Offset: 0x001D1E55
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NProceedButton._s, value);
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x001D3C6D File Offset: 0x001D1E6D
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NProceedButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D17 RID: 15639 RVA: 0x001D3C88 File Offset: 0x001D1E88
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NProceedButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProceedButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProceedButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProceedButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProceedButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProceedButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProceedButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProceedButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProceedButton.MethodName.DebugToggleVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProceedButton.MethodName.SetPulseState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isPulsing", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NProceedButton.MethodName.StartGlowTween, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProceedButton.MethodName.StopGlowTween, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NProceedButton.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NProceedButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D18 RID: 15640 RVA: 0x001D3FA0 File Offset: 0x001D21A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NProceedButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.DebugToggleVisibility && args.Count == 0)
			{
				this.DebugToggleVisibility();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.SetPulseState && args.Count == 1)
			{
				this.SetPulseState(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.StartGlowTween && args.Count == 0)
			{
				this.StartGlowTween();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.StopGlowTween && args.Count == 0)
			{
				this.StopGlowTween();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NProceedButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003D19 RID: 15641 RVA: 0x001D41E8 File Offset: 0x001D23E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NProceedButton.MethodName._Ready || (ref method) == NProceedButton.MethodName._ExitTree || (ref method) == NProceedButton.MethodName.OnEnable || (ref method) == NProceedButton.MethodName.OnDisable || (ref method) == NProceedButton.MethodName.OnRelease || (ref method) == NProceedButton.MethodName.OnFocus || (ref method) == NProceedButton.MethodName.OnUnfocus || (ref method) == NProceedButton.MethodName.OnPress || (ref method) == NProceedButton.MethodName.DebugToggleVisibility || (ref method) == NProceedButton.MethodName.SetPulseState || (ref method) == NProceedButton.MethodName.StartGlowTween || (ref method) == NProceedButton.MethodName.StopGlowTween || (ref method) == NProceedButton.MethodName.UpdateShaderS || (ref method) == NProceedButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003D1A RID: 15642 RVA: 0x001D42D0 File Offset: 0x001D24D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NProceedButton.PropertyName.IsSkip)
			{
				this.IsSkip = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._buttonImage)
			{
				this._buttonImage = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._viewport)
			{
				this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._defaultOutlineColor)
			{
				this._defaultOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._hoveredOutlineColor)
			{
				this._hoveredOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._downColor)
			{
				this._downColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._outlineColor)
			{
				this._outlineColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._outlineTransparentColor)
			{
				this._outlineTransparentColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._animTween)
			{
				this._animTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._glowTween)
			{
				this._glowTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._elapsedTime)
			{
				this._elapsedTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._shouldPulse)
			{
				this._shouldPulse = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003D1B RID: 15643 RVA: 0x001D4498 File Offset: 0x001D2698
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NProceedButton.PropertyName.IsSkip)
			{
				bool isSkip = this.IsSkip;
				value = VariantUtils.CreateFrom<bool>(ref isSkip);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName.ShowPos)
			{
				Vector2 vector = this.ShowPos;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName.HidePos)
			{
				Vector2 vector = this.HidePos;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outline);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._buttonImage)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._buttonImage);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._viewport)
			{
				value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._defaultOutlineColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._defaultOutlineColor);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._hoveredOutlineColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._hoveredOutlineColor);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._downColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._downColor);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._outlineColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._outlineColor);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._outlineTransparentColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._outlineTransparentColor);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._animTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animTween);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._glowTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._glowTween);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._elapsedTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._elapsedTime);
				return true;
			}
			if ((ref name) == NProceedButton.PropertyName._shouldPulse)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._shouldPulse);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D1C RID: 15644 RVA: 0x001D471C File Offset: 0x001D291C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NProceedButton.PropertyName.IsSkip, 0L, "", 4096L, false),
				new PropertyInfo(34L, NProceedButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProceedButton.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProceedButton.PropertyName._buttonImage, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProceedButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProceedButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProceedButton.PropertyName._viewport, 0L, "", 4096L, false),
				new PropertyInfo(20L, NProceedButton.PropertyName._defaultOutlineColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NProceedButton.PropertyName._hoveredOutlineColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NProceedButton.PropertyName._downColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NProceedButton.PropertyName._outlineColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NProceedButton.PropertyName._outlineTransparentColor, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProceedButton.PropertyName._animTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProceedButton.PropertyName._glowTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NProceedButton.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(3L, NProceedButton.PropertyName._elapsedTime, 0L, "", 4096L, false),
				new PropertyInfo(1L, NProceedButton.PropertyName._shouldPulse, 0L, "", 4096L, false),
				new PropertyInfo(5L, NProceedButton.PropertyName.ShowPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NProceedButton.PropertyName.HidePos, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D1D RID: 15645 RVA: 0x001D49A0 File Offset: 0x001D2BA0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isSkip = NProceedButton.PropertyName.IsSkip;
			bool isSkip2 = this.IsSkip;
			info.AddProperty(isSkip, Variant.From<bool>(ref isSkip2));
			info.AddProperty(NProceedButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
			info.AddProperty(NProceedButton.PropertyName._buttonImage, Variant.From<Control>(ref this._buttonImage));
			info.AddProperty(NProceedButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NProceedButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NProceedButton.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
			info.AddProperty(NProceedButton.PropertyName._defaultOutlineColor, Variant.From<Color>(ref this._defaultOutlineColor));
			info.AddProperty(NProceedButton.PropertyName._hoveredOutlineColor, Variant.From<Color>(ref this._hoveredOutlineColor));
			info.AddProperty(NProceedButton.PropertyName._downColor, Variant.From<Color>(ref this._downColor));
			info.AddProperty(NProceedButton.PropertyName._outlineColor, Variant.From<Color>(ref this._outlineColor));
			info.AddProperty(NProceedButton.PropertyName._outlineTransparentColor, Variant.From<Color>(ref this._outlineTransparentColor));
			info.AddProperty(NProceedButton.PropertyName._animTween, Variant.From<Tween>(ref this._animTween));
			info.AddProperty(NProceedButton.PropertyName._glowTween, Variant.From<Tween>(ref this._glowTween));
			info.AddProperty(NProceedButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NProceedButton.PropertyName._elapsedTime, Variant.From<float>(ref this._elapsedTime));
			info.AddProperty(NProceedButton.PropertyName._shouldPulse, Variant.From<bool>(ref this._shouldPulse));
		}

		/// <inheritdoc />
		// Token: 0x06003D1E RID: 15646 RVA: 0x001D4B18 File Offset: 0x001D2D18
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NProceedButton.PropertyName.IsSkip, ref variant))
			{
				this.IsSkip = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NProceedButton.PropertyName._outline, ref variant2))
			{
				this._outline = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NProceedButton.PropertyName._buttonImage, ref variant3))
			{
				this._buttonImage = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NProceedButton.PropertyName._label, ref variant4))
			{
				this._label = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NProceedButton.PropertyName._hsv, ref variant5))
			{
				this._hsv = variant5.As<ShaderMaterial>();
			}
			Variant variant6;
			if (info.TryGetProperty(NProceedButton.PropertyName._viewport, ref variant6))
			{
				this._viewport = variant6.As<Viewport>();
			}
			Variant variant7;
			if (info.TryGetProperty(NProceedButton.PropertyName._defaultOutlineColor, ref variant7))
			{
				this._defaultOutlineColor = variant7.As<Color>();
			}
			Variant variant8;
			if (info.TryGetProperty(NProceedButton.PropertyName._hoveredOutlineColor, ref variant8))
			{
				this._hoveredOutlineColor = variant8.As<Color>();
			}
			Variant variant9;
			if (info.TryGetProperty(NProceedButton.PropertyName._downColor, ref variant9))
			{
				this._downColor = variant9.As<Color>();
			}
			Variant variant10;
			if (info.TryGetProperty(NProceedButton.PropertyName._outlineColor, ref variant10))
			{
				this._outlineColor = variant10.As<Color>();
			}
			Variant variant11;
			if (info.TryGetProperty(NProceedButton.PropertyName._outlineTransparentColor, ref variant11))
			{
				this._outlineTransparentColor = variant11.As<Color>();
			}
			Variant variant12;
			if (info.TryGetProperty(NProceedButton.PropertyName._animTween, ref variant12))
			{
				this._animTween = variant12.As<Tween>();
			}
			Variant variant13;
			if (info.TryGetProperty(NProceedButton.PropertyName._glowTween, ref variant13))
			{
				this._glowTween = variant13.As<Tween>();
			}
			Variant variant14;
			if (info.TryGetProperty(NProceedButton.PropertyName._hoverTween, ref variant14))
			{
				this._hoverTween = variant14.As<Tween>();
			}
			Variant variant15;
			if (info.TryGetProperty(NProceedButton.PropertyName._elapsedTime, ref variant15))
			{
				this._elapsedTime = variant15.As<float>();
			}
			Variant variant16;
			if (info.TryGetProperty(NProceedButton.PropertyName._shouldPulse, ref variant16))
			{
				this._shouldPulse = variant16.As<bool>();
			}
		}

		// Token: 0x040017CB RID: 6091
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040017CC RID: 6092
		private static readonly StringName _s = new StringName("s");

		// Token: 0x040017CE RID: 6094
		private Control _outline;

		// Token: 0x040017CF RID: 6095
		private Control _buttonImage;

		// Token: 0x040017D0 RID: 6096
		private MegaLabel _label;

		// Token: 0x040017D1 RID: 6097
		private ShaderMaterial _hsv;

		// Token: 0x040017D2 RID: 6098
		private Viewport _viewport;

		// Token: 0x040017D3 RID: 6099
		private Color _defaultOutlineColor = StsColors.cream;

		// Token: 0x040017D4 RID: 6100
		private Color _hoveredOutlineColor = StsColors.gold;

		// Token: 0x040017D5 RID: 6101
		private Color _downColor = Colors.Gray;

		// Token: 0x040017D6 RID: 6102
		private Color _outlineColor = new Color("FFCC00C0");

		// Token: 0x040017D7 RID: 6103
		private Color _outlineTransparentColor = new Color("FF000000");

		// Token: 0x040017D8 RID: 6104
		[Nullable(2)]
		private Tween _animTween;

		// Token: 0x040017D9 RID: 6105
		[Nullable(2)]
		private Tween _glowTween;

		// Token: 0x040017DA RID: 6106
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x040017DB RID: 6107
		private static readonly Vector2 _hoverScale = Vector2.One * 1.05f;

		// Token: 0x040017DC RID: 6108
		private float _elapsedTime;

		// Token: 0x040017DD RID: 6109
		private bool _shouldPulse = true;

		// Token: 0x040017DE RID: 6110
		private static readonly Vector2 _showPosRatio = new Vector2(1583f, 764f) / NGame.devResolution;

		// Token: 0x040017DF RID: 6111
		private static readonly Vector2 _hidePosRatio = NProceedButton._showPosRatio + new Vector2(400f, 0f) / NGame.devResolution;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200187B RID: 6267
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005FAC RID: 24492
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005FAD RID: 24493
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04005FAE RID: 24494
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04005FAF RID: 24495
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005FB0 RID: 24496
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005FB1 RID: 24497
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005FB2 RID: 24498
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005FB3 RID: 24499
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'DebugToggleVisibility' method.
			/// </summary>
			// Token: 0x04005FB4 RID: 24500
			public static readonly StringName DebugToggleVisibility = "DebugToggleVisibility";

			/// <summary>
			/// Cached name for the 'SetPulseState' method.
			/// </summary>
			// Token: 0x04005FB5 RID: 24501
			public static readonly StringName SetPulseState = "SetPulseState";

			/// <summary>
			/// Cached name for the 'StartGlowTween' method.
			/// </summary>
			// Token: 0x04005FB6 RID: 24502
			public static readonly StringName StartGlowTween = "StartGlowTween";

			/// <summary>
			/// Cached name for the 'StopGlowTween' method.
			/// </summary>
			// Token: 0x04005FB7 RID: 24503
			public static readonly StringName StopGlowTween = "StopGlowTween";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x04005FB8 RID: 24504
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04005FB9 RID: 24505
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200187C RID: 6268
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsSkip' property.
			/// </summary>
			// Token: 0x04005FBA RID: 24506
			public static readonly StringName IsSkip = "IsSkip";

			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005FBB RID: 24507
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the 'ShowPos' property.
			/// </summary>
			// Token: 0x04005FBC RID: 24508
			public static readonly StringName ShowPos = "ShowPos";

			/// <summary>
			/// Cached name for the 'HidePos' property.
			/// </summary>
			// Token: 0x04005FBD RID: 24509
			public static readonly StringName HidePos = "HidePos";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04005FBE RID: 24510
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_buttonImage' field.
			/// </summary>
			// Token: 0x04005FBF RID: 24511
			public static readonly StringName _buttonImage = "_buttonImage";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005FC0 RID: 24512
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005FC1 RID: 24513
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_viewport' field.
			/// </summary>
			// Token: 0x04005FC2 RID: 24514
			public static readonly StringName _viewport = "_viewport";

			/// <summary>
			/// Cached name for the '_defaultOutlineColor' field.
			/// </summary>
			// Token: 0x04005FC3 RID: 24515
			public static readonly StringName _defaultOutlineColor = "_defaultOutlineColor";

			/// <summary>
			/// Cached name for the '_hoveredOutlineColor' field.
			/// </summary>
			// Token: 0x04005FC4 RID: 24516
			public static readonly StringName _hoveredOutlineColor = "_hoveredOutlineColor";

			/// <summary>
			/// Cached name for the '_downColor' field.
			/// </summary>
			// Token: 0x04005FC5 RID: 24517
			public static readonly StringName _downColor = "_downColor";

			/// <summary>
			/// Cached name for the '_outlineColor' field.
			/// </summary>
			// Token: 0x04005FC6 RID: 24518
			public static readonly StringName _outlineColor = "_outlineColor";

			/// <summary>
			/// Cached name for the '_outlineTransparentColor' field.
			/// </summary>
			// Token: 0x04005FC7 RID: 24519
			public static readonly StringName _outlineTransparentColor = "_outlineTransparentColor";

			/// <summary>
			/// Cached name for the '_animTween' field.
			/// </summary>
			// Token: 0x04005FC8 RID: 24520
			public static readonly StringName _animTween = "_animTween";

			/// <summary>
			/// Cached name for the '_glowTween' field.
			/// </summary>
			// Token: 0x04005FC9 RID: 24521
			public static readonly StringName _glowTween = "_glowTween";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04005FCA RID: 24522
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_elapsedTime' field.
			/// </summary>
			// Token: 0x04005FCB RID: 24523
			public static readonly StringName _elapsedTime = "_elapsedTime";

			/// <summary>
			/// Cached name for the '_shouldPulse' field.
			/// </summary>
			// Token: 0x04005FCC RID: 24524
			public static readonly StringName _shouldPulse = "_shouldPulse";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200187D RID: 6269
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
