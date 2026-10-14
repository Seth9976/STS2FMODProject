using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000875 RID: 2165
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BygoneEffigyElite : EncounterModel
	{
		// Token: 0x17001A2D RID: 6701
		// (get) Token: 0x06006A1C RID: 27164 RVA: 0x0027EED8 File Offset: 0x0027D0D8
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x06006A1D RID: 27165 RVA: 0x0027EEDB File Offset: 0x0027D0DB
		public override float GetCameraScaling()
		{
			return 0.88f;
		}

		// Token: 0x06006A1E RID: 27166 RVA: 0x0027EEE2 File Offset: 0x0027D0E2
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 50f;
		}

		// Token: 0x17001A2E RID: 6702
		// (get) Token: 0x06006A1F RID: 27167 RVA: 0x0027EEF3 File Offset: 0x0027D0F3
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<BygoneEffigy>());
			}
		}

		// Token: 0x06006A20 RID: 27168 RVA: 0x0027EEFF File Offset: 0x0027D0FF
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<BygoneEffigy>().ToMutable(), null));
		}
	}
}
