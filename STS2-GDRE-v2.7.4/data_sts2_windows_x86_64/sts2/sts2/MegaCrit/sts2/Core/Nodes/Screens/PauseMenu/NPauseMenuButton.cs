using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu
{
	// Token: 0x020002E1 RID: 737
	[ScriptPath("res://src/Core/Nodes/Screens/PauseMenu/NPauseMenuButton.cs")]
	public class NPauseMenuButton : NButton
	{
		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002776 RID: 10102 RVA: 0x0010DE95 File Offset: 0x0010C095
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x0010DE98 File Offset: 0x0010C098
		public override void _Ready()
		{
			this.ConnectSignals();
			this._label = base.GetNode<MegaLabel>("Label");
			this._image = base.GetNode<TextureRect>("ButtonImage");
			this._hsv = (ShaderMaterial)this._image.Material;
		}

		/// <summary>
		/// Hover. Brighten a lil.
		/// </summary>
		// Token: 0x06002778 RID: 10104 RVA: 0x0010DEF0 File Offset: 0x0010C0F0
		protected override void OnFocus()
		{
			base.OnFocus();
			this._hsv.SetShaderParameter(NPauseMenuButton._s, 1.1f);
			this._hsv.SetShaderParameter(NPauseMenuButton._v, 1.1f);
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", Vector2.One * 0.95f, 0.05);
			this._tween.TweenProperty(this._label, "theme_override_colors/font_color", StsColors.gold, 0.05000000074505806);
		}

		/// <summary>
		/// Unhover
		/// </summary>
		// Token: 0x06002779 RID: 10105 RVA: 0x0010DFC4 File Offset: 0x0010C1C4
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NPauseMenuButton._s), 0.8f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPauseMenuButton._v), 0.9f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._image, "scale", Vector2.One * 0.9f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._image, "position:y", 0f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._label, "position:y", 0f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._label, "theme_override_colors/font_color", StsColors.cream, 0.05000000074505806);
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x0010E186 File Offset: 0x0010C386
		protected override void OnDisable()
		{
			base.OnDisable();
			base.Modulate = StsColors.gray;
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x0010E199 File Offset: 0x0010C399
		protected override void OnEnable()
		{
			base.OnDisable();
			base.Modulate = Colors.White;
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x0010E1AC File Offset: 0x0010C3AC
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NPauseMenuButton._s), 0.8f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPauseMenuButton._v), 0.9f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._image, "scale", Vector2.One * 0.85f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._image, "position:y", 6f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this._label, "position:y", 6f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x0010E33F File Offset: 0x0010C53F
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NPauseMenuButton._s, value);
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x0010E357 File Offset: 0x0010C557
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NPauseMenuButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600277F RID: 10111 RVA: 0x0010E370 File Offset: 0x0010C570
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NPauseMenuButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPauseMenuButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPauseMenuButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPauseMenuButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPauseMenuButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPauseMenuButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPauseMenuButton.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPauseMenuButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002780 RID: 10112 RVA: 0x0010E548 File Offset: 0x0010C748
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPauseMenuButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenuButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenuButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenuButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenuButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenuButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenuButton.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPauseMenuButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002781 RID: 10113 RVA: 0x0010E6A4 File Offset: 0x0010C8A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPauseMenuButton.MethodName._Ready || (ref method) == NPauseMenuButton.MethodName.OnFocus || (ref method) == NPauseMenuButton.MethodName.OnUnfocus || (ref method) == NPauseMenuButton.MethodName.OnDisable || (ref method) == NPauseMenuButton.MethodName.OnEnable || (ref method) == NPauseMenuButton.MethodName.OnPress || (ref method) == NPauseMenuButton.MethodName.UpdateShaderS || (ref method) == NPauseMenuButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002782 RID: 10114 RVA: 0x0010E730 File Offset: 0x0010C930
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPauseMenuButton.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenuButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenuButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenuButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NPauseMenuButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002783 RID: 10115 RVA: 0x0010E7CC File Offset: 0x0010C9CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPauseMenuButton.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NPauseMenuButton.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NPauseMenuButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NPauseMenuButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NPauseMenuButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NPauseMenuButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002784 RID: 10116 RVA: 0x0010E8A4 File Offset: 0x0010CAA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPauseMenuButton.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenuButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenuButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenuButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPauseMenuButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPauseMenuButton.PropertyName.UseSharedBackstop, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002785 RID: 10117 RVA: 0x0010E980 File Offset: 0x0010CB80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPauseMenuButton.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NPauseMenuButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NPauseMenuButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NPauseMenuButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NPauseMenuButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06002786 RID: 10118 RVA: 0x0010EA04 File Offset: 0x0010CC04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPauseMenuButton.PropertyName._backButton, ref variant))
			{
				this._backButton = variant.As<NBackButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPauseMenuButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPauseMenuButton.PropertyName._image, ref variant3))
			{
				this._image = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPauseMenuButton.PropertyName._hsv, ref variant4))
			{
				this._hsv = variant4.As<ShaderMaterial>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPauseMenuButton.PropertyName._tween, ref variant5))
			{
				this._tween = variant5.As<Tween>();
			}
		}

		// Token: 0x04000EB4 RID: 3764
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000EB5 RID: 3765
		[Nullable(1)]
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04000EB6 RID: 3766
		[Nullable(1)]
		private NBackButton _backButton;

		// Token: 0x04000EB7 RID: 3767
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x04000EB8 RID: 3768
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04000EB9 RID: 3769
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04000EBA RID: 3770
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000EBB RID: 3771
		private const float _hoverS = 1.1f;

		// Token: 0x04000EBC RID: 3772
		private const float _hoverV = 1.1f;

		// Token: 0x04000EBD RID: 3773
		private const float _unhoverS = 0.8f;

		// Token: 0x04000EBE RID: 3774
		private const float _unhoverV = 0.9f;

		// Token: 0x04000EBF RID: 3775
		private const float _pressDownYOffset = 6f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200147A RID: 5242
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004DB7 RID: 19895
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004DB8 RID: 19896
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004DB9 RID: 19897
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04004DBA RID: 19898
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04004DBB RID: 19899
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004DBC RID: 19900
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x04004DBD RID: 19901
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04004DBE RID: 19902
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200147B RID: 5243
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04004DBF RID: 19903
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04004DC0 RID: 19904
			public static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004DC1 RID: 19905
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004DC2 RID: 19906
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04004DC3 RID: 19907
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004DC4 RID: 19908
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200147C RID: 5244
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
