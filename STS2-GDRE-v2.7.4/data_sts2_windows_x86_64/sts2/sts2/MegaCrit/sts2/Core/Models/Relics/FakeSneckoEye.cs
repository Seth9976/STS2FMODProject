using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000545 RID: 1349
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeSneckoEye : RelicModel
	{
		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x060050B3 RID: 20659 RVA: 0x0023F83E File Offset: 0x0023DA3E
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x060050B4 RID: 20660 RVA: 0x0023F841 File Offset: 0x0023DA41
		public override int MerchantCost
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x060050B5 RID: 20661 RVA: 0x0023F848 File Offset: 0x0023DA48
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<ConfusedPower>(null));
			}
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x0023F868 File Offset: 0x0023DA68
		public override async Task AfterObtained()
		{
			if (CombatManager.Instance.IsInProgress)
			{
				await this.ApplyPower();
			}
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x0023F8AC File Offset: 0x0023DAAC
		public override async Task BeforeCombatStart()
		{
			await this.ApplyPower();
		}

		// Token: 0x060050B8 RID: 20664 RVA: 0x0023F8F0 File Offset: 0x0023DAF0
		private async Task ApplyPower()
		{
			await PowerCmd.Apply<ConfusedPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, 1m, base.Owner.Creature, null, false);
			this.ApplyTestEnergyCostOverrideToPower();
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x0023F933 File Offset: 0x0023DB33
		public void SetTestEnergyCostOverride(int value)
		{
			TestMode.AssertOn();
			base.AssertMutable();
			this._testEnergyCostOverride = value;
			this.ApplyTestEnergyCostOverrideToPower();
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x0023F950 File Offset: 0x0023DB50
		private void ApplyTestEnergyCostOverrideToPower()
		{
			if (this._testEnergyCostOverride < 0)
			{
				return;
			}
			ConfusedPower power = base.Owner.Creature.GetPower<ConfusedPower>();
			if (power == null)
			{
				return;
			}
			power.TestEnergyCostOverride = this._testEnergyCostOverride;
		}

		// Token: 0x04002446 RID: 9286
		private int _testEnergyCostOverride = -1;
	}
}
