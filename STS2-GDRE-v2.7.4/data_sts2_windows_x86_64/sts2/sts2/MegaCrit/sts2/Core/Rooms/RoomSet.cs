using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Rooms
{
	// Token: 0x0200012F RID: 303
	[NullableContext(1)]
	[Nullable(0)]
	public class RoomSet
	{
		/// <summary>
		/// This is false in exactly one scenario: when Neow is not spawned as part of the player's first run
		/// </summary>
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06000EAF RID: 3759 RVA: 0x0005C5C9 File Offset: 0x0005A7C9
		public bool HasAncient
		{
			get
			{
				return this._ancient != null;
			}
		}

		/// <summary>
		/// Whether this act has a second boss (Double Boss ascension mode).
		/// </summary>
		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x0005C5D4 File Offset: 0x0005A7D4
		public bool HasSecondBoss
		{
			get
			{
				return this.SecondBoss != null;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x0005C5DF File Offset: 0x0005A7DF
		// (set) Token: 0x06000EB2 RID: 3762 RVA: 0x0005C5F6 File Offset: 0x0005A7F6
		public AncientEventModel Ancient
		{
			get
			{
				AncientEventModel ancient = this._ancient;
				if (ancient == null)
				{
					throw new InvalidOperationException("RoomSet.Ancient not set! You must call GenerateRooms");
				}
				return ancient;
			}
			set
			{
				this._ancient = value;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x0005C5FF File Offset: 0x0005A7FF
		// (set) Token: 0x06000EB4 RID: 3764 RVA: 0x0005C616 File Offset: 0x0005A816
		public EncounterModel Boss
		{
			get
			{
				EncounterModel boss = this._boss;
				if (boss == null)
				{
					throw new InvalidOperationException("RoomSet.Boss not set! You must call GenerateRooms");
				}
				return boss;
			}
			set
			{
				this._boss = value;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x0005C61F File Offset: 0x0005A81F
		// (set) Token: 0x06000EB6 RID: 3766 RVA: 0x0005C627 File Offset: 0x0005A827
		[Nullable(2)]
		public EncounterModel SecondBoss
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0005C630 File Offset: 0x0005A830
		public void MarkVisited(RoomType roomType)
		{
			switch (roomType)
			{
			case RoomType.Monster:
				this.normalEncountersVisited++;
				return;
			case RoomType.Elite:
				this.eliteEncountersVisited++;
				return;
			case RoomType.Boss:
				this.bossEncountersVisited++;
				break;
			case RoomType.Treasure:
			case RoomType.Shop:
				break;
			case RoomType.Event:
				this.eventsVisited++;
				return;
			default:
				return;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x0005C699 File Offset: 0x0005A899
		public EventModel NextEvent
		{
			get
			{
				return this.events[this.eventsVisited % this.events.Count];
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x0005C6B8 File Offset: 0x0005A8B8
		public EncounterModel NextNormalEncounter
		{
			get
			{
				return this.normalEncounters[this.normalEncountersVisited % this.normalEncounters.Count];
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x0005C6D7 File Offset: 0x0005A8D7
		public EncounterModel NextEliteEncounter
		{
			get
			{
				return this.eliteEncounters[this.eliteEncountersVisited % this.eliteEncounters.Count];
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x0005C6F6 File Offset: 0x0005A8F6
		public EncounterModel NextBossEncounter
		{
			get
			{
				if (this.bossEncountersVisited != 0 && this.SecondBoss != null)
				{
					return this.SecondBoss;
				}
				return this.Boss;
			}
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0005C718 File Offset: 0x0005A918
		public void EnsureNextEventIsValid(RunState runState)
		{
			if (this.events.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.events.Count; i++)
			{
				if (this.NextEvent.IsAllowed(runState) && !runState.VisitedEventIds.Contains(this.NextEvent.Id))
				{
					return;
				}
				this.eventsVisited++;
			}
			Log.Warn("All unique events exhausted, allowing repetition", 2);
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0005C78C File Offset: 0x0005A98C
		public SerializableRoomSet ToSave()
		{
			SerializableRoomSet serializableRoomSet = new SerializableRoomSet();
			serializableRoomSet.EventIds = this.events.Select<EventModel, ModelId>((EventModel e) => e.Id).ToList<ModelId>();
			serializableRoomSet.EventsVisited = this.eventsVisited;
			serializableRoomSet.NormalEncounterIds = this.normalEncounters.Select<EncounterModel, ModelId>((EncounterModel e) => e.Id).ToList<ModelId>();
			serializableRoomSet.NormalEncountersVisited = this.normalEncountersVisited;
			serializableRoomSet.EliteEncounterIds = this.eliteEncounters.Select<EncounterModel, ModelId>((EncounterModel e) => e.Id).ToList<ModelId>();
			serializableRoomSet.EliteEncountersVisited = this.eliteEncountersVisited;
			serializableRoomSet.BossEncountersVisited = this.bossEncountersVisited;
			EncounterModel boss = this._boss;
			serializableRoomSet.BossId = ((boss != null) ? boss.Id : null);
			EncounterModel secondBoss = this.SecondBoss;
			serializableRoomSet.SecondBossId = ((secondBoss != null) ? secondBoss.Id : null);
			AncientEventModel ancient = this._ancient;
			serializableRoomSet.AncientId = ((ancient != null) ? ancient.Id : null);
			return serializableRoomSet;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0005C8B8 File Offset: 0x0005AAB8
		public static RoomSet FromSave(SerializableRoomSet save)
		{
			RoomSet roomSet = new RoomSet();
			List<EventModel> list = roomSet.events;
			IEnumerable<ModelId> eventIds = save.EventIds;
			Func<ModelId, EventModel> func;
			if ((func = RoomSet.<>O.<0>__EventOrDeprecated) == null)
			{
				func = (RoomSet.<>O.<0>__EventOrDeprecated = new Func<ModelId, EventModel>(SaveUtil.EventOrDeprecated));
			}
			list.AddRange(from e in eventIds.Select<ModelId, EventModel>(func)
				where !(e is DeprecatedEvent)
				select e);
			roomSet.eventsVisited = save.EventsVisited;
			List<EncounterModel> list2 = roomSet.normalEncounters;
			IEnumerable<ModelId> normalEncounterIds = save.NormalEncounterIds;
			Func<ModelId, EncounterModel> func2;
			if ((func2 = RoomSet.<>O.<1>__EncounterOrDeprecated) == null)
			{
				func2 = (RoomSet.<>O.<1>__EncounterOrDeprecated = new Func<ModelId, EncounterModel>(SaveUtil.EncounterOrDeprecated));
			}
			list2.AddRange(from e in normalEncounterIds.Select<ModelId, EncounterModel>(func2)
				where !(e is DeprecatedEncounter) && e.RoomType == RoomType.Monster
				select e);
			roomSet.normalEncountersVisited = save.NormalEncountersVisited;
			List<EncounterModel> list3 = roomSet.eliteEncounters;
			IEnumerable<ModelId> eliteEncounterIds = save.EliteEncounterIds;
			Func<ModelId, EncounterModel> func3;
			if ((func3 = RoomSet.<>O.<1>__EncounterOrDeprecated) == null)
			{
				func3 = (RoomSet.<>O.<1>__EncounterOrDeprecated = new Func<ModelId, EncounterModel>(SaveUtil.EncounterOrDeprecated));
			}
			list3.AddRange(from e in eliteEncounterIds.Select<ModelId, EncounterModel>(func3)
				where !(e is DeprecatedEncounter) && e.RoomType == RoomType.Elite
				select e);
			roomSet.eliteEncountersVisited = save.EliteEncountersVisited;
			roomSet.bossEncountersVisited = save.BossEncountersVisited;
			roomSet._boss = ((save.BossId != null) ? SaveUtil.EncounterOrDeprecated(save.BossId) : null);
			roomSet.SecondBoss = ((save.SecondBossId != null) ? SaveUtil.EncounterOrDeprecated(save.SecondBossId) : null);
			roomSet._ancient = ((save.AncientId != null) ? SaveUtil.AncientEventOrDeprecated(save.AncientId) : null);
			return roomSet;
		}

		/// <summary>
		/// This forces a specific model type to be at a specific index in a list.
		/// First, it searches for a model of type TSpecificModel in the list. If one already exists in the list, it is
		/// swapped with the model at the passed index. Otherwise, the model at the passed index is replaced a new instance
		/// of TSpecificModel.
		/// Used when setting order of specific encounters or events in the tutorial.
		/// </summary>
		/// <param name="list">The list to search in.</param>
		/// <param name="desiredIndex">The index where the model type will be at, in the end.</param>
		/// <typeparam name="TBaseModel">The type of models contained in the list.</typeparam>
		/// <typeparam name="TSpecificModel">The specific kind of model we'll force to be at desiredIndex.</typeparam>
		// Token: 0x06000EBF RID: 3775 RVA: 0x0005CA64 File Offset: 0x0005AC64
		public static void SwapToOrCreateAtIndex<[Nullable(0)] TBaseModel, [Nullable(0)] TSpecificModel>(List<TBaseModel> list, int desiredIndex) where TBaseModel : AbstractModel where TSpecificModel : TBaseModel
		{
			int num = list.FindIndex((TBaseModel elem) => elem is TSpecificModel);
			if (num >= 0)
			{
				int num2 = num;
				TBaseModel tbaseModel = list[num];
				TBaseModel tbaseModel2 = list[desiredIndex];
				list[desiredIndex] = tbaseModel;
				list[num2] = tbaseModel2;
				return;
			}
			list[desiredIndex] = ModelDb.GetById<TBaseModel>(ModelDb.GetId<TSpecificModel>());
		}

		// Token: 0x0400054C RID: 1356
		public readonly List<EventModel> events = new List<EventModel>();

		// Token: 0x0400054D RID: 1357
		public int eventsVisited;

		// Token: 0x0400054E RID: 1358
		public readonly List<EncounterModel> normalEncounters = new List<EncounterModel>();

		// Token: 0x0400054F RID: 1359
		public int normalEncountersVisited;

		// Token: 0x04000550 RID: 1360
		public readonly List<EncounterModel> eliteEncounters = new List<EncounterModel>();

		// Token: 0x04000551 RID: 1361
		public int eliteEncountersVisited;

		// Token: 0x04000552 RID: 1362
		public int bossEncountersVisited;

		// Token: 0x04000553 RID: 1363
		[Nullable(2)]
		private AncientEventModel _ancient;

		// Token: 0x04000554 RID: 1364
		[Nullable(2)]
		private EncounterModel _boss;

		// Token: 0x02000EF6 RID: 3830
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04003BE1 RID: 15329
			[Nullable(0)]
			public static Func<ModelId, EventModel> <0>__EventOrDeprecated;

			// Token: 0x04003BE2 RID: 15330
			[Nullable(0)]
			public static Func<ModelId, EncounterModel> <1>__EncounterOrDeprecated;
		}
	}
}
