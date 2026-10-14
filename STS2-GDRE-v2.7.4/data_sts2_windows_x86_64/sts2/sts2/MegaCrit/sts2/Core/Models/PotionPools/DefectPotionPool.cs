using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.PotionPools
{
	// Token: 0x02000784 RID: 1924
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefectPotionPool : PotionPoolModel
	{
		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x06005ED1 RID: 24273 RVA: 0x002599AB File Offset: 0x00257BAB
		public override string EnergyColorName
		{
			get
			{
				return "defect";
			}
		}

		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x06005ED2 RID: 24274 RVA: 0x002599B2 File Offset: 0x00257BB2
		public override Color LabOutlineColor
		{
			get
			{
				return StsColors.blue;
			}
		}

		// Token: 0x06005ED3 RID: 24275 RVA: 0x002599B9 File Offset: 0x00257BB9
		protected override IEnumerable<PotionModel> GenerateAllPotions()
		{
			return Defect4Epoch.Potions;
		}

		/// <summary>
		/// Only return the Potions if the associated Epoch is revealed.
		/// NOTE: This needs to be updated if a character has more than 3 potions. See: Defect4Epoch.cs
		/// </summary>
		// Token: 0x06005ED4 RID: 24276 RVA: 0x002599C0 File Offset: 0x00257BC0
		public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
		{
			if (!unlockState.IsEpochRevealed<Defect4Epoch>())
			{
				return Array.Empty<PotionModel>();
			}
			return this.GenerateAllPotions();
		}
	}
}
