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
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	/// <summary>
	/// The Display selection dropdown in the Graphics settings.
	/// </summary>
	// Token: 0x0200028E RID: 654
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NDisplayDropdown.cs")]
	public class NDisplayDropdown : NSettingsDropdown
	{
		// Token: 0x060022C9 RID: 8905 RVA: 0x000ED770 File Offset: 0x000EB970
		public override void _Ready()
		{
			this.ConnectSignals();
			NGame.Instance.Connect(NGame.SignalName.WindowChange, Callable.From<bool>(new Action<bool>(this.OnWindowChange)), 0U);
			this.OnWindowChange(SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto);
			base.ClearDropdownItems();
			for (int i = 0; i < DisplayServer.GetScreenCount(); i++)
			{
				NDisplayDropdownItem ndisplayDropdownItem = this._dropdownItemScene.Instantiate<NDisplayDropdownItem>(0L);
				this._dropdownItems.AddChildSafely(ndisplayDropdownItem);
				ndisplayDropdownItem.Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
				ndisplayDropdownItem.Init(i);
			}
			this._dropdownItems.GetParent<NDropdownContainer>().RefreshLayout();
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x000ED822 File Offset: 0x000EBA22
		public override void _Notification(int what)
		{
			if ((long)what == 1012L && base.IsNodeReady())
			{
				this.OnWindowChange(SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto);
			}
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000ED850 File Offset: 0x000EBA50
		private void OnWindowChange(bool _)
		{
			long num = (long)base.GetWindow().CurrentScreen;
			if (num != (long)this._currentDisplayIndex)
			{
				this._currentDisplayIndex = (int)num;
				NDisplayDropdown._optionString.Add("MonitorIndex", this._currentDisplayIndex);
				this._currentOptionLabel.SetTextAutoSize(NDisplayDropdown._optionString.GetFormattedText());
				SaveManager.Instance.SettingsSave.TargetDisplay = this._currentDisplayIndex;
				NResolutionDropdown instance = NResolutionDropdown.Instance;
				if (instance != null && instance.IsNodeReady())
				{
					NResolutionDropdown.Instance.RefreshCurrentlySelectedResolution();
					NResolutionDropdown.Instance.PopulateDropdownItems();
				}
			}
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000ED8E8 File Offset: 0x000EBAE8
		[NullableContext(1)]
		private void OnDropdownItemSelected(NDropdownItem nDropdownItem)
		{
			SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
			NDisplayDropdownItem ndisplayDropdownItem = (NDisplayDropdownItem)nDropdownItem;
			if (ndisplayDropdownItem.displayIndex == this._currentDisplayIndex)
			{
				return;
			}
			base.CloseDropdown();
			settingsSave.TargetDisplay = ndisplayDropdownItem.displayIndex;
			if (!settingsSave.Fullscreen && !PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen())
			{
				Vector2I vector2I = DisplayServer.ScreenGetSize(ndisplayDropdownItem.displayIndex);
				settingsSave.WindowPosition = vector2I / 8;
				settingsSave.WindowSize = new Vector2I((int)((float)vector2I.X * 0.75f), (int)((float)vector2I.Y * 0.75f));
			}
			NResolutionDropdown.Instance.RefreshCurrentlySelectedResolution();
			NResolutionDropdown.Instance.PopulateDropdownItems();
			NGame.Instance.ApplyDisplaySettings();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022CD RID: 8909 RVA: 0x000ED99C File Offset: 0x000EBB9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NDisplayDropdown.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDisplayDropdown.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDisplayDropdown.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "_", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDisplayDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "nDropdownItem", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022CE RID: 8910 RVA: 0x000EDAE8 File Offset: 0x000EBCE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDisplayDropdown.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDisplayDropdown.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDisplayDropdown.MethodName.OnWindowChange && args.Count == 1)
			{
				this.OnWindowChange(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDisplayDropdown.MethodName.OnDropdownItemSelected && args.Count == 1)
			{
				this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060022CF RID: 8911 RVA: 0x000EDBBC File Offset: 0x000EBDBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDisplayDropdown.MethodName._Ready || (ref method) == NDisplayDropdown.MethodName._Notification || (ref method) == NDisplayDropdown.MethodName.OnWindowChange || (ref method) == NDisplayDropdown.MethodName.OnDropdownItemSelected || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060022D0 RID: 8912 RVA: 0x000EDC0C File Offset: 0x000EBE0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDisplayDropdown.PropertyName._dropdownItemScene)
			{
				this._dropdownItemScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NDisplayDropdown.PropertyName._currentDisplayIndex)
			{
				this._currentDisplayIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060022D1 RID: 8913 RVA: 0x000EDC4C File Offset: 0x000EBE4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDisplayDropdown.PropertyName._dropdownItemScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._dropdownItemScene);
				return true;
			}
			if ((ref name) == NDisplayDropdown.PropertyName._currentDisplayIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._currentDisplayIndex);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022D2 RID: 8914 RVA: 0x000EDCA4 File Offset: 0x000EBEA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDisplayDropdown.PropertyName._dropdownItemScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(2L, NDisplayDropdown.PropertyName._currentDisplayIndex, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022D3 RID: 8915 RVA: 0x000EDCFA File Offset: 0x000EBEFA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDisplayDropdown.PropertyName._dropdownItemScene, Variant.From<PackedScene>(ref this._dropdownItemScene));
			info.AddProperty(NDisplayDropdown.PropertyName._currentDisplayIndex, Variant.From<int>(ref this._currentDisplayIndex));
		}

		/// <inheritdoc />
		// Token: 0x060022D4 RID: 8916 RVA: 0x000EDD30 File Offset: 0x000EBF30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDisplayDropdown.PropertyName._dropdownItemScene, ref variant))
			{
				this._dropdownItemScene = variant.As<PackedScene>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDisplayDropdown.PropertyName._currentDisplayIndex, ref variant2))
			{
				this._currentDisplayIndex = variant2.As<int>();
			}
		}

		// Token: 0x04000D6F RID: 3439
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _dropdownItemScene;

		// Token: 0x04000D70 RID: 3440
		[Nullable(1)]
		private static readonly LocString _optionString = new LocString("settings_ui", "DISPLAY_DROPDOWN_OPTION");

		// Token: 0x04000D71 RID: 3441
		private int _currentDisplayIndex = -1;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200135D RID: 4957
		public new class MethodName : NSettingsDropdown.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004AFC RID: 19196
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x04004AFD RID: 19197
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04004AFE RID: 19198
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'OnDropdownItemSelected' method.
			/// </summary>
			// Token: 0x04004AFF RID: 19199
			public static readonly StringName OnDropdownItemSelected = "OnDropdownItemSelected";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200135E RID: 4958
		public new class PropertyName : NSettingsDropdown.PropertyName
		{
			/// <summary>
			/// Cached name for the '_dropdownItemScene' field.
			/// </summary>
			// Token: 0x04004B00 RID: 19200
			public static readonly StringName _dropdownItemScene = "_dropdownItemScene";

			/// <summary>
			/// Cached name for the '_currentDisplayIndex' field.
			/// </summary>
			// Token: 0x04004B01 RID: 19201
			public static readonly StringName _currentDisplayIndex = "_currentDisplayIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200135F RID: 4959
		public new class SignalName : NSettingsDropdown.SignalName
		{
		}
	}
}
