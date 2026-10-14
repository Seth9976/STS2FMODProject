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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009FB RID: 2555
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HelixDrill : CardModel
	{
		// Token: 0x060072E7 RID: 29415 RVA: 0x0028E49B File Offset: 0x0028C69B
		public HelixDrill()
			: base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E85 RID: 7813
		// (get) Token: 0x060072E8 RID: 29416 RVA: 0x0028E4A8 File Offset: 0x0028C6A8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[4];
				array[0] = new DamageVar(3m, ValueProp.Move);
				array[1] = new CalculationBaseVar(0m);
				array[2] = new CalculationExtraVar(1m);
				array[3] = new CalculatedVar("CalculatedHits").WithMultiplier(delegate(CardModel card, [Nullable(2)] Creature _)
				{
					if (card.Pile == null)
					{
						return 0m;
					}
					int num = (from e in CombatManager.Instance.History.Entries.OfType<EnergySpentEntry>()
						where e.HappenedThisTurn(card.CombatState) && e.Actor.Player == card.Owner
						select e).Sum<EnergySpentEntry>((EnergySpentEntry c) => c.Amount);
					if (card.Pile.Type == PileType.Play)
					{
						num -= card.EnergyCost.GetWithModifiers(CostModifiers.All);
					}
					return num;
				});
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x17001E86 RID: 7814
		// (get) Token: 0x060072E9 RID: 29417 RVA: 0x0028E51A File Offset: 0x0028C71A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x060072EA RID: 29418 RVA: 0x0028E528 File Offset: 0x0028C728
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount((int)((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x060072EB RID: 29419 RVA: 0x0028E57B File Offset: 0x0028C77B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}

		// Token: 0x0400287F RID: 10367
		private const string _calculatedHitsKey = "CalculatedHits";
	}
}
