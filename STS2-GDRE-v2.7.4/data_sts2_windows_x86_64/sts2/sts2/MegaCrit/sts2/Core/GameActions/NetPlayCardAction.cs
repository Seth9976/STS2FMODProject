using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C28 RID: 3112
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetPlayCardAction : INetAction, IPacketSerializable
	{
		// Token: 0x0600807F RID: 32895 RVA: 0x002B8E53 File Offset: 0x002B7053
		public GameAction ToGameAction(Player player)
		{
			return new PlayCardAction(player, this.card, this.modelId, this.targetId);
		}

		// Token: 0x06008080 RID: 32896 RVA: 0x002B8E70 File Offset: 0x002B7070
		public void Serialize(PacketWriter writer)
		{
			writer.Write<NetCombatCard>(this.card);
			writer.WriteModelEntry(this.modelId);
			writer.WriteBool(this.targetId != null);
			if (this.targetId != null)
			{
				writer.WriteUInt(this.targetId.GetValueOrDefault(), 6);
			}
		}

		// Token: 0x06008081 RID: 32897 RVA: 0x002B8EC8 File Offset: 0x002B70C8
		public void Deserialize(PacketReader reader)
		{
			this.card = reader.Read<NetCombatCard>();
			this.modelId = reader.ReadModelIdAssumingType<CardModel>();
			if (reader.ReadBool())
			{
				this.targetId = new uint?(reader.ReadUInt(6));
				return;
			}
			this.targetId = null;
		}

		// Token: 0x06008082 RID: 32898 RVA: 0x002B8F14 File Offset: 0x002B7114
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
			defaultInterpolatedStringHandler.AppendLiteral("NetPlayCardAction (");
			defaultInterpolatedStringHandler.AppendFormatted<NetCombatCard>(this.card);
			defaultInterpolatedStringHandler.AppendLiteral(") target: ");
			defaultInterpolatedStringHandler.AppendFormatted(((this.targetId != null) ? this.targetId.GetValueOrDefault().ToString() : null) ?? "null");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002B02 RID: 11010
		public NetCombatCard card;

		// Token: 0x04002B03 RID: 11011
		public ModelId modelId;

		// Token: 0x04002B04 RID: 11012
		public uint? targetId;
	}
}
