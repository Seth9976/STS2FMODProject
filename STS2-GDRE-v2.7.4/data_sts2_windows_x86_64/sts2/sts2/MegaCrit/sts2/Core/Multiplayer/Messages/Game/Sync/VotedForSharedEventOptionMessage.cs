using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync
{
	/// <summary>
	/// Broadcast from peers to all other peers when they vote on an event option during a shared event.
	/// This only records the votes; the host is the one that ultimately chooses the event option and sends it via
	/// SharedEventOptionChosenMessage.
	/// </summary>
	// Token: 0x02000488 RID: 1160
	public struct VotedForSharedEventOptionMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x060046B7 RID: 18103 RVA: 0x00218DC4 File Offset: 0x00216FC4
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x060046B8 RID: 18104 RVA: 0x00218DC7 File Offset: 0x00216FC7
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x060046B9 RID: 18105 RVA: 0x00218DCA File Offset: 0x00216FCA
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x060046BA RID: 18106 RVA: 0x00218DCD File Offset: 0x00216FCD
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x060046BB RID: 18107 RVA: 0x00218DD0 File Offset: 0x00216FD0
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x00218DD8 File Offset: 0x00216FD8
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteUInt(this.optionIndex, 4);
			writer.WriteUInt(this.pageIndex, 4);
			writer.Write<RunLocation>(this.location);
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x00218E00 File Offset: 0x00217000
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.optionIndex = reader.ReadUInt(4);
			this.pageIndex = reader.ReadUInt(4);
			this.location = reader.Read<RunLocation>();
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x00218E28 File Offset: 0x00217028
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 3);
			defaultInterpolatedStringHandler.AppendFormatted("VotedForSharedEventOptionMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" index ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.optionIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" page ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.pageIndex);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04001BAF RID: 7087
		public uint optionIndex;

		// Token: 0x04001BB0 RID: 7088
		public uint pageIndex;

		// Token: 0x04001BB1 RID: 7089
		public RunLocation location;
	}
}
