using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent by a client to the host as their first message, requesting that they join a loaded lobby.
	/// Only sent if the lobby was started from a loaded game.
	/// </summary>
	// Token: 0x0200045E RID: 1118
	public struct ClientLoadJoinRequestMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x0600459D RID: 17821 RVA: 0x0021775B File Offset: 0x0021595B
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x0600459E RID: 17822 RVA: 0x0021775E File Offset: 0x0021595E
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x0600459F RID: 17823 RVA: 0x00217761 File Offset: 0x00215961
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Info;
			}
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x060045A0 RID: 17824 RVA: 0x00217764 File Offset: 0x00215964
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x00217767 File Offset: 0x00215967
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x00217769 File Offset: 0x00215969
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
		}
	}
}
