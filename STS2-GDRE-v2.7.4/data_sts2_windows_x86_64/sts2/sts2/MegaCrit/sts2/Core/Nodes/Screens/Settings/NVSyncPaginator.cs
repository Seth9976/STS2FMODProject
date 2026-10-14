using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002C6 RID: 710
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NVSyncPaginator.cs")]
	public class NVSyncPaginator : NPaginator, IResettableSettingNode
	{
		// Token: 0x0600259F RID: 9631 RVA: 0x000FE7F0 File Offset: 0x000FC9F0
		public override void _Ready()
		{
			base.ConnectSignals();
			this._options.Add(new LocString("settings_ui", "VSYNC_OFF").GetFormattedText());
			this._options.Add(new LocString("settings_ui", "VSYNC_ON").GetFormattedText());
			this._options.Add(new LocString("settings_ui", "VSYNC_ADAPTIVE").GetFormattedText());
			this.SetFromSettings();
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x000FE868 File Offset: 0x000FCA68
		public void SetFromSettings()
		{
			int num = this._options.IndexOf(NVSyncPaginator.GetVSyncString(SaveManager.Instance.SettingsSave.VSync));
			if (num != -1)
			{
				this._currentIndex = num;
			}
			else
			{
				this._currentIndex = 2;
			}
			this._label.SetTextAutoSize(this._options[this._currentIndex]);
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x000FE8C5 File Offset: 0x000FCAC5
		[NullableContext(1)]
		private static string GetVSyncString(VSyncType vsyncType)
		{
			return new LocString("settings_ui", NVSyncPaginator.GetVSyncLabelKey(vsyncType)).GetFormattedText();
		}

		/// <summary>
		/// Maps a stored <see cref="T:MegaCrit.Sts2.Core.Settings.VSyncType" /> to the localization key of the label the paginator
		/// shows for it. This is the inverse of the index-to-<see cref="T:MegaCrit.Sts2.Core.Settings.VSyncType" /> mapping in
		/// <see cref="M:MegaCrit.Sts2.Core.Nodes.Screens.Settings.NVSyncPaginator.OnIndexChanged(System.Int32)" />, so the two must stay in sync: the label shown on load has to
		/// match the value that toggling to that label writes back. Pure and Godot-free so the mapping
		/// can be unit tested without a scene.
		/// </summary>
		// Token: 0x060025A2 RID: 9634 RVA: 0x000FE8DC File Offset: 0x000FCADC
		[NullableContext(1)]
		public static string GetVSyncLabelKey(VSyncType vsyncType)
		{
			switch (vsyncType)
			{
			case VSyncType.Off:
				return "VSYNC_OFF";
			case VSyncType.On:
				return "VSYNC_ON";
			case VSyncType.Adaptive:
				return "VSYNC_ADAPTIVE";
			}
			Log.Error("Invalid VSync type: " + vsyncType.ToString(), 2);
			throw new ArgumentOutOfRangeException("vsyncType", vsyncType, null);
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x000FE948 File Offset: 0x000FCB48
		protected override void OnIndexChanged(int index)
		{
			this._currentIndex = index;
			this._label.SetTextAutoSize(this._options[index]);
			switch (index)
			{
			case 0:
				SaveManager.Instance.SettingsSave.VSync = VSyncType.Off;
				break;
			case 1:
				SaveManager.Instance.SettingsSave.VSync = VSyncType.On;
				break;
			case 2:
				SaveManager.Instance.SettingsSave.VSync = VSyncType.Adaptive;
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid VSync index: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(index);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				break;
			}
			}
			NGame.ApplySyncSetting();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060025A4 RID: 9636 RVA: 0x000FE9F0 File Offset: 0x000FCBF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NVSyncPaginator.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVSyncPaginator.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NVSyncPaginator.MethodName.GetVSyncString, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "vsyncType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NVSyncPaginator.MethodName.GetVSyncLabelKey, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "vsyncType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NVSyncPaginator.MethodName.OnIndexChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060025A5 RID: 9637 RVA: 0x000FEB64 File Offset: 0x000FCD64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NVSyncPaginator.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVSyncPaginator.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NVSyncPaginator.MethodName.GetVSyncString && args.Count == 1)
			{
				string vsyncString = NVSyncPaginator.GetVSyncString(VariantUtils.ConvertTo<VSyncType>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref vsyncString);
				return true;
			}
			if ((ref method) == NVSyncPaginator.MethodName.GetVSyncLabelKey && args.Count == 1)
			{
				string vsyncLabelKey = NVSyncPaginator.GetVSyncLabelKey(VariantUtils.ConvertTo<VSyncType>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref vsyncLabelKey);
				return true;
			}
			if ((ref method) == NVSyncPaginator.MethodName.OnIndexChanged && args.Count == 1)
			{
				this.OnIndexChanged(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x000FEC6C File Offset: 0x000FCE6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NVSyncPaginator.MethodName.GetVSyncString && args.Count == 1)
			{
				string vsyncString = NVSyncPaginator.GetVSyncString(VariantUtils.ConvertTo<VSyncType>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref vsyncString);
				return true;
			}
			if ((ref method) == NVSyncPaginator.MethodName.GetVSyncLabelKey && args.Count == 1)
			{
				string vsyncLabelKey = NVSyncPaginator.GetVSyncLabelKey(VariantUtils.ConvertTo<VSyncType>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref vsyncLabelKey);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060025A7 RID: 9639 RVA: 0x000FECF4 File Offset: 0x000FCEF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NVSyncPaginator.MethodName._Ready || (ref method) == NVSyncPaginator.MethodName.SetFromSettings || (ref method) == NVSyncPaginator.MethodName.GetVSyncString || (ref method) == NVSyncPaginator.MethodName.GetVSyncLabelKey || (ref method) == NVSyncPaginator.MethodName.OnIndexChanged || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060025A8 RID: 9640 RVA: 0x000FED53 File Offset: 0x000FCF53
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060025A9 RID: 9641 RVA: 0x000FED5C File Offset: 0x000FCF5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001410 RID: 5136
		public new class MethodName : NPaginator.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C58 RID: 19544
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004C59 RID: 19545
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'GetVSyncString' method.
			/// </summary>
			// Token: 0x04004C5A RID: 19546
			public static readonly StringName GetVSyncString = "GetVSyncString";

			/// <summary>
			/// Cached name for the 'GetVSyncLabelKey' method.
			/// </summary>
			// Token: 0x04004C5B RID: 19547
			public static readonly StringName GetVSyncLabelKey = "GetVSyncLabelKey";

			/// <summary>
			/// Cached name for the 'OnIndexChanged' method.
			/// </summary>
			// Token: 0x04004C5C RID: 19548
			public new static readonly StringName OnIndexChanged = "OnIndexChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001411 RID: 5137
		public new class PropertyName : NPaginator.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001412 RID: 5138
		public new class SignalName : NPaginator.SignalName
		{
		}
	}
}
