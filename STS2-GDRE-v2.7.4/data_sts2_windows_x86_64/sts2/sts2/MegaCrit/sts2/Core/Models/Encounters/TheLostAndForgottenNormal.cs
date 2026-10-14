using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008BD RID: 2237
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheLostAndForgottenNormal : EncounterModel
	{
		// Token: 0x17001B29 RID: 6953
		// (get) Token: 0x06006BD5 RID: 27605 RVA: 0x002813A7 File Offset: 0x0027F5A7
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B2A RID: 6954
		// (get) Token: 0x06006BD6 RID: 27606 RVA: 0x002813AA File Offset: 0x0027F5AA
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<TheLost>(),
					ModelDb.Monster<TheForgotten>()
				});
			}
		}

		// Token: 0x06006BD7 RID: 27607 RVA: 0x002813C7 File Offset: 0x0027F5C7
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<TheLost>().ToMutable(), null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<TheForgotten>().ToMutable(), null)
			});
		}
	}
}
