using System;

namespace MegaCrit.Sts2.Core.Platform
{
	// Token: 0x0200015B RID: 347
	public static class SupportedWindowModeExtensions
	{
		// Token: 0x06001095 RID: 4245 RVA: 0x000624BF File Offset: 0x000606BF
		public static bool ShouldForceFullscreen(this SupportedWindowMode mode)
		{
			return mode == SupportedWindowMode.FullscreenOnly || mode == SupportedWindowMode.FullscreenOnlyDisplayToggle;
		}
	}
}
