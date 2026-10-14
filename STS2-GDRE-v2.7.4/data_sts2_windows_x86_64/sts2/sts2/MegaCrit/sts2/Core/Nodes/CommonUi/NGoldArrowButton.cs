using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Arrow buttons are used for toggling left and right on several screens. The Man. The Myth. The Legend.
	/// </summary>
	// Token: 0x020003DF RID: 991
	[ScriptPath("res://src/Core/Nodes/CommonUi/NGoldArrowButton.cs")]
	public class NGoldArrowButton : NButton
	{
		// Token: 0x06003C54 RID: 15444 RVA: 0x001CDA78 File Offset: 0x001CBC78
		public override void _Ready()
		{
			this.ConnectSignals();
			this._icon = base.GetNode<TextureRect>("TextureRect");
			this._hsv = (ShaderMaterial)this._icon.Material;
			this._hsv.SetShaderParameter(NGoldArrowButton._v, this._valueDefault);
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x001CDAD4 File Offset: 0x001CBCD4
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween animTween = this._animTween;
			if (animTween != null)
			{
				animTween.Kill();
			}
			this._hsv.SetShaderParameter(NGoldArrowButton._v, this._valueHovered);
			this._icon.Scale = this._hoverScale;
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x001CDB24 File Offset: 0x001CBD24
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween animTween = this._animTween;
			if (animTween != null)
			{
				animTween.Kill();
			}
			this._animTween = base.CreateTween().SetParallel(true);
			this._animTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._valueHovered, this._valueDefault, 1.0).SetEase(1L).SetTrans(5L);
			this._animTween.TweenProperty(this._icon, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x001CDBE4 File Offset: 0x001CBDE4
		protected override void OnPress()
		{
			base.OnPress();
			Tween animTween = this._animTween;
			if (animTween != null)
			{
				animTween.Kill();
			}
			this._animTween = base.CreateTween();
			this._hsv.SetShaderParameter(NGoldArrowButton._v, 0.7f);
			this._animTween.TweenProperty(this._icon, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x001CDC6C File Offset: 0x001CBE6C
		protected override void OnRelease()
		{
			if (base.IsFocused)
			{
				Tween animTween = this._animTween;
				if (animTween != null)
				{
					animTween.Kill();
				}
				this._hsv.SetShaderParameter(NGoldArrowButton._v, this._valueHovered);
				this._icon.Scale = this._hoverScale;
			}
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x001CDCBE File Offset: 0x001CBEBE
		private void UpdateShaderParam(float newV)
		{
			this._hsv.SetShaderParameter(NGoldArrowButton._v, newV);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C5A RID: 15450 RVA: 0x001CDCD8 File Offset: 0x001CBED8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NGoldArrowButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGoldArrowButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGoldArrowButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGoldArrowButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGoldArrowButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGoldArrowButton.MethodName.UpdateShaderParam, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "newV", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C5B RID: 15451 RVA: 0x001CDE2C File Offset: 0x001CC02C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGoldArrowButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGoldArrowButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGoldArrowButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGoldArrowButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGoldArrowButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGoldArrowButton.MethodName.UpdateShaderParam && args.Count == 1)
			{
				this.UpdateShaderParam(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003C5C RID: 15452 RVA: 0x001CDF30 File Offset: 0x001CC130
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGoldArrowButton.MethodName._Ready || (ref method) == NGoldArrowButton.MethodName.OnFocus || (ref method) == NGoldArrowButton.MethodName.OnUnfocus || (ref method) == NGoldArrowButton.MethodName.OnPress || (ref method) == NGoldArrowButton.MethodName.OnRelease || (ref method) == NGoldArrowButton.MethodName.UpdateShaderParam || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003C5D RID: 15453 RVA: 0x001CDFA0 File Offset: 0x001CC1A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGoldArrowButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NGoldArrowButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NGoldArrowButton.PropertyName._animTween)
			{
				this._animTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NGoldArrowButton.PropertyName._valueDefault)
			{
				this._valueDefault = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NGoldArrowButton.PropertyName._valueHovered)
			{
				this._valueHovered = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NGoldArrowButton.PropertyName._hoverScale)
			{
				this._hoverScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003C5E RID: 15454 RVA: 0x001CE058 File Offset: 0x001CC258
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGoldArrowButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NGoldArrowButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NGoldArrowButton.PropertyName._animTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animTween);
				return true;
			}
			if ((ref name) == NGoldArrowButton.PropertyName._valueDefault)
			{
				value = VariantUtils.CreateFrom<float>(ref this._valueDefault);
				return true;
			}
			if ((ref name) == NGoldArrowButton.PropertyName._valueHovered)
			{
				value = VariantUtils.CreateFrom<float>(ref this._valueHovered);
				return true;
			}
			if ((ref name) == NGoldArrowButton.PropertyName._hoverScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._hoverScale);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003C5F RID: 15455 RVA: 0x001CE130 File Offset: 0x001CC330
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGoldArrowButton.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGoldArrowButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGoldArrowButton.PropertyName._animTween, 0L, "", 4096L, false),
				new PropertyInfo(3L, NGoldArrowButton.PropertyName._valueDefault, 0L, "", 4096L, false),
				new PropertyInfo(3L, NGoldArrowButton.PropertyName._valueHovered, 0L, "", 4096L, false),
				new PropertyInfo(5L, NGoldArrowButton.PropertyName._hoverScale, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003C60 RID: 15456 RVA: 0x001CE208 File Offset: 0x001CC408
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGoldArrowButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NGoldArrowButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NGoldArrowButton.PropertyName._animTween, Variant.From<Tween>(ref this._animTween));
			info.AddProperty(NGoldArrowButton.PropertyName._valueDefault, Variant.From<float>(ref this._valueDefault));
			info.AddProperty(NGoldArrowButton.PropertyName._valueHovered, Variant.From<float>(ref this._valueHovered));
			info.AddProperty(NGoldArrowButton.PropertyName._hoverScale, Variant.From<Vector2>(ref this._hoverScale));
		}

		/// <inheritdoc />
		// Token: 0x06003C61 RID: 15457 RVA: 0x001CE2A0 File Offset: 0x001CC4A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGoldArrowButton.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGoldArrowButton.PropertyName._hsv, ref variant2))
			{
				this._hsv = variant2.As<ShaderMaterial>();
			}
			Variant variant3;
			if (info.TryGetProperty(NGoldArrowButton.PropertyName._animTween, ref variant3))
			{
				this._animTween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NGoldArrowButton.PropertyName._valueDefault, ref variant4))
			{
				this._valueDefault = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NGoldArrowButton.PropertyName._valueHovered, ref variant5))
			{
				this._valueHovered = variant5.As<float>();
			}
			Variant variant6;
			if (info.TryGetProperty(NGoldArrowButton.PropertyName._hoverScale, ref variant6))
			{
				this._hoverScale = variant6.As<Vector2>();
			}
		}

		// Token: 0x04001791 RID: 6033
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001792 RID: 6034
		[Nullable(1)]
		protected TextureRect _icon;

		// Token: 0x04001793 RID: 6035
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04001794 RID: 6036
		[Nullable(2)]
		private Tween _animTween;

		// Token: 0x04001795 RID: 6037
		private float _valueDefault = 0.9f;

		// Token: 0x04001796 RID: 6038
		private float _valueHovered = 1.2f;

		// Token: 0x04001797 RID: 6039
		private Vector2 _hoverScale = Vector2.One * 1.1f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200184D RID: 6221
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005F2F RID: 24367
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005F30 RID: 24368
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005F31 RID: 24369
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005F32 RID: 24370
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005F33 RID: 24371
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'UpdateShaderParam' method.
			/// </summary>
			// Token: 0x04005F34 RID: 24372
			public static readonly StringName UpdateShaderParam = "UpdateShaderParam";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200184E RID: 6222
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04005F35 RID: 24373
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005F36 RID: 24374
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_animTween' field.
			/// </summary>
			// Token: 0x04005F37 RID: 24375
			public static readonly StringName _animTween = "_animTween";

			/// <summary>
			/// Cached name for the '_valueDefault' field.
			/// </summary>
			// Token: 0x04005F38 RID: 24376
			public static readonly StringName _valueDefault = "_valueDefault";

			/// <summary>
			/// Cached name for the '_valueHovered' field.
			/// </summary>
			// Token: 0x04005F39 RID: 24377
			public static readonly StringName _valueHovered = "_valueHovered";

			/// <summary>
			/// Cached name for the '_hoverScale' field.
			/// </summary>
			// Token: 0x04005F3A RID: 24378
			public static readonly StringName _hoverScale = "_hoverScale";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200184F RID: 6223
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
