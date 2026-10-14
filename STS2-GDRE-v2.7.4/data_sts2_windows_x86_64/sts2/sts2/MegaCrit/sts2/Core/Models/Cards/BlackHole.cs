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
	// Token: 0x0200091B RID: 2331
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BlackHole : CardModel
	{
		// Token: 0x06006E3B RID: 28219 RVA: 0x00284F60 File Offset: 0x00283160
		public BlackHole()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001C7E RID: 7294
		// (get) Token: 0x06006E3C RID: 28220 RVA: 0x00284F6D File Offset: 0x0028316D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<BlackHolePower>(3m));
			}
		}

		// Token: 0x06006E3D RID: 28221 RVA: 0x00284F80 File Offset: 0x00283180
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<BlackHolePower>(choiceContext, base.Owner.Creature, base.DynamicVars["BlackHolePower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006E3E RID: 28222 RVA: 0x00284FCB File Offset: 0x002831CB
		protected override void OnUpgrade()
		{
			base.DynamicVars["BlackHolePower"].UpgradeValueBy(1m);
		}
	}
}
