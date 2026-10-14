using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006B9 RID: 1721
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NoEnergyGainPower : PowerModel
	{
		// Token: 0x17001302 RID: 4866
		// (get) Token: 0x060059BF RID: 22975 RVA: 0x00250FA3 File Offset: 0x0024F1A3
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001303 RID: 4867
		// (get) Token: 0x060059C0 RID: 22976 RVA: 0x00250FA6 File Offset: 0x0024F1A6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x060059C1 RID: 22977 RVA: 0x00250FA9 File Offset: 0x0024F1A9
		public override decimal ModifyEnergyGain(Player player, decimal amount)
		{
			if (player != base.Owner.Player)
			{
				return amount;
			}
			return 0m;
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x00250FC0 File Offset: 0x0024F1C0
		public override Task AfterModifyingEnergyGain()
		{
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x060059C3 RID: 22979 RVA: 0x00250FD0 File Offset: 0x0024F1D0
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}
	}
}
