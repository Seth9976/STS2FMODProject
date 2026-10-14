using System;

namespace MegaCrit.Sts2.Core.Rooms
{
	/// <summary>
	/// The mode that the combat room is in.
	/// This allows for combat rooms to be used in many different scenarios. See the specific options for details.
	/// </summary>
	// Token: 0x02000129 RID: 297
	public enum CombatRoomMode
	{
		/// <summary>
		/// Full combat - shows all creatures, combat UI, enables interactivity, runs hooks.
		/// </summary>
		// Token: 0x04000542 RID: 1346
		ActiveCombat,
		/// <summary>
		/// Finished combat - shows players only, no combat UI, no interactivity, no hooks.
		/// Used when loading a save after combat ended but before leaving the room.
		/// </summary>
		// Token: 0x04000543 RID: 1347
		FinishedCombat,
		/// <summary>
		/// Visual only - shows all creatures (allies and enemies) but no combat UI, no interactivity, no hooks.
		/// Used for combat-style events where enemies are visible but not yet fightable.
		/// </summary>
		// Token: 0x04000544 RID: 1348
		VisualOnly
	}
}
