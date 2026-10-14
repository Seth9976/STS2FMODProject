using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Have 25 or more relics on death (win or lose).
	/// </summary>
	// Token: 0x02000B6A RID: 2922
	public class ILikeShiny : Badge
	{
		/// <summary>
		/// Have 25 or more relics on death (win or lose).
		/// </summary>
		// Token: 0x06007AE0 RID: 31456 RVA: 0x0029F9A5 File Offset: 0x0029DBA5
		[NullableContext(1)]
		public ILikeShiny(SerializableRun run, bool won, ulong playerId)
			: base(run, won, playerId, "ILIKESHINY", false, false)
		{
		}

		// Token: 0x170021E6 RID: 8678
		// (get) Token: 0x06007AE1 RID: 31457 RVA: 0x0029F9B7 File Offset: 0x0029DBB7
		public override BadgeRarity Rarity
		{
			get
			{
				return BadgeRarity.Bronze;
			}
		}

		// Token: 0x06007AE2 RID: 31458 RVA: 0x0029F9BA File Offset: 0x0029DBBA
		public override bool IsObtained()
		{
			return this._localPlayer.Relics.Count >= 25;
		}

		// Token: 0x04002901 RID: 10497
		private const int _relicRequirement = 25;
	}
}
