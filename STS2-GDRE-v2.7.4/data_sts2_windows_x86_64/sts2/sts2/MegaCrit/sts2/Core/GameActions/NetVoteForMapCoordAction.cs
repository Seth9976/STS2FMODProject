using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C31 RID: 3121
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetVoteForMapCoordAction : INetAction, IPacketSerializable
	{
		// Token: 0x060080CE RID: 32974 RVA: 0x002BA5C1 File Offset: 0x002B87C1
		public GameAction ToGameAction(Player player)
		{
			return new VoteForMapCoordAction(player, this.source, this.destination);
		}

		// Token: 0x060080CF RID: 32975 RVA: 0x002BA5D5 File Offset: 0x002B87D5
		public void Serialize(PacketWriter writer)
		{
			writer.Write<MapLocation>(this.source);
			writer.WriteBool(this.destination != null);
			if (this.destination != null)
			{
				writer.Write<MapVote>(this.destination.Value);
			}
		}

		// Token: 0x060080D0 RID: 32976 RVA: 0x002BA612 File Offset: 0x002B8812
		public void Deserialize(PacketReader reader)
		{
			this.source = reader.Read<MapLocation>();
			if (reader.ReadBool())
			{
				this.destination = new MapVote?(reader.Read<MapVote>());
			}
		}

		// Token: 0x060080D1 RID: 32977 RVA: 0x002BA63C File Offset: 0x002B883C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
			defaultInterpolatedStringHandler.AppendFormatted("NetVoteForMapCoordAction");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<MapLocation>(this.source);
			defaultInterpolatedStringHandler.AppendLiteral("->");
			defaultInterpolatedStringHandler.AppendFormatted<MapVote?>(this.destination);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002B1E RID: 11038
		public MapLocation source;

		// Token: 0x04002B1F RID: 11039
		public MapVote? destination;
	}
}
