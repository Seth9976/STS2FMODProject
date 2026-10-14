using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000968 RID: 2408
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Countdown : CardModel
	{
		// Token: 0x06006FC7 RID: 28615 RVA: 0x0028814B File Offset: 0x0028634B
		public Countdown()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001D23 RID: 7459
		// (get) Token: 0x06006FC8 RID: 28616 RVA: 0x00288158 File Offset: 0x00286358
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<CountdownPower>(6m));
			}
		}

		// Token: 0x17001D24 RID: 7460
		// (get) Token: 0x06006FC9 RID: 28617 RVA: 0x0028816C File Offset: 0x0028636C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x06006FCA RID: 28618 RVA: 0x0028818C File Offset: 0x0028638C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<CountdownPower>(choiceContext, base.Owner.Creature, base.DynamicVars["CountdownPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006FCB RID: 28619 RVA: 0x002881D7 File Offset: 0x002863D7
		protected override void OnUpgrade()
		{
			base.DynamicVars["CountdownPower"].UpgradeValueBy(3m);
		}
	}
}
