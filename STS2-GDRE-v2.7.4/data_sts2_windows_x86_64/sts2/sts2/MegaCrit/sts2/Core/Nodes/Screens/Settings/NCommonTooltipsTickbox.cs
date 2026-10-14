using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x0200028C RID: 652
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NCommonTooltipsTickbox.cs")]
	public class NCommonTooltipsTickbox : NSettingsTickbox, IResettableSettingNode
	{
		// Token: 0x060022B1 RID: 8881 RVA: 0x000ED18D File Offset: 0x000EB38D
		public override void _Ready()
		{
			this.ConnectSignals();
			this._settingsScreen = this.GetAncestorOfType<NSettingsScreen>();
			this.SetFromSettings();
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000ED1A7 File Offset: 0x000EB3A7
		public void SetFromSettings()
		{
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x000ED1A9 File Offset: 0x000EB3A9
		protected override void OnTick()
		{
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000ED1AB File Offset: 0x000EB3AB
		protected override void OnUntick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_NOT_IMPLEMENTED"));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022B5 RID: 8885 RVA: 0x000ED1C8 File Offset: 0x000EB3C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCommonTooltipsTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCommonTooltipsTickbox.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCommonTooltipsTickbox.MethodName.OnTick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCommonTooltipsTickbox.MethodName.OnUntick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022B6 RID: 8886 RVA: 0x000ED29C File Offset: 0x000EB49C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCommonTooltipsTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCommonTooltipsTickbox.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCommonTooltipsTickbox.MethodName.OnTick && args.Count == 0)
			{
				this.OnTick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCommonTooltipsTickbox.MethodName.OnUntick && args.Count == 0)
			{
				this.OnUntick();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060022B7 RID: 8887 RVA: 0x000ED348 File Offset: 0x000EB548
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCommonTooltipsTickbox.MethodName._Ready || (ref method) == NCommonTooltipsTickbox.MethodName.SetFromSettings || (ref method) == NCommonTooltipsTickbox.MethodName.OnTick || (ref method) == NCommonTooltipsTickbox.MethodName.OnUntick || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060022B8 RID: 8888 RVA: 0x000ED398 File Offset: 0x000EB598
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCommonTooltipsTickbox.PropertyName._settingsScreen)
			{
				this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060022B9 RID: 8889 RVA: 0x000ED3BD File Offset: 0x000EB5BD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCommonTooltipsTickbox.PropertyName._settingsScreen)
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
		// Token: 0x060022BA RID: 8890 RVA: 0x000ED3E8 File Offset: 0x000EB5E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCommonTooltipsTickbox.PropertyName._settingsScreen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022BB RID: 8891 RVA: 0x000ED41D File Offset: 0x000EB61D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCommonTooltipsTickbox.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
		}

		/// <inheritdoc />
		// Token: 0x060022BC RID: 8892 RVA: 0x000ED43C File Offset: 0x000EB63C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCommonTooltipsTickbox.PropertyName._settingsScreen, ref variant))
			{
				this._settingsScreen = variant.As<NSettingsScreen>();
			}
		}

		// Token: 0x04000D6C RID: 3436
		[Nullable(1)]
		private NSettingsScreen _settingsScreen;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001357 RID: 4951
		public new class MethodName : NSettingsTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004AF3 RID: 19187
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004AF4 RID: 19188
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'OnTick' method.
			/// </summary>
			// Token: 0x04004AF5 RID: 19189
			public new static readonly StringName OnTick = "OnTick";

			/// <summary>
			/// Cached name for the 'OnUntick' method.
			/// </summary>
			// Token: 0x04004AF6 RID: 19190
			public new static readonly StringName OnUntick = "OnUntick";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001358 RID: 4952
		public new class PropertyName : NSettingsTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_settingsScreen' field.
			/// </summary>
			// Token: 0x04004AF7 RID: 19191
			public new static readonly StringName _settingsScreen = "_settingsScreen";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001359 RID: 4953
		public new class SignalName : NSettingsTickbox.SignalName
		{
		}
	}
}
