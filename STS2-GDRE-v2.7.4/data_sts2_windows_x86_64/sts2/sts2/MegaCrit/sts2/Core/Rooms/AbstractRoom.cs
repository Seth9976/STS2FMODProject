using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Rooms
{
	// Token: 0x02000125 RID: 293
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class AbstractRoom
	{
		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06000E4D RID: 3661
		public abstract RoomType RoomType { get; }

		/// <summary>
		/// The model ID associated with this room. Non-null for combat rooms and event rooms.
		/// </summary>
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06000E4E RID: 3662
		[Nullable(2)]
		public abstract ModelId ModelId
		{
			[NullableContext(2)]
			get;
		}

		/// <summary>
		/// Whether or not this room should load in as already finished.
		/// Used by the save/load system when the player saves after finishing a room, but before choosing a new map
		/// location to visit.
		/// </summary>
		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x0005B592 File Offset: 0x00059792
		public virtual bool IsPreFinished
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// The ID of the room within the current map coordinate.
		/// This id is only unique within each map coordinate. You usually shouldn't have to use this directly; look at
		/// <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.RunLocation" /> instead.
		/// This is not stable across save/load.
		/// </summary>
		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x0005B595 File Offset: 0x00059795
		// (set) Token: 0x06000E51 RID: 3665 RVA: 0x0005B59D File Offset: 0x0005979D
		public int? Id { get; private set; }

		/// <summary>
		/// Called when this room is being entered.
		/// </summary>
		/// <param name="runState">The state of the run that this room is being entered in.</param>
		/// <param name="isRestoringRoomStackBase">
		/// If true, skip hooks and room visit tracking. Used when reconstructing the base of the room stack on load
		/// (e.g., pushing a parent EventRoom underneath a pre-finished CombatRoom).
		/// </param>
		// Token: 0x06000E52 RID: 3666 RVA: 0x0005B5A8 File Offset: 0x000597A8
		public Task Enter([Nullable(2)] IRunState runState, bool isRestoringRoomStackBase)
		{
			this.Id = ((runState != null) ? new int?(runState.GetAndIncrementNextRoomId()) : null);
			return this.EnterInternal(runState, isRestoringRoomStackBase);
		}

		/// <summary>
		/// Override this in implementors.
		/// </summary>
		/// <param name="runState">The state of the run that this room is being entered in.</param>
		/// <param name="isRestoringRoomStackBase">
		/// If true, skip hooks and room visit tracking. Used when reconstructing the base of the room stack on load
		/// (e.g., pushing a parent EventRoom underneath a pre-finished CombatRoom).
		/// </param>
		// Token: 0x06000E53 RID: 3667
		public abstract Task EnterInternal([Nullable(2)] IRunState runState, bool isRestoringRoomStackBase);

		/// <summary>
		/// Called when this room is being exited.
		/// </summary>
		/// <param name="runState">The state of the run that this room is being exited in.</param>
		// Token: 0x06000E54 RID: 3668
		public abstract Task Exit([Nullable(2)] IRunState runState);

		/// <summary>
		/// Called when another room is popped off the room stack, causing this to become the current room again.
		/// </summary>
		/// <param name="exitedRoom">The room that was exited before this was called.</param>
		/// <param name="runState">The state of the run that this room is being resumed in.</param>
		// Token: 0x06000E55 RID: 3669
		public abstract Task Resume(AbstractRoom exitedRoom, [Nullable(2)] IRunState runState);

		/// <summary>
		/// Is this the "Victory" room where the player meets the Architect and the run ends?
		/// </summary>
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0005B5DC File Offset: 0x000597DC
		public bool IsVictoryRoom
		{
			get
			{
				EventRoom eventRoom = this as EventRoom;
				return eventRoom != null && eventRoom.CanonicalEvent is TheArchitect;
			}
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0005B603 File Offset: 0x00059803
		public virtual SerializableRoom ToSerializable()
		{
			return new SerializableRoom
			{
				RoomType = this.RoomType
			};
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0005B618 File Offset: 0x00059818
		[NullableContext(2)]
		public static AbstractRoom FromSerializable(SerializableRoom serializableRoom, IRunState runState)
		{
			if (serializableRoom == null)
			{
				return null;
			}
			RoomType roomType = serializableRoom.RoomType;
			AbstractRoom abstractRoom;
			if (roomType - RoomType.Monster > 2)
			{
				if (roomType != RoomType.Event)
				{
					throw new ArgumentOutOfRangeException();
				}
				abstractRoom = new EventRoom(serializableRoom);
			}
			else
			{
				abstractRoom = CombatRoom.FromSerializable(serializableRoom, runState);
			}
			return abstractRoom;
		}
	}
}
