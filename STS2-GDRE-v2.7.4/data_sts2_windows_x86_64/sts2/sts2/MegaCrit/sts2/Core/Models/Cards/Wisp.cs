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
	// Token: 0x02000B3D RID: 2877
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Wisp : CardModel
	{
		// Token: 0x060079AE RID: 31150 RVA: 0x0029C3E0 File Offset: 0x0029A5E0
		public Wisp()
			: base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x1700214A RID: 8522
		// (get) Token: 0x060079AF RID: 31151 RVA: 0x0029C3ED File Offset: 0x0029A5ED
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));
			}
		}

		// Token: 0x1700214B RID: 8523
		// (get) Token: 0x060079B0 RID: 31152 RVA: 0x0029C3FA File Offset: 0x0029A5FA
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x1700214C RID: 8524
		// (get) Token: 0x060079B1 RID: 31153 RVA: 0x0029C402 File Offset: 0x0029A602
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x060079B2 RID: 31154 RVA: 0x0029C410 File Offset: 0x0029A610
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
		}

		// Token: 0x060079B3 RID: 31155 RVA: 0x0029C453 File Offset: 0x0029A653
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Retain);
		}
	}
}
