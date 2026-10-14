using System;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CC7 RID: 3271
	public enum TargetType
	{
		// Token: 0x04002DFD RID: 11773
		None,
		/// <summary>Can only target self. No target selection.</summary>
		// Token: 0x04002DFE RID: 11774
		Self,
		/// <summary>Can target any enemy. Target selection is performed to determine the enemy.</summary>
		// Token: 0x04002DFF RID: 11775
		AnyEnemy,
		/// <summary>Targets all enemies. No target selection.</summary>
		// Token: 0x04002E00 RID: 11776
		AllEnemies,
		/// <summary>Targets all enemies. No target selection.</summary>
		// Token: 0x04002E01 RID: 11777
		RandomEnemy,
		/// <summary>
		/// Targets any player.
		/// In multiplayer, target selection is performed to determine the player.
		/// In singleplayer, no target selection is performed.
		/// </summary>
		// Token: 0x04002E02 RID: 11778
		AnyPlayer,
		/// <summary>
		/// Targets any ally (any player excluding itself).
		/// In multiplayer, target selection is performed to determine the player.
		/// You should not see this in singleplayer.
		/// </summary>
		// Token: 0x04002E03 RID: 11779
		AnyAlly,
		/// <summary>Targets all allies. No target selection.</summary>
		// Token: 0x04002E04 RID: 11780
		AllAllies,
		/// <summary>
		/// Target selection is performed, but no creature target is expected.
		/// Currently only used by <see cref="T:MegaCrit.Sts2.Core.Models.Potions.FoulPotion" /> to target the merchant, which is not a creature.
		/// </summary>
		// Token: 0x04002E05 RID: 11781
		TargetedNoCreature,
		/// <summary>Targets the local player's Osty</summary>
		// Token: 0x04002E06 RID: 11782
		Osty
	}
}
