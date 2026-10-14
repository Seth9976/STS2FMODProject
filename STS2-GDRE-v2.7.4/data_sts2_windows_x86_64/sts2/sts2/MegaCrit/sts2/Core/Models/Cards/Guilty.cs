using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009ED RID: 2541
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Guilty : CardModel
	{
		// Token: 0x0600729E RID: 29342 RVA: 0x0028DB27 File Offset: 0x0028BD27
		public Guilty()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001E67 RID: 7783
		// (get) Token: 0x0600729F RID: 29343 RVA: 0x0028DB35 File Offset: 0x0028BD35
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001E68 RID: 7784
		// (get) Token: 0x060072A0 RID: 29344 RVA: 0x0028DB38 File Offset: 0x0028BD38
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Combats", 5m));
			}
		}

		// Token: 0x17001E69 RID: 7785
		// (get) Token: 0x060072A1 RID: 29345 RVA: 0x0028DB4F File Offset: 0x0028BD4F
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x17001E6A RID: 7786
		// (get) Token: 0x060072A2 RID: 29346 RVA: 0x0028DB57 File Offset: 0x0028BD57
		// (set) Token: 0x060072A3 RID: 29347 RVA: 0x0028DB5F File Offset: 0x0028BD5F
		[SavedProperty]
		public int CombatsSeen
		{
			get
			{
				return this._combatsSeen;
			}
			set
			{
				base.AssertMutable();
				this._combatsSeen = value;
				base.DynamicVars["Combats"].BaseValue = 5 - this.CombatsSeen;
			}
		}

		// Token: 0x060072A4 RID: 29348 RVA: 0x0028DB90 File Offset: 0x0028BD90
		public override async Task AfterCombatEnd(CombatRoom _)
		{
			CardPile pile = base.Pile;
			if (pile != null && pile.Type == PileType.Deck)
			{
				int combatsSeen = this.CombatsSeen;
				this.CombatsSeen = combatsSeen + 1;
				if (this.CombatsSeen >= 5 && base.Pile.Type == PileType.Deck)
				{
					await CardPileCmd.RemoveFromDeck(this, true);
				}
			}
		}

		// Token: 0x0400287A RID: 10362
		public const int maxCombats = 5;

		// Token: 0x0400287B RID: 10363
		private const string _combatsKey = "Combats";

		// Token: 0x0400287C RID: 10364
		private int _combatsSeen;
	}
}
