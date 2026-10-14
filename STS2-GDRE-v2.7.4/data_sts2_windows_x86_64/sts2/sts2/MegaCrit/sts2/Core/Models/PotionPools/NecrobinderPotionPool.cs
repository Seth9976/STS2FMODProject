using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.PotionPools
{
	// Token: 0x02000789 RID: 1929
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NecrobinderPotionPool : PotionPoolModel
	{
		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x06005EE5 RID: 24293 RVA: 0x00259A99 File Offset: 0x00257C99
		public override string EnergyColorName
		{
			get
			{
				return "necrobinder";
			}
		}

		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x06005EE6 RID: 24294 RVA: 0x00259AA0 File Offset: 0x00257CA0
		public override Color LabOutlineColor
		{
			get
			{
				return StsColors.pink;
			}
		}

		// Token: 0x06005EE7 RID: 24295 RVA: 0x00259AA7 File Offset: 0x00257CA7
		protected override IEnumerable<PotionModel> GenerateAllPotions()
		{
			return Necrobinder4Epoch.Potions;
		}

		/// <summary>
		/// Only return the Potions if the associated Epoch is revealed.
		/// </summary>
		// Token: 0x06005EE8 RID: 24296 RVA: 0x00259AB0 File Offset: 0x00257CB0
		public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
		{
			if (!unlockState.IsEpochRevealed<Necrobinder4Epoch>())
			{
				return Array.Empty<PotionModel>();
			}
			return this.GenerateAllPotions();
		}
	}
}
