using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	/// <summary>
	/// Sent at a treasure room when a remote player opens the treasure chest.
	/// </summary>
	// Token: 0x0200047E RID: 1150
	public class TreasureChestOpenedMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x0600466E RID: 18030 RVA: 0x002185D8 File Offset: 0x002167D8
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x0600466F RID: 18031 RVA: 0x002185DB File Offset: 0x002167DB
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06004670 RID: 18032 RVA: 0x002185DE File Offset: 0x002167DE
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Debug;
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06004671 RID: 18033 RVA: 0x002185E1 File Offset: 0x002167E1
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06004672 RID: 18034 RVA: 0x002185E4 File Offset: 0x002167E4
		// (set) Token: 0x06004673 RID: 18035 RVA: 0x002185EC File Offset: 0x002167EC
		public RunLocation Location { get; set; }

		// Token: 0x06004674 RID: 18036 RVA: 0x002185F5 File Offset: 0x002167F5
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<RunLocation>(this.Location);
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x00218603 File Offset: 0x00216803
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.Location = reader.Read<RunLocation>();
		}
	}
}
