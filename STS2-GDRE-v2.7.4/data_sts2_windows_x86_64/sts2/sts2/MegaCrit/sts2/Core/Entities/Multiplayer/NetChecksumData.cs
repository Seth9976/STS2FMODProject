using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	// Token: 0x02000C85 RID: 3205
	public struct NetChecksumData : IPacketSerializable
	{
		// Token: 0x0600835E RID: 33630 RVA: 0x002C4B16 File Offset: 0x002C2D16
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteUInt(this.id, 32);
			writer.WriteUInt(this.checksum, 32);
		}

		// Token: 0x0600835F RID: 33631 RVA: 0x002C4B34 File Offset: 0x002C2D34
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.id = reader.ReadUInt(32);
			this.checksum = reader.ReadUInt(32);
		}

		// Token: 0x04002C90 RID: 11408
		public uint id;

		// Token: 0x04002C91 RID: 11409
		public uint checksum;
	}
}
