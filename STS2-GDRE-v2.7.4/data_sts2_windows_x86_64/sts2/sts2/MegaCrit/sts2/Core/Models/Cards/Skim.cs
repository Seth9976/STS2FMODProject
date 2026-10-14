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
	// Token: 0x02000ACB RID: 2763
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Skim : CardModel
	{
		// Token: 0x0600774A RID: 30538 RVA: 0x00297637 File Offset: 0x00295837
		public Skim()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17002058 RID: 8280
		// (get) Token: 0x0600774B RID: 30539 RVA: 0x00297644 File Offset: 0x00295844
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x0600774C RID: 30540 RVA: 0x00297654 File Offset: 0x00295854
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x0600774D RID: 30541 RVA: 0x0029769F File Offset: 0x0029589F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
