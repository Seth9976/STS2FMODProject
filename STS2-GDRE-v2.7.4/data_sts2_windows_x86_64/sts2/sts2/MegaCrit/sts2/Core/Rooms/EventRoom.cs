using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Rooms
{
	// Token: 0x0200012A RID: 298
	[NullableContext(1)]
	[Nullable(0)]
	public class EventRoom : AbstractRoom
	{
		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0005BF43 File Offset: 0x0005A143
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Event;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0005BF46 File Offset: 0x0005A146
		public override ModelId ModelId
		{
			get
			{
				return this.CanonicalEvent.Id;
			}
		}

		/// <summary>
		/// The canonical version of the event that the player is doing in this room.
		/// Unlike CombatRoom.Encounter, we want this to be canonical, because we create a separate mutable copy for each
		/// player.
		/// </summary>
		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0005BF53 File Offset: 0x0005A153
		public EventModel CanonicalEvent { get; }

		/// <summary>
		/// The mutable version of the event that the local player is doing in this room.
		/// When using this, keep in mind that the
		/// </summary>
		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0005BF5B File Offset: 0x0005A15B
		public EventModel LocalMutableEvent
		{
			get
			{
				return RunManager.Instance.EventSynchronizer.GetLocalEvent();
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0005BF6C File Offset: 0x0005A16C
		// (set) Token: 0x06000E89 RID: 3721 RVA: 0x0005BF74 File Offset: 0x0005A174
		[Nullable(new byte[] { 2, 1 })]
		public Action<EventModel> OnStart
		{
			[return: Nullable(new byte[] { 2, 1 })]
			private get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0005BF7D File Offset: 0x0005A17D
		public override bool IsPreFinished
		{
			get
			{
				return this._isPreFinished;
			}
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0005BF85 File Offset: 0x0005A185
		public EventRoom(EventModel eventModel)
		{
			eventModel.AssertCanonical();
			this.CanonicalEvent = eventModel;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0005BF9A File Offset: 0x0005A19A
		public EventRoom(SerializableRoom serializableRoom)
		{
			this.CanonicalEvent = SaveUtil.EventOrDeprecated(serializableRoom.EventId);
			if (serializableRoom.IsPreFinished)
			{
				this.MarkPreFinished();
			}
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0005BFC4 File Offset: 0x0005A1C4
		public override async Task EnterInternal([Nullable(2)] IRunState runState, bool isRestoringRoomStackBase)
		{
			RunManager.Instance.EventSynchronizer.BeginEvent(this.CanonicalEvent, this.IsPreFinished, this.OnStart);
			foreach (EventModel eventModel in RunManager.Instance.EventSynchronizer.Events)
			{
				eventModel.StateChanged += this.OnEventStateChanged;
				if (eventModel.IsFinished && !this.IsPreFinished)
				{
					this.OnEventStateChanged(eventModel);
				}
			}
			EventModel localEvent = RunManager.Instance.EventSynchronizer.GetLocalEvent();
			RunManager.Instance.EventSynchronizer.GenerateInternalCombatStateIfNecessary(localEvent);
			await PreloadManager.LoadRoomEventAssets(this.CanonicalEvent, runState ?? NullRunState.Instance);
			if (!isRestoringRoomStackBase)
			{
				NEventRoom neventRoom = NEventRoom.Create(localEvent, runState, this._isPreFinished);
				NRun instance = NRun.Instance;
				if (instance != null)
				{
					instance.SetCurrentRoom(neventRoom);
				}
			}
			if (runState != null)
			{
				await Hook.AfterRoomEntered(runState, this);
			}
			await localEvent.AfterEventStarted();
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0005C018 File Offset: 0x0005A218
		public override async Task Exit([Nullable(2)] IRunState runState)
		{
			await RunManager.Instance.EventSynchronizer.AwaitPendingOptionTasks();
			EventModel localEvent = RunManager.Instance.EventSynchronizer.GetLocalEvent();
			if (localEvent.IsDeterministic)
			{
				ChecksumTracker checksumTracker = RunManager.Instance.ChecksumTracker;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Exiting event room ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(localEvent.Id);
				checksumTracker.GenerateChecksum(defaultInterpolatedStringHandler.ToStringAndClear(), null);
			}
			RunManager.Instance.EventSynchronizer.BeforeExitingRoom();
			foreach (EventModel eventModel in RunManager.Instance.EventSynchronizer.Events)
			{
				eventModel.StateChanged -= this.OnEventStateChanged;
				eventModel.EnsureCleanup();
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0005C05C File Offset: 0x0005A25C
		public override Task Resume(AbstractRoom exitedRoom, [Nullable(2)] IRunState runState)
		{
			RunManager.Instance.EventSynchronizer.ResumeEvents(exitedRoom);
			EventModel localEvent = RunManager.Instance.EventSynchronizer.GetLocalEvent();
			NRun instance = NRun.Instance;
			if (instance != null)
			{
				instance.SetCurrentRoom(NEventRoom.Create(localEvent, runState, this._isPreFinished));
			}
			return Task.CompletedTask;
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0005C0AC File Offset: 0x0005A2AC
		public override SerializableRoom ToSerializable()
		{
			SerializableRoom serializableRoom = base.ToSerializable();
			serializableRoom.EventId = this.CanonicalEvent.Id;
			serializableRoom.IsPreFinished = this.IsPreFinished;
			return serializableRoom;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0005C0DE File Offset: 0x0005A2DE
		public void MarkPreFinished()
		{
			this._isPreFinished = true;
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0005C0E8 File Offset: 0x0005A2E8
		private void OnEventStateChanged(EventModel eventModel)
		{
			if (!(eventModel is AncientEventModel))
			{
				return;
			}
			foreach (EventModel eventModel2 in RunManager.Instance.EventSynchronizer.Events)
			{
				if (!eventModel2.IsFinished)
				{
					return;
				}
			}
			this.MarkPreFinished();
			TaskHelper.RunSafely(SaveManager.Instance.SaveRun(this, true));
		}

		// Token: 0x04000547 RID: 1351
		private bool _isPreFinished;
	}
}
