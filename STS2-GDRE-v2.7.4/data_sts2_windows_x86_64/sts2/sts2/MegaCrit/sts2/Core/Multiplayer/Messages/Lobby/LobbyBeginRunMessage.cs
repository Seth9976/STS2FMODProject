using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent when the lobby closes and the run begins.
	/// </summary>
	// Token: 0x02000467 RID: 1127
	[NullableContext(1)]
	[Nullable(0)]
	public struct LobbyBeginRunMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060045D5 RID: 17877 RVA: 0x00217D36 File Offset: 0x00215F36
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x060045D6 RID: 17878 RVA: 0x00217D39 File Offset: 0x00215F39
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x060045D7 RID: 17879 RVA: 0x00217D3C File Offset: 0x00215F3C
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x060045D8 RID: 17880 RVA: 0x00217D3F File Offset: 0x00215F3F
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x00217D44 File Offset: 0x00215F44
		public void Serialize(PacketWriter writer)
		{
			if (this.playersInLobby == null)
			{
				throw new InvalidOperationException("Tried to serialize ClientSlotGrantedMessage with null list!");
			}
			writer.WriteList<LobbyPlayer>(this.playersInLobby, 3);
			writer.WriteString(this.seed);
			writer.WriteList<SerializableModifier>(this.modifiers, 32);
			writer.WriteString(this.act1);
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x00217D97 File Offset: 0x00215F97
		public void Deserialize(PacketReader reader)
		{
			this.playersInLobby = reader.ReadList<LobbyPlayer>(3);
			this.seed = reader.ReadString();
			this.modifiers = reader.ReadList<SerializableModifier>(32);
			this.act1 = reader.ReadString();
		}

		// Token: 0x04001B68 RID: 7016
		[Nullable(2)]
		public List<LobbyPlayer> playersInLobby;

		// Token: 0x04001B69 RID: 7017
		public string seed;

		// Token: 0x04001B6A RID: 7018
		public List<SerializableModifier> modifiers;

		// Token: 0x04001B6B RID: 7019
		public string act1;
	}
}
