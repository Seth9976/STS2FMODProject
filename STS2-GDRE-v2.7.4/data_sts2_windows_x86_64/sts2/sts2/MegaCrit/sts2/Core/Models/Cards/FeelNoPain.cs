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
	// Token: 0x020009BD RID: 2493
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FeelNoPain : CardModel
	{
		// Token: 0x06007194 RID: 29076 RVA: 0x0028B907 File Offset: 0x00289B07
		public FeelNoPain()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001DF6 RID: 7670
		// (get) Token: 0x06007195 RID: 29077 RVA: 0x0028B914 File Offset: 0x00289B14
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 3m));
			}
		}

		// Token: 0x17001DF7 RID: 7671
		// (get) Token: 0x06007196 RID: 29078 RVA: 0x0028B92B File Offset: 0x00289B2B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x06007197 RID: 29079 RVA: 0x0028B950 File Offset: 0x00289B50
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<FeelNoPainPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Power"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007198 RID: 29080 RVA: 0x0028B99B File Offset: 0x00289B9B
		protected override void OnUpgrade()
		{
			base.DynamicVars["Power"].UpgradeValueBy(1m);
		}

		// Token: 0x0400286C RID: 10348
		private const string _powerVarName = "Power";
	}
}
