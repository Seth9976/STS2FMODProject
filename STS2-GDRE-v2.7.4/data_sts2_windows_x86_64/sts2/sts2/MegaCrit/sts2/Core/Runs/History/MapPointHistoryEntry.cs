using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs.History
{
	// Token: 0x02000122 RID: 290
	[NullableContext(1)]
	[Nullable(0)]
	public class MapPointHistoryEntry : IPacketSerializable
	{
		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0005B14E File Offset: 0x0005934E
		// (set) Token: 0x06000E31 RID: 3633 RVA: 0x0005B156 File Offset: 0x00059356
		[JsonPropertyName("map_point_type")]
		public MapPointType MapPointType { get; set; }

		/// <summary>
		/// All the rooms entered in this map point. Usually contains just 1, but can contain more when one type of
		/// room transitions into another (like when the Dense Vegetation event transitions into the Wrigglers combat).
		/// Accessing index 0 will usually give you what you want, but keep an eye out for cases where you may care
		/// about other rooms entered in the same map point.
		/// </summary>
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x0005B15F File Offset: 0x0005935F
		// (set) Token: 0x06000E33 RID: 3635 RVA: 0x0005B167 File Offset: 0x00059367
		[JsonPropertyName("rooms")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<MapPointRoomHistoryEntry> Rooms { get; set; } = new List<MapPointRoomHistoryEntry>();

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x0005B170 File Offset: 0x00059370
		// (set) Token: 0x06000E35 RID: 3637 RVA: 0x0005B178 File Offset: 0x00059378
		[JsonPropertyName("player_stats")]
		public List<PlayerMapPointHistoryEntry> PlayerStats { get; set; } = new List<PlayerMapPointHistoryEntry>();

		// Token: 0x06000E36 RID: 3638 RVA: 0x0005B181 File Offset: 0x00059381
		public MapPointHistoryEntry()
		{
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0005B1A0 File Offset: 0x000593A0
		public MapPointHistoryEntry(MapPointType mapPointType, IPlayerCollection playerCollection)
		{
			this.MapPointType = mapPointType;
			foreach (Player player in playerCollection.Players)
			{
				this.PlayerStats.Add(new PlayerMapPointHistoryEntry
				{
					PlayerId = player.NetId
				});
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0005B228 File Offset: 0x00059428
		public PlayerMapPointHistoryEntry GetEntry(ulong playerId)
		{
			PlayerMapPointHistoryEntry playerMapPointHistoryEntry = this.PlayerStats.FirstOrDefault<PlayerMapPointHistoryEntry>((PlayerMapPointHistoryEntry e) => e.PlayerId == playerId);
			if (playerMapPointHistoryEntry == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Player with ID ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
				defaultInterpolatedStringHandler.AppendLiteral(" not found in player stats for this run history! We have ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<ulong>(",", this.PlayerStats.Select<PlayerMapPointHistoryEntry, ulong>((PlayerMapPointHistoryEntry p) => p.PlayerId)));
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return playerMapPointHistoryEntry;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0005B2D8 File Offset: 0x000594D8
		public bool HasRoomOfType(RoomType roomType)
		{
			foreach (MapPointRoomHistoryEntry mapPointRoomHistoryEntry in this.Rooms)
			{
				if (mapPointRoomHistoryEntry.RoomType == roomType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0005B334 File Offset: 0x00059534
		public IEnumerable<MapPointRoomHistoryEntry> GetRoomsOfType(RoomType roomType)
		{
			MapPointHistoryEntry.<GetRoomsOfType>d__16 <GetRoomsOfType>d__ = new MapPointHistoryEntry.<GetRoomsOfType>d__16(-2);
			<GetRoomsOfType>d__.<>4__this = this;
			<GetRoomsOfType>d__.<>3__roomType = roomType;
			return <GetRoomsOfType>d__;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0005B34C File Offset: 0x0005954C
		[NullableContext(2)]
		public MapPointRoomHistoryEntry FirstRoomOfType(RoomType roomType)
		{
			foreach (MapPointRoomHistoryEntry mapPointRoomHistoryEntry in this.Rooms)
			{
				if (mapPointRoomHistoryEntry.RoomType == roomType)
				{
					return mapPointRoomHistoryEntry;
				}
			}
			return null;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0005B3A8 File Offset: 0x000595A8
		public void Serialize(PacketWriter writer)
		{
			writer.WriteEnum<MapPointType>(this.MapPointType);
			writer.WriteList<MapPointRoomHistoryEntry>(this.Rooms, 32);
			writer.WriteList<PlayerMapPointHistoryEntry>(this.PlayerStats, 32);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0005B3D2 File Offset: 0x000595D2
		public void Deserialize(PacketReader reader)
		{
			this.MapPointType = reader.ReadEnum<MapPointType>();
			this.Rooms = reader.ReadList<MapPointRoomHistoryEntry>(32);
			this.PlayerStats = reader.ReadList<PlayerMapPointHistoryEntry>(32);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0005B3FC File Offset: 0x000595FC
		public MapPointHistoryEntry Anonymized()
		{
			MapPointHistoryEntry mapPointHistoryEntry = new MapPointHistoryEntry();
			mapPointHistoryEntry.MapPointType = this.MapPointType;
			mapPointHistoryEntry.Rooms = this.Rooms;
			mapPointHistoryEntry.PlayerStats = this.PlayerStats.Select<PlayerMapPointHistoryEntry, PlayerMapPointHistoryEntry>((PlayerMapPointHistoryEntry p) => p.Anonymized()).ToList<PlayerMapPointHistoryEntry>();
			return mapPointHistoryEntry;
		}
	}
}
