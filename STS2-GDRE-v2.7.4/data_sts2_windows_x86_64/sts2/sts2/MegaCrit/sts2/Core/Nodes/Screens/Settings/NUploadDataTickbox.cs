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
	// Token: 0x020002C4 RID: 708
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NUploadDataTickbox.cs")]
	public class NUploadDataTickbox : NSettingsTickbox, IResettableSettingNode
	{
		// Token: 0x06002589 RID: 9609 RVA: 0x000FE284 File Offset: 0x000FC484
		public override void _Ready()
		{
			this.ConnectSignals();
			this._settingsScreen = this.GetAncestorOfType<NSettingsScreen>();
			this.SetFromSettings();
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000FE29E File Offset: 0x000FC49E
		public void SetFromSettings()
		{
			base.IsTicked = SaveManager.Instance.PrefsSave.UploadData;
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x000FE2B5 File Offset: 0x000FC4B5
		protected override void OnTick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_GAMEPLAY_DATA_ON"));
			SaveManager.Instance.PrefsSave.UploadData = true;
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x000FE2E1 File Offset: 0x000FC4E1
		protected override void OnUntick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_GAMEPLAY_DATA_OFF"));
			SaveManager.Instance.PrefsSave.UploadData = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600258D RID: 9613 RVA: 0x000FE310 File Offset: 0x000FC510
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NUploadDataTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUploadDataTickbox.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUploadDataTickbox.MethodName.OnTick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NUploadDataTickbox.MethodName.OnUntick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600258E RID: 9614 RVA: 0x000FE3E4 File Offset: 0x000FC5E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NUploadDataTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUploadDataTickbox.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUploadDataTickbox.MethodName.OnTick && args.Count == 0)
			{
				this.OnTick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NUploadDataTickbox.MethodName.OnUntick && args.Count == 0)
			{
				this.OnUntick();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600258F RID: 9615 RVA: 0x000FE490 File Offset: 0x000FC690
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NUploadDataTickbox.MethodName._Ready || (ref method) == NUploadDataTickbox.MethodName.SetFromSettings || (ref method) == NUploadDataTickbox.MethodName.OnTick || (ref method) == NUploadDataTickbox.MethodName.OnUntick || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002590 RID: 9616 RVA: 0x000FE4E0 File Offset: 0x000FC6E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NUploadDataTickbox.PropertyName._settingsScreen)
			{
				this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002591 RID: 9617 RVA: 0x000FE505 File Offset: 0x000FC705
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NUploadDataTickbox.PropertyName._settingsScreen)
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
		// Token: 0x06002592 RID: 9618 RVA: 0x000FE530 File Offset: 0x000FC730
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NUploadDataTickbox.PropertyName._settingsScreen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002593 RID: 9619 RVA: 0x000FE565 File Offset: 0x000FC765
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NUploadDataTickbox.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
		}

		/// <inheritdoc />
		// Token: 0x06002594 RID: 9620 RVA: 0x000FE584 File Offset: 0x000FC784
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NUploadDataTickbox.PropertyName._settingsScreen, ref variant))
			{
				this._settingsScreen = variant.As<NSettingsScreen>();
			}
		}

		// Token: 0x04000DF3 RID: 3571
		[Nullable(1)]
		private NSettingsScreen _settingsScreen;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200140A RID: 5130
		public new class MethodName : NSettingsTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C50 RID: 19536
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004C51 RID: 19537
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'OnTick' method.
			/// </summary>
			// Token: 0x04004C52 RID: 19538
			public new static readonly StringName OnTick = "OnTick";

			/// <summary>
			/// Cached name for the 'OnUntick' method.
			/// </summary>
			// Token: 0x04004C53 RID: 19539
			public new static readonly StringName OnUntick = "OnUntick";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200140B RID: 5131
		public new class PropertyName : NSettingsTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_settingsScreen' field.
			/// </summary>
			// Token: 0x04004C54 RID: 19540
			public new static readonly StringName _settingsScreen = "_settingsScreen";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200140C RID: 5132
		public new class SignalName : NSettingsTickbox.SignalName
		{
		}
	}
}
