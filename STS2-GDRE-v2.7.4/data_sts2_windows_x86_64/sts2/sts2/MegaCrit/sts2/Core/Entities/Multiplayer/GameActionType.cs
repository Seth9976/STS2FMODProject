using System;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	// Token: 0x02000C83 RID: 3203
	public enum GameActionType
	{
		// Token: 0x04002C85 RID: 11397
		None,
		/// <summary>
		/// Canceled if in the queue when we reach the end of combat, or if it is enqueued while we are not in combat.
		/// </summary>
		// Token: 0x04002C86 RID: 11398
		Combat,
		/// <summary>
		/// Canceled if in the queue when we reach the end of combat, or if it is enqueued while we are not in combat.
		/// If it is requested to be enqueued outside of the player's play phase, the request is deferred until it becomes
		/// the play phase for the local player.
		/// This is used for things like card plays and potion uses so that they occur only after the player turn is completely
		/// set up.
		/// </summary>
		// Token: 0x04002C87 RID: 11399
		CombatPlayPhaseOnly,
		/// <summary>
		/// Not canceled at the end of combat.
		/// If the action reaches the front of queue during combat, it will not be executed until the combat ends.
		/// </summary>
		// Token: 0x04002C88 RID: 11400
		NonCombat,
		/// <summary>
		/// Not canceled at the end of combat, and can be executed at any time (in or out of combat).
		/// </summary>
		// Token: 0x04002C89 RID: 11401
		Any
	}
}
