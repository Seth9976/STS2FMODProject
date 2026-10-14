using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A83 RID: 2691
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Prophesize : CardModel
	{
		// Token: 0x060075C0 RID: 30144 RVA: 0x002943CC File Offset: 0x002925CC
		public Prophesize()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001FB0 RID: 8112
		// (get) Token: 0x060075C1 RID: 30145 RVA: 0x002943D9 File Offset: 0x002925D9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(6));
			}
		}

		// Token: 0x060075C2 RID: 30146 RVA: 0x002943E8 File Offset: 0x002925E8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.AttackAnimDelay);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x060075C3 RID: 30147 RVA: 0x00294433 File Offset: 0x00292633
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(3m);
		}
	}
}
