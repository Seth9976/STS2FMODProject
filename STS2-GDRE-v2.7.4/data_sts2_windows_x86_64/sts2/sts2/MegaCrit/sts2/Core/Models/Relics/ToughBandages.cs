using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000600 RID: 1536
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ToughBandages : RelicModel
	{
		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x0600559A RID: 21914 RVA: 0x00248EF6 File Offset: 0x002470F6
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x0600559B RID: 21915 RVA: 0x00248EF9 File Offset: 0x002470F9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(3m, ValueProp.Unpowered));
			}
		}

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x0600559C RID: 21916 RVA: 0x00248F0C File Offset: 0x0024710C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x00248F20 File Offset: 0x00247120
		public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
		{
			if (card.Owner == base.Owner)
			{
				if (base.Owner.Creature.Side == base.Owner.Creature.CombatState.CurrentSide)
				{
					base.Flash();
					await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
				}
			}
		}
	}
}
