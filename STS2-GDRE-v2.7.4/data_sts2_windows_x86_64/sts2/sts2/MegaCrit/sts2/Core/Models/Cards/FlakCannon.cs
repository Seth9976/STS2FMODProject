using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009C6 RID: 2502
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FlakCannon : CardModel
	{
		// Token: 0x060071C5 RID: 29125 RVA: 0x0028BF7F File Offset: 0x0028A17F
		public FlakCannon()
			: base(2, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy, true)
		{
		}

		// Token: 0x17001E0B RID: 7691
		// (get) Token: 0x060071C6 RID: 29126 RVA: 0x0028BF8C File Offset: 0x0028A18C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[4];
				array[0] = new DamageVar(8m, ValueProp.Move);
				array[1] = new CalculationBaseVar(0m);
				array[2] = new CalculationExtraVar(1m);
				array[3] = new CalculatedVar("CalculatedHits").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => FlakCannon.GetStatuses(card.Owner).Count<CardModel>());
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x17001E0C RID: 7692
		// (get) Token: 0x060071C7 RID: 29127 RVA: 0x0028BFFE File Offset: 0x0028A1FE
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x060071C8 RID: 29128 RVA: 0x0028C00C File Offset: 0x0028A20C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			List<CardModel> list = FlakCannon.GetStatuses(base.Owner).ToList<CardModel>();
			int statusCount = (int)((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target);
			foreach (CardModel cardModel in list)
			{
				await CardCmd.Exhaust(choiceContext, cardModel, false, false);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(statusCount).FromCard(this, cardPlay)
				.TargetingRandomOpponents(base.CombatState, true)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x060071C9 RID: 29129 RVA: 0x0028C05F File Offset: 0x0028A25F
		private static IEnumerable<CardModel> GetStatuses(Player owner)
		{
			return owner.PlayerCombatState.AllCards.Where<CardModel>((CardModel c) => c.Type == CardType.Status && c.Pile.Type != PileType.Exhaust);
		}

		// Token: 0x060071CA RID: 29130 RVA: 0x0028C090 File Offset: 0x0028A290
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}

		// Token: 0x0400286F RID: 10351
		private const string _calculatedHitsKey = "CalculatedHits";
	}
}
