using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	// Token: 0x02000B5A RID: 2906
	public static class BadgePool
	{
		// Token: 0x06007AB6 RID: 31414 RVA: 0x0029F040 File Offset: 0x0029D240
		[NullableContext(1)]
		public static IReadOnlyCollection<Badge> CreateAll(SerializableRun run, ulong playerId, bool won)
		{
			return new <>z__ReadOnlyArray<Badge>(new Badge[]
			{
				new CccCombo(run, won, playerId),
				new Curses(run, won, playerId),
				new DamageLeader(run, won, playerId),
				new Debuffer(run, won, playerId),
				new DoubleSnecko(run, won, playerId),
				new EliteKiller(run, won, playerId),
				new Famished(run, won, playerId),
				new Glutton(run, won, playerId),
				new Healer(run, won, playerId),
				new Highlander(run, won, playerId),
				new Honed(run, won, playerId),
				new BigDeck(run, won, playerId),
				new ILikeShiny(run, won, playerId),
				new KaChing(run, won, playerId),
				new MoneyMoney(run, won, playerId),
				new MysteryMachine(run, won, playerId),
				new Perfect(run, won, playerId),
				new Restful(run, won, playerId),
				new Restless(run, won, playerId),
				new Speedy(run, won, playerId),
				new TabletBadge(run, won, playerId),
				new TeamPlayer(run, won, playerId),
				new TinyDeck(run, won, playerId)
			});
		}
	}
}
