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
using MegaCrit.Sts2.Core.Models.Orbs;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000704 RID: 1796
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StormPower : PowerModel
	{
		// Token: 0x170013DD RID: 5085
		// (get) Token: 0x06005B8B RID: 23435 RVA: 0x0025461E File Offset: 0x0025281E
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013DE RID: 5086
		// (get) Token: 0x06005B8C RID: 23436 RVA: 0x00254621 File Offset: 0x00252821
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005B8D RID: 23437 RVA: 0x00254624 File Offset: 0x00252824
		protected override object InitInternalData()
		{
			return new StormPower.Data();
		}

		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x06005B8E RID: 23438 RVA: 0x0025462B File Offset: 0x0025282B
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<LightningOrb>()
				});
			}
		}

		// Token: 0x06005B8F RID: 23439 RVA: 0x00254650 File Offset: 0x00252850
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner.Player)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Type != CardType.Power)
			{
				return Task.CompletedTask;
			}
			base.GetInternalData<StormPower.Data>().amountsForPlayedCards.Add(cardPlay.Card, base.Amount);
			return Task.CompletedTask;
		}

		// Token: 0x06005B90 RID: 23440 RVA: 0x002546B0 File Offset: 0x002528B0
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner.Player)
			{
				int lightning;
				if (base.GetInternalData<StormPower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, out lightning))
				{
					if (lightning > 0)
					{
						base.Flash();
						for (int i = 0; i < lightning; i++)
						{
							await OrbCmd.Channel<LightningOrb>(choiceContext, base.Owner.Player);
						}
					}
				}
			}
		}

		// Token: 0x02001C7A RID: 7290
		[NullableContext(0)]
		private class Data
		{
			/// <summary>
			/// Keep track of the Power cards we've seen played and the power amount at the time they were played.
			/// This lets Storm avoid triggering on cards that started play before it was applied, and avoid channeling
			/// extra lightning on multiple plays of Storm.
			/// </summary>
			// Token: 0x04007247 RID: 29255
			[Nullable(1)]
			public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
		}
	}
}
