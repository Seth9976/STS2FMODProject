using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent from host to all clients when a client has rejoined the game.
	/// </summary>
	// Token: 0x0200046F RID: 1135
	public struct PlayerRejoinedMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06004606 RID: 17926 RVA: 0x00217F18 File Offset: 0x00216118
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06004607 RID: 17927 RVA: 0x00217F1B File Offset: 0x0021611B
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06004608 RID: 17928 RVA: 0x00217F1E File Offset: 0x0021611E
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06004609 RID: 17929 RVA: 0x00217F21 File Offset: 0x00216121
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x00217F24 File Offset: 0x00216124
		public PlayerRejoinedMessage(ulong playerId)
		{
			this.playerId = playerId;
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x00217F2D File Offset: 0x0021612D
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteULong(this.playerId, 64);
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x00217F3D File Offset: 0x0021613D
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.playerId = reader.ReadULong(64);
		}

		// Token: 0x04001B73 RID: 7027
		public ulong playerId;
	}
}
