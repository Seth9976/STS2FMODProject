using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Logging;
using Steamworks;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.Steam
{
	// Token: 0x02000439 RID: 1081
	[NullableContext(1)]
	[Nullable(0)]
	public static class SteamUtil
	{
		// Token: 0x0600449F RID: 17567 RVA: 0x002121E0 File Offset: 0x002103E0
		public static NetTransferMode ModeFromFlags(int flags)
		{
			if ((flags & 8) > 0)
			{
				return NetTransferMode.Reliable;
			}
			if (flags == 0)
			{
				return NetTransferMode.Unreliable;
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x002121F4 File Offset: 0x002103F4
		public static int FlagsFromMode(NetTransferMode mode)
		{
			int num;
			if (mode != NetTransferMode.Unreliable)
			{
				if (mode != NetTransferMode.Reliable)
				{
					throw new ArgumentOutOfRangeException("mode", mode, null);
				}
				num = 8;
			}
			else
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x00212228 File Offset: 0x00210428
		public static SteamNetworkingIdentity ToNetId(this CSteamID id)
		{
			SteamNetworkingIdentity steamNetworkingIdentity = default(SteamNetworkingIdentity);
			steamNetworkingIdentity.SetSteamID(id);
			return steamNetworkingIdentity;
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x00212248 File Offset: 0x00210448
		public static SteamNetworkingIdentity ToNetId64(this ulong id)
		{
			SteamNetworkingIdentity steamNetworkingIdentity = default(SteamNetworkingIdentity);
			steamNetworkingIdentity.SetSteamID64(id);
			return steamNetworkingIdentity;
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x00212268 File Offset: 0x00210468
		public static void ProcessMessages(HSteamNetConnection conn, INetHandler handler, Logger logger)
		{
			int num;
			do
			{
				num = SteamNetworkingSockets.ReceiveMessagesOnConnection(conn, SteamUtil._messageBuffer, SteamUtil._messageBuffer.Length);
				if (num > 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Processing ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral(" packets");
					logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				}
				for (int i = 0; i < num; i++)
				{
					IntPtr intPtr = SteamUtil._messageBuffer[i];
					SteamNetworkingMessage_t steamNetworkingMessage_t = Marshal.PtrToStructure<SteamNetworkingMessage_t>(intPtr);
					byte[] array = new byte[steamNetworkingMessage_t.m_cbSize];
					Marshal.Copy(steamNetworkingMessage_t.m_pData, array, 0, steamNetworkingMessage_t.m_cbSize);
					NetTransferMode netTransferMode = SteamUtil.ModeFromFlags(steamNetworkingMessage_t.m_nFlags);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Received packet of size ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(array.Length);
					defaultInterpolatedStringHandler.AppendLiteral(" from sender ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(steamNetworkingMessage_t.m_identityPeer.GetSteamID64());
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted<NetTransferMode>(netTransferMode);
					defaultInterpolatedStringHandler.AppendLiteral(", ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(steamNetworkingMessage_t.m_nChannel);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					logger.VeryDebug(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
					handler.OnPacketReceived(steamNetworkingMessage_t.m_identityPeer.GetSteamID().m_SteamID, array, netTransferMode, steamNetworkingMessage_t.m_nChannel);
					SteamNetworkingMessage_t.Release(intPtr);
				}
			}
			while (num == SteamUtil._messageBuffer.Length);
		}

		// Token: 0x04001A9A RID: 6810
		public const uint handshakeMagicBytes = 2204332656U;

		// Token: 0x04001A9B RID: 6811
		private static readonly IntPtr[] _messageBuffer = new IntPtr[64];
	}
}
