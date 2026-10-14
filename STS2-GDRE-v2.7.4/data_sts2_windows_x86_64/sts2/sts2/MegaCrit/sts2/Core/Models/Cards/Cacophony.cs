using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200093F RID: 2367
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Cacophony : CardModel
	{
		// Token: 0x06006EF6 RID: 28406 RVA: 0x0028676F File Offset: 0x0028496F
		public Cacophony()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001CCC RID: 7372
		// (get) Token: 0x06006EF7 RID: 28407 RVA: 0x0028677C File Offset: 0x0028497C
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001CCD RID: 7373
		// (get) Token: 0x06006EF8 RID: 28408 RVA: 0x0028677F File Offset: 0x0028497F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(33),
					new DamageVar(66m, ValueProp.Unpowered)
				});
			}
		}

		// Token: 0x06006EF9 RID: 28409 RVA: 0x002867A8 File Offset: 0x002849A8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<CacophonyPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Damage.IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006EFA RID: 28410 RVA: 0x002867F3 File Offset: 0x002849F3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(33m);
		}

		// Token: 0x04002851 RID: 10321
		public const int cardAmount = 33;
	}
}
