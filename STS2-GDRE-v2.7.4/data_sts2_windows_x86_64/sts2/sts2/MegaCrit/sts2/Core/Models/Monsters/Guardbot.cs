using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007C2 RID: 1986
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Guardbot : MonsterModel
	{
		// Token: 0x17001716 RID: 5910
		// (get) Token: 0x060061C8 RID: 25032 RVA: 0x00261E71 File Offset: 0x00260071
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 17, 16);
			}
		}

		// Token: 0x17001717 RID: 5911
		// (get) Token: 0x060061C9 RID: 25033 RVA: 0x00261E7D File Offset: 0x0026007D
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 21, 20);
			}
		}

		// Token: 0x17001718 RID: 5912
		// (get) Token: 0x060061CA RID: 25034 RVA: 0x00261E89 File Offset: 0x00260089
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x060061CB RID: 25035 RVA: 0x00261E8C File Offset: 0x0026008C
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			if (TestMode.IsOff)
			{
				FabricatorNormal.SetBotFallPosition(NCombatRoom.Instance.GetCreatureNode(base.Creature));
			}
		}

		// Token: 0x060061CC RID: 25036 RVA: 0x00261ED0 File Offset: 0x002600D0
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("GUARD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GuardMove), new AbstractIntent[]
			{
				new DefendIntent()
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060061CD RID: 25037 RVA: 0x00261F20 File Offset: 0x00260120
		private async Task GuardMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.6f);
			List<Creature> list = base.CombatState.Enemies.Where<Creature>((Creature c) => c.Monster is Fabricator).ToList<Creature>();
			foreach (Creature creature in list)
			{
				await CreatureCmd.GainBlock(creature, 15m, ValueProp.Unpowered, null, false);
			}
			List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
		}
	}
}
