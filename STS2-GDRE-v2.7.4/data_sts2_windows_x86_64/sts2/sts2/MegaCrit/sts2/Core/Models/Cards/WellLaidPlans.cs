using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B38 RID: 2872
	public sealed class WellLaidPlans : CardModel
	{
		// Token: 0x06007998 RID: 31128 RVA: 0x0029C16B File Offset: 0x0029A36B
		public WellLaidPlans()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x06007999 RID: 31129 RVA: 0x0029C178 File Offset: 0x0029A378
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}

		// Token: 0x0600799A RID: 31130 RVA: 0x0029C188 File Offset: 0x0029A388
		[NullableContext(1)]
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<WellLaidPlansPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}
	}
}
