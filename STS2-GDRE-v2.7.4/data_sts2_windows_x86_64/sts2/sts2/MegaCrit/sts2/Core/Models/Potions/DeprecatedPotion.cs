using System;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	/// <summary>
	/// Represents a potion that has been removed from the game. Mostly used for the run history.
	/// </summary>
	// Token: 0x02000750 RID: 1872
	public sealed class DeprecatedPotion : PotionModel
	{
		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x06005D80 RID: 23936 RVA: 0x00257D43 File Offset: 0x00255F43
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.None;
			}
		}

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x06005D81 RID: 23937 RVA: 0x00257D46 File Offset: 0x00255F46
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x06005D82 RID: 23938 RVA: 0x00257D49 File Offset: 0x00255F49
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyEnemy;
			}
		}
	}
}
