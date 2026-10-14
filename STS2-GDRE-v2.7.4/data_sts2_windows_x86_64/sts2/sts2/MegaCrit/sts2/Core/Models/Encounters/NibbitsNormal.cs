using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200089C RID: 2204
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NibbitsNormal : EncounterModel
	{
		// Token: 0x17001AB4 RID: 6836
		// (get) Token: 0x06006B0B RID: 27403 RVA: 0x00280142 File Offset: 0x0027E342
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "front", "back" });
			}
		}

		// Token: 0x17001AB5 RID: 6837
		// (get) Token: 0x06006B0C RID: 27404 RVA: 0x0028015F File Offset: 0x0027E35F
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AB6 RID: 6838
		// (get) Token: 0x06006B0D RID: 27405 RVA: 0x00280162 File Offset: 0x0027E362
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AB7 RID: 6839
		// (get) Token: 0x06006B0E RID: 27406 RVA: 0x00280165 File Offset: 0x0027E365
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Nibbit>());
			}
		}

		// Token: 0x06006B0F RID: 27407 RVA: 0x00280174 File Offset: 0x0027E374
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			Nibbit nibbit = (Nibbit)ModelDb.Monster<Nibbit>().ToMutable();
			nibbit.IsFront = true;
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(nibbit, "front"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Nibbit>().ToMutable(), "back")
			});
		}

		// Token: 0x0400281E RID: 10270
		private const string _backSlot = "back";

		// Token: 0x0400281F RID: 10271
		private const string _frontSlot = "front";
	}
}
