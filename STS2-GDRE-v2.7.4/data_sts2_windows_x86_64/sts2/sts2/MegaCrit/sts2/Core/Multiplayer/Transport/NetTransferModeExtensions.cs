using System;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport
{
	// Token: 0x02000435 RID: 1077
	public static class NetTransferModeExtensions
	{
		/// <summary>
		/// Returns the channel that should be used to send a message with the given mode.
		/// ENet uses the channel ID to denote priority: https://www.linuxjournal.com/content/network-programming-enet
		/// Our reliable messages should be considered higher priority than our unreliable messages.
		/// </summary>
		// Token: 0x06004475 RID: 17525 RVA: 0x00210F58 File Offset: 0x0020F158
		public static int ToChannelId(this NetTransferMode mode)
		{
			int num;
			if (mode != NetTransferMode.Unreliable)
			{
				if (mode != NetTransferMode.Reliable)
				{
					throw new ArgumentOutOfRangeException("mode", mode, null);
				}
				num = 0;
			}
			else
			{
				num = 1;
			}
			return num;
		}
	}
}
