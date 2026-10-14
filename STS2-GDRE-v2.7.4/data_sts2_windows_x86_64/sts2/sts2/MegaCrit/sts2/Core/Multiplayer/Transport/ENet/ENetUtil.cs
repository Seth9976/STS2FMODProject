using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet
{
	// Token: 0x02000445 RID: 1093
	public static class ENetUtil
	{
		// Token: 0x060044D0 RID: 17616 RVA: 0x00213450 File Offset: 0x00211650
		public static NetTransferMode ModeFromFlags(int flags)
		{
			if (((long)flags & 1L) > 0L)
			{
				return NetTransferMode.Reliable;
			}
			if (((long)flags & 8L) > 0L)
			{
				return NetTransferMode.Unreliable;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Flags ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(flags);
			defaultInterpolatedStringHandler.AppendLiteral(" cannot be mapped to NetTransferMode!");
			throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x002134AC File Offset: 0x002116AC
		public static int FlagsFromMode(NetTransferMode mode)
		{
			int num;
			if (mode != NetTransferMode.Unreliable)
			{
				if (mode != NetTransferMode.Reliable)
				{
					throw new ArgumentOutOfRangeException("mode", mode, null);
				}
				num = 1;
			}
			else
			{
				num = 8;
			}
			return num;
		}
	}
}
