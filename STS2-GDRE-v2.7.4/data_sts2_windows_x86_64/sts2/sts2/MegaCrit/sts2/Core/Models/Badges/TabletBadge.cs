using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Won with 1 Max HP.
	/// </summary>
	// Token: 0x02000B72 RID: 2930
	public class TabletBadge : Badge
	{
		/// <summary>
		/// Won with 1 Max HP.
		/// </summary>
		// Token: 0x06007AF8 RID: 31480 RVA: 0x002A0166 File Offset: 0x0029E366
		[NullableContext(1)]
		public TabletBadge(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "TABLET", true, false)
		{
		}

		// Token: 0x170021EE RID: 8686
		// (get) Token: 0x06007AF9 RID: 31481 RVA: 0x002A0178 File Offset: 0x0029E378
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Gold;
			}
		}

		// Token: 0x06007AFA RID: 31482 RVA: 0x002A017B File Offset: 0x0029E37B
		public override bool IsObtained()
		{
			return this._localPlayer.MaxHp == 1;
		}
	}
}
