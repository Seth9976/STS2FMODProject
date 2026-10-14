using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	/// <summary>
	/// Serializable representation of a single map point for save files.
	/// </summary>
	// Token: 0x020000C8 RID: 200
	[NullableContext(2)]
	[Nullable(0)]
	public class SerializableMapPoint : IPacketSerializable
	{
		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x000444DC File Offset: 0x000426DC
		// (set) Token: 0x06000985 RID: 2437 RVA: 0x000444E4 File Offset: 0x000426E4
		[JsonPropertyName("coord")]
		public MapCoord Coord { get; set; }

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x000444ED File Offset: 0x000426ED
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x000444F5 File Offset: 0x000426F5
		[JsonPropertyName("type")]
		public MapPointType PointType { get; set; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x000444FE File Offset: 0x000426FE
		// (set) Token: 0x06000989 RID: 2441 RVA: 0x00044506 File Offset: 0x00042706
		[JsonPropertyName("can_modify")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool CanBeModified { get; set; } = true;

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0004450F File Offset: 0x0004270F
		// (set) Token: 0x0600098B RID: 2443 RVA: 0x00044517 File Offset: 0x00042717
		[JsonPropertyName("children")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<MapCoord> ChildCoords { get; set; }

		// Token: 0x0600098C RID: 2444 RVA: 0x00044520 File Offset: 0x00042720
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<MapCoord>(this.Coord);
			writer.WriteInt((int)this.PointType, 8);
			writer.WriteBool(this.CanBeModified);
			List<MapCoord> childCoords = this.ChildCoords;
			int num = ((childCoords != null) ? childCoords.Count : 0);
			writer.WriteInt(num, 8);
			if (this.ChildCoords != null)
			{
				foreach (MapCoord mapCoord in this.ChildCoords)
				{
					writer.Write<MapCoord>(mapCoord);
				}
			}
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x000445BC File Offset: 0x000427BC
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.Coord = reader.Read<MapCoord>();
			this.PointType = (MapPointType)reader.ReadInt(8);
			this.CanBeModified = reader.ReadBool();
			int num = reader.ReadInt(8);
			if (num > 0)
			{
				this.ChildCoords = new List<MapCoord>(num);
				for (int i = 0; i < num; i++)
				{
					this.ChildCoords.Add(reader.Read<MapCoord>());
				}
			}
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00044624 File Offset: 0x00042824
		[NullableContext(1)]
		public static SerializableMapPoint FromMapPoint(MapPoint point)
		{
			List<MapCoord> list = null;
			if (point.Children.Count > 0)
			{
				list = new List<MapCoord>(point.Children.Count);
				foreach (MapPoint mapPoint in point.Children)
				{
					list.Add(mapPoint.coord);
				}
			}
			return new SerializableMapPoint
			{
				Coord = point.coord,
				PointType = point.PointType,
				CanBeModified = point.CanBeModified,
				ChildCoords = list
			};
		}
	}
}
