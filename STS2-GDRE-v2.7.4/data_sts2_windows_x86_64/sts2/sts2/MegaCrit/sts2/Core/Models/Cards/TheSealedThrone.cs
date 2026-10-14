using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B11 RID: 2833
	public sealed class TheSealedThrone : CardModel
	{
		// Token: 0x060078CC RID: 30924 RVA: 0x0029A992 File Offset: 0x00298B92
		public TheSealedThrone()
			: base(1, CardType.Power, CardRarity.Ancient, TargetType.Self, true)
		{
		}

		// Token: 0x170020EE RID: 8430
		// (get) Token: 0x060078CD RID: 30925 RVA: 0x0029A99F File Offset: 0x00298B9F
		public override int CanonicalStarCost
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060078CE RID: 30926 RVA: 0x0029A9A4 File Offset: 0x00298BA4
		[NullableContext(1)]
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<TheSealedThronePower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x060078CF RID: 30927 RVA: 0x0029A9EF File Offset: 0x00298BEF
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
