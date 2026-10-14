using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009B2 RID: 2482
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ExpectAFight : CardModel
	{
		// Token: 0x06007156 RID: 29014 RVA: 0x0028B01B File Offset: 0x0028921B
		public ExpectAFight()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001DDA RID: 7642
		// (get) Token: 0x06007157 RID: 29015 RVA: 0x0028B028 File Offset: 0x00289228
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[4];
				array[0] = new EnergyVar(0);
				array[1] = new CalculationBaseVar(0m);
				array[2] = new CalculationExtraVar(1m);
				array[3] = new CalculatedVar("CalculatedEnergy").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => PileType.Hand.GetPile(card.Owner).Cards.Count<CardModel>((CardModel c) => c.Type == CardType.Attack));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x17001DDB RID: 7643
		// (get) Token: 0x06007158 RID: 29016 RVA: 0x0028B094 File Offset: 0x00289294
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06007159 RID: 29017 RVA: 0x0028B0A4 File Offset: 0x002892A4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PlayerCmd.GainEnergy(((CalculatedVar)base.DynamicVars["CalculatedEnergy"]).Calculate(cardPlay.Target), base.Owner);
		}

		// Token: 0x0600715A RID: 29018 RVA: 0x0028B0EF File Offset: 0x002892EF
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}

		// Token: 0x04002868 RID: 10344
		private const string _calculatedEnergyKey = "CalculatedEnergy";
	}
}
