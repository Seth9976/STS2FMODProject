using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x02000287 RID: 647
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NAspectRatioDropdownItem.cs")]
	public class NAspectRatioDropdownItem : NDropdownItem
	{
		// Token: 0x06002280 RID: 8832 RVA: 0x000EC56C File Offset: 0x000EA76C
		public void Init(AspectRatioSetting setAspectRatioSetting)
		{
			this.aspectRatioSetting = setAspectRatioSetting;
			switch (this.aspectRatioSetting)
			{
			case AspectRatioSetting.FourByThree:
				this._label.SetTextAutoSize(new LocString("settings_ui", "ASPECT_RATIO_FOUR_BY_THREE").GetFormattedText());
				return;
			case AspectRatioSetting.SixteenByTen:
				this._label.SetTextAutoSize(new LocString("settings_ui", "ASPECT_RATIO_SIXTEEN_BY_TEN").GetFormattedText());
				return;
			case AspectRatioSetting.SixteenByNine:
				this._label.SetTextAutoSize(new LocString("settings_ui", "ASPECT_RATIO_SIXTEEN_BY_NINE").GetFormattedText());
				return;
			case AspectRatioSetting.TwentyOneByNine:
				this._label.SetTextAutoSize(new LocString("settings_ui", "ASPECT_RATIO_TWENTY_ONE_BY_NINE").GetFormattedText());
				return;
			case AspectRatioSetting.Auto:
				this._label.SetTextAutoSize(new LocString("settings_ui", "ASPECT_RATIO_AUTO").GetFormattedText());
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Invalid Aspect Ratio: ");
			defaultInterpolatedStringHandler.AppendFormatted<AspectRatioSetting>(this.aspectRatioSetting);
			throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002281 RID: 8833 RVA: 0x000EC67C File Offset: 0x000EA87C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NAspectRatioDropdownItem.MethodName.Init, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "setAspectRatioSetting", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002282 RID: 8834 RVA: 0x000EC6E5 File Offset: 0x000EA8E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAspectRatioDropdownItem.MethodName.Init && args.Count == 1)
			{
				this.Init(VariantUtils.ConvertTo<AspectRatioSetting>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002283 RID: 8835 RVA: 0x000EC723 File Offset: 0x000EA923
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAspectRatioDropdownItem.MethodName.Init || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002284 RID: 8836 RVA: 0x000EC73B File Offset: 0x000EA93B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAspectRatioDropdownItem.PropertyName.aspectRatioSetting)
			{
				this.aspectRatioSetting = VariantUtils.ConvertTo<AspectRatioSetting>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002285 RID: 8837 RVA: 0x000EC760 File Offset: 0x000EA960
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAspectRatioDropdownItem.PropertyName.aspectRatioSetting)
			{
				value = VariantUtils.CreateFrom<AspectRatioSetting>(ref this.aspectRatioSetting);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002286 RID: 8838 RVA: 0x000EC78C File Offset: 0x000EA98C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NAspectRatioDropdownItem.PropertyName.aspectRatioSetting, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002287 RID: 8839 RVA: 0x000EC7C0 File Offset: 0x000EA9C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAspectRatioDropdownItem.PropertyName.aspectRatioSetting, Variant.From<AspectRatioSetting>(ref this.aspectRatioSetting));
		}

		/// <inheritdoc />
		// Token: 0x06002288 RID: 8840 RVA: 0x000EC7E0 File Offset: 0x000EA9E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAspectRatioDropdownItem.PropertyName.aspectRatioSetting, ref variant))
			{
				this.aspectRatioSetting = variant.As<AspectRatioSetting>();
			}
		}

		// Token: 0x04000D68 RID: 3432
		public AspectRatioSetting aspectRatioSetting;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001348 RID: 4936
		public new class MethodName : NDropdownItem.MethodName
		{
			/// <summary>
			/// Cached name for the 'Init' method.
			/// </summary>
			// Token: 0x04004AE4 RID: 19172
			public static readonly StringName Init = "Init";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001349 RID: 4937
		public new class PropertyName : NDropdownItem.PropertyName
		{
			/// <summary>
			/// Cached name for the 'aspectRatioSetting' field.
			/// </summary>
			// Token: 0x04004AE5 RID: 19173
			public static readonly StringName aspectRatioSetting = "aspectRatioSetting";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200134A RID: 4938
		public new class SignalName : NDropdownItem.SignalName
		{
		}
	}
}
