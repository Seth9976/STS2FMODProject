using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008C0 RID: 2240
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ToadpolesWeak : EncounterModel
	{
		// Token: 0x17001B33 RID: 6963
		// (get) Token: 0x06006BE5 RID: 27621 RVA: 0x002814A6 File Offset: 0x0027F6A6
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B34 RID: 6964
		// (get) Token: 0x06006BE6 RID: 27622 RVA: 0x002814A9 File Offset: 0x0027F6A9
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B35 RID: 6965
		// (get) Token: 0x06006BE7 RID: 27623 RVA: 0x002814AC File Offset: 0x0027F6AC
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Toadpole>());
			}
		}

		// Token: 0x06006BE8 RID: 27624 RVA: 0x002814B8 File Offset: 0x0027F6B8
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			Toadpole toadpole = (Toadpole)ModelDb.Monster<Toadpole>().ToMutable();
			toadpole.IsFront = true;
			Toadpole toadpole2 = (Toadpole)ModelDb.Monster<Toadpole>().ToMutable();
			toadpole2.IsFront = false;
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(toadpole, null),
				new ValueTuple<MonsterModel, string>(toadpole2, null)
			});
		}
	}
}
