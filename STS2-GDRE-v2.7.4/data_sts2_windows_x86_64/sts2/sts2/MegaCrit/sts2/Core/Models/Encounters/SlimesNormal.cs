using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008AF RID: 2223
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlimesNormal : EncounterModel
	{
		// Token: 0x06006B79 RID: 27513 RVA: 0x00280AE9 File Offset: 0x0027ECE9
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x06006B7A RID: 27514 RVA: 0x00280AF0 File Offset: 0x0027ECF0
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 50f;
		}

		// Token: 0x17001AF3 RID: 6899
		// (get) Token: 0x06006B7B RID: 27515 RVA: 0x00280B01 File Offset: 0x0027ED01
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AF4 RID: 6900
		// (get) Token: 0x06006B7C RID: 27516 RVA: 0x00280B04 File Offset: 0x0027ED04
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Slimes);
			}
		}

		// Token: 0x17001AF5 RID: 6901
		// (get) Token: 0x06006B7D RID: 27517 RVA: 0x00280B0C File Offset: 0x0027ED0C
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<LeafSlimeS>(),
					ModelDb.Monster<TwigSlimeS>(),
					ModelDb.Monster<TwigSlimeM>(),
					ModelDb.Monster<LeafSlimeM>()
				});
			}
		}

		// Token: 0x06006B7E RID: 27518 RVA: 0x00280B3C File Offset: 0x0027ED3C
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			bool flag = base.Rng.NextBool();
			MonsterModel monsterModel = (flag ? ModelDb.Monster<LeafSlimeS>() : ModelDb.Monster<TwigSlimeS>());
			MonsterModel monsterModel2 = (flag ? ModelDb.Monster<TwigSlimeS>() : ModelDb.Monster<LeafSlimeS>());
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<TwigSlimeM>().ToMutable(), null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<LeafSlimeM>().ToMutable(), null),
				new ValueTuple<MonsterModel, string>(monsterModel.ToMutable(), null),
				new ValueTuple<MonsterModel, string>(monsterModel2.ToMutable(), null)
			});
		}
	}
}
