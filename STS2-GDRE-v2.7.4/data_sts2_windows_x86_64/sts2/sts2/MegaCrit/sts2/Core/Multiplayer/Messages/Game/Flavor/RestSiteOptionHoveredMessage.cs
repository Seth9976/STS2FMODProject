using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor
{
	// Token: 0x0200048F RID: 1167
	[RequiredMember]
	public struct RestSiteOptionHoveredMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x060046E8 RID: 18152 RVA: 0x00219045 File Offset: 0x00217245
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x060046E9 RID: 18153 RVA: 0x00219048 File Offset: 0x00217248
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Unreliable;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x060046EA RID: 18154 RVA: 0x0021904B File Offset: 0x0021724B
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x060046EB RID: 18155 RVA: 0x0021904E File Offset: 0x0021724E
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x060046EC RID: 18156 RVA: 0x00219051 File Offset: 0x00217251
		// (set) Token: 0x060046ED RID: 18157 RVA: 0x00219059 File Offset: 0x00217259
		public RunLocation Location { readonly get; set; }

		// Token: 0x060046EE RID: 18158 RVA: 0x00219062 File Offset: 0x00217262
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteBool(this.optionIndex != null);
			if (this.optionIndex != null)
			{
				writer.WriteUInt(this.optionIndex.Value, 5);
			}
			writer.Write<RunLocation>(this.Location);
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x002190A0 File Offset: 0x002172A0
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			if (reader.ReadBool())
			{
				this.optionIndex = new uint?(reader.ReadUInt(5));
			}
			this.Location = reader.Read<RunLocation>();
		}

		// Token: 0x04001BBC RID: 7100
		[RequiredMember]
		public uint? optionIndex;
	}
}
