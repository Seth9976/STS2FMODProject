using System;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	// Token: 0x020002EC RID: 748
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NBaseMapDrawingButton.cs")]
	public class NBaseMapDrawingButton : NButton
	{
		/// <inheritdoc />
		// Token: 0x0600282E RID: 10286 RVA: 0x001137A5 File Offset: 0x001119A5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x0600282F RID: 10287 RVA: 0x001137AE File Offset: 0x001119AE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001495 RID: 5269
		public new class MethodName : NButton.MethodName
		{
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001496 RID: 5270
		public new class PropertyName : NButton.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001497 RID: 5271
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
