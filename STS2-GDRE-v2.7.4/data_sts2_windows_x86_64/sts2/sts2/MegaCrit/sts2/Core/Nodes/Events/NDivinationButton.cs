using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Events
{
	// Token: 0x020003BC RID: 956
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Events/NDivinationButton.cs")]
	public class NDivinationButton : NButton
	{
		// Token: 0x06003983 RID: 14723 RVA: 0x001B3D50 File Offset: 0x001B1F50
		public override void _Ready()
		{
			this.ConnectSignals();
			this._hsv = (ShaderMaterial)base.GetNode<TextureRect>("%ButtonImage").Material;
			this._outline = base.GetNode<Control>("%Outline");
			this._label = base.GetNode<MegaLabel>("%Label");
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06003984 RID: 14724 RVA: 0x001B3DAF File Offset: 0x001B1FAF
		protected override string[] Hotkeys
		{
			get
			{
				return this._customHotkeys;
			}
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x001B3DB7 File Offset: 0x001B1FB7
		public void SetLabel(LocString loc)
		{
			this._label.SetTextAutoSize(loc.GetFormattedText());
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x001B3DCC File Offset: 0x001B1FCC
		protected override void OnFocus()
		{
			if (this._outline.Visible)
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
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NDivinationButton._s), 1f, 0.05).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NDivinationButton._v), 1f, 0.05).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x001B3EAC File Offset: 0x001B20AC
		protected override void OnUnfocus()
		{
			if (this._outline.Visible)
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
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NDivinationButton._s), 0.9f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NDivinationButton._v), 0.8f, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x001B3F8C File Offset: 0x001B218C
		private void UpdateShaderS(float value)
		{
			this._hsv.SetShaderParameter(NDivinationButton._s, value);
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x001B3FA4 File Offset: 0x001B21A4
		private void UpdateShaderV(float value)
		{
			this._hsv.SetShaderParameter(NDivinationButton._v, value);
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x001B3FBC File Offset: 0x001B21BC
		public void SetActive(bool isActive)
		{
			this._outline.Visible = isActive;
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x001B3FCA File Offset: 0x001B21CA
		public void SetHotkeys(string[] hotkeys)
		{
			base.UnregisterHotkeys();
			this._customHotkeys = hotkeys;
			base.RegisterHotkeys();
			base.UpdateControllerButton();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600398C RID: 14732 RVA: 0x001B3FE8 File Offset: 0x001B21E8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NDivinationButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDivinationButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDivinationButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDivinationButton.MethodName.UpdateShaderS, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDivinationButton.MethodName.UpdateShaderV, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDivinationButton.MethodName.SetActive, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isActive", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDivinationButton.MethodName.SetHotkeys, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(34L, "hotkeys", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600398D RID: 14733 RVA: 0x001B41DC File Offset: 0x001B23DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDivinationButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDivinationButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDivinationButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDivinationButton.MethodName.UpdateShaderS && args.Count == 1)
			{
				this.UpdateShaderS(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDivinationButton.MethodName.UpdateShaderV && args.Count == 1)
			{
				this.UpdateShaderV(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDivinationButton.MethodName.SetActive && args.Count == 1)
			{
				this.SetActive(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDivinationButton.MethodName.SetHotkeys && args.Count == 1)
			{
				this.SetHotkeys(VariantUtils.ConvertTo<string[]>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600398E RID: 14734 RVA: 0x001B4330 File Offset: 0x001B2530
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDivinationButton.MethodName._Ready || (ref method) == NDivinationButton.MethodName.OnFocus || (ref method) == NDivinationButton.MethodName.OnUnfocus || (ref method) == NDivinationButton.MethodName.UpdateShaderS || (ref method) == NDivinationButton.MethodName.UpdateShaderV || (ref method) == NDivinationButton.MethodName.SetActive || (ref method) == NDivinationButton.MethodName.SetHotkeys || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600398F RID: 14735 RVA: 0x001B43B0 File Offset: 0x001B25B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDivinationButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NDivinationButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NDivinationButton.PropertyName._outlineTween)
			{
				this._outlineTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NDivinationButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NDivinationButton.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NDivinationButton.PropertyName._customHotkeys)
			{
				this._customHotkeys = VariantUtils.ConvertTo<string[]>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003990 RID: 14736 RVA: 0x001B4468 File Offset: 0x001B2668
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDivinationButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NDivinationButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NDivinationButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NDivinationButton.PropertyName._outlineTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._outlineTween);
				return true;
			}
			if ((ref name) == NDivinationButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NDivinationButton.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outline);
				return true;
			}
			if ((ref name) == NDivinationButton.PropertyName._customHotkeys)
			{
				value = VariantUtils.CreateFrom<string[]>(ref this._customHotkeys);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003991 RID: 14737 RVA: 0x001B4560 File Offset: 0x001B2760
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDivinationButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDivinationButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDivinationButton.PropertyName._outlineTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDivinationButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDivinationButton.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(34L, NDivinationButton.PropertyName._customHotkeys, 0L, "", 4096L, false),
				new PropertyInfo(34L, NDivinationButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003992 RID: 14738 RVA: 0x001B465C File Offset: 0x001B285C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDivinationButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NDivinationButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NDivinationButton.PropertyName._outlineTween, Variant.From<Tween>(ref this._outlineTween));
			info.AddProperty(NDivinationButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NDivinationButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
			info.AddProperty(NDivinationButton.PropertyName._customHotkeys, Variant.From<string[]>(ref this._customHotkeys));
		}

		/// <inheritdoc />
		// Token: 0x06003993 RID: 14739 RVA: 0x001B46F4 File Offset: 0x001B28F4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDivinationButton.PropertyName._hsv, ref variant))
			{
				this._hsv = variant.As<ShaderMaterial>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDivinationButton.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDivinationButton.PropertyName._outlineTween, ref variant3))
			{
				this._outlineTween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDivinationButton.PropertyName._label, ref variant4))
			{
				this._label = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDivinationButton.PropertyName._outline, ref variant5))
			{
				this._outline = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDivinationButton.PropertyName._customHotkeys, ref variant6))
			{
				this._customHotkeys = variant6.As<string[]>();
			}
		}

		// Token: 0x0400165E RID: 5726
		private static readonly StringName _v = new StringName("v");

		// Token: 0x0400165F RID: 5727
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04001660 RID: 5728
		private ShaderMaterial _hsv;

		// Token: 0x04001661 RID: 5729
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001662 RID: 5730
		[Nullable(2)]
		private Tween _outlineTween;

		// Token: 0x04001663 RID: 5731
		private MegaLabel _label;

		// Token: 0x04001664 RID: 5732
		private Control _outline;

		// Token: 0x04001665 RID: 5733
		private string[] _customHotkeys = Array.Empty<string>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017CE RID: 6094
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005CC4 RID: 23748
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005CC5 RID: 23749
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005CC6 RID: 23750
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'UpdateShaderS' method.
			/// </summary>
			// Token: 0x04005CC7 RID: 23751
			public static readonly StringName UpdateShaderS = "UpdateShaderS";

			/// <summary>
			/// Cached name for the 'UpdateShaderV' method.
			/// </summary>
			// Token: 0x04005CC8 RID: 23752
			public static readonly StringName UpdateShaderV = "UpdateShaderV";

			/// <summary>
			/// Cached name for the 'SetActive' method.
			/// </summary>
			// Token: 0x04005CC9 RID: 23753
			public static readonly StringName SetActive = "SetActive";

			/// <summary>
			/// Cached name for the 'SetHotkeys' method.
			/// </summary>
			// Token: 0x04005CCA RID: 23754
			public static readonly StringName SetHotkeys = "SetHotkeys";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017CF RID: 6095
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005CCB RID: 23755
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x04005CCC RID: 23756
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005CCD RID: 23757
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_outlineTween' field.
			/// </summary>
			// Token: 0x04005CCE RID: 23758
			public static readonly StringName _outlineTween = "_outlineTween";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005CCF RID: 23759
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04005CD0 RID: 23760
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_customHotkeys' field.
			/// </summary>
			// Token: 0x04005CD1 RID: 23761
			public static readonly StringName _customHotkeys = "_customHotkeys";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017D0 RID: 6096
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
