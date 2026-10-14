using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002C1 RID: 705
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NShowMpMapDrawingsTickbox.cs")]
	public class NShowMpMapDrawingsTickbox : NSettingsTickbox, IResettableSettingNode
	{
		// Token: 0x06002564 RID: 9572 RVA: 0x000FD935 File Offset: 0x000FBB35
		public override void _Ready()
		{
			this.ConnectSignals();
			this._settingsScreen = this.GetAncestorOfType<NSettingsScreen>();
			this.SetFromSettings();
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000FD94F File Offset: 0x000FBB4F
		public void SetFromSettings()
		{
			base.IsTicked = SaveManager.Instance.PrefsSave.ShowMultiplayerDrawings;
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000FD966 File Offset: 0x000FBB66
		protected override void OnTick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_SHOW_MP_DRAWINGS_ON"));
			SaveManager.Instance.PrefsSave.ShowMultiplayerDrawings = true;
			NShowMpMapDrawingsTickbox.TryRefreshMapDrawings();
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000FD997 File Offset: 0x000FBB97
		protected override void OnUntick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_SHOW_MP_DRAWINGS_OFF"));
			SaveManager.Instance.PrefsSave.ShowMultiplayerDrawings = false;
			NShowMpMapDrawingsTickbox.TryRefreshMapDrawings();
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000FD9C8 File Offset: 0x000FBBC8
		private static void TryRefreshMapDrawings()
		{
			if (NMapScreen.Instance != null)
			{
				NMapScreen.Instance.Drawings.UpdateVisibilityFromSettings();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002569 RID: 9577 RVA: 0x000FD9E0 File Offset: 0x000FBBE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NShowMpMapDrawingsTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShowMpMapDrawingsTickbox.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShowMpMapDrawingsTickbox.MethodName.OnTick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShowMpMapDrawingsTickbox.MethodName.OnUntick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShowMpMapDrawingsTickbox.MethodName.TryRefreshMapDrawings, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600256A RID: 9578 RVA: 0x000FDAE4 File Offset: 0x000FBCE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NShowMpMapDrawingsTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShowMpMapDrawingsTickbox.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShowMpMapDrawingsTickbox.MethodName.OnTick && args.Count == 0)
			{
				this.OnTick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShowMpMapDrawingsTickbox.MethodName.OnUntick && args.Count == 0)
			{
				this.OnUntick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShowMpMapDrawingsTickbox.MethodName.TryRefreshMapDrawings && args.Count == 0)
			{
				NShowMpMapDrawingsTickbox.TryRefreshMapDrawings();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000FDBB2 File Offset: 0x000FBDB2
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NShowMpMapDrawingsTickbox.MethodName.TryRefreshMapDrawings && args.Count == 0)
			{
				NShowMpMapDrawingsTickbox.TryRefreshMapDrawings();
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600256C RID: 9580 RVA: 0x000FDBE0 File Offset: 0x000FBDE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NShowMpMapDrawingsTickbox.MethodName._Ready || (ref method) == NShowMpMapDrawingsTickbox.MethodName.SetFromSettings || (ref method) == NShowMpMapDrawingsTickbox.MethodName.OnTick || (ref method) == NShowMpMapDrawingsTickbox.MethodName.OnUntick || (ref method) == NShowMpMapDrawingsTickbox.MethodName.TryRefreshMapDrawings || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600256D RID: 9581 RVA: 0x000FDC3F File Offset: 0x000FBE3F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NShowMpMapDrawingsTickbox.PropertyName._settingsScreen)
			{
				this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600256E RID: 9582 RVA: 0x000FDC64 File Offset: 0x000FBE64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NShowMpMapDrawingsTickbox.PropertyName._settingsScreen)
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
		// Token: 0x0600256F RID: 9583 RVA: 0x000FDC90 File Offset: 0x000FBE90
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NShowMpMapDrawingsTickbox.PropertyName._settingsScreen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002570 RID: 9584 RVA: 0x000FDCC5 File Offset: 0x000FBEC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NShowMpMapDrawingsTickbox.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
		}

		/// <inheritdoc />
		// Token: 0x06002571 RID: 9585 RVA: 0x000FDCE4 File Offset: 0x000FBEE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NShowMpMapDrawingsTickbox.PropertyName._settingsScreen, ref variant))
			{
				this._settingsScreen = variant.As<NSettingsScreen>();
			}
		}

		// Token: 0x04000DF0 RID: 3568
		[Nullable(1)]
		private NSettingsScreen _settingsScreen;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001401 RID: 5121
		public new class MethodName : NSettingsTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C42 RID: 19522
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004C43 RID: 19523
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'OnTick' method.
			/// </summary>
			// Token: 0x04004C44 RID: 19524
			public new static readonly StringName OnTick = "OnTick";

			/// <summary>
			/// Cached name for the 'OnUntick' method.
			/// </summary>
			// Token: 0x04004C45 RID: 19525
			public new static readonly StringName OnUntick = "OnUntick";

			/// <summary>
			/// Cached name for the 'TryRefreshMapDrawings' method.
			/// </summary>
			// Token: 0x04004C46 RID: 19526
			public static readonly StringName TryRefreshMapDrawings = "TryRefreshMapDrawings";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001402 RID: 5122
		public new class PropertyName : NSettingsTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_settingsScreen' field.
			/// </summary>
			// Token: 0x04004C47 RID: 19527
			public new static readonly StringName _settingsScreen = "_settingsScreen";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001403 RID: 5123
		public new class SignalName : NSettingsTickbox.SignalName
		{
		}
	}
}
