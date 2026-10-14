using System;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	/// <summary>
	/// Represents a relic that has been removed from the game. Mostly used for the run history.
	/// </summary>
	// Token: 0x0200052C RID: 1324
	public sealed class DeprecatedRelic : RelicModel
	{
		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06005025 RID: 20517 RVA: 0x0023EA66 File Offset: 0x0023CC66
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.None;
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06005026 RID: 20518 RVA: 0x0023EA69 File Offset: 0x0023CC69
		public override bool IsStackable
		{
			get
			{
				return true;
			}
		}
	}
}
