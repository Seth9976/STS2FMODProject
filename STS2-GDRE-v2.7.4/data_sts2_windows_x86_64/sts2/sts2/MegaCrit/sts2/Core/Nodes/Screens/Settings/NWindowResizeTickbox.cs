using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002C8 RID: 712
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NWindowResizeTickbox.cs")]
	public class NWindowResizeTickbox : NSettingsTickbox, IResettableSettingNode
	{
		// Token: 0x060025B4 RID: 9652 RVA: 0x000FEFA4 File Offset: 0x000FD1A4
		public override void _Ready()
		{
			this.ConnectSignals();
			this._settingsScreen = this.GetAncestorOfType<NSettingsScreen>();
			NGame.Instance.Connect(NGame.SignalName.WindowChange, Callable.From<bool>(new Action<bool>(this.OnWindowChange)), 0U);
			this.SetFromSettings();
			this.RefreshEnabled();
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000FEFF1 File Offset: 0x000FD1F1
		protected override void OnTick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_WINDOW_RESIZING_ON"));
			SaveManager.Instance.SettingsSave.ResizeWindows = true;
			NGame.Instance.ApplyDisplaySettings();
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x000FF027 File Offset: 0x000FD227
		protected override void OnUntick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_WINDOW_RESIZING_OFF"));
			SaveManager.Instance.SettingsSave.ResizeWindows = false;
			NGame.Instance.ApplyDisplaySettings();
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x000FF05D File Offset: 0x000FD25D
		public void SetFromSettings()
		{
			base.IsTicked = SaveManager.Instance.SettingsSave.ResizeWindows;
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x000FF074 File Offset: 0x000FD274
		private void OnWindowChange(bool isAutoAspectRatio)
		{
			this.RefreshEnabled();
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x000FF07C File Offset: 0x000FD27C
		private void RefreshEnabled()
		{
			if (SaveManager.Instance.SettingsSave.Fullscreen || PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen())
			{
				base.Disable();
				return;
			}
			base.Enable();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060025BA RID: 9658 RVA: 0x000FF0A8 File Offset: 0x000FD2A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NWindowResizeTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWindowResizeTickbox.MethodName.OnTick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWindowResizeTickbox.MethodName.OnUntick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWindowResizeTickbox.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWindowResizeTickbox.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isAutoAspectRatio", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NWindowResizeTickbox.MethodName.RefreshEnabled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060025BB RID: 9659 RVA: 0x000FF1FC File Offset: 0x000FD3FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NWindowResizeTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWindowResizeTickbox.MethodName.OnTick && args.Count == 0)
			{
				this.OnTick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWindowResizeTickbox.MethodName.OnUntick && args.Count == 0)
			{
				this.OnUntick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWindowResizeTickbox.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWindowResizeTickbox.MethodName.OnWindowChange && args.Count == 1)
			{
				this.OnWindowChange(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWindowResizeTickbox.MethodName.RefreshEnabled && args.Count == 0)
			{
				this.RefreshEnabled();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060025BC RID: 9660 RVA: 0x000FF300 File Offset: 0x000FD500
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NWindowResizeTickbox.MethodName._Ready || (ref method) == NWindowResizeTickbox.MethodName.OnTick || (ref method) == NWindowResizeTickbox.MethodName.OnUntick || (ref method) == NWindowResizeTickbox.MethodName.SetFromSettings || (ref method) == NWindowResizeTickbox.MethodName.OnWindowChange || (ref method) == NWindowResizeTickbox.MethodName.RefreshEnabled || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060025BD RID: 9661 RVA: 0x000FF36E File Offset: 0x000FD56E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NWindowResizeTickbox.PropertyName._settingsScreen)
			{
				this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060025BE RID: 9662 RVA: 0x000FF393 File Offset: 0x000FD593
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NWindowResizeTickbox.PropertyName._settingsScreen)
			{
				value = VariantUtils.CreateFrom<NSettingsScreen>(ref this._settingsScreen);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060025BF RID: 9663 RVA: 0x000FF3C0 File Offset: 0x000FD5C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NWindowResizeTickbox.PropertyName._settingsScreen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060025C0 RID: 9664 RVA: 0x000FF3F5 File Offset: 0x000FD5F5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NWindowResizeTickbox.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
		}

		/// <inheritdoc />
		// Token: 0x060025C1 RID: 9665 RVA: 0x000FF414 File Offset: 0x000FD614
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NWindowResizeTickbox.PropertyName._settingsScreen, ref variant))
			{
				this._settingsScreen = variant.As<NSettingsScreen>();
			}
		}

		// Token: 0x04000DF6 RID: 3574
		[Nullable(1)]
		private NSettingsScreen _settingsScreen;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001416 RID: 5142
		public new class MethodName : NSettingsTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C60 RID: 19552
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnTick' method.
			/// </summary>
			// Token: 0x04004C61 RID: 19553
			public new static readonly StringName OnTick = "OnTick";

			/// <summary>
			/// Cached name for the 'OnUntick' method.
			/// </summary>
			// Token: 0x04004C62 RID: 19554
			public new static readonly StringName OnUntick = "OnUntick";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004C63 RID: 19555
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04004C64 RID: 19556
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'RefreshEnabled' method.
			/// </summary>
			// Token: 0x04004C65 RID: 19557
			public static readonly StringName RefreshEnabled = "RefreshEnabled";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001417 RID: 5143
		public new class PropertyName : NSettingsTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_settingsScreen' field.
			/// </summary>
			// Token: 0x04004C66 RID: 19558
			public new static readonly StringName _settingsScreen = "_settingsScreen";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001418 RID: 5144
		public new class SignalName : NSettingsTickbox.SignalName
		{
		}
	}
}
