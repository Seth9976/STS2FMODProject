using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;

namespace RiderTestRunner
{
	// Token: 0x02000018 RID: 24
	[ScriptPath("res://RiderTestRunner/NetCoreRunner.cs")]
	public class NetCoreRunner : Node
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000067B1 File Offset: 0x000049B1
		// (set) Token: 0x060000AD RID: 173 RVA: 0x000067B8 File Offset: 0x000049B8
		[Nullable(2)]
		public static NetCoreRunner Instance
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		/// <inheritdoc />
		// Token: 0x060000AE RID: 174 RVA: 0x000067C0 File Offset: 0x000049C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060000AF RID: 175 RVA: 0x000067C9 File Offset: 0x000049C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DD6 RID: 3542
		public class MethodName : Node.MethodName
		{
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DD7 RID: 3543
		public class PropertyName : Node.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DD8 RID: 3544
		public class SignalName : Node.SignalName
		{
		}
	}
}
