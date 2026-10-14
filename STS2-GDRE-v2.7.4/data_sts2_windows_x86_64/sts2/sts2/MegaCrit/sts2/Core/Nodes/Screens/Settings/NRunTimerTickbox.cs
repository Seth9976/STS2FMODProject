using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002B2 RID: 690
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NRunTimerTickbox.cs")]
	public class NRunTimerTickbox : NSettingsTickbox, IResettableSettingNode
	{
		// Token: 0x06002481 RID: 9345 RVA: 0x000F7845 File Offset: 0x000F5A45
		public override void _Ready()
		{
			this.ConnectSignals();
			this._settingsScreen = this.GetAncestorOfType<NSettingsScreen>();
			this.SetFromSettings();
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000F785F File Offset: 0x000F5A5F
		public void SetFromSettings()
		{
			base.IsTicked = SaveManager.Instance.PrefsSave.ShowRunTimer;
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000F7876 File Offset: 0x000F5A76
		protected override void OnTick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_RUN_TIMER_ON"));
			SaveManager.Instance.PrefsSave.ShowRunTimer = true;
			NRunTimerTickbox.TryRefreshRunTimer();
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000F78A7 File Offset: 0x000F5AA7
		protected override void OnUntick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_RUN_TIMER_OFF"));
			SaveManager.Instance.PrefsSave.ShowRunTimer = false;
			NRunTimerTickbox.TryRefreshRunTimer();
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000F78D8 File Offset: 0x000F5AD8
		private static void TryRefreshRunTimer()
		{
			if (NRun.Instance != null)
			{
				NRun.Instance.GlobalUi.TopBar.Timer.RefreshVisibility();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002486 RID: 9350 RVA: 0x000F78FC File Offset: 0x000F5AFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NRunTimerTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunTimerTickbox.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunTimerTickbox.MethodName.OnTick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunTimerTickbox.MethodName.OnUntick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunTimerTickbox.MethodName.TryRefreshRunTimer, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002487 RID: 9351 RVA: 0x000F7A00 File Offset: 0x000F5C00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunTimerTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunTimerTickbox.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunTimerTickbox.MethodName.OnTick && args.Count == 0)
			{
				this.OnTick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunTimerTickbox.MethodName.OnUntick && args.Count == 0)
			{
				this.OnUntick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunTimerTickbox.MethodName.TryRefreshRunTimer && args.Count == 0)
			{
				NRunTimerTickbox.TryRefreshRunTimer();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x000F7ACE File Offset: 0x000F5CCE
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunTimerTickbox.MethodName.TryRefreshRunTimer && args.Count == 0)
			{
				NRunTimerTickbox.TryRefreshRunTimer();
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002489 RID: 9353 RVA: 0x000F7AFC File Offset: 0x000F5CFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRunTimerTickbox.MethodName._Ready || (ref method) == NRunTimerTickbox.MethodName.SetFromSettings || (ref method) == NRunTimerTickbox.MethodName.OnTick || (ref method) == NRunTimerTickbox.MethodName.OnUntick || (ref method) == NRunTimerTickbox.MethodName.TryRefreshRunTimer || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600248A RID: 9354 RVA: 0x000F7B5B File Offset: 0x000F5D5B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRunTimerTickbox.PropertyName._settingsScreen)
			{
				this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600248B RID: 9355 RVA: 0x000F7B80 File Offset: 0x000F5D80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRunTimerTickbox.PropertyName._settingsScreen)
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
		// Token: 0x0600248C RID: 9356 RVA: 0x000F7BAC File Offset: 0x000F5DAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRunTimerTickbox.PropertyName._settingsScreen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600248D RID: 9357 RVA: 0x000F7BE1 File Offset: 0x000F5DE1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRunTimerTickbox.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
		}

		/// <inheritdoc />
		// Token: 0x0600248E RID: 9358 RVA: 0x000F7C00 File Offset: 0x000F5E00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRunTimerTickbox.PropertyName._settingsScreen, ref variant))
			{
				this._settingsScreen = variant.As<NSettingsScreen>();
			}
		}

		// Token: 0x04000DBA RID: 3514
		[Nullable(1)]
		private NSettingsScreen _settingsScreen;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013CE RID: 5070
		public new class MethodName : NSettingsTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BC8 RID: 19400
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004BC9 RID: 19401
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'OnTick' method.
			/// </summary>
			// Token: 0x04004BCA RID: 19402
			public new static readonly StringName OnTick = "OnTick";

			/// <summary>
			/// Cached name for the 'OnUntick' method.
			/// </summary>
			// Token: 0x04004BCB RID: 19403
			public new static readonly StringName OnUntick = "OnUntick";

			/// <summary>
			/// Cached name for the 'TryRefreshRunTimer' method.
			/// </summary>
			// Token: 0x04004BCC RID: 19404
			public static readonly StringName TryRefreshRunTimer = "TryRefreshRunTimer";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013CF RID: 5071
		public new class PropertyName : NSettingsTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_settingsScreen' field.
			/// </summary>
			// Token: 0x04004BCD RID: 19405
			public new static readonly StringName _settingsScreen = "_settingsScreen";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013D0 RID: 5072
		public new class SignalName : NSettingsTickbox.SignalName
		{
		}
	}
}
