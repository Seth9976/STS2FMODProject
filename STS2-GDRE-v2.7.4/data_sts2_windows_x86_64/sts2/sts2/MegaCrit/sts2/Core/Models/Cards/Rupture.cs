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
	// Token: 0x02000AA9 RID: 2729
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Rupture : CardModel
	{
		// Token: 0x0600768F RID: 30351 RVA: 0x00295F38 File Offset: 0x00294138
		public Rupture()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17002007 RID: 8199
		// (get) Token: 0x06007690 RID: 30352 RVA: 0x00295F45 File Offset: 0x00294145
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(1m));
			}
		}

		// Token: 0x17002008 RID: 8200
		// (get) Token: 0x06007691 RID: 30353 RVA: 0x00295F58 File Offset: 0x00294158
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06007692 RID: 30354 RVA: 0x00295F78 File Offset: 0x00294178
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<RupturePower>(choiceContext, base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007693 RID: 30355 RVA: 0x00295FC3 File Offset: 0x002941C3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Strength.UpgradeValueBy(1m);
		}
	}
}
