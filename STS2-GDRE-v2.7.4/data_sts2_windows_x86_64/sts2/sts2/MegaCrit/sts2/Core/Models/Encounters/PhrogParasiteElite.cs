using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x020008A2 RID: 2210
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PhrogParasiteElite : EncounterModel
	{
		// Token: 0x17001AC9 RID: 6857
		// (get) Token: 0x06006B30 RID: 27440 RVA: 0x00280478 File Offset: 0x0027E678
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Elite;
			}
		}

		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x06006B31 RID: 27441 RVA: 0x0028047B File Offset: 0x0027E67B
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "phrog", "wriggler1", "wriggler2", "wriggler3", "wriggler4" });
			}
		}

		// Token: 0x06006B32 RID: 27442 RVA: 0x002804B0 File Offset: 0x0027E6B0
		public override float GetCameraScaling()
		{
			return 0.95f;
		}

		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x06006B33 RID: 27443 RVA: 0x002804B7 File Offset: 0x0027E6B7
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006B34 RID: 27444 RVA: 0x002804BC File Offset: 0x0027E6BC
		public static string GetWrigglerSlotName(int index)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
			defaultInterpolatedStringHandler.AppendFormatted("wriggler");
			defaultInterpolatedStringHandler.AppendFormatted<int>(index + 1);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x06006B35 RID: 27445 RVA: 0x002804EF File Offset: 0x0027E6EF
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<PhrogParasite>(),
					ModelDb.Monster<Wriggler>()
				});
			}
		}

		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x06006B36 RID: 27446 RVA: 0x0028050C File Offset: 0x0027E70C
		public override IEnumerable<string> ExtraAssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(ModelDb.Card<Infection>().OverlayPath);
			}
		}

		// Token: 0x06006B37 RID: 27447 RVA: 0x0028051D File Offset: 0x0027E71D
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<PhrogParasite>().ToMutable(), "phrog"));
		}

		// Token: 0x04002826 RID: 10278
		private const string _wrigglerSlotPrefix = "wriggler";

		// Token: 0x04002827 RID: 10279
		private const string _phrogSlot = "phrog";
	}
}
