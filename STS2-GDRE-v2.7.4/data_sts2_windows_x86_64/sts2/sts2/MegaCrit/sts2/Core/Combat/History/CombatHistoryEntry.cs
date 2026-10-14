using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Combat.History
{
	// Token: 0x02000D48 RID: 3400
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class CombatHistoryEntry
	{
		/// <summary>
		/// Who performed this action?
		/// Even if this was a player-specific action (like drawing a card or gaining energy),
		/// this will be the player's creature.
		/// Null if the action was not performed by any creature, like when a player loses HP from Royal Poison.
		/// </summary>
		// Token: 0x170025C5 RID: 9669
		// (get) Token: 0x060089E1 RID: 35297 RVA: 0x002E34D8 File Offset: 0x002E16D8
		public Creature Actor { get; }

		/// <summary>
		/// What round was it when this action occurred?
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Players.PlayerCombatState.TurnNumber" /> and <see cref="P:MegaCrit.Sts2.Core.Combat.ICombatState.RoundNumber" /> to understand the
		/// difference between the two.
		/// </summary>
		// Token: 0x170025C6 RID: 9670
		// (get) Token: 0x060089E2 RID: 35298 RVA: 0x002E34E0 File Offset: 0x002E16E0
		private int RoundNumber { get; }

		/// <summary>
		/// Which side's turn was it when this action occurred?
		///
		/// This will not always be the same as Actor.Side. For example, if you attack a monster with Thorns, the entry for
		/// the Thorns damage will have Actor.Side = Monster (since the monster is the one dealing damage back), but
		/// CurrentSide will be Player (since it's the player's turn).
		/// </summary>
		// Token: 0x170025C7 RID: 9671
		// (get) Token: 0x060089E3 RID: 35299 RVA: 0x002E34E8 File Offset: 0x002E16E8
		private CombatSide CurrentSide { get; }

		/// <summary>
		/// The history object that this entry is in.
		/// </summary>
		// Token: 0x170025C8 RID: 9672
		// (get) Token: 0x060089E4 RID: 35300 RVA: 0x002E34F0 File Offset: 0x002E16F0
		public CombatHistory History { get; }

		// Token: 0x060089E5 RID: 35301 RVA: 0x002E34F8 File Offset: 0x002E16F8
		protected CombatHistoryEntry(Creature actor, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
		{
			this.Actor = actor;
			this.RoundNumber = roundNumber;
			this.CurrentSide = currentSide;
			this.History = history;
			foreach (Player player in players)
			{
				this._playerTurnNumbers[player.NetId] = player.PlayerCombatState.TurnNumber;
			}
		}

		/// <summary>
		/// Did this action happen during the current turn?
		/// </summary>
		// Token: 0x060089E6 RID: 35302 RVA: 0x002E3584 File Offset: 0x002E1784
		[NullableContext(2)]
		public bool HappenedThisTurn(ICombatState state)
		{
			if (state == null)
			{
				return false;
			}
			if (this.RoundNumber != state.RoundNumber)
			{
				return false;
			}
			if (this.CurrentSide != state.CurrentSide)
			{
				return false;
			}
			foreach (KeyValuePair<ulong, int> keyValuePair in this._playerTurnNumbers)
			{
				ulong num;
				int num2;
				keyValuePair.Deconstruct(out num, out num2);
				ulong num3 = num;
				int num4 = num2;
				Player player = state.GetPlayer(num3);
				bool flag;
				if (player == null)
				{
					flag = true;
				}
				else
				{
					PlayerCombatState playerCombatState = player.PlayerCombatState;
					int? num5 = ((playerCombatState != null) ? new int?(playerCombatState.TurnNumber) : null);
					num2 = num4;
					flag = !((num5.GetValueOrDefault() == num2) & (num5 != null));
				}
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Did this action happen during the specified player's last turn?
		/// </summary>
		// Token: 0x060089E7 RID: 35303 RVA: 0x002E365C File Offset: 0x002E185C
		public bool HappenedLastPlayerTurn(Player player)
		{
			int num;
			if (!this._playerTurnNumbers.TryGetValue(player.NetId, out num))
			{
				return false;
			}
			int num2 = num;
			PlayerCombatState playerCombatState = player.PlayerCombatState;
			int? num3 = ((playerCombatState != null) ? new int?(playerCombatState.TurnNumber - 1) : null);
			return (num2 == num3.GetValueOrDefault()) & (num3 != null);
		}

		// Token: 0x170025C9 RID: 9673
		// (get) Token: 0x060089E8 RID: 35304 RVA: 0x002E36B4 File Offset: 0x002E18B4
		public string HumanReadableString
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Rd ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.RoundNumber);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<CombatSide>(this.CurrentSide);
				defaultInterpolatedStringHandler.AppendLiteral(" turn): ");
				defaultInterpolatedStringHandler.AppendFormatted(this.Description);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x170025CA RID: 9674
		// (get) Token: 0x060089E9 RID: 35305
		public abstract string Description { get; }

		/// <summary>
		/// The turn numbers of all the players in combat when this action occurred.
		/// </summary>
		// Token: 0x04002FF2 RID: 12274
		private readonly Dictionary<ulong, int> _playerTurnNumbers = new Dictionary<ulong, int>();
	}
}
