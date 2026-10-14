using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x02000295 RID: 661
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NFullscreenTickbox.cs")]
	public class NFullscreenTickbox : NSettingsTickbox
	{
		// Token: 0x06002316 RID: 8982 RVA: 0x000EEDF0 File Offset: 0x000ECFF0
		public override void _Ready()
		{
			this.ConnectSignals();
			NGame.Instance.Connect(NGame.SignalName.WindowChange, Callable.From<bool>(new Action<bool>(this.OnWindowChange)), 0U);
			this.OnWindowChange(SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto);
			if (PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen())
			{
				base.Disable();
			}
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x000EEE4F File Offset: 0x000ED04F
		protected override void OnTick()
		{
			NFullscreenTickbox.SetFullscreen(true);
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x000EEE57 File Offset: 0x000ED057
		protected override void OnUntick()
		{
			NFullscreenTickbox.SetFullscreen(false);
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x000EEE5F File Offset: 0x000ED05F
		private void OnWindowChange(bool _)
		{
			base.IsTicked = SaveManager.Instance.SettingsSave.Fullscreen;
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x000EEE78 File Offset: 0x000ED078
		public static void SetFullscreen(bool fullscreen)
		{
			if (PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen() && !fullscreen)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to go to windowed mode, but the current platform doesn't support it (");
				defaultInterpolatedStringHandler.AppendFormatted<SupportedWindowMode>(PlatformUtil.GetSupportedWindowMode());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			int num = DisplayServer.WindowGetCurrentScreen(0);
			SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
			if (fullscreen)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Setting FULLSCREEN for display [");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				settingsSave.TargetDisplay = num;
				settingsSave.Fullscreen = true;
				settingsSave.WindowSize = DisplayServer.WindowGetSize(0);
				settingsSave.WindowPosition = new Vector2I(-1, -1);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Exiting FULLSCREEN for display [");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				if (settingsSave.WindowSize >= DisplayServer.ScreenGetSize(num))
				{
					settingsSave.WindowSize = DisplayServer.ScreenGetSize(num) - new Vector2I(8, 48);
					settingsSave.WindowPosition = new Vector2I(4, 44);
				}
				settingsSave.Fullscreen = false;
			}
			NGame.Instance.ApplyDisplaySettings();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600231B RID: 8987 RVA: 0x000EEFC8 File Offset: 0x000ED1C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NFullscreenTickbox.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFullscreenTickbox.MethodName.OnTick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFullscreenTickbox.MethodName.OnUntick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFullscreenTickbox.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "_", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NFullscreenTickbox.MethodName.SetFullscreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "fullscreen", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600231C RID: 8988 RVA: 0x000EF114 File Offset: 0x000ED314
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFullscreenTickbox.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFullscreenTickbox.MethodName.OnTick && args.Count == 0)
			{
				this.OnTick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFullscreenTickbox.MethodName.OnUntick && args.Count == 0)
			{
				this.OnUntick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFullscreenTickbox.MethodName.OnWindowChange && args.Count == 1)
			{
				this.OnWindowChange(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFullscreenTickbox.MethodName.SetFullscreen && args.Count == 1)
			{
				NFullscreenTickbox.SetFullscreen(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x000EF1FE File Offset: 0x000ED3FE
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFullscreenTickbox.MethodName.SetFullscreen && args.Count == 1)
			{
				NFullscreenTickbox.SetFullscreen(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600231E RID: 8990 RVA: 0x000EF23C File Offset: 0x000ED43C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFullscreenTickbox.MethodName._Ready || (ref method) == NFullscreenTickbox.MethodName.OnTick || (ref method) == NFullscreenTickbox.MethodName.OnUntick || (ref method) == NFullscreenTickbox.MethodName.OnWindowChange || (ref method) == NFullscreenTickbox.MethodName.SetFullscreen || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600231F RID: 8991 RVA: 0x000EF29B File Offset: 0x000ED49B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002320 RID: 8992 RVA: 0x000EF2A4 File Offset: 0x000ED4A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001372 RID: 4978
		public new class MethodName : NSettingsTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B16 RID: 19222
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnTick' method.
			/// </summary>
			// Token: 0x04004B17 RID: 19223
			public new static readonly StringName OnTick = "OnTick";

			/// <summary>
			/// Cached name for the 'OnUntick' method.
			/// </summary>
			// Token: 0x04004B18 RID: 19224
			public new static readonly StringName OnUntick = "OnUntick";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04004B19 RID: 19225
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'SetFullscreen' method.
			/// </summary>
			// Token: 0x04004B1A RID: 19226
			public static readonly StringName SetFullscreen = "SetFullscreen";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001373 RID: 4979
		public new class PropertyName : NSettingsTickbox.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001374 RID: 4980
		public new class SignalName : NSettingsTickbox.SignalName
		{
		}
	}
}
