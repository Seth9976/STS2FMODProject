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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	// Token: 0x020003E8 RID: 1000
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NPopupYesNoButton.cs")]
	public class NPopupYesNoButton : NButton
	{
		/// <summary>
		/// Whether this is the Yes or No option of the Pop up
		/// </summary>
		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06003CEA RID: 15594 RVA: 0x001D2024 File Offset: 0x001D0224
		// (set) Token: 0x06003CEB RID: 15595 RVA: 0x001D202C File Offset: 0x001D022C
		public bool IsYes
		{
			get
			{
				return this._isYes;
			}
			set
			{
				this.DisconnectHotkeys();
				this._isYes = value;
				Callable.From(new Action(base.RegisterHotkeys)).CallDeferred(Array.Empty<Variant>());
				base.UpdateControllerButton();
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06003CEC RID: 15596 RVA: 0x001D206A File Offset: 0x001D026A
		protected override string[] Hotkeys
		{
			get
			{
				return new string[] { this._isYes ? MegaInput.select : MegaInput.cancel };
			}
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x001D2090 File Offset: 0x001D0290
		public override void _Ready()
		{
			this.ConnectSignals();
			this._visuals = base.GetNode<Control>("%Visuals");
			this._outline = base.GetNode<Control>("%Outline");
			this._image = base.GetNode<Control>("%Image");
			this._label = base.GetNode<MegaLabel>("%Label");
			this._hsv = (ShaderMaterial)this._image.GetMaterial();
			this._outlineMaterial = (CanvasItemMaterial)this._outline.GetMaterial();
			this._baseS = (float)this._hsv.GetShaderParameter(NPopupYesNoButton._s);
			this._baseV = (float)this._hsv.GetShaderParameter(NPopupYesNoButton._v);
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x001D215F File Offset: 0x001D035F
		public override void _ExitTree()
		{
			base._ExitTree();
			this.DisconnectHotkeys();
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x001D2170 File Offset: 0x001D0370
		public void DisconnectHotkeys()
		{
			foreach (string text in this.Hotkeys)
			{
				NHotkeyManager.Instance.RemoveHotkeyPressedBinding(text, new Action(base.OnPressHandler));
				NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(text, new Action(base.OnReleaseHandler));
			}
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x001D21C4 File Offset: 0x001D03C4
		public void SetText(string text)
		{
			this._label.SetTextAutoSize(text);
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x001D21D4 File Offset: 0x001D03D4
		protected override void OnFocus()
		{
			base.OnFocus();
			this._isFocused = true;
			this._outlineMaterial.BlendMode = 1L;
			this._outline.Modulate = Colors.White;
			this._outline.SelfModulate = NPopupYesNoButton._goldOutline;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._visuals, "scale", Vector2.One * 1.025f, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NPopupYesNoButton._s), this._baseS + 0.25f, 0.05).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPopupYesNoButton._v), this._baseV + 0.25f, 0.05).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x001D2324 File Offset: 0x001D0524
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this._isFocused = false;
			this._outlineMaterial.BlendMode = 0L;
			this._outline.Modulate = StsColors.halfTransparentWhite;
			this._outline.SelfModulate = Colors.Black;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._visuals, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NPopupYesNoButton._s), this._baseS, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPopupYesNoButton._v), this._baseV, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x001D246C File Offset: 0x001D066C
		protected override void OnPress()
		{
			base.OnPress();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._visuals, "scale", Vector2.One * 0.975f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NPopupYesNoButton._s), this._baseS - 0.1f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPopupYesNoButton._v), this._baseV - 0.1f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x001D2594 File Offset: 0x001D0794
		protected override void OnRelease()
		{
			this._isFocused = false;
			this._outlineMaterial.BlendMode = 0L;
			this._outline.Modulate = StsColors.halfTransparentWhite;
			this._outline.SelfModulate = Colors.Black;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._visuals, "scale", Vector2.One, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NPopupYesNoButton._s), this._baseS, 0.05);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPopupYesNoButton._v), this._baseV, 0.05);
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x001D26A9 File Offset: 0x001D08A9
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NPopupYesNoButton._s, value);
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x001D26C1 File Offset: 0x001D08C1
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NPopupYesNoButton._v, value);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003CF7 RID: 15607 RVA: 0x001D26DC File Offset: 0x001D08DC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NPopupYesNoButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPopupYesNoButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPopupYesNoButton.MethodName.DisconnectHotkeys, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPopupYesNoButton.MethodName.SetText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPopupYesNoButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPopupYesNoButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPopupYesNoButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPopupYesNoButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPopupYesNoButton.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPopupYesNoButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003CF8 RID: 15608 RVA: 0x001D2938 File Offset: 0x001D0B38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPopupYesNoButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPopupYesNoButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPopupYesNoButton.MethodName.DisconnectHotkeys && args.Count == 0)
			{
				this.DisconnectHotkeys();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPopupYesNoButton.MethodName.SetText && args.Count == 1)
			{
				this.SetText(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPopupYesNoButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPopupYesNoButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPopupYesNoButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPopupYesNoButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPopupYesNoButton.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPopupYesNoButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003CF9 RID: 15609 RVA: 0x001D2AEC File Offset: 0x001D0CEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPopupYesNoButton.MethodName._Ready || (ref method) == NPopupYesNoButton.MethodName._ExitTree || (ref method) == NPopupYesNoButton.MethodName.DisconnectHotkeys || (ref method) == NPopupYesNoButton.MethodName.SetText || (ref method) == NPopupYesNoButton.MethodName.OnFocus || (ref method) == NPopupYesNoButton.MethodName.OnUnfocus || (ref method) == NPopupYesNoButton.MethodName.OnPress || (ref method) == NPopupYesNoButton.MethodName.OnRelease || (ref method) == NPopupYesNoButton.MethodName.UpdateShaderS || (ref method) == NPopupYesNoButton.MethodName.UpdateShaderV || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003CFA RID: 15610 RVA: 0x001D2B98 File Offset: 0x001D0D98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPopupYesNoButton.PropertyName.IsYes)
			{
				this.IsYes = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._visuals)
			{
				this._visuals = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._baseS)
			{
				this._baseS = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._baseV)
			{
				this._baseV = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._outlineMaterial)
			{
				this._outlineMaterial = VariantUtils.ConvertTo<CanvasItemMaterial>(ref value);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._isFocused)
			{
				this._isFocused = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._isYes)
			{
				this._isYes = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003CFB RID: 15611 RVA: 0x001D2CF4 File Offset: 0x001D0EF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPopupYesNoButton.PropertyName.IsYes)
			{
				bool isYes = this.IsYes;
				value = VariantUtils.CreateFrom<bool>(ref isYes);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._visuals)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._visuals);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outline);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._image);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._baseS)
			{
				value = VariantUtils.CreateFrom<float>(ref this._baseS);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._baseV)
			{
				value = VariantUtils.CreateFrom<float>(ref this._baseV);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._outlineMaterial)
			{
				value = VariantUtils.CreateFrom<CanvasItemMaterial>(ref this._outlineMaterial);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._isFocused)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isFocused);
				return true;
			}
			if ((ref name) == NPopupYesNoButton.PropertyName._isYes)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isYes);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003CFC RID: 15612 RVA: 0x001D2EB0 File Offset: 0x001D10B0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPopupYesNoButton.PropertyName._visuals, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPopupYesNoButton.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPopupYesNoButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPopupYesNoButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPopupYesNoButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(3L, NPopupYesNoButton.PropertyName._baseS, 0L, "", 4096L, false),
				new PropertyInfo(3L, NPopupYesNoButton.PropertyName._baseV, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPopupYesNoButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPopupYesNoButton.PropertyName._outlineMaterial, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPopupYesNoButton.PropertyName._isFocused, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPopupYesNoButton.PropertyName._isYes, 0L, "", 4096L, false),
				new PropertyInfo(1L, NPopupYesNoButton.PropertyName.IsYes, 0L, "", 4096L, false),
				new PropertyInfo(34L, NPopupYesNoButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003CFD RID: 15613 RVA: 0x001D306C File Offset: 0x001D126C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isYes = NPopupYesNoButton.PropertyName.IsYes;
			bool isYes2 = this.IsYes;
			info.AddProperty(isYes, Variant.From<bool>(ref isYes2));
			info.AddProperty(NPopupYesNoButton.PropertyName._visuals, Variant.From<Control>(ref this._visuals));
			info.AddProperty(NPopupYesNoButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
			info.AddProperty(NPopupYesNoButton.PropertyName._image, Variant.From<Control>(ref this._image));
			info.AddProperty(NPopupYesNoButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NPopupYesNoButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NPopupYesNoButton.PropertyName._baseS, Variant.From<float>(ref this._baseS));
			info.AddProperty(NPopupYesNoButton.PropertyName._baseV, Variant.From<float>(ref this._baseV));
			info.AddProperty(NPopupYesNoButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NPopupYesNoButton.PropertyName._outlineMaterial, Variant.From<CanvasItemMaterial>(ref this._outlineMaterial));
			info.AddProperty(NPopupYesNoButton.PropertyName._isFocused, Variant.From<bool>(ref this._isFocused));
			info.AddProperty(NPopupYesNoButton.PropertyName._isYes, Variant.From<bool>(ref this._isYes));
		}

		/// <inheritdoc />
		// Token: 0x06003CFE RID: 15614 RVA: 0x001D318C File Offset: 0x001D138C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName.IsYes, ref variant))
			{
				this.IsYes = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName._visuals, ref variant2))
			{
				this._visuals = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName._outline, ref variant3))
			{
				this._outline = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName._image, ref variant4))
			{
				this._image = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName._label, ref variant5))
			{
				this._label = variant5.As<MegaLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName._tween, ref variant6))
			{
				this._tween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName._baseS, ref variant7))
			{
				this._baseS = variant7.As<float>();
			}
			Variant variant8;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName._baseV, ref variant8))
			{
				this._baseV = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName._hsv, ref variant9))
			{
				this._hsv = variant9.As<ShaderMaterial>();
			}
			Variant variant10;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName._outlineMaterial, ref variant10))
			{
				this._outlineMaterial = variant10.As<CanvasItemMaterial>();
			}
			Variant variant11;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName._isFocused, ref variant11))
			{
				this._isFocused = variant11.As<bool>();
			}
			Variant variant12;
			if (info.TryGetProperty(NPopupYesNoButton.PropertyName._isYes, ref variant12))
			{
				this._isYes = variant12.As<bool>();
			}
		}

		// Token: 0x040017BD RID: 6077
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040017BE RID: 6078
		private static readonly StringName _s = new StringName("s");

		// Token: 0x040017BF RID: 6079
		private Control _visuals;

		// Token: 0x040017C0 RID: 6080
		private Control _outline;

		// Token: 0x040017C1 RID: 6081
		private Control _image;

		// Token: 0x040017C2 RID: 6082
		private MegaLabel _label;

		// Token: 0x040017C3 RID: 6083
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040017C4 RID: 6084
		private float _baseS;

		// Token: 0x040017C5 RID: 6085
		private float _baseV;

		// Token: 0x040017C6 RID: 6086
		private ShaderMaterial _hsv;

		// Token: 0x040017C7 RID: 6087
		private CanvasItemMaterial _outlineMaterial;

		// Token: 0x040017C8 RID: 6088
		private bool _isFocused;

		// Token: 0x040017C9 RID: 6089
		private static readonly Color _goldOutline = new Color("f0b400");

		// Token: 0x040017CA RID: 6090
		private bool _isYes;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001878 RID: 6264
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005F95 RID: 24469
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005F96 RID: 24470
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'DisconnectHotkeys' method.
			/// </summary>
			// Token: 0x04005F97 RID: 24471
			public static readonly StringName DisconnectHotkeys = "DisconnectHotkeys";

			/// <summary>
			/// Cached name for the 'SetText' method.
			/// </summary>
			// Token: 0x04005F98 RID: 24472
			public static readonly StringName SetText = "SetText";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005F99 RID: 24473
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005F9A RID: 24474
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005F9B RID: 24475
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005F9C RID: 24476
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x04005F9D RID: 24477
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04005F9E RID: 24478
			public static readonly StringName UpdateShaderV = "UpdateShaderV";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001879 RID: 6265
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsYes' property.
			/// </summary>
			// Token: 0x04005F9F RID: 24479
			public static readonly StringName IsYes = "IsYes";

			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005FA0 RID: 24480
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_visuals' field.
			/// </summary>
			// Token: 0x04005FA1 RID: 24481
			public static readonly StringName _visuals = "_visuals";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04005FA2 RID: 24482
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04005FA3 RID: 24483
			public static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005FA4 RID: 24484
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005FA5 RID: 24485
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_baseS' field.
			/// </summary>
			// Token: 0x04005FA6 RID: 24486
			public static readonly StringName _baseS = "_baseS";

			/// <summary>
			/// Cached name for the '_baseV' field.
			/// </summary>
			// Token: 0x04005FA7 RID: 24487
			public static readonly StringName _baseV = "_baseV";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005FA8 RID: 24488
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_outlineMaterial' field.
			/// </summary>
			// Token: 0x04005FA9 RID: 24489
			public static readonly StringName _outlineMaterial = "_outlineMaterial";

			/// <summary>
			/// Cached name for the '_isFocused' field.
			/// </summary>
			// Token: 0x04005FAA RID: 24490
			public static readonly StringName _isFocused = "_isFocused";

			/// <summary>
			/// Cached name for the '_isYes' field.
			/// </summary>
			// Token: 0x04005FAB RID: 24491
			public static readonly StringName _isYes = "_isYes";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200187A RID: 6266
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
