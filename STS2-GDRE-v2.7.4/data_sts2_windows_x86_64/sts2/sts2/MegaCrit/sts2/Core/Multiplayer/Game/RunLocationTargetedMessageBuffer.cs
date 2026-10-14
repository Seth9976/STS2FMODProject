using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	/// <summary>
	/// Responsible for holding messages that are not destined for the current map point, and waiting until that map point
	/// is entered before processing those messages.
	/// We may be slow to transition to a new map point (e.g. because of loading delays), and messages from other peers may
	/// come in before we're ready to receive them.
	/// Note that it is important that queued messages are sent out in the order that they were received.
	/// </summary>
	// Token: 0x020004A7 RID: 1191
	[NullableContext(1)]
	[Nullable(0)]
	public class RunLocationTargetedMessageBuffer
	{
		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x060047DE RID: 18398 RVA: 0x0021EDDB File Offset: 0x0021CFDB
		// (set) Token: 0x060047DF RID: 18399 RVA: 0x0021EDE3 File Offset: 0x0021CFE3
		public RunLocation CurrentLocation { get; private set; }

		// Token: 0x060047E0 RID: 18400 RVA: 0x0021EDEC File Offset: 0x0021CFEC
		public RunLocationTargetedMessageBuffer(INetGameService gameService)
		{
			this._gameService = gameService;
			this._visitedLocations.Add(this.CurrentLocation);
		}

		/// <summary>
		/// This should be called whenever the run location changes. If messages are blocked waiting for a location change,
		/// then they will be sent to registered message handlers in the order that they were received.
		/// </summary>
		// Token: 0x060047E1 RID: 18401 RVA: 0x0021EE4C File Offset: 0x0021D04C
		public void OnLocationChanged(RunLocation location)
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(81, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Run location changed to ");
			defaultInterpolatedStringHandler.AppendFormatted<RunLocation>(location);
			defaultInterpolatedStringHandler.AppendLiteral(" (previously at: ");
			defaultInterpolatedStringHandler.AppendFormatted<RunLocation>(this.CurrentLocation);
			defaultInterpolatedStringHandler.AppendLiteral("), checking if we have enqueued messages");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			this.CurrentLocation = location;
			this._visitedLocations.Add(this.CurrentLocation);
			for (int i = 0; i < this._messagesWaitingOnLocationChange.Count; i++)
			{
				RunLocationTargetedMessageBuffer.BlockedMessage blockedMessage = this._messagesWaitingOnLocationChange[i];
				if (this._visitedLocations.Contains(blockedMessage.location))
				{
					Logger logger2 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Handling enqueued message ");
					defaultInterpolatedStringHandler.AppendFormatted<INetMessage>(blockedMessage.message);
					defaultInterpolatedStringHandler.AppendLiteral(" of type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(blockedMessage.messageType);
					defaultInterpolatedStringHandler.AppendLiteral(" from ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(blockedMessage.senderId);
					logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					this.CallHandlersOfType(blockedMessage.messageType, blockedMessage.message, blockedMessage.senderId);
					this._messagesWaitingOnLocationChange.RemoveAt(i);
					i--;
				}
			}
			if (this._messagesWaitingOnLocationChange.Count > 0)
			{
				Logger logger3 = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(114, 3);
				defaultInterpolatedStringHandler.AppendLiteral("After transitioning to ");
				defaultInterpolatedStringHandler.AppendFormatted<RunLocation>(location);
				defaultInterpolatedStringHandler.AppendLiteral(", there are still ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._messagesWaitingOnLocationChange.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" messages for other locations. This is likely indicates a bug. Messages:\n");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<RunLocationTargetedMessageBuffer.BlockedMessage>("\n", this._messagesWaitingOnLocationChange));
				logger3.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			}
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x0021F018 File Offset: 0x0021D218
		private void CallHandlersOfType(Type type, INetMessage message, ulong senderId)
		{
			foreach (RunLocationTargetedMessageBuffer.TypeAndMessageHandlers typeAndMessageHandlers in this._messageHandlers)
			{
				if (typeAndMessageHandlers.messageType == type)
				{
					foreach (RunLocationTargetedMessageBuffer.MessageHandler messageHandler in typeAndMessageHandlers.handlers)
					{
						messageHandler.anonymizedHandler(message, senderId);
					}
				}
			}
		}

		/// <summary>
		/// Registers a message handler for map targeted messages.
		/// <see cref="T:MegaCrit.Sts2.Core.Multiplayer.Messages.Game.IRunLocationTargetedMessage" />s should be registered here instead of directly to the INetGameHandler; otherwise,
		/// they may be received at the wrong time.
		/// </summary>
		/// <param name="handler">
		/// The delegate to call when the message type is received or when we enter a new location and a message for that
		/// location is enqueued.
		/// </param>
		// Token: 0x060047E3 RID: 18403 RVA: 0x0021F0BC File Offset: 0x0021D2BC
		public void RegisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage, IRunLocationTargetedMessage
		{
			RunLocationTargetedMessageBuffer.<>c__DisplayClass16_0<T> CS$<>8__locals1 = new RunLocationTargetedMessageBuffer.<>c__DisplayClass16_0<T>();
			CS$<>8__locals1.handler = handler;
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Register message handler ");
			defaultInterpolatedStringHandler.AppendFormatted<MessageHandlerDelegate<T>>(CS$<>8__locals1.handler);
			defaultInterpolatedStringHandler.AppendLiteral(" for ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			RunLocationTargetedMessageBuffer.TypeAndMessageHandlers? typeAndMessageHandlers = null;
			foreach (RunLocationTargetedMessageBuffer.TypeAndMessageHandlers typeAndMessageHandlers2 in this._messageHandlers)
			{
				if (typeAndMessageHandlers2.messageType == typeof(T))
				{
					typeAndMessageHandlers = new RunLocationTargetedMessageBuffer.TypeAndMessageHandlers?(typeAndMessageHandlers2);
				}
			}
			if (typeAndMessageHandlers == null)
			{
				MessageHandlerDelegate<T> messageHandlerDelegate = new MessageHandlerDelegate<T>(this.HandleMessage<T>);
				typeAndMessageHandlers = new RunLocationTargetedMessageBuffer.TypeAndMessageHandlers?(new RunLocationTargetedMessageBuffer.TypeAndMessageHandlers
				{
					messageType = typeof(T),
					netServiceHandler = messageHandlerDelegate,
					handlers = new List<RunLocationTargetedMessageBuffer.MessageHandler>()
				});
				this._gameService.RegisterMessageHandler<T>(messageHandlerDelegate);
				this._messageHandlers.Add(typeAndMessageHandlers.Value);
			}
			typeAndMessageHandlers.Value.handlers.Add(new RunLocationTargetedMessageBuffer.MessageHandler
			{
				anonymizedHandler = new RunLocationTargetedMessageBuffer.AnonymizedMessageHandlerDelegate(CS$<>8__locals1.<RegisterMessageHandler>g__AnonymousDelegate|0),
				originalHandler = CS$<>8__locals1.handler
			});
		}

		/// <summary>
		/// Unregisters a message handler for map targeted messages.
		/// </summary>
		// Token: 0x060047E4 RID: 18404 RVA: 0x0021F23C File Offset: 0x0021D43C
		public void UnregisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage, IRunLocationTargetedMessage
		{
			for (int i = 0; i < this._messageHandlers.Count; i++)
			{
				RunLocationTargetedMessageBuffer.TypeAndMessageHandlers typeAndMessageHandlers = this._messageHandlers[i];
				if (!(typeAndMessageHandlers.messageType != typeof(T)))
				{
					for (int j = 0; j < typeAndMessageHandlers.handlers.Count; j++)
					{
						RunLocationTargetedMessageBuffer.MessageHandler messageHandler = typeAndMessageHandlers.handlers[j];
						MessageHandlerDelegate<T> messageHandlerDelegate = messageHandler.originalHandler as MessageHandlerDelegate<T>;
						if (messageHandlerDelegate != null && messageHandlerDelegate == handler)
						{
							typeAndMessageHandlers.handlers.RemoveAt(j);
							j--;
						}
					}
					if (typeAndMessageHandlers.handlers.Count <= 0)
					{
						this._gameService.UnregisterMessageHandler<T>((MessageHandlerDelegate<T>)typeAndMessageHandlers.netServiceHandler);
						this._messageHandlers.RemoveAt(i);
						i--;
					}
				}
			}
		}

		/// <summary>
		/// Internal method which either sends the message to the handler, or enqueues it if the message is not destined
		/// for a location we have already visited.
		/// </summary>
		// Token: 0x060047E5 RID: 18405 RVA: 0x0021F310 File Offset: 0x0021D510
		private void HandleMessage<[Nullable(0)] T>(T message, ulong senderId) where T : INetMessage, IRunLocationTargetedMessage
		{
			Logger logger = this._logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Handling map-targeted message ");
			defaultInterpolatedStringHandler.AppendFormatted<T>(message);
			defaultInterpolatedStringHandler.AppendLiteral(" from ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(" for location ");
			defaultInterpolatedStringHandler.AppendFormatted<RunLocation>(message.Location);
			logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			if (this._visitedLocations.Contains(message.Location))
			{
				this.CallHandlersOfType(typeof(T), message, senderId);
				return;
			}
			Logger logger2 = this._logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(84, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Message ");
			defaultInterpolatedStringHandler.AppendFormatted<T>(message);
			defaultInterpolatedStringHandler.AppendLiteral(" from ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(senderId);
			defaultInterpolatedStringHandler.AppendLiteral(" is for location ");
			defaultInterpolatedStringHandler.AppendFormatted<RunLocation>(message.Location);
			defaultInterpolatedStringHandler.AppendLiteral(", enqueueing it because we are currently at location ");
			defaultInterpolatedStringHandler.AppendFormatted<RunLocation>(this.CurrentLocation);
			logger2.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
			RunLocationTargetedMessageBuffer.BlockedMessage blockedMessage = new RunLocationTargetedMessageBuffer.BlockedMessage
			{
				location = message.Location,
				message = message,
				messageType = message.GetType(),
				senderId = senderId
			};
			this._messagesWaitingOnLocationChange.Add(blockedMessage);
		}

		// Token: 0x04001C2D RID: 7213
		private readonly INetGameService _gameService;

		// Token: 0x04001C2E RID: 7214
		private readonly List<RunLocationTargetedMessageBuffer.BlockedMessage> _messagesWaitingOnLocationChange = new List<RunLocationTargetedMessageBuffer.BlockedMessage>();

		// Token: 0x04001C2F RID: 7215
		private readonly List<RunLocationTargetedMessageBuffer.TypeAndMessageHandlers> _messageHandlers = new List<RunLocationTargetedMessageBuffer.TypeAndMessageHandlers>();

		// Token: 0x04001C30 RID: 7216
		private readonly HashSet<RunLocation> _visitedLocations = new HashSet<RunLocation>();

		// Token: 0x04001C31 RID: 7217
		private readonly Logger _logger = new Logger("RunLocationTargetedMessageBuffer", LogType.GameSync);

		// Token: 0x020019D4 RID: 6612
		// (Invoke) Token: 0x0600ABA4 RID: 43940
		[NullableContext(0)]
		private delegate void AnonymizedMessageHandlerDelegate(INetMessage message, ulong senderId);

		// Token: 0x020019D5 RID: 6613
		[Nullable(0)]
		private struct TypeAndMessageHandlers
		{
			// Token: 0x0400660A RID: 26122
			public Type messageType;

			// Token: 0x0400660B RID: 26123
			public object netServiceHandler;

			// Token: 0x0400660C RID: 26124
			public List<RunLocationTargetedMessageBuffer.MessageHandler> handlers;
		}

		// Token: 0x020019D6 RID: 6614
		[Nullable(0)]
		private struct MessageHandler
		{
			// Token: 0x0400660D RID: 26125
			public object originalHandler;

			// Token: 0x0400660E RID: 26126
			public RunLocationTargetedMessageBuffer.AnonymizedMessageHandlerDelegate anonymizedHandler;
		}

		// Token: 0x020019D7 RID: 6615
		[Nullable(0)]
		private struct BlockedMessage
		{
			// Token: 0x0400660F RID: 26127
			public RunLocation location;

			// Token: 0x04006610 RID: 26128
			public INetMessage message;

			// Token: 0x04006611 RID: 26129
			public ulong senderId;

			// Token: 0x04006612 RID: 26130
			public Type messageType;
		}
	}
}
