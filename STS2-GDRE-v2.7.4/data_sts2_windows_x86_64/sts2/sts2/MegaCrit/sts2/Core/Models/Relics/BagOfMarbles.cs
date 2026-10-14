using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004F9 RID: 1273
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BagOfMarbles : RelicModel
	{
		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06004EF5 RID: 20213 RVA: 0x0023C69F File Offset: 0x0023A89F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06004EF6 RID: 20214 RVA: 0x0023C6A2 File Offset: 0x0023A8A2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<VulnerablePower>(1m));
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06004EF7 RID: 20215 RVA: 0x0023C6B4 File Offset: 0x0023A8B4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x0023C6D4 File Offset: 0x0023A8D4
		public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					base.Flash();
					await PowerCmd.Apply<VulnerablePower>(choiceContext, combatState.HittableEnemies, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, null, false);
				}
			}
		}
	}
}
