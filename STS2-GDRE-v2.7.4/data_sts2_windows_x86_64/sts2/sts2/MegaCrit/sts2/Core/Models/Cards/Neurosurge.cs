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
	// Token: 0x02000A51 RID: 2641
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Neurosurge : CardModel
	{
		// Token: 0x060074C1 RID: 29889 RVA: 0x002922B0 File Offset: 0x002904B0
		public Neurosurge()
			: base(0, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F49 RID: 8009
		// (get) Token: 0x060074C2 RID: 29890 RVA: 0x002922BD File Offset: 0x002904BD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<NeurosurgePower>(3m),
					new EnergyVar(3),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x17001F4A RID: 8010
		// (get) Token: 0x060074C3 RID: 29891 RVA: 0x002922EC File Offset: 0x002904EC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<DoomPower>(null),
					base.EnergyHoverTip
				});
			}
		}

		// Token: 0x060074C4 RID: 29892 RVA: 0x00292320 File Offset: 0x00290520
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			await PowerCmd.Apply<NeurosurgePower>(choiceContext, base.Owner.Creature, base.DynamicVars["NeurosurgePower"].IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060074C5 RID: 29893 RVA: 0x0029236B File Offset: 0x0029056B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
