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
	// Token: 0x02000A7F RID: 2687
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PrepTime : CardModel
	{
		// Token: 0x060075AD RID: 30125 RVA: 0x002941B6 File Offset: 0x002923B6
		public PrepTime()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001FA8 RID: 8104
		// (get) Token: 0x060075AE RID: 30126 RVA: 0x002941C3 File Offset: 0x002923C3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PrepTimePower>(4m));
			}
		}

		// Token: 0x17001FA9 RID: 8105
		// (get) Token: 0x060075AF RID: 30127 RVA: 0x002941D8 File Offset: 0x002923D8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VigorPower>(null));
			}
		}

		// Token: 0x060075B0 RID: 30128 RVA: 0x002941F8 File Offset: 0x002923F8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<PrepTimePower>(choiceContext, base.Owner.Creature, base.DynamicVars["PrepTimePower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060075B1 RID: 30129 RVA: 0x00294243 File Offset: 0x00292443
		protected override void OnUpgrade()
		{
			base.DynamicVars["PrepTimePower"].UpgradeValueBy(2m);
		}
	}
}
