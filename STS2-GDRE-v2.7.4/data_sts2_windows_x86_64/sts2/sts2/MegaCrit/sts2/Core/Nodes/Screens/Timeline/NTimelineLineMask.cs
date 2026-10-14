using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	// Token: 0x02000264 RID: 612
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NTimelineLineMask.cs")]
	public class NTimelineLineMask : Control
	{
		// Token: 0x0600200B RID: 8203 RVA: 0x000D8814 File Offset: 0x000D6A14
		public override void _Process(double delta)
		{
			base._Process(delta);
			base.Position = Vector2.Zero;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600200C RID: 8204 RVA: 0x000D8828 File Offset: 0x000D6A28
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NTimelineLineMask.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600200D RID: 8205 RVA: 0x000D8891 File Offset: 0x000D6A91
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTimelineLineMask.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600200E RID: 8206 RVA: 0x000D88CF File Offset: 0x000D6ACF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTimelineLineMask.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600200F RID: 8207 RVA: 0x000D88E7 File Offset: 0x000D6AE7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002010 RID: 8208 RVA: 0x000D88F0 File Offset: 0x000D6AF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012BE RID: 4798
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004901 RID: 18689
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012BF RID: 4799
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012C0 RID: 4800
		public class SignalName : Control.SignalName
		{
		}
	}
}
