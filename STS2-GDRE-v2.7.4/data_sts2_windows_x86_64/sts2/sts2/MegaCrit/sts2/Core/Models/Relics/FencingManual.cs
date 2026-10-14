using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000548 RID: 1352
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FencingManual : RelicModel
	{
		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x060050CA RID: 20682 RVA: 0x0023FAF3 File Offset: 0x0023DCF3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x060050CB RID: 20683 RVA: 0x0023FAF6 File Offset: 0x0023DCF6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new ForgeVar(10));
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x060050CC RID: 20684 RVA: 0x0023FB04 File Offset: 0x0023DD04
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x0023FB0C File Offset: 0x0023DD0C
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					await ForgeCmd.Forge(base.DynamicVars.Forge.BaseValue, base.Owner, this);
				}
			}
		}
	}
}
