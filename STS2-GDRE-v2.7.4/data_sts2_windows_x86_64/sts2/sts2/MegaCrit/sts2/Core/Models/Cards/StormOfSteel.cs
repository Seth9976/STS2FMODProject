using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AEE RID: 2798
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StormOfSteel : CardModel
	{
		// Token: 0x06007815 RID: 30741 RVA: 0x002990F7 File Offset: 0x002972F7
		public StormOfSteel()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x170020A8 RID: 8360
		// (get) Token: 0x06007816 RID: 30742 RVA: 0x00299104 File Offset: 0x00297304
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(base.IsUpgraded));
			}
		}

		// Token: 0x06007817 RID: 30743 RVA: 0x00299118 File Offset: 0x00297318
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(base.Owner).Cards.ToList<CardModel>();
			int handSize = enumerable.Count<CardModel>();
			await CardCmd.Discard(choiceContext, enumerable);
			await Cmd.CustomScaledWait(0f, 0.25f, false, default(CancellationToken));
			IEnumerable<CardModel> enumerable2 = await Shiv.CreateInHand(base.Owner, handSize, base.CombatState, null);
			if (base.IsUpgraded)
			{
				foreach (CardModel cardModel in enumerable2)
				{
					CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
				}
			}
		}
	}
}
