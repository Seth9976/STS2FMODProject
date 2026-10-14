using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x0200029A RID: 666
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NIntroLogoHoverTip.cs")]
	public class NIntroLogoHoverTip : Control
	{
		// Token: 0x0600235E RID: 9054 RVA: 0x000F0F10 File Offset: 0x000EF110
		public override void _Ready()
		{
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
			this._hoverTip = new HoverTip(new LocString("settings_ui", "SKIP_INTRO_LOGO_HEADER"), new LocString("settings_ui", "SKIP_INTRO_LOGO_DESCRIPTION"), null);
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x000F0F88 File Offset: 0x000EF188
		private void OnHovered()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + NSettingsScreen.settingTipsOffset, false);
			}
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x000F0FBD File Offset: 0x000EF1BD
		private void OnUnhovered()
		{
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002361 RID: 9057 RVA: 0x000F0FC8 File Offset: 0x000EF1C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NIntroLogoHoverTip.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIntroLogoHoverTip.MethodName.OnHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NIntroLogoHoverTip.MethodName.OnUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002362 RID: 9058 RVA: 0x000F106C File Offset: 0x000EF26C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NIntroLogoHoverTip.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIntroLogoHoverTip.MethodName.OnHovered && args.Count == 0)
			{
				this.OnHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NIntroLogoHoverTip.MethodName.OnUnhovered && args.Count == 0)
			{
				this.OnUnhovered();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002363 RID: 9059 RVA: 0x000F10F1 File Offset: 0x000EF2F1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NIntroLogoHoverTip.MethodName._Ready || (ref method) == NIntroLogoHoverTip.MethodName.OnHovered || (ref method) == NIntroLogoHoverTip.MethodName.OnUnhovered || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002364 RID: 9060 RVA: 0x000F1127 File Offset: 0x000EF327
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002365 RID: 9061 RVA: 0x000F1130 File Offset: 0x000EF330
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000D87 RID: 3463
		[Nullable(1)]
		private IHoverTip _hoverTip;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001384 RID: 4996
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B44 RID: 19268
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnHovered' method.
			/// </summary>
			// Token: 0x04004B45 RID: 19269
			public static readonly StringName OnHovered = "OnHovered";

			/// <summary>
			/// Cached name for the 'OnUnhovered' method.
			/// </summary>
			// Token: 0x04004B46 RID: 19270
			public static readonly StringName OnUnhovered = "OnUnhovered";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001385 RID: 4997
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001386 RID: 4998
		public class SignalName : Control.SignalName
		{
		}
	}
}
