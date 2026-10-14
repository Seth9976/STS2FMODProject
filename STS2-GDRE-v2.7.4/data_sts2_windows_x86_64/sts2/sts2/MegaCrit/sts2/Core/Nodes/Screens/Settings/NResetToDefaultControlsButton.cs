using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002AD RID: 685
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NResetToDefaultControlsButton.cs")]
	public class NResetToDefaultControlsButton : NSettingsButton
	{
		// Token: 0x06002442 RID: 9282 RVA: 0x000F60C0 File Offset: 0x000F42C0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._image = base.GetNode<Control>("Image");
			this._label = base.GetNode<MegaRichTextLabel>("Label");
			this._label.SetTextAutoSize(new LocString("settings_ui", "INPUT_SETTINGS.RESET_TO_DEFAULT").GetFormattedText());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002443 RID: 9283 RVA: 0x000F6120 File Offset: 0x000F4320
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NResetToDefaultControlsButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002444 RID: 9284 RVA: 0x000F6164 File Offset: 0x000F4364
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NResetToDefaultControlsButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002445 RID: 9285 RVA: 0x000F6194 File Offset: 0x000F4394
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NResetToDefaultControlsButton.MethodName._Ready || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002446 RID: 9286 RVA: 0x000F61AC File Offset: 0x000F43AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NResetToDefaultControlsButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NResetToDefaultControlsButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002447 RID: 9287 RVA: 0x000F61EC File Offset: 0x000F43EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NResetToDefaultControlsButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._image);
				return true;
			}
			if ((ref name) == NResetToDefaultControlsButton.PropertyName._label)
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
		// Token: 0x06002448 RID: 9288 RVA: 0x000F6244 File Offset: 0x000F4444
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NResetToDefaultControlsButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NResetToDefaultControlsButton.PropertyName._label, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002449 RID: 9289 RVA: 0x000F629A File Offset: 0x000F449A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NResetToDefaultControlsButton.PropertyName._image, Variant.From<Control>(ref this._image));
			info.AddProperty(NResetToDefaultControlsButton.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x0600244A RID: 9290 RVA: 0x000F62D0 File Offset: 0x000F44D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NResetToDefaultControlsButton.PropertyName._image, ref variant))
			{
				this._image = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NResetToDefaultControlsButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x04000DAE RID: 3502
		[Nullable(1)]
		private Control _image;

		// Token: 0x04000DAF RID: 3503
		[Nullable(1)]
		private MegaRichTextLabel _label;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013BF RID: 5055
		public new class MethodName : NSettingsButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BB0 RID: 19376
			public new static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013C0 RID: 5056
		public new class PropertyName : NSettingsButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004BB1 RID: 19377
			public new static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004BB2 RID: 19378
			public new static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013C1 RID: 5057
		public new class SignalName : NSettingsButton.SignalName
		{
		}
	}
}
