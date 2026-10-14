using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200087E RID: 2174
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DecimillipedeElite : EncounterModel
	{
		// Token: 0x17001A46 RID: 6726
		// (get) Token: 0x06006A4C RID: 27212 RVA: 0x0027F2D9 File Offset: 0x0027D4D9
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A47 RID: 6727
		// (get) Token: 0x06006A4D RID: 27213 RVA: 0x0027F2DC File Offset: 0x0027D4DC
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "segment1", "segment2", "segment3" });
			}
		}

		// Token: 0x06006A4E RID: 27214 RVA: 0x0027F301 File Offset: 0x0027D501
		public override float GetCameraScaling()
		{
			return 0.87f;
		}

		// Token: 0x06006A4F RID: 27215 RVA: 0x0027F308 File Offset: 0x0027D508
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 50f;
		}

		// Token: 0x17001A48 RID: 6728
		// (get) Token: 0x06006A50 RID: 27216 RVA: 0x0027F319 File Offset: 0x0027D519
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x17001A49 RID: 6729
		// (get) Token: 0x06006A51 RID: 27217 RVA: 0x0027F31C File Offset: 0x0027D51C
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<DecimillipedeSegmentFront>(),
					ModelDb.Monster<DecimillipedeSegmentMiddle>(),
					ModelDb.Monster<DecimillipedeSegmentBack>()
				});
			}
		}

		// Token: 0x06006A52 RID: 27218 RVA: 0x0027F344 File Offset: 0x0027D544
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			DecimillipedeSegment decimillipedeSegment = (DecimillipedeSegment)ModelDb.Monster<DecimillipedeSegmentFront>().ToMutable();
			DecimillipedeSegment decimillipedeSegment2 = (DecimillipedeSegment)ModelDb.Monster<DecimillipedeSegmentMiddle>().ToMutable();
			DecimillipedeSegment decimillipedeSegment3 = (DecimillipedeSegment)ModelDb.Monster<DecimillipedeSegmentBack>().ToMutable();
			int num = base.Rng.NextInt(3);
			decimillipedeSegment.StarterMoveIdx = num;
			decimillipedeSegment2.StarterMoveIdx = (num + 1) % 3;
			decimillipedeSegment3.StarterMoveIdx = (num + 2) % 3;
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(decimillipedeSegment, "segment1"),
				new ValueTuple<MonsterModel, string>(decimillipedeSegment2, "segment2"),
				new ValueTuple<MonsterModel, string>(decimillipedeSegment3, "segment3")
			});
		}

		// Token: 0x04002807 RID: 10247
		private const string _segmentSlot = "segment";
	}
}
