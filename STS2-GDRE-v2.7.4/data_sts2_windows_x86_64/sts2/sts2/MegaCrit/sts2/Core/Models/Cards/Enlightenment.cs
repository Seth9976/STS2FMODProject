using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009A8 RID: 2472
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Enlightenment : CardModel
	{
		// Token: 0x0600711F RID: 28959 RVA: 0x0028AA08 File Offset: 0x00288C08
		public Enlightenment()
			: base(0, CardType.Skill, CardRarity.Event, TargetType.Self, true)
		{
		}

		// Token: 0x17001DC0 RID: 7616
		// (get) Token: 0x06007120 RID: 28960 RVA: 0x0028AA15 File Offset: 0x00288C15
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x06007121 RID: 28961 RVA: 0x0028AA20 File Offset: 0x00288C20
		protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			foreach (CardModel cardModel in PileType.Hand.GetPile(base.Owner).Cards)
			{
				if (base.IsUpgraded)
				{
					cardModel.EnergyCost.SetThisCombat(1, true);
				}
				else
				{
					cardModel.EnergyCost.SetThisTurnOrUntilPlayed(1, true);
				}
			}
			return Task.CompletedTask;
		}
	}
}
