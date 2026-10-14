using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000529 RID: 1321
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DaughterOfTheWind : RelicModel
	{
		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x0600501A RID: 20506 RVA: 0x0023E927 File Offset: 0x0023CB27
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x0600501B RID: 20507 RVA: 0x0023E92A File Offset: 0x0023CB2A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(1m, ValueProp.Unpowered));
			}
		}

		// Token: 0x0600501C RID: 20508 RVA: 0x0023E93C File Offset: 0x0023CB3C
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Type == CardType.Attack)
			{
				if (cardPlay.Card.Owner == base.Owner)
				{
					base.Flash();
					await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, true);
				}
			}
		}
	}
}
