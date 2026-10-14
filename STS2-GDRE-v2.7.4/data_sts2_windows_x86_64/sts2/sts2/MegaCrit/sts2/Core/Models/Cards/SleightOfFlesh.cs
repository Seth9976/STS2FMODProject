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
	// Token: 0x02000ACC RID: 2764
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SleightOfFlesh : CardModel
	{
		// Token: 0x0600774E RID: 30542 RVA: 0x002976B6 File Offset: 0x002958B6
		public SleightOfFlesh()
			: base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17002059 RID: 8281
		// (get) Token: 0x0600774F RID: 30543 RVA: 0x002976C3 File Offset: 0x002958C3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SleightOfFleshPower>(9m));
			}
		}

		// Token: 0x06007750 RID: 30544 RVA: 0x002976D8 File Offset: 0x002958D8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<SleightOfFleshPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SleightOfFleshPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007751 RID: 30545 RVA: 0x00297723 File Offset: 0x00295923
		protected override void OnUpgrade()
		{
			base.DynamicVars["SleightOfFleshPower"].UpgradeValueBy(4m);
		}
	}
}
