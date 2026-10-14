using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008B3 RID: 2227
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlumberingBeetleNormal : EncounterModel
	{
		// Token: 0x17001B00 RID: 6912
		// (get) Token: 0x06006B92 RID: 27538 RVA: 0x00280FA1 File Offset: 0x0027F1A1
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Workers);
			}
		}

		// Token: 0x17001B01 RID: 6913
		// (get) Token: 0x06006B93 RID: 27539 RVA: 0x00280FA9 File Offset: 0x0027F1A9
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B02 RID: 6914
		// (get) Token: 0x06006B94 RID: 27540 RVA: 0x00280FAC File Offset: 0x0027F1AC
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<BowlbugRock>(),
					ModelDb.Monster<SlumberingBeetle>(),
					ModelDb.Monster<BowlbugSilk>()
				});
			}
		}

		// Token: 0x06006B95 RID: 27541 RVA: 0x00280FD4 File Offset: 0x0027F1D4
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<BowlbugRock>().ToMutable(), "first"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<BowlbugSilk>().ToMutable(), "second"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<SlumberingBeetle>().ToMutable(), "third")
			});
		}

		// Token: 0x06006B96 RID: 27542 RVA: 0x0028103D File Offset: 0x0027F23D
		public override float GetCameraScaling()
		{
			return 0.85f;
		}

		// Token: 0x06006B97 RID: 27543 RVA: 0x00281044 File Offset: 0x0027F244
		public override Vector2 GetCameraOffset()
		{
			return new Vector2(0f, 50f);
		}

		// Token: 0x17001B03 RID: 6915
		// (get) Token: 0x06006B98 RID: 27544 RVA: 0x00281055 File Offset: 0x0027F255
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}
	}
}
