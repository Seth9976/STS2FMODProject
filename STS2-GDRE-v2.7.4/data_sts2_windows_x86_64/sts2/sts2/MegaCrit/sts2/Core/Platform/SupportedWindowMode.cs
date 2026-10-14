using System;

namespace MegaCrit.Sts2.Core.Platform
{
	/// <summary>
	/// The type of fullscreen that this platform supports.
	/// </summary>
	// Token: 0x0200015A RID: 346
	public enum SupportedWindowMode
	{
		/// <summary>
		/// Invalid value.
		/// </summary>
		// Token: 0x040005D1 RID: 1489
		None,
		/// <summary>
		/// This platform supports both fullscreen and non-fullscreen.
		/// </summary>
		// Token: 0x040005D2 RID: 1490
		Any,
		/// <summary>
		/// This platform forces being in fullscreen. If the settings.save says that the game should be windowed, it is
		/// forced into fullscreen anyway (with the settings.save unchanged). The settings menu toggle will show as disabled.
		/// Used on platforms for which fullscreen may be supported depending on the current configuration; for example,
		/// Big Picture mode on a PC.
		/// </summary>
		// Token: 0x040005D3 RID: 1491
		FullscreenOnlyDisplayToggle,
		/// <summary>
		/// This platform only allows being in fullscreen. If the settings.save says that the game should be windowed, it is
		/// forced into fullscreen anyway. The settings menu toggle will be hidden.
		/// Used on platforms like consoles and mobile where fullscreen is never supported.
		/// </summary>
		// Token: 0x040005D4 RID: 1492
		FullscreenOnly
	}
}
