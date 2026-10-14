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
	// Token: 0x02000AA7 RID: 2727
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RoyalGamble : CardModel
	{
		// Token: 0x06007684 RID: 30340 RVA: 0x00295E40 File Offset: 0x00294040
		public RoyalGamble()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17002002 RID: 8194
		// (get) Token: 0x06007685 RID: 30341 RVA: 0x00295E4D File Offset: 0x0029404D
		public override int CanonicalStarCost
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17002003 RID: 8195
		// (get) Token: 0x06007686 RID: 30342 RVA: 0x00295E50 File Offset: 0x00294050
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StarsVar(9));
			}
		}

		// Token: 0x17002004 RID: 8196
		// (get) Token: 0x06007687 RID: 30343 RVA: 0x00295E5E File Offset: 0x0029405E
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007688 RID: 30344 RVA: 0x00295E68 File Offset: 0x00294068
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.AttackAnimDelay);
			await PlayerCmd.GainStars(base.DynamicVars.Stars.BaseValue, base.Owner);
		}

		// Token: 0x06007689 RID: 30345 RVA: 0x00295EAB File Offset: 0x002940AB
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Retain);
		}
	}
}
