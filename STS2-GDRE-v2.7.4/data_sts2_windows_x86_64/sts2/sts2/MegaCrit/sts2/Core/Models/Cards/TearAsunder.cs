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
	// Token: 0x02000B08 RID: 2824
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TearAsunder : CardModel
	{
		// Token: 0x06007894 RID: 30868 RVA: 0x0029A207 File Offset: 0x00298407
		public TearAsunder()
			: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020D9 RID: 8409
		// (get) Token: 0x06007895 RID: 30869 RVA: 0x0029A214 File Offset: 0x00298414
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[5];
				array[0] = new DamageVar(5m, ValueProp.Move);
				array[1] = new RepeatVar(1);
				array[2] = new CalculationBaseVar(0m);
				array[3] = new CalculationExtraVar(1m);
				array[4] = new CalculatedVar("CalculatedHits").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => 1 + CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Count<DamageReceivedEntry>((DamageReceivedEntry e) => e.Receiver == card.Owner.Creature && e.Result.UnblockedDamage > 0));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x06007896 RID: 30870 RVA: 0x0029A290 File Offset: 0x00298490
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount((int)((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06007897 RID: 30871 RVA: 0x0029A2E3 File Offset: 0x002984E3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}

		// Token: 0x040028CC RID: 10444
		private const string _calculatedHitsKey = "CalculatedHits";
	}
}
