using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Runs
{
	/// <summary>
	/// Interface for a collection of players. Primarily used to allow mocking the player collection for testing, so we don't
	/// need to create an entire run instance to test parts of the code.
	/// </summary>
	// Token: 0x02000106 RID: 262
	[NullableContext(1)]
	public interface IPlayerCollection
	{
		/// <summary>
		/// Players that are present.
		/// </summary>
		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000B43 RID: 2883
		IReadOnlyList<Player> Players { get; }

		/// <returns>The slot index of the player, or -1 if the player is not in Players.</returns>
		// Token: 0x06000B44 RID: 2884
		int GetPlayerSlotIndex(Player player);

		/// <returns>The player who has player ID playerId, or null if there is no player in Players.</returns>
		// Token: 0x06000B45 RID: 2885
		[NullableContext(2)]
		Player GetPlayer(ulong netId);
	}
}
