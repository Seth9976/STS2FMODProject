using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000877 RID: 2167
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CeremonialBeastBoss : EncounterModel
	{
		// Token: 0x17001A31 RID: 6705
		// (get) Token: 0x06006A26 RID: 27174 RVA: 0x0027EF4C File Offset: 0x0027D14C
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Boss;
			}
		}

		// Token: 0x17001A32 RID: 6706
		// (get) Token: 0x06006A27 RID: 27175 RVA: 0x0027EF4F File Offset: 0x0027D14F
		public override string CustomBgm
		{
			get
			{
				return "event:/music/act1_boss_ceremonial_beast";
			}
		}

		// Token: 0x06006A28 RID: 27176 RVA: 0x0027EF56 File Offset: 0x0027D156
		public override float GetCameraScaling()
		{
			return 0.9f;
		}

		// Token: 0x06006A29 RID: 27177 RVA: 0x0027EF5D File Offset: 0x0027D15D
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 50f;
		}

		// Token: 0x17001A33 RID: 6707
		// (get) Token: 0x06006A2A RID: 27178 RVA: 0x0027EF6E File Offset: 0x0027D16E
		protected override bool HasCustomBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A34 RID: 6708
		// (get) Token: 0x06006A2B RID: 27179 RVA: 0x0027EF71 File Offset: 0x0027D171
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<MonsterModel>(ModelDb.Monster<CeremonialBeast>());
			}
		}

		// Token: 0x17001A35 RID: 6709
		// (get) Token: 0x06006A2C RID: 27180 RVA: 0x0027EF7D File Offset: 0x0027D17D
		public override IEnumerable<string> ExtraAssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(ModelDb.Affliction<Ringing>().OverlayPath);
			}
		}

		// Token: 0x06006A2D RID: 27181 RVA: 0x0027EF8E File Offset: 0x0027D18E
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<CeremonialBeast>().ToMutable(), null));
		}
	}
}
