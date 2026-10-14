using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A36 RID: 2614
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MasterPlanner : CardModel
	{
		// Token: 0x0600742E RID: 29742 RVA: 0x00290FC3 File Offset: 0x0028F1C3
		public MasterPlanner()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F0C RID: 7948
		// (get) Token: 0x0600742F RID: 29743 RVA: 0x00290FD0 File Offset: 0x0028F1D0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Sly));
			}
		}

		// Token: 0x06007430 RID: 29744 RVA: 0x00290FE0 File Offset: 0x0028F1E0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<MasterPlannerPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x06007431 RID: 29745 RVA: 0x0029102B File Offset: 0x0028F22B
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
