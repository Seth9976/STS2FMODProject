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
	// Token: 0x02000A11 RID: 2577
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Inferno : CardModel
	{
		// Token: 0x06007360 RID: 29536 RVA: 0x0028F395 File Offset: 0x0028D595
		public Inferno()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001EBA RID: 7866
		// (get) Token: 0x06007361 RID: 29537 RVA: 0x0028F3A2 File Offset: 0x0028D5A2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<InfernoPower>(6m));
			}
		}

		// Token: 0x06007362 RID: 29538 RVA: 0x0028F3B4 File Offset: 0x0028D5B4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			InfernoPower infernoPower = await PowerCmd.Apply<InfernoPower>(choiceContext, base.Owner.Creature, base.DynamicVars["InfernoPower"].BaseValue, base.Owner.Creature, this, false);
			if (infernoPower != null)
			{
				infernoPower.IncrementSelfDamage();
			}
		}

		// Token: 0x06007363 RID: 29539 RVA: 0x0028F3FF File Offset: 0x0028D5FF
		protected override void OnUpgrade()
		{
			base.DynamicVars["InfernoPower"].UpgradeValueBy(3m);
		}
	}
}
