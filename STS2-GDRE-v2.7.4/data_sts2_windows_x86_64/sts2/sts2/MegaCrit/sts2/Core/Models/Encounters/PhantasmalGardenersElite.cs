using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008A1 RID: 2209
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PhantasmalGardenersElite : EncounterModel
	{
		// Token: 0x17001AC5 RID: 6853
		// (get) Token: 0x06006B28 RID: 27432 RVA: 0x00280394 File Offset: 0x0027E594
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "first", "second", "third", "fourth" });
			}
		}

		// Token: 0x17001AC6 RID: 6854
		// (get) Token: 0x06006B29 RID: 27433 RVA: 0x002803C1 File Offset: 0x0027E5C1
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x17001AC7 RID: 6855
		// (get) Token: 0x06006B2A RID: 27434 RVA: 0x002803C4 File Offset: 0x0027E5C4
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006B2B RID: 27435 RVA: 0x002803C7 File Offset: 0x0027E5C7
		public override float GetCameraScaling()
		{
			return 0.85f;
		}

		// Token: 0x06006B2C RID: 27436 RVA: 0x002803CE File Offset: 0x0027E5CE
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 40f;
		}

		// Token: 0x17001AC8 RID: 6856
		// (get) Token: 0x06006B2D RID: 27437 RVA: 0x002803DF File Offset: 0x0027E5DF
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<PhantasmalGardener>());
			}
		}

		// Token: 0x06006B2E RID: 27438 RVA: 0x002803EC File Offset: 0x0027E5EC
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<PhantasmalGardener>().ToMutable(), "first"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<PhantasmalGardener>().ToMutable(), "second"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<PhantasmalGardener>().ToMutable(), "third"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<PhantasmalGardener>().ToMutable(), "fourth")
			});
		}

		// Token: 0x04002822 RID: 10274
		public const string firstSlot = "first";

		// Token: 0x04002823 RID: 10275
		public const string secondSlot = "second";

		// Token: 0x04002824 RID: 10276
		public const string thirdSlot = "third";

		// Token: 0x04002825 RID: 10277
		public const string fourthSlot = "fourth";
	}
}
