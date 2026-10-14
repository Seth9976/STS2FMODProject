using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent by a host to a client in response to a ClientLoadJoinRequestMessage.
	/// </summary>
	// Token: 0x0200045F RID: 1119
	[NullableContext(1)]
	[Nullable(0)]
	public struct ClientLoadJoinResponseMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x060045A3 RID: 17827 RVA: 0x0021776B File Offset: 0x0021596B
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060045A4 RID: 17828 RVA: 0x0021776E File Offset: 0x0021596E
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060045A5 RID: 17829 RVA: 0x00217771 File Offset: 0x00215971
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Info;
			}
		}

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x060045A6 RID: 17830 RVA: 0x00217774 File Offset: 0x00215974
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x00217778 File Offset: 0x00215978
		public void Serialize(PacketWriter writer)
		{
			writer.Write<SerializableRun>(this.serializableRun);
			writer.WriteInt(this.playersAlreadyConnected.Count, 6);
			foreach (ulong num in this.playersAlreadyConnected)
			{
				writer.WriteULong(num, 64);
			}
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x002177EC File Offset: 0x002159EC
		public void Deserialize(PacketReader reader)
		{
			this.serializableRun = reader.Read<SerializableRun>();
			this.playersAlreadyConnected = new List<ulong>();
			int num = reader.ReadInt(6);
			for (int i = 0; i < num; i++)
			{
				this.playersAlreadyConnected.Add(reader.ReadULong(64));
			}
		}

		// Token: 0x04001B54 RID: 6996
		public SerializableRun serializableRun;

		// Token: 0x04001B55 RID: 6997
		public List<ulong> playersAlreadyConnected;
	}
}
