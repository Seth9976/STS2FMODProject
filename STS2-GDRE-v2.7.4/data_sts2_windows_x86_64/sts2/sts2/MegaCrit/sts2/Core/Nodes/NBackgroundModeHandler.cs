using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes
{
	/// <summary>
	/// Limits FPS to 30 when the game window loses focus to reduce GPU load and save battery.
	/// </summary>
	// Token: 0x02000177 RID: 375
	[ScriptPath("res://src/Core/Nodes/NBackgroundModeHandler.cs")]
	public class NBackgroundModeHandler : Node
	{
		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x00066282 File Offset: 0x00064482
		private static bool IsHeadless
		{
			get
			{
				return DisplayServer.GetName().Equals("headless", StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x00066294 File Offset: 0x00064494
		private static bool IsEditor
		{
			get
			{
				return OS.HasFeature("editor");
			}
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x000662A0 File Offset: 0x000644A0
		public override void _Notification(int what)
		{
			if (NBackgroundModeHandler.IsHeadless || NBackgroundModeHandler.IsEditor || NonInteractiveMode.IsActive)
			{
				return;
			}
			if ((long)what == 1005L)
			{
				this.EnterBackgroundMode();
				return;
			}
			if ((long)what == 1004L)
			{
				this.ExitBackgroundMode();
			}
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x000662DC File Offset: 0x000644DC
		private void EnterBackgroundMode()
		{
			if (this._isBackgrounded)
			{
				Log.Info("BackgroundMode: duplicate FocusOut (already backgrounded)", 2);
				return;
			}
			if (!SaveManager.Instance.SettingsSave.LimitFpsInBackground)
			{
				return;
			}
			INetGameService netService = RunManager.Instance.NetService;
			bool flag = netService != null && netService.Type.IsMultiplayer();
			if (flag)
			{
				return;
			}
			this._isBackgrounded = true;
			this._savedMaxFps = Engine.MaxFps;
			Engine.MaxFps = 30;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Limiting background FPS to ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(30);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00066374 File Offset: 0x00064574
		private void ExitBackgroundMode()
		{
			if (!this._isBackgrounded)
			{
				return;
			}
			this._isBackgrounded = false;
			Engine.MaxFps = this._savedMaxFps;
			Log.Info("Restored foreground FPS", 2);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001186 RID: 4486 RVA: 0x0006639C File Offset: 0x0006459C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NBackgroundModeHandler.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBackgroundModeHandler.MethodName.EnterBackgroundMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBackgroundModeHandler.MethodName.ExitBackgroundMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001187 RID: 4487 RVA: 0x00066464 File Offset: 0x00064664
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBackgroundModeHandler.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBackgroundModeHandler.MethodName.EnterBackgroundMode && args.Count == 0)
			{
				this.EnterBackgroundMode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBackgroundModeHandler.MethodName.ExitBackgroundMode && args.Count == 0)
			{
				this.ExitBackgroundMode();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001188 RID: 4488 RVA: 0x000664F7 File Offset: 0x000646F7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBackgroundModeHandler.MethodName._Notification || (ref method) == NBackgroundModeHandler.MethodName.EnterBackgroundMode || (ref method) == NBackgroundModeHandler.MethodName.ExitBackgroundMode || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001189 RID: 4489 RVA: 0x0006652D File Offset: 0x0006472D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBackgroundModeHandler.PropertyName._savedMaxFps)
			{
				this._savedMaxFps = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NBackgroundModeHandler.PropertyName._isBackgrounded)
			{
				this._isBackgrounded = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600118A RID: 4490 RVA: 0x00066570 File Offset: 0x00064770
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBackgroundModeHandler.PropertyName._savedMaxFps)
			{
				value = VariantUtils.CreateFrom<int>(ref this._savedMaxFps);
				return true;
			}
			if ((ref name) == NBackgroundModeHandler.PropertyName._isBackgrounded)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isBackgrounded);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600118B RID: 4491 RVA: 0x000665C8 File Offset: 0x000647C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NBackgroundModeHandler.PropertyName._savedMaxFps, 0L, "", 4096L, false),
				new PropertyInfo(1L, NBackgroundModeHandler.PropertyName._isBackgrounded, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600118C RID: 4492 RVA: 0x0006661C File Offset: 0x0006481C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBackgroundModeHandler.PropertyName._savedMaxFps, Variant.From<int>(ref this._savedMaxFps));
			info.AddProperty(NBackgroundModeHandler.PropertyName._isBackgrounded, Variant.From<bool>(ref this._isBackgrounded));
		}

		/// <inheritdoc />
		// Token: 0x0600118D RID: 4493 RVA: 0x00066654 File Offset: 0x00064854
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBackgroundModeHandler.PropertyName._savedMaxFps, ref variant))
			{
				this._savedMaxFps = variant.As<int>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBackgroundModeHandler.PropertyName._isBackgrounded, ref variant2))
			{
				this._isBackgrounded = variant2.As<bool>();
			}
		}

		// Token: 0x0400063D RID: 1597
		private const int _backgroundFps = 30;

		// Token: 0x0400063E RID: 1598
		private int _savedMaxFps;

		// Token: 0x0400063F RID: 1599
		private bool _isBackgrounded;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F5B RID: 3931
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x04003D07 RID: 15623
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'EnterBackgroundMode' method.
			/// </summary>
			// Token: 0x04003D08 RID: 15624
			public static readonly StringName EnterBackgroundMode = "EnterBackgroundMode";

			/// <summary>
			/// Cached name for the 'ExitBackgroundMode' method.
			/// </summary>
			// Token: 0x04003D09 RID: 15625
			public static readonly StringName ExitBackgroundMode = "ExitBackgroundMode";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F5C RID: 3932
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_savedMaxFps' field.
			/// </summary>
			// Token: 0x04003D0A RID: 15626
			public static readonly StringName _savedMaxFps = "_savedMaxFps";

			/// <summary>
			/// Cached name for the '_isBackgrounded' field.
			/// </summary>
			// Token: 0x04003D0B RID: 15627
			public static readonly StringName _isBackgrounded = "_isBackgrounded";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F5D RID: 3933
		public class SignalName : Node.SignalName
		{
		}
	}
}
