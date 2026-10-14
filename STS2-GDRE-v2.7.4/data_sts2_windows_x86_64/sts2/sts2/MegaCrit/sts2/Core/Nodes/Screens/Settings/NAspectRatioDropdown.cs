using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	/// <summary>
	/// The display mode dropdown in the SettingsScreen.
	/// </summary>
	// Token: 0x02000286 RID: 646
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NAspectRatioDropdown.cs")]
	public class NAspectRatioDropdown : NSettingsDropdown, IResettableSettingNode
	{
		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x0600226F RID: 8815 RVA: 0x000EBF66 File Offset: 0x000EA166
		[Nullable(1)]
		private static string AspectRatioDropdownItemScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("ui/aspect_ratio_dropdown_item");
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002270 RID: 8816 RVA: 0x000EBF72 File Offset: 0x000EA172
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NAspectRatioDropdown.AspectRatioDropdownItemScenePath);
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000EBF80 File Offset: 0x000EA180
		public override void _Ready()
		{
			this.ConnectSignals();
			base.ClearDropdownItems();
			this.AddDropdownItem(AspectRatioSetting.Auto);
			this.AddDropdownItem(AspectRatioSetting.FourByThree);
			this.AddDropdownItem(AspectRatioSetting.SixteenByTen);
			this.AddDropdownItem(AspectRatioSetting.SixteenByNine);
			this.AddDropdownItem(AspectRatioSetting.TwentyOneByNine);
			this._dropdownItems.GetParent<NDropdownContainer>().RefreshLayout();
			this.SetFromSettings();
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000EBFD2 File Offset: 0x000EA1D2
		public void SetFromSettings()
		{
			this._currentAspectRatioSetting = SaveManager.Instance.SettingsSave.AspectRatioSetting;
			this._currentOptionLabel.SetTextAutoSize(NAspectRatioDropdown.GetAspectRatioSettingString(this._currentAspectRatioSetting));
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000EC000 File Offset: 0x000EA200
		private void AddDropdownItem(AspectRatioSetting aspectRatioSetting)
		{
			NAspectRatioDropdownItem naspectRatioDropdownItem = ResourceLoader.Load<PackedScene>(NAspectRatioDropdown.AspectRatioDropdownItemScenePath, null, 1L).Instantiate<NAspectRatioDropdownItem>(0L);
			this._dropdownItems.AddChildSafely(naspectRatioDropdownItem);
			naspectRatioDropdownItem.Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
			naspectRatioDropdownItem.Init(aspectRatioSetting);
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000EC054 File Offset: 0x000EA254
		[NullableContext(1)]
		private void OnDropdownItemSelected(NDropdownItem nDropdownItem)
		{
			NAspectRatioDropdownItem naspectRatioDropdownItem = (NAspectRatioDropdownItem)nDropdownItem;
			if (naspectRatioDropdownItem.aspectRatioSetting == this._currentAspectRatioSetting)
			{
				return;
			}
			base.CloseDropdown();
			SaveManager.Instance.SettingsSave.AspectRatioSetting = naspectRatioDropdownItem.aspectRatioSetting;
			this.SetFromSettings();
			NGame.Instance.ApplyDisplaySettings();
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000EC0A4 File Offset: 0x000EA2A4
		[NullableContext(1)]
		private static string GetAspectRatioSettingString(AspectRatioSetting aspectRatioSettingString)
		{
			switch (aspectRatioSettingString)
			{
			case AspectRatioSetting.FourByThree:
				return new LocString("settings_ui", "ASPECT_RATIO_FOUR_BY_THREE").GetFormattedText();
			case AspectRatioSetting.SixteenByTen:
				return new LocString("settings_ui", "ASPECT_RATIO_SIXTEEN_BY_TEN").GetFormattedText();
			case AspectRatioSetting.SixteenByNine:
				return new LocString("settings_ui", "ASPECT_RATIO_SIXTEEN_BY_NINE").GetFormattedText();
			case AspectRatioSetting.TwentyOneByNine:
				return new LocString("settings_ui", "ASPECT_RATIO_TWENTY_ONE_BY_NINE").GetFormattedText();
			case AspectRatioSetting.Auto:
				return new LocString("settings_ui", "ASPECT_RATIO_AUTO").GetFormattedText();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Invalid Aspect Ratio: ");
			defaultInterpolatedStringHandler.AppendFormatted<AspectRatioSetting>(aspectRatioSettingString);
			throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002276 RID: 8822 RVA: 0x000EC164 File Offset: 0x000EA364
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NAspectRatioDropdown.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAspectRatioDropdown.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAspectRatioDropdown.MethodName.AddDropdownItem, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "aspectRatioSetting", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAspectRatioDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "nDropdownItem", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NAspectRatioDropdown.MethodName.GetAspectRatioSettingString, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "aspectRatioSettingString", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002277 RID: 8823 RVA: 0x000EC2E0 File Offset: 0x000EA4E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAspectRatioDropdown.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAspectRatioDropdown.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAspectRatioDropdown.MethodName.AddDropdownItem && args.Count == 1)
			{
				this.AddDropdownItem(VariantUtils.ConvertTo<AspectRatioSetting>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAspectRatioDropdown.MethodName.OnDropdownItemSelected && args.Count == 1)
			{
				this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAspectRatioDropdown.MethodName.GetAspectRatioSettingString && args.Count == 1)
			{
				string aspectRatioSettingString = NAspectRatioDropdown.GetAspectRatioSettingString(VariantUtils.ConvertTo<AspectRatioSetting>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref aspectRatioSettingString);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000EC3E0 File Offset: 0x000EA5E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAspectRatioDropdown.MethodName.GetAspectRatioSettingString && args.Count == 1)
			{
				string aspectRatioSettingString = NAspectRatioDropdown.GetAspectRatioSettingString(VariantUtils.ConvertTo<AspectRatioSetting>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref aspectRatioSettingString);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002279 RID: 8825 RVA: 0x000EC430 File Offset: 0x000EA630
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAspectRatioDropdown.MethodName._Ready || (ref method) == NAspectRatioDropdown.MethodName.SetFromSettings || (ref method) == NAspectRatioDropdown.MethodName.AddDropdownItem || (ref method) == NAspectRatioDropdown.MethodName.OnDropdownItemSelected || (ref method) == NAspectRatioDropdown.MethodName.GetAspectRatioSettingString || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600227A RID: 8826 RVA: 0x000EC48F File Offset: 0x000EA68F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAspectRatioDropdown.PropertyName._currentAspectRatioSetting)
			{
				this._currentAspectRatioSetting = VariantUtils.ConvertTo<AspectRatioSetting>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600227B RID: 8827 RVA: 0x000EC4B4 File Offset: 0x000EA6B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAspectRatioDropdown.PropertyName._currentAspectRatioSetting)
			{
				value = VariantUtils.CreateFrom<AspectRatioSetting>(ref this._currentAspectRatioSetting);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600227C RID: 8828 RVA: 0x000EC4E0 File Offset: 0x000EA6E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NAspectRatioDropdown.PropertyName._currentAspectRatioSetting, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600227D RID: 8829 RVA: 0x000EC514 File Offset: 0x000EA714
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAspectRatioDropdown.PropertyName._currentAspectRatioSetting, Variant.From<AspectRatioSetting>(ref this._currentAspectRatioSetting));
		}

		/// <inheritdoc />
		// Token: 0x0600227E RID: 8830 RVA: 0x000EC534 File Offset: 0x000EA734
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAspectRatioDropdown.PropertyName._currentAspectRatioSetting, ref variant))
			{
				this._currentAspectRatioSetting = variant.As<AspectRatioSetting>();
			}
		}

		// Token: 0x04000D67 RID: 3431
		private AspectRatioSetting _currentAspectRatioSetting;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001345 RID: 4933
		public new class MethodName : NSettingsDropdown.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004ADE RID: 19166
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004ADF RID: 19167
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'AddDropdownItem' method.
			/// </summary>
			// Token: 0x04004AE0 RID: 19168
			public static readonly StringName AddDropdownItem = "AddDropdownItem";

			/// <summary>
			/// Cached name for the 'OnDropdownItemSelected' method.
			/// </summary>
			// Token: 0x04004AE1 RID: 19169
			public static readonly StringName OnDropdownItemSelected = "OnDropdownItemSelected";

			/// <summary>
			/// Cached name for the 'GetAspectRatioSettingString' method.
			/// </summary>
			// Token: 0x04004AE2 RID: 19170
			public static readonly StringName GetAspectRatioSettingString = "GetAspectRatioSettingString";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001346 RID: 4934
		public new class PropertyName : NSettingsDropdown.PropertyName
		{
			/// <summary>
			/// Cached name for the '_currentAspectRatioSetting' field.
			/// </summary>
			// Token: 0x04004AE3 RID: 19171
			public static readonly StringName _currentAspectRatioSetting = "_currentAspectRatioSetting";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001347 RID: 4935
		public new class SignalName : NSettingsDropdown.SignalName
		{
		}
	}
}
