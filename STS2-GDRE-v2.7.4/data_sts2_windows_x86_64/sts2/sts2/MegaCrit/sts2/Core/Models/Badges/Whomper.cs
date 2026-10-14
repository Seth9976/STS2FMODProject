using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// THIS BADGE IS CURRENTLY NOT IN USE
	/// Deal more than 100 damage with a single card.
	/// DESIGN NOTE: Not a great badge because it overvalues a specific archetype.
	/// </summary>
	// Token: 0x02000B75 RID: 2933
	public class Whomper : Badge
	{
		/// <summary>
		/// THIS BADGE IS CURRENTLY NOT IN USE
		/// Deal more than 100 damage with a single card.
		/// DESIGN NOTE: Not a great badge because it overvalues a specific archetype.
		/// </summary>
		// Token: 0x06007B01 RID: 31489 RVA: 0x002A0272 File Offset: 0x0029E472
		[NullableContext(1)]
		public Whomper(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "WHOMPER", false, false)
		{
		}

		// Token: 0x170021F1 RID: 8689
		// (get) Token: 0x06007B02 RID: 31490 RVA: 0x002A0284 File Offset: 0x0029E484
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007B03 RID: 31491 RVA: 0x002A0287 File Offset: 0x0029E487
		public override bool IsObtained()
		{
			return false;
		}

		// Token: 0x04002907 RID: 10503
		private const float _damageRequirement = 100f;
	}
}
