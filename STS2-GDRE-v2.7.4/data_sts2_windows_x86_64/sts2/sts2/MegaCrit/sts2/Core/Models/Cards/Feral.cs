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
	// Token: 0x020009BE RID: 2494
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Feral : CardModel
	{
		// Token: 0x06007199 RID: 29081 RVA: 0x0028B9B7 File Offset: 0x00289BB7
		public Feral()
			: base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001DF8 RID: 7672
		// (get) Token: 0x0600719A RID: 29082 RVA: 0x0028B9C4 File Offset: 0x00289BC4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<FeralPower>(1m));
			}
		}

		// Token: 0x0600719B RID: 29083 RVA: 0x0028B9D8 File Offset: 0x00289BD8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<FeralPower>(choiceContext, base.Owner.Creature, base.DynamicVars["FeralPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600719C RID: 29084 RVA: 0x0028BA23 File Offset: 0x00289C23
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
