using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Runs
{
	/// <summary>
	/// Uniquely identifies a location within a run for RunLocationTargetedMessageBuffer.
	/// This location takes into account which room the player is in, which is important for multi-room locations, like
	/// those that happen in combat events.
	/// Make sure you look at <see cref="T:MegaCrit.Sts2.Core.Runs.MapLocation" /> as well, which identifies locations on the map without considering
	/// the room.
	/// </summary>
	// Token: 0x0200010F RID: 271
	public struct RunLocation : IEquatable<RunLocation>, IComparable<RunLocation>, IPacketSerializable
	{
		// Token: 0x06000C37 RID: 3127 RVA: 0x0004E458 File Offset: 0x0004C658
		public RunLocation(MapLocation mapLocation, int? roomId)
		{
			this.mapLocation = mapLocation;
			this.roomId = roomId;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0004E468 File Offset: 0x0004C668
		public RunLocation(int actIndex, MapCoord? coord, int? roomId)
		{
			this.mapLocation = new MapLocation(coord, actIndex);
			this.roomId = roomId;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0004E47E File Offset: 0x0004C67E
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteBool(this.roomId != null);
			if (this.roomId != null)
			{
				writer.WriteInt(this.roomId.Value, 4);
			}
			writer.Write<MapLocation>(this.mapLocation);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0004E4BC File Offset: 0x0004C6BC
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			if (reader.ReadBool())
			{
				this.roomId = new int?(reader.ReadInt(4));
			}
			this.mapLocation = reader.Read<MapLocation>();
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x0004E4E4 File Offset: 0x0004C6E4
		public static bool operator ==(RunLocation first, RunLocation second)
		{
			return first.Equals(second);
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0004E4EE File Offset: 0x0004C6EE
		public static bool operator !=(RunLocation first, RunLocation second)
		{
			return !(first == second);
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0004E4FC File Offset: 0x0004C6FC
		public bool Equals(RunLocation other)
		{
			int? num = this.roomId;
			int? num2 = other.roomId;
			return ((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null))) && this.mapLocation == other.mapLocation;
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x0004E550 File Offset: 0x0004C750
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is RunLocation)
			{
				RunLocation runLocation = (RunLocation)obj;
				return this.Equals(runLocation);
			}
			return false;
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x0004E578 File Offset: 0x0004C778
		public override int GetHashCode()
		{
			return new ValueTuple<int?, int, int?, int?>(this.roomId, this.mapLocation.actIndex, (this.mapLocation.coord != null) ? new int?(this.mapLocation.coord.GetValueOrDefault().col) : null, (this.mapLocation.coord != null) ? new int?(this.mapLocation.coord.GetValueOrDefault().row) : null).GetHashCode();
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0004E605 File Offset: 0x0004C805
		public int CompareTo(RunLocation other)
		{
			if (this.mapLocation != other.mapLocation)
			{
				return this.mapLocation.CompareTo(other.mapLocation);
			}
			return Comparer<int?>.Default.Compare(this.roomId, other.roomId);
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x0004E644 File Offset: 0x0004C844
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 2);
			defaultInterpolatedStringHandler.AppendFormatted<MapLocation>(this.mapLocation);
			defaultInterpolatedStringHandler.AppendLiteral(" room ");
			defaultInterpolatedStringHandler.AppendFormatted<int?>(this.roomId);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// The act and map coordinate that the location is at.
		/// </summary>
		// Token: 0x0400043E RID: 1086
		public MapLocation mapLocation;

		/// <summary>
		/// The room index that this location is in.
		/// Zero is the root room. Any room other than zero is a sub-room, entered using RunManager.EnterRoomWithoutExitingCurrentRoom.
		/// </summary>
		// Token: 0x0400043F RID: 1087
		public int? roomId;
	}
}
