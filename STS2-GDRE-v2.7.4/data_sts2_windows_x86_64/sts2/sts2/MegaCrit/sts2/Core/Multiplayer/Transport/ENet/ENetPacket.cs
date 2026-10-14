using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet
{
	// Token: 0x02000443 RID: 1091
	[NullableContext(1)]
	[Nullable(0)]
	public class ENetPacket
	{
		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060044C5 RID: 17605 RVA: 0x00213150 File Offset: 0x00211350
		public ENetPacketType PacketType
		{
			get
			{
				return (ENetPacketType)this._packetBytes[0];
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060044C6 RID: 17606 RVA: 0x0021315A File Offset: 0x0021135A
		public byte[] AllBytes
		{
			get
			{
				return this._packetBytes;
			}
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x00213162 File Offset: 0x00211362
		public ENetPacket(byte[] bytes)
		{
			this._packetBytes = bytes;
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x00213174 File Offset: 0x00211374
		public static ENetPacket FromHandshakeRequest(ENetHandshakeRequest request)
		{
			byte[] array = new byte[9];
			array[0] = 0;
			Span<byte> span = array.AsSpan<byte>();
			ref Span<byte> ptr = ref span;
			BinaryPrimitives.WriteUInt64BigEndian(ptr.Slice(1, ptr.Length - 1), request.netId);
			return new ENetPacket(array);
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x002131B8 File Offset: 0x002113B8
		public ENetHandshakeRequest AsHandshakeRequest()
		{
			if (this.PacketType != ENetPacketType.HandshakeRequest)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to interpret ENet packet of type ");
				defaultInterpolatedStringHandler.AppendFormatted<ENetPacketType>(this.PacketType);
				defaultInterpolatedStringHandler.AppendLiteral(" as handshake request");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ulong num = BinaryPrimitives.ReadUInt64BigEndian(RuntimeHelpers.GetSubArray<byte>(this._packetBytes, Range.StartAt(1)));
			return new ENetHandshakeRequest
			{
				netId = num
			};
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x00213240 File Offset: 0x00211440
		public static ENetPacket FromHandshakeResponse(ENetHandshakeResponse response)
		{
			byte[] array = new byte[10];
			array[0] = 1;
			array[1] = (byte)response.status;
			Span<byte> span = array.AsSpan<byte>();
			ref Span<byte> ptr = ref span;
			BinaryPrimitives.WriteUInt64BigEndian(ptr.Slice(2, ptr.Length - 2), response.netId);
			return new ENetPacket(array);
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x0021328C File Offset: 0x0021148C
		public ENetHandshakeResponse AsHandshakeResponse()
		{
			if (this.PacketType != ENetPacketType.HandshakeResponse)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to interpret ENet packet of type ");
				defaultInterpolatedStringHandler.AppendFormatted<ENetPacketType>(this.PacketType);
				defaultInterpolatedStringHandler.AppendLiteral(" as handshake response");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ENetHandshakeStatus enetHandshakeStatus = (ENetHandshakeStatus)this._packetBytes[1];
			ulong num = BinaryPrimitives.ReadUInt64BigEndian(RuntimeHelpers.GetSubArray<byte>(this._packetBytes, Range.StartAt(2)));
			return new ENetHandshakeResponse
			{
				netId = num,
				status = enetHandshakeStatus
			};
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x00213324 File Offset: 0x00211524
		public static ENetPacket FromDisconnection(ENetDisconnection disconnection)
		{
			return new ENetPacket(new byte[]
			{
				2,
				(byte)disconnection.reason
			});
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x0021334C File Offset: 0x0021154C
		public ENetDisconnection AsDisconnection()
		{
			if (this.PacketType != ENetPacketType.Disconnection)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to interpret ENet packet of type ");
				defaultInterpolatedStringHandler.AppendFormatted<ENetPacketType>(this.PacketType);
				defaultInterpolatedStringHandler.AppendLiteral(" as disconnection");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			NetError netError = (NetError)this._packetBytes[1];
			return new ENetDisconnection
			{
				reason = netError
			};
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x002133BC File Offset: 0x002115BC
		public static ENetPacket FromAppMessage(byte[] messageBytes, int count)
		{
			byte[] array = new byte[count + 1];
			array[0] = 3;
			Array.Copy(messageBytes, 0, array, 1, count);
			return new ENetPacket(array);
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x002133E8 File Offset: 0x002115E8
		public byte[] AsAppMessage()
		{
			if (this.PacketType != ENetPacketType.ApplicationMessage)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to interpret ENet packet of type ");
				defaultInterpolatedStringHandler.AppendFormatted<ENetPacketType>(this.PacketType);
				defaultInterpolatedStringHandler.AppendLiteral(" as disconnection");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return RuntimeHelpers.GetSubArray<byte>(this._packetBytes, Range.StartAt(1));
		}

		// Token: 0x04001ABD RID: 6845
		private readonly byte[] _packetBytes;
	}
}
