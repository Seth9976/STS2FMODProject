using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AA6 RID: 2726
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RollingBoulder : CardModel
	{
		// Token: 0x06007680 RID: 30336 RVA: 0x00295D9D File Offset: 0x00293F9D
		public RollingBoulder()
			: base(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002001 RID: 8193
		// (get) Token: 0x06007681 RID: 30337 RVA: 0x00295DAA File Offset: 0x00293FAA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<RollingBoulderPower>(5m),
					new DynamicVar("IncrementAmount", 5m)
				});
			}
		}

		// Token: 0x06007682 RID: 30338 RVA: 0x00295DD8 File Offset: 0x00293FD8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<RollingBoulderPower>(choiceContext, base.Owner.Creature, base.DynamicVars["RollingBoulderPower"].IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007683 RID: 30339 RVA: 0x00295E23 File Offset: 0x00294023
		protected override void OnUpgrade()
		{
			base.DynamicVars["RollingBoulderPower"].UpgradeValueBy(5m);
		}

		// Token: 0x040028B8 RID: 10424
		public const int incrementAmount = 5;

		// Token: 0x040028B9 RID: 10425
		private const string _incrementAmountKey = "IncrementAmount";
	}
}
