using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008B9 RID: 2233
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TestSubjectBoss : EncounterModel
	{
		// Token: 0x17001B13 RID: 6931
		// (get) Token: 0x06006BB4 RID: 27572 RVA: 0x002811C7 File Offset: 0x0027F3C7
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x17001B14 RID: 6932
		// (get) Token: 0x06006BB5 RID: 27573 RVA: 0x002811CA File Offset: 0x0027F3CA
		public override string BossNodePath
		{
			get
			{
				return "res://images/map/placeholder/" + base.Id.Entry.ToLowerInvariant() + "_icon";
			}
		}

		// Token: 0x17001B15 RID: 6933
		// (get) Token: 0x06006BB6 RID: 27574 RVA: 0x002811EB File Offset: 0x0027F3EB
		public override string CustomBgm
		{
			get
			{
				return "event:/music/act3_boss_test_subject";
			}
		}

		// Token: 0x17001B16 RID: 6934
		// (get) Token: 0x06006BB7 RID: 27575 RVA: 0x002811F2 File Offset: 0x0027F3F2
		[Nullable(2)]
		public override MegaSkeletonDataResource BossNodeSpineResource
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001B17 RID: 6935
		// (get) Token: 0x06006BB8 RID: 27576 RVA: 0x002811F5 File Offset: 0x0027F3F5
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B18 RID: 6936
		// (get) Token: 0x06006BB9 RID: 27577 RVA: 0x002811F8 File Offset: 0x0027F3F8
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<TestSubject>());
			}
		}

		// Token: 0x06006BBA RID: 27578 RVA: 0x00281204 File Offset: 0x0027F404
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<TestSubject>().ToMutable(), null));
		}
	}
}
