using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput
{
	/// <summary>
	/// Keeps track of the local player's screen state and synchronizes it to remote peers via the PeerInputSynchronizer.
	/// </summary>
	// Token: 0x020004B2 RID: 1202
	public class ScreenStateTracker
	{
		// Token: 0x06004838 RID: 18488 RVA: 0x00221584 File Offset: 0x0021F784
		[NullableContext(1)]
		public ScreenStateTracker(NMapScreen mapScreen, NCapstoneContainer capstoneContainer, NOverlayStack overlayStack)
		{
			this._onRewardsScreenCompleted = Callable.From(new Action(this.SyncLocalScreen));
			capstoneContainer.Connect(NCapstoneContainer.SignalName.Changed, Callable.From(new Action(this.OnCapstoneScreenChanged)), 0U);
			overlayStack.Connect(NOverlayStack.SignalName.Changed, Callable.From(new Action(this.OnOverlayStackChanged)), 0U);
			mapScreen.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnMapScreenVisibilityChanged)), 0U);
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x00221608 File Offset: 0x0021F808
		private void OnCapstoneScreenChanged()
		{
			if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
			{
				return;
			}
			ICapstoneScreen currentCapstoneScreen = NCapstoneContainer.Instance.CurrentCapstoneScreen;
			this._capstoneScreen = ((currentCapstoneScreen != null) ? currentCapstoneScreen.ScreenType : NetScreenType.None);
			this.SyncLocalScreen();
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x00221648 File Offset: 0x0021F848
		private void OnOverlayStackChanged()
		{
			if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
			{
				return;
			}
			IOverlayScreen overlayScreen = NOverlayStack.Instance.Peek();
			NRewardsScreen nrewardsScreen = overlayScreen as NRewardsScreen;
			if (nrewardsScreen != null && !nrewardsScreen.IsConnected(NRewardsScreen.SignalName.Completed, this._onRewardsScreenCompleted))
			{
				nrewardsScreen.Connect(NRewardsScreen.SignalName.Completed, this._onRewardsScreenCompleted, 0U);
			}
			this._overlayScreen = ((overlayScreen != null) ? overlayScreen.ScreenType : NetScreenType.None);
			this.SyncLocalScreen();
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x002216B5 File Offset: 0x0021F8B5
		private void SyncLocalScreen()
		{
			RunManager.Instance.InputSynchronizer.SyncLocalScreen(this.GetCurrentScreen());
		}

		/// <summary>
		/// The map is not an overlay or a capstone, it's part of <see cref="T:MegaCrit.Sts2.Core.Nodes.NRun" />.
		/// </summary>
		// Token: 0x0600483C RID: 18492 RVA: 0x002216CC File Offset: 0x0021F8CC
		private void OnMapScreenVisibilityChanged()
		{
			this._mapScreenVisible = NMapScreen.Instance.Visible;
			RunManager.Instance.InputSynchronizer.SyncLocalScreen(this.GetCurrentScreen());
		}

		/// <summary>
		/// The shared relic picking screen at the treasure room is not an overlay or a capstone, it's part of the state of
		/// the room, so it needs to get synchronized manually.
		/// </summary>
		// Token: 0x0600483D RID: 18493 RVA: 0x002216F3 File Offset: 0x0021F8F3
		public void SetIsInSharedRelicPickingScreen(bool isInSharedRelicPicking)
		{
			this._isInSharedRelicPicking = isInSharedRelicPicking;
			RunManager.Instance.InputSynchronizer.SyncLocalScreen(this.GetCurrentScreen());
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x00221714 File Offset: 0x0021F914
		private NetScreenType GetCurrentScreen()
		{
			if (this._capstoneScreen != NetScreenType.None)
			{
				return this._capstoneScreen;
			}
			if (this._mapScreenVisible)
			{
				return NetScreenType.Map;
			}
			if (this._overlayScreen == NetScreenType.Rewards)
			{
				NRewardsScreen nrewardsScreen = NOverlayStack.Instance.Peek() as NRewardsScreen;
				if (nrewardsScreen != null && !nrewardsScreen.IsComplete)
				{
					return this._overlayScreen;
				}
			}
			else if (this._overlayScreen != NetScreenType.None)
			{
				return this._overlayScreen;
			}
			if (this._isInSharedRelicPicking)
			{
				return NetScreenType.SharedRelicPicking;
			}
			return NetScreenType.Room;
		}

		// Token: 0x04001C6C RID: 7276
		private NetScreenType _capstoneScreen;

		// Token: 0x04001C6D RID: 7277
		private NetScreenType _overlayScreen;

		// Token: 0x04001C6E RID: 7278
		private bool _mapScreenVisible;

		// Token: 0x04001C6F RID: 7279
		private bool _isInSharedRelicPicking;

		/// <summary>
		/// Callable.From() creates a new delegate each call, so we store it to ensure IsConnected() can match the same
		/// instance used to Connect(). This prevents Godot's "signal already connected" error when the rewards screen
		/// re-emerges as the top overlay after another overlay is pushed on top and later popped.
		/// </summary>
		// Token: 0x04001C70 RID: 7280
		private readonly Callable _onRewardsScreenCompleted;
	}
}
