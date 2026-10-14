using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters.Mocks;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters.Mocks
{
	// Token: 0x020008CE RID: 2254
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockPlatingEncounter : EncounterModel
	{
		// Token: 0x17001B66 RID: 7014
		// (get) Token: 0x06006C39 RID: 27705 RVA: 0x002819B8 File Offset: 0x0027FBB8
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B67 RID: 7015
		// (get) Token: 0x06006C3A RID: 27706 RVA: 0x002819BB File Offset: 0x0027FBBB
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B68 RID: 7016
		// (get) Token: 0x06006C3B RID: 27707 RVA: 0x002819BE File Offset: 0x0027FBBE
		// (set) Token: 0x06006C3C RID: 27708 RVA: 0x002819C6 File Offset: 0x0027FBC6
		public int PlatingAmount
		{
			get
			{
				return this._platingAmount;
			}
			set
			{
				base.AssertMutable();
				this._platingAmount = value;
			}
		}

		// Token: 0x17001B69 RID: 7017
		// (get) Token: 0x06006C3D RID: 27709 RVA: 0x002819D5 File Offset: 0x0027FBD5
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<MockPlatingMonster>());
			}
		}

		// Token: 0x06006C3E RID: 27710 RVA: 0x002819E4 File Offset: 0x0027FBE4
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			MonsterModel monsterModel = ModelDb.Monster<MockPlatingMonster>().ToMutable();
			((MockPlatingMonster)monsterModel).PlatingAmount = this.PlatingAmount;
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(monsterModel, null));
		}

		// Token: 0x04002831 RID: 10289
		private int _platingAmount = 1;
	}
}
