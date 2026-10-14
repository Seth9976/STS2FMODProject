using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200057B RID: 1403
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LizardTail : RelicModel
	{
		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06005230 RID: 21040 RVA: 0x00242728 File Offset: 0x00240928
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x06005231 RID: 21041 RVA: 0x0024272B File Offset: 0x0024092B
		public override bool IsUsedUp
		{
			get
			{
				return this._wasUsed;
			}
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x06005232 RID: 21042 RVA: 0x00242733 File Offset: 0x00240933
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(50m));
			}
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06005233 RID: 21043 RVA: 0x00242746 File Offset: 0x00240946
		// (set) Token: 0x06005234 RID: 21044 RVA: 0x0024274E File Offset: 0x0024094E
		[SavedProperty]
		public bool WasUsed
		{
			get
			{
				return this._wasUsed;
			}
			set
			{
				base.AssertMutable();
				this._wasUsed = value;
				if (this.IsUsedUp)
				{
					base.Status = RelicStatus.Disabled;
				}
			}
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x0024276C File Offset: 0x0024096C
		public override bool ShouldDieLate(Creature creature)
		{
			return creature != base.Owner.Creature || this.WasUsed;
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x0024278C File Offset: 0x0024098C
		public override async Task AfterPreventingDeath(Creature creature)
		{
			base.Flash();
			this.WasUsed = true;
			decimal num = Math.Max(1m, creature.MaxHp * (base.DynamicVars.Heal.BaseValue / 100m));
			await CreatureCmd.Heal(creature, num, true);
		}

		// Token: 0x04002472 RID: 9330
		private bool _wasUsed;
	}
}
