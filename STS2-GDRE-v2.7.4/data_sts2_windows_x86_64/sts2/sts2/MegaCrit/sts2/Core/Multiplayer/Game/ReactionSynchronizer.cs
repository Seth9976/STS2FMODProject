using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;
using MegaCrit.Sts2.Core.Nodes.Reaction;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Handles miscellaneous multiplayer messages that are for game feel and fun.
	/// </summary>
	// Token: 0x020004A3 RID: 1187
	[NullableContext(1)]
	[Nullable(0)]
	public class ReactionSynchronizer : IDisposable
	{
		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06004793 RID: 18323 RVA: 0x0021CBE3 File Offset: 0x0021ADE3
		public INetGameService NetService { get; }

		// Token: 0x06004794 RID: 18324 RVA: 0x0021CBEB File Offset: 0x0021ADEB
		public ReactionSynchronizer(INetGameService netService, NReactionContainer container)
		{
			this.NetService = netService;
			this._container = container;
			this.NetService.RegisterMessageHandler<ReactionMessage>(new MessageHandlerDelegate<ReactionMessage>(this.HandleReactionMessage));
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x0021CC18 File Offset: 0x0021AE18
		public void Dispose()
		{
			this.NetService.UnregisterMessageHandler<ReactionMessage>(new MessageHandlerDelegate<ReactionMessage>(this.HandleReactionMessage));
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x0021CC34 File Offset: 0x0021AE34
		public void SendLocalReaction(ReactionType type, Vector2 mouseScreenPos)
		{
			ReactionMessage reactionMessage = new ReactionMessage
			{
				type = type,
				normalizedPosition = NetCursorHelper.GetNormalizedPosition(mouseScreenPos, this._container)
			};
			this.NetService.SendMessage<ReactionMessage>(reactionMessage);
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x0021CC72 File Offset: 0x0021AE72
		private void HandleReactionMessage(ReactionMessage message, ulong senderId)
		{
			this._container.DoRemoteReaction(message.type, NetCursorHelper.GetControlSpacePosition(message.normalizedPosition, this._container));
		}

		// Token: 0x04001C11 RID: 7185
		private readonly NReactionContainer _container;
	}
}
