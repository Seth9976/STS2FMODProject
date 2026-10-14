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
	// Token: 0x020007D3 RID: 2003
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MultiAttackMoveMonster : MonsterModel
	{
		// Token: 0x17001799 RID: 6041
		// (get) Token: 0x06006303 RID: 25347 RVA: 0x00265B72 File Offset: 0x00263D72
		public override LocString Title
		{
			get
			{
				return MonsterModel.L10NMonsterLookup("BIG_DUMMY.name");
			}
		}

		// Token: 0x1700179A RID: 6042
		// (get) Token: 0x06006304 RID: 25348 RVA: 0x00265B7E File Offset: 0x00263D7E
		public override int MinInitialHp
		{
			get
			{
				return 999;
			}
		}

		// Token: 0x1700179B RID: 6043
		// (get) Token: 0x06006305 RID: 25349 RVA: 0x00265B85 File Offset: 0x00263D85
		public override int MaxInitialHp
		{
			get
			{
				return 999;
			}
		}

		// Token: 0x06006306 RID: 25350 RVA: 0x00265B8C File Offset: 0x00263D8C
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("POKE", new Func<IReadOnlyList<Creature>, Task>(this.PokeMove), new AbstractIntent[]
			{
				new MultiAttackIntent(1, 5)
			});
			moveState.FollowUpState = moveState;
			list.Add(moveState);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x06006307 RID: 25351 RVA: 0x00265BDC File Offset: 0x00263DDC
		private async Task PokeMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(1m).WithHitCount(5).FromMonster(this)
				.Execute(null);
		}

		// Token: 0x04002632 RID: 9778
		public const int repeat = 5;
	}
}
