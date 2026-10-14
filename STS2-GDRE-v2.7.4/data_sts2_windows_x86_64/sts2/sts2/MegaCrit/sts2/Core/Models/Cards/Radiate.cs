using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A8D RID: 2701
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Radiate : CardModel
	{
		// Token: 0x060075F3 RID: 30195 RVA: 0x00294B27 File Offset: 0x00292D27
		public Radiate()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001FC5 RID: 8133
		// (get) Token: 0x060075F4 RID: 30196 RVA: 0x00294B34 File Offset: 0x00292D34
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[5];
				array[0] = new DamageVar(3m, ValueProp.Move);
				array[1] = new StarsVar(1);
				array[2] = new CalculationBaseVar(0m);
				array[3] = new CalculationExtraVar(1m);
				array[4] = new CalculatedVar("CalculatedHits").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => (from e in CombatManager.Instance.History.Entries.OfType<StarsModifiedEntry>()
					where e.HappenedThisTurn(card.CombatState) && e.Amount > 0 && e.Actor == card.Owner.Creature
					select e).Sum<StarsModifiedEntry>((StarsModifiedEntry e) => e.Amount));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x060075F5 RID: 30197 RVA: 0x00294BB0 File Offset: 0x00292DB0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount((int)((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard(this, cardPlay)
				.TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_starry_impact", null, "slash_attack.mp3")
				.SpawningHitVfxOnEachCreature()
				.Execute(choiceContext);
		}

		// Token: 0x060075F6 RID: 30198 RVA: 0x00294C03 File Offset: 0x00292E03
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
		}

		// Token: 0x040028B2 RID: 10418
		private const string _calculatedHitsKey = "CalculatedHits";
	}
}
