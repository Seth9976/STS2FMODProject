using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary
{
	/// <summary>
	/// Pretty much a copy of NTickbox.
	/// Doesn't inherit it because it doesn't use the standard tickbox visuals.
	/// </summary>
	// Token: 0x02000350 RID: 848
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardCostTickbox.cs")]
	public class NCardCostTickbox : NButton
	{
		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060030CA RID: 12490 RVA: 0x00168584 File Offset: 0x00166784
		// (set) Token: 0x060030CB RID: 12491 RVA: 0x0016858C File Offset: 0x0016678C
		public LocString Loc { get; set; }

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060030CC RID: 12492 RVA: 0x00168595 File Offset: 0x00166795
		// (set) Token: 0x060030CD RID: 12493 RVA: 0x0016859D File Offset: 0x0016679D
		public bool IsTicked
		{
			get
			{
				return this._isTicked;
			}
			set
			{
				this._isTicked = value;
				this._outline.Visible = this._isTicked;
				this.OnToggle();
			}
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x001685C0 File Offset: 0x001667C0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._baseScale = base.Scale;
			this._outline = base.GetNode<Control>("%Outline");
			this._image = base.GetNode<Control>("%Image");
			this._hsv = (ShaderMaterial)this._image.Material;
			base.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this.IsTicked = !this.IsTicked;
			}), 0U);
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x00168640 File Offset: 0x00166840
		private void OnToggle()
		{
			this._baseS = (this._isTicked ? 1f : 0.65f);
			this._baseV = (this._isTicked ? 1.2f : 0.7f);
			this.UpdateShaderS(this._baseS);
			this.UpdateShaderV(this._baseV);
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x0016869C File Offset: 0x0016689C
		protected override void OnRelease()
		{
			base.OnRelease();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale * this._hoverScale, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardCostTickbox._s), this._baseS, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardCostTickbox._v), this._baseV, 0.05);
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x00168790 File Offset: 0x00166990
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale * this._hoverScale, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardCostTickbox._s), this._isTicked ? 1.4f : 1f, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardCostTickbox._v), this._isTicked ? 1.4f : 1f, 0.05);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, new HoverTip(this.Loc, null), HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(new Vector2(310f, base.GlobalPosition.Y), false);
			}
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x001688D8 File Offset: 0x00166AD8
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardCostTickbox._s), this._baseS, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardCostTickbox._v), this._baseV, 0.5).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x001689F0 File Offset: 0x00166BF0
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale * this._pressDownScale, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardCostTickbox._s), this._baseS, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardCostTickbox._v), this._baseV, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x00168B0D File Offset: 0x00166D0D
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NCardCostTickbox._s, value);
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x00168B25 File Offset: 0x00166D25
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NCardCostTickbox._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060030D6 RID: 12502 RVA: 0x00168B40 File Offset: 0x00166D40
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NCardCostTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardCostTickbox.MethodName.OnToggle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardCostTickbox.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardCostTickbox.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardCostTickbox.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardCostTickbox.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardCostTickbox.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardCostTickbox.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060030D7 RID: 12503 RVA: 0x00168D18 File Offset: 0x00166F18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardCostTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardCostTickbox.MethodName.OnToggle && args.Count == 0)
			{
				this.OnToggle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardCostTickbox.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardCostTickbox.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardCostTickbox.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardCostTickbox.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardCostTickbox.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardCostTickbox.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060030D8 RID: 12504 RVA: 0x00168E74 File Offset: 0x00167074
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardCostTickbox.MethodName._Ready || (ref method) == NCardCostTickbox.MethodName.OnToggle || (ref method) == NCardCostTickbox.MethodName.OnRelease || (ref method) == NCardCostTickbox.MethodName.OnFocus || (ref method) == NCardCostTickbox.MethodName.OnUnfocus || (ref method) == NCardCostTickbox.MethodName.OnPress || (ref method) == NCardCostTickbox.MethodName.UpdateShaderS || (ref method) == NCardCostTickbox.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060030D9 RID: 12505 RVA: 0x00168F00 File Offset: 0x00167100
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardCostTickbox.PropertyName.IsTicked)
			{
				this.IsTicked = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._isTicked)
			{
				this._isTicked = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._baseS)
			{
				this._baseS = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._baseV)
			{
				this._baseV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._baseScale)
			{
				this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._hoverV)
			{
				this._hoverV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._hoverScale)
			{
				this._hoverScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._pressDownScale)
			{
				this._pressDownScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060030DA RID: 12506 RVA: 0x0016905C File Offset: 0x0016725C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardCostTickbox.PropertyName.IsTicked)
			{
				bool isTicked = this.IsTicked;
				value = VariantUtils.CreateFrom<bool>(ref isTicked);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._isTicked)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isTicked);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._baseS)
			{
				value = VariantUtils.CreateFrom<float>(ref this._baseS);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._baseV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._baseV);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outline);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._image);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._baseScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._hoverV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._hoverV);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._hoverScale)
			{
				value = VariantUtils.CreateFrom<float>(ref this._hoverScale);
				return true;
			}
			if ((ref name) == NCardCostTickbox.PropertyName._pressDownScale)
			{
				value = VariantUtils.CreateFrom<float>(ref this._pressDownScale);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060030DB RID: 12507 RVA: 0x001691F4 File Offset: 0x001673F4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NCardCostTickbox.PropertyName._isTicked, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardCostTickbox.PropertyName._baseS, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardCostTickbox.PropertyName._baseV, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardCostTickbox.PropertyName.IsTicked, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardCostTickbox.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardCostTickbox.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardCostTickbox.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardCostTickbox.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCardCostTickbox.PropertyName._baseScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardCostTickbox.PropertyName._hoverV, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardCostTickbox.PropertyName._hoverScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardCostTickbox.PropertyName._pressDownScale, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060030DC RID: 12508 RVA: 0x0016938C File Offset: 0x0016758C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isTicked = NCardCostTickbox.PropertyName.IsTicked;
			bool isTicked2 = this.IsTicked;
			info.AddProperty(isTicked, Variant.From<bool>(ref isTicked2));
			info.AddProperty(NCardCostTickbox.PropertyName._isTicked, Variant.From<bool>(ref this._isTicked));
			info.AddProperty(NCardCostTickbox.PropertyName._baseS, Variant.From<float>(ref this._baseS));
			info.AddProperty(NCardCostTickbox.PropertyName._baseV, Variant.From<float>(ref this._baseV));
			info.AddProperty(NCardCostTickbox.PropertyName._outline, Variant.From<Control>(ref this._outline));
			info.AddProperty(NCardCostTickbox.PropertyName._image, Variant.From<Control>(ref this._image));
			info.AddProperty(NCardCostTickbox.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NCardCostTickbox.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NCardCostTickbox.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
			info.AddProperty(NCardCostTickbox.PropertyName._hoverV, Variant.From<float>(ref this._hoverV));
			info.AddProperty(NCardCostTickbox.PropertyName._hoverScale, Variant.From<float>(ref this._hoverScale));
			info.AddProperty(NCardCostTickbox.PropertyName._pressDownScale, Variant.From<float>(ref this._pressDownScale));
		}

		/// <inheritdoc />
		// Token: 0x060030DD RID: 12509 RVA: 0x001694AC File Offset: 0x001676AC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName.IsTicked, ref variant))
			{
				this.IsTicked = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName._isTicked, ref variant2))
			{
				this._isTicked = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName._baseS, ref variant3))
			{
				this._baseS = variant3.As<float>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName._baseV, ref variant4))
			{
				this._baseV = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName._outline, ref variant5))
			{
				this._outline = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName._image, ref variant6))
			{
				this._image = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName._hsv, ref variant7))
			{
				this._hsv = variant7.As<ShaderMaterial>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName._tween, ref variant8))
			{
				this._tween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName._baseScale, ref variant9))
			{
				this._baseScale = variant9.As<Vector2>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName._hoverV, ref variant10))
			{
				this._hoverV = variant10.As<float>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName._hoverScale, ref variant11))
			{
				this._hoverScale = variant11.As<float>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCardCostTickbox.PropertyName._pressDownScale, ref variant12))
			{
				this._pressDownScale = variant12.As<float>();
			}
		}

		// Token: 0x040012E6 RID: 4838
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040012E7 RID: 4839
		private static readonly StringName _s = new StringName("s");

		// Token: 0x040012E9 RID: 4841
		private bool _isTicked = true;

		// Token: 0x040012EA RID: 4842
		private float _baseS = 1f;

		// Token: 0x040012EB RID: 4843
		private float _baseV = 1.2f;

		// Token: 0x040012EC RID: 4844
		private Control _outline;

		// Token: 0x040012ED RID: 4845
		private Control _image;

		// Token: 0x040012EE RID: 4846
		private ShaderMaterial _hsv;

		// Token: 0x040012EF RID: 4847
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040012F0 RID: 4848
		private Vector2 _baseScale;

		// Token: 0x040012F1 RID: 4849
		private float _hoverV = 1.2f;

		// Token: 0x040012F2 RID: 4850
		private float _hoverScale = 1.2f;

		// Token: 0x040012F3 RID: 4851
		private float _pressDownScale = 0.9f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001626 RID: 5670
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005613 RID: 22035
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnToggle' method.
			/// </summary>
			// Token: 0x04005614 RID: 22036
			public static readonly StringName OnToggle = "OnToggle";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005615 RID: 22037
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005616 RID: 22038
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005617 RID: 22039
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005618 RID: 22040
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x04005619 RID: 22041
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x0400561A RID: 22042
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001627 RID: 5671
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsTicked' property.
			/// </summary>
			// Token: 0x0400561B RID: 22043
			public static readonly StringName IsTicked = "IsTicked";

			/// <summary>
			/// Cached name for the '_isTicked' field.
			/// </summary>
			// Token: 0x0400561C RID: 22044
			public static readonly StringName _isTicked = "_isTicked";

			/// <summary>
			/// Cached name for the '_baseS' field.
			/// </summary>
			// Token: 0x0400561D RID: 22045
			public static readonly StringName _baseS = "_baseS";

			/// <summary>
			/// Cached name for the '_baseV' field.
			/// </summary>
			// Token: 0x0400561E RID: 22046
			public static readonly StringName _baseV = "_baseV";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x0400561F RID: 22047
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04005620 RID: 22048
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005621 RID: 22049
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005622 RID: 22050
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_baseScale' field.
			/// </summary>
			// Token: 0x04005623 RID: 22051
			public static readonly StringName _baseScale = "_baseScale";

			/// <summary>
			/// Cached name for the '_hoverV' field.
			/// </summary>
			// Token: 0x04005624 RID: 22052
			public static readonly StringName _hoverV = "_hoverV";

			/// <summary>
			/// Cached name for the '_hoverScale' field.
			/// </summary>
			// Token: 0x04005625 RID: 22053
			public static readonly StringName _hoverScale = "_hoverScale";

			/// <summary>
			/// Cached name for the '_pressDownScale' field.
			/// </summary>
			// Token: 0x04005626 RID: 22054
			public static readonly StringName _pressDownScale = "_pressDownScale";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001628 RID: 5672
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
