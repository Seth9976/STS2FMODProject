using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000883 RID: 2179
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ExoskeletonsNormal : EncounterModel
	{
		// Token: 0x17001A55 RID: 6741
		// (get) Token: 0x06006A67 RID: 27239 RVA: 0x0027F528 File Offset: 0x0027D728
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Exoskeletons);
			}
		}

		// Token: 0x17001A56 RID: 6742
		// (get) Token: 0x06006A68 RID: 27240 RVA: 0x0027F531 File Offset: 0x0027D731
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "first", "second", "third", "fourth" });
			}
		}

		// Token: 0x17001A57 RID: 6743
		// (get) Token: 0x06006A69 RID: 27241 RVA: 0x0027F55E File Offset: 0x0027D75E
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A58 RID: 6744
		// (get) Token: 0x06006A6A RID: 27242 RVA: 0x0027F561 File Offset: 0x0027D761
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A59 RID: 6745
		// (get) Token: 0x06006A6B RID: 27243 RVA: 0x0027F564 File Offset: 0x0027D764
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Exoskeleton>());
			}
		}

		// Token: 0x06006A6C RID: 27244 RVA: 0x0027F570 File Offset: 0x0027D770
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Exoskeleton>().ToMutable(), "first"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Exoskeleton>().ToMutable(), "second"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Exoskeleton>().ToMutable(), "third"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Exoskeleton>().ToMutable(), "fourth")
			});
		}

		// Token: 0x04002809 RID: 10249
		public const string firstSlot = "first";

		// Token: 0x0400280A RID: 10250
		public const string secondSlot = "second";

		// Token: 0x0400280B RID: 10251
		public const string thirdSlot = "third";

		// Token: 0x0400280C RID: 10252
		public const string fourthSlot = "fourth";
	}
}
