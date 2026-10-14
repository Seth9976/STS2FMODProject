using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000515 RID: 1301
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BurningSticks : RelicModel
	{
		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x06004FA9 RID: 20393 RVA: 0x0023DCD3 File Offset: 0x0023BED3
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Shop;
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x06004FAA RID: 20394 RVA: 0x0023DCD6 File Offset: 0x0023BED6
		// (set) Token: 0x06004FAB RID: 20395 RVA: 0x0023DCDE File Offset: 0x0023BEDE
		private bool WasUsedThisCombat
		{
			get
			{
				return this._wasUsedThisCombat;
			}
			set
			{
				base.AssertMutable();
				this._wasUsedThisCombat = value;
			}
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x06004FAC RID: 20396 RVA: 0x0023DCED File Offset: 0x0023BEED
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
			}
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x0023DCFA File Offset: 0x0023BEFA
		public override Task AfterRoomEntered(AbstractRoom room)
		{
			if (!(room is CombatRoom))
			{
				return Task.CompletedTask;
			}
			this.WasUsedThisCombat = false;
			base.Status = RelicStatus.Active;
			return Task.CompletedTask;
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x0023DD20 File Offset: 0x0023BF20
		public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
		{
			if (card.Owner == base.Owner)
			{
				if (!this.WasUsedThisCombat)
				{
					if (card.Type == CardType.Skill)
					{
						base.Flash();
						CardModel cardModel = card.CreateClone();
						await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner, CardPilePosition.Bottom);
						base.Status = RelicStatus.Normal;
						this.WasUsedThisCombat = true;
					}
				}
			}
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x0023DD6B File Offset: 0x0023BF6B
		public override Task AfterCombatEnd(CombatRoom _)
		{
			this.WasUsedThisCombat = false;
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x04002431 RID: 9265
		private bool _wasUsedThisCombat;
	}
}
