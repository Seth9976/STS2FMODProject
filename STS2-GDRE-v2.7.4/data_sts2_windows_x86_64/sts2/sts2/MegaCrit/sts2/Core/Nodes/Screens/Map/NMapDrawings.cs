using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.MapDrawing;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	/// <summary>
	/// Takes care of drawing both local and remote players' map drawings.
	///
	/// Notes on the tech art side:
	/// Lines are placed into a subviewport which renders to a half-resolution texture. Drawn lines are simple colored Line2D
	/// nodes. Eraser lines are Line2D nodes with a shader with a subtractive blend mode. More details are in the shader itself.
	/// The subviewport texture is drawn on top of the map. Importantly, it is drawn using the premultiplied-alpha blend mode.
	/// Without this, black artifacts are seen on the edges of the lines because of the intermediate subviewport texture
	/// blending.
	/// </summary>
	// Token: 0x020002F3 RID: 755
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapDrawings.cs")]
	public class NMapDrawings : Control
	{
		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06002899 RID: 10393 RVA: 0x00116B30 File Offset: 0x00114D30
		private static IEnumerable<string> SelfAssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NMapDrawings._lineDrawScenePath,
					NMapDrawings._lineEraseScenePath,
					NMapDrawings._playerDrawingPath,
					"res://images/packed/common_ui/cursor_quill.png",
					"res://images/packed/common_ui/cursor_quill_tilted.png",
					"res://images/packed/common_ui/cursor_eraser.png",
					"res://images/packed/common_ui/cursor_eraser_tilted.png"
				});
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600289A RID: 10394 RVA: 0x00116B80 File Offset: 0x00114D80
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return NMapDrawings.SelfAssetPaths.Concat<string>(NMapDrawButton.AssetPaths);
			}
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x00116B94 File Offset: 0x00114D94
		public override void _Ready()
		{
			this._lineDrawScene = PreloadManager.Cache.GetScene(NMapDrawings._lineDrawScenePath);
			this._lineEraseScene = PreloadManager.Cache.GetScene(NMapDrawings._lineEraseScenePath);
			this._cursorManager = NGame.Instance.CursorManager;
			Line2D line2D = this._lineEraseScene.Instantiate<Line2D>(0L);
			this._eraserMaterial = line2D.Material;
			line2D.QueueFreeSafely();
			this._defaultSize = base.Size;
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x00116C08 File Offset: 0x00114E08
		public void Initialize(INetGameService netService, IPlayerCollection playerCollection, PeerInputSynchronizer inputSynchronizer)
		{
			this._netService = netService;
			this._playerCollection = playerCollection;
			this._inputSynchronizer = inputSynchronizer;
			this._netService.RegisterMessageHandler<MapDrawingMessage>(new MessageHandlerDelegate<MapDrawingMessage>(this.HandleDrawingMessage));
			this._netService.RegisterMessageHandler<ClearMapDrawingsMessage>(new MessageHandlerDelegate<ClearMapDrawingsMessage>(this.HandleClearMapDrawingsMessage));
			this._netService.RegisterMessageHandler<MapDrawingModeChangedMessage>(new MessageHandlerDelegate<MapDrawingModeChangedMessage>(this.HandleMapDrawingModeChangedMessage));
			inputSynchronizer.ScreenChanged += this.OnPlayerScreenChanged;
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x00116C84 File Offset: 0x00114E84
		public override void _ExitTree()
		{
			this._netService.UnregisterMessageHandler<MapDrawingMessage>(new MessageHandlerDelegate<MapDrawingMessage>(this.HandleDrawingMessage));
			this._netService.UnregisterMessageHandler<ClearMapDrawingsMessage>(new MessageHandlerDelegate<ClearMapDrawingsMessage>(this.HandleClearMapDrawingsMessage));
			this._netService.UnregisterMessageHandler<MapDrawingModeChangedMessage>(new MessageHandlerDelegate<MapDrawingModeChangedMessage>(this.HandleMapDrawingModeChangedMessage));
			this._inputSynchronizer.ScreenChanged -= this.OnPlayerScreenChanged;
		}

		/// <summary>
		/// Called when the local player begins drawing a line.
		/// </summary>
		/// <param name="position">The position at which the player started drawing.</param>
		/// <param name="overrideDrawingMode">The mode to override the current mode with. If null, the current mode in the
		/// set using SetDrawingMode is used. If it is null and the current mode is None, an exception is thrown.</param>
		// Token: 0x0600289E RID: 10398 RVA: 0x00116CF0 File Offset: 0x00114EF0
		public void BeginLineLocal(Vector2 position, DrawingMode? overrideDrawingMode)
		{
			this.BeginLine(this.GetDrawingStateForPlayer(this._netService.NetId), position, overrideDrawingMode);
			NetMapDrawingEvent netMapDrawingEvent = new NetMapDrawingEvent
			{
				type = MapDrawingEventType.BeginLine,
				position = this.ToNetPosition(position),
				overrideDrawingMode = overrideDrawingMode
			};
			this.QueueOrSendEvent(netMapDrawingEvent);
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x00116D48 File Offset: 0x00114F48
		public void UpdateCurrentLinePositionLocal(Vector2 position)
		{
			NMapDrawings.DrawingState drawingStateForPlayer = this.GetDrawingStateForPlayer(this._netService.NetId);
			this.UpdateCurrentLinePosition(drawingStateForPlayer, position);
			NetMapDrawingEvent netMapDrawingEvent = new NetMapDrawingEvent
			{
				type = MapDrawingEventType.ContinueLine,
				position = this.ToNetPosition(position),
				overrideDrawingMode = drawingStateForPlayer.overrideDrawingMode
			};
			this.QueueOrSendEvent(netMapDrawingEvent);
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x00116DA4 File Offset: 0x00114FA4
		public void StopLineLocal()
		{
			this.StopDrawingLine(this.GetDrawingStateForPlayer(this._netService.NetId));
			NetMapDrawingEvent netMapDrawingEvent = new NetMapDrawingEvent
			{
				type = MapDrawingEventType.EndLine
			};
			this.QueueOrSendEvent(netMapDrawingEvent);
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x00116DE4 File Offset: 0x00114FE4
		public void SetDrawingModeLocal(DrawingMode drawingMode)
		{
			this.SetDrawingMode(this.GetDrawingStateForPlayer(this._netService.NetId), drawingMode);
			MapDrawingModeChangedMessage mapDrawingModeChangedMessage = new MapDrawingModeChangedMessage
			{
				drawingMode = drawingMode
			};
			this._netService.SendMessage<MapDrawingModeChangedMessage>(mapDrawingModeChangedMessage);
			this.UpdateLocalCursor();
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x00116E30 File Offset: 0x00115030
		public void ClearDrawnLinesLocal()
		{
			this.ClearAllLinesForPlayer(this.GetDrawingStateForPlayer(this._netService.NetId));
			this.UpdateLocalCursor();
			this._netService.SendMessage<ClearMapDrawingsMessage>(default(ClearMapDrawingsMessage));
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x00116E6E File Offset: 0x0011506E
		public bool IsDrawing(ulong playerId)
		{
			return this.GetDrawingStateForPlayer(playerId).IsDrawing;
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x00116E7C File Offset: 0x0011507C
		public bool IsLocalDrawing()
		{
			return this.GetDrawingStateForPlayer(this._netService.NetId).IsDrawing;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x00116E94 File Offset: 0x00115094
		public DrawingMode GetDrawingMode(ulong playerId)
		{
			return this.GetDrawingStateForPlayer(playerId).CurrentDrawingMode;
		}

		/// <param name="useOverride">In most cases, you want this to be true. If false, this returns DrawingMode.None if the
		/// player is drawing using a shortcut (right click or middle click).</param>
		/// <returns>The kind of drawing that the player is currently doing.</returns>
		// Token: 0x060028A6 RID: 10406 RVA: 0x00116EA2 File Offset: 0x001150A2
		public DrawingMode GetLocalDrawingMode(bool useOverride = true)
		{
			if (!useOverride)
			{
				return this.GetDrawingStateForPlayer(this._netService.NetId).drawingMode;
			}
			return this.GetDrawingStateForPlayer(this._netService.NetId).CurrentDrawingMode;
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x00116ED4 File Offset: 0x001150D4
		private void QueueOrSendEvent(NetMapDrawingEvent ev)
		{
			if (this._queuedMessage == null)
			{
				this._queuedMessage = new MapDrawingMessage();
			}
			if (!this._queuedMessage.TryAddEvent(ev))
			{
				this._queuedMessage.drawingMode = new DrawingMode?(this.GetDrawingStateForPlayer(this._netService.NetId).drawingMode);
				this._netService.SendMessage<MapDrawingMessage>(this._queuedMessage);
				this._queuedMessage = new MapDrawingMessage();
				if (!this._queuedMessage.TryAddEvent(ev))
				{
					throw new InvalidOperationException();
				}
			}
			this.TrySendSyncMessage();
			this.UpdateLocalCursor();
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x00116F66 File Offset: 0x00115166
		private Vector2 ToNetPosition(Vector2 pos)
		{
			pos.X -= base.Size.X * 0.5f;
			pos /= new Vector2(960f, base.Size.Y);
			return pos;
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x00116FA2 File Offset: 0x001151A2
		private Vector2 FromNetPosition(Vector2 pos)
		{
			pos *= new Vector2(960f, base.Size.Y);
			pos.X += base.Size.X * 0.5f;
			return pos;
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x00116FE0 File Offset: 0x001151E0
		private void HandleDrawingMessage(MapDrawingMessage message, ulong senderId)
		{
			NMapDrawings.DrawingState drawingStateForPlayer = this.GetDrawingStateForPlayer(senderId);
			foreach (NetMapDrawingEvent netMapDrawingEvent in message.Events)
			{
				if (netMapDrawingEvent.type == MapDrawingEventType.BeginLine)
				{
					if (this.GetDrawingMode(senderId) != DrawingMode.None)
					{
						this.StopDrawingLine(drawingStateForPlayer);
					}
					this.BeginLine(drawingStateForPlayer, this.FromNetPosition(netMapDrawingEvent.position), netMapDrawingEvent.overrideDrawingMode);
				}
				else if (netMapDrawingEvent.type == MapDrawingEventType.ContinueLine)
				{
					if (!drawingStateForPlayer.IsDrawing)
					{
						if (message.drawingMode != null)
						{
							DrawingMode drawingMode = drawingStateForPlayer.drawingMode;
							DrawingMode? drawingMode2 = message.drawingMode;
							if (!((drawingMode == drawingMode2.GetValueOrDefault()) & (drawingMode2 != null)))
							{
								this.SetDrawingMode(drawingStateForPlayer, message.drawingMode.Value);
							}
						}
						this.BeginLine(drawingStateForPlayer, this.FromNetPosition(netMapDrawingEvent.position), netMapDrawingEvent.overrideDrawingMode);
					}
					this.UpdateCurrentLinePosition(drawingStateForPlayer, this.FromNetPosition(netMapDrawingEvent.position));
				}
				else
				{
					this.StopDrawingLine(drawingStateForPlayer);
				}
			}
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x001170F0 File Offset: 0x001152F0
		private void HandleClearMapDrawingsMessage(ClearMapDrawingsMessage message, ulong senderId)
		{
			this.ClearAllLinesForPlayer(this.GetDrawingStateForPlayer(senderId));
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x001170FF File Offset: 0x001152FF
		private void HandleMapDrawingModeChangedMessage(MapDrawingModeChangedMessage message, ulong senderId)
		{
			this.SetDrawingMode(this.GetDrawingStateForPlayer(senderId), message.drawingMode);
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x00117114 File Offset: 0x00115314
		private void BeginLine(NMapDrawings.DrawingState state, Vector2 position, DrawingMode? overrideDrawingMode)
		{
			Player player = this._playerCollection.GetPlayer(state.playerId);
			DrawingMode drawingMode = overrideDrawingMode ?? state.drawingMode;
			if (drawingMode == DrawingMode.None)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(state.playerId);
				defaultInterpolatedStringHandler.AppendLiteral(" is not currently in a drawing mode and no override was passed!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			state.overrideDrawingMode = overrideDrawingMode;
			state.currentlyDrawingLine = this.CreateLineForPlayer(player, drawingMode == DrawingMode.Erasing);
			state.currentlyDrawingLine.AddPoint(position * 0.5f, -1);
			state.currentlyDrawingLine.AddPoint(position * 0.5f + new Vector2(0f, 0.5f), -1);
			state.drawViewport.AddChildSafely(state.currentlyDrawingLine);
			NGame.Instance.RemoteCursorContainer.DrawingCursorStateChanged(state.playerId);
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x00117210 File Offset: 0x00115410
		private Line2D CreateLineForPlayer(Player player, bool isErasing)
		{
			PackedScene packedScene = (isErasing ? this._lineEraseScene : this._lineDrawScene);
			Line2D line2D = packedScene.Instantiate<Line2D>(0L);
			line2D.DefaultColor = player.Character.MapDrawingColor;
			line2D.ClearPoints();
			line2D.Position = Vector2.Zero;
			return line2D;
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x0011725B File Offset: 0x0011545B
		private void StopDrawingLine(NMapDrawings.DrawingState state)
		{
			state.overrideDrawingMode = null;
			state.currentlyDrawingLine = null;
			NGame.Instance.RemoteCursorContainer.DrawingCursorStateChanged(state.playerId);
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x00117285 File Offset: 0x00115485
		private void SetDrawingMode(NMapDrawings.DrawingState state, DrawingMode drawingMode)
		{
			if (state.drawingMode != drawingMode)
			{
				state.drawingMode = drawingMode;
				NGame.Instance.RemoteCursorContainer.DrawingCursorStateChanged(state.playerId);
			}
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x001172AC File Offset: 0x001154AC
		private void UpdateCurrentLinePosition(NMapDrawings.DrawingState state, Vector2 position)
		{
			if (state.currentlyDrawingLine == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(93, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to update current line position for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(state.playerId);
				defaultInterpolatedStringHandler.AppendLiteral(", but they are not currently drawing a line!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Vector2[] points = state.currentlyDrawingLine.Points;
			Vector2 vector = points[points.Length - 1];
			if (vector.DistanceSquaredTo(position) < 4f)
			{
				return;
			}
			state.currentlyDrawingLine.AddPoint(position * 0.5f, -1);
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x0011733C File Offset: 0x0011553C
		private NMapDrawings.DrawingState GetDrawingStateForPlayer(ulong playerId)
		{
			NMapDrawings.DrawingState drawingState = this._drawingStates.FirstOrDefault<NMapDrawings.DrawingState>((NMapDrawings.DrawingState s) => s.playerId == playerId);
			if (drawingState == null)
			{
				Control control = PreloadManager.Cache.GetScene(NMapDrawings._playerDrawingPath).Instantiate<Control>(0L);
				this.AddChildSafely(control);
				drawingState = new NMapDrawings.DrawingState
				{
					playerId = playerId,
					drawViewport = control.GetNode<SubViewport>("DrawViewport"),
					drawingTexture = control.GetNode<TextureRect>("DrawViewportTextureRect")
				};
				TaskHelper.RunSafely(this.SetVisibleLater(drawingState));
				this._drawingStates.Add(drawingState);
			}
			return drawingState;
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x001173E8 File Offset: 0x001155E8
		private async Task SetVisibleLater(NMapDrawings.DrawingState state)
		{
			state.drawViewport.RenderTargetUpdateMode = 4L;
			state.drawingTexture.Visible = false;
			await this.AwaitProcessFrame(default(CancellationToken));
			await this.AwaitProcessFrame(default(CancellationToken));
			await this.AwaitProcessFrame(default(CancellationToken));
			state.drawingTexture.Visible = this.ShouldShowMapDrawing(state);
			state.drawViewport.RenderTargetUpdateMode = 2L;
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x00117434 File Offset: 0x00115634
		public void UpdateVisibilityFromSettings()
		{
			foreach (NMapDrawings.DrawingState drawingState in this._drawingStates)
			{
				drawingState.drawingTexture.Visible = this.ShouldShowMapDrawing(drawingState);
			}
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x00117494 File Offset: 0x00115694
		private bool ShouldShowMapDrawing(NMapDrawings.DrawingState state)
		{
			if (!SaveManager.Instance.PrefsSave.ShowMultiplayerDrawings)
			{
				ulong? netId = LocalContext.NetId;
				ulong playerId = state.playerId;
				return (netId.GetValueOrDefault() == playerId) & (netId != null);
			}
			return true;
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x001174D4 File Offset: 0x001156D4
		public void ClearAllLines()
		{
			foreach (NMapDrawings.DrawingState drawingState in this._drawingStates)
			{
				foreach (Line2D line2D in drawingState.drawViewport.GetChildren(false).OfType<Line2D>())
				{
					line2D.QueueFreeSafely();
				}
			}
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x00117568 File Offset: 0x00115768
		public SerializableMapDrawings GetSerializableMapDrawings()
		{
			SerializableMapDrawings serializableMapDrawings = new SerializableMapDrawings();
			foreach (NMapDrawings.DrawingState drawingState in this._drawingStates)
			{
				SerializablePlayerMapDrawings serializablePlayerMapDrawings = new SerializablePlayerMapDrawings
				{
					playerId = drawingState.playerId
				};
				serializableMapDrawings.drawings.Add(serializablePlayerMapDrawings);
				foreach (Line2D line2D in drawingState.drawViewport.GetChildren(false).OfType<Line2D>())
				{
					SerializableMapDrawingLine serializableMapDrawingLine = new SerializableMapDrawingLine
					{
						mapPoints = new List<Vector2>()
					};
					serializableMapDrawingLine.isEraser = line2D.Material == this._eraserMaterial;
					serializablePlayerMapDrawings.lines.Add(serializableMapDrawingLine);
					foreach (Vector2 vector in line2D.Points)
					{
						serializableMapDrawingLine.mapPoints.Add(this.ToNetPosition(vector));
					}
				}
			}
			return serializableMapDrawings;
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x00117698 File Offset: 0x00115898
		public void LoadDrawings(SerializableMapDrawings drawings)
		{
			foreach (SerializablePlayerMapDrawings serializablePlayerMapDrawings in drawings.drawings)
			{
				Player player = this._playerCollection.GetPlayer(serializablePlayerMapDrawings.playerId);
				if (player == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(serializablePlayerMapDrawings.playerId);
					defaultInterpolatedStringHandler.AppendLiteral(" has map drawings, but doesn't exist in the run!");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				else
				{
					NMapDrawings.DrawingState drawingStateForPlayer = this.GetDrawingStateForPlayer(serializablePlayerMapDrawings.playerId);
					foreach (SerializableMapDrawingLine serializableMapDrawingLine in serializablePlayerMapDrawings.lines)
					{
						Line2D line2D = this.CreateLineForPlayer(player, serializableMapDrawingLine.isEraser);
						drawingStateForPlayer.drawViewport.AddChildSafely(line2D);
						foreach (Vector2 vector in serializableMapDrawingLine.mapPoints)
						{
							line2D.AddPoint(this.FromNetPosition(vector), -1);
						}
					}
				}
			}
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x0011781C File Offset: 0x00115A1C
		private void ClearAllLinesForPlayer(NMapDrawings.DrawingState state)
		{
			foreach (Line2D line2D in state.drawViewport.GetChildren(false).OfType<Line2D>())
			{
				line2D.QueueFreeSafely();
			}
			this.SetDrawingMode(state, DrawingMode.None);
		}

		/// <summary>
		/// Called when a remote player changes which screen they're looking at.
		/// If the player was looking at the map and drawing a line, then this will revert their cursor back to the default
		/// one. It's necessary because drawing packets are unreliable, so the end packet may not be received.
		/// </summary>
		/// <param name="playerId">The player whose screen changed.</param>
		/// <param name="oldScreenType">The screen the player was looking at.</param>
		// Token: 0x060028BA RID: 10426 RVA: 0x0011787C File Offset: 0x00115A7C
		private void OnPlayerScreenChanged(ulong playerId, NetScreenType oldScreenType)
		{
			if (playerId == this._netService.NetId)
			{
				return;
			}
			NetScreenType screenType = this._inputSynchronizer.GetScreenType(playerId);
			if (oldScreenType == NetScreenType.Map && screenType != NetScreenType.Map)
			{
				NMapDrawings.DrawingState drawingStateForPlayer = this.GetDrawingStateForPlayer(playerId);
				if (drawingStateForPlayer.IsDrawing)
				{
					this.StopDrawingLine(drawingStateForPlayer);
				}
				if (drawingStateForPlayer.drawingMode != DrawingMode.None)
				{
					this.SetDrawingMode(drawingStateForPlayer, DrawingMode.None);
				}
			}
		}

		/// <summary>
		/// Sends a sync message if enough time has passed since the last one, or buffers it to be sent otherwise.
		/// </summary>
		// Token: 0x060028BB RID: 10427 RVA: 0x001178D4 File Offset: 0x00115AD4
		private void TrySendSyncMessage()
		{
			if (this._sendMessageTask != null)
			{
				return;
			}
			if (!this._netService.IsConnected)
			{
				return;
			}
			int num = (int)(this._lastMessageMsec + 50UL - Time.GetTicksMsec());
			if (num <= 0)
			{
				this._sendMessageTask = TaskHelper.RunSafely(this.SendSyncMessageAfterSmallDelay());
				return;
			}
			this._sendMessageTask = TaskHelper.RunSafely(this.QueueSyncMessage(num));
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x00117934 File Offset: 0x00115B34
		private async Task QueueSyncMessage(int delayMsec)
		{
			await Task.Delay(delayMsec);
			this.SendSyncMessage();
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x00117980 File Offset: 0x00115B80
		private async Task SendSyncMessageAfterSmallDelay()
		{
			await Task.Yield();
			this.SendSyncMessage();
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x001179C4 File Offset: 0x00115BC4
		private void SendSyncMessage()
		{
			if (!this._netService.IsConnected)
			{
				return;
			}
			this._queuedMessage.drawingMode = new DrawingMode?(this.GetDrawingStateForPlayer(this._netService.NetId).drawingMode);
			this._netService.SendMessage<MapDrawingMessage>(this._queuedMessage);
			this._lastMessageMsec = Time.GetTicksMsec();
			this._queuedMessage = null;
			this._sendMessageTask = null;
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x00117A30 File Offset: 0x00115C30
		private void UpdateLocalCursor()
		{
			NMapDrawings.DrawingState drawingStateForPlayer = this.GetDrawingStateForPlayer(this._netService.NetId);
			if (drawingStateForPlayer.CurrentDrawingMode == DrawingMode.Drawing)
			{
				Image asset = PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_quill.png");
				Image asset2 = PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_quill_tilted.png");
				this._cursorManager.OverrideCursor(asset2, asset, NMapDrawings.drawingCursorHotspot);
				return;
			}
			if (drawingStateForPlayer.CurrentDrawingMode == DrawingMode.Erasing)
			{
				Image asset3 = PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_eraser.png");
				Image asset4 = PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_eraser_tilted.png");
				this._cursorManager.OverrideCursor(asset4, asset3, NMapDrawings.erasingCursorHotspot);
				return;
			}
			this._cursorManager.StopOverridingCursor();
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x00117AD4 File Offset: 0x00115CD4
		public void RepositionBasedOnBackground(Control mapBg)
		{
			base.Position = new Vector2(mapBg.Position.X + (mapBg.Size.X - base.Size.X) * 0.5f, mapBg.Position.Y);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060028C1 RID: 10433 RVA: 0x00117B20 File Offset: 0x00115D20
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(19)
			{
				new MethodInfo(NMapDrawings.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawings.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawings.MethodName.UpdateCurrentLinePositionLocal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapDrawings.MethodName.StopLineLocal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawings.MethodName.SetDrawingModeLocal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "drawingMode", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapDrawings.MethodName.ClearDrawnLinesLocal, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawings.MethodName.IsDrawing, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapDrawings.MethodName.IsLocalDrawing, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawings.MethodName.GetDrawingMode, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapDrawings.MethodName.GetLocalDrawingMode, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "useOverride", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapDrawings.MethodName.ToNetPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "pos", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapDrawings.MethodName.FromNetPosition, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "pos", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapDrawings.MethodName.UpdateVisibilityFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawings.MethodName.ClearAllLines, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawings.MethodName.OnPlayerScreenChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false),
					new PropertyInfo(2L, "oldScreenType", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapDrawings.MethodName.TrySendSyncMessage, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawings.MethodName.SendSyncMessage, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawings.MethodName.UpdateLocalCursor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapDrawings.MethodName.RepositionBasedOnBackground, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "mapBg", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060028C2 RID: 10434 RVA: 0x0011802C File Offset: 0x0011622C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapDrawings.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.UpdateCurrentLinePositionLocal && args.Count == 1)
			{
				this.UpdateCurrentLinePositionLocal(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.StopLineLocal && args.Count == 0)
			{
				this.StopLineLocal();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.SetDrawingModeLocal && args.Count == 1)
			{
				this.SetDrawingModeLocal(VariantUtils.ConvertTo<DrawingMode>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.ClearDrawnLinesLocal && args.Count == 0)
			{
				this.ClearDrawnLinesLocal();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.IsDrawing && args.Count == 1)
			{
				bool flag = this.IsDrawing(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.IsLocalDrawing && args.Count == 0)
			{
				bool flag2 = this.IsLocalDrawing();
				ret = VariantUtils.CreateFrom<bool>(ref flag2);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.GetDrawingMode && args.Count == 1)
			{
				DrawingMode drawingMode = this.GetDrawingMode(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<DrawingMode>(ref drawingMode);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.GetLocalDrawingMode && args.Count == 1)
			{
				DrawingMode localDrawingMode = this.GetLocalDrawingMode(VariantUtils.ConvertTo<bool>(args[0]));
				ret = VariantUtils.CreateFrom<DrawingMode>(ref localDrawingMode);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.ToNetPosition && args.Count == 1)
			{
				Vector2 vector = this.ToNetPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.FromNetPosition && args.Count == 1)
			{
				Vector2 vector2 = this.FromNetPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<Vector2>(ref vector2);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.UpdateVisibilityFromSettings && args.Count == 0)
			{
				this.UpdateVisibilityFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.ClearAllLines && args.Count == 0)
			{
				this.ClearAllLines();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.OnPlayerScreenChanged && args.Count == 2)
			{
				this.OnPlayerScreenChanged(VariantUtils.ConvertTo<ulong>(args[0]), VariantUtils.ConvertTo<NetScreenType>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.TrySendSyncMessage && args.Count == 0)
			{
				this.TrySendSyncMessage();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.SendSyncMessage && args.Count == 0)
			{
				this.SendSyncMessage();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.UpdateLocalCursor && args.Count == 0)
			{
				this.UpdateLocalCursor();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapDrawings.MethodName.RepositionBasedOnBackground && args.Count == 1)
			{
				this.RepositionBasedOnBackground(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060028C3 RID: 10435 RVA: 0x001183B8 File Offset: 0x001165B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapDrawings.MethodName._Ready || (ref method) == NMapDrawings.MethodName._ExitTree || (ref method) == NMapDrawings.MethodName.UpdateCurrentLinePositionLocal || (ref method) == NMapDrawings.MethodName.StopLineLocal || (ref method) == NMapDrawings.MethodName.SetDrawingModeLocal || (ref method) == NMapDrawings.MethodName.ClearDrawnLinesLocal || (ref method) == NMapDrawings.MethodName.IsDrawing || (ref method) == NMapDrawings.MethodName.IsLocalDrawing || (ref method) == NMapDrawings.MethodName.GetDrawingMode || (ref method) == NMapDrawings.MethodName.GetLocalDrawingMode || (ref method) == NMapDrawings.MethodName.ToNetPosition || (ref method) == NMapDrawings.MethodName.FromNetPosition || (ref method) == NMapDrawings.MethodName.UpdateVisibilityFromSettings || (ref method) == NMapDrawings.MethodName.ClearAllLines || (ref method) == NMapDrawings.MethodName.OnPlayerScreenChanged || (ref method) == NMapDrawings.MethodName.TrySendSyncMessage || (ref method) == NMapDrawings.MethodName.SendSyncMessage || (ref method) == NMapDrawings.MethodName.UpdateLocalCursor || (ref method) == NMapDrawings.MethodName.RepositionBasedOnBackground || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060028C4 RID: 10436 RVA: 0x001184EC File Offset: 0x001166EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapDrawings.PropertyName._lineDrawScene)
			{
				this._lineDrawScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NMapDrawings.PropertyName._lineEraseScene)
			{
				this._lineEraseScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NMapDrawings.PropertyName._cursorManager)
			{
				this._cursorManager = VariantUtils.ConvertTo<NCursorManager>(ref value);
				return true;
			}
			if ((ref name) == NMapDrawings.PropertyName._eraserMaterial)
			{
				this._eraserMaterial = VariantUtils.ConvertTo<Material>(ref value);
				return true;
			}
			if ((ref name) == NMapDrawings.PropertyName._defaultSize)
			{
				this._defaultSize = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMapDrawings.PropertyName._lastMessageMsec)
			{
				this._lastMessageMsec = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060028C5 RID: 10437 RVA: 0x001185A4 File Offset: 0x001167A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapDrawings.PropertyName._lineDrawScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._lineDrawScene);
				return true;
			}
			if ((ref name) == NMapDrawings.PropertyName._lineEraseScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._lineEraseScene);
				return true;
			}
			if ((ref name) == NMapDrawings.PropertyName._cursorManager)
			{
				value = VariantUtils.CreateFrom<NCursorManager>(ref this._cursorManager);
				return true;
			}
			if ((ref name) == NMapDrawings.PropertyName._eraserMaterial)
			{
				value = VariantUtils.CreateFrom<Material>(ref this._eraserMaterial);
				return true;
			}
			if ((ref name) == NMapDrawings.PropertyName._defaultSize)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._defaultSize);
				return true;
			}
			if ((ref name) == NMapDrawings.PropertyName._lastMessageMsec)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._lastMessageMsec);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060028C6 RID: 10438 RVA: 0x0011867C File Offset: 0x0011687C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapDrawings.PropertyName._lineDrawScene, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapDrawings.PropertyName._lineEraseScene, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapDrawings.PropertyName._cursorManager, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapDrawings.PropertyName._eraserMaterial, 0L, "", 4096L, false),
				new PropertyInfo(5L, NMapDrawings.PropertyName._defaultSize, 0L, "", 4096L, false),
				new PropertyInfo(2L, NMapDrawings.PropertyName._lastMessageMsec, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060028C7 RID: 10439 RVA: 0x00118754 File Offset: 0x00116954
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapDrawings.PropertyName._lineDrawScene, Variant.From<PackedScene>(ref this._lineDrawScene));
			info.AddProperty(NMapDrawings.PropertyName._lineEraseScene, Variant.From<PackedScene>(ref this._lineEraseScene));
			info.AddProperty(NMapDrawings.PropertyName._cursorManager, Variant.From<NCursorManager>(ref this._cursorManager));
			info.AddProperty(NMapDrawings.PropertyName._eraserMaterial, Variant.From<Material>(ref this._eraserMaterial));
			info.AddProperty(NMapDrawings.PropertyName._defaultSize, Variant.From<Vector2>(ref this._defaultSize));
			info.AddProperty(NMapDrawings.PropertyName._lastMessageMsec, Variant.From<ulong>(ref this._lastMessageMsec));
		}

		/// <inheritdoc />
		// Token: 0x060028C8 RID: 10440 RVA: 0x001187EC File Offset: 0x001169EC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapDrawings.PropertyName._lineDrawScene, ref variant))
			{
				this._lineDrawScene = variant.As<PackedScene>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapDrawings.PropertyName._lineEraseScene, ref variant2))
			{
				this._lineEraseScene = variant2.As<PackedScene>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapDrawings.PropertyName._cursorManager, ref variant3))
			{
				this._cursorManager = variant3.As<NCursorManager>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMapDrawings.PropertyName._eraserMaterial, ref variant4))
			{
				this._eraserMaterial = variant4.As<Material>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMapDrawings.PropertyName._defaultSize, ref variant5))
			{
				this._defaultSize = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMapDrawings.PropertyName._lastMessageMsec, ref variant6))
			{
				this._lastMessageMsec = variant6.As<ulong>();
			}
		}

		// Token: 0x04000F21 RID: 3873
		private const int _minUpdateMsec = 50;

		// Token: 0x04000F22 RID: 3874
		private static readonly string _lineDrawScenePath = SceneHelper.GetScenePath("screens/map/map_line_draw");

		// Token: 0x04000F23 RID: 3875
		private static readonly string _lineEraseScenePath = SceneHelper.GetScenePath("screens/map/map_line_erase");

		// Token: 0x04000F24 RID: 3876
		private static readonly string _playerDrawingPath = SceneHelper.GetScenePath("screens/map/map_drawing");

		// Token: 0x04000F25 RID: 3877
		public const string drawingCursorPath = "res://images/packed/common_ui/cursor_quill.png";

		// Token: 0x04000F26 RID: 3878
		public const string drawingCursorTiltedPath = "res://images/packed/common_ui/cursor_quill_tilted.png";

		// Token: 0x04000F27 RID: 3879
		public static readonly Vector2 drawingCursorHotspot = new Vector2(2f, 56f);

		// Token: 0x04000F28 RID: 3880
		public const string erasingCursorPath = "res://images/packed/common_ui/cursor_eraser.png";

		// Token: 0x04000F29 RID: 3881
		public const string erasingCursorTiltedPath = "res://images/packed/common_ui/cursor_eraser_tilted.png";

		// Token: 0x04000F2A RID: 3882
		public static readonly Vector2 erasingCursorHotspot = new Vector2(24f, 58f);

		// Token: 0x04000F2B RID: 3883
		private const float _minimumPointDistance = 2f;

		// Token: 0x04000F2C RID: 3884
		private INetGameService _netService;

		// Token: 0x04000F2D RID: 3885
		private IPlayerCollection _playerCollection;

		// Token: 0x04000F2E RID: 3886
		private PeerInputSynchronizer _inputSynchronizer;

		// Token: 0x04000F2F RID: 3887
		private PackedScene _lineDrawScene;

		// Token: 0x04000F30 RID: 3888
		private PackedScene _lineEraseScene;

		// Token: 0x04000F31 RID: 3889
		private NCursorManager _cursorManager;

		// Token: 0x04000F32 RID: 3890
		private Material _eraserMaterial;

		// Token: 0x04000F33 RID: 3891
		private Vector2 _defaultSize;

		// Token: 0x04000F34 RID: 3892
		private readonly List<NMapDrawings.DrawingState> _drawingStates = new List<NMapDrawings.DrawingState>();

		// Token: 0x04000F35 RID: 3893
		[Nullable(2)]
		private MapDrawingMessage _queuedMessage;

		// Token: 0x04000F36 RID: 3894
		private ulong _lastMessageMsec;

		// Token: 0x04000F37 RID: 3895
		[Nullable(2)]
		private Task _sendMessageTask;

		// Token: 0x020014AB RID: 5291
		[Nullable(0)]
		[RequiredMember]
		private class DrawingState
		{
			// Token: 0x170026B3 RID: 9907
			// (get) Token: 0x0600A13C RID: 41276 RVA: 0x00331189 File Offset: 0x0032F389
			public bool IsDrawing
			{
				get
				{
					return this.currentlyDrawingLine != null;
				}
			}

			// Token: 0x170026B4 RID: 9908
			// (get) Token: 0x0600A13D RID: 41277 RVA: 0x00331194 File Offset: 0x0032F394
			public DrawingMode CurrentDrawingMode
			{
				get
				{
					DrawingMode? drawingMode = this.overrideDrawingMode;
					if (drawingMode == null)
					{
						return this.drawingMode;
					}
					return drawingMode.GetValueOrDefault();
				}
			}

			// Token: 0x0600A13E RID: 41278 RVA: 0x003311BF File Offset: 0x0032F3BF
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public DrawingState()
			{
			}

			// Token: 0x04004E60 RID: 20064
			public DrawingMode? overrideDrawingMode;

			// Token: 0x04004E61 RID: 20065
			public DrawingMode drawingMode;

			// Token: 0x04004E62 RID: 20066
			public ulong playerId;

			// Token: 0x04004E63 RID: 20067
			[Nullable(2)]
			public Line2D currentlyDrawingLine;

			// Token: 0x04004E64 RID: 20068
			[RequiredMember]
			public SubViewport drawViewport;

			// Token: 0x04004E65 RID: 20069
			[RequiredMember]
			public TextureRect drawingTexture;
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014AC RID: 5292
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004E66 RID: 20070
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004E67 RID: 20071
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdateCurrentLinePositionLocal' method.
			/// </summary>
			// Token: 0x04004E68 RID: 20072
			public static readonly StringName UpdateCurrentLinePositionLocal = "UpdateCurrentLinePositionLocal";

			/// <summary>
			/// Cached name for the 'StopLineLocal' method.
			/// </summary>
			// Token: 0x04004E69 RID: 20073
			public static readonly StringName StopLineLocal = "StopLineLocal";

			/// <summary>
			/// Cached name for the 'SetDrawingModeLocal' method.
			/// </summary>
			// Token: 0x04004E6A RID: 20074
			public static readonly StringName SetDrawingModeLocal = "SetDrawingModeLocal";

			/// <summary>
			/// Cached name for the 'ClearDrawnLinesLocal' method.
			/// </summary>
			// Token: 0x04004E6B RID: 20075
			public static readonly StringName ClearDrawnLinesLocal = "ClearDrawnLinesLocal";

			/// <summary>
			/// Cached name for the 'IsDrawing' method.
			/// </summary>
			// Token: 0x04004E6C RID: 20076
			public static readonly StringName IsDrawing = "IsDrawing";

			/// <summary>
			/// Cached name for the 'IsLocalDrawing' method.
			/// </summary>
			// Token: 0x04004E6D RID: 20077
			public static readonly StringName IsLocalDrawing = "IsLocalDrawing";

			/// <summary>
			/// Cached name for the 'GetDrawingMode' method.
			/// </summary>
			// Token: 0x04004E6E RID: 20078
			public static readonly StringName GetDrawingMode = "GetDrawingMode";

			/// <summary>
			/// Cached name for the 'GetLocalDrawingMode' method.
			/// </summary>
			// Token: 0x04004E6F RID: 20079
			public static readonly StringName GetLocalDrawingMode = "GetLocalDrawingMode";

			/// <summary>
			/// Cached name for the 'ToNetPosition' method.
			/// </summary>
			// Token: 0x04004E70 RID: 20080
			public static readonly StringName ToNetPosition = "ToNetPosition";

			/// <summary>
			/// Cached name for the 'FromNetPosition' method.
			/// </summary>
			// Token: 0x04004E71 RID: 20081
			public static readonly StringName FromNetPosition = "FromNetPosition";

			/// <summary>
			/// Cached name for the 'UpdateVisibilityFromSettings' method.
			/// </summary>
			// Token: 0x04004E72 RID: 20082
			public static readonly StringName UpdateVisibilityFromSettings = "UpdateVisibilityFromSettings";

			/// <summary>
			/// Cached name for the 'ClearAllLines' method.
			/// </summary>
			// Token: 0x04004E73 RID: 20083
			public static readonly StringName ClearAllLines = "ClearAllLines";

			/// <summary>
			/// Cached name for the 'OnPlayerScreenChanged' method.
			/// </summary>
			// Token: 0x04004E74 RID: 20084
			public static readonly StringName OnPlayerScreenChanged = "OnPlayerScreenChanged";

			/// <summary>
			/// Cached name for the 'TrySendSyncMessage' method.
			/// </summary>
			// Token: 0x04004E75 RID: 20085
			public static readonly StringName TrySendSyncMessage = "TrySendSyncMessage";

			/// <summary>
			/// Cached name for the 'SendSyncMessage' method.
			/// </summary>
			// Token: 0x04004E76 RID: 20086
			public static readonly StringName SendSyncMessage = "SendSyncMessage";

			/// <summary>
			/// Cached name for the 'UpdateLocalCursor' method.
			/// </summary>
			// Token: 0x04004E77 RID: 20087
			public static readonly StringName UpdateLocalCursor = "UpdateLocalCursor";

			/// <summary>
			/// Cached name for the 'RepositionBasedOnBackground' method.
			/// </summary>
			// Token: 0x04004E78 RID: 20088
			public static readonly StringName RepositionBasedOnBackground = "RepositionBasedOnBackground";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014AD RID: 5293
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_lineDrawScene' field.
			/// </summary>
			// Token: 0x04004E79 RID: 20089
			public static readonly StringName _lineDrawScene = "_lineDrawScene";

			/// <summary>
			/// Cached name for the '_lineEraseScene' field.
			/// </summary>
			// Token: 0x04004E7A RID: 20090
			public static readonly StringName _lineEraseScene = "_lineEraseScene";

			/// <summary>
			/// Cached name for the '_cursorManager' field.
			/// </summary>
			// Token: 0x04004E7B RID: 20091
			public static readonly StringName _cursorManager = "_cursorManager";

			/// <summary>
			/// Cached name for the '_eraserMaterial' field.
			/// </summary>
			// Token: 0x04004E7C RID: 20092
			public static readonly StringName _eraserMaterial = "_eraserMaterial";

			/// <summary>
			/// Cached name for the '_defaultSize' field.
			/// </summary>
			// Token: 0x04004E7D RID: 20093
			public static readonly StringName _defaultSize = "_defaultSize";

			/// <summary>
			/// Cached name for the '_lastMessageMsec' field.
			/// </summary>
			// Token: 0x04004E7E RID: 20094
			public static readonly StringName _lastMessageMsec = "_lastMessageMsec";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014AE RID: 5294
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
