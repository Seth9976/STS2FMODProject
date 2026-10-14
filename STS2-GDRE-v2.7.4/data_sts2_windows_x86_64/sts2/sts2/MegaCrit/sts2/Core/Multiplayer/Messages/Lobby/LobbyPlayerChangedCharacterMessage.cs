using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Broadcast to all peers when a player in the lobby changes their selected character.
	/// </summary>
	// Token: 0x02000469 RID: 1129
	public struct LobbyPlayerChangedCharacterMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060045E1 RID: 17889 RVA: 0x00217DF8 File Offset: 0x00215FF8
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x060045E2 RID: 17890 RVA: 0x00217DFB File Offset: 0x00215FFB
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x060045E3 RID: 17891 RVA: 0x00217DFE File Offset: 0x00215FFE
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x060045E4 RID: 17892 RVA: 0x00217E01 File Offset: 0x00216001
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x00217E04 File Offset: 0x00216004
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteModel(this.character);
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x00217E12 File Offset: 0x00216012
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.character = reader.ReadModel<CharacterModel>();
		}

		// Token: 0x04001B6D RID: 7021
		[Nullable(1)]
		public CharacterModel character;
	}
}
