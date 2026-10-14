using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.RelicPools
{
	// Token: 0x02000622 RID: 1570
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NecrobinderRelicPool : RelicPoolModel
	{
		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x06005677 RID: 22135 RVA: 0x0024AEA2 File Offset: 0x002490A2
		public override string EnergyColorName
		{
			get
			{
				return "necrobinder";
			}
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x06005678 RID: 22136 RVA: 0x0024AEA9 File Offset: 0x002490A9
		public override Color LabOutlineColor
		{
			get
			{
				return StsColors.pink;
			}
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x0024AEB0 File Offset: 0x002490B0
		protected override IEnumerable<RelicModel> GenerateAllRelics()
		{
			return new <>z__ReadOnlyArray<RelicModel>(new RelicModel[]
			{
				ModelDb.Relic<BigHat>(),
				ModelDb.Relic<BoneFlute>(),
				ModelDb.Relic<BookRepairKnife>(),
				ModelDb.Relic<Bookmark>(),
				ModelDb.Relic<BoundPhylactery>(),
				ModelDb.Relic<FuneraryMask>(),
				ModelDb.Relic<IvoryTile>(),
				ModelDb.Relic<UndyingSigil>()
			});
		}

		// Token: 0x0600567A RID: 22138 RVA: 0x0024AF08 File Offset: 0x00249108
		public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
		{
			List<RelicModel> list = base.AllRelics.ToList<RelicModel>();
			if (!unlockState.IsEpochRevealed<Necrobinder3Epoch>())
			{
				list.RemoveAll((RelicModel r) => Necrobinder3Epoch.Relics.Any<RelicModel>((RelicModel relic) => relic.Id == r.Id));
			}
			if (!unlockState.IsEpochRevealed<Necrobinder6Epoch>())
			{
				list.RemoveAll((RelicModel r) => Necrobinder6Epoch.Relics.Any<RelicModel>((RelicModel relic) => relic.Id == r.Id));
			}
			return list;
		}
	}
}
