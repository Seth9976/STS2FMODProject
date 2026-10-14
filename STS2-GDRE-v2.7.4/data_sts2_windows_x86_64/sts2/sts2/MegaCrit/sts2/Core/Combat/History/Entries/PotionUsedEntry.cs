using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Combat.History.Entries
{
	// Token: 0x02000D56 RID: 3414
	[NullableContext(1)]
	[Nullable(0)]
	public class PotionUsedEntry : CombatHistoryEntry
	{
		// Token: 0x170025F1 RID: 9713
		// (get) Token: 0x06008A20 RID: 35360 RVA: 0x002E3EEC File Offset: 0x002E20EC
		public PotionModel Potion { get; }

		// Token: 0x170025F2 RID: 9714
		// (get) Token: 0x06008A21 RID: 35361 RVA: 0x002E3EF4 File Offset: 0x002E20F4
		[Nullable(2)]
		public Creature Target
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170025F3 RID: 9715
		// (get) Token: 0x06008A22 RID: 35362 RVA: 0x002E3EFC File Offset: 0x002E20FC
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(base.Actor.Player.Character.Id.Entry + " drank " + this.Potion.Id.Entry);
				if (this.Target != null)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder3 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral(" targeting ");
					appendInterpolatedStringHandler.AppendFormatted(this.Target.Monster.Id.Entry);
					stringBuilder3.Append(ref appendInterpolatedStringHandler);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06008A23 RID: 35363 RVA: 0x002E3F8F File Offset: 0x002E218F
		public PotionUsedEntry(PotionModel potion, [Nullable(2)] Creature target, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
			: base(potion.Owner.Creature, roundNumber, currentSide, history, players)
		{
			this.Potion = potion;
			this.Target = target;
		}
	}
}
