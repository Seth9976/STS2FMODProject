using System;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	/// <summary>
	/// Used in multiplayer to sync which screen the player is on.
	/// </summary>
	// Token: 0x02000C8D RID: 3213
	public enum NetScreenType
	{
		// Token: 0x04002CC7 RID: 11463
		None,
		/// <summary>
		/// Whichever room is the current room (e.g. the combat room, or the rest site).
		/// </summary>
		// Token: 0x04002CC8 RID: 11464
		Room,
		/// <summary>
		/// Map screen.
		/// </summary>
		// Token: 0x04002CC9 RID: 11465
		Map,
		/// <summary>
		/// Settings screen.
		/// </summary>
		// Token: 0x04002CCA RID: 11466
		Settings,
		/// <summary>
		/// Compendium screen.
		/// </summary>
		// Token: 0x04002CCB RID: 11467
		Compendium,
		/// <summary>
		/// Deck view screen.
		/// </summary>
		// Token: 0x04002CCC RID: 11468
		DeckView,
		/// <summary>
		/// Any combat card pile screen (exhaust, draw).
		/// </summary>
		// Token: 0x04002CCD RID: 11469
		CardPile,
		/// <summary>
		/// Simple card view screen, e.g. when viewing cards that were transformed by Rebirth
		/// </summary>
		// Token: 0x04002CCE RID: 11470
		SimpleCardsView,
		/// <summary>
		/// Any card selection screen (Survivor card selection, smith card selection, event card selection).
		/// </summary>
		// Token: 0x04002CCF RID: 11471
		CardSelection,
		/// <summary>
		/// Run history screen shown at the end of a run.
		/// </summary>
		// Token: 0x04002CD0 RID: 11472
		GameOver,
		/// <summary>
		/// The Pause menu (ESC during gameplay).
		/// </summary>
		// Token: 0x04002CD1 RID: 11473
		PauseMenu,
		/// <summary>
		/// Any reward offer screen.
		/// </summary>
		// Token: 0x04002CD2 RID: 11474
		Rewards,
		/// <summary>
		/// The send feedback screen.
		/// </summary>
		// Token: 0x04002CD3 RID: 11475
		Feedback,
		/// <summary>
		/// The shared relic picking screen at the treasure room.
		/// </summary>
		// Token: 0x04002CD4 RID: 11476
		SharedRelicPicking,
		/// <summary>
		/// The expanded state displayed when a remote player is clicked.
		/// </summary>
		// Token: 0x04002CD5 RID: 11477
		RemotePlayerExpandedState
	}
}
