using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200099E RID: 2462
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DrumOfBattle : CardModel
	{
		// Token: 0x060070E8 RID: 28904 RVA: 0x0028A3AC File Offset: 0x002885AC
		public DrumOfBattle()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001DA8 RID: 7592
		// (get) Token: 0x060070E9 RID: 28905 RVA: 0x0028A3B9 File Offset: 0x002885B9
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x17001DA9 RID: 7593
		// (get) Token: 0x060070EA RID: 28906 RVA: 0x0028A3C6 File Offset: 0x002885C6
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(2),
					new EnergyVar(2)
				});
			}
		}

		// Token: 0x060070EB RID: 28907 RVA: 0x0028A3E8 File Offset: 0x002885E8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x060070EC RID: 28908 RVA: 0x0028A434 File Offset: 0x00288634
		public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
		{
			if (card == this)
			{
				if (base.CombatState != null)
				{
					int num = await base.GeneratePlayCount(base.CombatState, null);
					int playCount = num;
					for (int i = 0; i < playCount; i++)
					{
						await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
					}
				}
			}
		}

		// Token: 0x060070ED RID: 28909 RVA: 0x0028A47F File Offset: 0x0028867F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
