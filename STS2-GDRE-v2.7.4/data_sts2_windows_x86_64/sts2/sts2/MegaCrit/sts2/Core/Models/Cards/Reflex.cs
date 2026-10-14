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
	// Token: 0x02000A9B RID: 2715
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Reflex : CardModel
	{
		// Token: 0x0600763D RID: 30269 RVA: 0x00295497 File Offset: 0x00293697
		public Reflex()
			: base(3, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001FE3 RID: 8163
		// (get) Token: 0x0600763E RID: 30270 RVA: 0x002954A4 File Offset: 0x002936A4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x17001FE4 RID: 8164
		// (get) Token: 0x0600763F RID: 30271 RVA: 0x002954B1 File Offset: 0x002936B1
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Sly);
			}
		}

		// Token: 0x06007640 RID: 30272 RVA: 0x002954BC File Offset: 0x002936BC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.AttackAnimDelay);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x06007641 RID: 30273 RVA: 0x00295507 File Offset: 0x00293707
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
