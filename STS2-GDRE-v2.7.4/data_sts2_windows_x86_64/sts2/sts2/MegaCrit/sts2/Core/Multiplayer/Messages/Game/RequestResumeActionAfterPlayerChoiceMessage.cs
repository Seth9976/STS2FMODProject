using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	/// <summary>
	/// Requests that the action currently paused on the sender's action queue be resumed.
	/// The action ID sent must be an action that is in the WaitingForPlayerChoice state.
	/// The GameAction should not be resumed until the host sends back a ResumeActionAfterPlayerChoiceMessage.
	/// </summary>
	// Token: 0x02000479 RID: 1145
	public struct RequestResumeActionAfterPlayerChoiceMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x0600464E RID: 17998 RVA: 0x002184C8 File Offset: 0x002166C8
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x0600464F RID: 17999 RVA: 0x002184CB File Offset: 0x002166CB
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06004650 RID: 18000 RVA: 0x002184CE File Offset: 0x002166CE
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06004651 RID: 18001 RVA: 0x002184D6 File Offset: 0x002166D6
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06004652 RID: 18002 RVA: 0x002184D9 File Offset: 0x002166D9
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x002184DC File Offset: 0x002166DC
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteUInt(this.actionId, 32);
			writer.Write<RunLocation>(this.location);
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x002184F8 File Offset: 0x002166F8
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.actionId = reader.ReadUInt(32);
			this.location = reader.Read<RunLocation>();
		}

		// Token: 0x04001B87 RID: 7047
		public uint actionId;

		// Token: 0x04001B88 RID: 7048
		public RunLocation location;
	}
}
