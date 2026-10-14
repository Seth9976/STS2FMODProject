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
	// Token: 0x020004F1 RID: 1265
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Akabeko : RelicModel
	{
		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06004EB9 RID: 20153 RVA: 0x0023BF21 File Offset: 0x0023A121
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06004EBA RID: 20154 RVA: 0x0023BF24 File Offset: 0x0023A124
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<VigorPower>(8m));
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06004EBB RID: 20155 RVA: 0x0023BF38 File Offset: 0x0023A138
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VigorPower>(null));
			}
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x0023BF58 File Offset: 0x0023A158
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					base.Flash();
					await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["VigorPower"].IntValue, base.Owner.Creature, null, false);
				}
			}
		}
	}
}
