using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000569 RID: 1385
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IvoryTile : RelicModel
	{
		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x0600519E RID: 20894 RVA: 0x00241457 File Offset: 0x0023F657
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x0600519F RID: 20895 RVA: 0x0024145A File Offset: 0x0023F65A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(1),
					new EnergyVar("EnergyThreshold", 3)
				});
			}
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x060051A0 RID: 20896 RVA: 0x0024147E File Offset: 0x0023F67E
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x0024148C File Offset: 0x0023F68C
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (cardPlay.Resources.EnergyValue >= base.DynamicVars["EnergyThreshold"].IntValue)
				{
					base.Flash();
					await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
				}
			}
		}

		// Token: 0x04002460 RID: 9312
		private const string _energyThresholdKey = "EnergyThreshold";
	}
}
