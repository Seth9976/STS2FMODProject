using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200097D RID: 2429
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Debris : CardModel
	{
		// Token: 0x0600702F RID: 28719 RVA: 0x00288FDD File Offset: 0x002871DD
		public Debris()
			: base(1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x17001D4D RID: 7501
		// (get) Token: 0x06007030 RID: 28720 RVA: 0x00288FEA File Offset: 0x002871EA
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001D4E RID: 7502
		// (get) Token: 0x06007031 RID: 28721 RVA: 0x00288FED File Offset: 0x002871ED
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007032 RID: 28722 RVA: 0x00288FF5 File Offset: 0x002871F5
		protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			return Task.CompletedTask;
		}
	}
}
