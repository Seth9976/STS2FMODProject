using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	/// <summary>
	/// Represents an encounter that has been removed from the game. Mostly used for the run history.
	/// </summary>
	// Token: 0x02000880 RID: 2176
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeprecatedEncounter : EncounterModel
	{
		// Token: 0x17001A4E RID: 6734
		// (get) Token: 0x06006A5A RID: 27226 RVA: 0x0027F4B0 File Offset: 0x0027D6B0
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A4F RID: 6735
		// (get) Token: 0x06006A5B RID: 27227 RVA: 0x0027F4B3 File Offset: 0x0027D6B3
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return Array.Empty<MonsterModel>();
			}
		}

		// Token: 0x06006A5C RID: 27228 RVA: 0x0027F4BA File Offset: 0x0027D6BA
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return Array.Empty<ValueTuple<MonsterModel, string>>();
		}
	}
}
