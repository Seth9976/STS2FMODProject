using System;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	// Token: 0x02000C80 RID: 3200
	public enum ActionSynchronizerCombatState
	{
		/// <summary>
		/// We're not in combat.
		/// </summary>
		// Token: 0x04002C70 RID: 11376
		NotInCombat,
		/// <summary>
		/// We're about to be in combat, and we're draining the queue of non-combat actions.
		/// </summary>
		// Token: 0x04002C71 RID: 11377
		PreCombatSetup,
		/// <summary>
		/// We're in combat, and the player is fully in control.
		/// </summary>
		// Token: 0x04002C72 RID: 11378
		PlayPhase,
		/// <summary>
		/// We're in combat, and we're in phase one of ending the turn.
		/// Queues are unpaused during this time to allow generated hook actions (like Well-Laid Plans) to execute, but all
		/// other types of actions will be automatically canceled to prevent interleaving of actions with hooks.
		/// </summary>
		// Token: 0x04002C73 RID: 11379
		EndTurnPhaseOne,
		/// <summary>
		/// We're in combat, and the player is not fully in control. This encapsulates the enemy turn and the draw phase of
		/// the player's turn.
		/// </summary>
		// Token: 0x04002C74 RID: 11380
		NotPlayPhase
	}
}
