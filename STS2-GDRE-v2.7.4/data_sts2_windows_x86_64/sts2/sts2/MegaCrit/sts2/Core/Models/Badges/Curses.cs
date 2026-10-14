using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Won with 5 or more Curses.
	/// </summary>
	// Token: 0x02000B5F RID: 2911
	public class Curses : Badge
	{
		/// <summary>
		/// Won with 5 or more Curses.
		/// </summary>
		// Token: 0x06007AC0 RID: 31424 RVA: 0x0029F279 File Offset: 0x0029D479
		[NullableContext(1)]
		public Curses(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "CURSES", true, false)
		{
		}

		// Token: 0x170021DC RID: 8668
		// (get) Token: 0x06007AC1 RID: 31425 RVA: 0x0029F28B File Offset: 0x0029D48B
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007AC2 RID: 31426 RVA: 0x0029F290 File Offset: 0x0029D490
		public override bool IsObtained()
		{
			int num = 0;
			foreach (SerializableCard serializableCard in this._localPlayer.Deck)
			{
				if (SaveUtil.CardOrDeprecated(serializableCard.Id).Type == CardType.Curse)
				{
					num++;
				}
			}
			return num >= 5;
		}

		// Token: 0x040028FF RID: 10495
		private const int _curseRequirement = 5;
	}
}
