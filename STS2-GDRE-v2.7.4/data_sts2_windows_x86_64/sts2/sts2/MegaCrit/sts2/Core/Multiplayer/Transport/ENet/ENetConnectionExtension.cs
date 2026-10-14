using System;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Collections;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet
{
	// Token: 0x0200043C RID: 1084
	public static class ENetConnectionExtension
	{
		/// <summary>
		/// Polls an ENetConnection, returning the events that have occurred since the last poll.
		/// This wraps ENet.Service into a more digestible method.
		/// </summary>
		/// <param name="connection">The connection to poll.</param>
		/// <param name="output">The event we polled from the connection, if any.</param>
		/// <returns></returns>
		// Token: 0x060044B1 RID: 17585 RVA: 0x002128B8 File Offset: 0x00210AB8
		[NullableContext(1)]
		public static bool TryService(this ENetConnection connection, out ENetServiceData? output)
		{
			Array array = connection.Service(0);
			output = null;
			if (array == null)
			{
				return false;
			}
			ENetConnection.EventType eventType = array[0].As<ENetConnection.EventType>();
			if (eventType == null)
			{
				return false;
			}
			ENetServiceData enetServiceData = new ENetServiceData
			{
				type = eventType,
				peer = array[1].As<ENetPacketPeer>(),
				originalData = array
			};
			if (eventType == 3L)
			{
				enetServiceData.channel = array[3].As<int>();
				enetServiceData.packetData = enetServiceData.peer.GetPacket();
				enetServiceData.error = enetServiceData.peer.GetPacketError();
				enetServiceData.mode = NetTransferMode.None;
			}
			output = new ENetServiceData?(enetServiceData);
			return true;
		}
	}
}
