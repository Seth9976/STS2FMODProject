using System;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	/// <summary>
	/// The state of the multiplayer game that the client is joining.
	/// </summary>
	// Token: 0x02000C91 RID: 3217
	public enum RunSessionState
	{
		// Token: 0x04002CE4 RID: 11492
		None,
		// Token: 0x04002CE5 RID: 11493
		InLobby,
		// Token: 0x04002CE6 RID: 11494
		InLoadedLobby,
		// Token: 0x04002CE7 RID: 11495
		Running
	}
}
