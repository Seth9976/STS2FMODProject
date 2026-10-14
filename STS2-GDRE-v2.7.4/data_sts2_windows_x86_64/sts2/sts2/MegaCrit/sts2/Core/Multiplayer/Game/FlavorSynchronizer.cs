using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Handles miscellaneous multiplayer messages that are for game feel and fun.
	/// </summary>
	// Token: 0x02000496 RID: 1174
	[NullableContext(1)]
	[Nullable(0)]
	public class FlavorSynchronizer : IDisposable
	{
		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06004738 RID: 18232 RVA: 0x0021B6C4 File Offset: 0x002198C4
		// (remove) Token: 0x06004739 RID: 18233 RVA: 0x0021B6FC File Offset: 0x002198FC
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<ulong> OnEndTurnPingReceived;

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x0600473A RID: 18234 RVA: 0x0021B731 File Offset: 0x00219931
		private Player LocalPlayer
		{
			get
			{
				return this._playerCollection.GetPlayer(this._localPlayerId);
			}
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x0021B744 File Offset: 0x00219944
		public FlavorSynchronizer(INetGameService gameService, IPlayerCollection playerCollection, ulong localPlayerId)
		{
			this._gameService = gameService;
			this._playerCollection = playerCollection;
			this._localPlayerId = localPlayerId;
			this._gameService.RegisterMessageHandler<EndTurnPingMessage>(new MessageHandlerDelegate<EndTurnPingMessage>(this.HandleEndTurnPingMessage));
			this._gameService.RegisterMessageHandler<MapPingMessage>(new MessageHandlerDelegate<MapPingMessage>(this.HandleMapPingMessage));
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x0021B7A5 File Offset: 0x002199A5
		public void Dispose()
		{
			this._gameService.UnregisterMessageHandler<EndTurnPingMessage>(new MessageHandlerDelegate<EndTurnPingMessage>(this.HandleEndTurnPingMessage));
			this._gameService.UnregisterMessageHandler<MapPingMessage>(new MessageHandlerDelegate<MapPingMessage>(this.HandleMapPingMessage));
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x0021B7D8 File Offset: 0x002199D8
		public void SendEndTurnPing()
		{
			if (Time.GetTicksMsec() < this._nextAllowedPingTime)
			{
				return;
			}
			this._gameService.SendMessage<EndTurnPingMessage>(default(EndTurnPingMessage));
			this._nextAllowedPingTime = Time.GetTicksMsec() + 1000UL;
			this.CreateEndTurnPingDialogueIfNecessary(this.LocalPlayer);
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x0021B828 File Offset: 0x00219A28
		public void SendMapPing(MapCoord coord)
		{
			if (Time.GetTicksMsec() < this._nextAllowedPingTime)
			{
				return;
			}
			this._gameService.SendMessage<MapPingMessage>(new MapPingMessage
			{
				coord = coord
			});
			this._nextAllowedPingTime = Time.GetTicksMsec() + 200UL;
			this.CreateMapPing(coord, this.LocalPlayer);
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x0021B87E File Offset: 0x00219A7E
		private void HandleEndTurnPingMessage(EndTurnPingMessage message, ulong senderId)
		{
			Action<ulong> onEndTurnPingReceived = this.OnEndTurnPingReceived;
			if (onEndTurnPingReceived != null)
			{
				onEndTurnPingReceived(senderId);
			}
			this.CreateEndTurnPingDialogueIfNecessary(this._playerCollection.GetPlayer(senderId));
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x0021B8A4 File Offset: 0x00219AA4
		private void CreateEndTurnPingDialogueIfNecessary(Player player)
		{
			if (NRun.Instance == null)
			{
				return;
			}
			NSpeechBubbleVfx nspeechBubbleVfx;
			this._endTurnPingDialogues.TryGetValue(player, out nspeechBubbleVfx);
			if (nspeechBubbleVfx != null && GodotObject.IsInstanceValid(nspeechBubbleVfx))
			{
				nspeechBubbleVfx.QueueFreeSafely();
			}
			string text = (player.Creature.IsDead ? "dead" : "alive");
			LocString locString = new LocString("characters", player.Character.Id.Entry + ".banter." + text + ".endTurnPing");
			nspeechBubbleVfx = NSpeechBubbleVfx.Create(locString.GetFormattedText(), player.Creature, 1.5, player.Character.SpeechBubbleColor);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.CombatVfxContainer.AddChildSafely(nspeechBubbleVfx);
			}
			this._endTurnPingDialogues[player] = nspeechBubbleVfx;
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0021B967 File Offset: 0x00219B67
		private void HandleMapPingMessage(MapPingMessage message, ulong senderId)
		{
			this.CreateMapPing(message.coord, this._playerCollection.GetPlayer(senderId));
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x0021B981 File Offset: 0x00219B81
		private void CreateMapPing(MapCoord coord, Player player)
		{
			if (NRun.Instance == null)
			{
				return;
			}
			NRun.Instance.GlobalUi.MapScreen.PingMapCoord(coord, player);
		}

		// Token: 0x04001BE3 RID: 7139
		private const ulong _pingDebounceMsec = 1000UL;

		// Token: 0x04001BE4 RID: 7140
		private const ulong _mapPingDebounceMsec = 200UL;

		// Token: 0x04001BE5 RID: 7141
		private readonly INetGameService _gameService;

		// Token: 0x04001BE6 RID: 7142
		private readonly IPlayerCollection _playerCollection;

		// Token: 0x04001BE7 RID: 7143
		private readonly ulong _localPlayerId;

		// Token: 0x04001BE8 RID: 7144
		private ulong _nextAllowedPingTime;

		// Token: 0x04001BE9 RID: 7145
		[Nullable(new byte[] { 1, 1, 2 })]
		private readonly Dictionary<Player, NSpeechBubbleVfx> _endTurnPingDialogues = new Dictionary<Player, NSpeechBubbleVfx>();
	}
}
