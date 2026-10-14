using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200065A RID: 1626
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DarkEmbracePower : PowerModel
	{
		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x060057B2 RID: 22450 RVA: 0x0024D55B File Offset: 0x0024B75B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x060057B3 RID: 22451 RVA: 0x0024D55E File Offset: 0x0024B75E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060057B4 RID: 22452 RVA: 0x0024D561 File Offset: 0x0024B761
		protected override object InitInternalData()
		{
			return new DarkEmbracePower.Data();
		}

		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x060057B5 RID: 22453 RVA: 0x0024D568 File Offset: 0x0024B768
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x060057B6 RID: 22454 RVA: 0x0024D578 File Offset: 0x0024B778
		public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
		{
			if (card.Owner.Creature == base.Owner)
			{
				if (causedByEthereal)
				{
					base.GetInternalData<DarkEmbracePower.Data>().etherealCount++;
				}
				else
				{
					await CardPileCmd.Draw(choiceContext, base.Amount, base.Owner.Player, false);
				}
			}
		}

		// Token: 0x060057B7 RID: 22455 RVA: 0x0024D5D4 File Offset: 0x0024B7D4
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				DarkEmbracePower.Data data = base.GetInternalData<DarkEmbracePower.Data>();
				await CardPileCmd.Draw(choiceContext, base.Amount * data.etherealCount, base.Owner.Player, false);
				data.etherealCount = 0;
			}
		}

		// Token: 0x02001BAE RID: 7086
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// If Ethereal cards are exhausted at turn end, we want to give the resulting cards to the player after the
			/// flush occurs. In STS1 this is handled because the card draws are put on the queue, but here we have to
			/// manually defer.
			/// </summary>
			// Token: 0x04006E2A RID: 28202
			public int etherealCount;
		}
	}
}
