using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200090E RID: 2318
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Barrage : CardModel
	{
		// Token: 0x06006DFD RID: 28157 RVA: 0x002846AC File Offset: 0x002828AC
		public Barrage()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001C65 RID: 7269
		// (get) Token: 0x06006DFE RID: 28158 RVA: 0x002846B9 File Offset: 0x002828B9
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17001C66 RID: 7270
		// (get) Token: 0x06006DFF RID: 28159 RVA: 0x002846CC File Offset: 0x002828CC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[4];
				array[0] = new DamageVar(5m, ValueProp.Move);
				array[1] = new CalculationBaseVar(0m);
				array[2] = new CalculationExtraVar(1m);
				array[3] = new CalculatedVar("CalculatedHits").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => card.Owner.PlayerCombatState.OrbQueue.Orbs.Count);
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x06006E00 RID: 28160 RVA: 0x00284740 File Offset: 0x00282940
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount((int)((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06006E01 RID: 28161 RVA: 0x00284793 File Offset: 0x00282993
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}

		// Token: 0x04002847 RID: 10311
		private const string _calculatedHitsKey = "CalculatedHits";
	}
}
