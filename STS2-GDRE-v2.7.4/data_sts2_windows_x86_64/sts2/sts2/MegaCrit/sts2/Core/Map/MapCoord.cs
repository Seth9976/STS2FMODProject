using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Map
{
	/// <summary>
	/// A simple struct representing the row and column that a given map point is on in the map.
	/// </summary>
	// Token: 0x02000BA0 RID: 2976
	[Serializable]
	public struct MapCoord : IEquatable<MapCoord>, IComparable<MapCoord>, IPacketSerializable
	{
		// Token: 0x06007C22 RID: 31778 RVA: 0x002A4CE2 File Offset: 0x002A2EE2
		public MapCoord(int col, int row)
		{
			this.col = col;
			this.row = row;
		}

		// Token: 0x06007C23 RID: 31779 RVA: 0x002A4CF2 File Offset: 0x002A2EF2
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteByte((byte)this.col, 8);
			writer.WriteByte((byte)this.row, 8);
		}

		// Token: 0x06007C24 RID: 31780 RVA: 0x002A4D10 File Offset: 0x002A2F10
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.col = (int)reader.ReadByte(8);
			this.row = (int)reader.ReadByte(8);
		}

		// Token: 0x06007C25 RID: 31781 RVA: 0x002A4D2C File Offset: 0x002A2F2C
		public static bool operator ==(MapCoord first, MapCoord second)
		{
			return first.Equals(second);
		}

		// Token: 0x06007C26 RID: 31782 RVA: 0x002A4D36 File Offset: 0x002A2F36
		public static bool operator !=(MapCoord first, MapCoord second)
		{
			return !(first == second);
		}

		// Token: 0x06007C27 RID: 31783 RVA: 0x002A4D42 File Offset: 0x002A2F42
		public bool Equals(MapCoord other)
		{
			return this.col == other.col && this.row == other.row;
		}

		// Token: 0x06007C28 RID: 31784 RVA: 0x002A4D64 File Offset: 0x002A2F64
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is MapCoord)
			{
				MapCoord mapCoord = (MapCoord)obj;
				return this.Equals(mapCoord);
			}
			return false;
		}

		// Token: 0x06007C29 RID: 31785 RVA: 0x002A4D8C File Offset: 0x002A2F8C
		public override int GetHashCode()
		{
			return new ValueTuple<int, int>(this.col, this.row).GetHashCode();
		}

		// Token: 0x06007C2A RID: 31786 RVA: 0x002A4DB8 File Offset: 0x002A2FB8
		public int CompareTo(MapCoord other)
		{
			return new ValueTuple<int, int>(this.col, this.row).CompareTo(new ValueTuple<int, int>(other.col, other.row));
		}

		// Token: 0x06007C2B RID: 31787 RVA: 0x002A4DF0 File Offset: 0x002A2FF0
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
			defaultInterpolatedStringHandler.AppendLiteral("MapCoord (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.col);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.row);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002956 RID: 10582
		[JsonInclude]
		public int col;

		// Token: 0x04002957 RID: 10583
		[JsonInclude]
		public int row;
	}
}
