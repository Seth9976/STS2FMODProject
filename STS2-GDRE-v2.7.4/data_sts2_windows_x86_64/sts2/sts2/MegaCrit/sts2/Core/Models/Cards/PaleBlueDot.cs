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
	// Token: 0x02000A67 RID: 2663
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaleBlueDot : CardModel
	{
		// Token: 0x06007535 RID: 30005 RVA: 0x0029317D File Offset: 0x0029137D
		public PaleBlueDot()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F7B RID: 8059
		// (get) Token: 0x06007536 RID: 30006 RVA: 0x0029318A File Offset: 0x0029138A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(1),
					new DynamicVar("CardPlay", 5m)
				});
			}
		}

		// Token: 0x06007537 RID: 30007 RVA: 0x002931B4 File Offset: 0x002913B4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<PaleBlueDotPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Cards.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007538 RID: 30008 RVA: 0x002931FF File Offset: 0x002913FF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
