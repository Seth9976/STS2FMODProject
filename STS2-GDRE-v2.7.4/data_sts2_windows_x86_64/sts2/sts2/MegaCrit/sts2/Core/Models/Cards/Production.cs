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
	// Token: 0x02000A81 RID: 2689
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Production : CardModel
	{
		// Token: 0x060075B5 RID: 30133 RVA: 0x002942C3 File Offset: 0x002924C3
		public Production()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001FAB RID: 8107
		// (get) Token: 0x060075B6 RID: 30134 RVA: 0x002942D0 File Offset: 0x002924D0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x17001FAC RID: 8108
		// (get) Token: 0x060075B7 RID: 30135 RVA: 0x002942DD File Offset: 0x002924DD
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001FAD RID: 8109
		// (get) Token: 0x060075B8 RID: 30136 RVA: 0x002942E5 File Offset: 0x002924E5
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x060075B9 RID: 30137 RVA: 0x002942F4 File Offset: 0x002924F4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
		}

		// Token: 0x060075BA RID: 30138 RVA: 0x00294337 File Offset: 0x00292537
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
