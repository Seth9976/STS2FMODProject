using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Afflictions;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200072A RID: 1834
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VitalSparkPower : PowerModel
	{
		// Token: 0x17001460 RID: 5216
		// (get) Token: 0x06005C85 RID: 23685 RVA: 0x002567F7 File Offset: 0x002549F7
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001461 RID: 5217
		// (get) Token: 0x06005C86 RID: 23686 RVA: 0x002567FA File Offset: 0x002549FA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001462 RID: 5218
		// (get) Token: 0x06005C87 RID: 23687 RVA: 0x002567FD File Offset: 0x002549FD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StringVar("AfflictionTitle", ModelDb.Affliction<Tainted>().Title.GetFormattedText()));
			}
		}

		// Token: 0x17001463 RID: 5219
		// (get) Token: 0x06005C88 RID: 23688 RVA: 0x0025681D File Offset: 0x00254A1D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromAffliction<Tainted>(base.Amount);
			}
		}

		// Token: 0x06005C89 RID: 23689 RVA: 0x0025682C File Offset: 0x00254A2C
		public override async Task BeforeCombatStart()
		{
			foreach (Creature creature in base.Owner.CombatState.Allies.ToList<Creature>())
			{
				if (creature.IsPlayer)
				{
					IEnumerable<CardModel> enumerable = creature.Player.PlayerCombatState.AllCards.Where<CardModel>((CardModel c) => c.Type == CardType.Skill);
					foreach (CardModel cardModel in enumerable)
					{
						await CardCmd.Afflict<Tainted>(cardModel, base.Amount);
					}
					IEnumerator<CardModel> enumerator2 = null;
				}
			}
			List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
		}

		// Token: 0x06005C8A RID: 23690 RVA: 0x00256870 File Offset: 0x00254A70
		public override async Task AfterCardEnteredCombat(CardModel card)
		{
			if (card.Affliction == null)
			{
				if (card.Type == CardType.Skill)
				{
					await CardCmd.Afflict<Tainted>(card, base.Amount);
				}
			}
		}

		// Token: 0x06005C8B RID: 23691 RVA: 0x002568BC File Offset: 0x00254ABC
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Affliction is Tainted)
			{
				base.Flash();
				await PowerCmd.Apply<TaintedPower>(choiceContext, cardPlay.Card.Owner.Creature, base.Amount, null, null, false);
			}
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x00256910 File Offset: 0x00254B10
		public override Task AfterRemoved(Creature oldOwner)
		{
			if (oldOwner.CombatState == null)
			{
				return Task.CompletedTask;
			}
			foreach (Creature creature in oldOwner.CombatState.Allies.ToList<Creature>())
			{
				if (creature.IsPlayer)
				{
					IEnumerable<CardModel> enumerable = creature.Player.PlayerCombatState.AllCards.Where<CardModel>((CardModel c) => c.Affliction is Tainted);
					foreach (CardModel cardModel in enumerable)
					{
						CardCmd.ClearAffliction(cardModel);
					}
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005C8D RID: 23693 RVA: 0x002569F0 File Offset: 0x00254BF0
		public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource)
		{
			if (power != this)
			{
				return Task.CompletedTask;
			}
			foreach (Creature creature in base.Owner.CombatState.Allies.ToList<Creature>())
			{
				if (creature.IsPlayer)
				{
					IEnumerable<CardModel> enumerable = creature.Player.PlayerCombatState.AllCards.Where<CardModel>((CardModel c) => c.Affliction is Tainted);
					foreach (CardModel cardModel in enumerable)
					{
						cardModel.Affliction.Amount = base.Amount;
					}
				}
			}
			return Task.CompletedTask;
		}
	}
}
