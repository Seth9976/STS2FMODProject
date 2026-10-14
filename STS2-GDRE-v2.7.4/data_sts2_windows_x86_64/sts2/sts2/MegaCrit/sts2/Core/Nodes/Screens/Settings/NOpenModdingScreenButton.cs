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
	// Token: 0x020002A6 RID: 678
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NOpenModdingScreenButton.cs")]
	public class NOpenModdingScreenButton : NSettingsButton
	{
		// Token: 0x060023EB RID: 9195 RVA: 0x000F3E74 File Offset: 0x000F2074
		public override void _Ready()
		{
			this.ConnectSignals();
			base.PivotOffset = base.Size * 0.5f;
			this._image = base.GetNode<TextureRect>("Image");
			this._label = base.GetNode<MegaLabel>("Label");
			this._label.SetTextAutoSize(new LocString("settings_ui", "MODDING_SCREEN_BUTTON_LABEL").GetFormattedText());
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x000F3EE8 File Offset: 0x000F20E8
		protected override void OnEnable()
		{
			base.OnEnable();
			this._image.Modulate = Colors.White;
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x000F3F00 File Offset: 0x000F2100
		protected override void OnDisable()
		{
			base.OnDisable();
			this._image.Modulate = Colors.Gray;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060023EE RID: 9198 RVA: 0x000F3F18 File Offset: 0x000F2118
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NOpenModdingScreenButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOpenModdingScreenButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NOpenModdingScreenButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060023EF RID: 9199 RVA: 0x000F3FBC File Offset: 0x000F21BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NOpenModdingScreenButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOpenModdingScreenButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NOpenModdingScreenButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060023F0 RID: 9200 RVA: 0x000F4041 File Offset: 0x000F2241
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NOpenModdingScreenButton.MethodName._Ready || (ref method) == NOpenModdingScreenButton.MethodName.OnEnable || (ref method) == NOpenModdingScreenButton.MethodName.OnDisable || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060023F1 RID: 9201 RVA: 0x000F4077 File Offset: 0x000F2277
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NOpenModdingScreenButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NOpenModdingScreenButton.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060023F2 RID: 9202 RVA: 0x000F40B8 File Offset: 0x000F22B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NOpenModdingScreenButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
				return true;
			}
			if ((ref name) == NOpenModdingScreenButton.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060023F3 RID: 9203 RVA: 0x000F4110 File Offset: 0x000F2310
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NOpenModdingScreenButton.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NOpenModdingScreenButton.PropertyName._label, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060023F4 RID: 9204 RVA: 0x000F4166 File Offset: 0x000F2366
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NOpenModdingScreenButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
			info.AddProperty(NOpenModdingScreenButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x060023F5 RID: 9205 RVA: 0x000F419C File Offset: 0x000F239C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NOpenModdingScreenButton.PropertyName._image, ref variant))
			{
				this._image = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NOpenModdingScreenButton.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
		}

		// Token: 0x04000D9B RID: 3483
		[Nullable(1)]
		private TextureRect _image;

		// Token: 0x04000D9C RID: 3484
		[Nullable(1)]
		private MegaLabel _label;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013A8 RID: 5032
		public new class MethodName : NSettingsButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B7C RID: 19324
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04004B7D RID: 19325
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04004B7E RID: 19326
			public new static readonly StringName OnDisable = "OnDisable";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013A9 RID: 5033
		public new class PropertyName : NSettingsButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004B7F RID: 19327
			public new static readonly StringName _image = "_image";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004B80 RID: 19328
			public new static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013AA RID: 5034
		public new class SignalName : NSettingsButton.SignalName
		{
		}
	}
}
