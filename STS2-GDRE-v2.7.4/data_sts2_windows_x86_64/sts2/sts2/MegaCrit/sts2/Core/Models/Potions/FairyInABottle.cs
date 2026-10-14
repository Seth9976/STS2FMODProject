using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x02000759 RID: 1881
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FairyInABottle : PotionModel
	{
		// Token: 0x1700150F RID: 5391
		// (get) Token: 0x06005DB3 RID: 23987 RVA: 0x002580DB File Offset: 0x002562DB
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x17001510 RID: 5392
		// (get) Token: 0x06005DB4 RID: 23988 RVA: 0x002580DE File Offset: 0x002562DE
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.Automatic;
			}
		}

		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x06005DB5 RID: 23989 RVA: 0x002580E1 File Offset: 0x002562E1
		public override TargetType TargetType
		{
			get
			{
				return TargetType.Self;
			}
		}

		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x06005DB6 RID: 23990 RVA: 0x002580E4 File Offset: 0x002562E4
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// This should never be executed directly by <see cref="T:MegaCrit.Sts2.Core.GameActions.UsePotionAction" />.
		/// It is only triggered by <see cref="M:MegaCrit.Sts2.Core.Models.Potions.FairyInABottle.AfterPreventingDeath(MegaCrit.Sts2.Core.Entities.Creatures.Creature)" />.
		/// </summary>
		/// <param name="choiceContext"></param>
		/// <param name="target"></param>
		// Token: 0x06005DB7 RID: 23991 RVA: 0x002580E8 File Offset: 0x002562E8
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			await CreatureCmd.Heal(target, Math.Max(target.MaxHp * 0.3m, 1m), true);
		}

		// Token: 0x06005DB8 RID: 23992 RVA: 0x0025812B File Offset: 0x0025632B
		public override bool ShouldDie(Creature creature)
		{
			return creature != base.Owner.Creature;
		}

		// Token: 0x06005DB9 RID: 23993 RVA: 0x00258140 File Offset: 0x00256340
		public override async Task AfterPreventingDeath(Creature creature)
		{
			await base.OnUseWrapper(new ThrowingPlayerChoiceContext(), creature);
		}
	}
}
