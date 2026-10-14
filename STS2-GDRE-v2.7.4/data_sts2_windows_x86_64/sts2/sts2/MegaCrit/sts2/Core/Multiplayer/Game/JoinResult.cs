using System;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Game
{
	// Token: 0x0200049C RID: 1180
	public struct JoinResult
	{
		// Token: 0x04001BF3 RID: 7155
		public GameMode gameMode;

		// Token: 0x04001BF4 RID: 7156
		public RunSessionState? sessionState;

		// Token: 0x04001BF5 RID: 7157
		public ClientRejoinResponseMessage? rejoinResponse;

		// Token: 0x04001BF6 RID: 7158
		public ClientLoadJoinResponseMessage? loadJoinResponse;

		// Token: 0x04001BF7 RID: 7159
		public ClientLobbyJoinResponseMessage? joinResponse;
	}
}
