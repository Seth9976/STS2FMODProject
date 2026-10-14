using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	/// <summary>
	/// Sent from the clients to host too request that a hook action be enqueued.
	/// It is implicitly assumed that the owner of the hook action to be enqueued is the sender.
	/// This is slightly different than RequestEnqueueActionMessage. The Action itself is not serialized, because it is
	/// difficult to serialize every different action that could be executed by a hook. Since hooks are executed by every
	/// player, we instead map the hook to an incrementing integer that is the same across all peers, and use that to
	/// identify which hook will be assigned to the GameAction that is created.
	/// </summary>
	// Token: 0x02000478 RID: 1144
	public struct RequestEnqueueHookActionMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06004647 RID: 17991 RVA: 0x00218464 File Offset: 0x00216664
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06004648 RID: 17992 RVA: 0x00218467 File Offset: 0x00216667
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06004649 RID: 17993 RVA: 0x0021846A File Offset: 0x0021666A
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x0600464A RID: 17994 RVA: 0x00218472 File Offset: 0x00216672
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x0600464B RID: 17995 RVA: 0x00218475 File Offset: 0x00216675
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x00218478 File Offset: 0x00216678
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<RunLocation>(this.location);
			writer.WriteUInt(this.hookActionId, 32);
			writer.WriteEnum<GameActionType>(this.gameActionType);
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x002184A0 File Offset: 0x002166A0
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.location = reader.Read<RunLocation>();
			this.hookActionId = reader.ReadUInt(32);
			this.gameActionType = reader.ReadEnum<GameActionType>();
		}

		// Token: 0x04001B84 RID: 7044
		public RunLocation location;

		// Token: 0x04001B85 RID: 7045
		public uint hookActionId;

		// Token: 0x04001B86 RID: 7046
		public GameActionType gameActionType;
	}
}
