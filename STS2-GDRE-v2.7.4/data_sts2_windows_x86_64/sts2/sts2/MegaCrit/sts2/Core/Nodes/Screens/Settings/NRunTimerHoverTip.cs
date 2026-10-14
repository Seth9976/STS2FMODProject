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
	// Token: 0x020002B1 RID: 689
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NRunTimerHoverTip.cs")]
	public class NRunTimerHoverTip : Control
	{
		// Token: 0x06002478 RID: 9336 RVA: 0x000F7614 File Offset: 0x000F5814
		public override void _Ready()
		{
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
			this._hoverTip = new HoverTip(new LocString("settings_ui", "SHOW_RUN_TIMER_HEADER"), new LocString("settings_ui", "SHOW_RUN_TIMER_DESCRIPTION"), null);
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x000F768C File Offset: 0x000F588C
		private void OnHovered()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + NSettingsScreen.settingTipsOffset, false);
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000F76C1 File Offset: 0x000F58C1
		private void OnUnhovered()
		{
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600247B RID: 9339 RVA: 0x000F76CC File Offset: 0x000F58CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NRunTimerHoverTip.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunTimerHoverTip.MethodName.OnHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunTimerHoverTip.MethodName.OnUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600247C RID: 9340 RVA: 0x000F7770 File Offset: 0x000F5970
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunTimerHoverTip.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunTimerHoverTip.MethodName.OnHovered && args.Count == 0)
			{
				this.OnHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunTimerHoverTip.MethodName.OnUnhovered && args.Count == 0)
			{
				this.OnUnhovered();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600247D RID: 9341 RVA: 0x000F77F5 File Offset: 0x000F59F5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRunTimerHoverTip.MethodName._Ready || (ref method) == NRunTimerHoverTip.MethodName.OnHovered || (ref method) == NRunTimerHoverTip.MethodName.OnUnhovered || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600247E RID: 9342 RVA: 0x000F782B File Offset: 0x000F5A2B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x0600247F RID: 9343 RVA: 0x000F7834 File Offset: 0x000F5A34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000DB9 RID: 3513
		[Nullable(1)]
		private IHoverTip _hoverTip;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013CB RID: 5067
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BC5 RID: 19397
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnHovered' method.
			/// </summary>
			// Token: 0x04004BC6 RID: 19398
			public static readonly StringName OnHovered = "OnHovered";

			/// <summary>
			/// Cached name for the 'OnUnhovered' method.
			/// </summary>
			// Token: 0x04004BC7 RID: 19399
			public static readonly StringName OnUnhovered = "OnUnhovered";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013CC RID: 5068
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013CD RID: 5069
		public class SignalName : Control.SignalName
		{
		}
	}
}
