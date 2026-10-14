using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006FA RID: 1786
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoulboundPower : PowerModel
	{
		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x06005B5B RID: 23387 RVA: 0x002541D5 File Offset: 0x002523D5
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x06005B5C RID: 23388 RVA: 0x002541D8 File Offset: 0x002523D8
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013C8 RID: 5064
		// (get) Token: 0x06005B5D RID: 23389 RVA: 0x002541DB File Offset: 0x002523DB
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>(false));
			}
		}

		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x06005B5E RID: 23390 RVA: 0x002541E8 File Offset: 0x002523E8
		// (set) Token: 0x06005B5F RID: 23391 RVA: 0x002541F0 File Offset: 0x002523F0
		private bool IsAddingSoul
		{
			get
			{
				return this._isAddingSoul;
			}
			set
			{
				base.AssertMutable();
				this._isAddingSoul = value;
			}
		}

		// Token: 0x06005B60 RID: 23392 RVA: 0x00254200 File Offset: 0x00252400
		public override async Task AfterCardGeneratedForCombat(CardModel card, [Nullable(2)] Player creator)
		{
			if (creator != null && creator.Creature == base.Applier)
			{
				if (card is Soul)
				{
					if (!this.IsAddingSoul)
					{
						this.IsAddingSoul = true;
						base.Flash();
						IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.AddGeneratedCardsToCombat(Soul.Create(base.Owner.Player, base.Amount, base.CombatState), PileType.Draw, base.Owner.Player, CardPilePosition.Random);
						IReadOnlyList<CardPileAddResult> readOnlyList2 = readOnlyList;
						CardCmd.PreviewCardPileAdd(readOnlyList2, 1.2f, CardPreviewStyle.HorizontalLayout);
						this.IsAddingSoul = false;
					}
				}
			}
		}

		// Token: 0x0400251D RID: 9501
		private bool _isAddingSoul;
	}
}
