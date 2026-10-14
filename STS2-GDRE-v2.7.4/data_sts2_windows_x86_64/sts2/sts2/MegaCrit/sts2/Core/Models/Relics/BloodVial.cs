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
	// Token: 0x02000507 RID: 1287
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BloodVial : RelicModel
	{
		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06004F4A RID: 20298 RVA: 0x0023D0C2 File Offset: 0x0023B2C2
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06004F4B RID: 20299 RVA: 0x0023D0C5 File Offset: 0x0023B2C5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new HealVar(2m));
			}
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x0023D0D8 File Offset: 0x0023B2D8
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
