using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	/// <summary>
	/// Sent from the host to clients to indicate that a GameAction has been enqueued.
	/// This is one of the most important messages in the game. The game assumes that:
	/// - These messages are sent and received reliably
	/// - These messages are received in the order that they are sent
	/// - These messages are ordered correctly with ResumeActionAfterPlayerChoiceMessages
	/// </summary>
	// Token: 0x02000470 RID: 1136
	public struct ActionEnqueuedMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x0600460D RID: 17933 RVA: 0x00217F4D File Offset: 0x0021614D
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x0600460E RID: 17934 RVA: 0x00217F50 File Offset: 0x00216150
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x0600460F RID: 17935 RVA: 0x00217F53 File Offset: 0x00216153
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Debug;
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06004610 RID: 17936 RVA: 0x00217F56 File Offset: 0x00216156
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06004611 RID: 17937 RVA: 0x00217F59 File Offset: 0x00216159
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x00217F61 File Offset: 0x00216161
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteULong(this.playerId, 64);
			writer.Write<RunLocation>(this.location);
			writer.WriteByte((byte)this.action.ToId(), 8);
			writer.Write<INetAction>(this.action);
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x00217F9C File Offset: 0x0021619C
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.playerId = reader.ReadULong(64);
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

		// Token: 0x06004614 RID: 17940 RVA: 0x0021802C File Offset: 0x0021622C
		[NullableContext(1)]
		public override string ToString()
		{
			string text = "";
			INetAction netAction = this.action;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (netAction is NetPlayCardAction)
			{
				NetPlayCardAction netPlayCardAction = (NetPlayCardAction)netAction;
				CardModel cardModel = netPlayCardAction.card.ToCardModelOrNull();
				if (cardModel != null)
				{
					text = "(card: " + cardModel.Title + ")";
				}
				else
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
					defaultInterpolatedStringHandler.AppendLiteral("(Card ID ");
					defaultInterpolatedStringHandler.AppendFormatted<uint>(netPlayCardAction.card.CombatCardIndex);
					defaultInterpolatedStringHandler.AppendLiteral(" not found in database!)");
					text = defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 4);
			defaultInterpolatedStringHandler.AppendLiteral("ActionEnqueuedMessage PlayerID: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.playerId);
			defaultInterpolatedStringHandler.AppendLiteral(" Action: ");
			defaultInterpolatedStringHandler.AppendFormatted<INetAction>(this.action);
			defaultInterpolatedStringHandler.AppendLiteral(" Source Location: ");
			defaultInterpolatedStringHandler.AppendFormatted<RunLocation>(this.location);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04001B74 RID: 7028
		public ulong playerId;

		// Token: 0x04001B75 RID: 7029
		public RunLocation location;

		// Token: 0x04001B76 RID: 7030
		[Nullable(1)]
		public INetAction action;
	}
}
