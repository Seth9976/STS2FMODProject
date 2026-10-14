using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Won a run with 20, 10, 5 or fewer cards.
	/// </summary>
	// Token: 0x02000B74 RID: 2932
	public class TinyDeck : Badge
	{
		/// <summary>
		/// Won a run with 20, 10, 5 or fewer cards.
		/// </summary>
		// Token: 0x06007AFE RID: 31486 RVA: 0x002A0214 File Offset: 0x0029E414
		[NullableContext(1)]
		public TinyDeck(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "TINY_DECK", true, false)
		{
		}

		// Token: 0x170021F0 RID: 8688
		// (get) Token: 0x06007AFF RID: 31487 RVA: 0x002A0228 File Offset: 0x0029E428
		public override BadgeRarity Rarity
		{
			get
			{
				int count = this._localPlayer.Deck.Count;
				BadgeRarity badgeRarity;
				if (count <= 10)
				{
					if (count > 5)
					{
						badgeRarity = BadgeRarity.Silver;
					}
					else
					{
						badgeRarity = BadgeRarity.Gold;
					}
				}
				else if (count > 20)
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

		// Token: 0x06007B00 RID: 31488 RVA: 0x002A0267 File Offset: 0x0029E467
		public override bool IsObtained()
		{
			return this.Rarity > BadgeRarity.None;
		}
	}
}
