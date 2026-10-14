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
	// Token: 0x020002B3 RID: 691
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NScreenshakeHoverTip.cs")]
	public class NScreenshakeHoverTip : Control
	{
		// Token: 0x06002490 RID: 9360 RVA: 0x000F7C38 File Offset: 0x000F5E38
		public override void _Ready()
		{
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
			this._hoverTip = new HoverTip(new LocString("settings_ui", "SCREENSHAKE_HEADER"), new LocString("settings_ui", "SCREENSHAKE_DESCRIPTION"), null);
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x000F7CB0 File Offset: 0x000F5EB0
		private void OnHovered()
		{
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + NSettingsScreen.settingTipsOffset, false);
			}
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000F7CE5 File Offset: 0x000F5EE5
		private void OnUnhovered()
		{
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002493 RID: 9363 RVA: 0x000F7CF0 File Offset: 0x000F5EF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NScreenshakeHoverTip.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScreenshakeHoverTip.MethodName.OnHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScreenshakeHoverTip.MethodName.OnUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002494 RID: 9364 RVA: 0x000F7D94 File Offset: 0x000F5F94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScreenshakeHoverTip.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenshakeHoverTip.MethodName.OnHovered && args.Count == 0)
			{
				this.OnHovered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenshakeHoverTip.MethodName.OnUnhovered && args.Count == 0)
			{
				this.OnUnhovered();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002495 RID: 9365 RVA: 0x000F7E19 File Offset: 0x000F6019
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NScreenshakeHoverTip.MethodName._Ready || (ref method) == NScreenshakeHoverTip.MethodName.OnHovered || (ref method) == NScreenshakeHoverTip.MethodName.OnUnhovered || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002496 RID: 9366 RVA: 0x000F7E4F File Offset: 0x000F604F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002497 RID: 9367 RVA: 0x000F7E58 File Offset: 0x000F6058
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000DBB RID: 3515
		[Nullable(1)]
		private IHoverTip _hoverTip;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013D1 RID: 5073
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BCE RID: 19406
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnHovered' method.
			/// </summary>
			// Token: 0x04004BCF RID: 19407
			public static readonly StringName OnHovered = "OnHovered";

			/// <summary>
			/// Cached name for the 'OnUnhovered' method.
			/// </summary>
			// Token: 0x04004BD0 RID: 19408
			public static readonly StringName OnUnhovered = "OnUnhovered";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013D2 RID: 5074
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013D3 RID: 5075
		public class SignalName : Control.SignalName
		{
		}
	}
}
