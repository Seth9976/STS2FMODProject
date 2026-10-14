using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200053F RID: 1343
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeBloodVial : RelicModel
	{
		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06005088 RID: 20616 RVA: 0x0023F4B7 File Offset: 0x0023D6B7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06005089 RID: 20617 RVA: 0x0023F4BA File Offset: 0x0023D6BA
		public override int MerchantCost
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x0600508A RID: 20618 RVA: 0x0023F4BE File Offset: 0x0023D6BE
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(1m));
			}
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x0023F4D0 File Offset: 0x0023D6D0
		public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.IntValue, true);
				}
			}
		}
	}
}
