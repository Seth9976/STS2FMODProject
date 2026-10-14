using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006A1 RID: 1697
	public sealed class IterationPower : PowerModel
	{
		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x06005941 RID: 22849 RVA: 0x002502EF File Offset: 0x0024E4EF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x06005942 RID: 22850 RVA: 0x002502F2 File Offset: 0x0024E4F2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005943 RID: 22851 RVA: 0x002502F8 File Offset: 0x0024E4F8
		[NullableContext(1)]
		public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card.Owner.Creature == base.Owner)
			{
				if (card.Type == CardType.Status)
				{
					int num = CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>().Count<CardDrawnEntry>((CardDrawnEntry e) => e.HappenedThisTurn(base.CombatState) && e.Actor == base.Owner && e.Card.Type == CardType.Status);
					if (num <= 1)
					{
						base.Flash();
						await CardPileCmd.Draw(choiceContext, base.Amount, base.Owner.Player, false);
					}
				}
			}
		}
	}
}
