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
	// Token: 0x02000A4A RID: 2634
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MonarchsGaze : CardModel
	{
		// Token: 0x0600749E RID: 29854 RVA: 0x00291E0B File Offset: 0x0029000B
		public MonarchsGaze()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F3A RID: 7994
		// (get) Token: 0x0600749F RID: 29855 RVA: 0x00291E18 File Offset: 0x00290018
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("StrengthLoss", 1m));
			}
		}

		// Token: 0x17001F3B RID: 7995
		// (get) Token: 0x060074A0 RID: 29856 RVA: 0x00291E30 File Offset: 0x00290030
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x060074A1 RID: 29857 RVA: 0x00291E50 File Offset: 0x00290050
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<MonarchsGazePower>(choiceContext, base.Owner.Creature, base.DynamicVars["StrengthLoss"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060074A2 RID: 29858 RVA: 0x00291E9B File Offset: 0x0029009B
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}

		// Token: 0x040028A4 RID: 10404
		private const string _strengthLossKey = "StrengthLoss";
	}
}
