using System;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	// Token: 0x020004A0 RID: 1184
	public static class NetGameTypeExtensions
	{
		// Token: 0x06004781 RID: 18305 RVA: 0x0021C6A4 File Offset: 0x0021A8A4
		public static bool IsMultiplayer(this NetGameType type)
		{
			return type - NetGameType.Host <= 1;
		}
	}
}
