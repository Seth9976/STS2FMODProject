using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006BA RID: 1722
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NostalgiaPower : PowerModel
	{
		// Token: 0x17001304 RID: 4868
		// (get) Token: 0x060059C5 RID: 22981 RVA: 0x00251023 File Offset: 0x0024F223
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001305 RID: 4869
		// (get) Token: 0x060059C6 RID: 22982 RVA: 0x00251026 File Offset: 0x0024F226
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x060059C7 RID: 22983 RVA: 0x0025102C File Offset: 0x0024F22C
		public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources, CardLocation location)
		{
			if (card.Owner.Creature != base.Owner)
			{
				return location;
			}
			CardType type = card.Type;
			if (type - CardType.Attack > 1)
			{
				return location;
			}
			if (location.pileType != PileType.Discard)
			{
				return location;
			}
			int num = CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>(delegate(CardPlayStartedEntry e)
			{
				bool flag = e.HappenedThisTurn(base.CombatState);
				bool flag2 = flag;
				if (flag2)
				{
					CardType type2 = e.CardPlay.Card.Type;
					bool flag3 = type2 - CardType.Attack <= 1;
					flag2 = flag3;
				}
				return flag2 && e.CardPlay.Player == base.Owner.Player;
			});
			if (num >= base.Amount)
			{
				return location;
			}
			location.pileType = PileType.Draw;
			location.position = CardPilePosition.Top;
			return location;
		}

		// Token: 0x060059C8 RID: 22984 RVA: 0x002510B4 File Offset: 0x0024F2B4
		public override Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation location)
		{
			if (card.Owner.Creature != base.Owner)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			return Task.CompletedTask;
		}
	}
}
