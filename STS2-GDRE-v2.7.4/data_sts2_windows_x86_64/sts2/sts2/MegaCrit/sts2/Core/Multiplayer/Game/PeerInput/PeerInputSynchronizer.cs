using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput
{
	/// <summary>
	/// Synchronizes our local mouse state with peers and remote peers' with ours.
	/// </summary>
	// Token: 0x020004B1 RID: 1201
	[NullableContext(2)]
	[Nullable(0)]
	public class PeerInputSynchronizer : IDisposable
	{
		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06004816 RID: 18454 RVA: 0x002208C5 File Offset: 0x0021EAC5
		[Nullable(1)]
		public INetGameService NetService
		{
			[NullableContext(1)]
			get
			{
				return this._netService;
			}
		}

		/// <summary>
		/// Event fired when we first add a PeerInputState for a player.
		/// This occurs when a player emits a <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.PeerInputMessage" />, or when we attempt to get the input state for a player.
		/// </summary>
		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06004817 RID: 18455 RVA: 0x002208D0 File Offset: 0x0021EAD0
		// (remove) Token: 0x06004818 RID: 18456 RVA: 0x00220908 File Offset: 0x0021EB08
		public event Action<ulong> StateAdded;

		/// <summary>
		/// Event fired when a player disconnects, and their input state is removed.
		/// </summary>
		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06004819 RID: 18457 RVA: 0x00220940 File Offset: 0x0021EB40
		// (remove) Token: 0x0600481A RID: 18458 RVA: 0x00220978 File Offset: 0x0021EB78
		public event Action<ulong> StateRemoved;

		/// <summary>
		/// Event fired when any part of a player's input state is changed.
		/// </summary>
		// Token: 0x14000077 RID: 119
		// (add) Token: 0x0600481B RID: 18459 RVA: 0x002209B0 File Offset: 0x0021EBB0
		// (remove) Token: 0x0600481C RID: 18460 RVA: 0x002209E8 File Offset: 0x0021EBE8
		public event Action<ulong> StateChanged;

		/// <summary>
		/// Event fired only when a player emits a PeerInputMessage with a new screen type.
		/// The second argument is the old screen type.
		/// </summary>
		// Token: 0x14000078 RID: 120
		// (add) Token: 0x0600481D RID: 18461 RVA: 0x00220A20 File Offset: 0x0021EC20
		// (remove) Token: 0x0600481E RID: 18462 RVA: 0x00220A58 File Offset: 0x0021EC58
		public event Action<ulong, NetScreenType> ScreenChanged;

		// Token: 0x0600481F RID: 18463 RVA: 0x00220A90 File Offset: 0x0021EC90
		[NullableContext(1)]
		public PeerInputSynchronizer(INetGameService netService)
		{
			this._netService = netService;
			this._netService.RegisterMessageHandler<PeerInputMessage>(new MessageHandlerDelegate<PeerInputMessage>(this.HandlePeerInputMessage));
			this.GetOrCreateStateForPlayer(this._netService.NetId);
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x00220AEF File Offset: 0x0021ECEF
		public void Dispose()
		{
			this._netService.UnregisterMessageHandler<PeerInputMessage>(new MessageHandlerDelegate<PeerInputMessage>(this.HandlePeerInputMessage));
		}

		/// <summary>
		/// Sync the local mouse position to other clients.
		/// This should be called any time the mouse position changes - the message will be buffered and sent at regular intervals.
		/// </summary>
		/// <param name="mouseScreenPos">The screen position of the mouse.</param>
		/// <param name="rootControl">The root control, used to normalize mouse coordinates across different resolutions and aspect ratios.</param>
		// Token: 0x06004821 RID: 18465 RVA: 0x00220B08 File Offset: 0x0021ED08
		public void SyncLocalMousePos(Vector2 mouseScreenPos, Control rootControl)
		{
			PeerInputSynchronizer.PeerInputState orCreateStateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
			if (this._syncMessageToSend == null)
			{
				this._syncMessageToSend = new PeerInputMessage();
			}
			INetCursorPositionTranslator cursorTranslator = this._cursorTranslator;
			Vector2 vector = ((cursorTranslator != null) ? cursorTranslator.GetNetPositionFromScreenPosition(mouseScreenPos) : NetCursorHelper.GetNormalizedPosition(mouseScreenPos, rootControl));
			this._syncMessageToSend.netMousePos = new Vector2?(vector);
			orCreateStateForPlayer.netMousePosition = vector;
			Action<ulong> stateChanged = this.StateChanged;
			if (stateChanged != null)
			{
				stateChanged(this._netService.NetId);
			}
			this.TrySendSyncMessage();
		}

		/// <summary>
		/// Sync the position of the currently focused control node with other clients.
		/// </summary>
		/// <param name="focusPosition">the screen position of the currently focused control node</param>
		/// <param name="rootControl">The root control, used to normalize mouse coordinates across different resolutions and aspect ratios.</param>
		// Token: 0x06004822 RID: 18466 RVA: 0x00220B90 File Offset: 0x0021ED90
		public void SyncLocalControllerFocus(Vector2 focusPosition, Control rootControl)
		{
			PeerInputSynchronizer.PeerInputState orCreateStateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
			if (this._syncMessageToSend == null)
			{
				this._syncMessageToSend = new PeerInputMessage();
			}
			INetCursorPositionTranslator cursorTranslator = this._cursorTranslator;
			Vector2 vector = ((cursorTranslator != null) ? cursorTranslator.GetNetPositionFromScreenPosition(focusPosition) : NetCursorHelper.GetNormalizedPosition(focusPosition, rootControl));
			this._syncMessageToSend.controllerFocusPosition = new Vector2?(vector);
			orCreateStateForPlayer.controllerFocusPosition = vector;
			Action<ulong> stateChanged = this.StateChanged;
			if (stateChanged != null)
			{
				stateChanged(this._netService.NetId);
			}
			this.TrySendSyncMessage();
		}

		/// <summary>
		/// Sync whether or not the local player is using a controller.
		/// </summary>
		// Token: 0x06004823 RID: 18467 RVA: 0x00220C18 File Offset: 0x0021EE18
		public void SyncLocalIsUsingController(bool isUsingController)
		{
			PeerInputSynchronizer.PeerInputState orCreateStateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
			if (this._syncMessageToSend == null)
			{
				this._syncMessageToSend = new PeerInputMessage();
			}
			this._syncMessageToSend.isUsingController = isUsingController;
			orCreateStateForPlayer.isUsingController = isUsingController;
			Action<ulong> stateChanged = this.StateChanged;
			if (stateChanged != null)
			{
				stateChanged(this._netService.NetId);
			}
			this.TrySendSyncMessage();
		}

		/// <summary>
		/// Sync the local mouse down state with other clients (so they show the correct cursor).
		/// </summary>
		/// <param name="mouseDown">True if the mouse is down, false otherwise.</param>
		// Token: 0x06004824 RID: 18468 RVA: 0x00220C80 File Offset: 0x0021EE80
		public void SyncLocalMouseDown(bool mouseDown)
		{
			PeerInputSynchronizer.PeerInputState orCreateStateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
			if (this._syncMessageToSend == null)
			{
				this._syncMessageToSend = new PeerInputMessage();
			}
			orCreateStateForPlayer.isMouseDown = mouseDown;
			orCreateStateForPlayer.isUsingController = false;
			Action<ulong> stateChanged = this.StateChanged;
			if (stateChanged != null)
			{
				stateChanged(this._netService.NetId);
			}
			this.TrySendSyncMessage();
		}

		/// <summary>
		/// Sync the topmost overlay/capstone on the local player's screen to other peers.
		/// </summary>
		/// <param name="netScreenType">The topmost overlay/capstone on the local player's screen.</param>
		// Token: 0x06004825 RID: 18469 RVA: 0x00220CE4 File Offset: 0x0021EEE4
		public void SyncLocalScreen(NetScreenType netScreenType)
		{
			PeerInputSynchronizer.PeerInputState orCreateStateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
			if (this._syncMessageToSend == null)
			{
				this._syncMessageToSend = new PeerInputMessage();
			}
			if (orCreateStateForPlayer.netScreenType != netScreenType)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Local screen changed: ");
				defaultInterpolatedStringHandler.AppendFormatted<NetScreenType>(orCreateStateForPlayer.netScreenType);
				defaultInterpolatedStringHandler.AppendLiteral("->");
				defaultInterpolatedStringHandler.AppendFormatted<NetScreenType>(netScreenType);
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				orCreateStateForPlayer.netScreenType = netScreenType;
				this.TrySendSyncMessage();
				Action<ulong> stateChanged = this.StateChanged;
				if (stateChanged == null)
				{
					return;
				}
				stateChanged(this._netService.NetId);
			}
		}

		/// <summary>
		/// ONLY the HoveredModelTracker should call this! You likely want to call one of the HoveredModelTracker methods instead.
		/// Sync the model that the player is currently hovering.
		/// </summary>
		/// <param name="model">The model that the local player is hovering.</param>
		// Token: 0x06004826 RID: 18470 RVA: 0x00220D94 File Offset: 0x0021EF94
		public void SyncLocalHoveredModel(AbstractModel model)
		{
			PeerInputSynchronizer.PeerInputState orCreateStateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
			if (this._syncMessageToSend == null)
			{
				this._syncMessageToSend = new PeerInputMessage();
			}
			HoveredModelData hoveredModelData = HoveredModelData.FromModel(model);
			if (!hoveredModelData.Equals(orCreateStateForPlayer.hoveredModelData))
			{
				orCreateStateForPlayer.hoveredModelData = hoveredModelData;
				this.TrySendSyncMessage();
				Action<ulong> stateChanged = this.StateChanged;
				if (stateChanged == null)
				{
					return;
				}
				stateChanged(this._netService.NetId);
			}
		}

		/// <summary>
		/// Sync whether or not the player is currently in targeting mode.
		/// </summary>
		/// <param name="isTargeting">True if we're currently displaying the targeting arrow locally, false otherwise.</param>
		// Token: 0x06004827 RID: 18471 RVA: 0x00220E04 File Offset: 0x0021F004
		public void SyncLocalIsTargeting(bool isTargeting)
		{
			PeerInputSynchronizer.PeerInputState orCreateStateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
			if (this._syncMessageToSend == null)
			{
				this._syncMessageToSend = new PeerInputMessage();
			}
			if (orCreateStateForPlayer.isTargeting != isTargeting)
			{
				orCreateStateForPlayer.isTargeting = isTargeting;
				this.TrySendSyncMessage();
				Action<ulong> stateChanged = this.StateChanged;
				if (stateChanged == null)
				{
					return;
				}
				stateChanged(this._netService.NetId);
			}
		}

		/// <summary>
		/// Sends a sync message if enough time has passed since the last one, or buffers it to be sent otherwise.
		/// </summary>
		// Token: 0x06004828 RID: 18472 RVA: 0x00220E68 File Offset: 0x0021F068
		private void TrySendSyncMessage()
		{
			if (this._syncMessageTask != null)
			{
				return;
			}
			int num = (int)(this._lastSyncMsec + 50UL - this.GetTicksMsec());
			if (num <= 0)
			{
				this._syncMessageTask = TaskHelper.RunSafely(this.SendSyncMessageAfterSmallDelay());
				return;
			}
			this._syncMessageTask = TaskHelper.RunSafely(this.QueueSyncMessage(num));
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x00220EBC File Offset: 0x0021F0BC
		[NullableContext(1)]
		private async Task QueueSyncMessage(int delayMsec)
		{
			Func<int, Task> func = this.mockDelay;
			await (((func != null) ? func(delayMsec) : null) ?? Task.Delay(delayMsec));
			this.SendSyncMessage();
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x00220F08 File Offset: 0x0021F108
		[NullableContext(1)]
		private async Task SendSyncMessageAfterSmallDelay()
		{
			if (this.mockWaitSmall != null)
			{
				await this.mockWaitSmall();
			}
			else
			{
				await Task.Yield();
			}
			this.SendSyncMessage();
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x00220F4C File Offset: 0x0021F14C
		private void SendSyncMessage()
		{
			if (!this._netService.IsConnected)
			{
				return;
			}
			PeerInputSynchronizer.PeerInputState orCreateStateForPlayer = this.GetOrCreateStateForPlayer(this._netService.NetId);
			this._syncMessageToSend.mouseDown = orCreateStateForPlayer.isMouseDown;
			this._syncMessageToSend.screenType = orCreateStateForPlayer.netScreenType;
			this._syncMessageToSend.isTargeting = orCreateStateForPlayer.isTargeting;
			this._syncMessageToSend.hoveredModelData = orCreateStateForPlayer.hoveredModelData;
			this._syncMessageToSend.isUsingController = orCreateStateForPlayer.isUsingController;
			this._syncMessageToSend.controllerFocusPosition = new Vector2?(orCreateStateForPlayer.controllerFocusPosition);
			this._netService.SendMessage<PeerInputMessage>(this._syncMessageToSend);
			this._lastSyncMsec = this.GetTicksMsec();
			this._syncMessageToSend = null;
			this._syncMessageTask = null;
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x00221010 File Offset: 0x0021F210
		private PeerInputSynchronizer.PeerInputState GetStateForPlayer(ulong playerId)
		{
			int num = this._inputStates.FindIndex((PeerInputSynchronizer.PeerInputState s) => s.playerId == playerId);
			if (num >= 0)
			{
				return this._inputStates[num];
			}
			return null;
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x00221054 File Offset: 0x0021F254
		[NullableContext(1)]
		private PeerInputSynchronizer.PeerInputState GetOrCreateStateForPlayer(ulong playerId)
		{
			PeerInputSynchronizer.PeerInputState peerInputState = this.GetStateForPlayer(playerId);
			if (peerInputState == null)
			{
				peerInputState = new PeerInputSynchronizer.PeerInputState
				{
					playerId = playerId
				};
				this._inputStates.Add(peerInputState);
				Action<ulong> stateAdded = this.StateAdded;
				if (stateAdded != null)
				{
					stateAdded(playerId);
				}
			}
			return peerInputState;
		}

		/// <summary>
		/// Override the normalization calculation for the mouse cursor position.
		/// On screens like the Map screen, synchronizing cursor position based on screen position is not correct. This
		/// method allows you to override the translation from net position to screen position and vice versa.
		/// </summary>
		/// <param name="positionTranslator">The object to use when translating cursor positions.</param>
		// Token: 0x0600482E RID: 18478 RVA: 0x00221098 File Offset: 0x0021F298
		[NullableContext(1)]
		public void StartOverridingCursorPositioning(INetCursorPositionTranslator positionTranslator)
		{
			this._cursorTranslator = positionTranslator;
		}

		/// <summary>
		/// Stop overriding the normalization calculation for the mouse cursor position.
		/// Should be called after StartOverridingCursorPositioning when the screen is exited.
		/// </summary>
		// Token: 0x0600482F RID: 18479 RVA: 0x002210A1 File Offset: 0x0021F2A1
		public void StopOverridingCursorPositioning()
		{
			this._cursorTranslator = null;
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x002210AC File Offset: 0x0021F2AC
		[NullableContext(1)]
		private void HandlePeerInputMessage(PeerInputMessage message, ulong senderId)
		{
			PeerInputSynchronizer.PeerInputState orCreateStateForPlayer = this.GetOrCreateStateForPlayer(senderId);
			if (orCreateStateForPlayer.isMouseDown != message.mouseDown)
			{
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Mouse down state for ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
				defaultInterpolatedStringHandler.AppendLiteral(" changed: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(orCreateStateForPlayer.isMouseDown);
				defaultInterpolatedStringHandler.AppendLiteral("->");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(message.mouseDown);
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			if (orCreateStateForPlayer.netScreenType != message.screenType)
			{
				Logger logger2 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Remote screen for ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
				defaultInterpolatedStringHandler.AppendLiteral(" changed: ");
				defaultInterpolatedStringHandler.AppendFormatted<NetScreenType>(orCreateStateForPlayer.netScreenType);
				defaultInterpolatedStringHandler.AppendLiteral("->");
				defaultInterpolatedStringHandler.AppendFormatted<NetScreenType>(message.screenType);
				logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			if (orCreateStateForPlayer.isTargeting != message.isTargeting)
			{
				Logger logger3 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Targeting state for ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
				defaultInterpolatedStringHandler.AppendLiteral(" changed: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(orCreateStateForPlayer.isTargeting);
				defaultInterpolatedStringHandler.AppendLiteral("->");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(message.isTargeting);
				logger3.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			if (!orCreateStateForPlayer.hoveredModelData.Equals(message.hoveredModelData))
			{
				Logger logger4 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Hovered model for ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
				defaultInterpolatedStringHandler.AppendLiteral(" changed: ");
				defaultInterpolatedStringHandler.AppendFormatted<HoveredModelData>(orCreateStateForPlayer.hoveredModelData);
				defaultInterpolatedStringHandler.AppendLiteral("->");
				defaultInterpolatedStringHandler.AppendFormatted<HoveredModelData>(message.hoveredModelData);
				logger4.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			if (!orCreateStateForPlayer.controllerFocusPosition.Equals(message.controllerFocusPosition))
			{
				Logger logger5 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Controller focus position for ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
				defaultInterpolatedStringHandler.AppendLiteral(" changed: ");
				defaultInterpolatedStringHandler.AppendFormatted<Vector2>(orCreateStateForPlayer.controllerFocusPosition);
				defaultInterpolatedStringHandler.AppendLiteral("->");
				defaultInterpolatedStringHandler.AppendFormatted<Vector2?>(message.controllerFocusPosition);
				logger5.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			if (!orCreateStateForPlayer.isUsingController.Equals(message.isUsingController))
			{
				Logger logger6 = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Using controller state state for ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
				defaultInterpolatedStringHandler.AppendLiteral(" changed: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(orCreateStateForPlayer.isUsingController);
				defaultInterpolatedStringHandler.AppendLiteral("->");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(message.isUsingController);
				logger6.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
			NetScreenType netScreenType = orCreateStateForPlayer.netScreenType;
			orCreateStateForPlayer.netMousePosition = message.netMousePos ?? orCreateStateForPlayer.netMousePosition;
			orCreateStateForPlayer.isMouseDown = message.mouseDown;
			orCreateStateForPlayer.netScreenType = message.screenType;
			orCreateStateForPlayer.isTargeting = message.isTargeting;
			orCreateStateForPlayer.hoveredModelData = message.hoveredModelData;
			orCreateStateForPlayer.isUsingController = message.isUsingController;
			orCreateStateForPlayer.controllerFocusPosition = message.controllerFocusPosition ?? orCreateStateForPlayer.controllerFocusPosition;
			Action<ulong> stateChanged = this.StateChanged;
			if (stateChanged != null)
			{
				stateChanged(senderId);
			}
			if (netScreenType != orCreateStateForPlayer.netScreenType)
			{
				Action<ulong, NetScreenType> screenChanged = this.ScreenChanged;
				if (screenChanged == null)
				{
					return;
				}
				screenChanged(senderId, netScreenType);
			}
		}

		/// <summary>
		/// Returns the control-space position of a given player within the given control.
		/// If the player is using a mouse, then this would be the mouse's position
		/// If the player is using the controller, then this would be the position of the control node that that player is
		/// currently focused on.
		/// Different resolutions and aspect ratios are all mapped to the same 1920x1080 reference resolution in control-
		/// space. Essentially, the position returned by this method can be used set the position property on a Control with
		/// a cursor image as rootControl's child and have it look approximately the same across all peers.
		/// </summary>
		/// <param name="playerId">Player for which the mouse cursor position should be obtained.</param>
		/// <param name="rootControl">The root control to position the mouse cursor relative to. Should only be null in tests.</param>
		/// <returns></returns>
		// Token: 0x06004831 RID: 18481 RVA: 0x0022143C File Offset: 0x0021F63C
		public Vector2 GetControlSpaceFocusPosition(ulong playerId, Control rootControl)
		{
			PeerInputSynchronizer.PeerInputState orCreateStateForPlayer = this.GetOrCreateStateForPlayer(playerId);
			Vector2 vector = (orCreateStateForPlayer.isUsingController ? orCreateStateForPlayer.controllerFocusPosition : orCreateStateForPlayer.netMousePosition);
			if (this._cursorTranslator == null)
			{
				return NetCursorHelper.GetControlSpacePosition(vector, rootControl);
			}
			Vector2 screenPositionFromNetPosition = this._cursorTranslator.GetScreenPositionFromNetPosition(vector);
			if (rootControl != null)
			{
				return rootControl.GetGlobalTransformWithCanvas() * screenPositionFromNetPosition;
			}
			if (TestMode.IsOn)
			{
				return screenPositionFromNetPosition;
			}
			throw new InvalidOperationException("Root node should only be null in tests!");
		}

		/// <returns>True if the given player is holding the mouse down, false otherwise.</returns>
		// Token: 0x06004832 RID: 18482 RVA: 0x002214A8 File Offset: 0x0021F6A8
		public bool GetMouseDown(ulong playerId)
		{
			return this.GetOrCreateStateForPlayer(playerId).isMouseDown;
		}

		/// <returns>The top-most overlay or capstone that the given player is looking at.</returns>
		// Token: 0x06004833 RID: 18483 RVA: 0x002214B6 File Offset: 0x0021F6B6
		public NetScreenType GetScreenType(ulong playerId)
		{
			return this.GetOrCreateStateForPlayer(playerId).netScreenType;
		}

		/// <summary> WARNING: You probably want HoveredModelTracker methods instead! </summary>
		/// <returns>Info about what model the player is hovering.</returns>
		// Token: 0x06004834 RID: 18484 RVA: 0x002214C4 File Offset: 0x0021F6C4
		public HoveredModelData GetHoveredModelData(ulong playerId)
		{
			return this.GetOrCreateStateForPlayer(playerId).hoveredModelData;
		}

		/// <returns>True if the player is currently in combat targeting mode, false otherwise.</returns>
		// Token: 0x06004835 RID: 18485 RVA: 0x002214D2 File Offset: 0x0021F6D2
		public bool GetIsTargeting(ulong playerId)
		{
			return this.GetOrCreateStateForPlayer(playerId).isTargeting;
		}

		/// <summary>
		/// Removes the player state from the synchronizer if it exists and notifies listeners.
		/// Note that, if the player is still connected to the session and sends a PeerInputMessage, the state will be
		/// re-created, so this method should be called as late as possible in the disconnection flow.
		/// </summary>
		/// <param name="playerId">The player that left the game.</param>
		// Token: 0x06004836 RID: 18486 RVA: 0x002214E0 File Offset: 0x0021F6E0
		public void OnPlayerDisconnected(ulong playerId)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Disconnected player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(playerId);
			defaultInterpolatedStringHandler.AppendLiteral(", removing PeerInputState");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this._inputStates.RemoveAll((PeerInputSynchronizer.PeerInputState p) => p.playerId == playerId);
			Action<ulong> stateRemoved = this.StateRemoved;
			if (stateRemoved == null)
			{
				return;
			}
			stateRemoved(playerId);
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x0022156A File Offset: 0x0021F76A
		private ulong GetTicksMsec()
		{
			Func<ulong> func = this.mockGetTicksMsec;
			if (func == null)
			{
				return Time.GetTicksMsec();
			}
			return func();
		}

		// Token: 0x04001C5D RID: 7261
		public const int minUpdateMsec = 50;

		// Token: 0x04001C5E RID: 7262
		[Nullable(1)]
		private readonly INetGameService _netService;

		// Token: 0x04001C5F RID: 7263
		[Nullable(1)]
		private readonly List<PeerInputSynchronizer.PeerInputState> _inputStates = new List<PeerInputSynchronizer.PeerInputState>();

		// Token: 0x04001C60 RID: 7264
		private ulong _lastSyncMsec;

		// Token: 0x04001C61 RID: 7265
		private Task _syncMessageTask;

		// Token: 0x04001C62 RID: 7266
		private PeerInputMessage _syncMessageToSend;

		// Token: 0x04001C63 RID: 7267
		private INetCursorPositionTranslator _cursorTranslator;

		// Token: 0x04001C64 RID: 7268
		[Nullable(1)]
		private Logger _logger = new Logger("PeerInputSynchronizer", LogType.VisualSync);

		/// <summary>
		/// If this is unset, Time.GetTicksMsec is used to check how much time has passed.
		/// This is used to mock time passing for tests.
		/// </summary>
		// Token: 0x04001C69 RID: 7273
		public Func<ulong> mockGetTicksMsec;

		/// <summary>
		/// If this is unset, Task.Delay is used to buffer messages.
		/// This is used to mock time passing for tests.
		/// </summary>
		// Token: 0x04001C6A RID: 7274
		[Nullable(new byte[] { 2, 1 })]
		public Func<int, Task> mockDelay;

		/// <summary>
		/// If this is unset, Task.Yield is used to wait a moment before messages are sent.
		/// This is used to mock time passing for tests.
		/// </summary>
		// Token: 0x04001C6B RID: 7275
		[Nullable(new byte[] { 2, 1 })]
		public Func<Task> mockWaitSmall;

		// Token: 0x020019DE RID: 6622
		[NullableContext(0)]
		private class PeerInputState
		{
			/// <summary> The player to whom this state belongs. </summary>
			// Token: 0x0400661F RID: 26143
			public ulong playerId;

			/// <summary> The mouse position of the player. </summary>
			// Token: 0x04006620 RID: 26144
			public Vector2 netMousePosition;

			/// <summary> True if the player's mouse is held down, false otherwise. </summary>
			// Token: 0x04006621 RID: 26145
			public bool isMouseDown;

			/// <summary> Which screen this player is looking at. </summary>
			// Token: 0x04006622 RID: 26146
			public NetScreenType netScreenType;

			/// <summary> The combat model the player is hovering over. Should only be non-null in combat. </summary>
			// Token: 0x04006623 RID: 26147
			public HoveredModelData hoveredModelData;

			/// <summary> True if the player is in combat and is currently in targeting mode (drawing the targeting arrow),
			/// false otherwise. </summary>
			// Token: 0x04006624 RID: 26148
			public bool isTargeting;

			/// <summary>
			/// True if the player is currently using a controller.
			/// </summary>
			// Token: 0x04006625 RID: 26149
			public bool isUsingController;

			/// <summary>
			/// Tracks the position of the current focused control. Only valid if isUsingController is true.
			/// </summary>
			// Token: 0x04006626 RID: 26150
			public Vector2 controllerFocusPosition;
		}
	}
}
