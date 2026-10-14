using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200089D RID: 2205
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NibbitsWeak : EncounterModel
	{
		// Token: 0x17001AB8 RID: 6840
		// (get) Token: 0x06006B11 RID: 27409 RVA: 0x002801D8 File Offset: 0x0027E3D8
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Nibbit);
			}
		}

		// Token: 0x17001AB9 RID: 6841
		// (get) Token: 0x06006B12 RID: 27410 RVA: 0x002801E0 File Offset: 0x0027E3E0
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001ABA RID: 6842
		// (get) Token: 0x06006B13 RID: 27411 RVA: 0x002801E3 File Offset: 0x0027E3E3
		public override bool IsWeak
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001ABB RID: 6843
		// (get) Token: 0x06006B14 RID: 27412 RVA: 0x002801E6 File Offset: 0x0027E3E6
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Nibbit>());
			}
		}

		// Token: 0x06006B15 RID: 27413 RVA: 0x002801F4 File Offset: 0x0027E3F4
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			Nibbit nibbit = (Nibbit)ModelDb.Monster<Nibbit>().ToMutable();
			nibbit.IsAlone = true;
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(nibbit, null));
		}
	}
}
