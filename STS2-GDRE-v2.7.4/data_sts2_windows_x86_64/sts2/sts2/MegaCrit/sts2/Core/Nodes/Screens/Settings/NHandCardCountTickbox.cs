using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x02000297 RID: 663
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NHandCardCountTickbox.cs")]
	public class NHandCardCountTickbox : NSettingsTickbox, IResettableSettingNode
	{
		// Token: 0x0600232B RID: 9003 RVA: 0x000EF4E9 File Offset: 0x000ED6E9
		public override void _Ready()
		{
			this.ConnectSignals();
			this._settingsScreen = this.GetAncestorOfType<NSettingsScreen>();
			this.SetFromSettings();
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x000EF503 File Offset: 0x000ED703
		public void SetFromSettings()
		{
			base.IsTicked = SaveManager.Instance.PrefsSave.ShowCardIndices;
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x000EF51A File Offset: 0x000ED71A
		protected override void OnTick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_HAND_CARD_COUNT_ON"));
			SaveManager.Instance.PrefsSave.ShowCardIndices = true;
			NHandCardCountTickbox.TryRefreshHandIndices();
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x000EF54B File Offset: 0x000ED74B
		protected override void OnUntick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_HAND_CARD_COUNT_OFF"));
			SaveManager.Instance.PrefsSave.ShowCardIndices = false;
			NHandCardCountTickbox.TryRefreshHandIndices();
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x000EF57C File Offset: 0x000ED77C
		private static void TryRefreshHandIndices()
		{
			if (NPlayerHand.Instance != null)
			{
				NPlayerHand.Instance.ForceRefreshCardIndices();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002330 RID: 9008 RVA: 0x000EF590 File Offset: 0x000ED790
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NHandCardCountTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardCountTickbox.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardCountTickbox.MethodName.OnTick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardCountTickbox.MethodName.OnUntick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHandCardCountTickbox.MethodName.TryRefreshHandIndices, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002331 RID: 9009 RVA: 0x000EF694 File Offset: 0x000ED894
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHandCardCountTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardCountTickbox.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardCountTickbox.MethodName.OnTick && args.Count == 0)
			{
				this.OnTick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardCountTickbox.MethodName.OnUntick && args.Count == 0)
			{
				this.OnUntick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHandCardCountTickbox.MethodName.TryRefreshHandIndices && args.Count == 0)
			{
				NHandCardCountTickbox.TryRefreshHandIndices();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x000EF762 File Offset: 0x000ED962
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHandCardCountTickbox.MethodName.TryRefreshHandIndices && args.Count == 0)
			{
				NHandCardCountTickbox.TryRefreshHandIndices();
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002333 RID: 9011 RVA: 0x000EF790 File Offset: 0x000ED990
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHandCardCountTickbox.MethodName._Ready || (ref method) == NHandCardCountTickbox.MethodName.SetFromSettings || (ref method) == NHandCardCountTickbox.MethodName.OnTick || (ref method) == NHandCardCountTickbox.MethodName.OnUntick || (ref method) == NHandCardCountTickbox.MethodName.TryRefreshHandIndices || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002334 RID: 9012 RVA: 0x000EF7EF File Offset: 0x000ED9EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHandCardCountTickbox.PropertyName._settingsScreen)
			{
				this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002335 RID: 9013 RVA: 0x000EF814 File Offset: 0x000EDA14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHandCardCountTickbox.PropertyName._settingsScreen)
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
		// Token: 0x06002336 RID: 9014 RVA: 0x000EF840 File Offset: 0x000EDA40
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NHandCardCountTickbox.PropertyName._settingsScreen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002337 RID: 9015 RVA: 0x000EF875 File Offset: 0x000EDA75
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHandCardCountTickbox.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
		}

		/// <inheritdoc />
		// Token: 0x06002338 RID: 9016 RVA: 0x000EF894 File Offset: 0x000EDA94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHandCardCountTickbox.PropertyName._settingsScreen, ref variant))
			{
				this._settingsScreen = variant.As<NSettingsScreen>();
			}
		}

		// Token: 0x04000D78 RID: 3448
		[Nullable(1)]
		private NSettingsScreen _settingsScreen;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001378 RID: 4984
		public new class MethodName : NSettingsTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B1E RID: 19230
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004B1F RID: 19231
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'OnTick' method.
			/// </summary>
			// Token: 0x04004B20 RID: 19232
			public new static readonly StringName OnTick = "OnTick";

			/// <summary>
			/// Cached name for the 'OnUntick' method.
			/// </summary>
			// Token: 0x04004B21 RID: 19233
			public new static readonly StringName OnUntick = "OnUntick";

			/// <summary>
			/// Cached name for the 'TryRefreshHandIndices' method.
			/// </summary>
			// Token: 0x04004B22 RID: 19234
			public static readonly StringName TryRefreshHandIndices = "TryRefreshHandIndices";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001379 RID: 4985
		public new class PropertyName : NSettingsTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_settingsScreen' field.
			/// </summary>
			// Token: 0x04004B23 RID: 19235
			public new static readonly StringName _settingsScreen = "_settingsScreen";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200137A RID: 4986
		public new class SignalName : NSettingsTickbox.SignalName
		{
		}
	}
}
