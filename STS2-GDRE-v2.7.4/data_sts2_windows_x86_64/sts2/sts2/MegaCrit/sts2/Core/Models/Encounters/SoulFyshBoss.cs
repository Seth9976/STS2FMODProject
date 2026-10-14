using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008B5 RID: 2229
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoulFyshBoss : EncounterModel
	{
		// Token: 0x17001B07 RID: 6919
		// (get) Token: 0x06006B9F RID: 27551 RVA: 0x002810DA File Offset: 0x0027F2DA
		public override string BossNodePath
		{
			get
			{
				return "res://images/map/placeholder/" + base.Id.Entry.ToLowerInvariant() + "_icon";
			}
		}

		// Token: 0x17001B08 RID: 6920
		// (get) Token: 0x06006BA0 RID: 27552 RVA: 0x002810FB File Offset: 0x0027F2FB
		public override string CustomBgm
		{
			get
			{
				return "event:/music/act1_b_boss_soul_fysh";
			}
		}

		// Token: 0x17001B09 RID: 6921
		// (get) Token: 0x06006BA1 RID: 27553 RVA: 0x00281102 File Offset: 0x0027F302
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B0A RID: 6922
		// (get) Token: 0x06006BA2 RID: 27554 RVA: 0x00281105 File Offset: 0x0027F305
		[Nullable(2)]
		public override MegaSkeletonDataResource BossNodeSpineResource
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001B0B RID: 6923
		// (get) Token: 0x06006BA3 RID: 27555 RVA: 0x00281108 File Offset: 0x0027F308
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x17001B0C RID: 6924
		// (get) Token: 0x06006BA4 RID: 27556 RVA: 0x0028110B File Offset: 0x0027F30B
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<SoulFysh>());
			}
		}

		// Token: 0x06006BA5 RID: 27557 RVA: 0x00281117 File Offset: 0x0027F317
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<SoulFysh>().ToMutable(), null));
		}
	}
}
