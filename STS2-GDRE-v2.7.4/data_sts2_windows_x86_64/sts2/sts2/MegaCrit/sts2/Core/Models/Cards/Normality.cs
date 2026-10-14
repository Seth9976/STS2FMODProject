using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A56 RID: 2646
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Normality : CardModel
	{
		// Token: 0x060074DC RID: 29916 RVA: 0x00292697 File Offset: 0x00290897
		public Normality()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001F55 RID: 8021
		// (get) Token: 0x060074DD RID: 29917 RVA: 0x002926A5 File Offset: 0x002908A5
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return this.ShouldPreventCardPlay;
			}
		}

		// Token: 0x17001F56 RID: 8022
		// (get) Token: 0x060074DE RID: 29918 RVA: 0x002926AD File Offset: 0x002908AD
		private bool ShouldPreventCardPlay
		{
			get
			{
				return this.CardsPlayedThisTurn >= 3;
			}
		}

		// Token: 0x17001F57 RID: 8023
		// (get) Token: 0x060074DF RID: 29919 RVA: 0x002926BB File Offset: 0x002908BB
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001F58 RID: 8024
		// (get) Token: 0x060074E0 RID: 29920 RVA: 0x002926BE File Offset: 0x002908BE
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x17001F59 RID: 8025
		// (get) Token: 0x060074E1 RID: 29921 RVA: 0x002926C8 File Offset: 0x002908C8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(3m);
				array[1] = new CalculationExtraVar(-1m);
				array[2] = new CalculatedVar("CalculatedCards").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => Math.Min(3, ((Normality)card).CardsPlayedThisTurn));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x060074E2 RID: 29922 RVA: 0x0029272C File Offset: 0x0029092C
		public override bool ShouldPlay(CardModel card, AutoPlayType _)
		{
			if (card.Owner != base.Owner)
			{
				return true;
			}
			CardPile pile = base.Pile;
			return pile == null || pile.Type != PileType.Hand || !this.ShouldPreventCardPlay;
		}

		// Token: 0x17001F5A RID: 8026
		// (get) Token: 0x060074E3 RID: 29923 RVA: 0x00292763 File Offset: 0x00290963
		private int CardsPlayedThisTurn
		{
			get
			{
				return CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((CardPlayStartedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Player == base.Owner);
			}
		}

		// Token: 0x040028A8 RID: 10408
		private const int _numOfCardsPerTurn = 3;

		// Token: 0x040028A9 RID: 10409
		private const string _calculatedCardsKey = "CalculatedCards";
	}
}
