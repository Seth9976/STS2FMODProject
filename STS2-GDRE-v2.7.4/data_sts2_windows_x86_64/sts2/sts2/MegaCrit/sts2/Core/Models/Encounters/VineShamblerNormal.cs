using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008C6 RID: 2246
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VineShamblerNormal : EncounterModel
	{
		// Token: 0x17001B4A RID: 6986
		// (get) Token: 0x06006C0C RID: 27660 RVA: 0x002817ED File Offset: 0x0027F9ED
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001B4B RID: 6987
		// (get) Token: 0x06006C0D RID: 27661 RVA: 0x002817F0 File Offset: 0x0027F9F0
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<VineShambler>());
			}
		}

		// Token: 0x17001B4C RID: 6988
		// (get) Token: 0x06006C0E RID: 27662 RVA: 0x002817FC File Offset: 0x0027F9FC
		public override IEnumerable<string> ExtraAssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(ModelDb.Affliction<Entangled>().OverlayPath);
			}
		}

		// Token: 0x06006C0F RID: 27663 RVA: 0x0028180D File Offset: 0x0027FA0D
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<VineShambler>().ToMutable(), null));
		}
	}
}
