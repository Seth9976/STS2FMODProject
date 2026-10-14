using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200055E RID: 1374
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GremlinHorn : RelicModel
	{
		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x06005158 RID: 20824 RVA: 0x00240CA7 File Offset: 0x0023EEA7
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x06005159 RID: 20825 RVA: 0x00240CAA File Offset: 0x0023EEAA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(1),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x0600515A RID: 20826 RVA: 0x00240CC9 File Offset: 0x0023EEC9
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x00240CD8 File Offset: 0x0023EED8
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature target, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (target.Side != base.Owner.Creature.Side)
			{
				base.Flash();
				await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
				await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			}
		}
	}
}
