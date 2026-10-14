using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Multiplayer
{
	// Token: 0x0200042C RID: 1068
	[NullableContext(1)]
	[Nullable(0)]
	public class NetMessageBus
	{
		// Token: 0x0600442F RID: 17455 RVA: 0x002107B4 File Offset: 0x0020E9B4
		public byte[] SerializeMessage<[Nullable(0)] T>(ulong senderId, T message, out int length) where T : INetMessage
		{
			this._writer.Reset();
			this._writer.WriteByte((byte)message.ToId(), 8);
			this._writer.WriteULong(senderId, 64);
			message.Serialize(this._writer);
			length = (int)Math.Ceiling((double)((float)this._writer.BitPosition / 8f));
			return this._writer.Buffer;
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x0021082C File Offset: 0x0020EA2C
		public bool TryDeserializeMessage(byte[] packetBytes, [Nullable(2)] out INetMessage message, out ulong? overrideSenderId)
		{
			overrideSenderId = null;
			message = null;
			this._reader.Reset(packetBytes);
			byte b = this._reader.ReadByte(8);
			Type type;
			if (!MessageTypes.TryGetMessageType((int)b, out type))
			{
				if (ModManager.IsRunningModded() && (int)b >= MessageTypes.Count && !this._warnedMessageTypes.Contains(b))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(219, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Received message with length ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(packetBytes.Length);
					defaultInterpolatedStringHandler.AppendLiteral(" and first byte ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(b);
					defaultInterpolatedStringHandler.AppendLiteral(" that is outside the bounds of our known messages (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(MessageTypes.Count);
					defaultInterpolatedStringHandler.AppendLiteral("). Since we are modded, we are assuming this is a message that does not affect gameplay and will not warn about this again.");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					this._warnedMessageTypes.Add(b);
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Received message with length ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(packetBytes.Length);
					defaultInterpolatedStringHandler.AppendLiteral(" and first byte ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(b);
					defaultInterpolatedStringHandler.AppendLiteral(" that is not a valid message ID!");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				return false;
			}
			overrideSenderId = new ulong?(this._reader.ReadULong(64));
			message = (INetMessage)Activator.CreateInstance(type);
			message.Deserialize(this._reader);
			return true;
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x00210988 File Offset: 0x0020EB88
		public void SendMessageToAllHandlers(INetMessage message, ulong senderId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this._isBufferingMessages && message.ShouldBuffer)
			{
				Logger logger = this._logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received message of type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(message.GetType());
				defaultInterpolatedStringHandler.AppendLiteral(" but we are currently buffering messages.");
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				this._bufferedMessages.Add(new ValueTuple<INetMessage, ulong>(message, senderId));
				return;
			}
			List<NetMessageBus.CallbackPair> list;
			if (!this._messageHandlers.TryGetValue(message.GetType(), out list) || list.Count == 0)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(80, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received message of type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(message.GetType());
				defaultInterpolatedStringHandler.AppendLiteral(", but no message handlers are registered for that type!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			this._cachedPairList.Clear();
			this._cachedPairList.AddRange(list);
			Logger logger2 = this._logger;
			LogLevel logLevel = message.LogLevel;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Received message ");
			defaultInterpolatedStringHandler.AppendFormatted<INetMessage>(message);
			defaultInterpolatedStringHandler.AppendLiteral(", sending to ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._cachedPairList.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" handlers");
			logger2.LogMessage(logLevel, defaultInterpolatedStringHandler.ToStringAndClear(), 0);
			foreach (NetMessageBus.CallbackPair callbackPair in this._cachedPairList)
			{
				try
				{
					callbackPair.handler(message, senderId);
				}
				catch (Exception ex)
				{
					Logger logger3 = this._logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Exception encountered while processing message ");
					defaultInterpolatedStringHandler.AppendFormatted<INetMessage>(message);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
					logger3.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				}
			}
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x00210B70 File Offset: 0x0020ED70
		public void SetBufferMessages(bool bufferMessages)
		{
			if (this._isBufferingMessages != bufferMessages)
			{
				this._isBufferingMessages = bufferMessages;
				if (bufferMessages)
				{
					this._logger.Debug("NetMessageBus is starting to buffer messages.", 1);
					return;
				}
				Logger logger = this._logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
				defaultInterpolatedStringHandler.AppendLiteral("NetMessageBus is releasing ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._bufferedMessages.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" buffered messages.");
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				foreach (ValueTuple<INetMessage, ulong> valueTuple in this._bufferedMessages)
				{
					INetMessage item = valueTuple.Item1;
					ulong item2 = valueTuple.Item2;
					this.SendMessageToAllHandlers(item, item2);
				}
				this._bufferedMessages.Clear();
			}
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x00210C4C File Offset: 0x0020EE4C
		public void RegisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage
		{
			if (typeof(T) == typeof(INetMessage))
			{
				throw new InvalidOperationException("RegisterMessageHandler must be called with a concrete implementation of INetMessage!");
			}
			List<NetMessageBus.CallbackPair> list;
			if (!this._messageHandlers.TryGetValue(typeof(T), out list))
			{
				list = new List<NetMessageBus.CallbackPair>();
				this._messageHandlers[typeof(T)] = list;
			}
			NetMessageBus.CallbackPair callbackPair = new NetMessageBus.CallbackPair
			{
				handler = delegate(INetMessage message, ulong senderId)
				{
					handler((T)((object)message), senderId);
				},
				originalHandler = handler
			};
			list.Add(callbackPair);
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x00210CF0 File Offset: 0x0020EEF0
		public void UnregisterMessageHandler<[Nullable(0)] T>(MessageHandlerDelegate<T> handler) where T : INetMessage
		{
			if (typeof(T) == typeof(INetMessage))
			{
				throw new InvalidOperationException("UnregisterMessageHandler must be called with a concrete implementation of INetMessage!");
			}
			List<NetMessageBus.CallbackPair> list;
			if (!this._messageHandlers.TryGetValue(typeof(T), out list))
			{
				return;
			}
			list.RemoveAll((NetMessageBus.CallbackPair p) => (MessageHandlerDelegate<T>)p.originalHandler == handler);
		}

		// Token: 0x04001A75 RID: 6773
		private readonly PacketReader _reader = new PacketReader();

		// Token: 0x04001A76 RID: 6774
		private readonly PacketWriter _writer = new PacketWriter();

		// Token: 0x04001A77 RID: 6775
		private readonly Logger _logger = new Logger("NetMessageBus", LogType.Network);

		// Token: 0x04001A78 RID: 6776
		private readonly Dictionary<Type, List<NetMessageBus.CallbackPair>> _messageHandlers = new Dictionary<Type, List<NetMessageBus.CallbackPair>>();

		// Token: 0x04001A79 RID: 6777
		private readonly List<NetMessageBus.CallbackPair> _cachedPairList = new List<NetMessageBus.CallbackPair>();

		// Token: 0x04001A7A RID: 6778
		private bool _isBufferingMessages;

		// Token: 0x04001A7B RID: 6779
		private readonly HashSet<byte> _warnedMessageTypes = new HashSet<byte>();

		// Token: 0x04001A7C RID: 6780
		[Nullable(new byte[] { 1, 0, 1 })]
		private readonly List<ValueTuple<INetMessage, ulong>> _bufferedMessages = new List<ValueTuple<INetMessage, ulong>>();

		// Token: 0x02001992 RID: 6546
		// (Invoke) Token: 0x0600AB15 RID: 43797
		[NullableContext(0)]
		private delegate void AnonymizedMessageHandlerDelegate(INetMessage message, ulong senderId);

		// Token: 0x02001993 RID: 6547
		[Nullable(0)]
		private struct CallbackPair
		{
			// Token: 0x040064F6 RID: 25846
			public NetMessageBus.AnonymizedMessageHandlerDelegate handler;

			// Token: 0x040064F7 RID: 25847
			public object originalHandler;
		}
	}
}
