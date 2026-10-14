using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Runs
{
	/// <summary>
	/// Identifies a coordinate on the map that the player is at.
	/// Contains an act index + map coordinate.
	/// Make sure you look at <see cref="T:MegaCrit.Sts2.Core.Runs.RunLocation" /> as well, which also includes a room identifier. This class is more
	/// suitable for situations where the room doesn't matter, e.g. map voting.
	/// </summary>
	// Token: 0x02000108 RID: 264
	public struct MapLocation : IEquatable<MapLocation>, IComparable<MapLocation>, IPacketSerializable
	{
		// Token: 0x06000B6B RID: 2923 RVA: 0x0004C634 File Offset: 0x0004A834
		public MapLocation(MapCoord? coord, int actIndex)
		{
			this.coord = coord;
			this.actIndex = actIndex;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0004C644 File Offset: 0x0004A844
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.actIndex, 4);
			writer.WriteBool(this.coord != null);
			if (this.coord != null)
			{
				writer.Write<MapCoord>(this.coord.Value);
			}
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0004C684 File Offset: 0x0004A884
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.actIndex = reader.ReadInt(4);
			bool flag = reader.ReadBool();
			if (flag)
			{
				this.coord = new MapCoord?(reader.Read<MapCoord>());
			}
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0004C6B9 File Offset: 0x0004A8B9
		public static bool operator ==(MapLocation first, MapLocation second)
		{
			return first.Equals(second);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0004C6C3 File Offset: 0x0004A8C3
		public static bool operator !=(MapLocation first, MapLocation second)
		{
			return !(first == second);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0004C6D0 File Offset: 0x0004A8D0
		public bool Equals(MapLocation other)
		{
			return this.actIndex == other.actIndex && this.coord == other.coord;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0004C72C File Offset: 0x0004A92C
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is MapLocation)
			{
				MapLocation mapLocation = (MapLocation)obj;
				return this.Equals(mapLocation);
			}
			return false;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0004C754 File Offset: 0x0004A954
		public override int GetHashCode()
		{
			return new ValueTuple<int, int?, int?>(this.actIndex, (this.coord != null) ? new int?(this.coord.GetValueOrDefault().col) : null, (this.coord != null) ? new int?(this.coord.GetValueOrDefault().row) : null).GetHashCode();
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0004C7CC File Offset: 0x0004A9CC
		public int CompareTo(MapLocation other)
		{
			if (this.actIndex != other.actIndex)
			{
				return this.actIndex.CompareTo(other.actIndex);
			}
			if (this.coord == null && other.coord == null)
			{
				return 0;
			}
			if (this.coord == null && other.coord != null)
			{
				return -1;
			}
			if (this.coord != null && other.coord == null)
			{
				return 1;
			}
			return this.coord.Value.CompareTo(other.coord.Value);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0004C870 File Offset: 0x0004AA70
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
			defaultInterpolatedStringHandler.AppendLiteral("act ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.actIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" coord (");
			string text;
			if (this.coord == null)
			{
				text = "null";
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler2.AppendFormatted<int>(this.coord.Value.col);
				defaultInterpolatedStringHandler2.AppendLiteral(", ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(this.coord.Value.row);
				text = defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// The act that this location is in.
		/// </summary>
		// Token: 0x040003FE RID: 1022
		public int actIndex;

		/// <summary>
		/// The coordinate that this location is at.
		/// Will be null when we're in the map room (at the start of an act, before picking the ancient map point).
		/// </summary>
		// Token: 0x040003FF RID: 1023
		public MapCoord? coord;
	}
}
