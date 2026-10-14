using System;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	/// <summary>
	/// Sent in the InitialGameInfoMessage. If a client receives this, then they will be disconnected shortly after receiving
	/// the message, and they should show a relevant error.
	/// This is used specifically during connection. See DisconnectionReason for generic disconnection reasons.
	/// </summary>
	// Token: 0x02000C82 RID: 3202
	public enum ConnectionFailureReason
	{
		// Token: 0x04002C7E RID: 11390
		None,
		/// <summary>
		/// The lobby has reached the maximum number of players.
		/// </summary>
		// Token: 0x04002C7F RID: 11391
		LobbyFull,
		/// <summary>
		/// The host loaded from a save file and the client attempting to join is not one of the players in the save.
		/// </summary>
		// Token: 0x04002C80 RID: 11392
		NotInSaveGame,
		/// <summary>
		/// The run is already in progress and the client attempting to join is not one of the players in the run.
		/// </summary>
		// Token: 0x04002C81 RID: 11393
		RunInProgress,
		/// <summary>
		/// The host's game version does not match ours.
		/// </summary>
		// Token: 0x04002C82 RID: 11394
		VersionMismatch,
		/// <summary>
		/// Either the host has mods that we don't have, or we have mods that the host doesn't have.
		/// </summary>
		// Token: 0x04002C83 RID: 11395
		ModMismatch
	}
}
