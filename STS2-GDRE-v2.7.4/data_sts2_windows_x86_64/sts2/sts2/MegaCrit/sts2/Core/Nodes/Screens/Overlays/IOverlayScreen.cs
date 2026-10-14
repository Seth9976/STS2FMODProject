using System;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Overlays
{
	/// <summary>
	/// An interface used by the NOverlayStack to manage screens that need to visibly stack on top of each other
	/// The requirements to what should be considered to be made an overlay screen are as follows:
	/// - need specific interaction when opened on top of with other screens
	/// - These screens are created and destroyed on demand
	///
	/// Ex : "Combat reward screen" and "Choose a Card Screen"
	///   - both are used for a limited amount of time, so we can create/destroy them as needed
	///   - when I open the "Choose a Card Screen", the "Combat reward screen" sits underneath it while being disabled
	///   - when I close the "Choose a Card Screen", the "Combat reward screen" needs to become visible again
	/// </summary>
	// Token: 0x020002E2 RID: 738
	public interface IOverlayScreen : IScreenContext
	{
		/// <summary>
		/// The screen type which will be synced with peers.
		/// This is used to display an icon next to the multiplayer player's icon, and to determine whether the remote
		/// player's mouse should be shown (i.e. whether they're on the same screen as you).
		/// </summary>
		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002789 RID: 10121
		NetScreenType ScreenType { get; }

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x0600278A RID: 10122
		bool UseSharedBackstop { get; }

		// Token: 0x0600278B RID: 10123
		void AfterOverlayOpened();

		// Token: 0x0600278C RID: 10124
		void AfterOverlayClosed();

		// Token: 0x0600278D RID: 10125
		void AfterOverlayShown();

		// Token: 0x0600278E RID: 10126
		void AfterOverlayHidden();
	}
}
