using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Defeat X Elites. The more you kill, the higher the rarity.
	/// </summary>
	// Token: 0x02000B64 RID: 2916
	public class EliteKiller : Badge
	{
		/// <summary>
		/// Defeat X Elites. The more you kill, the higher the rarity.
		/// </summary>
		// Token: 0x06007ACE RID: 31438 RVA: 0x0029F5CF File Offset: 0x0029D7CF
		[NullableContext(1)]
		public EliteKiller(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "ELITE", false, false)
		{
		}

		// Token: 0x170021E0 RID: 8672
		// (get) Token: 0x06007ACF RID: 31439 RVA: 0x0029F5E4 File Offset: 0x0029D7E4
		public override BadgeRarity Rarity
		{
			get
			{
				int elitesKilledCount = ScoreUtility.GetElitesKilledCount(this._run.MapPointHistory);
				BadgeRarity badgeRarity;
				if (elitesKilledCount >= 6)
				{
					if (elitesKilledCount < 9)
					{
						badgeRarity = BadgeRarity.Silver;
					}
					else
					{
						badgeRarity = BadgeRarity.Gold;
					}
				}
				else if (elitesKilledCount < 3)
				{
					badgeRarity = BadgeRarity.None;
				}
				else
				{
					badgeRarity = BadgeRarity.Bronze;
				}
				return badgeRarity;
			}
		}

		// Token: 0x06007AD0 RID: 31440 RVA: 0x0029F622 File Offset: 0x0029D822
		public override bool IsObtained()
		{
			return this.Rarity > BadgeRarity.None;
		}
	}
}
