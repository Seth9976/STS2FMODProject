using System;

namespace MegaCrit.Sts2.Core.Runs
{
	/// <summary>
	/// Various flags which can be passed to the CardCreationOptions to control card generation behavior.
	/// This controls both built-in behavior (rolling for upgrades in rewards) as well as hook behavior (Molten Egg, Ruby
	/// Earrings).
	/// </summary>
	// Token: 0x020000FE RID: 254
	[Flags]
	public enum CardCreationFlags
	{
		/// <summary>
		/// Indicates that rarity should not be modified.
		/// This should be used when an event option specifies the rarity. For example, <see cref="T:MegaCrit.Sts2.Core.Models.Events.RoomFullOfCheese" />
		/// specifies that its rewards are uncommon, so no relics should modify the rarities.
		/// </summary>
		// Token: 0x040003D9 RID: 985
		NoRarityModification = 1,
		/// <summary>
		/// Indicates that the built-in reward upgrade should not be applied.
		/// When offering combat rewards, there is a chance that cards are offered upgraded. This should not apply to most
		/// event rewards, but it does for some other rewards like those offered from <see cref="T:MegaCrit.Sts2.Core.Models.Relics.Orrery" />.
		/// </summary>
		// Token: 0x040003DA RID: 986
		NoUpgradeRoll = 2,
		/// <summary>
		/// Indicates that all modify hooks that upgrade cards should not apply their effect.
		/// Applies to things like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.MoltenEgg" />.
		/// </summary>
		// Token: 0x040003DB RID: 987
		NoHookUpgrades = 4,
		/// <summary>
		/// Indicates that no modify hooks should be run.
		/// This is used when a modify hook itself generates a card, to prevent infinite looping. For example,
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Relics.LastingCandy" /> uses this when generating new rewards so that it itself doesn't apply to the reward
		/// it generates.
		/// </summary>
		// Token: 0x040003DC RID: 988
		NoModifyHooks = 8,
		/// <summary>
		/// Indicates that the card pool passed to the reward generation should not be modified.
		/// Relics like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.PrismaticGem" /> can modify the reward pool passed into the reward generation. However,
		/// relics like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.LastingCandy" /> generate a card from a pool of only powers, which should not be modified
		/// by hooks.
		/// </summary>
		// Token: 0x040003DD RID: 989
		NoCardPoolModifications = 16,
		/// <summary>
		/// Indicates that the cards passed to the reward generation should not be modified.
		/// FTUE can manually set the cards that are offered to the player. <see cref="T:MegaCrit.Sts2.Core.Models.Relics.SilverCrucible" /> should be allowed
		/// to upgraded these cards (the player can get it if they're playing their first run in multiplayer), but Prismatic
		/// Shard should not be allowed to change the cards.
		/// </summary>
		// Token: 0x040003DE RID: 990
		NoCardModelModifications = 32,
		/// <summary>
		/// Indicates that the odds of rolling rare cards should be modified and that we should use the modified odds, even
		/// if this is not an encounter roll.
		/// </summary>
		// Token: 0x040003DF RID: 991
		ForceRarityOddsChange = 64,
		/// <summary>
		/// Indicates that the cards created other specifically for a card reward. It is explicitly added in  <see cref="T:MegaCrit.Sts2.Core.Rewards.CardReward" />.
		/// Important for relics like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.PrismaticGem" /> or <see cref="T:MegaCrit.Sts2.Core.Models.Relics.DingyRug" /> that only affect card rewards.
		/// </summary>
		// Token: 0x040003E0 RID: 992
		IsCardReward = 128,
		/// <summary>
		/// Indicates that the cards created are specifically for a card reward from a combat.
		/// Important for relics like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.LastingCandy" /> that should only apply to combat card rewards.
		/// </summary>
		// Token: 0x040003E1 RID: 993
		IsFromCombat = 256,
		/// <summary>
		/// Indicates that no upgrades should be applied to the generated card at all.
		/// </summary>
		// Token: 0x040003E2 RID: 994
		NoUpgrades = 6,
		/// <summary>
		/// Specifies that the pool and cards generated may not be modified.
		/// </summary>
		// Token: 0x040003E3 RID: 995
		NoModifications = -1
	}
}
