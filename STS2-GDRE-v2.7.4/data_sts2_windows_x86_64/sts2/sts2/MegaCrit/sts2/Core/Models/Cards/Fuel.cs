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
	// Token: 0x020009D6 RID: 2518
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Fuel : CardModel
	{
		// Token: 0x06007221 RID: 29217 RVA: 0x0028CBD1 File Offset: 0x0028ADD1
		public Fuel()
			: base(0, CardType.Skill, CardRarity.Token, TargetType.Self, true)
		{
		}

		// Token: 0x17001E31 RID: 7729
		// (get) Token: 0x06007222 RID: 29218 RVA: 0x0028CBDE File Offset: 0x0028ADDE
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy(this));
			}
		}

		// Token: 0x17001E32 RID: 7730
		// (get) Token: 0x06007223 RID: 29219 RVA: 0x0028CBEB File Offset: 0x0028ADEB
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001E33 RID: 7731
		// (get) Token: 0x06007224 RID: 29220 RVA: 0x0028CBF3 File Offset: 0x0028ADF3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x06007225 RID: 29221 RVA: 0x0028CC00 File Offset: 0x0028AE00
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
		}

		// Token: 0x06007226 RID: 29222 RVA: 0x0028CC43 File Offset: 0x0028AE43
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
