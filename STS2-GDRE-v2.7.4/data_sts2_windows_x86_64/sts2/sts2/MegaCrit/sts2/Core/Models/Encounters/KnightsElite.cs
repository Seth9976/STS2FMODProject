using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000893 RID: 2195
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KnightsElite : EncounterModel
	{
		// Token: 0x17001A93 RID: 6803
		// (get) Token: 0x06006ACE RID: 27342 RVA: 0x0027FD9A File Offset: 0x0027DF9A
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x17001A94 RID: 6804
		// (get) Token: 0x06006ACF RID: 27343 RVA: 0x0027FD9D File Offset: 0x0027DF9D
		public override IEnumerable<EncounterTag> Tags
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Knights);
			}
		}

		// Token: 0x06006AD0 RID: 27344 RVA: 0x0027FDA6 File Offset: 0x0027DFA6
		public override float GetCameraScaling()
		{
			return 0.87f;
		}

		// Token: 0x06006AD1 RID: 27345 RVA: 0x0027FDAD File Offset: 0x0027DFAD
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 50f;
		}

		// Token: 0x17001A95 RID: 6805
		// (get) Token: 0x06006AD2 RID: 27346 RVA: 0x0027FDBE File Offset: 0x0027DFBE
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A96 RID: 6806
		// (get) Token: 0x06006AD3 RID: 27347 RVA: 0x0027FDC1 File Offset: 0x0027DFC1
		public override IEnumerable<string> ExtraAssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(ModelDb.Affliction<Hexed>().OverlayPath);
			}
		}

		// Token: 0x17001A97 RID: 6807
		// (get) Token: 0x06006AD4 RID: 27348 RVA: 0x0027FDD2 File Offset: 0x0027DFD2
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<FlailKnight>(),
					ModelDb.Monster<SpectralKnight>(),
					ModelDb.Monster<MagiKnight>()
				});
			}
		}

		// Token: 0x06006AD5 RID: 27349 RVA: 0x0027FDF8 File Offset: 0x0027DFF8
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>[]
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<FlailKnight>().ToMutable(), "first"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<SpectralKnight>().ToMutable(), "second"),
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<MagiKnight>().ToMutable(), "third")
			});
		}
	}
}
