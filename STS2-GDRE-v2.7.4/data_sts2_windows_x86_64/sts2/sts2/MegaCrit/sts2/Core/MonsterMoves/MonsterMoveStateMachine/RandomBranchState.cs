using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine
{
	// Token: 0x020004C2 RID: 1218
	[NullableContext(1)]
	[Nullable(0)]
	public class RandomBranchState : MonsterState
	{
		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x060048FC RID: 18684 RVA: 0x00225567 File Offset: 0x00223767
		public string StateId { get; }

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x060048FD RID: 18685 RVA: 0x0022556F File Offset: 0x0022376F
		// (set) Token: 0x060048FE RID: 18686 RVA: 0x00225577 File Offset: 0x00223777
		public List<RandomBranchState.StateWeight> States { get; set; } = new List<RandomBranchState.StateWeight>();

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x060048FF RID: 18687 RVA: 0x00225580 File Offset: 0x00223780
		public override bool ShouldAppearInLogs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x00225583 File Offset: 0x00223783
		public RandomBranchState(string id)
		{
			this.StateId = id;
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x002255A0 File Offset: 0x002237A0
		public void AddBranch(MonsterState state, int cooldown, MoveRepeatType repeatType, Func<float> weight)
		{
			if (repeatType.Equals(MoveRepeatType.CanRepeatXTimes))
			{
				throw new ArgumentException("Use other constructor to specify number of repeats");
			}
			RandomBranchState.StateWeight stateWeight = new RandomBranchState.StateWeight
			{
				repeatType = repeatType,
				stateId = state.Id,
				weightLambda = weight,
				cooldown = cooldown
			};
			this.States.Add(stateWeight);
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x0022560C File Offset: 0x0022380C
		public void AddBranch(MonsterState state, int cooldown, int maxRepeats, Func<float> weight)
		{
			RandomBranchState.StateWeight stateWeight = new RandomBranchState.StateWeight
			{
				maxTimes = maxRepeats,
				repeatType = MoveRepeatType.CanRepeatXTimes,
				stateId = state.Id,
				weightLambda = weight,
				cooldown = cooldown
			};
			this.States.Add(stateWeight);
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x0022565D File Offset: 0x0022385D
		public void AddBranch(MonsterState state, int maxRepeats, Func<float> weight)
		{
			this.AddBranch(state, 0, maxRepeats, weight);
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x0022566C File Offset: 0x0022386C
		public void AddBranch(MonsterState state, int cooldown, MoveRepeatType repeatType, float weight)
		{
			this.AddBranch(state, cooldown, repeatType, () => weight);
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x0022569C File Offset: 0x0022389C
		public void AddBranch(MonsterState state, MoveRepeatType repeatType, float weight)
		{
			this.AddBranch(state, repeatType, () => weight);
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x002256CA File Offset: 0x002238CA
		public void AddBranch(MonsterState state, MoveRepeatType repeatType, Func<float> weight)
		{
			this.AddBranch(state, 0, repeatType, weight);
		}

		// Token: 0x06004907 RID: 18695 RVA: 0x002256D8 File Offset: 0x002238D8
		public void AddBranch(MonsterState state, int maxRepeats, float weight)
		{
			this.AddBranch(state, maxRepeats, () => weight);
		}

		// Token: 0x06004908 RID: 18696 RVA: 0x00225706 File Offset: 0x00223906
		public void AddBranch(MonsterState state, int cooldown, MoveRepeatType repeatType)
		{
			this.AddBranch(state, cooldown, repeatType, 1f);
		}

		// Token: 0x06004909 RID: 18697 RVA: 0x00225716 File Offset: 0x00223916
		public void AddBranch(MonsterState state, int maxRepeats)
		{
			this.AddBranch(state, maxRepeats, 1f);
		}

		// Token: 0x0600490A RID: 18698 RVA: 0x00225725 File Offset: 0x00223925
		public void AddBranch(MonsterState state, MoveRepeatType repeatType)
		{
			this.AddBranch(state, repeatType, 1f);
		}

		// Token: 0x0600490B RID: 18699 RVA: 0x00225734 File Offset: 0x00223934
		public override string GetNextState(Creature owner, Rng rng)
		{
			float num = this.States.Sum<RandomBranchState.StateWeight>((RandomBranchState.StateWeight x) => RandomBranchState.GetStateWeight(x, owner));
			float num2 = rng.NextFloat(num);
			foreach (RandomBranchState.StateWeight stateWeight in this.States)
			{
				num2 -= RandomBranchState.GetStateWeight(stateWeight, owner);
				if (num2 <= 0f)
				{
					return stateWeight.stateId;
				}
			}
			throw new InvalidOperationException("No valid state found in RandomBranchState " + this.Id + "!");
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x002257F0 File Offset: 0x002239F0
		private static float GetStateWeight(RandomBranchState.StateWeight stateWeight, Creature owner)
		{
			MonsterMoveStateMachine moveStateMachine = owner.Monster.MoveStateMachine;
			float num = 1f;
			if (stateWeight.repeatType.Equals(MoveRepeatType.UseOnlyOnce))
			{
				MonsterState monsterState = moveStateMachine.States[stateWeight.stateId];
				if (moveStateMachine.StateLog.Contains(monsterState))
				{
					num = 0f;
				}
			}
			else if (!stateWeight.repeatType.Equals(MoveRepeatType.CanRepeatForever))
			{
				float num2 = (float)(stateWeight.repeatType.Equals(MoveRepeatType.CannotRepeat) ? 1 : stateWeight.maxTimes);
				num = (float)moveStateMachine.StateLog.Count < num2;
				int num3 = 0;
				while ((float)moveStateMachine.StateLog.Count >= num2 && (float)num3 < num2 && moveStateMachine.StateLog.Count - num3 > 0)
				{
					MonsterState monsterState2 = moveStateMachine.States[stateWeight.stateId];
					if (moveStateMachine.StateLog[moveStateMachine.StateLog.Count - 1 - num3] != monsterState2)
					{
						num = 1f;
						break;
					}
					num3++;
				}
			}
			if (stateWeight.cooldown > 0)
			{
				IEnumerable<MonsterState> enumerable = moveStateMachine.StateLog.Where<MonsterState>((MonsterState state) => state.IsMove).Reverse<MonsterState>().Take<MonsterState>(stateWeight.cooldown);
				if (enumerable.Any<MonsterState>((MonsterState move) => move.Id == stateWeight.stateId))
				{
					return 0f;
				}
			}
			return num * stateWeight.GetWeight();
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x002259B8 File Offset: 0x00223BB8
		public override void RegisterStates(Dictionary<string, MonsterState> monsterStates)
		{
			monsterStates.Add(this.Id, this);
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x0600490E RID: 18702 RVA: 0x002259C7 File Offset: 0x00223BC7
		public override string Id
		{
			get
			{
				return this.StateId;
			}
		}

		// Token: 0x020019F6 RID: 6646
		[Nullable(0)]
		public struct StateWeight
		{
			// Token: 0x0600ABE9 RID: 44009 RVA: 0x0036441A File Offset: 0x0036261A
			public float GetWeight()
			{
				if (this.weightLambda != null)
				{
					return this.weightLambda();
				}
				throw new InvalidOperationException(this.stateId + " doesn't have a weight");
			}

			// Token: 0x04006669 RID: 26217
			public string stateId;

			// Token: 0x0400666A RID: 26218
			public MoveRepeatType repeatType;

			// Token: 0x0400666B RID: 26219
			public int maxTimes;

			// Token: 0x0400666C RID: 26220
			public Func<float> weightLambda;

			// Token: 0x0400666D RID: 26221
			public int cooldown;
		}
	}
}
