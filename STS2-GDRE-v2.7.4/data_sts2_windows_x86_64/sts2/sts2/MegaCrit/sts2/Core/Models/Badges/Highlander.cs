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
	/// Every card in the deck is unique (excluding Starters).
	/// </summary>
	// Token: 0x02000B68 RID: 2920
	public class Highlander : Badge
	{
		/// <summary>
		/// Every card in the deck is unique (excluding Starters).
		/// </summary>
		// Token: 0x06007ADA RID: 31450 RVA: 0x0029F873 File Offset: 0x0029DA73
		[NullableContext(1)]
		public Highlander(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "HIGHLANDER", true, false)
		{
		}

		// Token: 0x170021E4 RID: 8676
		// (get) Token: 0x06007ADB RID: 31451 RVA: 0x0029F885 File Offset: 0x0029DA85
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007ADC RID: 31452 RVA: 0x0029F888 File Offset: 0x0029DA88
		public override bool IsObtained()
		{
			List<SerializableCard> list = this._localPlayer.Deck.Where<SerializableCard>((SerializableCard c) => SaveUtil.CardOrDeprecated(c.Id).Rarity != CardRarity.Basic).ToList<SerializableCard>();
			return list.Select<SerializableCard, ModelId>((SerializableCard card) => card.Id).Distinct<ModelId>().Count<ModelId>() == list.Count;
		}
	}
}
