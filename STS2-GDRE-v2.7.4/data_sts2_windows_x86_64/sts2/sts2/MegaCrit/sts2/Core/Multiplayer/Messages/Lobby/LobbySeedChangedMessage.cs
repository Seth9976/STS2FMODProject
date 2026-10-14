using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent by the host when the overridden seed in the custom run menu changes.
	/// </summary>
	// Token: 0x0200046B RID: 1131
	public struct LobbySeedChangedMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x060045ED RID: 17901 RVA: 0x00217E48 File Offset: 0x00216048
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060045EE RID: 17902 RVA: 0x00217E4B File Offset: 0x0021604B
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x060045EF RID: 17903 RVA: 0x00217E4E File Offset: 0x0021604E
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x060045F0 RID: 17904 RVA: 0x00217E51 File Offset: 0x00216051
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x00217E54 File Offset: 0x00216054
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteBool(this.seed != null);
			if (this.seed != null)
			{
				writer.WriteString(this.seed);
			}
		}

		// Token: 0x060045F2 RID: 17906 RVA: 0x00217E79 File Offset: 0x00216079
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			if (reader.ReadBool())
			{
				this.seed = reader.ReadString();
			}
		}

		// Token: 0x04001B6F RID: 7023
		[Nullable(2)]
		public string seed;
	}
}
