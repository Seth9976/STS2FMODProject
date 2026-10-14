using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005C8 RID: 1480
	public sealed class RazorTooth : RelicModel
	{
		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x06005444 RID: 21572 RVA: 0x002463B6 File Offset: 0x002445B6
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x06005445 RID: 21573 RVA: 0x002463BC File Offset: 0x002445BC
		[NullableContext(1)]
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			CardType type = cardPlay.Card.Type;
			if (type - CardType.Attack > 1)
			{
				return Task.CompletedTask;
			}
			if (!cardPlay.Card.IsUpgradable)
			{
				return Task.CompletedTask;
			}
			CardCmd.Upgrade(cardPlay.Card, CardPreviewStyle.HorizontalLayout);
			return Task.CompletedTask;
		}
	}
}
