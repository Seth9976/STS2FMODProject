using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Checksums
{
	/// <summary>
	/// Sent from clients to host when a checksum is generated. The host should validate the checksum. No action is needed
	/// if the checksum matches the host's, but if it does not, a StateDivergenceMessage should be generated and sent to
	/// the client.
	/// </summary>
	// Token: 0x02000490 RID: 1168
	public struct ChecksumDataMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x060046F0 RID: 18160 RVA: 0x002190C8 File Offset: 0x002172C8
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x060046F1 RID: 18161 RVA: 0x002190CB File Offset: 0x002172CB
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x060046F2 RID: 18162 RVA: 0x002190CE File Offset: 0x002172CE
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x060046F3 RID: 18163 RVA: 0x002190D1 File Offset: 0x002172D1
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x002190D4 File Offset: 0x002172D4
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<NetChecksumData>(this.checksumData);
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x002190E2 File Offset: 0x002172E2
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.checksumData = reader.Read<NetChecksumData>();
		}

		// Token: 0x04001BBD RID: 7101
		public NetChecksumData checksumData;
	}
}
