using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Events
{
	// Token: 0x0200022F RID: 559
	[ScriptPath("res://src/Core/Nodes/Vfx/Events/NSymbioteEyeMovementVfx.cs")]
	public class NSymbioteEyeMovementVfx : PathFollow2D
	{
		// Token: 0x06001C5F RID: 7263 RVA: 0x000B80D0 File Offset: 0x000B62D0
		public override void _Process(double delta)
		{
			base.ProgressRatio += (float)delta * 0.1f;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C60 RID: 7264 RVA: 0x000B80E8 File Offset: 0x000B62E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NSymbioteEyeMovementVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C61 RID: 7265 RVA: 0x000B8151 File Offset: 0x000B6351
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSymbioteEyeMovementVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001C62 RID: 7266 RVA: 0x000B818F File Offset: 0x000B638F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSymbioteEyeMovementVfx.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001C63 RID: 7267 RVA: 0x000B81A7 File Offset: 0x000B63A7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06001C64 RID: 7268 RVA: 0x000B81B0 File Offset: 0x000B63B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000AD6 RID: 2774
		private const float _speed = 0.1f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011E8 RID: 4584
		public class MethodName : PathFollow2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004580 RID: 17792
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011E9 RID: 4585
		public class PropertyName : PathFollow2D.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011EA RID: 4586
		public class SignalName : PathFollow2D.SignalName
		{
		}
	}
}
