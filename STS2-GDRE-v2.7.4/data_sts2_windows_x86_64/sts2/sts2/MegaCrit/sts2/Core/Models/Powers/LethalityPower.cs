using System;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006A6 RID: 1702
	public sealed class LethalityPower : PowerModel
	{
		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x0600595E RID: 22878 RVA: 0x00250638 File Offset: 0x0024E838
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x0600595F RID: 22879 RVA: 0x0025063B File Offset: 0x0024E83B
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005960 RID: 22880 RVA: 0x00250640 File Offset: 0x0024E840
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			if (cardSource == null)
			{
				return 1m;
			}
			if (cardSource.Owner.Creature != base.Owner)
			{
				return 1m;
			}
			CardPile cardPile;
			if (cardSource != null)
			{
				cardPile = cardSource.Pile;
				if (cardPile != null && cardPile.Type == PileType.Play && cardSource.CurrentPlayIndex > 0)
				{
					return 1m;
				}
			}
			int num = CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((CardPlayStartedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Player == base.Owner.Player);
			cardPile = cardSource.Pile;
			int num2 = (((cardPile != null && cardPile.Type == PileType.Play) > false) ? 1 : 0);
			if (num > num2)
			{
				return 1m;
			}
			return 1m + base.Amount / 100m;
		}
	}
}
