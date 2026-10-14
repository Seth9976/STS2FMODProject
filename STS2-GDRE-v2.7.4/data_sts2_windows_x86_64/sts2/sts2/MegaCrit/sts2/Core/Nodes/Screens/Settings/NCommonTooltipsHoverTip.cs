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
	// Token: 0x0200028B RID: 651
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NCommonTooltipsHoverTip.cs")]
	public class NCommonTooltipsHoverTip : Control
	{
		// Token: 0x060022A8 RID: 8872 RVA: 0x000ECF5C File Offset: 0x000EB15C
		public override void _Ready()
		{
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
			this._hoverTip = new HoverTip(new LocString("settings_ui", "COMMON_TOOLTIPS_HEADER"), new LocString("settings_ui", "COMMON_TOOLTIPS_DESCRIPTION"), null);
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x000ECFD4 File Offset: 0x000EB1D4
		private void OnHovered()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + NSettingsScreen.settingTipsOffset, false);
			}
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x000ED009 File Offset: 0x000EB209
		private void OnUnhovered()
		{
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022AB RID: 8875 RVA: 0x000ED014 File Offset: 0x000EB214
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NCommonTooltipsHoverTip.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCommonTooltipsHoverTip.MethodName.OnHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCommonTooltipsHoverTip.MethodName.OnUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022AC RID: 8876 RVA: 0x000ED0B8 File Offset: 0x000EB2B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCommonTooltipsHoverTip.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCommonTooltipsHoverTip.MethodName.OnHovered && args.Count == 0)
			{
				this.OnHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCommonTooltipsHoverTip.MethodName.OnUnhovered && args.Count == 0)
			{
				this.OnUnhovered();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060022AD RID: 8877 RVA: 0x000ED13D File Offset: 0x000EB33D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCommonTooltipsHoverTip.MethodName._Ready || (ref method) == NCommonTooltipsHoverTip.MethodName.OnHovered || (ref method) == NCommonTooltipsHoverTip.MethodName.OnUnhovered || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060022AE RID: 8878 RVA: 0x000ED173 File Offset: 0x000EB373
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060022AF RID: 8879 RVA: 0x000ED17C File Offset: 0x000EB37C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000D6B RID: 3435
		[Nullable(1)]
		private IHoverTip _hoverTip;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001354 RID: 4948
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004AF0 RID: 19184
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnHovered' method.
			/// </summary>
			// Token: 0x04004AF1 RID: 19185
			public static readonly StringName OnHovered = "OnHovered";

			/// <summary>
			/// Cached name for the 'OnUnhovered' method.
			/// </summary>
			// Token: 0x04004AF2 RID: 19186
			public static readonly StringName OnUnhovered = "OnUnhovered";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001355 RID: 4949
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001356 RID: 4950
		public class SignalName : Control.SignalName
		{
		}
	}
}
