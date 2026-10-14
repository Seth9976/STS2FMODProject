using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001AD RID: 429
	[ScriptPath("res://src/Core/Nodes/Vfx/NFollowCursor.cs")]
	public class NFollowCursor : Node2D
	{
		// Token: 0x06001545 RID: 5445 RVA: 0x0008202C File Offset: 0x0008022C
		public override void _Process(double delta)
		{
			Vector2 mousePosition = base.GetViewport().GetMousePosition();
			base.GlobalPosition = mousePosition;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001546 RID: 5446 RVA: 0x0008204C File Offset: 0x0008024C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NFollowCursor.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001547 RID: 5447 RVA: 0x000820B5 File Offset: 0x000802B5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFollowCursor.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001548 RID: 5448 RVA: 0x000820F3 File Offset: 0x000802F3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFollowCursor.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001549 RID: 5449 RVA: 0x0008210B File Offset: 0x0008030B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x0600154A RID: 5450 RVA: 0x00082114 File Offset: 0x00080314
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001036 RID: 4150
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004020 RID: 16416
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001037 RID: 4151
		public class PropertyName : Node2D.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001038 RID: 4152
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
