using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	/// <summary>
	/// Serializable representation of an act map for save files.
	/// Captures the full map topology so it can be restored without regeneration.
	/// </summary>
	// Token: 0x020000C2 RID: 194
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableActMap : IPacketSerializable
	{
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x00043B98 File Offset: 0x00041D98
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x00043BA0 File Offset: 0x00041DA0
		[JsonPropertyName("points")]
		public List<SerializableMapPoint> Points { get; set; } = new List<SerializableMapPoint>();

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00043BA9 File Offset: 0x00041DA9
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x00043BB1 File Offset: 0x00041DB1
		[JsonPropertyName("boss")]
		public SerializableMapPoint BossPoint { get; set; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x00043BBA File Offset: 0x00041DBA
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x00043BC2 File Offset: 0x00041DC2
		[Nullable(2)]
		[JsonPropertyName("second_boss")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public SerializableMapPoint SecondBossPoint
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x00043BCB File Offset: 0x00041DCB
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x00043BD3 File Offset: 0x00041DD3
		[JsonPropertyName("start")]
		public SerializableMapPoint StartingPoint { get; set; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x00043BDC File Offset: 0x00041DDC
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x00043BE4 File Offset: 0x00041DE4
		[Nullable(2)]
		[JsonPropertyName("start_coords")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<MapCoord> StartMapPointCoords
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x00043BED File Offset: 0x00041DED
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x00043BF5 File Offset: 0x00041DF5
		[JsonPropertyName("width")]
		public int GridWidth { get; set; }

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x00043BFE File Offset: 0x00041DFE
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x00043C06 File Offset: 0x00041E06
		[JsonPropertyName("height")]
		public int GridHeight { get; set; }

		// Token: 0x0600094B RID: 2379 RVA: 0x00043C10 File Offset: 0x00041E10
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.GridWidth, 8);
			writer.WriteInt(this.GridHeight, 8);
			writer.Write<SerializableMapPoint>(this.BossPoint);
			writer.Write<SerializableMapPoint>(this.StartingPoint);
			writer.WriteBool(this.SecondBossPoint != null);
			if (this.SecondBossPoint != null)
			{
				writer.Write<SerializableMapPoint>(this.SecondBossPoint);
			}
			writer.WriteInt(this.Points.Count, 16);
			foreach (SerializableMapPoint serializableMapPoint in this.Points)
			{
				writer.Write<SerializableMapPoint>(serializableMapPoint);
			}
			List<MapCoord> startMapPointCoords = this.StartMapPointCoords;
			int num = ((startMapPointCoords != null) ? startMapPointCoords.Count : 0);
			writer.WriteInt(num, 8);
			if (this.StartMapPointCoords != null)
			{
				foreach (MapCoord mapCoord in this.StartMapPointCoords)
				{
					writer.Write<MapCoord>(mapCoord);
				}
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00043D34 File Offset: 0x00041F34
		public void Deserialize(PacketReader reader)
		{
			this.GridWidth = reader.ReadInt(8);
			this.GridHeight = reader.ReadInt(8);
			this.BossPoint = reader.Read<SerializableMapPoint>();
			this.StartingPoint = reader.Read<SerializableMapPoint>();
			if (reader.ReadBool())
			{
				this.SecondBossPoint = reader.Read<SerializableMapPoint>();
			}
			int num = reader.ReadInt(16);
			this.Points = new List<SerializableMapPoint>(num);
			for (int i = 0; i < num; i++)
			{
				this.Points.Add(reader.Read<SerializableMapPoint>());
			}
			int num2 = reader.ReadInt(8);
			if (num2 > 0)
			{
				this.StartMapPointCoords = new List<MapCoord>(num2);
				for (int j = 0; j < num2; j++)
				{
					this.StartMapPointCoords.Add(reader.Read<MapCoord>());
				}
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00043DF0 File Offset: 0x00041FF0
		public static SerializableActMap FromActMap(ActMap map)
		{
			List<SerializableMapPoint> list = new List<SerializableMapPoint>();
			foreach (MapPoint mapPoint in map.GetAllMapPoints())
			{
				list.Add(SerializableMapPoint.FromMapPoint(mapPoint));
			}
			List<MapCoord> list2 = null;
			if (map.startMapPoints.Count > 0)
			{
				list2 = new List<MapCoord>();
				foreach (MapPoint mapPoint2 in map.startMapPoints)
				{
					list2.Add(mapPoint2.coord);
				}
			}
			return new SerializableActMap
			{
				Points = list,
				BossPoint = SerializableMapPoint.FromMapPoint(map.BossMapPoint),
				SecondBossPoint = ((map.SecondBossMapPoint != null) ? SerializableMapPoint.FromMapPoint(map.SecondBossMapPoint) : null),
				StartingPoint = SerializableMapPoint.FromMapPoint(map.StartingMapPoint),
				StartMapPointCoords = list2,
				GridWidth = map.GetColumnCount(),
				GridHeight = map.GetRowCount()
			};
		}
	}
}
