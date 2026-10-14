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
	// Token: 0x02000B0D RID: 2829
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheBomb : CardModel
	{
		// Token: 0x060078B0 RID: 30896 RVA: 0x0029A66C File Offset: 0x0029886C
		public TheBomb()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x170020E3 RID: 8419
		// (get) Token: 0x060078B1 RID: 30897 RVA: 0x0029A679 File Offset: 0x00298879
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("Turns", 3m),
					new DynamicVar("BombDamage", 40m)
				});
			}
		}

		// Token: 0x060078B2 RID: 30898 RVA: 0x0029A6B0 File Offset: 0x002988B0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			TheBombPower theBombPower = await PowerCmd.Apply<TheBombPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Turns"].BaseValue, base.Owner.Creature, this, false);
			TheBombPower theBombPower2 = theBombPower;
			theBombPower2.SetDamage(base.DynamicVars["BombDamage"].BaseValue);
		}

		// Token: 0x060078B3 RID: 30899 RVA: 0x0029A6FB File Offset: 0x002988FB
		protected override void OnUpgrade()
		{
			base.DynamicVars["BombDamage"].UpgradeValueBy(10m);
		}

		// Token: 0x040028CF RID: 10447
		private const string _turnsKey = "Turns";

		// Token: 0x040028D0 RID: 10448
		private const string _bombDamageKey = "BombDamage";
	}
}
