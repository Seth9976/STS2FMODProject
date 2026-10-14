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
	// Token: 0x02000AB8 RID: 2744
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SentryMode : CardModel
	{
		// Token: 0x060076DC RID: 30428 RVA: 0x0029688F File Offset: 0x00294A8F
		public SentryMode()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002026 RID: 8230
		// (get) Token: 0x060076DD RID: 30429 RVA: 0x0029689C File Offset: 0x00294A9C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SentryModePower>(1m));
			}
		}

		// Token: 0x17002027 RID: 8231
		// (get) Token: 0x060076DE RID: 30430 RVA: 0x002968AD File Offset: 0x00294AAD
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<SweepingGaze>(false));
			}
		}

		// Token: 0x060076DF RID: 30431 RVA: 0x002968BC File Offset: 0x00294ABC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<SentryModePower>(choiceContext, base.Owner.Creature, base.DynamicVars["SentryModePower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060076E0 RID: 30432 RVA: 0x00296907 File Offset: 0x00294B07
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
