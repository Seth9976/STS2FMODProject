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
	// Token: 0x020002A2 RID: 674
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NMsaaHoverTip.cs")]
	public class NMsaaHoverTip : Control
	{
		// Token: 0x060023C1 RID: 9153 RVA: 0x000F3254 File Offset: 0x000F1454
		public override void _Ready()
		{
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
			this._hoverTip = new HoverTip(new LocString("settings_ui", "MSAA_HEADER"), new LocString("settings_ui", "MSAA_DESCRIPTION"), null);
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000F32CC File Offset: 0x000F14CC
		private void OnHovered()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + NSettingsScreen.settingTipsOffset, false);
			}
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x000F3301 File Offset: 0x000F1501
		private void OnUnhovered()
		{
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060023C4 RID: 9156 RVA: 0x000F330C File Offset: 0x000F150C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMsaaHoverTip.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMsaaHoverTip.MethodName.OnHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMsaaHoverTip.MethodName.OnUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060023C5 RID: 9157 RVA: 0x000F33B0 File Offset: 0x000F15B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMsaaHoverTip.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMsaaHoverTip.MethodName.OnHovered && args.Count == 0)
			{
				this.OnHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMsaaHoverTip.MethodName.OnUnhovered && args.Count == 0)
			{
				this.OnUnhovered();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060023C6 RID: 9158 RVA: 0x000F3435 File Offset: 0x000F1635
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMsaaHoverTip.MethodName._Ready || (ref method) == NMsaaHoverTip.MethodName.OnHovered || (ref method) == NMsaaHoverTip.MethodName.OnUnhovered || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060023C7 RID: 9159 RVA: 0x000F346B File Offset: 0x000F166B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060023C8 RID: 9160 RVA: 0x000F3474 File Offset: 0x000F1674
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000D97 RID: 3479
		[Nullable(1)]
		private IHoverTip _hoverTip;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200139C RID: 5020
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B6D RID: 19309
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnHovered' method.
			/// </summary>
			// Token: 0x04004B6E RID: 19310
			public static readonly StringName OnHovered = "OnHovered";

			/// <summary>
			/// Cached name for the 'OnUnhovered' method.
			/// </summary>
			// Token: 0x04004B6F RID: 19311
			public static readonly StringName OnUnhovered = "OnUnhovered";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200139D RID: 5021
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200139E RID: 5022
		public class SignalName : Control.SignalName
		{
		}
	}
}
