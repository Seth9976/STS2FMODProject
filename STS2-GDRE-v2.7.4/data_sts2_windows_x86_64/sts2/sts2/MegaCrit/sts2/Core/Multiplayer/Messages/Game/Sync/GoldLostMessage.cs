using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync
{
	// Token: 0x0200047F RID: 1151
	[RequiredMember]
	public struct GoldLostMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06004677 RID: 18039 RVA: 0x00218619 File Offset: 0x00216819
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06004678 RID: 18040 RVA: 0x0021861C File Offset: 0x0021681C
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06004679 RID: 18041 RVA: 0x0021861F File Offset: 0x0021681F
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x0600467A RID: 18042 RVA: 0x00218622 File Offset: 0x00216822
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x0600467B RID: 18043 RVA: 0x00218625 File Offset: 0x00216825
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x0021862D File Offset: 0x0021682D
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.goldLost, 32);
			writer.Write<RunLocation>(this.location);
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x00218649 File Offset: 0x00216849
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.goldLost = reader.ReadInt(32);
			this.location = reader.Read<RunLocation>();
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x00218668 File Offset: 0x00216868
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 3);
			defaultInterpolatedStringHandler.AppendFormatted("GoldLostMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" gold: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.goldLost);
			defaultInterpolatedStringHandler.AppendLiteral(" location: ");
			defaultInterpolatedStringHandler.AppendFormatted<RunLocation>(this.location);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04001B8F RID: 7055
		[RequiredMember]
		public int goldLost;

		// Token: 0x04001B90 RID: 7056
		[RequiredMember]
		public RunLocation location;
	}
}
