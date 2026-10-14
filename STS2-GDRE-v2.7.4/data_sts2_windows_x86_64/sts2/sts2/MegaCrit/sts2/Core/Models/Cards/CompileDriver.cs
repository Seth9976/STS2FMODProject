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
	// Token: 0x0200095B RID: 2395
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CompileDriver : CardModel
	{
		// Token: 0x06006F82 RID: 28546 RVA: 0x0028789E File Offset: 0x00285A9E
		public CompileDriver()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D05 RID: 7429
		// (get) Token: 0x06006F83 RID: 28547 RVA: 0x002878AC File Offset: 0x00285AAC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[4];
				array[0] = new DamageVar(7m, ValueProp.Move);
				array[1] = new CalculationBaseVar(0m);
				array[2] = new CalculationExtraVar(1m);
				array[3] = new CalculatedVar("CalculatedCards").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => (from orb in card.Owner.PlayerCombatState.OrbQueue.Orbs
					group orb by orb.Id).Count<IGrouping<ModelId, OrbModel>>());
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x06006F84 RID: 28548 RVA: 0x00287920 File Offset: 0x00285B20
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			await CardPileCmd.Draw(choiceContext, ((CalculatedVar)base.DynamicVars["CalculatedCards"]).Calculate(cardPlay.Target), base.Owner, false);
		}

		// Token: 0x06006F85 RID: 28549 RVA: 0x00287973 File Offset: 0x00285B73
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}

		// Token: 0x04002856 RID: 10326
		private const string _calculatedCardsKey = "CalculatedCards";
	}
}
