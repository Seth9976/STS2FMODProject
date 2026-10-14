using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D57 RID: 3415
	[NullableContext(1)]
	[Nullable(0)]
	public class PowerReceivedEntry : CombatHistoryEntry
	{
		// Token: 0x170025F4 RID: 9716
		// (get) Token: 0x06008A24 RID: 35364 RVA: 0x002E3FB7 File Offset: 0x002E21B7
		public PowerModel Power { get; }

		// Token: 0x170025F5 RID: 9717
		// (get) Token: 0x06008A25 RID: 35365 RVA: 0x002E3FBF File Offset: 0x002E21BF
		public decimal Amount { get; }

		// Token: 0x170025F6 RID: 9718
		// (get) Token: 0x06008A26 RID: 35366 RVA: 0x002E3FC7 File Offset: 0x002E21C7
		[Nullable(2)]
		public Creature Applier
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170025F7 RID: 9719
		// (get) Token: 0x06008A27 RID: 35367 RVA: 0x002E3FD0 File Offset: 0x002E21D0
		public override string Description
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (this.Applier != null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 4);
					defaultInterpolatedStringHandler.AppendFormatted(this.Applier.ModelId.Entry);
					defaultInterpolatedStringHandler.AppendLiteral(" applied ");
					defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Amount);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted(this.Power.Id.Entry);
					defaultInterpolatedStringHandler.AppendLiteral(" to ");
					defaultInterpolatedStringHandler.AppendFormatted(base.Actor.ModelId.Entry);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 3);
				defaultInterpolatedStringHandler.AppendFormatted(base.Actor.ModelId.Entry);
				defaultInterpolatedStringHandler.AppendLiteral(" received ");
				defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Amount);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(this.Power.Id.Entry);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x06008A28 RID: 35368 RVA: 0x002E40D4 File Offset: 0x002E22D4
		public PowerReceivedEntry(PowerModel power, decimal amount, [Nullable(2)] Creature applier, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(power.Owner, roundNumber, currentSide, history, players)
		{
			this.Power = power;
			this.Amount = amount;
			this.Applier = applier;
		}
	}
}
