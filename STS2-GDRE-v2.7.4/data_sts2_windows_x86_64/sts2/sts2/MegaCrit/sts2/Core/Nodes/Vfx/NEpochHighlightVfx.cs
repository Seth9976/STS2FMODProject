using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Glowing Pulse vfx for Epochs that need to be revealed by the player.
	/// </summary>
	// Token: 0x020001A3 RID: 419
	[ScriptPath("res://src/Core/Nodes/Vfx/NEpochHighlightVfx.cs")]
	public class NEpochHighlightVfx : Control
	{
		// Token: 0x060014BB RID: 5307 RVA: 0x0007EAD8 File Offset: 0x0007CCD8
		[NullableContext(1)]
		public static NEpochHighlightVfx Create()
		{
			return PreloadManager.Cache.GetScene(NEpochHighlightVfx.scenePath).Instantiate<NEpochHighlightVfx>(0L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014BC RID: 5308 RVA: 0x0007EAF0 File Offset: 0x0007CCF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NEpochHighlightVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014BD RID: 5309 RVA: 0x0007EB40 File Offset: 0x0007CD40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochHighlightVfx.MethodName.Create && args.Count == 0)
			{
				NEpochHighlightVfx nepochHighlightVfx = NEpochHighlightVfx.Create();
				ret = VariantUtils.CreateFrom<NEpochHighlightVfx>(ref nepochHighlightVfx);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0007EB84 File Offset: 0x0007CD84
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochHighlightVfx.MethodName.Create && args.Count == 0)
			{
				NEpochHighlightVfx nepochHighlightVfx = NEpochHighlightVfx.Create();
				ret = VariantUtils.CreateFrom<NEpochHighlightVfx>(ref nepochHighlightVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060014BF RID: 5311 RVA: 0x0007EBC4 File Offset: 0x0007CDC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEpochHighlightVfx.MethodName.Create || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060014C0 RID: 5312 RVA: 0x0007EBDC File Offset: 0x0007CDDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060014C1 RID: 5313 RVA: 0x0007EBE5 File Offset: 0x0007CDE5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000774 RID: 1908
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/epoch_highlight_vfx");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001015 RID: 4117
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003FE1 RID: 16353
			public static readonly StringName Create = "Create";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001016 RID: 4118
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001017 RID: 4119
		public class SignalName : Control.SignalName
		{
		}
	}
}
