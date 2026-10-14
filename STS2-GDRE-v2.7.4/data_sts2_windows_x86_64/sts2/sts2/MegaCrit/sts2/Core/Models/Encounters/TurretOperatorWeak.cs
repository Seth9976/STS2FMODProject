using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008C3 RID: 2243
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TurretOperatorWeak : EncounterModel
	{
		// Token: 0x17001B3D RID: 6973
		// (get) Token: 0x06006BF5 RID: 27637 RVA: 0x002815ED File Offset: 0x0027F7ED
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B3E RID: 6974
		// (get) Token: 0x06006BF6 RID: 27638 RVA: 0x002815F0 File Offset: 0x0027F7F0
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B3F RID: 6975
		// (get) Token: 0x06006BF7 RID: 27639 RVA: 0x002815F3 File Offset: 0x0027F7F3
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<LivingShield>(),
					ModelDb.Monster<TurretOperator>()
				});
			}
		}

		// Token: 0x06006BF8 RID: 27640 RVA: 0x00281610 File Offset: 0x0027F810
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<LivingShield>().ToMutable(), null),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<TurretOperator>().ToMutable(), null)
			});
		}
	}
}
