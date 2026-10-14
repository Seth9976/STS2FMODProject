using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Won with a deck of 40, 60, or 100 or more cards.
	/// </summary>
	// Token: 0x02000B5C RID: 2908
	public class BigDeck : Badge
	{
		/// <summary>
		/// Won with a deck of 40, 60, or 100 or more cards.
		/// </summary>
		// Token: 0x06007AB7 RID: 31415 RVA: 0x0029F164 File Offset: 0x0029D364
		[NullableContext(1)]
		public BigDeck(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "BIG_DECK", true, false)
		{
		}

		// Token: 0x170021DA RID: 8666
		// (get) Token: 0x06007AB8 RID: 31416 RVA: 0x0029F178 File Offset: 0x0029D378
		public override BadgeRarity Rarity
		{
			get
			{
				int count = this._localPlayer.Deck.Count;
				BadgeRarity badgeRarity;
				if (count >= 60)
				{
					if (count < 100)
					{
						badgeRarity = BadgeRarity.Silver;
					}
					else
					{
						badgeRarity = BadgeRarity.Gold;
					}
				}
				else if (count < 40)
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

		// Token: 0x06007AB9 RID: 31417 RVA: 0x0029F1B8 File Offset: 0x0029D3B8
		public override bool IsObtained()
		{
			return this.Rarity > BadgeRarity.None;
		}
	}
}
