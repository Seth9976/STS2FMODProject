using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000754 RID: 1876
	public sealed class Duplicator : PotionModel
	{
		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x06005D96 RID: 23958 RVA: 0x00257EC3 File Offset: 0x002560C3
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x06005D97 RID: 23959 RVA: 0x00257EC6 File Offset: 0x002560C6
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x06005D98 RID: 23960 RVA: 0x00257EC9 File Offset: 0x002560C9
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x06005D99 RID: 23961 RVA: 0x00257ECC File Offset: 0x002560CC
		[NullableContext(1)]
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await PowerCmd.Apply<DuplicationPower>(choiceContext, target, 1m, target, null, false);
		}
	}
}
