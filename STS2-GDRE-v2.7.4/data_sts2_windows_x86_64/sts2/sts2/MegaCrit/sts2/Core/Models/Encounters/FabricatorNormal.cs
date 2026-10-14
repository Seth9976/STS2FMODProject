using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Encounters
{
	// Token: 0x02000885 RID: 2181
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FabricatorNormal : EncounterModel
	{
		// Token: 0x17001A60 RID: 6752
		// (get) Token: 0x06006A76 RID: 27254 RVA: 0x0027F6B1 File Offset: 0x0027D8B1
		public override RoomType RoomType
		{
			get
			{
				return RoomType.Monster;
			}
		}

		// Token: 0x17001A61 RID: 6753
		// (get) Token: 0x06006A77 RID: 27255 RVA: 0x0027F6B4 File Offset: 0x0027D8B4
		public override bool HasScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A62 RID: 6754
		// (get) Token: 0x06006A78 RID: 27256 RVA: 0x0027F6B7 File Offset: 0x0027D8B7
		public override IReadOnlyList<string> Slots
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "bot1", "bot2", "fabricator", "bot3", "bot4" });
			}
		}

		// Token: 0x17001A63 RID: 6755
		// (get) Token: 0x06006A79 RID: 27257 RVA: 0x0027F6EC File Offset: 0x0027D8EC
		public override IEnumerable<MonsterModel> AllPossibleMonsters
		{
			get
			{
				MonsterModel monsterModel = ModelDb.Monster<Fabricator>();
				HashSet<MonsterModel> defenseSpawns = Fabricator.defenseSpawns;
				HashSet<MonsterModel> aggroSpawns = Fabricator.aggroSpawns;
				int num = 0;
				MonsterModel[] array = new MonsterModel[1 + (defenseSpawns.Count + aggroSpawns.Count)];
				array[num] = monsterModel;
				num++;
				foreach (MonsterModel monsterModel2 in defenseSpawns)
				{
					array[num] = monsterModel2;
					num++;
				}
				foreach (MonsterModel monsterModel2 in aggroSpawns)
				{
					array[num] = monsterModel2;
					num++;
				}
				return new <>z__ReadOnlyArray<MonsterModel>(array);
			}
		}

		// Token: 0x06006A7A RID: 27258 RVA: 0x0027F7BC File Offset: 0x0027D9BC
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected override IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters()
		{
			return new <>z__ReadOnlySingleElementList<ValueTuple<MonsterModel, string>>(new ValueTuple<MonsterModel, string>(ModelDb.Monster<Fabricator>().ToMutable(), "fabricator"));
		}

		// Token: 0x06006A7B RID: 27259 RVA: 0x0027F7D7 File Offset: 0x0027D9D7
		public override float GetCameraScaling()
		{
			return 0.85f;
		}

		// Token: 0x06006A7C RID: 27260 RVA: 0x0027F7DE File Offset: 0x0027D9DE
		public override Vector2 GetCameraOffset()
		{
			return Vector2.Down * 60f;
		}

		// Token: 0x06006A7D RID: 27261 RVA: 0x0027F7F0 File Offset: 0x0027D9F0
		public static void SetBotFallPosition(NCreature creatureNode)
		{
			Node2D specialNode = creatureNode.GetSpecialNode<Node2D>("Visuals/FallControl");
			if (specialNode == null)
			{
				return;
			}
			float num = 125f;
			specialNode.Position = Vector2.Down * ((num - creatureNode.Position.Y) / creatureNode.Visuals.GetCurrentBody().Scale.Y);
		}

		// Token: 0x0400280D RID: 10253
		private const string _fabricatorSlot = "fabricator";

		// Token: 0x0400280E RID: 10254
		private const string _botSlotPrefix = "bot";
	}
}
