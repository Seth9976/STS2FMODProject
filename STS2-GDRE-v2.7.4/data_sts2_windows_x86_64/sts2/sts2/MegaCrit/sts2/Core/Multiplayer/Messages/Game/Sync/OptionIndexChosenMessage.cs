using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync
{
	/// <summary>
	/// Used both in events and rest sites to synchronize the option chosen by a player.
	/// In events, optionIndex is the index of the currently displayed EventOption to choose.
	/// In rest sites, optionIndex is the index of the currently displayed RestSiteOption to choose.
	/// </summary>
	// Token: 0x02000480 RID: 1152
	public struct OptionIndexChosenMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x0600467F RID: 18047 RVA: 0x002186C4 File Offset: 0x002168C4
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06004680 RID: 18048 RVA: 0x002186C7 File Offset: 0x002168C7
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06004681 RID: 18049 RVA: 0x002186CA File Offset: 0x002168CA
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06004682 RID: 18050 RVA: 0x002186CD File Offset: 0x002168CD
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06004683 RID: 18051 RVA: 0x002186D0 File Offset: 0x002168D0
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x002186D8 File Offset: 0x002168D8
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteEnum<OptionIndexType>(this.type);
			writer.WriteUInt(this.optionIndex, 4);
			writer.Write<RunLocation>(this.location);
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x002186FF File Offset: 0x002168FF
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.type = reader.ReadEnum<OptionIndexType>();
			this.optionIndex = reader.ReadUInt(4);
			this.location = reader.Read<RunLocation>();
		}

		// Token: 0x06004686 RID: 18054 RVA: 0x00218728 File Offset: 0x00216928
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 3);
			defaultInterpolatedStringHandler.AppendFormatted("OptionIndexChosenMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" type ");
			defaultInterpolatedStringHandler.AppendFormatted<OptionIndexType>(this.type);
			defaultInterpolatedStringHandler.AppendLiteral(" index ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.optionIndex);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04001B91 RID: 7057
		public OptionIndexType type;

		// Token: 0x04001B92 RID: 7058
		public uint optionIndex;

		// Token: 0x04001B93 RID: 7059
		public RunLocation location;
	}
}
