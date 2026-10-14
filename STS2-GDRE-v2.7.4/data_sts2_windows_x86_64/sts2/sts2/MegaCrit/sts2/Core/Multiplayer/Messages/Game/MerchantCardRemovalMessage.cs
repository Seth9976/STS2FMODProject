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
	// Token: 0x02000475 RID: 1141
	public class MerchantCardRemovalMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06004630 RID: 17968 RVA: 0x002182A4 File Offset: 0x002164A4
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06004631 RID: 17969 RVA: 0x002182A7 File Offset: 0x002164A7
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06004632 RID: 17970 RVA: 0x002182AA File Offset: 0x002164AA
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06004633 RID: 17971 RVA: 0x002182AD File Offset: 0x002164AD
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06004634 RID: 17972 RVA: 0x002182B0 File Offset: 0x002164B0
		// (set) Token: 0x06004635 RID: 17973 RVA: 0x002182B8 File Offset: 0x002164B8
		public RunLocation Location { get; set; }

		// Token: 0x06004636 RID: 17974 RVA: 0x002182C1 File Offset: 0x002164C1
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.goldCost, 32);
			writer.Write<RunLocation>(this.Location);
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x002182DD File Offset: 0x002164DD
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.goldCost = reader.ReadInt(32);
			this.Location = reader.Read<RunLocation>();
		}

		// Token: 0x04001B7F RID: 7039
		public int goldCost;
	}
}
