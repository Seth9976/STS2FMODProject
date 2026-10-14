using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005D2 RID: 1490
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RoyalPoison : RelicModel
	{
		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x0600547B RID: 21627 RVA: 0x00246A81 File Offset: 0x00244C81
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x0600547C RID: 21628 RVA: 0x00246A84 File Offset: 0x00244C84
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(4m, ValueProp.Unblockable | ValueProp.Unpowered));
			}
		}

		// Token: 0x0600547D RID: 21629 RVA: 0x00246A98 File Offset: 0x00244C98
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					base.Flash();
					await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.Damage, null, null, null);
				}
			}
		}
	}
}
