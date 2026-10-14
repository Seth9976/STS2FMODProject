using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200087F RID: 2175
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DenseVegetationEventEncounter : EncounterModel
	{
		// Token: 0x17001A4A RID: 6730
		// (get) Token: 0x06006A54 RID: 27220 RVA: 0x0027F3F4 File Offset: 0x0027D5F4
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A4B RID: 6731
		// (get) Token: 0x06006A55 RID: 27221 RVA: 0x0027F3F7 File Offset: 0x0027D5F7
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "wriggler1", "wriggler2", "wriggler3", "wriggler4" });
			}
		}

		// Token: 0x17001A4C RID: 6732
		// (get) Token: 0x06006A56 RID: 27222 RVA: 0x0027F424 File Offset: 0x0027D624
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A4D RID: 6733
		// (get) Token: 0x06006A57 RID: 27223 RVA: 0x0027F427 File Offset: 0x0027D627
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<Wriggler>());
			}
		}

		// Token: 0x06006A58 RID: 27224 RVA: 0x0027F434 File Offset: 0x0027D634
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			List<ValueTuple<MonsterModel, string>> list = new List<ValueTuple<MonsterModel, string>>();
			foreach (string text in this.Slots)
			{
				Wriggler wriggler = (Wriggler)ModelDb.Monster<Wriggler>().ToMutable();
				wriggler.StartStunned = false;
				list.Add(new ValueTuple<MonsterModel, string>(wriggler, text));
			}
			return list;
		}

		// Token: 0x04002808 RID: 10248
		private const string _wrigglerSlotPrefix = "wriggler";
	}
}
