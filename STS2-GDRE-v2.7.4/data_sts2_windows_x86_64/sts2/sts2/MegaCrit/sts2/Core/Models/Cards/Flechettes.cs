using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009CB RID: 2507
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Flechettes : CardModel
	{
		// Token: 0x060071E4 RID: 29156 RVA: 0x0028C3E1 File Offset: 0x0028A5E1
		public Flechettes()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E16 RID: 7702
		// (get) Token: 0x060071E5 RID: 29157 RVA: 0x0028C3F0 File Offset: 0x0028A5F0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[4];
				array[0] = new DamageVar(5m, ValueProp.Move);
				array[1] = new CalculationBaseVar(0m);
				array[2] = new CalculationExtraVar(1m);
				array[3] = new CalculatedVar("CalculatedHits").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => PileType.Hand.GetPile(card.Owner).Cards.Count<CardModel>((CardModel c) => c.Type == CardType.Skill));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x060071E6 RID: 29158 RVA: 0x0028C464 File Offset: 0x0028A664
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount((int)((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, "dagger_throw.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x060071E7 RID: 29159 RVA: 0x0028C4B7 File Offset: 0x0028A6B7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}

		// Token: 0x04002871 RID: 10353
		private const string _calculatedHitsKey = "CalculatedHits";
	}
}
