using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent from host to clients when a client has joined.
	/// The newly joined client receives ClientSlotGrantedMessage and not this message.
	/// </summary>
	// Token: 0x0200046C RID: 1132
	public struct PlayerJoinedMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x060045F3 RID: 17907 RVA: 0x00217E8F File Offset: 0x0021608F
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x060045F4 RID: 17908 RVA: 0x00217E92 File Offset: 0x00216092
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x060045F5 RID: 17909 RVA: 0x00217E95 File Offset: 0x00216095
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x060045F6 RID: 17910 RVA: 0x00217E98 File Offset: 0x00216098
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x00217E9B File Offset: 0x0021609B
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<LobbyPlayer>(this.lobbyPlayer);
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x00217EA9 File Offset: 0x002160A9
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.lobbyPlayer = reader.Read<LobbyPlayer>();
		}

		// Token: 0x04001B70 RID: 7024
		public LobbyPlayer lobbyPlayer;
	}
}
