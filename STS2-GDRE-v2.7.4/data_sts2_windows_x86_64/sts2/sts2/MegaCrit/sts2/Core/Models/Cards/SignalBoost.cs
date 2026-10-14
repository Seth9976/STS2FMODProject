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
	// Token: 0x02000AC9 RID: 2761
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SignalBoost : CardModel
	{
		// Token: 0x06007740 RID: 30528 RVA: 0x00297528 File Offset: 0x00295728
		public SignalBoost()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002054 RID: 8276
		// (get) Token: 0x06007741 RID: 30529 RVA: 0x00297535 File Offset: 0x00295735
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17002055 RID: 8277
		// (get) Token: 0x06007742 RID: 30530 RVA: 0x0029753D File Offset: 0x0029573D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SignalBoostPower>(1m));
			}
		}

		// Token: 0x06007743 RID: 30531 RVA: 0x00297550 File Offset: 0x00295750
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<SignalBoostPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SignalBoostPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007744 RID: 30532 RVA: 0x0029759B File Offset: 0x0029579B
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
