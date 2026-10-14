using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008BC RID: 2236
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheKinBoss : EncounterModel
	{
		// Token: 0x17001B21 RID: 6945
		// (get) Token: 0x06006BC9 RID: 27593 RVA: 0x0028129A File Offset: 0x0027F49A
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x17001B22 RID: 6946
		// (get) Token: 0x06006BCA RID: 27594 RVA: 0x0028129D File Offset: 0x0027F49D
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B23 RID: 6947
		// (get) Token: 0x06006BCB RID: 27595 RVA: 0x002812A0 File Offset: 0x0027F4A0
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006BCC RID: 27596 RVA: 0x002812A3 File Offset: 0x0027F4A3
		public override float GetCameraScaling()
		{
			return 0.85f;
		}

		// Token: 0x06006BCD RID: 27597 RVA: 0x002812AA File Offset: 0x0027F4AA
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 50f;
		}

		// Token: 0x17001B24 RID: 6948
		// (get) Token: 0x06006BCE RID: 27598 RVA: 0x002812BB File Offset: 0x0027F4BB
		public override string CustomBgm
		{
			get
			{
				return "event:/music/act1_boss_the_kin";
			}
		}

		// Token: 0x17001B25 RID: 6949
		// (get) Token: 0x06006BCF RID: 27599 RVA: 0x002812C2 File Offset: 0x0027F4C2
		public override string BossNodePath
		{
			get
			{
				return "res://images/map/placeholder/" + base.Id.Entry.ToLowerInvariant() + "_icon";
			}
		}

		// Token: 0x17001B26 RID: 6950
		// (get) Token: 0x06006BD0 RID: 27600 RVA: 0x002812E3 File Offset: 0x0027F4E3
		[Nullable(2)]
		public override MegaSkeletonDataResource BossNodeSpineResource
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001B27 RID: 6951
		// (get) Token: 0x06006BD1 RID: 27601 RVA: 0x002812E6 File Offset: 0x0027F4E6
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "slot1", "slot2", "leaderSlot" });
			}
		}

		// Token: 0x17001B28 RID: 6952
		// (get) Token: 0x06006BD2 RID: 27602 RVA: 0x0028130B File Offset: 0x0027F50B
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<KinFollower>(),
					ModelDb.Monster<KinPriest>()
				});
			}
		}

		// Token: 0x06006BD3 RID: 27603 RVA: 0x00281328 File Offset: 0x0027F528
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			KinFollower kinFollower = (KinFollower)ModelDb.Monster<KinFollower>().ToMutable();
			kinFollower.StartsWithDance = true;
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(kinFollower, "slot1"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<KinFollower>().ToMutable(), "slot2"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<KinPriest>().ToMutable(), "leaderSlot")
			});
		}
	}
}
