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
	/// Sent from the host to clients to indicate that a GenericHookGameAction has been enqueued.
	/// This is slightly different than ActionEnqueuedMessage. The Action itself is not serialized, because it is difficult
	/// to serialize every different action that could be executed by a hook. Since hooks are executed by every player,
	/// we instead map the hook to an incrementing integer that is the same across all peers, and use that to identify
	/// which hook will be assigned to the GameAction that is created.
	/// </summary>
	// Token: 0x02000473 RID: 1139
	public struct HookActionEnqueuedMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06004627 RID: 17959 RVA: 0x002181D3 File Offset: 0x002163D3
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06004628 RID: 17960 RVA: 0x002181D6 File Offset: 0x002163D6
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06004629 RID: 17961 RVA: 0x002181D9 File Offset: 0x002163D9
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Debug;
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x0600462A RID: 17962 RVA: 0x002181DC File Offset: 0x002163DC
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x0600462B RID: 17963 RVA: 0x002181DF File Offset: 0x002163DF
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x002181E7 File Offset: 0x002163E7
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<RunLocation>(this.location);
			writer.WriteULong(this.ownerId, 64);
			writer.WriteUInt(this.hookActionId, 32);
			writer.WriteEnum<GameActionType>(this.gameActionType);
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x0021821D File Offset: 0x0021641D
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.location = reader.Read<RunLocation>();
			this.ownerId = reader.ReadULong(64);
			this.hookActionId = reader.ReadUInt(32);
			this.gameActionType = reader.ReadEnum<GameActionType>();
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x00218254 File Offset: 0x00216454
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
			defaultInterpolatedStringHandler.AppendLiteral("HookActionEnqueuedMessage id: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.hookActionId);
			defaultInterpolatedStringHandler.AppendLiteral(" type: ");
			defaultInterpolatedStringHandler.AppendFormatted<GameActionType>(this.gameActionType);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04001B7A RID: 7034
		public RunLocation location;

		// Token: 0x04001B7B RID: 7035
		public ulong ownerId;

		// Token: 0x04001B7C RID: 7036
		public uint hookActionId;

		// Token: 0x04001B7D RID: 7037
		public GameActionType gameActionType;
	}
}
