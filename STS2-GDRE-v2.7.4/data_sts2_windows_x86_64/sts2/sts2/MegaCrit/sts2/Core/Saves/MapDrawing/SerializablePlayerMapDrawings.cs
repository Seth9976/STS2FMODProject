using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.MapDrawing
{
	/// <summary>
	/// A serializable form of the drawings that a single player has drawn on the map.
	/// There are multiple of these in the save in multiplayer.
	/// </summary>
	// Token: 0x020000F7 RID: 247
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializablePlayerMapDrawings : IPacketSerializable
	{
		// Token: 0x06000ABC RID: 2748 RVA: 0x00049858 File Offset: 0x00047A58
		public void Serialize(PacketWriter writer)
		{
			writer.WriteULong(this.playerId, 64);
			writer.WriteList<SerializableMapDrawingLine>(this.lines, 32);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00049876 File Offset: 0x00047A76
		public void Deserialize(PacketReader reader)
		{
			this.playerId = reader.ReadULong(64);
			this.lines = reader.ReadList<SerializableMapDrawingLine>(32);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00049894 File Offset: 0x00047A94
		public SerializablePlayerMapDrawings Anonymized()
		{
			return new SerializablePlayerMapDrawings
			{
				playerId = IdAnonymizer.Anonymize(this.playerId),
				lines = this.lines
			};
		}

		// Token: 0x040003B5 RID: 949
		public ulong playerId;

		// Token: 0x040003B6 RID: 950
		public List<SerializableMapDrawingLine> lines = new List<SerializableMapDrawingLine>();
	}
}
