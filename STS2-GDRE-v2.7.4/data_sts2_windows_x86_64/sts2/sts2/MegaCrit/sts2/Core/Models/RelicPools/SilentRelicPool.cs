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
	// Token: 0x02000625 RID: 1573
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SilentRelicPool : RelicPoolModel
	{
		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x06005685 RID: 22149 RVA: 0x0024B5B4 File Offset: 0x002497B4
		public override string EnergyColorName
		{
			get
			{
				return "silent";
			}
		}

		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x06005686 RID: 22150 RVA: 0x0024B5BB File Offset: 0x002497BB
		public override Color LabOutlineColor
		{
			get
			{
				return StsColors.green;
			}
		}

		// Token: 0x06005687 RID: 22151 RVA: 0x0024B5C4 File Offset: 0x002497C4
		protected override IEnumerable<RelicModel> GenerateAllRelics()
		{
			return new <>z__ReadOnlyArray<RelicModel>(new RelicModel[]
			{
				ModelDb.Relic<HelicalDart>(),
				ModelDb.Relic<NinjaScroll>(),
				ModelDb.Relic<PaperKrane>(),
				ModelDb.Relic<RingOfTheSnake>(),
				ModelDb.Relic<SneckoSkull>(),
				ModelDb.Relic<Tingsha>(),
				ModelDb.Relic<ToughBandages>(),
				ModelDb.Relic<TwistedFunnel>()
			});
		}

		// Token: 0x06005688 RID: 22152 RVA: 0x0024B61C File Offset: 0x0024981C
		public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
		{
			List<RelicModel> list = base.AllRelics.ToList<RelicModel>();
			if (!unlockState.IsEpochRevealed<Silent3Epoch>())
			{
				list.RemoveAll((RelicModel r) => Silent3Epoch.Relics.Any<RelicModel>((RelicModel relic) => relic.Id == r.Id));
			}
			if (!unlockState.IsEpochRevealed<Silent6Epoch>())
			{
				list.RemoveAll((RelicModel r) => Silent6Epoch.Relics.Any<RelicModel>((RelicModel relic) => relic.Id == r.Id));
			}
			return list;
		}
	}
}
