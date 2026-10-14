using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008C4 RID: 2244
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TwoTailedRatsNormal : EncounterModel
	{
		// Token: 0x17001B40 RID: 6976
		// (get) Token: 0x06006BFA RID: 27642 RVA: 0x00281653 File Offset: 0x0027F853
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B41 RID: 6977
		// (get) Token: 0x06006BFB RID: 27643 RVA: 0x00281656 File Offset: 0x0027F856
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "first", "second", "third", "fourth", "fifth" });
			}
		}

		// Token: 0x17001B42 RID: 6978
		// (get) Token: 0x06006BFC RID: 27644 RVA: 0x0028168B File Offset: 0x0027F88B
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x06006BFD RID: 27645 RVA: 0x0028168E File Offset: 0x0027F88E
		public override float GetCameraScaling()
		{
			return 0.85f;
		}

		// Token: 0x06006BFE RID: 27646 RVA: 0x00281695 File Offset: 0x0027F895
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 25f;
		}

		// Token: 0x17001B43 RID: 6979
		// (get) Token: 0x06006BFF RID: 27647 RVA: 0x002816A6 File Offset: 0x0027F8A6
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<TwoTailedRat>());
			}
		}

		// Token: 0x06006C00 RID: 27648 RVA: 0x002816B4 File Offset: 0x0027F8B4
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			TwoTailedRat twoTailedRat = (TwoTailedRat)ModelDb.Monster<TwoTailedRat>().ToMutable();
			TwoTailedRat twoTailedRat2 = (TwoTailedRat)ModelDb.Monster<TwoTailedRat>().ToMutable();
			TwoTailedRat twoTailedRat3 = (TwoTailedRat)ModelDb.Monster<TwoTailedRat>().ToMutable();
			int num = base.Rng.NextInt(3);
			twoTailedRat.StarterMoveIndex = num;
			twoTailedRat2.StarterMoveIndex = (num + 1) % 3;
			twoTailedRat3.StarterMoveIndex = (num + 2) % 3;
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(twoTailedRat, this.Slots[2]),
				new ValueTuple<MonsterModel, string>(twoTailedRat2, this.Slots[3]),
				new ValueTuple<MonsterModel, string>(twoTailedRat3, this.Slots[4])
			});
		}
	}
}
