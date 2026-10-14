using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200086E RID: 2158
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AxebotsNormal : EncounterModel
	{
		// Token: 0x17001A15 RID: 6677
		// (get) Token: 0x060069F3 RID: 27123 RVA: 0x0027EB33 File Offset: 0x0027CD33
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("front");
			}
		}

		// Token: 0x17001A16 RID: 6678
		// (get) Token: 0x060069F4 RID: 27124 RVA: 0x0027EB3F File Offset: 0x0027CD3F
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A17 RID: 6679
		// (get) Token: 0x060069F5 RID: 27125 RVA: 0x0027EB42 File Offset: 0x0027CD42
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A18 RID: 6680
		// (get) Token: 0x060069F6 RID: 27126 RVA: 0x0027EB45 File Offset: 0x0027CD45
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Axebot>());
			}
		}

		// Token: 0x060069F7 RID: 27127 RVA: 0x0027EB51 File Offset: 0x0027CD51
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<Axebot>().ToMutable(), "front"));
		}

		// Token: 0x04002800 RID: 10240
		private const string _frontSlot = "front";
	}
}
