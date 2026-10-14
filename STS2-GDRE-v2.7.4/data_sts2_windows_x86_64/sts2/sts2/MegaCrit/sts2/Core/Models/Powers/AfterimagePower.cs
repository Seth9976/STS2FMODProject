using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000629 RID: 1577
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AfterimagePower : PowerModel
	{
		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x0600569D RID: 22173 RVA: 0x0024B7DD File Offset: 0x002499DD
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x0600569E RID: 22174 RVA: 0x0024B7E0 File Offset: 0x002499E0
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x0600569F RID: 22175 RVA: 0x0024B7E3 File Offset: 0x002499E3
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060056A0 RID: 22176 RVA: 0x0024B7F5 File Offset: 0x002499F5
		protected override object InitInternalData()
		{
			return new AfterimagePower.Data();
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x0024B7FC File Offset: 0x002499FC
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner.Creature != base.Owner)
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<AfterimagePower.Data>().amountsForPlayedCards.Add(cardPlay.Card, base.Amount);
			return Task.CompletedTask;
		}

		// Token: 0x060056A2 RID: 22178 RVA: 0x0024B848 File Offset: 0x00249A48
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner.Creature == base.Owner)
			{
				int num;
				if (base.GetInternalData<AfterimagePower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, out num))
				{
					if (num > 0)
					{
						await CreatureCmd.GainBlock(base.Owner, num, ValueProp.Unpowered, null, true);
					}
				}
			}
		}

		// Token: 0x02001B71 RID: 7025
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// Keep track of the cards we've seen played and the power amount at the time they were played.
			/// This lets After Image avoid triggering on cards that started play before it was applied, and avoid gaining
			/// extra block on multiple plays of After Image.
			/// </summary>
			// Token: 0x04006D08 RID: 27912
			[Nullable(1)]
			public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
		}
	}
}
