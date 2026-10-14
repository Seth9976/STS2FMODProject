using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200086D RID: 2157
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AeonglassBoss : EncounterModel
	{
		// Token: 0x17001A0F RID: 6671
		// (get) Token: 0x060069E9 RID: 27113 RVA: 0x0027EABF File Offset: 0x0027CCBF
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x17001A10 RID: 6672
		// (get) Token: 0x060069EA RID: 27114 RVA: 0x0027EAC2 File Offset: 0x0027CCC2
		[Nullable(2)]
		public override MegaSkeletonDataResource BossNodeSpineResource
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001A11 RID: 6673
		// (get) Token: 0x060069EB RID: 27115 RVA: 0x0027EAC5 File Offset: 0x0027CCC5
		public override string BossNodePath
		{
			get
			{
				return "res://images/map/placeholder/" + base.Id.Entry.ToLowerInvariant() + "_icon";
			}
		}

		// Token: 0x17001A12 RID: 6674
		// (get) Token: 0x060069EC RID: 27116 RVA: 0x0027EAE6 File Offset: 0x0027CCE6
		public override string CustomBgm
		{
			get
			{
				return "event:/music/act3_boss_queen";
			}
		}

		// Token: 0x060069ED RID: 27117 RVA: 0x0027EAED File Offset: 0x0027CCED
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x060069EE RID: 27118 RVA: 0x0027EAF4 File Offset: 0x0027CCF4
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 60f;
		}

		// Token: 0x17001A13 RID: 6675
		// (get) Token: 0x060069EF RID: 27119 RVA: 0x0027EB05 File Offset: 0x0027CD05
		protected override bool HasCustomBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A14 RID: 6676
		// (get) Token: 0x060069F0 RID: 27120 RVA: 0x0027EB08 File Offset: 0x0027CD08
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Aeonglass>());
			}
		}

		// Token: 0x060069F1 RID: 27121 RVA: 0x0027EB14 File Offset: 0x0027CD14
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<Aeonglass>().ToMutable(), null));
		}
	}
}
