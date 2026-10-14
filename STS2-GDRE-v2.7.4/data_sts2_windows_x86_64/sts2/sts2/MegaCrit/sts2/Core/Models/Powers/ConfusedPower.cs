using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000647 RID: 1607
	public sealed class ConfusedPower : PowerModel
	{
		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x06005747 RID: 22343 RVA: 0x0024C98B File Offset: 0x0024AB8B
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x06005748 RID: 22344 RVA: 0x0024C98E File Offset: 0x0024AB8E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170011DA RID: 4570
		// (get) Token: 0x06005749 RID: 22345 RVA: 0x0024C991 File Offset: 0x0024AB91
		// (set) Token: 0x0600574A RID: 22346 RVA: 0x0024C999 File Offset: 0x0024AB99
		public int TestEnergyCostOverride
		{
			private get
			{
				return this._testEnergyCostOverride;
			}
			set
			{
				TestMode.AssertOn();
				base.AssertMutable();
				this._testEnergyCostOverride = value;
			}
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x0024C9B0 File Offset: 0x0024ABB0
		[NullableContext(1)]
		public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card.Owner != base.Owner.Player)
			{
				return Task.CompletedTask;
			}
			if (card.EnergyCost.Canonical < 0)
			{
				return Task.CompletedTask;
			}
			int num = this.NextEnergyCost();
			card.EnergyCost.SetThisCombat(num, false);
			NCard ncard = NCard.FindOnTable(card, null);
			if (ncard != null)
			{
				ncard.PlayRandomizeCostAnim();
			}
			return Task.CompletedTask;
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x0024CA1D File Offset: 0x0024AC1D
		private int NextEnergyCost()
		{
			if (this.TestEnergyCostOverride >= 0)
			{
				return this.TestEnergyCostOverride;
			}
			return base.Owner.Player.RunState.Rng.CombatEnergyCosts.NextInt(4);
		}

		// Token: 0x040024F4 RID: 9460
		private int _testEnergyCostOverride = -1;
	}
}
