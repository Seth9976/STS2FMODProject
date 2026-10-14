using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	// Token: 0x020003E3 RID: 995
	[ScriptPath("res://src/Core/Nodes/CommonUi/NLoadingOverlay.cs")]
	public class NLoadingOverlay : Control
	{
		// Token: 0x06003CA0 RID: 15520 RVA: 0x001D036F File Offset: 0x001CE56F
		public override void _Ready()
		{
			base.GetNode<MegaLabel>("%Label").SetTextAutoSize(new LocString("main_menu_ui", "LOADING_OVERLAY.label").GetFormattedText());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003CA1 RID: 15521 RVA: 0x001D039C File Offset: 0x001CE59C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NLoadingOverlay.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003CA2 RID: 15522 RVA: 0x001D03E0 File Offset: 0x001CE5E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLoadingOverlay.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003CA3 RID: 15523 RVA: 0x001D0410 File Offset: 0x001CE610
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLoadingOverlay.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003CA4 RID: 15524 RVA: 0x001D0428 File Offset: 0x001CE628
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003CA5 RID: 15525 RVA: 0x001D0431 File Offset: 0x001CE631
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200185E RID: 6238
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005F5A RID: 24410
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200185F RID: 6239
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001860 RID: 6240
		public class SignalName : Control.SignalName
		{
		}
	}
}
