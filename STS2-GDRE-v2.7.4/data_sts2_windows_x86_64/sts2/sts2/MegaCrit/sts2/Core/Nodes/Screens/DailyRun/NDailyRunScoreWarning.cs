using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun
{
	/// <summary>
	/// Displays when the player's already uploaded a score for today's daily.
	/// When hovered, displays info about why the score won't be uploaded.
	/// </summary>
	// Token: 0x02000330 RID: 816
	[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunScoreWarning.cs")]
	public class NDailyRunScoreWarning : NClickableControl
	{
		// Token: 0x06002DA4 RID: 11684 RVA: 0x00147B02 File Offset: 0x00145D02
		public override void _Ready()
		{
			this.ConnectSignals();
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x00147B0C File Offset: 0x00145D0C
		protected override void OnFocus()
		{
			HoverTip hoverTip = new HoverTip(new LocString("main_menu_ui", "DAILY_RUN_MENU.NO_UPLOAD_HOVERTIP.title"), new LocString("main_menu_ui", "DAILY_RUN_MENU.NO_UPLOAD_HOVERTIP.description"), null);
			NHoverTipSet.CreateAndShow(this, hoverTip, HoverTipAlignment.Left);
			base.Scale = Vector2.One * 1.1f;
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x00147B62 File Offset: 0x00145D62
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this);
			base.Scale = Vector2.One;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002DA7 RID: 11687 RVA: 0x00147B78 File Offset: 0x00145D78
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NDailyRunScoreWarning.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScoreWarning.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDailyRunScoreWarning.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002DA8 RID: 11688 RVA: 0x00147C1C File Offset: 0x00145E1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDailyRunScoreWarning.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScoreWarning.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDailyRunScoreWarning.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002DA9 RID: 11689 RVA: 0x00147CA1 File Offset: 0x00145EA1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDailyRunScoreWarning.MethodName._Ready || (ref method) == NDailyRunScoreWarning.MethodName.OnFocus || (ref method) == NDailyRunScoreWarning.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002DAA RID: 11690 RVA: 0x00147CD7 File Offset: 0x00145ED7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002DAB RID: 11691 RVA: 0x00147CE0 File Offset: 0x00145EE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015A2 RID: 5538
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400530A RID: 21258
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400530B RID: 21259
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400530C RID: 21260
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015A3 RID: 5539
		public new class PropertyName : NClickableControl.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015A4 RID: 5540
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
