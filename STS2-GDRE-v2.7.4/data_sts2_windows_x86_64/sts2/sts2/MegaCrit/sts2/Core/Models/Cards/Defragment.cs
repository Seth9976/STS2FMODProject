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
	// Token: 0x02000988 RID: 2440
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Defragment : CardModel
	{
		// Token: 0x0600706D RID: 28781 RVA: 0x00289597 File Offset: 0x00287797
		public Defragment()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001D6D RID: 7533
		// (get) Token: 0x0600706E RID: 28782 RVA: 0x002895A4 File Offset: 0x002877A4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>(null));
			}
		}

		// Token: 0x17001D6E RID: 7534
		// (get) Token: 0x0600706F RID: 28783 RVA: 0x002895C4 File Offset: 0x002877C4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<FocusPower>(1m));
			}
		}

		// Token: 0x06007070 RID: 28784 RVA: 0x002895D8 File Offset: 0x002877D8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<FocusPower>(choiceContext, base.Owner.Creature, base.DynamicVars["FocusPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007071 RID: 28785 RVA: 0x00289623 File Offset: 0x00287823
		protected override void OnUpgrade()
		{
			base.DynamicVars["FocusPower"].UpgradeValueBy(1m);
		}
	}
}
