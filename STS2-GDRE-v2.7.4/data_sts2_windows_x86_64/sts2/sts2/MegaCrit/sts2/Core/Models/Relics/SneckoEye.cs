using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005E6 RID: 1510
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SneckoEye : RelicModel
	{
		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x060054FA RID: 21754 RVA: 0x00247B8F File Offset: 0x00245D8F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x060054FB RID: 21755 RVA: 0x00247B92 File Offset: 0x00245D92
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x060054FC RID: 21756 RVA: 0x00247BA0 File Offset: 0x00245DA0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<ConfusedPower>(null));
			}
		}

		// Token: 0x060054FD RID: 21757 RVA: 0x00247BC0 File Offset: 0x00245DC0
		public override async Task AfterObtained()
		{
			if (CombatManager.Instance.IsInProgress)
			{
				await this.ApplyPower();
			}
		}

		// Token: 0x060054FE RID: 21758 RVA: 0x00247C04 File Offset: 0x00245E04
		public override async Task BeforeCombatStart()
		{
			await this.ApplyPower();
		}

		// Token: 0x060054FF RID: 21759 RVA: 0x00247C47 File Offset: 0x00245E47
		public override decimal ModifyHandDraw(Player player, decimal count)
		{
			if (player != base.Owner)
			{
				return count;
			}
			return count + base.DynamicVars.Cards.BaseValue;
		}

		// Token: 0x06005500 RID: 21760 RVA: 0x00247C6C File Offset: 0x00245E6C
		private async Task ApplyPower()
		{
			await PowerCmd.Apply<ConfusedPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, 1m, base.Owner.Creature, null, false);
			this.ApplyTestEnergyCostOverrideToPower();
		}

		// Token: 0x06005501 RID: 21761 RVA: 0x00247CAF File Offset: 0x00245EAF
		public void SetTestEnergyCostOverride(int value)
		{
			TestMode.AssertOn();
			base.AssertMutable();
			this._testEnergyCostOverride = value;
			this.ApplyTestEnergyCostOverrideToPower();
		}

		// Token: 0x06005502 RID: 21762 RVA: 0x00247CCC File Offset: 0x00245ECC
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

		// Token: 0x040024C0 RID: 9408
		private int _testEnergyCostOverride = -1;
	}
}
