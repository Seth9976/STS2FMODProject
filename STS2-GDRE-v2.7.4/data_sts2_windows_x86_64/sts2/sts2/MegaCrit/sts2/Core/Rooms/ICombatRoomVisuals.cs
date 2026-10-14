using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Rooms
{
	// Token: 0x0200012B RID: 299
	[NullableContext(1)]
	public interface ICombatRoomVisuals
	{
		/// <summary>
		/// The encounter (for background, scene/slots).
		/// </summary>
		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06000E93 RID: 3731
		EncounterModel Encounter { get; }

		/// <summary>
		/// Creatures to display on the ally side.
		/// </summary>
		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06000E94 RID: 3732
		IEnumerable<Creature> Allies { get; }

		/// <summary>
		/// Creatures to display on the enemy side.
		/// </summary>
		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06000E95 RID: 3733
		IEnumerable<Creature> Enemies { get; }

		/// <summary>
		/// The act this combat room is being shown in.
		/// Used to get a fallback background scene if the encounter doesn't have one.
		/// </summary>
		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06000E96 RID: 3734
		ActModel Act { get; }
	}
}
