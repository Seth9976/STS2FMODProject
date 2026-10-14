using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Play 20 cards in a single turn.
	/// </summary>
	// Token: 0x02000B5D RID: 2909
	public class CccCombo : Badge
	{
		/// <summary>
		/// Play 20 cards in a single turn.
		/// </summary>
		// Token: 0x06007ABA RID: 31418 RVA: 0x0029F1C3 File Offset: 0x0029D3C3
		[NullableContext(1)]
		public CccCombo(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "CCCCOMBO", false, false)
		{
		}

		// Token: 0x170021DB RID: 8667
		// (get) Token: 0x06007ABB RID: 31419 RVA: 0x0029F1D5 File Offset: 0x0029D3D5
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007ABC RID: 31420 RVA: 0x0029F1D8 File Offset: 0x0029D3D8
		public override bool IsObtained()
		{
			return this._localPlayer.ExtraFields.CccomboBadgeUnlocked;
		}
	}
}
