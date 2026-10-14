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
	// Token: 0x02000AE2 RID: 2786
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpoilsOfBattle : CardModel
	{
		// Token: 0x060077D8 RID: 30680 RVA: 0x0029886B File Offset: 0x00296A6B
		public SpoilsOfBattle()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17002090 RID: 8336
		// (get) Token: 0x060077D9 RID: 30681 RVA: 0x00298878 File Offset: 0x00296A78
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new ForgeVar(5),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x17002091 RID: 8337
		// (get) Token: 0x060077DA RID: 30682 RVA: 0x00298897 File Offset: 0x00296A97
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return HoverTipFactory.FromForge();
			}
		}

		// Token: 0x060077DB RID: 30683 RVA: 0x002988A0 File Offset: 0x00296AA0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await ForgeCmd.Forge(base.DynamicVars.Forge.IntValue, base.Owner, this);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x060077DC RID: 30684 RVA: 0x002988EB File Offset: 0x00296AEB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Forge.UpgradeValueBy(3m);
		}
	}
}
