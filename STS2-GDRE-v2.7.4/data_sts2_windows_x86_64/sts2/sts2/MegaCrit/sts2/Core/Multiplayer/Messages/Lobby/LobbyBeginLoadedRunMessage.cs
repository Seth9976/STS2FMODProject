using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent when the <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby" /> closes and the run begins.
	/// Used only in <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby" />. Lobby uses <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyBeginRunMessage" />.
	/// </summary>
	// Token: 0x02000466 RID: 1126
	public struct LobbyBeginLoadedRunMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060045CF RID: 17871 RVA: 0x00217D26 File Offset: 0x00215F26
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060045D0 RID: 17872 RVA: 0x00217D29 File Offset: 0x00215F29
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060045D1 RID: 17873 RVA: 0x00217D2C File Offset: 0x00215F2C
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x060045D2 RID: 17874 RVA: 0x00217D2F File Offset: 0x00215F2F
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x00217D32 File Offset: 0x00215F32
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x00217D34 File Offset: 0x00215F34
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
		}
	}
}
