using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008BE RID: 2238
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheObscuraNormal : EncounterModel
	{
		// Token: 0x17001B2B RID: 6955
		// (get) Token: 0x06006BD9 RID: 27609 RVA: 0x0028140A File Offset: 0x0027F60A
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B2C RID: 6956
		// (get) Token: 0x06006BDA RID: 27610 RVA: 0x0028140D File Offset: 0x0027F60D
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "illusion", "obscura" });
			}
		}

		// Token: 0x17001B2D RID: 6957
		// (get) Token: 0x06006BDB RID: 27611 RVA: 0x0028142A File Offset: 0x0027F62A
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B2E RID: 6958
		// (get) Token: 0x06006BDC RID: 27612 RVA: 0x0028142D File Offset: 0x0027F62D
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<TheObscura>(),
					ModelDb.Monster<Parafright>()
				});
			}
		}

		// Token: 0x06006BDD RID: 27613 RVA: 0x0028144A File Offset: 0x0027F64A
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<TheObscura>().ToMutable(), "obscura"));
		}

		// Token: 0x0400282F RID: 10287
		public const string illusionSlot = "illusion";

		// Token: 0x04002830 RID: 10288
		private const string _obscuraSlot = "obscura";
	}
}
