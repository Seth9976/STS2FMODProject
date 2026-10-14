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
	/// Oscillates the rain direction from -20 to 20 degrees rapidly (exaggerated so effect can be seen clearly)
	/// </summary>
	// Token: 0x020001E4 RID: 484
	[ScriptPath("res://src/Core/Nodes/Vfx/NRainVfx.cs")]
	public class NRainVfx : GpuParticles2D
	{
		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001888 RID: 6280 RVA: 0x000994CD File Offset: 0x000976CD
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("vfx/whole_screen/vfx_rain");
			}
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x000994D9 File Offset: 0x000976D9
		[NullableContext(1)]
		public static NRainVfx Create()
		{
			return PreloadManager.Cache.GetScene(NRainVfx.ScenePath).Instantiate<NRainVfx>(0L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600188A RID: 6282 RVA: 0x000994F4 File Offset: 0x000976F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NRainVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("GPUParticles2D"), false), 33L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600188B RID: 6283 RVA: 0x00099544 File Offset: 0x00097744
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRainVfx.MethodName.Create && args.Count == 0)
			{
				NRainVfx nrainVfx = NRainVfx.Create();
				ret = VariantUtils.CreateFrom<NRainVfx>(ref nrainVfx);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00099588 File Offset: 0x00097788
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRainVfx.MethodName.Create && args.Count == 0)
			{
				NRainVfx nrainVfx = NRainVfx.Create();
				ret = VariantUtils.CreateFrom<NRainVfx>(ref nrainVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600188D RID: 6285 RVA: 0x000995C8 File Offset: 0x000977C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRainVfx.MethodName.Create || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600188E RID: 6286 RVA: 0x000995E0 File Offset: 0x000977E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x0600188F RID: 6287 RVA: 0x000995E9 File Offset: 0x000977E9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001101 RID: 4353
		public class MethodName : GpuParticles2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004273 RID: 17011
			public static readonly StringName Create = "Create";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001102 RID: 4354
		public class PropertyName : GpuParticles2D.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001103 RID: 4355
		public class SignalName : GpuParticles2D.SignalName
		{
		}
	}
}
