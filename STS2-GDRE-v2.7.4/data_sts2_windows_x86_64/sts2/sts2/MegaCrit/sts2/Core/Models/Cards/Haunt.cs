using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009F4 RID: 2548
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Haunt : CardModel
	{
		// Token: 0x060072C4 RID: 29380 RVA: 0x0028DF87 File Offset: 0x0028C187
		public Haunt()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E78 RID: 7800
		// (get) Token: 0x060072C5 RID: 29381 RVA: 0x0028DF94 File Offset: 0x0028C194
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HpLossVar(7m));
			}
		}

		// Token: 0x17001E79 RID: 7801
		// (get) Token: 0x060072C6 RID: 29382 RVA: 0x0028DFA6 File Offset: 0x0028C1A6
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>(false));
			}
		}

		// Token: 0x060072C7 RID: 29383 RVA: 0x0028DFB4 File Offset: 0x0028C1B4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<HauntPower>(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060072C8 RID: 29384 RVA: 0x0028DFFF File Offset: 0x0028C1FF
		protected override void OnUpgrade()
		{
			base.DynamicVars.HpLoss.UpgradeValueBy(2m);
		}
	}
}
