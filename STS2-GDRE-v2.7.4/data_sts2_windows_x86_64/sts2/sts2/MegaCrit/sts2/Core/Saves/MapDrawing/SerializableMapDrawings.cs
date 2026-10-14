using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.MapDrawing
{
	/// <summary>
	/// A serializable form of the drawings that all players have drawn on the map.
	/// </summary>
	// Token: 0x020000F5 RID: 245
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableMapDrawings : IPacketSerializable
	{
		// Token: 0x06000AB4 RID: 2740 RVA: 0x00049678 File Offset: 0x00047878
		public void Serialize(PacketWriter writer)
		{
			writer.WriteList<SerializablePlayerMapDrawings>(this.drawings, 32);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00049688 File Offset: 0x00047888
		public void Deserialize(PacketReader reader)
		{
			this.drawings = reader.ReadList<SerializablePlayerMapDrawings>(32);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00049698 File Offset: 0x00047898
		public SerializableMapDrawings Anonymized()
		{
			SerializableMapDrawings serializableMapDrawings = new SerializableMapDrawings();
			serializableMapDrawings.drawings = this.drawings.Select<SerializablePlayerMapDrawings, SerializablePlayerMapDrawings>((SerializablePlayerMapDrawings d) => d.Anonymized()).ToList<SerializablePlayerMapDrawings>();
			return serializableMapDrawings;
		}

		// Token: 0x040003B3 RID: 947
		public List<SerializablePlayerMapDrawings> drawings = new List<SerializablePlayerMapDrawings>();
	}
}
