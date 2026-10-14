using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	// Token: 0x02000B61 RID: 2913
	public class Debuffer : Badge
	{
		// Token: 0x06007AC6 RID: 31430 RVA: 0x0029F3F8 File Offset: 0x0029D5F8
		[NullableContext(1)]
		public Debuffer(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "DEBUFFER", false, true)
		{
		}

		// Token: 0x170021DE RID: 8670
		// (get) Token: 0x06007AC7 RID: 31431 RVA: 0x0029F40A File Offset: 0x0029D60A
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007AC8 RID: 31432 RVA: 0x0029F410 File Offset: 0x0029D610
		public override bool IsObtained()
		{
			if (this._run.MapPointHistory.Count < 1 || this._run.MapPointHistory[0].Count < 5)
			{
				return false;
			}
			SerializablePlayer serializablePlayer = null;
			foreach (SerializablePlayer serializablePlayer2 in this._run.Players)
			{
				if (serializablePlayer == null || serializablePlayer2.ExtraFields.DebuffsApplied > serializablePlayer.ExtraFields.DebuffsApplied)
				{
					serializablePlayer = serializablePlayer2;
				}
			}
			ulong? num = ((serializablePlayer != null) ? new ulong?(serializablePlayer.NetId) : null);
			ulong netId = this._localPlayer.NetId;
			return (num.GetValueOrDefault() == netId) & (num != null);
		}
	}
}
