using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.MapDrawing
{
	/// <summary>
	/// A serializable form of a single line that a player has drawn on the map.
	/// </summary>
	// Token: 0x020000F4 RID: 244
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableMapDrawingLine : IPacketSerializable
	{
		// Token: 0x06000AB0 RID: 2736 RVA: 0x0004955C File Offset: 0x0004775C
		public void Serialize(PacketWriter writer)
		{
			writer.WriteBool(this.isEraser);
			writer.WriteInt(this.mapPoints.Count, 16);
			foreach (Vector2 vector in this.mapPoints)
			{
				writer.WriteVector2(vector, new QuantizeParams?(SerializableMapDrawingLine._quantizeParamsX), new QuantizeParams?(SerializableMapDrawingLine._quantizeParamsY));
			}
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x000495E4 File Offset: 0x000477E4
		public void Deserialize(PacketReader reader)
		{
			this.isEraser = reader.ReadBool();
			int num = reader.ReadInt(16);
			for (int i = 0; i < num; i++)
			{
				this.mapPoints.Add(reader.ReadVector2(new QuantizeParams?(SerializableMapDrawingLine._quantizeParamsX), new QuantizeParams?(SerializableMapDrawingLine._quantizeParamsY)));
			}
		}

		// Token: 0x040003AF RID: 943
		private static readonly QuantizeParams _quantizeParamsX = new QuantizeParams(-3f, 3f, 13);

		// Token: 0x040003B0 RID: 944
		private static readonly QuantizeParams _quantizeParamsY = new QuantizeParams(-2f, 2f, 16);

		// Token: 0x040003B1 RID: 945
		public bool isEraser;

		// Token: 0x040003B2 RID: 946
		public List<Vector2> mapPoints = new List<Vector2>();
	}
}
