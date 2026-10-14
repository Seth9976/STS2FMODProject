using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007E7 RID: 2023
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SingleAttackMoveMonster : MonsterModel
	{
		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x06006439 RID: 25657 RVA: 0x0026940C File Offset: 0x0026760C
		public override LocString Title
		{
			get
			{
				return MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");
			}
		}

		// Token: 0x17001820 RID: 6176
		// (get) Token: 0x0600643A RID: 25658 RVA: 0x00269418 File Offset: 0x00267618
		public override int MinInitialHp
		{
			get
			{
				return 999;
			}
		}

		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x0600643B RID: 25659 RVA: 0x0026941F File Offset: 0x0026761F
		public override int MaxInitialHp
		{
			get
			{
				return 999;
			}
		}

		// Token: 0x0600643C RID: 25660 RVA: 0x00269428 File Offset: 0x00267628
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("POKE", new Func<IReadOnlyList<Creature>, Task>(this.PokeMove), new AbstractIntent[]
			{
				new SingleAttackIntent(1)
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600643D RID: 25661 RVA: 0x00269478 File Offset: 0x00267678
		private async Task PokeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(1m).FromMonster(this).Execute(null);
		}
	}
}
