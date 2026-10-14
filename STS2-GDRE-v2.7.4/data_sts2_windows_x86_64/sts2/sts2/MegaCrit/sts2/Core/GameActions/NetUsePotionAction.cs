using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C2F RID: 3119
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetUsePotionAction : INetAction, IPacketSerializable
	{
		// Token: 0x060080C4 RID: 32964 RVA: 0x002BA2F4 File Offset: 0x002B84F4
		public GameAction ToGameAction(Player player)
		{
			return new UsePotionAction(player, this.potionIndex, this.targetId, this.targetPlayerId, this.enqueuedInCombat);
		}

		// Token: 0x060080C5 RID: 32965 RVA: 0x002BA314 File Offset: 0x002B8514
		public void Serialize(PacketWriter writer)
		{
			writer.WriteUInt(this.potionIndex, 4);
			writer.WriteBool(this.enqueuedInCombat);
			writer.WriteBool(this.targetId != null);
			if (this.targetId != null)
			{
				writer.WriteUInt(this.targetId.GetValueOrDefault(), 6);
			}
			writer.WriteBool(this.targetPlayerId != null);
			if (this.targetPlayerId != null)
			{
				writer.WriteULong(this.targetPlayerId.Value, 64);
			}
		}

		// Token: 0x060080C6 RID: 32966 RVA: 0x002BA39C File Offset: 0x002B859C
		public void Deserialize(PacketReader reader)
		{
			this.potionIndex = reader.ReadUInt(4);
			this.enqueuedInCombat = reader.ReadBool();
			if (reader.ReadBool())
			{
				this.targetId = new uint?(reader.ReadUInt(6));
			}
			else
			{
				this.targetId = null;
			}
			if (reader.ReadBool())
			{
				this.targetPlayerId = new ulong?(reader.ReadULong(64));
				return;
			}
			this.targetPlayerId = null;
		}

		// Token: 0x060080C7 RID: 32967 RVA: 0x002BA414 File Offset: 0x002B8614
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 4);
			defaultInterpolatedStringHandler.AppendLiteral("NetUsePotionAction ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.potionIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendLiteral("target: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint?>(this.targetId);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendLiteral("player: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong?>(this.targetPlayerId);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendLiteral("combat: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.enqueuedInCombat);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002B17 RID: 11031
		public uint potionIndex;

		// Token: 0x04002B18 RID: 11032
		public uint? targetId;

		// Token: 0x04002B19 RID: 11033
		public ulong? targetPlayerId;

		// Token: 0x04002B1A RID: 11034
		public bool enqueuedInCombat;
	}
}
