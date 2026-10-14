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
	// Token: 0x02000AD0 RID: 2768
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Smokestack : CardModel
	{
		// Token: 0x06007760 RID: 30560 RVA: 0x002978A3 File Offset: 0x00295AA3
		public Smokestack()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17002061 RID: 8289
		// (get) Token: 0x06007761 RID: 30561 RVA: 0x002978B0 File Offset: 0x00295AB0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SmokestackPower>(5m));
			}
		}

		// Token: 0x06007762 RID: 30562 RVA: 0x002978C4 File Offset: 0x00295AC4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<SmokestackPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SmokestackPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007763 RID: 30563 RVA: 0x0029790F File Offset: 0x00295B0F
		protected override void OnUpgrade()
		{
			base.DynamicVars["SmokestackPower"].UpgradeValueBy(2m);
		}
	}
}
