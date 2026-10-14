using System;

namespace MegaCrit.Sts2.Core.Combat
{
	/// <summary>
	/// Marks the phase of the player's turn in combat.
	/// </summary>
	// Token: 0x02000D46 RID: 3398
	public enum PlayerTurnPhase
	{
		/// <summary>
		/// No phase. Used when combat is not in progress and during the enemy's turn.
		/// </summary>
		// Token: 0x04002FE6 RID: 12262
		None,
		/// <summary>
		/// The start of the turn.
		/// The player is in this phase from the moment combat switches over from the enemy's turn until the player is
		/// allowed to play cards.
		///
		/// This encompasses many things, including:
		/// - Block clear
		/// - <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.AfterSideTurnStart(MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Entities.Creatures.Creature})" />
		/// - Start-of-turn orb passive triggers
		/// - Energy reset
		/// - Hand draw
		/// </summary>
		// Token: 0x04002FE7 RID: 12263
		Start,
		/// <summary>
		/// The phase of the turn where start-of-turn auto-play effects happen.
		/// This is when <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.AfterAutoPrePlayPhaseEntered(MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext,MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.Entities.Players.Player)" /> is called.
		///
		/// Examples:
		/// - <see cref="T:MegaCrit.Sts2.Core.Models.Relics.HistoryCourse" />
		/// - <see cref="T:MegaCrit.Sts2.Core.Models.Enchantments.Imbued" />
		/// - <see cref="T:MegaCrit.Sts2.Core.Models.Relics.WhisperingEarring" />
		/// </summary>
		// Token: 0x04002FE8 RID: 12264
		AutoPrePlay,
		/// <summary>
		/// The phase of the turn where the player is allowed to manually play cards and use potions.
		/// In this phase, hooks only happen in response to player actions.
		/// </summary>
		// Token: 0x04002FE9 RID: 12265
		Play,
		/// <summary>
		/// The phase of the turn where end-of-turn auto-play effects happen.
		/// This is when <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.AfterAutoPostPlayPhaseEntered(MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext,MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.Entities.Players.Player)" /> is called.
		///
		/// Examples:
		/// - <see cref="T:MegaCrit.Sts2.Core.Models.Cards.IAmInvincible" />
		/// - <see cref="T:MegaCrit.Sts2.Core.Models.Powers.StampedePower" />
		/// </summary>
		// Token: 0x04002FEA RID: 12266
		AutoPostPlay,
		/// <summary>
		/// The end of the turn.
		/// The player is in this phase from the moment all players have hit "end turn" and all queued cards have finished
		/// playing until combat switches over to the enemy's turn.
		///
		/// This encompasses many things, including:
		/// - <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.BeforeSideTurnEnd(MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IEnumerable{MegaCrit.Sts2.Core.Entities.Creatures.Creature})" />
		/// - End-of-turn orb passive triggers
		/// - Hand flush
		/// - <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.AfterSideTurnEnd(MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.Combat.CombatSide,System.Collections.Generic.IEnumerable{MegaCrit.Sts2.Core.Entities.Creatures.Creature})" />
		/// </summary>
		// Token: 0x04002FEB RID: 12267
		End
	}
}
