using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Represents a player's vote for the map coordinate to move to next.
	/// Used in multiplayer to tally player votes and move on to the one that the majority have voted for.
	/// </summary>
	// Token: 0x0200049E RID: 1182
	[NullableContext(1)]
	[Nullable(0)]
	public struct MapVote : IPacketSerializable
	{
		// Token: 0x0600477E RID: 18302 RVA: 0x0021C5DF File Offset: 0x0021A7DF
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.mapGenerationCount, 4);
			writer.Write<MapCoord>(this.coord);
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x0021C5FA File Offset: 0x0021A7FA
		public void Deserialize(PacketReader reader)
		{
			this.mapGenerationCount = reader.ReadInt(4);
			this.coord = reader.Read<MapCoord>();
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x0021C618 File Offset: 0x0021A818
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 4);
			defaultInterpolatedStringHandler.AppendFormatted("MapVote");
			defaultInterpolatedStringHandler.AppendLiteral(" (gen: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.mapGenerationCount);
			defaultInterpolatedStringHandler.AppendLiteral(" coord: (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.coord.col);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.coord.row);
			defaultInterpolatedStringHandler.AppendLiteral("))");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// How many map generations have occurred before this map vote was received.
		/// Used to invalidate map votes for old maps, in the case that a map was generated multiple times for the same act.
		/// See Golden Compass.
		/// </summary>
		// Token: 0x04001C03 RID: 7171
		public int mapGenerationCount;

		/// <summary>
		/// The coordinate within the map the player has voted for.
		/// </summary>
		// Token: 0x04001C04 RID: 7172
		public MapCoord coord;
	}
}
