using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008C5 RID: 2245
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VantomBoss : EncounterModel
	{
		// Token: 0x17001B44 RID: 6980
		// (get) Token: 0x06006C02 RID: 27650 RVA: 0x00281779 File Offset: 0x0027F979
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x17001B45 RID: 6981
		// (get) Token: 0x06006C03 RID: 27651 RVA: 0x0028177C File Offset: 0x0027F97C
		public override string BossNodePath
		{
			get
			{
				return "res://images/map/placeholder/" + base.Id.Entry.ToLowerInvariant() + "_icon";
			}
		}

		// Token: 0x17001B46 RID: 6982
		// (get) Token: 0x06006C04 RID: 27652 RVA: 0x0028179D File Offset: 0x0027F99D
		[Nullable(2)]
		public override MegaSkeletonDataResource BossNodeSpineResource
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x06006C05 RID: 27653 RVA: 0x002817A0 File Offset: 0x0027F9A0
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x06006C06 RID: 27654 RVA: 0x002817A7 File Offset: 0x0027F9A7
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 50f;
		}

		// Token: 0x17001B47 RID: 6983
		// (get) Token: 0x06006C07 RID: 27655 RVA: 0x002817B8 File Offset: 0x0027F9B8
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B48 RID: 6984
		// (get) Token: 0x06006C08 RID: 27656 RVA: 0x002817BB File Offset: 0x0027F9BB
		public override string CustomBgm
		{
			get
			{
				return "event:/music/act1_boss_vantom";
			}
		}

		// Token: 0x17001B49 RID: 6985
		// (get) Token: 0x06006C09 RID: 27657 RVA: 0x002817C2 File Offset: 0x0027F9C2
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Vantom>());
			}
		}

		// Token: 0x06006C0A RID: 27658 RVA: 0x002817CE File Offset: 0x0027F9CE
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<Vantom>().ToMutable(), null));
		}
	}
}
