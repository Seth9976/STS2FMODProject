using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// The faster you win a run, the better the badge!
	/// </summary>
	// Token: 0x02000B71 RID: 2929
	public class Speedy : Badge
	{
		/// <summary>
		/// The faster you win a run, the better the badge!
		/// </summary>
		// Token: 0x06007AF5 RID: 31477 RVA: 0x002A0100 File Offset: 0x0029E300
		[NullableContext(1)]
		public Speedy(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "SPEEDY", true, false)
		{
		}

		// Token: 0x170021ED RID: 8685
		// (get) Token: 0x06007AF6 RID: 31478 RVA: 0x002A0114 File Offset: 0x0029E314
		public override BadgeRarity Rarity
		{
			get
			{
				long winTime = this._run.WinTime;
				BadgeRarity badgeRarity;
				if (winTime <= 2400L)
				{
					if (winTime > 1800L)
					{
						badgeRarity = BadgeRarity.Silver;
					}
					else
					{
						badgeRarity = BadgeRarity.Gold;
					}
				}
				else if (winTime > 3000L)
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

		// Token: 0x06007AF7 RID: 31479 RVA: 0x002A015B File Offset: 0x0029E35B
		public override bool IsObtained()
		{
			return this.Rarity > BadgeRarity.None;
		}

		// Token: 0x04002904 RID: 10500
		private const int _winTimeGold = 1800;

		// Token: 0x04002905 RID: 10501
		private const int _winTimeSilver = 2400;

		// Token: 0x04002906 RID: 10502
		private const int _winTimeBronze = 3000;
	}
}
