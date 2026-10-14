using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.PotionPools
{
	// Token: 0x02000787 RID: 1927
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IroncladPotionPool : PotionPoolModel
	{
		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x06005EDC RID: 24284 RVA: 0x00259A3A File Offset: 0x00257C3A
		public override string EnergyColorName
		{
			get
			{
				return "ironclad";
			}
		}

		// Token: 0x170015D6 RID: 5590
		// (get) Token: 0x06005EDD RID: 24285 RVA: 0x00259A41 File Offset: 0x00257C41
		public override Color LabOutlineColor
		{
			get
			{
				return StsColors.red;
			}
		}

		// Token: 0x06005EDE RID: 24286 RVA: 0x00259A48 File Offset: 0x00257C48
		protected override IEnumerable<PotionModel> GenerateAllPotions()
		{
			return Ironclad4Epoch.Potions;
		}

		/// <summary>
		/// Only return the Potions if the associated Epoch is revealed.
		/// NOTE: This needs to be updated if a character has more than 3 potions. See: Ironclad4Epoch.cs
		/// </summary>
		// Token: 0x06005EDF RID: 24287 RVA: 0x00259A50 File Offset: 0x00257C50
		public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
		{
			if (!unlockState.IsEpochRevealed<Ironclad4Epoch>())
			{
				return Array.Empty<PotionModel>();
			}
			return this.GenerateAllPotions();
		}
	}
}
