using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.PotionPools
{
	// Token: 0x0200078A RID: 1930
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RegentPotionPool : PotionPoolModel
	{
		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x06005EEA RID: 24298 RVA: 0x00259ADB File Offset: 0x00257CDB
		public override string EnergyColorName
		{
			get
			{
				return "regent";
			}
		}

		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x06005EEB RID: 24299 RVA: 0x00259AE2 File Offset: 0x00257CE2
		public override Color LabOutlineColor
		{
			get
			{
				return StsColors.orange;
			}
		}

		// Token: 0x06005EEC RID: 24300 RVA: 0x00259AE9 File Offset: 0x00257CE9
		protected override IEnumerable<PotionModel> GenerateAllPotions()
		{
			return Regent4Epoch.Potions;
		}

		/// <summary>
		/// Only return the Potions if the associated Epoch is revealed.
		/// NOTE: This needs to be updated if a character has more than 3 potions. See: Regent4Epoch.cs
		/// </summary>
		// Token: 0x06005EED RID: 24301 RVA: 0x00259AF0 File Offset: 0x00257CF0
		public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
		{
			if (!unlockState.IsEpochRevealed<Regent4Epoch>())
			{
				return Array.Empty<PotionModel>();
			}
			return this.GenerateAllPotions();
		}
	}
}
