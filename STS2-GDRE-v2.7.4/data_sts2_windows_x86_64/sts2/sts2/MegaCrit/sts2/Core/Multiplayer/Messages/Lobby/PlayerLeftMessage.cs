using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent from host to all clients when a client has disconnected from the lobby.
	/// </summary>
	// Token: 0x0200046D RID: 1133
	public struct PlayerLeftMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x060045F9 RID: 17913 RVA: 0x00217EB7 File Offset: 0x002160B7
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x060045FA RID: 17914 RVA: 0x00217EBA File Offset: 0x002160BA
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x060045FB RID: 17915 RVA: 0x00217EBD File Offset: 0x002160BD
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x060045FC RID: 17916 RVA: 0x00217EC0 File Offset: 0x002160C0
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x00217EC3 File Offset: 0x002160C3
		public PlayerLeftMessage(ulong playerId)
		{
			this.playerId = playerId;
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x00217ECC File Offset: 0x002160CC
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteULong(this.playerId, 64);
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x00217EDC File Offset: 0x002160DC
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.playerId = reader.ReadULong(64);
		}

		// Token: 0x04001B71 RID: 7025
		public ulong playerId;
	}
}
