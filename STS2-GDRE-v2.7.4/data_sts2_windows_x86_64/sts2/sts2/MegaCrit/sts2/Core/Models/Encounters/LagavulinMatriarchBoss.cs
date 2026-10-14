using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000895 RID: 2197
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LagavulinMatriarchBoss : EncounterModel
	{
		// Token: 0x17001A9E RID: 6814
		// (get) Token: 0x06006AE1 RID: 27361 RVA: 0x0027FEDD File Offset: 0x0027E0DD
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x06006AE2 RID: 27362 RVA: 0x0027FEE0 File Offset: 0x0027E0E0
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x17001A9F RID: 6815
		// (get) Token: 0x06006AE3 RID: 27363 RVA: 0x0027FEE7 File Offset: 0x0027E0E7
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AA0 RID: 6816
		// (get) Token: 0x06006AE4 RID: 27364 RVA: 0x0027FEEA File Offset: 0x0027E0EA
		public override string BossNodePath
		{
			get
			{
				return "res://images/map/placeholder/" + base.Id.Entry.ToLowerInvariant() + "_icon";
			}
		}

		// Token: 0x17001AA1 RID: 6817
		// (get) Token: 0x06006AE5 RID: 27365 RVA: 0x0027FF0B File Offset: 0x0027E10B
		[Nullable(2)]
		public override MegaSkeletonDataResource BossNodeSpineResource
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001AA2 RID: 6818
		// (get) Token: 0x06006AE6 RID: 27366 RVA: 0x0027FF0E File Offset: 0x0027E10E
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<LagavulinMatriarch>());
			}
		}

		// Token: 0x06006AE7 RID: 27367 RVA: 0x0027FF1A File Offset: 0x0027E11A
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<LagavulinMatriarch>().ToMutable(), null));
		}
	}
}
