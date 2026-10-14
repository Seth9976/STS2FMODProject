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
	// Token: 0x02000A6B RID: 2667
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Parse : CardModel
	{
		// Token: 0x06007548 RID: 30024 RVA: 0x00293404 File Offset: 0x00291604
		public Parse()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F82 RID: 8066
		// (get) Token: 0x06007549 RID: 30025 RVA: 0x00293411 File Offset: 0x00291611
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x17001F83 RID: 8067
		// (get) Token: 0x0600754A RID: 30026 RVA: 0x0029341E File Offset: 0x0029161E
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
			}
		}

		// Token: 0x0600754B RID: 30027 RVA: 0x00293428 File Offset: 0x00291628
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner, false);
		}

		// Token: 0x0600754C RID: 30028 RVA: 0x00293473 File Offset: 0x00291673
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
