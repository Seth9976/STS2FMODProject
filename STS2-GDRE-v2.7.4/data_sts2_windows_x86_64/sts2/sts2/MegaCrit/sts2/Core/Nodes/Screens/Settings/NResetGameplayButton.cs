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
	// Token: 0x020002AB RID: 683
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NResetGameplayButton.cs")]
	public class NResetGameplayButton : NSettingsButton
	{
		// Token: 0x06002430 RID: 9264 RVA: 0x000F5D38 File Offset: 0x000F3F38
		public override void _Ready()
		{
			this.ConnectSignals();
			base.PivotOffset = base.Size * 0.5f;
			base.GetNode<MegaLabel>("Label").SetTextAutoSize(new LocString("settings_ui", "RESET_SETTINGS_BUTTON").GetFormattedText());
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000F5D8C File Offset: 0x000F3F8C
		[NullableContext(1)]
		private async Task ResetSettingsAfterConfirmation()
		{
			NGenericPopup ngenericPopup = NGenericPopup.Create();
			NModalContainer.Instance.Add(ngenericPopup, true);
			TaskAwaiter<bool> taskAwaiter = ngenericPopup.WaitForConfirmation(new LocString("settings_ui", "RESET_GAMEPLAY_CONFIRMATION.body"), new LocString("settings_ui", "RESET_CONFIRMATION.header"), new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), new LocString("main_menu_ui", "GENERIC_POPUP.confirm")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				Log.Info("Player reset general settings", 2);
				SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
				settingsSave.LimitFpsInBackground = true;
				settingsSave.SkipIntroLogo = false;
				PrefsSave prefsSave = SaveManager.Instance.PrefsSave;
				prefsSave.ScreenShakeOptionIndex = 2;
				prefsSave.FastMode = FastModeType.Normal;
				prefsSave.ShowRunTimer = false;
				prefsSave.ShowCardIndices = false;
				prefsSave.PhobiaMode = false;
				prefsSave.IsLongPressEnabled = false;
				prefsSave.UploadData = true;
				prefsSave.TextEffectsEnabled = true;
				prefsSave.ShowMultiplayerDrawings = true;
				NGame instance = NGame.Instance;
				if (instance != null)
				{
					instance.EmitSignal(NGame.SignalName.PhobiaModeToggled, Array.Empty<Variant>());
				}
				NSettingsPanel ancestorOfType = this.GetAncestorOfType<NSettingsPanel>();
				foreach (IResettableSettingNode resettableSettingNode in ancestorOfType.GetChildrenRecursive<IResettableSettingNode>())
				{
					resettableSettingNode.SetFromSettings();
				}
			}
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000F5DCF File Offset: 0x000F3FCF
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
		// Token: 0x06002433 RID: 9267 RVA: 0x000F5DE4 File Offset: 0x000F3FE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NResetGameplayButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResetGameplayButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002434 RID: 9268 RVA: 0x000F5E58 File Offset: 0x000F4058
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NResetGameplayButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResetGameplayButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002435 RID: 9269 RVA: 0x000F5EB8 File Offset: 0x000F40B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NResetGameplayButton.MethodName._Ready || (ref method) == NResetGameplayButton.MethodName.OnRelease || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002436 RID: 9270 RVA: 0x000F5EDF File Offset: 0x000F40DF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002437 RID: 9271 RVA: 0x000F5EE8 File Offset: 0x000F40E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013B7 RID: 5047
		public new class MethodName : NSettingsButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BA4 RID: 19364
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004BA5 RID: 19365
			public new static readonly StringName OnRelease = "OnRelease";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013B8 RID: 5048
		public new class PropertyName : NSettingsButton.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013B9 RID: 5049
		public new class SignalName : NSettingsButton.SignalName
		{
		}
	}
}
