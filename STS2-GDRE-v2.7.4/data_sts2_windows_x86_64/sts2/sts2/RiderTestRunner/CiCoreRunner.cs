using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;

namespace RiderTestRunner
{
	// Token: 0x02000017 RID: 23
	[ScriptPath("res://RiderTestRunner/CiCoreRunner.cs")]
	public class CiCoreRunner : Node
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00006788 File Offset: 0x00004988
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x0000678F File Offset: 0x0000498F
		[Nullable(2)]
		public static CiCoreRunner Instance
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		/// <inheritdoc />
		// Token: 0x060000A9 RID: 169 RVA: 0x00006797 File Offset: 0x00004997
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060000AA RID: 170 RVA: 0x000067A0 File Offset: 0x000049A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DD3 RID: 3539
		public class MethodName : Node.MethodName
		{
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DD4 RID: 3540
		public class PropertyName : Node.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DD5 RID: 3541
		public class SignalName : Node.SignalName
		{
		}
	}
}
