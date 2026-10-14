using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000892 RID: 2194
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KaiserCrabBoss : EncounterModel
	{
		// Token: 0x17001A8A RID: 6794
		// (get) Token: 0x06006AC1 RID: 27329 RVA: 0x0027FCB9 File Offset: 0x0027DEB9
		public override string BossNodePath
		{
			get
			{
				return "res://images/map/placeholder/" + base.Id.Entry.ToLowerInvariant() + "_icon";
			}
		}

		// Token: 0x17001A8B RID: 6795
		// (get) Token: 0x06006AC2 RID: 27330 RVA: 0x0027FCDA File Offset: 0x0027DEDA
		public override string CustomBgm
		{
			get
			{
				return "event:/music/act2_boss_kaiser_crab";
			}
		}

		// Token: 0x06006AC3 RID: 27331 RVA: 0x0027FCE1 File Offset: 0x0027DEE1
		public override float GetCameraScaling()
		{
			return 0.75f;
		}

		// Token: 0x06006AC4 RID: 27332 RVA: 0x0027FCE8 File Offset: 0x0027DEE8
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 35f;
		}

		// Token: 0x17001A8C RID: 6796
		// (get) Token: 0x06006AC5 RID: 27333 RVA: 0x0027FCF9 File Offset: 0x0027DEF9
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A8D RID: 6797
		// (get) Token: 0x06006AC6 RID: 27334 RVA: 0x0027FCFC File Offset: 0x0027DEFC
		[Nullable(2)]
		public override MegaSkeletonDataResource BossNodeSpineResource
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001A8E RID: 6798
		// (get) Token: 0x06006AC7 RID: 27335 RVA: 0x0027FCFF File Offset: 0x0027DEFF
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x17001A8F RID: 6799
		// (get) Token: 0x06006AC8 RID: 27336 RVA: 0x0027FD02 File Offset: 0x0027DF02
		public override bool FullyCenterPlayers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A90 RID: 6800
		// (get) Token: 0x06006AC9 RID: 27337 RVA: 0x0027FD05 File Offset: 0x0027DF05
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "crusher", "rocket" });
			}
		}

		// Token: 0x17001A91 RID: 6801
		// (get) Token: 0x06006ACA RID: 27338 RVA: 0x0027FD22 File Offset: 0x0027DF22
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A92 RID: 6802
		// (get) Token: 0x06006ACB RID: 27339 RVA: 0x0027FD25 File Offset: 0x0027DF25
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<Crusher>(),
					ModelDb.Monster<Rocket>()
				});
			}
		}

		// Token: 0x06006ACC RID: 27340 RVA: 0x0027FD44 File Offset: 0x0027DF44
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Crusher>().ToMutable(), "crusher"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Rocket>().ToMutable(), "rocket")
			});
		}

		// Token: 0x04002817 RID: 10263
		public const string kaiserCrabCustomTrackName = "kaiser_crab_progress";

		// Token: 0x04002818 RID: 10264
		private const string _crusherSlot = "crusher";

		// Token: 0x04002819 RID: 10265
		private const string _rocketSlot = "rocket";
	}
}
