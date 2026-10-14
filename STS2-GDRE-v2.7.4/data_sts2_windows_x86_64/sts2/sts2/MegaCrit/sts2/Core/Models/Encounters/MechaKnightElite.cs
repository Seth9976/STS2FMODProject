using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000899 RID: 2201
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MechaKnightElite : EncounterModel
	{
		// Token: 0x06006AF9 RID: 27385 RVA: 0x00280030 File Offset: 0x0027E230
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x06006AFA RID: 27386 RVA: 0x00280037 File Offset: 0x0027E237
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 50f;
		}

		// Token: 0x17001AAC RID: 6828
		// (get) Token: 0x06006AFB RID: 27387 RVA: 0x00280048 File Offset: 0x0027E248
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x17001AAD RID: 6829
		// (get) Token: 0x06006AFC RID: 27388 RVA: 0x0028004B File Offset: 0x0027E24B
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<MechaKnight>());
			}
		}

		// Token: 0x06006AFD RID: 27389 RVA: 0x00280057 File Offset: 0x0027E257
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<MechaKnight>().ToMutable(), null));
		}
	}
}
