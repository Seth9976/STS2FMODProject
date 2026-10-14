using System;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Obtained both fake and real Snecko Eyes.
	/// </summary>
	// Token: 0x02000B63 RID: 2915
	public class DoubleSnecko : Badge
	{
		/// <summary>
		/// Obtained both fake and real Snecko Eyes.
		/// </summary>
		// Token: 0x06007ACB RID: 31435 RVA: 0x0029F548 File Offset: 0x0029D748
		[NullableContext(1)]
		public DoubleSnecko(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "DOUBLE_SNECKO", false, false)
		{
		}

		// Token: 0x170021DF RID: 8671
		// (get) Token: 0x06007ACC RID: 31436 RVA: 0x0029F55A File Offset: 0x0029D75A
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007ACD RID: 31437 RVA: 0x0029F560 File Offset: 0x0029D760
		public override bool IsObtained()
		{
			if (this._localPlayer.Relics.Any<SerializableRelic>((SerializableRelic r) => r.Id == ModelDb.Relic<SneckoEye>().Id))
			{
				return this._localPlayer.Relics.Any<SerializableRelic>((SerializableRelic r) => r.Id == ModelDb.Relic<FakeSneckoEye>().Id);
			}
			return false;
		}
	}
}
