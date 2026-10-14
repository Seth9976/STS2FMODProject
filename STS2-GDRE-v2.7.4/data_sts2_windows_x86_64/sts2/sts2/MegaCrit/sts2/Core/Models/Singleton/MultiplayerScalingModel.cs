using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Singleton
{
	/// <summary>
	/// Scales block gained by enemies.
	/// </summary>
	// Token: 0x020004F0 RID: 1264
	[NullableContext(1)]
	[Nullable(0)]
	public class MultiplayerScalingModel : SingletonModel
	{
		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06004EB2 RID: 20146 RVA: 0x0023BE07 File Offset: 0x0023A007
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Initialize with the specified <see cref="T:MegaCrit.Sts2.Core.Runs.RunState" />.
		/// </summary>
		// Token: 0x06004EB3 RID: 20147 RVA: 0x0023BE0A File Offset: 0x0023A00A
		public void Initialize(RunState state)
		{
			if (this._runState != null)
			{
				throw new InvalidOperationException("Already initialized");
			}
			this._runState = state;
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x0023BE26 File Offset: 0x0023A026
		public void OnCombatEntered(CombatState combatState)
		{
			this._combatState = combatState;
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x0023BE2F File Offset: 0x0023A02F
		public void OnCombatFinished()
		{
			this._combatState = null;
		}

		// Token: 0x06004EB6 RID: 20150 RVA: 0x0023BE38 File Offset: 0x0023A038
		[NullableContext(2)]
		public override decimal ModifyBlockMultiplicative([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			if (target != null && !target.IsPrimaryEnemy && !target.IsSecondaryEnemy)
			{
				return 1m;
			}
			if (!props.IsPoweredCardOrMonsterMoveBlock())
			{
				return 1m;
			}
			int count = this._runState.Players.Count;
			if (count <= 2)
			{
				return count;
			}
			return count * MultiplayerScalingModel.GetMultiplayerScaling(this._combatState.Encounter, this._runState.CurrentActIndex);
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x0023BEB0 File Offset: 0x0023A0B0
		[NullableContext(2)]
		public static decimal GetMultiplayerScaling(EncounterModel encounter, int actIndex)
		{
			if (actIndex == 0)
			{
				return 1.1m;
			}
			if (actIndex == 1)
			{
				return 1.2m;
			}
			if (actIndex != 2)
			{
				throw new ArgumentOutOfRangeException("actIndex", actIndex, "Invalid act index for HP scaling");
			}
			if (encounter != null && encounter.RoomType == RoomType.Boss)
			{
				return 1.3m;
			}
			return 1.2m;
		}

		/// <summary>
		/// The <see cref="T:MegaCrit.Sts2.Core.Runs.RunState" /> that this model is scaling.
		/// Should always be set when creating a run, so we mark it as non-nullable.
		/// </summary>
		// Token: 0x04002418 RID: 9240
		private RunState _runState;

		/// <summary>
		/// The CombatState that this model is scaling.
		/// Only set when we are in combat.
		/// </summary>
		// Token: 0x04002419 RID: 9241
		[Nullable(2)]
		private CombatState _combatState;
	}
}
