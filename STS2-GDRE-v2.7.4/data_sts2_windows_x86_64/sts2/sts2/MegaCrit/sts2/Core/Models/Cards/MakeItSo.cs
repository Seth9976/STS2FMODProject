using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A31 RID: 2609
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MakeItSo : CardModel
	{
		// Token: 0x06007412 RID: 29714 RVA: 0x00290BF0 File Offset: 0x0028EDF0
		public MakeItSo()
			: base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F00 RID: 7936
		// (get) Token: 0x06007413 RID: 29715 RVA: 0x00290BFD File Offset: 0x0028EDFD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(6m, ValueProp.Move),
					new CardsVar(3)
				});
			}
		}

		// Token: 0x06007414 RID: 29716 RVA: 0x00290C24 File Offset: 0x0028EE24
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_starry_impact", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06007415 RID: 29717 RVA: 0x00290C78 File Offset: 0x0028EE78
		public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (cardPlay.Card.Type == CardType.Skill)
				{
					if (base.Pile.Type != PileType.Hand)
					{
						int num = CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((CardPlayFinishedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Card.Type == CardType.Skill && e.CardPlay.Player == base.Owner);
						if (num % base.DynamicVars.Cards.IntValue == 0)
						{
							await CardPileCmd.Add(this, PileType.Hand, CardPilePosition.Bottom, null, false);
						}
					}
				}
			}
		}

		// Token: 0x06007416 RID: 29718 RVA: 0x00290CC3 File Offset: 0x0028EEC3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
