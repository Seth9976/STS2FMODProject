using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000756 RID: 1878
	public sealed class EntropicBrew : PotionModel
	{
		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x06005DA2 RID: 23970 RVA: 0x00257F97 File Offset: 0x00256197
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x06005DA3 RID: 23971 RVA: 0x00257F9A File Offset: 0x0025619A
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.AnyTime;
			}
		}

		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x06005DA4 RID: 23972 RVA: 0x00257F9D File Offset: 0x0025619D
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x06005DA5 RID: 23973 RVA: 0x00257FA0 File Offset: 0x002561A0
		[NullableContext(1)]
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			Player targetPlayer = target.Player;
			while (targetPlayer.HasOpenPotionSlots)
			{
				PotionModel potionModel = PotionFactory.CreateRandomPotionOutOfCombat(targetPlayer, targetPlayer.RunState.Rng.CombatPotionGeneration, null).ToMutable();
				PotionProcureResult potionProcureResult = await PotionCmd.TryToProcure(potionModel, targetPlayer, -1);
				PotionProcureResult potionProcureResult2 = potionProcureResult;
				if (!potionProcureResult2.success)
				{
					break;
				}
			}
		}
	}
}
