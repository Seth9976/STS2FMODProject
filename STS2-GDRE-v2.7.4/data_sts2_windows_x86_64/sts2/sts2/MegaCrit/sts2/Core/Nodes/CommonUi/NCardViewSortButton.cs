using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	// Token: 0x020003D4 RID: 980
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NCardViewSortButton.cs")]
	public class NCardViewSortButton : NButton
	{
		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06003B49 RID: 15177 RVA: 0x001C4DBF File Offset: 0x001C2FBF
		// (set) Token: 0x06003B4A RID: 15178 RVA: 0x001C4DC7 File Offset: 0x001C2FC7
		public bool IsDescending
		{
			get
			{
				return this._isDescending;
			}
			set
			{
				this._isDescending = value;
				this.OnToggle();
			}
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x001C4DD8 File Offset: 0x001C2FD8
		public override void _Ready()
		{
			this.ConnectSignals();
			this._button = base.GetNode<Control>("%ButtonImage");
			this._hsv = (ShaderMaterial)this._button.GetMaterial();
			this._label = base.GetNode<MegaLabel>("%Label");
			this._sortIcon = base.GetNode<TextureRect>("%Image");
			this._selectionReticle = base.GetNode<NSelectionReticle>("SelectionReticle");
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x001C4E59 File Offset: 0x001C3059
		public void SetHue(ShaderMaterial mat)
		{
			this._hsv.SetShaderParameter(NCardViewSortButton._h, mat.GetShaderParameter(NCardViewSortButton._h));
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x001C4E76 File Offset: 0x001C3076
		private void OnToggle()
		{
			this._sortIcon.SetFlipV(!this._isDescending);
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x001C4E8C File Offset: 0x001C308C
		protected override void OnRelease()
		{
			base.OnRelease();
			this.IsDescending = !this.IsDescending;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._button, "scale", new Vector2(1.05f, 1.05f), 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardViewSortButton._s), 1f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardViewSortButton._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x001C4FA8 File Offset: 0x001C31A8
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._button, "scale", new Vector2(1.05f, 1.05f), 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardViewSortButton._s), 1f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardViewSortButton._v), 1f, 0.5).SetEase(1L).SetTrans(5L);
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x001C50CC File Offset: 0x001C32CC
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._button, "scale", new Vector2(1.05f, 1f), 0.5);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardViewSortButton._s), 0.8f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardViewSortButton._v), 0.8f, 0.5).SetEase(1L).SetTrans(5L);
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnDeselect();
			}
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x001C51F0 File Offset: 0x001C33F0
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._button, "scale", new Vector2(1f, 0.95f), 0.25).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardViewSortButton._s), 0.8f, 0.25).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardViewSortButton._v), 0.8f, 0.25).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x001C5309 File Offset: 0x001C3509
		public void SetLabel(string text)
		{
			this._label.SetTextAutoSize(text);
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x001C5317 File Offset: 0x001C3517
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NCardViewSortButton._s, value);
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x001C532F File Offset: 0x001C352F
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NCardViewSortButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B55 RID: 15189 RVA: 0x001C5348 File Offset: 0x001C3548
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NCardViewSortButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardViewSortButton.MethodName.SetHue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "mat", 0L, "", 6L, new StringName("ShaderMaterial"), false)
				}, null),
				new MethodInfo(NCardViewSortButton.MethodName.OnToggle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardViewSortButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardViewSortButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardViewSortButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardViewSortButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardViewSortButton.MethodName.SetLabel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardViewSortButton.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardViewSortButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B56 RID: 15190 RVA: 0x001C55D4 File Offset: 0x001C37D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardViewSortButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardViewSortButton.MethodName.SetHue && args.Count == 1)
			{
				this.SetHue(VariantUtils.ConvertTo<ShaderMaterial>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardViewSortButton.MethodName.OnToggle && args.Count == 0)
			{
				this.OnToggle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardViewSortButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardViewSortButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardViewSortButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardViewSortButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardViewSortButton.MethodName.SetLabel && args.Count == 1)
			{
				this.SetLabel(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardViewSortButton.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardViewSortButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003B57 RID: 15191 RVA: 0x001C5794 File Offset: 0x001C3994
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardViewSortButton.MethodName._Ready || (ref method) == NCardViewSortButton.MethodName.SetHue || (ref method) == NCardViewSortButton.MethodName.OnToggle || (ref method) == NCardViewSortButton.MethodName.OnRelease || (ref method) == NCardViewSortButton.MethodName.OnFocus || (ref method) == NCardViewSortButton.MethodName.OnUnfocus || (ref method) == NCardViewSortButton.MethodName.OnPress || (ref method) == NCardViewSortButton.MethodName.SetLabel || (ref method) == NCardViewSortButton.MethodName.UpdateShaderS || (ref method) == NCardViewSortButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003B58 RID: 15192 RVA: 0x001C5840 File Offset: 0x001C3A40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardViewSortButton.PropertyName.IsDescending)
			{
				this.IsDescending = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._isDescending)
			{
				this._isDescending = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._button)
			{
				this._button = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._sortIcon)
			{
				this._sortIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003B59 RID: 15193 RVA: 0x001C5930 File Offset: 0x001C3B30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardViewSortButton.PropertyName.IsDescending)
			{
				bool isDescending = this.IsDescending;
				value = VariantUtils.CreateFrom<bool>(ref isDescending);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._isDescending)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDescending);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._button)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._button);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._sortIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._sortIcon);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NCardViewSortButton.PropertyName._tween)
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
		// Token: 0x06003B5A RID: 15194 RVA: 0x001C5A48 File Offset: 0x001C3C48
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NCardViewSortButton.PropertyName._isDescending, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardViewSortButton.PropertyName._button, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardViewSortButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardViewSortButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardViewSortButton.PropertyName._sortIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardViewSortButton.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardViewSortButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardViewSortButton.PropertyName.IsDescending, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B5B RID: 15195 RVA: 0x001C5B64 File Offset: 0x001C3D64
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isDescending = NCardViewSortButton.PropertyName.IsDescending;
			bool isDescending2 = this.IsDescending;
			info.AddProperty(isDescending, Variant.From<bool>(ref isDescending2));
			info.AddProperty(NCardViewSortButton.PropertyName._isDescending, Variant.From<bool>(ref this._isDescending));
			info.AddProperty(NCardViewSortButton.PropertyName._button, Variant.From<Control>(ref this._button));
			info.AddProperty(NCardViewSortButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NCardViewSortButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NCardViewSortButton.PropertyName._sortIcon, Variant.From<TextureRect>(ref this._sortIcon));
			info.AddProperty(NCardViewSortButton.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NCardViewSortButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06003B5C RID: 15196 RVA: 0x001C5C2C File Offset: 0x001C3E2C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardViewSortButton.PropertyName.IsDescending, ref variant))
			{
				this.IsDescending = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardViewSortButton.PropertyName._isDescending, ref variant2))
			{
				this._isDescending = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardViewSortButton.PropertyName._button, ref variant3))
			{
				this._button = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardViewSortButton.PropertyName._hsv, ref variant4))
			{
				this._hsv = variant4.As<ShaderMaterial>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardViewSortButton.PropertyName._label, ref variant5))
			{
				this._label = variant5.As<MegaLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardViewSortButton.PropertyName._sortIcon, ref variant6))
			{
				this._sortIcon = variant6.As<TextureRect>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardViewSortButton.PropertyName._selectionReticle, ref variant7))
			{
				this._selectionReticle = variant7.As<NSelectionReticle>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCardViewSortButton.PropertyName._tween, ref variant8))
			{
				this._tween = variant8.As<Tween>();
			}
		}

		// Token: 0x04001721 RID: 5921
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001722 RID: 5922
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04001723 RID: 5923
		private static readonly StringName _h = new StringName("h");

		// Token: 0x04001724 RID: 5924
		private bool _isDescending;

		// Token: 0x04001725 RID: 5925
		private Control _button;

		// Token: 0x04001726 RID: 5926
		private ShaderMaterial _hsv;

		// Token: 0x04001727 RID: 5927
		private MegaLabel _label;

		// Token: 0x04001728 RID: 5928
		private TextureRect _sortIcon;

		// Token: 0x04001729 RID: 5929
		private NSelectionReticle _selectionReticle;

		// Token: 0x0400172A RID: 5930
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001822 RID: 6178
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005E5C RID: 24156
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetHue' method.
			/// </summary>
			// Token: 0x04005E5D RID: 24157
			public static readonly StringName SetHue = "SetHue";

			/// <summary>
			/// Cached name for the 'OnToggle' method.
			/// </summary>
			// Token: 0x04005E5E RID: 24158
			public static readonly StringName OnToggle = "OnToggle";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005E5F RID: 24159
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005E60 RID: 24160
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005E61 RID: 24161
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005E62 RID: 24162
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'SetLabel' method.
			/// </summary>
			// Token: 0x04005E63 RID: 24163
			public static readonly StringName SetLabel = "SetLabel";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x04005E64 RID: 24164
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04005E65 RID: 24165
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001823 RID: 6179
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsDescending' property.
			/// </summary>
			// Token: 0x04005E66 RID: 24166
			public static readonly StringName IsDescending = "IsDescending";

			/// <summary>
			/// Cached name for the '_isDescending' field.
			/// </summary>
			// Token: 0x04005E67 RID: 24167
			public static readonly StringName _isDescending = "_isDescending";

			/// <summary>
			/// Cached name for the '_button' field.
			/// </summary>
			// Token: 0x04005E68 RID: 24168
			public static readonly StringName _button = "_button";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005E69 RID: 24169
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005E6A RID: 24170
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_sortIcon' field.
			/// </summary>
			// Token: 0x04005E6B RID: 24171
			public static readonly StringName _sortIcon = "_sortIcon";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04005E6C RID: 24172
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005E6D RID: 24173
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001824 RID: 6180
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
