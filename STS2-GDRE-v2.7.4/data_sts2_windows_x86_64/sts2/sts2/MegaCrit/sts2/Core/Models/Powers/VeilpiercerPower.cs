using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000727 RID: 1831
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VeilpiercerPower : PowerModel
	{
		// Token: 0x17001458 RID: 5208
		// (get) Token: 0x06005C73 RID: 23667 RVA: 0x002564FB File Offset: 0x002546FB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001459 RID: 5209
		// (get) Token: 0x06005C74 RID: 23668 RVA: 0x002564FE File Offset: 0x002546FE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700145A RID: 5210
		// (get) Token: 0x06005C75 RID: 23669 RVA: 0x00256501 File Offset: 0x00254701
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
			}
		}

		// Token: 0x06005C76 RID: 23670 RVA: 0x00256510 File Offset: 0x00254710
		public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
		{
			modifiedCost = originalCost;
			if (card.Owner.Creature != base.Owner)
			{
				return false;
			}
			if (!card.Keywords.Contains(CardKeyword.Ethereal))
			{
				return false;
			}
			CardPile pile = card.Pile;
			PileType? pileType = ((pile != null) ? new PileType?(pile.Type) : null);
			bool flag;
			if (pileType != null)
			{
				PileType valueOrDefault = pileType.GetValueOrDefault();
				if (valueOrDefault == PileType.Hand || valueOrDefault == PileType.Play)
				{
					flag = true;
					goto IL_006B;
				}
			}
			flag = false;
			IL_006B:
			if (!flag)
			{
				return false;
			}
			modifiedCost = 0m;
			return true;
		}

		// Token: 0x06005C77 RID: 23671 RVA: 0x00256598 File Offset: 0x00254798
		public override async Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner.Creature == base.Owner)
			{
				if (cardPlay.Card.Keywords.Contains(CardKeyword.Ethereal))
				{
					CardPile pile = cardPlay.Card.Pile;
					PileType? pileType = ((pile != null) ? new PileType?(pile.Type) : null);
					bool flag;
					if (pileType != null)
					{
						PileType valueOrDefault = pileType.GetValueOrDefault();
						if (valueOrDefault == PileType.Hand || valueOrDefault == PileType.Play)
						{
							flag = true;
							goto IL_009F;
						}
					}
					flag = false;
					IL_009F:
					if (flag)
					{
						await PowerCmd.Decrement(this);
					}
				}
			}
		}
	}
}
