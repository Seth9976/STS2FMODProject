using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000886 RID: 2182
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeMerchantEventEncounter : EncounterModel
	{
		// Token: 0x17001A64 RID: 6756
		// (get) Token: 0x06006A7F RID: 27263 RVA: 0x0027F84E File Offset: 0x0027DA4E
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A65 RID: 6757
		// (get) Token: 0x06006A80 RID: 27264 RVA: 0x0027F851 File Offset: 0x0027DA51
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A66 RID: 6758
		// (get) Token: 0x06006A81 RID: 27265 RVA: 0x0027F854 File Offset: 0x0027DA54
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A67 RID: 6759
		// (get) Token: 0x06006A82 RID: 27266 RVA: 0x0027F857 File Offset: 0x0027DA57
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("merchant");
			}
		}

		// Token: 0x17001A68 RID: 6760
		// (get) Token: 0x06006A83 RID: 27267 RVA: 0x0027F863 File Offset: 0x0027DA63
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<FakeMerchantMonster>());
			}
		}

		// Token: 0x17001A69 RID: 6761
		// (get) Token: 0x06006A84 RID: 27268 RVA: 0x0027F86F File Offset: 0x0027DA6F
		public override int MinGoldReward
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x17001A6A RID: 6762
		// (get) Token: 0x06006A85 RID: 27269 RVA: 0x0027F876 File Offset: 0x0027DA76
		public override int MaxGoldReward
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x06006A86 RID: 27270 RVA: 0x0027F87D File Offset: 0x0027DA7D
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<FakeMerchantMonster>().ToMutable(), "merchant"));
		}

		// Token: 0x0400280F RID: 10255
		private const string _merchantSlot = "merchant";
	}
}
