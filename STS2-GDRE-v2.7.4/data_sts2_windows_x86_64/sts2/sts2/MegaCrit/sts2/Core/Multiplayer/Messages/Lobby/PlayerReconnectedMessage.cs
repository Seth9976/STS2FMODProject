using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent from host to clients when a client has reconnected, either in <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby" /> or during a run.
	/// The newly joined client receives ClientRejoinResponseMessage and not this message.
	/// </summary>
	// Token: 0x0200046E RID: 1134
	public struct PlayerReconnectedMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06004600 RID: 17920 RVA: 0x00217EEC File Offset: 0x002160EC
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06004601 RID: 17921 RVA: 0x00217EEF File Offset: 0x002160EF
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06004602 RID: 17922 RVA: 0x00217EF2 File Offset: 0x002160F2
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06004603 RID: 17923 RVA: 0x00217EF5 File Offset: 0x002160F5
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x00217EF8 File Offset: 0x002160F8
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteULong(this.playerId, 64);
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x00217F08 File Offset: 0x00216108
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.playerId = reader.ReadULong(64);
		}

		// Token: 0x04001B72 RID: 7026
		public ulong playerId;
	}
}
