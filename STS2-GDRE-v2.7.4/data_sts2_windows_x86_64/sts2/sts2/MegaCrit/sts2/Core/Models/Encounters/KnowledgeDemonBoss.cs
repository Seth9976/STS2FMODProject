using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000894 RID: 2196
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KnowledgeDemonBoss : EncounterModel
	{
		// Token: 0x17001A98 RID: 6808
		// (get) Token: 0x06006AD7 RID: 27351 RVA: 0x0027FE69 File Offset: 0x0027E069
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x06006AD8 RID: 27352 RVA: 0x0027FE6C File Offset: 0x0027E06C
		public override float GetCameraScaling()
		{
			return 0.85f;
		}

		// Token: 0x06006AD9 RID: 27353 RVA: 0x0027FE73 File Offset: 0x0027E073
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 70f;
		}

		// Token: 0x17001A99 RID: 6809
		// (get) Token: 0x06006ADA RID: 27354 RVA: 0x0027FE84 File Offset: 0x0027E084
		public override string CustomBgm
		{
			get
			{
				return "event:/music/act2_boss_knowledge_demon";
			}
		}

		// Token: 0x17001A9A RID: 6810
		// (get) Token: 0x06006ADB RID: 27355 RVA: 0x0027FE8B File Offset: 0x0027E08B
		public override string BossNodePath
		{
			get
			{
				return "res://images/map/placeholder/" + base.Id.Entry.ToLowerInvariant() + "_icon";
			}
		}

		// Token: 0x17001A9B RID: 6811
		// (get) Token: 0x06006ADC RID: 27356 RVA: 0x0027FEAC File Offset: 0x0027E0AC
		[Nullable(2)]
		public override MegaSkeletonDataResource BossNodeSpineResource
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17001A9C RID: 6812
		// (get) Token: 0x06006ADD RID: 27357 RVA: 0x0027FEAF File Offset: 0x0027E0AF
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<KnowledgeDemon>());
			}
		}

		// Token: 0x06006ADE RID: 27358 RVA: 0x0027FEBB File Offset: 0x0027E0BB
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<KnowledgeDemon>().ToMutable(), null));
		}

		// Token: 0x17001A9D RID: 6813
		// (get) Token: 0x06006ADF RID: 27359 RVA: 0x0027FED2 File Offset: 0x0027E0D2
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}
	}
}
