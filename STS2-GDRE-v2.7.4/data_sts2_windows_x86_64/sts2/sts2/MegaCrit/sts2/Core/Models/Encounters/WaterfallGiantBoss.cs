using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008C7 RID: 2247
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WaterfallGiantBoss : EncounterModel
	{
		// Token: 0x17001B4D RID: 6989
		// (get) Token: 0x06006C11 RID: 27665 RVA: 0x0028182C File Offset: 0x0027FA2C
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x06006C12 RID: 27666 RVA: 0x0028182F File Offset: 0x0027FA2F
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x17001B4E RID: 6990
		// (get) Token: 0x06006C13 RID: 27667 RVA: 0x00281836 File Offset: 0x0027FA36
		public override string CustomBgm
		{
			get
			{
				return "event:/music/act1_b_boss_waterfall_giant";
			}
		}

		// Token: 0x17001B4F RID: 6991
		// (get) Token: 0x06006C14 RID: 27668 RVA: 0x0028183D File Offset: 0x0027FA3D
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B50 RID: 6992
		// (get) Token: 0x06006C15 RID: 27669 RVA: 0x00281840 File Offset: 0x0027FA40
		public override string BossNodePath
		{
			get
			{
				return "res://images/map/placeholder/" + base.Id.Entry.ToLowerInvariant() + "_icon";
			}
		}

		// Token: 0x17001B51 RID: 6993
		// (get) Token: 0x06006C16 RID: 27670 RVA: 0x00281861 File Offset: 0x0027FA61
		[Nullable(2)]
		public override MegaSkeletonDataResource BossNodeSpineResource
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001B52 RID: 6994
		// (get) Token: 0x06006C17 RID: 27671 RVA: 0x00281864 File Offset: 0x0027FA64
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<WaterfallGiant>());
			}
		}

		// Token: 0x06006C18 RID: 27672 RVA: 0x00281870 File Offset: 0x0027FA70
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<WaterfallGiant>().ToMutable(), null));
		}
	}
}
