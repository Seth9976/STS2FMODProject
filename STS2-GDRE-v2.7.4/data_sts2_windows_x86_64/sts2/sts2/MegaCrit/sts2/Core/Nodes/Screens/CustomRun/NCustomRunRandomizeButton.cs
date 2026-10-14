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

namespace MegaCrit.Sts2.Core.Nodes.Screens.CustomRun
{
	// Token: 0x02000337 RID: 823
	[ScriptPath("res://src/Core/Nodes/Screens/CustomRun/NCustomRunRandomizeButton.cs")]
	public class NCustomRunRandomizeButton : NButton
	{
		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002E3F RID: 11839 RVA: 0x0014D4D3 File Offset: 0x0014B6D3
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.peek };
			}
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x0014D4E8 File Offset: 0x0014B6E8
		public override void _Ready()
		{
			this.ConnectSignals();
			this._label = base.GetNode<MegaRichTextLabel>("Label");
			this._label.SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.RANDOMIZE").GetFormattedText());
			this._shaderMaterial = (ShaderMaterial)base.GetNode<Control>("Background").Material;
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x0014D550 File Offset: 0x0014B750
		protected override void OnFocus()
		{
			this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._s, 1.1f);
			this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._v, 1.1f);
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x0014D586 File Offset: 0x0014B786
		protected override void OnUnfocus()
		{
			this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._s, 1f);
			this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._v, 1f);
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x0014D5BC File Offset: 0x0014B7BC
		protected override void OnEnable()
		{
			base.OnEnable();
			this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._s, 1f);
			this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._v, 1f);
			this._label.Modulate = Colors.White;
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x0014D614 File Offset: 0x0014B814
		protected override void OnDisable()
		{
			base.OnDisable();
			this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._s, 0f);
			this._shaderMaterial.SetShaderParameter(NCustomRunRandomizeButton._v, 0.5f);
			this._label.Modulate = StsColors.gray;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E45 RID: 11845 RVA: 0x0014D66C File Offset: 0x0014B86C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NCustomRunRandomizeButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunRandomizeButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunRandomizeButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunRandomizeButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCustomRunRandomizeButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E46 RID: 11846 RVA: 0x0014D76C File Offset: 0x0014B96C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCustomRunRandomizeButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunRandomizeButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunRandomizeButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunRandomizeButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCustomRunRandomizeButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002E47 RID: 11847 RVA: 0x0014D83C File Offset: 0x0014BA3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCustomRunRandomizeButton.MethodName._Ready || (ref method) == NCustomRunRandomizeButton.MethodName.OnFocus || (ref method) == NCustomRunRandomizeButton.MethodName.OnUnfocus || (ref method) == NCustomRunRandomizeButton.MethodName.OnEnable || (ref method) == NCustomRunRandomizeButton.MethodName.OnDisable || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002E48 RID: 11848 RVA: 0x0014D89B File Offset: 0x0014BA9B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCustomRunRandomizeButton.PropertyName._shaderMaterial)
			{
				this._shaderMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NCustomRunRandomizeButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002E49 RID: 11849 RVA: 0x0014D8DC File Offset: 0x0014BADC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCustomRunRandomizeButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NCustomRunRandomizeButton.PropertyName._shaderMaterial)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._shaderMaterial);
				return true;
			}
			if ((ref name) == NCustomRunRandomizeButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002E4A RID: 11850 RVA: 0x0014D954 File Offset: 0x0014BB54
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCustomRunRandomizeButton.PropertyName._shaderMaterial, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCustomRunRandomizeButton.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(34L, NCustomRunRandomizeButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002E4B RID: 11851 RVA: 0x0014D9CB File Offset: 0x0014BBCB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCustomRunRandomizeButton.PropertyName._shaderMaterial, Variant.From<ShaderMaterial>(ref this._shaderMaterial));
			info.AddProperty(NCustomRunRandomizeButton.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x06002E4C RID: 11852 RVA: 0x0014DA00 File Offset: 0x0014BC00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCustomRunRandomizeButton.PropertyName._shaderMaterial, ref variant))
			{
				this._shaderMaterial = variant.As<ShaderMaterial>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCustomRunRandomizeButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x040011B1 RID: 4529
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x040011B2 RID: 4530
		[Nullable(1)]
		private static readonly StringName _s = new StringName("s");

		// Token: 0x040011B3 RID: 4531
		[Nullable(1)]
		private ShaderMaterial _shaderMaterial;

		// Token: 0x040011B4 RID: 4532
		[Nullable(1)]
		private MegaRichTextLabel _label;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015C0 RID: 5568
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400539E RID: 21406
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400539F RID: 21407
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040053A0 RID: 21408
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x040053A1 RID: 21409
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x040053A2 RID: 21410
			public new static readonly StringName OnDisable = "OnDisable";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015C1 RID: 5569
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x040053A3 RID: 21411
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_shaderMaterial' field.
			/// </summary>
			// Token: 0x040053A4 RID: 21412
			public static readonly StringName _shaderMaterial = "_shaderMaterial";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x040053A5 RID: 21413
			public static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015C2 RID: 5570
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
