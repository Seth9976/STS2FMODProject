using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A53 RID: 2643
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NeutronAegis : CardModel
	{
		// Token: 0x060074CB RID: 29899 RVA: 0x0029245B File Offset: 0x0029065B
		public NeutronAegis()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F4D RID: 8013
		// (get) Token: 0x060074CC RID: 29900 RVA: 0x00292468 File Offset: 0x00290668
		public override int CanonicalStarCost
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17001F4E RID: 8014
		// (get) Token: 0x060074CD RID: 29901 RVA: 0x0029246C File Offset: 0x0029066C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PlatingPower>(null));
			}
		}

		// Token: 0x17001F4F RID: 8015
		// (get) Token: 0x060074CE RID: 29902 RVA: 0x0029248C File Offset: 0x0029068C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PlatingPower>(8m));
			}
		}

		// Token: 0x060074CF RID: 29903 RVA: 0x002924A0 File Offset: 0x002906A0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<PlatingPower>(choiceContext, base.Owner.Creature, base.DynamicVars["PlatingPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060074D0 RID: 29904 RVA: 0x002924EB File Offset: 0x002906EB
		protected override void OnUpgrade()
		{
			base.DynamicVars["PlatingPower"].UpgradeValueBy(3m);
		}
	}
}
