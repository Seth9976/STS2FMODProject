using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	// Token: 0x02000449 RID: 1097
	[NullableContext(1)]
	public interface IPacketSerializable
	{
		// Token: 0x060044DC RID: 17628
		void Serialize(PacketWriter writer);

		// Token: 0x060044DD RID: 17629
		void Deserialize(PacketReader reader);
	}
}
