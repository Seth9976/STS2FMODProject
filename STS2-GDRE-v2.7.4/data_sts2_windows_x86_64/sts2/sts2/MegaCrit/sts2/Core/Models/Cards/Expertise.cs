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
	// Token: 0x020009B3 RID: 2483
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Expertise : CardModel
	{
		// Token: 0x0600715B RID: 29019 RVA: 0x0028B0FD File Offset: 0x002892FD
		public Expertise()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001DDC RID: 7644
		// (get) Token: 0x0600715C RID: 29020 RVA: 0x0028B10A File Offset: 0x0028930A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x0600715D RID: 29021 RVA: 0x0028B118 File Offset: 0x00289318
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<CardModel> enumerable = await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner, false);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				CardCmd.ApplySingleTurnRetain(cardModel);
			}
		}

		// Token: 0x0600715E RID: 29022 RVA: 0x0028B163 File Offset: 0x00289363
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
