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
	// Token: 0x02000A2C RID: 2604
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Loop : CardModel
	{
		// Token: 0x060073E7 RID: 29671 RVA: 0x002904F7 File Offset: 0x0028E6F7
		public Loop()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001EEF RID: 7919
		// (get) Token: 0x060073E8 RID: 29672 RVA: 0x00290504 File Offset: 0x0028E704
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Loop", 1m));
			}
		}

		// Token: 0x060073E9 RID: 29673 RVA: 0x0029051C File Offset: 0x0028E71C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<LoopPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Loop"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060073EA RID: 29674 RVA: 0x00290567 File Offset: 0x0028E767
		protected override void OnUpgrade()
		{
			base.DynamicVars["Loop"].UpgradeValueBy(1m);
		}

		// Token: 0x04002887 RID: 10375
		private const string _loopKey = "Loop";
	}
}
