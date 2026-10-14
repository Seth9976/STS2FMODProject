using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	/// <summary>
	/// Sent from clients to host when they initiate a GameAction.
	/// The GameAction must not be enqueued until the host sends back an ActionEnqueuedMessage, to preserve determinism.
	/// </summary>
	// Token: 0x02000477 RID: 1143
	public struct RequestEnqueueActionMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x0600463F RID: 17983 RVA: 0x00218345 File Offset: 0x00216545
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06004640 RID: 17984 RVA: 0x00218348 File Offset: 0x00216548
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06004641 RID: 17985 RVA: 0x0021834B File Offset: 0x0021654B
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06004642 RID: 17986 RVA: 0x00218353 File Offset: 0x00216553
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06004643 RID: 17987 RVA: 0x00218356 File Offset: 0x00216556
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x00218359 File Offset: 0x00216559
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<RunLocation>(this.location);
			writer.WriteByte((byte)this.action.ToId(), 8);
			writer.Write<INetAction>(this.action);
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x00218388 File Offset: 0x00216588
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.location = reader.Read<RunLocation>();
			int num = (int)reader.ReadByte(8);
			Type type;
			if (!ActionTypes.TryGetActionType(num, out type))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received net action of type ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" that does not map to any type!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.action = (INetAction)Activator.CreateInstance(type);
			this.action.Deserialize(reader);
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x00218408 File Offset: 0x00216608
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 3);
			defaultInterpolatedStringHandler.AppendFormatted("RequestEnqueueActionMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" location: ");
			defaultInterpolatedStringHandler.AppendFormatted<RunLocation>(this.location);
			defaultInterpolatedStringHandler.AppendLiteral(" action: ");
			defaultInterpolatedStringHandler.AppendFormatted<INetAction>(this.action);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04001B82 RID: 7042
		public RunLocation location;

		// Token: 0x04001B83 RID: 7043
		[Nullable(1)]
		public INetAction action;
	}
}
