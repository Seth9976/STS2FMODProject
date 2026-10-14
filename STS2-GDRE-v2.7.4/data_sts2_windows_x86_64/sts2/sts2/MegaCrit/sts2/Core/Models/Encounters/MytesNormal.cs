using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200089B RID: 2203
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MytesNormal : EncounterModel
	{
		// Token: 0x17001AB0 RID: 6832
		// (get) Token: 0x06006B03 RID: 27395 RVA: 0x002800A4 File Offset: 0x0027E2A4
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "first", "second" });
			}
		}

		// Token: 0x17001AB1 RID: 6833
		// (get) Token: 0x06006B04 RID: 27396 RVA: 0x002800C1 File Offset: 0x0027E2C1
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AB2 RID: 6834
		// (get) Token: 0x06006B05 RID: 27397 RVA: 0x002800C4 File Offset: 0x0027E2C4
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006B06 RID: 27398 RVA: 0x002800C7 File Offset: 0x0027E2C7
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x06006B07 RID: 27399 RVA: 0x002800CE File Offset: 0x0027E2CE
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 50f;
		}

		// Token: 0x17001AB3 RID: 6835
		// (get) Token: 0x06006B08 RID: 27400 RVA: 0x002800DF File Offset: 0x0027E2DF
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Myte>());
			}
		}

		// Token: 0x06006B09 RID: 27401 RVA: 0x002800EC File Offset: 0x0027E2EC
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Myte>().ToMutable(), "first"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Myte>().ToMutable(), "second")
			});
		}

		// Token: 0x0400281C RID: 10268
		public const string firstSlot = "first";

		// Token: 0x0400281D RID: 10269
		public const string secondSlot = "second";
	}
}
