using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Win a run with 200, 400, 600 gold remaining.
	/// </summary>
	// Token: 0x02000B6C RID: 2924
	public class MoneyMoney : Badge
	{
		/// <summary>
		/// Win a run with 200, 400, 600 gold remaining.
		/// </summary>
		// Token: 0x06007AE6 RID: 31462 RVA: 0x0029FB30 File Offset: 0x0029DD30
		[NullableContext(1)]
		public MoneyMoney(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "MONEY_MONEY", true, false)
		{
		}

		// Token: 0x170021E8 RID: 8680
		// (get) Token: 0x06007AE7 RID: 31463 RVA: 0x0029FB44 File Offset: 0x0029DD44
		public override BadgeRarity Rarity
		{
			get
			{
				int gold = this._localPlayer.Gold;
				BadgeRarity badgeRarity;
				if (gold >= 400)
				{
					if (gold < 600)
					{
						badgeRarity = BadgeRarity.Silver;
					}
					else
					{
						badgeRarity = BadgeRarity.Gold;
					}
				}
				else if (gold < 200)
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

		// Token: 0x06007AE8 RID: 31464 RVA: 0x0029FB88 File Offset: 0x0029DD88
		public override bool IsObtained()
		{
			return this.Rarity > BadgeRarity.None;
		}
	}
}
