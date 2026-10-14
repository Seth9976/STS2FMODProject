using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006E5 RID: 1765
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SentryModePower : PowerModel
	{
		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x06005ADA RID: 23258 RVA: 0x0025335F File Offset: 0x0025155F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x06005ADB RID: 23259 RVA: 0x00253362 File Offset: 0x00251562
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x06005ADC RID: 23260 RVA: 0x00253365 File Offset: 0x00251565
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<SweepingGaze>(false));
			}
		}

		// Token: 0x06005ADD RID: 23261 RVA: 0x00253374 File Offset: 0x00251574
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner.Player)
			{
				for (int i = 0; i < base.Amount; i++)
				{
					CardModel cardModel = combatState.CreateCard<SweepingGaze>(base.Owner.Player);
					await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner.Player, CardPilePosition.Bottom);
				}
			}
		}
	}
}
