using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002AC RID: 684
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NResetGraphicsButton.cs")]
	public class NResetGraphicsButton : NSettingsButton
	{
		// Token: 0x06002439 RID: 9273 RVA: 0x000F5EFC File Offset: 0x000F40FC
		public override void _Ready()
		{
			this.ConnectSignals();
			base.PivotOffset = base.Size * 0.5f;
			base.GetNode<MegaLabel>("Label").SetTextAutoSize(new LocString("settings_ui", "RESET_SETTINGS_BUTTON").GetFormattedText());
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000F5F50 File Offset: 0x000F4150
		[NullableContext(1)]
		private async Task ResetSettingsAfterConfirmation()
		{
			NGenericPopup ngenericPopup = NGenericPopup.Create();
			NModalContainer.Instance.Add(ngenericPopup, true);
			TaskAwaiter<bool> taskAwaiter = ngenericPopup.WaitForConfirmation(new LocString("settings_ui", "RESET_GRAPHICS_CONFIRMATION.body"), new LocString("settings_ui", "RESET_CONFIRMATION.header"), new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), new LocString("main_menu_ui", "GENERIC_POPUP.confirm")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				Log.Info("Player reset graphics settings", 2);
				SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
				settingsSave.FpsLimit = 60;
				settingsSave.WindowPosition = new Vector2I(-1, -1);
				settingsSave.WindowSize = new Vector2I(1920, 1080);
				settingsSave.Fullscreen = true;
				settingsSave.AspectRatioSetting = AspectRatioSetting.SixteenByNine;
				settingsSave.TargetDisplay = -1;
				settingsSave.ResizeWindows = true;
				settingsSave.VSync = VSyncType.Adaptive;
				settingsSave.Msaa = 2;
				NGame.Instance.ApplyDisplaySettings();
				NSettingsPanel ancestorOfType = this.GetAncestorOfType<NSettingsPanel>();
				foreach (IResettableSettingNode resettableSettingNode in ancestorOfType.GetChildrenRecursive<IResettableSettingNode>())
				{
					resettableSettingNode.SetFromSettings();
				}
			}
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x000F5F93 File Offset: 0x000F4193
		protected override void OnRelease()
		{
			base.OnRelease();
			TaskHelper.RunSafely(this.ResetSettingsAfterConfirmation());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600243C RID: 9276 RVA: 0x000F5FA8 File Offset: 0x000F41A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NResetGraphicsButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResetGraphicsButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600243D RID: 9277 RVA: 0x000F601C File Offset: 0x000F421C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NResetGraphicsButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResetGraphicsButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600243E RID: 9278 RVA: 0x000F607C File Offset: 0x000F427C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NResetGraphicsButton.MethodName._Ready || (ref method) == NResetGraphicsButton.MethodName.OnRelease || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600243F RID: 9279 RVA: 0x000F60A3 File Offset: 0x000F42A3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002440 RID: 9280 RVA: 0x000F60AC File Offset: 0x000F42AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013BB RID: 5051
		public new class MethodName : NSettingsButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BAA RID: 19370
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004BAB RID: 19371
			public new static readonly StringName OnRelease = "OnRelease";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013BC RID: 5052
		public new class PropertyName : NSettingsButton.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013BD RID: 5053
		public new class SignalName : NSettingsButton.SignalName
		{
		}
	}
}
