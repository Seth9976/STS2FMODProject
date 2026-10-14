using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Rooms
{
	/// <summary>
	/// Represents the visuals that should be used for a combat-style event.
	/// </summary>
	// Token: 0x02000127 RID: 295
	[NullableContext(1)]
	[Nullable(0)]
	public class CombatEventVisuals : ICombatRoomVisuals
	{
		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x0005B925 File Offset: 0x00059B25
		public EncounterModel Encounter { get; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x0005B92D File Offset: 0x00059B2D
		public IEnumerable<Creature> Allies { get; }

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x0005B935 File Offset: 0x00059B35
		public IEnumerable<Creature> Enemies
		{
			get
			{
				return this.Encounter.MonstersWithSlots.Select<ValueTuple<MonsterModel, string>, Creature>((ValueTuple<MonsterModel, string> m) => m.Item1.Creature);
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x0005B966 File Offset: 0x00059B66
		public ActModel Act { get; }

		// Token: 0x06000E65 RID: 3685 RVA: 0x0005B970 File Offset: 0x00059B70
		public CombatEventVisuals(EncounterModel encounter, IEnumerable<Player> players, ActModel act)
		{
			this.Encounter = encounter;
			this.Allies = players.Select<Player, Creature>((Player p) => p.Creature);
			this.Act = act;
		}
	}
}
