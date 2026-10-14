using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000881 RID: 2177
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DevotedSculptorWeak : EncounterModel
	{
		// Token: 0x17001A50 RID: 6736
		// (get) Token: 0x06006A5E RID: 27230 RVA: 0x0027F4C9 File Offset: 0x0027D6C9
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A51 RID: 6737
		// (get) Token: 0x06006A5F RID: 27231 RVA: 0x0027F4CC File Offset: 0x0027D6CC
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A52 RID: 6738
		// (get) Token: 0x06006A60 RID: 27232 RVA: 0x0027F4CF File Offset: 0x0027D6CF
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<DevotedSculptor>());
			}
		}

		// Token: 0x06006A61 RID: 27233 RVA: 0x0027F4DB File Offset: 0x0027D6DB
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<DevotedSculptor>().ToMutable(), null));
		}
	}
}
