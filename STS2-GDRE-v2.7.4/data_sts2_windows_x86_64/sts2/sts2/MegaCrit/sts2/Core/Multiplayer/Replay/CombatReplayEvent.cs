using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Multiplayer.Replay
{
	// Token: 0x02000455 RID: 1109
	public struct CombatReplayEvent : IPacketSerializable
	{
		// Token: 0x06004558 RID: 17752 RVA: 0x002162A4 File Offset: 0x002144A4
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt((int)this.eventType, 3);
			switch (this.eventType)
			{
			case CombatReplayEventType.GameAction:
				writer.WriteULong(this.playerId.Value, 64);
				writer.WriteByte((byte)this.action.ToId(), 8);
				writer.Write<INetAction>(this.action);
				return;
			case CombatReplayEventType.HookAction:
				writer.WriteULong(this.playerId.Value, 64);
				writer.WriteUInt(this.hookId.Value, 32);
				writer.WriteEnum<GameActionType>(this.gameActionType.Value);
				return;
			case CombatReplayEventType.ResumeAction:
				writer.WriteUInt(this.actionId.Value, 32);
				return;
			case CombatReplayEventType.PlayerChoice:
				writer.WriteULong(this.playerId.Value, 64);
				writer.WriteUInt(this.choiceId.Value, 32);
				writer.Write<NetPlayerChoiceResult>(this.playerChoiceResult.Value);
				return;
			default:
				throw new ArgumentOutOfRangeException("eventType");
			}
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x002163A4 File Offset: 0x002145A4
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.eventType = (CombatReplayEventType)reader.ReadInt(3);
			switch (this.eventType)
			{
			case CombatReplayEventType.GameAction:
			{
				this.playerId = new ulong?(reader.ReadULong(64));
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
				return;
			}
			case CombatReplayEventType.HookAction:
				this.playerId = new ulong?(reader.ReadULong(64));
				this.hookId = new uint?(reader.ReadUInt(32));
				this.gameActionType = new GameActionType?(reader.ReadEnum<GameActionType>());
				return;
			case CombatReplayEventType.ResumeAction:
				this.actionId = new uint?(reader.ReadUInt(32));
				return;
			case CombatReplayEventType.PlayerChoice:
				this.playerId = new ulong?(reader.ReadULong(64));
				this.choiceId = new uint?(reader.ReadUInt(32));
				this.playerChoiceResult = new NetPlayerChoiceResult?(reader.Read<NetPlayerChoiceResult>());
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x002164E8 File Offset: 0x002146E8
		public CombatReplayEvent Anonymized()
		{
			CombatReplayEvent combatReplayEvent = this;
			combatReplayEvent.playerId = ((this.playerId != null) ? new ulong?(IdAnonymizer.Anonymize(this.playerId.Value)) : null);
			return combatReplayEvent;
		}

		// Token: 0x04001B22 RID: 6946
		public CombatReplayEventType eventType;

		// Token: 0x04001B23 RID: 6947
		public ulong? playerId;

		// Token: 0x04001B24 RID: 6948
		[Nullable(2)]
		public INetAction action;

		// Token: 0x04001B25 RID: 6949
		public uint? hookId;

		// Token: 0x04001B26 RID: 6950
		public uint? actionId;

		// Token: 0x04001B27 RID: 6951
		public GameActionType? gameActionType;

		// Token: 0x04001B28 RID: 6952
		public uint? choiceId;

		// Token: 0x04001B29 RID: 6953
		public NetPlayerChoiceResult? playerChoiceResult;
	}
}
