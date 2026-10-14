using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Deck contains 5+ of the same card
	/// </summary>
	// Token: 0x02000B69 RID: 2921
	public class Honed : Badge
	{
		/// <summary>
		/// Deck contains 5+ of the same card
		/// </summary>
		// Token: 0x06007ADD RID: 31453 RVA: 0x0029F901 File Offset: 0x0029DB01
		[NullableContext(1)]
		public Honed(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "HONED", true, false)
		{
		}

		// Token: 0x170021E5 RID: 8677
		// (get) Token: 0x06007ADE RID: 31454 RVA: 0x0029F913 File Offset: 0x0029DB13
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007ADF RID: 31455 RVA: 0x0029F918 File Offset: 0x0029DB18
		public override bool IsObtained()
		{
			List<SerializableCard> list = this._localPlayer.Deck.Where<SerializableCard>((SerializableCard c) => SaveUtil.CardOrDeprecated(c.Id).Rarity != CardRarity.Basic).ToList<SerializableCard>();
			return (from c in list
				group c by c.Id).Any<IGrouping<ModelId, SerializableCard>>((IGrouping<ModelId, SerializableCard> g) => g.Count<SerializableCard>() >= 5);
		}

		// Token: 0x04002900 RID: 10496
		private const int _sameCardAmount = 5;
	}
}
