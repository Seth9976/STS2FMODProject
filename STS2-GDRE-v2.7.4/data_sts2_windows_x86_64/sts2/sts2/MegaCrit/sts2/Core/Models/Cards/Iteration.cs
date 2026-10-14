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
	// Token: 0x02000A18 RID: 2584
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Iteration : CardModel
	{
		// Token: 0x06007382 RID: 29570 RVA: 0x0028F7C5 File Offset: 0x0028D9C5
		public Iteration()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001EC8 RID: 7880
		// (get) Token: 0x06007383 RID: 29571 RVA: 0x0028F7D2 File Offset: 0x0028D9D2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<IterationPower>(2m));
			}
		}

		// Token: 0x06007384 RID: 29572 RVA: 0x0028F7E4 File Offset: 0x0028D9E4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<IterationPower>(choiceContext, base.Owner.Creature, base.DynamicVars["IterationPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007385 RID: 29573 RVA: 0x0028F82F File Offset: 0x0028DA2F
		protected override void OnUpgrade()
		{
			base.DynamicVars["IterationPower"].UpgradeValueBy(1m);
		}
	}
}
