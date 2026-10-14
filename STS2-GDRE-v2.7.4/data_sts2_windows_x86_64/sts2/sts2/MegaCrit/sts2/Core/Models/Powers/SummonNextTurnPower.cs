using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200070A RID: 1802
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SummonNextTurnPower : PowerModel
	{
		// Token: 0x170013ED RID: 5101
		// (get) Token: 0x06005BAE RID: 23470 RVA: 0x00254A0F File Offset: 0x00252C0F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x06005BAF RID: 23471 RVA: 0x00254A12 File Offset: 0x00252C12
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x06005BB0 RID: 23472 RVA: 0x00254A15 File Offset: 0x00252C15
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonStatic, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005BB1 RID: 23473 RVA: 0x00254A28 File Offset: 0x00252C28
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner.Player)
			{
				if (base.AmountOnTurnStart != 0)
				{
					await OstyCmd.Summon(choiceContext, base.Owner.Player, base.Amount, this);
					await PowerCmd.Remove(this);
				}
			}
		}
	}
}
