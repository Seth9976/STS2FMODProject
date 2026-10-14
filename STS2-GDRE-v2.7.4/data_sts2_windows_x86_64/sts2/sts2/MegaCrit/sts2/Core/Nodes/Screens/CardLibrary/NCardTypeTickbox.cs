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
	// Token: 0x02000357 RID: 855
	[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardTypeTickbox.cs")]
	public class NCardTypeTickbox : NButton
	{
		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x0600316D RID: 12653 RVA: 0x0016F230 File Offset: 0x0016D430
		// (set) Token: 0x0600316E RID: 12654 RVA: 0x0016F238 File Offset: 0x0016D438
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

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x0600316F RID: 12655 RVA: 0x0016F258 File Offset: 0x0016D458
		// (set) Token: 0x06003170 RID: 12656 RVA: 0x0016F260 File Offset: 0x0016D460
		[Nullable(1)]
		public LocString Loc
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x0016F26C File Offset: 0x0016D46C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._baseScale = base.Scale;
			this._image = base.GetNode<Control>("%Image");
			this._outline = base.GetNode<Control>("%Outline");
			this._hsv = (ShaderMaterial)this._image.Material;
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x0016F2D0 File Offset: 0x0016D4D0
		private void OnToggle()
		{
			this._baseS = (this._isTicked ? 1f : 0.65f);
			this._baseV = (this._isTicked ? 1.2f : 0.7f);
			this.UpdateShaderS(this._baseS);
			this.UpdateShaderV(this._baseV);
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x0016F32C File Offset: 0x0016D52C
		protected override void OnRelease()
		{
			base.OnRelease();
			this.IsTicked = !this.IsTicked;
			base.EmitSignal(NCardTypeTickbox.SignalName.Toggled, new Variant[] { this });
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale * this._hoverScale, 0.25).SetEase(1L).SetTrans(10L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardTypeTickbox._s), this._baseS, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardTypeTickbox._v), this._baseV, 0.05);
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x0016F45C File Offset: 0x0016D65C
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
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardTypeTickbox._s), this._isTicked ? 1.4f : 1f, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardTypeTickbox._v), this._isTicked ? 1.4f : 1f, 0.05);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, new HoverTip(this.Loc, null), HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(new Vector2(310f, base.GlobalPosition.Y), false);
			}
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x0016F5A4 File Offset: 0x0016D7A4
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
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardTypeTickbox._s), this._baseS, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardTypeTickbox._v), this._baseV, 0.5).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x0016F6BC File Offset: 0x0016D8BC
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", this._baseScale * this._pressDownScale, 0.25).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardTypeTickbox._s), this._baseS, 0.25).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardTypeTickbox._v), this._baseV, 0.25).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x0016F7D9 File Offset: 0x0016D9D9
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NCardTypeTickbox._s, value);
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x0016F7F1 File Offset: 0x0016D9F1
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NCardTypeTickbox._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003179 RID: 12665 RVA: 0x0016F80C File Offset: 0x0016DA0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NCardTypeTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardTypeTickbox.MethodName.OnToggle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardTypeTickbox.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardTypeTickbox.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardTypeTickbox.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardTypeTickbox.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardTypeTickbox.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardTypeTickbox.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600317A RID: 12666 RVA: 0x0016F9E4 File Offset: 0x0016DBE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardTypeTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTypeTickbox.MethodName.OnToggle && args.Count == 0)
			{
				this.OnToggle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTypeTickbox.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTypeTickbox.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTypeTickbox.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTypeTickbox.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTypeTickbox.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardTypeTickbox.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600317B RID: 12667 RVA: 0x0016FB40 File Offset: 0x0016DD40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardTypeTickbox.MethodName._Ready || (ref method) == NCardTypeTickbox.MethodName.OnToggle || (ref method) == NCardTypeTickbox.MethodName.OnRelease || (ref method) == NCardTypeTickbox.MethodName.OnFocus || (ref method) == NCardTypeTickbox.MethodName.OnUnfocus || (ref method) == NCardTypeTickbox.MethodName.OnPress || (ref method) == NCardTypeTickbox.MethodName.UpdateShaderS || (ref method) == NCardTypeTickbox.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600317C RID: 12668 RVA: 0x0016FBCC File Offset: 0x0016DDCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardTypeTickbox.PropertyName.IsTicked)
			{
				this.IsTicked = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._isTicked)
			{
				this._isTicked = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._baseS)
			{
				this._baseS = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._baseV)
			{
				this._baseV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._baseScale)
			{
				this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._hoverScale)
			{
				this._hoverScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._pressDownScale)
			{
				this._pressDownScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600317D RID: 12669 RVA: 0x0016FD0C File Offset: 0x0016DF0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardTypeTickbox.PropertyName.IsTicked)
			{
				bool isTicked = this.IsTicked;
				value = VariantUtils.CreateFrom<bool>(ref isTicked);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._isTicked)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isTicked);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._baseS)
			{
				value = VariantUtils.CreateFrom<float>(ref this._baseS);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._baseV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._baseV);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outline);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._image);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._baseScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._hoverScale)
			{
				value = VariantUtils.CreateFrom<float>(ref this._hoverScale);
				return true;
			}
			if ((ref name) == NCardTypeTickbox.PropertyName._pressDownScale)
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
		// Token: 0x0600317E RID: 12670 RVA: 0x0016FE84 File Offset: 0x0016E084
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NCardTypeTickbox.PropertyName._isTicked, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardTypeTickbox.PropertyName._baseS, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardTypeTickbox.PropertyName._baseV, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardTypeTickbox.PropertyName.IsTicked, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardTypeTickbox.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardTypeTickbox.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardTypeTickbox.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardTypeTickbox.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCardTypeTickbox.PropertyName._baseScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardTypeTickbox.PropertyName._hoverScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardTypeTickbox.PropertyName._pressDownScale, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600317F RID: 12671 RVA: 0x0016FFFC File Offset: 0x0016E1FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isTicked = NCardTypeTickbox.PropertyName.IsTicked;
			bool isTicked2 = this.IsTicked;
			info.AddProperty(isTicked, Variant.From<bool>(ref isTicked2));
			info.AddProperty(NCardTypeTickbox.PropertyName._isTicked, Variant.From<bool>(ref this._isTicked));
			info.AddProperty(NCardTypeTickbox.PropertyName._baseS, Variant.From<float>(ref this._baseS));
			info.AddProperty(NCardTypeTickbox.PropertyName._baseV, Variant.From<float>(ref this._baseV));
			info.AddProperty(NCardTypeTickbox.PropertyName._outline, Variant.From<Control>(ref this._outline));
			info.AddProperty(NCardTypeTickbox.PropertyName._image, Variant.From<Control>(ref this._image));
			info.AddProperty(NCardTypeTickbox.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NCardTypeTickbox.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NCardTypeTickbox.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
			info.AddProperty(NCardTypeTickbox.PropertyName._hoverScale, Variant.From<float>(ref this._hoverScale));
			info.AddProperty(NCardTypeTickbox.PropertyName._pressDownScale, Variant.From<float>(ref this._pressDownScale));
			info.AddSignalEventDelegate(NCardTypeTickbox.SignalName.Toggled, this.backing_Toggled);
		}

		/// <inheritdoc />
		// Token: 0x06003180 RID: 12672 RVA: 0x00170118 File Offset: 0x0016E318
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardTypeTickbox.PropertyName.IsTicked, ref variant))
			{
				this.IsTicked = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardTypeTickbox.PropertyName._isTicked, ref variant2))
			{
				this._isTicked = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardTypeTickbox.PropertyName._baseS, ref variant3))
			{
				this._baseS = variant3.As<float>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardTypeTickbox.PropertyName._baseV, ref variant4))
			{
				this._baseV = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardTypeTickbox.PropertyName._outline, ref variant5))
			{
				this._outline = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardTypeTickbox.PropertyName._image, ref variant6))
			{
				this._image = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardTypeTickbox.PropertyName._hsv, ref variant7))
			{
				this._hsv = variant7.As<ShaderMaterial>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCardTypeTickbox.PropertyName._tween, ref variant8))
			{
				this._tween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCardTypeTickbox.PropertyName._baseScale, ref variant9))
			{
				this._baseScale = variant9.As<Vector2>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCardTypeTickbox.PropertyName._hoverScale, ref variant10))
			{
				this._hoverScale = variant10.As<float>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCardTypeTickbox.PropertyName._pressDownScale, ref variant11))
			{
				this._pressDownScale = variant11.As<float>();
			}
			NCardTypeTickbox.ToggledEventHandler toggledEventHandler;
			if (info.TryGetSignalEventDelegate<NCardTypeTickbox.ToggledEventHandler>(NCardTypeTickbox.SignalName.Toggled, ref toggledEventHandler))
			{
				this.backing_Toggled = toggledEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003181 RID: 12673 RVA: 0x00170278 File Offset: 0x0016E478
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NCardTypeTickbox.SignalName.Toggled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary.NCardTypeTickbox.ToggledEventHandler" />
		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06003182 RID: 12674 RVA: 0x001702EC File Offset: 0x0016E4EC
		// (remove) Token: 0x06003183 RID: 12675 RVA: 0x00170305 File Offset: 0x0016E505
		public event NCardTypeTickbox.ToggledEventHandler Toggled
		{
			add
			{
				this.backing_Toggled = (NCardTypeTickbox.ToggledEventHandler)Delegate.Combine(this.backing_Toggled, value);
			}
			remove
			{
				this.backing_Toggled = (NCardTypeTickbox.ToggledEventHandler)Delegate.Remove(this.backing_Toggled, value);
			}
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x0017031E File Offset: 0x0016E51E
		protected void EmitSignalToggled(NCardTypeTickbox tickbox)
		{
			base.EmitSignal(NCardTypeTickbox.SignalName.Toggled, new Variant[] { tickbox });
		}

		/// <inheritdoc />
		// Token: 0x06003185 RID: 12677 RVA: 0x0017033F File Offset: 0x0016E53F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NCardTypeTickbox.SignalName.Toggled) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(in signal, args);
				return;
			}
			NCardTypeTickbox.ToggledEventHandler toggledEventHandler = this.backing_Toggled;
			if (toggledEventHandler == null)
			{
				return;
			}
			toggledEventHandler(VariantUtils.ConvertTo<NCardTypeTickbox>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x06003186 RID: 12678 RVA: 0x0017037E File Offset: 0x0016E57E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NCardTypeTickbox.SignalName.Toggled || base.HasGodotClassSignal(in signal);
		}

		// Token: 0x04001343 RID: 4931
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001344 RID: 4932
		[Nullable(1)]
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04001345 RID: 4933
		private bool _isTicked = true;

		// Token: 0x04001346 RID: 4934
		private float _baseS = 1f;

		// Token: 0x04001347 RID: 4935
		private float _baseV = 1.2f;

		// Token: 0x04001349 RID: 4937
		[Nullable(1)]
		private Control _outline;

		// Token: 0x0400134A RID: 4938
		[Nullable(1)]
		private Control _image;

		// Token: 0x0400134B RID: 4939
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x0400134C RID: 4940
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x0400134D RID: 4941
		private Vector2 _baseScale;

		// Token: 0x0400134E RID: 4942
		private float _hoverScale = 1.2f;

		// Token: 0x0400134F RID: 4943
		private float _pressDownScale = 0.9f;

		// Token: 0x04001350 RID: 4944
		private NCardTypeTickbox.ToggledEventHandler backing_Toggled;

		// Token: 0x02001642 RID: 5698
		// (Invoke) Token: 0x0600A465 RID: 42085
		[Signal]
		public delegate void ToggledEventHandler(NCardTypeTickbox tickbox);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001643 RID: 5699
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040056B3 RID: 22195
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnToggle' method.
			/// </summary>
			// Token: 0x040056B4 RID: 22196
			public static readonly StringName OnToggle = "OnToggle";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x040056B5 RID: 22197
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040056B6 RID: 22198
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040056B7 RID: 22199
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x040056B8 RID: 22200
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x040056B9 RID: 22201
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x040056BA RID: 22202
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001644 RID: 5700
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsTicked' property.
			/// </summary>
			// Token: 0x040056BB RID: 22203
			public static readonly StringName IsTicked = "IsTicked";

			/// <summary>
			/// Cached name for the '_isTicked' field.
			/// </summary>
			// Token: 0x040056BC RID: 22204
			public static readonly StringName _isTicked = "_isTicked";

			/// <summary>
			/// Cached name for the '_baseS' field.
			/// </summary>
			// Token: 0x040056BD RID: 22205
			public static readonly StringName _baseS = "_baseS";

			/// <summary>
			/// Cached name for the '_baseV' field.
			/// </summary>
			// Token: 0x040056BE RID: 22206
			public static readonly StringName _baseV = "_baseV";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x040056BF RID: 22207
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x040056C0 RID: 22208
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x040056C1 RID: 22209
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040056C2 RID: 22210
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_baseScale' field.
			/// </summary>
			// Token: 0x040056C3 RID: 22211
			public static readonly StringName _baseScale = "_baseScale";

			/// <summary>
			/// Cached name for the '_hoverScale' field.
			/// </summary>
			// Token: 0x040056C4 RID: 22212
			public static readonly StringName _hoverScale = "_hoverScale";

			/// <summary>
			/// Cached name for the '_pressDownScale' field.
			/// </summary>
			// Token: 0x040056C5 RID: 22213
			public static readonly StringName _pressDownScale = "_pressDownScale";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001645 RID: 5701
		public new class SignalName : NButton.SignalName
		{
			/// <summary>
			/// Cached name for the 'Toggled' signal.
			/// </summary>
			// Token: 0x040056C6 RID: 22214
			public static readonly StringName Toggled = "Toggled";
		}
	}
}
