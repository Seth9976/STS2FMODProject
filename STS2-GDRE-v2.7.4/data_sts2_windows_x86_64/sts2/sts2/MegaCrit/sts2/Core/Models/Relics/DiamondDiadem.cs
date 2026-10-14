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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200052D RID: 1325
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DiamondDiadem : RelicModel
	{
		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06005028 RID: 20520 RVA: 0x0023EA74 File Offset: 0x0023CC74
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06005029 RID: 20521 RVA: 0x0023EA77 File Offset: 0x0023CC77
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(20m, ValueProp.Unpowered));
			}
		}

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x0600502A RID: 20522 RVA: 0x0023EA8B File Offset: 0x0023CC8B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x0023EAA0 File Offset: 0x0023CCA0
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					base.Flash();
					await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
					await PowerCmd.Apply<BlurPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, 1m, base.Owner.Creature, null, false);
				}
			}
		}
	}
}
