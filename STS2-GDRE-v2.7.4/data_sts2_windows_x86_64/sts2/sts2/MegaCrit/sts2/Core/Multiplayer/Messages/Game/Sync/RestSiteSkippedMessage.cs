using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync
{
	/// <summary>
	/// Sent when a player is exiting a rest site with options remaining.
	/// When a rest site room is exiting, players must wait to receive this message from players who still have options
	/// remaining before generating the room-exit checksum. Otherwise, there may be a race between that checksum and another
	/// player's rest site choice.
	/// </summary>
	// Token: 0x02000483 RID: 1155
	public struct RestSiteSkippedMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x0600468F RID: 18063 RVA: 0x00218923 File Offset: 0x00216B23
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06004690 RID: 18064 RVA: 0x00218926 File Offset: 0x00216B26
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06004691 RID: 18065 RVA: 0x00218929 File Offset: 0x00216B29
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06004692 RID: 18066 RVA: 0x0021892C File Offset: 0x00216B2C
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06004693 RID: 18067 RVA: 0x0021892F File Offset: 0x00216B2F
		// (set) Token: 0x06004694 RID: 18068 RVA: 0x00218937 File Offset: 0x00216B37
		public RunLocation Location { readonly get; set; }

		// Token: 0x06004695 RID: 18069 RVA: 0x00218940 File Offset: 0x00216B40
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<RunLocation>(this.Location);
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x0021894E File Offset: 0x00216B4E
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.Location = reader.Read<RunLocation>();
		}
	}
}
