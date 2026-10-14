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
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x02000292 RID: 658
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NFastModeTickbox.cs")]
	public class NFastModeTickbox : NSettingsTickbox, IResettableSettingNode
	{
		// Token: 0x060022F7 RID: 8951 RVA: 0x000EE505 File Offset: 0x000EC705
		public override void _Ready()
		{
			this.ConnectSignals();
			this._settingsScreen = this.GetAncestorOfType<NSettingsScreen>();
			this.SetFromSettings();
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x000EE51F File Offset: 0x000EC71F
		public void SetFromSettings()
		{
			base.IsTicked = SaveManager.Instance.PrefsSave.FastMode != FastModeType.Normal;
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x000EE53C File Offset: 0x000EC73C
		protected override void OnTick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_FAST_MODE_ON"));
			SaveManager.Instance.PrefsSave.FastMode = FastModeType.Fast;
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x000EE568 File Offset: 0x000EC768
		protected override void OnUntick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_FAST_MODE_OFF"));
			SaveManager.Instance.PrefsSave.FastMode = FastModeType.Normal;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022FB RID: 8955 RVA: 0x000EE594 File Offset: 0x000EC794
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NFastModeTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFastModeTickbox.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFastModeTickbox.MethodName.OnTick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFastModeTickbox.MethodName.OnUntick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022FC RID: 8956 RVA: 0x000EE668 File Offset: 0x000EC868
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFastModeTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFastModeTickbox.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFastModeTickbox.MethodName.OnTick && args.Count == 0)
			{
				this.OnTick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFastModeTickbox.MethodName.OnUntick && args.Count == 0)
			{
				this.OnUntick();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060022FD RID: 8957 RVA: 0x000EE714 File Offset: 0x000EC914
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFastModeTickbox.MethodName._Ready || (ref method) == NFastModeTickbox.MethodName.SetFromSettings || (ref method) == NFastModeTickbox.MethodName.OnTick || (ref method) == NFastModeTickbox.MethodName.OnUntick || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060022FE RID: 8958 RVA: 0x000EE764 File Offset: 0x000EC964
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFastModeTickbox.PropertyName._settingsScreen)
			{
				this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060022FF RID: 8959 RVA: 0x000EE789 File Offset: 0x000EC989
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFastModeTickbox.PropertyName._settingsScreen)
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
		// Token: 0x06002300 RID: 8960 RVA: 0x000EE7B4 File Offset: 0x000EC9B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFastModeTickbox.PropertyName._settingsScreen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002301 RID: 8961 RVA: 0x000EE7E9 File Offset: 0x000EC9E9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFastModeTickbox.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
		}

		/// <inheritdoc />
		// Token: 0x06002302 RID: 8962 RVA: 0x000EE808 File Offset: 0x000ECA08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFastModeTickbox.PropertyName._settingsScreen, ref variant))
			{
				this._settingsScreen = variant.As<NSettingsScreen>();
			}
		}

		// Token: 0x04000D75 RID: 3445
		[Nullable(1)]
		private NSettingsScreen _settingsScreen;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001369 RID: 4969
		public new class MethodName : NSettingsTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B0B RID: 19211
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004B0C RID: 19212
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'OnTick' method.
			/// </summary>
			// Token: 0x04004B0D RID: 19213
			public new static readonly StringName OnTick = "OnTick";

			/// <summary>
			/// Cached name for the 'OnUntick' method.
			/// </summary>
			// Token: 0x04004B0E RID: 19214
			public new static readonly StringName OnUntick = "OnUntick";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200136A RID: 4970
		public new class PropertyName : NSettingsTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_settingsScreen' field.
			/// </summary>
			// Token: 0x04004B0F RID: 19215
			public new static readonly StringName _settingsScreen = "_settingsScreen";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200136B RID: 4971
		public new class SignalName : NSettingsTickbox.SignalName
		{
		}
	}
}
