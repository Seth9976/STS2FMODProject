using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200050A RID: 1290
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Bookmark : RelicModel
	{
		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06004F5B RID: 20315 RVA: 0x0023D317 File Offset: 0x0023B517
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06004F5C RID: 20316 RVA: 0x0023D31A File Offset: 0x0023B51A
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Retain));
			}
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x0023D328 File Offset: 0x0023B528
		public override Task AfterFlush(PlayerChoiceContext choiceContext, Player player, IReadOnlyCollection<CardModel> flushedCards, IReadOnlyCollection<CardModel> retainedCards)
		{
			if (player != base.Owner)
			{
				return Task.CompletedTask;
			}
			List<CardModel> list = retainedCards.Where<CardModel>((CardModel c) => !c.EnergyCost.CostsX && c.EnergyCost.GetWithModifiers(CostModifiers.Local) > 0).ToList<CardModel>();
			if (list.Count == 0)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			CardModel cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem<CardModel>(list);
			if (cardModel != null)
			{
				cardModel.EnergyCost.AddUntilPlayed(-1, false);
			}
			return Task.CompletedTask;
		}
	}
}
