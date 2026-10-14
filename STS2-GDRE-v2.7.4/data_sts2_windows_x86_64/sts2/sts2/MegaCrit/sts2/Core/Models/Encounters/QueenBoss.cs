using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008A5 RID: 2213
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class QueenBoss : EncounterModel
	{
		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x06006B41 RID: 27457 RVA: 0x0028060B File Offset: 0x0027E80B
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x17001AD3 RID: 6867
		// (get) Token: 0x06006B42 RID: 27458 RVA: 0x0028060E File Offset: 0x0027E80E
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "amalgam", "queen" });
			}
		}

		// Token: 0x17001AD4 RID: 6868
		// (get) Token: 0x06006B43 RID: 27459 RVA: 0x0028062B File Offset: 0x0027E82B
		public override string CustomBgm
		{
			get
			{
				return "event:/music/act3_boss_queen";
			}
		}

		// Token: 0x06006B44 RID: 27460 RVA: 0x00280632 File Offset: 0x0027E832
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x06006B45 RID: 27461 RVA: 0x00280639 File Offset: 0x0027E839
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 60f;
		}

		// Token: 0x17001AD5 RID: 6869
		// (get) Token: 0x06006B46 RID: 27462 RVA: 0x0028064A File Offset: 0x0027E84A
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AD6 RID: 6870
		// (get) Token: 0x06006B47 RID: 27463 RVA: 0x0028064D File Offset: 0x0027E84D
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AD7 RID: 6871
		// (get) Token: 0x06006B48 RID: 27464 RVA: 0x00280650 File Offset: 0x0027E850
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<Queen>(),
					ModelDb.Monster<TorchHeadAmalgam>()
				});
			}
		}

		// Token: 0x17001AD8 RID: 6872
		// (get) Token: 0x06006B49 RID: 27465 RVA: 0x0028066D File Offset: 0x0027E86D
		public override IEnumerable<string> ExtraAssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(ModelDb.Affliction<Bound>().OverlayPath);
			}
		}

		// Token: 0x06006B4A RID: 27466 RVA: 0x00280680 File Offset: 0x0027E880
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<TorchHeadAmalgam>().ToMutable(), "amalgam"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Queen>().ToMutable(), "queen")
			});
		}

		// Token: 0x04002828 RID: 10280
		private const string _queenSlot = "queen";

		// Token: 0x04002829 RID: 10281
		private const string _amalgamSlot = "amalgam";
	}
}
