using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D49 RID: 3401
	[NullableContext(1)]
	[Nullable(0)]
	public class BlockGainedEntry : CombatHistoryEntry
	{
		// Token: 0x170025CB RID: 9675
		// (get) Token: 0x060089EA RID: 35306 RVA: 0x002E3729 File Offset: 0x002E1929
		public int Amount { get; }

		// Token: 0x170025CC RID: 9676
		// (get) Token: 0x060089EB RID: 35307 RVA: 0x002E3731 File Offset: 0x002E1931
		public Creature Receiver
		{
			get
			{
				return base.Actor;
			}
		}

		// Token: 0x170025CD RID: 9677
		// (get) Token: 0x060089EC RID: 35308 RVA: 0x002E3739 File Offset: 0x002E1939
		public ValueProp Props { get; }

		// Token: 0x170025CE RID: 9678
		// (get) Token: 0x060089ED RID: 35309 RVA: 0x002E3741 File Offset: 0x002E1941
		[Nullable(2)]
		public CardPlay CardPlay
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170025CF RID: 9679
		// (get) Token: 0x060089EE RID: 35310 RVA: 0x002E374C File Offset: 0x002E194C
		public override string Description
		{
			get
			{
				string id = BlockGainedEntry.GetId(this.Receiver);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 2);
				defaultInterpolatedStringHandler.AppendFormatted(id);
				defaultInterpolatedStringHandler.AppendLiteral(" gained ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Amount);
				defaultInterpolatedStringHandler.AppendLiteral(" block");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x060089EF RID: 35311 RVA: 0x002E37A3 File Offset: 0x002E19A3
		public BlockGainedEntry(int amount, ValueProp props, [Nullable(2)] CardPlay cardPlay, Creature receiver, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(receiver, roundNumber, currentSide, history, players)
		{
			this.Amount = amount;
			this.Props = props;
			this.CardPlay = cardPlay;
		}

		// Token: 0x060089F0 RID: 35312 RVA: 0x002E37CA File Offset: 0x002E19CA
		private static string GetId(Creature creature)
		{
			if (!creature.IsPlayer)
			{
				return creature.Monster.Id.Entry;
			}
			return creature.Player.Character.Id.Entry;
		}
	}
}
