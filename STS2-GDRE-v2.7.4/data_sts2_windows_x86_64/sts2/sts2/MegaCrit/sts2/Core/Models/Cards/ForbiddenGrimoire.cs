using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009D0 RID: 2512
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ForbiddenGrimoire : CardModel
	{
		// Token: 0x060071FC RID: 29180 RVA: 0x0028C71E File Offset: 0x0028A91E
		public ForbiddenGrimoire()
			: base(2, CardType.Power, CardRarity.Ancient, TargetType.Self, true)
		{
		}

		// Token: 0x17001E21 RID: 7713
		// (get) Token: 0x060071FD RID: 29181 RVA: 0x0028C72B File Offset: 0x0028A92B
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Eternal);
			}
		}

		// Token: 0x060071FE RID: 29182 RVA: 0x0028C734 File Offset: 0x0028A934
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<ForbiddenGrimoirePower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x060071FF RID: 29183 RVA: 0x0028C77F File Offset: 0x0028A97F
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
