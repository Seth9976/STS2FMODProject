using System;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	/// <summary>
	/// A list of reasons for which we may be disconnected from a remote host. Each of these is associated with a particular
	/// error message that we can show to the user.
	/// It's likely that this list will expand over time.
	/// </summary>
	// Token: 0x02000C89 RID: 3209
	public enum NetError
	{
		/// <summary>
		/// No reason was passed.
		/// </summary>
		// Token: 0x04002C97 RID: 11415
		None,
		/// <summary>
		/// Normal quit (Host save and quit or quit the application).
		/// </summary>
		// Token: 0x04002C98 RID: 11416
		Quit,
		/// <summary>
		/// Normal quit at the end of the run. Signals to clients they should not also quit the application.
		/// </summary>
		// Token: 0x04002C99 RID: 11417
		QuitGameOver,
		/// <summary>
		/// Host abandoned the game without saving it.
		/// </summary>
		// Token: 0x04002C9A RID: 11418
		HostAbandoned,
		/// <summary>
		/// We were forcibly removed from the game.
		/// </summary>
		// Token: 0x04002C9B RID: 11419
		Kicked,
		/// <summary>
		/// Tried to join a user that is not currently in a multiplayer game.
		/// </summary>
		// Token: 0x04002C9C RID: 11420
		InvalidJoin,
		/// <summary>
		/// The user cancelled the join flow before it was completed.
		/// </summary>
		// Token: 0x04002C9D RID: 11421
		CancelledJoin,
		/// <summary>The lobby we tried to connect to is full.</summary>
		// Token: 0x04002C9E RID: 11422
		LobbyFull,
		/// <summary>
		/// The run is already in progress, and rejoining is not implemented.
		/// </summary>
		// Token: 0x04002C9F RID: 11423
		RunInProgress,
		/// <summary>
		/// The run was loaded from a save file, and the player attempting to connect is not in the save.
		/// </summary>
		// Token: 0x04002CA0 RID: 11424
		NotInSaveGame,
		/// <summary>
		/// The host's version does not match the client's.
		/// </summary>
		// Token: 0x04002CA1 RID: 11425
		VersionMismatch,
		/// <summary>
		/// You are banned from the lobby, you have blocked someone in the lobby, or someone in the lobby blocked you.
		/// </summary>
		// Token: 0x04002CA2 RID: 11426
		JoinBlockedByUser,
		/// <summary>
		/// Our state, as a client, diverged from the host's during combat.
		/// </summary>
		// Token: 0x04002CA3 RID: 11427
		StateDivergence,
		/// <summary>
		/// The client did not send the lobby join handshake response in time.
		/// Different from an internet timeout, as that is below the application layer.
		/// </summary>
		// Token: 0x04002CA4 RID: 11428
		HandshakeTimeout,
		/// <summary>
		/// Either the host had mods that we didn't have, or we had mods that the host didn't have.
		/// </summary>
		// Token: 0x04002CA5 RID: 11429
		ModMismatch,
		/// <summary>
		/// Couldn't connect to the session, likely because of internet issues.
		/// </summary>
		// Token: 0x04002CA6 RID: 11430
		NoInternet,
		/// <summary>
		/// Connection timed out.
		/// </summary>
		// Token: 0x04002CA7 RID: 11431
		Timeout,
		/// <summary>
		/// Internal error, like an exception or a similar local bug.
		/// </summary>
		// Token: 0x04002CA8 RID: 11432
		InternalError,
		/// <summary>
		/// Network issue that we are not sure how to diagnose.
		/// </summary>
		// Token: 0x04002CA9 RID: 11433
		UnknownNetworkError,
		/// <summary>
		/// Too many attempts to do the same thing. Player should try again in a bit.
		/// </summary>
		// Token: 0x04002CAA RID: 11434
		RateLimited,
		/// <summary>
		/// Generic transient issue. Player should try again later.
		/// </summary>
		// Token: 0x04002CAB RID: 11435
		TryAgainLater,
		/// <summary>
		/// Hosting the game failed.
		/// </summary>
		// Token: 0x04002CAC RID: 11436
		FailedToHost,
		/// <summary>
		/// Couldn't make secure connection (Steam BadCert and BadCrypt). Most common cause is out-of-sync clocks.
		/// </summary>
		// Token: 0x04002CAD RID: 11437
		SecureConnectionFailed
	}
}
