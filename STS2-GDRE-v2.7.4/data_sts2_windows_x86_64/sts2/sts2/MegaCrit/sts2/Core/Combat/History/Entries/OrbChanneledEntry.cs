using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D55 RID: 3413
	[NullableContext(1)]
	[Nullable(0)]
	public class OrbChanneledEntry : CombatHistoryEntry
	{
		// Token: 0x170025EF RID: 9711
		// (get) Token: 0x06008A1D RID: 35357 RVA: 0x002E3E8E File Offset: 0x002E208E
		public OrbModel Orb { get; }

		// Token: 0x170025F0 RID: 9712
		// (get) Token: 0x06008A1E RID: 35358 RVA: 0x002E3E96 File Offset: 0x002E2096
		public override string Description
		{
			get
			{
				return base.Actor.Player.Character.Id.Entry + " channeled " + this.Orb.Id.Entry;
			}
		}

		// Token: 0x06008A1F RID: 35359 RVA: 0x002E3ECC File Offset: 0x002E20CC
		public OrbChanneledEntry(OrbModel orb, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(orb.Owner.Creature, roundNumber, currentSide, history, players)
		{
			this.Orb = orb;
		}
	}
}
