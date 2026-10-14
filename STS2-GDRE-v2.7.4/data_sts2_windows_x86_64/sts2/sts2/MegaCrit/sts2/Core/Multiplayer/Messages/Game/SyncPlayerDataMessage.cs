using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	/// <summary>
	/// Broadcast to all peers at the start of combat to sync player data (deck, HP, max potion slots, etc).
	/// </summary>
	// Token: 0x0200047C RID: 1148
	public struct SyncPlayerDataMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06004662 RID: 18018 RVA: 0x00218570 File Offset: 0x00216770
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06004663 RID: 18019 RVA: 0x00218573 File Offset: 0x00216773
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06004664 RID: 18020 RVA: 0x00218576 File Offset: 0x00216776
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06004665 RID: 18021 RVA: 0x00218579 File Offset: 0x00216779
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x0021857C File Offset: 0x0021677C
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<SerializablePlayer>(this.player);
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x0021858A File Offset: 0x0021678A
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.player = reader.Read<SerializablePlayer>();
		}

		// Token: 0x04001B8B RID: 7051
		[Nullable(1)]
		public SerializablePlayer player;
	}
}
