using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync
{
	/// <summary>
	/// Sent by the host to all clients when votes are received by all players during a shared event. This message specifies
	/// the option that all players should execute.
	/// </summary>
	// Token: 0x02000487 RID: 1159
	public struct SharedEventOptionChosenMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x060046AF RID: 18095 RVA: 0x00218D01 File Offset: 0x00216F01
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x060046B0 RID: 18096 RVA: 0x00218D04 File Offset: 0x00216F04
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x060046B1 RID: 18097 RVA: 0x00218D07 File Offset: 0x00216F07
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060046B2 RID: 18098 RVA: 0x00218D0A File Offset: 0x00216F0A
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x060046B3 RID: 18099 RVA: 0x00218D0D File Offset: 0x00216F0D
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x00218D15 File Offset: 0x00216F15
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteUInt(this.optionIndex, 4);
			writer.WriteUInt(this.pageIndex, 4);
			writer.Write<RunLocation>(this.location);
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x00218D3D File Offset: 0x00216F3D
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.optionIndex = reader.ReadUInt(4);
			this.pageIndex = reader.ReadUInt(4);
			this.location = reader.Read<RunLocation>();
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x00218D68 File Offset: 0x00216F68
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 3);
			defaultInterpolatedStringHandler.AppendFormatted("SharedEventOptionChosenMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" index ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.optionIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" page ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.pageIndex);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04001BAC RID: 7084
		public uint optionIndex;

		// Token: 0x04001BAD RID: 7085
		public uint pageIndex;

		// Token: 0x04001BAE RID: 7086
		public RunLocation location;
	}
}
