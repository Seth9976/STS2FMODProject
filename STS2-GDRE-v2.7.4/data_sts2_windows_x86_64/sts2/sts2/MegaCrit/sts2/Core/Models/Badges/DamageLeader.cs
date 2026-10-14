using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// The player who dealt the most damage in a multiplayer game gets this badge.
	/// </summary>
	// Token: 0x02000B60 RID: 2912
	public class DamageLeader : Badge
	{
		/// <summary>
		/// The player who dealt the most damage in a multiplayer game gets this badge.
		/// </summary>
		// Token: 0x06007AC3 RID: 31427 RVA: 0x0029F304 File Offset: 0x0029D504
		[NullableContext(1)]
		public DamageLeader(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "DAMAGE_LEADER", false, true)
		{
		}

		// Token: 0x170021DD RID: 8669
		// (get) Token: 0x06007AC4 RID: 31428 RVA: 0x0029F316 File Offset: 0x0029D516
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007AC5 RID: 31429 RVA: 0x0029F31C File Offset: 0x0029D51C
		public override bool IsObtained()
		{
			if (this._run.MapPointHistory.Count < 1 || this._run.MapPointHistory[0].Count < 5)
			{
				return false;
			}
			SerializablePlayer serializablePlayer = null;
			foreach (SerializablePlayer serializablePlayer2 in this._run.Players)
			{
				if (serializablePlayer == null || serializablePlayer2.ExtraFields.DamageDealt > serializablePlayer.ExtraFields.DamageDealt)
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
