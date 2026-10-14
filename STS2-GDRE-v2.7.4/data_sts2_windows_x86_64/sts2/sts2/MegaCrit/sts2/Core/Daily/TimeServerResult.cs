using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Daily
{
	// Token: 0x02000D1B RID: 3355
	public struct TimeServerResult : IPacketSerializable
	{
		// Token: 0x0600875E RID: 34654 RVA: 0x002D8967 File Offset: 0x002D6B67
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteLong(this.serverTime.ToUnixTimeSeconds(), 64);
			writer.WriteLong(this.localReceivedTime.ToUnixTimeSeconds(), 64);
		}

		// Token: 0x0600875F RID: 34655 RVA: 0x002D898F File Offset: 0x002D6B8F
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.serverTime = DateTimeOffset.FromUnixTimeSeconds(reader.ReadLong(64));
			this.localReceivedTime = DateTimeOffset.FromUnixTimeSeconds(reader.ReadLong(64));
		}

		/// <summary>
		/// The time that was returned from the time server.
		/// </summary>
		// Token: 0x04002EF9 RID: 12025
		public DateTimeOffset serverTime;

		/// <summary>
		/// The local time at which we requested the time from the server.
		/// </summary>
		// Token: 0x04002EFA RID: 12026
		public DateTimeOffset localReceivedTime;
	}
}
