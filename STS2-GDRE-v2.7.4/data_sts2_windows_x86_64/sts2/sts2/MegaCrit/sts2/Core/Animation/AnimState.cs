using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Animation
{
	// Token: 0x02000D99 RID: 3481
	[NullableContext(1)]
	[Nullable(0)]
	public class AnimState
	{
		// Token: 0x1700266F RID: 9839
		// (get) Token: 0x06008C00 RID: 35840 RVA: 0x002EC082 File Offset: 0x002EA282
		public string Id { get; }

		/// <summary>
		/// Is this a looping animation?
		/// </summary>
		// Token: 0x17002670 RID: 9840
		// (get) Token: 0x06008C01 RID: 35841 RVA: 0x002EC08A File Offset: 0x002EA28A
		public bool IsLooping { get; }

		/// <summary>
		/// If this is a looping animation, has it already looped at least once?
		/// </summary>
		// Token: 0x17002671 RID: 9841
		// (get) Token: 0x06008C02 RID: 35842 RVA: 0x002EC092 File Offset: 0x002EA292
		// (set) Token: 0x06008C03 RID: 35843 RVA: 0x002EC09A File Offset: 0x002EA29A
		public bool HasLooped { get; private set; }

		// Token: 0x17002672 RID: 9842
		// (get) Token: 0x06008C04 RID: 35844 RVA: 0x002EC0A3 File Offset: 0x002EA2A3
		// (set) Token: 0x06008C05 RID: 35845 RVA: 0x002EC0AB File Offset: 0x002EA2AB
		[Nullable(2)]
		public AnimState NextState
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17002673 RID: 9843
		// (get) Token: 0x06008C06 RID: 35846 RVA: 0x002EC0B4 File Offset: 0x002EA2B4
		// (set) Token: 0x06008C07 RID: 35847 RVA: 0x002EC0BC File Offset: 0x002EA2BC
		[Nullable(2)]
		public string BoundsContainer
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x06008C08 RID: 35848 RVA: 0x002EC0C5 File Offset: 0x002EA2C5
		public AnimState(string id, bool isLooping = false)
		{
			this.Id = id;
			this.IsLooping = isLooping;
			this._branchedStates = new Dictionary<string, List<AnimState.Branch>>();
		}

		// Token: 0x06008C09 RID: 35849 RVA: 0x002EC0E8 File Offset: 0x002EA2E8
		public void AddBranch(string trigger, AnimState state, [Nullable(2)] Func<bool> condition = null)
		{
			AnimState.Branch branch = new AnimState.Branch
			{
				state = state,
				condition = condition
			};
			List<AnimState.Branch> list;
			if (!this._branchedStates.TryGetValue(trigger, out list))
			{
				list = new List<AnimState.Branch>();
				this._branchedStates[trigger] = list;
			}
			list.Add(branch);
		}

		// Token: 0x06008C0A RID: 35850 RVA: 0x002EC13C File Offset: 0x002EA33C
		[return: Nullable(2)]
		public AnimState CallTrigger(string trigger)
		{
			List<AnimState.Branch> list;
			if (this._branchedStates.TryGetValue(trigger, out list))
			{
				foreach (AnimState.Branch branch in list)
				{
					Func<bool> condition = branch.condition;
					if (condition == null || condition())
					{
						return branch.state;
					}
				}
			}
			return null;
		}

		// Token: 0x06008C0B RID: 35851 RVA: 0x002EC1B4 File Offset: 0x002EA3B4
		public bool HasTrigger(string trigger)
		{
			return this._branchedStates.ContainsKey(trigger);
		}

		/// <summary>
		/// Mark that this animation has looped at least once.
		/// </summary>
		// Token: 0x06008C0C RID: 35852 RVA: 0x002EC1C2 File Offset: 0x002EA3C2
		public void MarkHasLooped()
		{
			this.HasLooped = true;
		}

		// Token: 0x040030D9 RID: 12505
		public const string attackAnim = "attack";

		// Token: 0x040030DA RID: 12506
		public const string castAnim = "cast";

		// Token: 0x040030DB RID: 12507
		public const string dieAnim = "die";

		// Token: 0x040030DC RID: 12508
		public const string hurtAnim = "hurt";

		// Token: 0x040030DD RID: 12509
		public const string idleAnim = "idle_loop";

		// Token: 0x040030DE RID: 12510
		public const string reviveAnim = "revive";

		// Token: 0x040030DF RID: 12511
		public const string stunAnim = "stun";

		// Token: 0x040030E3 RID: 12515
		private readonly Dictionary<string, List<AnimState.Branch>> _branchedStates;

		// Token: 0x02002572 RID: 9586
		[NullableContext(0)]
		private struct Branch
		{
			// Token: 0x0400A046 RID: 41030
			[Nullable(1)]
			public AnimState state;

			// Token: 0x0400A047 RID: 41031
			[Nullable(2)]
			public Func<bool> condition;
		}
	}
}
