using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Back Button. The Man. The Myth. The Legend.
	/// </summary>
	// Token: 0x020003D2 RID: 978
	[ScriptPath("res://src/Core/Nodes/CommonUi/NBackButton.cs")]
	public class NBackButton : NButton
	{
		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06003B2C RID: 15148 RVA: 0x001C3B65 File Offset: 0x001C1D65
		[Nullable(1)]
		protected override string ClickedSfx
		{
			[NullableContext(1)]
			get
			{
				return "event:/sfx/ui/clicks/ui_back";
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06003B2D RID: 15149 RVA: 0x001C3B6C File Offset: 0x001C1D6C
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[]
				{
					MegaInput.cancel,
					MegaInput.pauseAndBack,
					MegaInput.back
				};
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06003B2E RID: 15150 RVA: 0x001C3B9B File Offset: 0x001C1D9B
		[Nullable(1)]
		protected override string ControllerIconHotkey
		{
			[NullableContext(1)]
			get
			{
				return MegaInput.cancel;
			}
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x001C3BA8 File Offset: 0x001C1DA8
		public override void _Ready()
		{
			this.ConnectSignals();
			this._outline = base.GetNode<Control>("Outline");
			this._buttonImage = base.GetNode<Control>("Image");
			this._isEnabled = false;
			this._posOffset = new Vector2(base.OffsetLeft + 80f, -base.OffsetBottom + 110f);
			base.GetTree().Root.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
			this.OnWindowChange();
			this.OnDisable();
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x001C3C48 File Offset: 0x001C1E48
		private void OnWindowChange()
		{
			this._showPos = new Vector2(0f, (float)base.GetWindow().ContentScaleSize.Y) - this._posOffset;
			this._hidePos = this._showPos + NBackButton._hideOffset;
			base.Position = (this._isEnabled ? this._showPos : this._hidePos);
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x001C3CB3 File Offset: 0x001C1EB3
		public void MoveToHidePosition()
		{
			base.GlobalPosition = this._hidePos;
		}

		/// <summary>
		/// Call when we want this button to animate in.
		/// We refresh a lot of values here as the screen may not have been disposed and we reuse the button.
		/// May need to separate this logic into a Refresh() call for organization.
		/// </summary>
		// Token: 0x06003B32 RID: 15154 RVA: 0x001C3CC4 File Offset: 0x001C1EC4
		protected override void OnEnable()
		{
			base.OnEnable();
			this._isEnabled = true;
			this._outline.Modulate = Colors.Transparent;
			this._buttonImage.Modulate = Colors.White;
			base.Scale = Vector2.One;
			Tween moveTween = this._moveTween;
			if (moveTween != null)
			{
				moveTween.Kill();
			}
			this._moveTween = base.CreateTween();
			this._moveTween.TweenProperty(this, "global_position", this._showPos, 0.35).SetEase(1L).SetTrans(10L);
		}

		/// <summary>
		/// Call when we want this button to hide this button (Disables clickability/hotkeys)
		/// </summary>
		// Token: 0x06003B33 RID: 15155 RVA: 0x001C3D60 File Offset: 0x001C1F60
		protected override void OnDisable()
		{
			base.OnDisable();
			this._isEnabled = false;
			Tween moveTween = this._moveTween;
			if (moveTween != null)
			{
				moveTween.Kill();
			}
			this._moveTween = base.CreateTween();
			this._moveTween.TweenProperty(this, "global_position", this._hidePos, 0.35).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x001C3DD0 File Offset: 0x001C1FD0
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", NBackButton._hoverScale, 0.05);
			this._hoverTween.TweenProperty(this._outline, "modulate", this._outlineColor, 0.05);
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x001C3E60 File Offset: 0x001C2060
		protected override void OnUnfocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", NBackButton._hoverScale, 0.5).SetTrans(5L).SetEase(1L);
			this._hoverTween.TweenProperty(this._outline, "modulate", this._outlineTransparentColor, 0.5);
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x001C3EF8 File Offset: 0x001C20F8
		protected override void OnPress()
		{
			base.OnPress();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this, "scale", NBackButton._downScale, 0.25).SetTrans(5L).SetEase(1L);
			this._hoverTween.TweenProperty(this._buttonImage, "modulate", this._downColor, 0.25).SetTrans(7L).SetEase(1L);
			this._hoverTween.TweenProperty(this._outline, "modulate", this._outlineTransparentColor, 0.25).SetTrans(7L).SetEase(1L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B37 RID: 15159 RVA: 0x001C3FE4 File Offset: 0x001C21E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NBackButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBackButton.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBackButton.MethodName.MoveToHidePosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBackButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBackButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBackButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBackButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBackButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B38 RID: 15160 RVA: 0x001C4174 File Offset: 0x001C2374
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBackButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBackButton.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBackButton.MethodName.MoveToHidePosition && args.Count == 0)
			{
				this.MoveToHidePosition();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBackButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBackButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBackButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBackButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBackButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003B39 RID: 15161 RVA: 0x001C42B4 File Offset: 0x001C24B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBackButton.MethodName._Ready || (ref method) == NBackButton.MethodName.OnWindowChange || (ref method) == NBackButton.MethodName.MoveToHidePosition || (ref method) == NBackButton.MethodName.OnEnable || (ref method) == NBackButton.MethodName.OnDisable || (ref method) == NBackButton.MethodName.OnFocus || (ref method) == NBackButton.MethodName.OnUnfocus || (ref method) == NBackButton.MethodName.OnPress || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003B3A RID: 15162 RVA: 0x001C4340 File Offset: 0x001C2540
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBackButton.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._buttonImage)
			{
				this._buttonImage = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._defaultOutlineColor)
			{
				this._defaultOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._hoveredOutlineColor)
			{
				this._hoveredOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._downColor)
			{
				this._downColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._outlineColor)
			{
				this._outlineColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._outlineTransparentColor)
			{
				this._outlineTransparentColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._posOffset)
			{
				this._posOffset = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._showPos)
			{
				this._showPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._hidePos)
			{
				this._hidePos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._moveTween)
			{
				this._moveTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003B3B RID: 15163 RVA: 0x001C449C File Offset: 0x001C269C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBackButton.PropertyName.ClickedSfx)
			{
				string text = this.ClickedSfx;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName.ControllerIconHotkey)
			{
				string text = this.ControllerIconHotkey;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outline);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._buttonImage)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._buttonImage);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._defaultOutlineColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._defaultOutlineColor);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._hoveredOutlineColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._hoveredOutlineColor);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._downColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._downColor);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._outlineColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._outlineColor);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._outlineTransparentColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._outlineTransparentColor);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._posOffset)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._posOffset);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._showPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._showPos);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._hidePos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._hidePos);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NBackButton.PropertyName._moveTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._moveTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B3C RID: 15164 RVA: 0x001C469C File Offset: 0x001C289C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NBackButton.PropertyName.ClickedSfx, 0L, "", 4096L, false),
				new PropertyInfo(34L, NBackButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(4L, NBackButton.PropertyName.ControllerIconHotkey, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBackButton.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBackButton.PropertyName._buttonImage, 0L, "", 4096L, false),
				new PropertyInfo(20L, NBackButton.PropertyName._defaultOutlineColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NBackButton.PropertyName._hoveredOutlineColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NBackButton.PropertyName._downColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NBackButton.PropertyName._outlineColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NBackButton.PropertyName._outlineTransparentColor, 0L, "", 4096L, false),
				new PropertyInfo(5L, NBackButton.PropertyName._posOffset, 0L, "", 4096L, false),
				new PropertyInfo(5L, NBackButton.PropertyName._showPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NBackButton.PropertyName._hidePos, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBackButton.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBackButton.PropertyName._moveTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B3D RID: 15165 RVA: 0x001C489C File Offset: 0x001C2A9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBackButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
			info.AddProperty(NBackButton.PropertyName._buttonImage, Variant.From<Control>(ref this._buttonImage));
			info.AddProperty(NBackButton.PropertyName._defaultOutlineColor, Variant.From<Color>(ref this._defaultOutlineColor));
			info.AddProperty(NBackButton.PropertyName._hoveredOutlineColor, Variant.From<Color>(ref this._hoveredOutlineColor));
			info.AddProperty(NBackButton.PropertyName._downColor, Variant.From<Color>(ref this._downColor));
			info.AddProperty(NBackButton.PropertyName._outlineColor, Variant.From<Color>(ref this._outlineColor));
			info.AddProperty(NBackButton.PropertyName._outlineTransparentColor, Variant.From<Color>(ref this._outlineTransparentColor));
			info.AddProperty(NBackButton.PropertyName._posOffset, Variant.From<Vector2>(ref this._posOffset));
			info.AddProperty(NBackButton.PropertyName._showPos, Variant.From<Vector2>(ref this._showPos));
			info.AddProperty(NBackButton.PropertyName._hidePos, Variant.From<Vector2>(ref this._hidePos));
			info.AddProperty(NBackButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NBackButton.PropertyName._moveTween, Variant.From<Tween>(ref this._moveTween));
		}

		/// <inheritdoc />
		// Token: 0x06003B3E RID: 15166 RVA: 0x001C49B8 File Offset: 0x001C2BB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBackButton.PropertyName._outline, ref variant))
			{
				this._outline = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBackButton.PropertyName._buttonImage, ref variant2))
			{
				this._buttonImage = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBackButton.PropertyName._defaultOutlineColor, ref variant3))
			{
				this._defaultOutlineColor = variant3.As<Color>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBackButton.PropertyName._hoveredOutlineColor, ref variant4))
			{
				this._hoveredOutlineColor = variant4.As<Color>();
			}
			Variant variant5;
			if (info.TryGetProperty(NBackButton.PropertyName._downColor, ref variant5))
			{
				this._downColor = variant5.As<Color>();
			}
			Variant variant6;
			if (info.TryGetProperty(NBackButton.PropertyName._outlineColor, ref variant6))
			{
				this._outlineColor = variant6.As<Color>();
			}
			Variant variant7;
			if (info.TryGetProperty(NBackButton.PropertyName._outlineTransparentColor, ref variant7))
			{
				this._outlineTransparentColor = variant7.As<Color>();
			}
			Variant variant8;
			if (info.TryGetProperty(NBackButton.PropertyName._posOffset, ref variant8))
			{
				this._posOffset = variant8.As<Vector2>();
			}
			Variant variant9;
			if (info.TryGetProperty(NBackButton.PropertyName._showPos, ref variant9))
			{
				this._showPos = variant9.As<Vector2>();
			}
			Variant variant10;
			if (info.TryGetProperty(NBackButton.PropertyName._hidePos, ref variant10))
			{
				this._hidePos = variant10.As<Vector2>();
			}
			Variant variant11;
			if (info.TryGetProperty(NBackButton.PropertyName._hoverTween, ref variant11))
			{
				this._hoverTween = variant11.As<Tween>();
			}
			Variant variant12;
			if (info.TryGetProperty(NBackButton.PropertyName._moveTween, ref variant12))
			{
				this._moveTween = variant12.As<Tween>();
			}
		}

		// Token: 0x04001711 RID: 5905
		[Nullable(1)]
		private Control _outline;

		// Token: 0x04001712 RID: 5906
		[Nullable(1)]
		private Control _buttonImage;

		// Token: 0x04001713 RID: 5907
		private Color _defaultOutlineColor = StsColors.cream;

		// Token: 0x04001714 RID: 5908
		private Color _hoveredOutlineColor = StsColors.gold;

		// Token: 0x04001715 RID: 5909
		private Color _downColor = Colors.Gray;

		// Token: 0x04001716 RID: 5910
		private Color _outlineColor = new Color("F0B400");

		// Token: 0x04001717 RID: 5911
		private Color _outlineTransparentColor = new Color("FF000000");

		// Token: 0x04001718 RID: 5912
		private static readonly Vector2 _hoverScale = Vector2.One * 1.05f;

		// Token: 0x04001719 RID: 5913
		private static readonly Vector2 _downScale = Vector2.One;

		// Token: 0x0400171A RID: 5914
		private const double _animInOutDur = 0.35;

		// Token: 0x0400171B RID: 5915
		private Vector2 _posOffset;

		// Token: 0x0400171C RID: 5916
		private Vector2 _showPos;

		// Token: 0x0400171D RID: 5917
		private Vector2 _hidePos;

		// Token: 0x0400171E RID: 5918
		private static readonly Vector2 _hideOffset = new Vector2(-180f, 0f);

		// Token: 0x0400171F RID: 5919
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x04001720 RID: 5920
		[Nullable(2)]
		private Tween _moveTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200181C RID: 6172
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005E43 RID: 24131
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04005E44 RID: 24132
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'MoveToHidePosition' method.
			/// </summary>
			// Token: 0x04005E45 RID: 24133
			public static readonly StringName MoveToHidePosition = "MoveToHidePosition";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04005E46 RID: 24134
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04005E47 RID: 24135
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005E48 RID: 24136
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005E49 RID: 24137
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005E4A RID: 24138
			public new static readonly StringName OnPress = "OnPress";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200181D RID: 6173
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ClickedSfx' property.
			/// </summary>
			// Token: 0x04005E4B RID: 24139
			public new static readonly StringName ClickedSfx = "ClickedSfx";

			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005E4C RID: 24140
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the 'ControllerIconHotkey' property.
			/// </summary>
			// Token: 0x04005E4D RID: 24141
			public new static readonly StringName ControllerIconHotkey = "ControllerIconHotkey";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04005E4E RID: 24142
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_buttonImage' field.
			/// </summary>
			// Token: 0x04005E4F RID: 24143
			public static readonly StringName _buttonImage = "_buttonImage";

			/// <summary>
			/// Cached name for the '_defaultOutlineColor' field.
			/// </summary>
			// Token: 0x04005E50 RID: 24144
			public static readonly StringName _defaultOutlineColor = "_defaultOutlineColor";

			/// <summary>
			/// Cached name for the '_hoveredOutlineColor' field.
			/// </summary>
			// Token: 0x04005E51 RID: 24145
			public static readonly StringName _hoveredOutlineColor = "_hoveredOutlineColor";

			/// <summary>
			/// Cached name for the '_downColor' field.
			/// </summary>
			// Token: 0x04005E52 RID: 24146
			public static readonly StringName _downColor = "_downColor";

			/// <summary>
			/// Cached name for the '_outlineColor' field.
			/// </summary>
			// Token: 0x04005E53 RID: 24147
			public static readonly StringName _outlineColor = "_outlineColor";

			/// <summary>
			/// Cached name for the '_outlineTransparentColor' field.
			/// </summary>
			// Token: 0x04005E54 RID: 24148
			public static readonly StringName _outlineTransparentColor = "_outlineTransparentColor";

			/// <summary>
			/// Cached name for the '_posOffset' field.
			/// </summary>
			// Token: 0x04005E55 RID: 24149
			public static readonly StringName _posOffset = "_posOffset";

			/// <summary>
			/// Cached name for the '_showPos' field.
			/// </summary>
			// Token: 0x04005E56 RID: 24150
			public static readonly StringName _showPos = "_showPos";

			/// <summary>
			/// Cached name for the '_hidePos' field.
			/// </summary>
			// Token: 0x04005E57 RID: 24151
			public static readonly StringName _hidePos = "_hidePos";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04005E58 RID: 24152
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_moveTween' field.
			/// </summary>
			// Token: 0x04005E59 RID: 24153
			public static readonly StringName _moveTween = "_moveTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200181E RID: 6174
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
