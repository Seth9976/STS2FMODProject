using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	/// <summary>
	/// Represents a monster that has been removed from the game. Mostly used for the run history.
	/// </summary>
	// Token: 0x020007B1 RID: 1969
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeprecatedMonster : MonsterModel
	{
		// Token: 0x170016B0 RID: 5808
		// (get) Token: 0x060060D6 RID: 24790 RVA: 0x0025ED76 File Offset: 0x0025CF76
		public override int MinInitialHp
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170016B1 RID: 5809
		// (get) Token: 0x060060D7 RID: 24791 RVA: 0x0025ED79 File Offset: 0x0025CF79
		public override int MaxInitialHp
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x060060D8 RID: 24792 RVA: 0x0025ED7C File Offset: 0x0025CF7C
		public override bool HasDeathSfx
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170016B3 RID: 5811
		// (get) Token: 0x060060D9 RID: 24793 RVA: 0x0025ED7F File Offset: 0x0025CF7F
		public override IEnumerable<string> AssetPaths
		{
			get
			{
				return Array.Empty<string>();
			}
		}

		// Token: 0x060060DA RID: 24794 RVA: 0x0025ED88 File Offset: 0x0025CF88
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			MoveState moveState = new MoveState("STUB", (IReadOnlyList<Creature> _) => Task.CompletedTask, new AbstractIntent[]
			{
				new HiddenIntent()
			});
			moveState.FollowUpState = moveState;
			return new MonsterMoveStateMachine(new <>z__ReadOnlySingleElementList<MonsterState>(moveState), moveState);
		}
	}
}
