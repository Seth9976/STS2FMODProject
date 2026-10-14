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
	// Token: 0x02000931 RID: 2353
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BrightestFlame : CardModel
	{
		// Token: 0x06006EB2 RID: 28338 RVA: 0x00285EC3 File Offset: 0x002840C3
		public BrightestFlame()
			: base(0, CardType.Skill, CardRarity.Ancient, TargetType.Self, true)
		{
		}

		// Token: 0x17001CB0 RID: 7344
		// (get) Token: 0x06006EB3 RID: 28339 RVA: 0x00285ED0 File Offset: 0x002840D0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new MaxHpVar(1m),
					new EnergyVar(2),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x17001CB1 RID: 7345
		// (get) Token: 0x06006EB4 RID: 28340 RVA: 0x00285EFC File Offset: 0x002840FC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06006EB5 RID: 28341 RVA: 0x00285F0C File Offset: 0x0028410C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			await CreatureCmd.LoseMaxHp(choiceContext, base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue, true);
		}

		// Token: 0x06006EB6 RID: 28342 RVA: 0x00285F57 File Offset: 0x00284157
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
