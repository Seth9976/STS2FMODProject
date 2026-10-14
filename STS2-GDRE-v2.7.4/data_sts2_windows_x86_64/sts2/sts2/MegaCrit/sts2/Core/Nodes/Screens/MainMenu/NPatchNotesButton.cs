using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x0200030F RID: 783
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NPatchNotesButton.cs")]
	public class NPatchNotesButton : NButton
	{
		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002B44 RID: 11076 RVA: 0x00131197 File Offset: 0x0012F397
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.accept };
			}
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x001311AC File Offset: 0x0012F3AC
		public override void _Ready()
		{
			this.ConnectSignals();
			this._icon = base.GetNode<TextureRect>("Icon");
			this._hsv = (ShaderMaterial)this._icon.Material;
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x001311E0 File Offset: 0x0012F3E0
		protected override void OnFocus()
		{
			base.OnFocus();
			this._hsv.SetShaderParameter(NPatchNotesButton._v, 1.2f);
			this._icon.RotationDegrees = 5f;
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x00131212 File Offset: 0x0012F412
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this._hsv.SetShaderParameter(NPatchNotesButton._v, 1f);
			this._icon.RotationDegrees = 0f;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B48 RID: 11080 RVA: 0x00131244 File Offset: 0x0012F444
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NPatchNotesButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPatchNotesButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPatchNotesButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B49 RID: 11081 RVA: 0x001312E8 File Offset: 0x0012F4E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPatchNotesButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPatchNotesButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPatchNotesButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002B4A RID: 11082 RVA: 0x0013136D File Offset: 0x0012F56D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPatchNotesButton.MethodName._Ready || (ref method) == NPatchNotesButton.MethodName.OnFocus || (ref method) == NPatchNotesButton.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002B4B RID: 11083 RVA: 0x001313A3 File Offset: 0x0012F5A3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPatchNotesButton.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NPatchNotesButton.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002B4C RID: 11084 RVA: 0x001313E4 File Offset: 0x0012F5E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPatchNotesButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NPatchNotesButton.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NPatchNotesButton.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._icon);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002B4D RID: 11085 RVA: 0x0013145C File Offset: 0x0012F65C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NPatchNotesButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesButton.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPatchNotesButton.PropertyName._icon, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002B4E RID: 11086 RVA: 0x001314D3 File Offset: 0x0012F6D3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPatchNotesButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NPatchNotesButton.PropertyName._icon, Variant.From<Control>(ref this._icon));
		}

		/// <inheritdoc />
		// Token: 0x06002B4F RID: 11087 RVA: 0x00131508 File Offset: 0x0012F708
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPatchNotesButton.PropertyName._hsv, ref variant))
			{
				this._hsv = variant.As<ShaderMaterial>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPatchNotesButton.PropertyName._icon, ref variant2))
			{
				this._icon = variant2.As<Control>();
			}
		}

		// Token: 0x04001042 RID: 4162
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04001043 RID: 4163
		[Nullable(1)]
		private ShaderMaterial _hsv;

		// Token: 0x04001044 RID: 4164
		[Nullable(1)]
		private Control _icon;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001526 RID: 5414
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040050C9 RID: 20681
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040050CA RID: 20682
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040050CB RID: 20683
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001527 RID: 5415
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x040050CC RID: 20684
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x040050CD RID: 20685
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x040050CE RID: 20686
			public static readonly StringName _icon = "_icon";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001528 RID: 5416
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
