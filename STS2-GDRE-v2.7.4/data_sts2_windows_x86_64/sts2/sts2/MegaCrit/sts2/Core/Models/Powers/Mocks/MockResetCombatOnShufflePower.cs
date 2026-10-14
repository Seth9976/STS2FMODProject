using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x0200073D RID: 1853
	public sealed class MockResetCombatOnShufflePower : PowerModel
	{
		// Token: 0x170014A1 RID: 5281
		// (get) Token: 0x06005D08 RID: 23816 RVA: 0x00257483 File Offset: 0x00255683
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014A2 RID: 5282
		// (get) Token: 0x06005D09 RID: 23817 RVA: 0x00257486 File Offset: 0x00255686
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170014A3 RID: 5283
		// (get) Token: 0x06005D0A RID: 23818 RVA: 0x00257489 File Offset: 0x00255689
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170014A4 RID: 5284
		// (get) Token: 0x06005D0B RID: 23819 RVA: 0x0025748C File Offset: 0x0025568C
		// (set) Token: 0x06005D0C RID: 23820 RVA: 0x00257494 File Offset: 0x00255694
		private bool HasReset
		{
			get
			{
				return this._hasReset;
			}
			set
			{
				base.AssertMutable();
				this._hasReset = value;
			}
		}

		// Token: 0x06005D0D RID: 23821 RVA: 0x002574A4 File Offset: 0x002556A4
		[NullableContext(1)]
		public override Task AfterCardChangedPiles(CardModel card, PileType oldPileType, [Nullable(2)] AbstractModel clonedBy)
		{
			if (!this.HasReset && oldPileType == PileType.Discard)
			{
				CardPile pile = card.Pile;
				if (pile != null && pile.Type == PileType.Draw)
				{
					this.HasReset = true;
					CombatManager.Instance.Reset(true);
					return Task.CompletedTask;
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x0400252C RID: 9516
		private bool _hasReset;
	}
}
