using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	/// <summary>
	/// Sent from the host to the client. Informs clients that an action has been resumed after it was placed in the
	/// WaitingForPlayerChoice state.
	/// Note that this message and ActionEnqueuedMessage must be received in the same order that they are sent.
	/// </summary>
	// Token: 0x0200047A RID: 1146
	public struct ResumeActionAfterPlayerChoiceMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06004655 RID: 18005 RVA: 0x00218514 File Offset: 0x00216714
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06004656 RID: 18006 RVA: 0x00218517 File Offset: 0x00216717
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06004657 RID: 18007 RVA: 0x0021851A File Offset: 0x0021671A
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06004658 RID: 18008 RVA: 0x00218522 File Offset: 0x00216722
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06004659 RID: 18009 RVA: 0x00218525 File Offset: 0x00216725
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x00218528 File Offset: 0x00216728
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteUInt(this.actionId, 32);
			writer.Write<RunLocation>(this.location);
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x00218544 File Offset: 0x00216744
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.actionId = reader.ReadUInt(32);
			this.location = reader.Read<RunLocation>();
		}

		// Token: 0x04001B89 RID: 7049
		public uint actionId;

		// Token: 0x04001B8A RID: 7050
		public RunLocation location;
	}
}
