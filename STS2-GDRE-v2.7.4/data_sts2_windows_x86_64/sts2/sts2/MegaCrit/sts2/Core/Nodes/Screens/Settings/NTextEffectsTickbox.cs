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
	// Token: 0x020002C3 RID: 707
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NTextEffectsTickbox.cs")]
	public class NTextEffectsTickbox : NSettingsTickbox, IResettableSettingNode
	{
		// Token: 0x0600257C RID: 9596 RVA: 0x000FDF4D File Offset: 0x000FC14D
		public override void _Ready()
		{
			this.ConnectSignals();
			this._settingsScreen = this.GetAncestorOfType<NSettingsScreen>();
			this.SetFromSettings();
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000FDF67 File Offset: 0x000FC167
		public void SetFromSettings()
		{
			base.IsTicked = SaveManager.Instance.PrefsSave.TextEffectsEnabled;
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x000FDF7E File Offset: 0x000FC17E
		protected override void OnTick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_TEXT_EFFECTS_ON"));
			SaveManager.Instance.PrefsSave.TextEffectsEnabled = true;
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x000FDFAA File Offset: 0x000FC1AA
		protected override void OnUntick()
		{
			this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_TEXT_EFFECTS_OFF"));
			SaveManager.Instance.PrefsSave.TextEffectsEnabled = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002580 RID: 9600 RVA: 0x000FDFD8 File Offset: 0x000FC1D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NTextEffectsTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTextEffectsTickbox.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTextEffectsTickbox.MethodName.OnTick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTextEffectsTickbox.MethodName.OnUntick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002581 RID: 9601 RVA: 0x000FE0AC File Offset: 0x000FC2AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTextEffectsTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTextEffectsTickbox.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTextEffectsTickbox.MethodName.OnTick && args.Count == 0)
			{
				this.OnTick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTextEffectsTickbox.MethodName.OnUntick && args.Count == 0)
			{
				this.OnUntick();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002582 RID: 9602 RVA: 0x000FE158 File Offset: 0x000FC358
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTextEffectsTickbox.MethodName._Ready || (ref method) == NTextEffectsTickbox.MethodName.SetFromSettings || (ref method) == NTextEffectsTickbox.MethodName.OnTick || (ref method) == NTextEffectsTickbox.MethodName.OnUntick || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002583 RID: 9603 RVA: 0x000FE1A8 File Offset: 0x000FC3A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTextEffectsTickbox.PropertyName._settingsScreen)
			{
				this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002584 RID: 9604 RVA: 0x000FE1CD File Offset: 0x000FC3CD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTextEffectsTickbox.PropertyName._settingsScreen)
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
		// Token: 0x06002585 RID: 9605 RVA: 0x000FE1F8 File Offset: 0x000FC3F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTextEffectsTickbox.PropertyName._settingsScreen, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002586 RID: 9606 RVA: 0x000FE22D File Offset: 0x000FC42D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTextEffectsTickbox.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
		}

		/// <inheritdoc />
		// Token: 0x06002587 RID: 9607 RVA: 0x000FE24C File Offset: 0x000FC44C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTextEffectsTickbox.PropertyName._settingsScreen, ref variant))
			{
				this._settingsScreen = variant.As<NSettingsScreen>();
			}
		}

		// Token: 0x04000DF2 RID: 3570
		[Nullable(1)]
		private NSettingsScreen _settingsScreen;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001407 RID: 5127
		public new class MethodName : NSettingsTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C4B RID: 19531
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004C4C RID: 19532
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'OnTick' method.
			/// </summary>
			// Token: 0x04004C4D RID: 19533
			public new static readonly StringName OnTick = "OnTick";

			/// <summary>
			/// Cached name for the 'OnUntick' method.
			/// </summary>
			// Token: 0x04004C4E RID: 19534
			public new static readonly StringName OnUntick = "OnUntick";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001408 RID: 5128
		public new class PropertyName : NSettingsTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_settingsScreen' field.
			/// </summary>
			// Token: 0x04004C4F RID: 19535
			public new static readonly StringName _settingsScreen = "_settingsScreen";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001409 RID: 5129
		public new class SignalName : NSettingsTickbox.SignalName
		{
		}
	}
}
