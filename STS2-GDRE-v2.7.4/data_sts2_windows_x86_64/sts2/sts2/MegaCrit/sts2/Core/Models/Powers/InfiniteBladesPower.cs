using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200069E RID: 1694
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InfiniteBladesPower : PowerModel
	{
		// Token: 0x170012BC RID: 4796
		// (get) Token: 0x0600592C RID: 22828 RVA: 0x0025002E File Offset: 0x0024E22E
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012BD RID: 4797
		// (get) Token: 0x0600592D RID: 22829 RVA: 0x00250031 File Offset: 0x0024E231
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170012BE RID: 4798
		// (get) Token: 0x0600592E RID: 22830 RVA: 0x00250034 File Offset: 0x0024E234
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(false));
			}
		}

		// Token: 0x0600592F RID: 22831 RVA: 0x00250044 File Offset: 0x0024E244
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner.Player)
			{
				base.Flash();
				await Shiv.CreateInHand(base.Owner.Player, base.Amount, combatState, null);
			}
		}
	}
}
