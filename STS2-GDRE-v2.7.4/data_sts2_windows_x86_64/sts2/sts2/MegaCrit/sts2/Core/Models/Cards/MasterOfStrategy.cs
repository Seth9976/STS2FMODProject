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
	// Token: 0x02000A35 RID: 2613
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MasterOfStrategy : CardModel
	{
		// Token: 0x06007429 RID: 29737 RVA: 0x00290F3E File Offset: 0x0028F13E
		public MasterOfStrategy()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001F0A RID: 7946
		// (get) Token: 0x0600742A RID: 29738 RVA: 0x00290F4B File Offset: 0x0028F14B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x17001F0B RID: 7947
		// (get) Token: 0x0600742B RID: 29739 RVA: 0x00290F58 File Offset: 0x0028F158
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x0600742C RID: 29740 RVA: 0x00290F60 File Offset: 0x0028F160
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}

		// Token: 0x0600742D RID: 29741 RVA: 0x00290F78 File Offset: 0x0028F178
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}
	}
}
