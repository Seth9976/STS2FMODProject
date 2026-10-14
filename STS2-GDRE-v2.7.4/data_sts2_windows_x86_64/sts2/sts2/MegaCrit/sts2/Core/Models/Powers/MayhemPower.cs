using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006AD RID: 1709
	public sealed class MayhemPower : PowerModel
	{
		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x0600597D RID: 22909 RVA: 0x0025099F File Offset: 0x0024EB9F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x0600597E RID: 22910 RVA: 0x002509A2 File Offset: 0x0024EBA2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x0600597F RID: 22911 RVA: 0x002509A8 File Offset: 0x0024EBA8
		[NullableContext(1)]
		public override async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner.Player)
			{
				await CardPileCmd.AutoPlayFromDrawPile(choiceContext, base.Owner.Player, base.Amount, CardPilePosition.Top, false);
			}
		}
	}
}
