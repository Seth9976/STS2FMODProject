using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000ADD RID: 2781
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Spinner : CardModel
	{
		// Token: 0x060077B7 RID: 30647 RVA: 0x00298360 File Offset: 0x00296560
		public Spinner()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002085 RID: 8325
		// (get) Token: 0x060077B8 RID: 30648 RVA: 0x0029836D File Offset: 0x0029656D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SpinnerPower>(1m));
			}
		}

		// Token: 0x17002086 RID: 8326
		// (get) Token: 0x060077B9 RID: 30649 RVA: 0x0029837E File Offset: 0x0029657E
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<GlassOrb>()
				});
			}
		}

		// Token: 0x060077BA RID: 30650 RVA: 0x002983A4 File Offset: 0x002965A4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			if (base.IsUpgraded)
			{
				await OrbCmd.Channel<GlassOrb>(new ThrowingPlayerChoiceContext(), base.Owner);
			}
			await PowerCmd.Apply<SpinnerPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SpinnerPower"].BaseValue, base.Owner.Creature, this, false);
		}
	}
}
