using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x0200089F RID: 2207
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OvicopterNormal : EncounterModel
	{
		// Token: 0x17001ABF RID: 6847
		// (get) Token: 0x06006B1C RID: 27420 RVA: 0x002802A4 File Offset: 0x0027E4A4
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AC0 RID: 6848
		// (get) Token: 0x06006B1D RID: 27421 RVA: 0x002802A7 File Offset: 0x0027E4A7
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "egg1", "egg2", "egg3", "egg4", "egg5", "ovicopter" });
			}
		}

		// Token: 0x06006B1E RID: 27422 RVA: 0x002802E4 File Offset: 0x0027E4E4
		public override float GetCameraScaling()
		{
			return 0.8f;
		}

		// Token: 0x06006B1F RID: 27423 RVA: 0x002802EB File Offset: 0x0027E4EB
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 50f + Vector2.Left * 100f;
		}

		// Token: 0x17001AC1 RID: 6849
		// (get) Token: 0x06006B20 RID: 27424 RVA: 0x00280310 File Offset: 0x0027E510
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001AC2 RID: 6850
		// (get) Token: 0x06006B21 RID: 27425 RVA: 0x00280313 File Offset: 0x0027E513
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				return new <>z__ReadOnlyArray<MonsterModel>(new MonsterModel[]
				{
					ModelDb.Monster<Ovicopter>(),
					ModelDb.Monster<ToughEgg>()
				});
			}
		}

		// Token: 0x06006B22 RID: 27426 RVA: 0x00280330 File Offset: 0x0027E530
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new List<ValueTuple<MonsterModel, string>>
			{
				new ValueTuple<MonsterModel, string>(ModelDb.Monster<Ovicopter>().ToMutable(), "ovicopter")
			};
		}

		// Token: 0x04002820 RID: 10272
		private const string _ovicopterSlot = "ovicopter";

		// Token: 0x04002821 RID: 10273
		private const string _eggSlotPrefix = "egg";
	}
}
