using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.PotionPools
{
	// Token: 0x0200078C RID: 1932
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SilentPotionPool : PotionPoolModel
	{
		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x06005EF3 RID: 24307 RVA: 0x00259D88 File Offset: 0x00257F88
		public override string EnergyColorName
		{
			get
			{
				return "silent";
			}
		}

		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x06005EF4 RID: 24308 RVA: 0x00259D8F File Offset: 0x00257F8F
		public override Color LabOutlineColor
		{
			get
			{
				return StsColors.green;
			}
		}

		// Token: 0x06005EF5 RID: 24309 RVA: 0x00259D96 File Offset: 0x00257F96
		protected override IEnumerable<PotionModel> GenerateAllPotions()
		{
			return Silent4Epoch.Potions;
		}

		/// <summary>
		/// Only return the Potions if the associated Epoch is revealed.
		/// NOTE: This needs to be updated if a character has more than 3 potions. See: Silent4Epoch.cs
		/// </summary>
		// Token: 0x06005EF6 RID: 24310 RVA: 0x00259DA0 File Offset: 0x00257FA0
		public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
		{
			if (!unlockState.IsEpochRevealed<Silent4Epoch>())
			{
				return Array.Empty<PotionModel>();
			}
			return this.GenerateAllPotions();
		}
	}
}
