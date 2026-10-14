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
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000581 RID: 1409
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LunarPastry : RelicModel
	{
		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06005251 RID: 21073 RVA: 0x00242A63 File Offset: 0x00240C63
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06005252 RID: 21074 RVA: 0x00242A66 File Offset: 0x00240C66
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StarsVar(1));
			}
		}

		// Token: 0x06005253 RID: 21075 RVA: 0x00242A74 File Offset: 0x00240C74
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
			}
		}
	}
}
