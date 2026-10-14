using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	/// <summary>
	/// Sent when a player begins removing a card at the merchant.
	/// </summary>
	// Token: 0x02000471 RID: 1137
	public class CardRemovedMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06004615 RID: 17941 RVA: 0x0021812A File Offset: 0x0021632A
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06004616 RID: 17942 RVA: 0x0021812D File Offset: 0x0021632D
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06004617 RID: 17943 RVA: 0x00218130 File Offset: 0x00216330
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06004618 RID: 17944 RVA: 0x00218133 File Offset: 0x00216333
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06004619 RID: 17945 RVA: 0x00218136 File Offset: 0x00216336
		// (set) Token: 0x0600461A RID: 17946 RVA: 0x0021813E File Offset: 0x0021633E
		public RunLocation Location { get; set; }

		// Token: 0x0600461B RID: 17947 RVA: 0x00218147 File Offset: 0x00216347
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<RunLocation>(this.Location);
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x00218155 File Offset: 0x00216355
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.Location = reader.Read<RunLocation>();
		}
	}
}
