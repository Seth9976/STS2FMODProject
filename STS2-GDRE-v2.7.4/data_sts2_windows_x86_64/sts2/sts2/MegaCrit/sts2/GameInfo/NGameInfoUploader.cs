using System;
using System.ComponentModel;
using Godot;
using Godot.Bridge;

namespace MegaCrit.Sts2.GameInfo
{
	// Token: 0x02000024 RID: 36
	[ScriptPath("res://src/GameInfo/NGameInfoUploader.cs")]
	public class NGameInfoUploader : Node
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00010E78 File Offset: 0x0000F078
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00010E7F File Offset: 0x0000F07F
		public static bool IsRunning { get; private set; }

		/// <inheritdoc />
		// Token: 0x06000185 RID: 389 RVA: 0x00010E87 File Offset: 0x0000F087
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06000186 RID: 390 RVA: 0x00010E90 File Offset: 0x0000F090
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000E01 RID: 3585
		public class MethodName : Node.MethodName
		{
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000E02 RID: 3586
		public class PropertyName : Node.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000E03 RID: 3587
		public class SignalName : Node.SignalName
		{
		}
	}
}
