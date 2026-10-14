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
	// Token: 0x0200061D RID: 1565
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefectRelicPool : RelicPoolModel
	{
		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x06005664 RID: 22116 RVA: 0x0024A757 File Offset: 0x00248957
		public override string EnergyColorName
		{
			get
			{
				return "defect";
			}
		}

		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x06005665 RID: 22117 RVA: 0x0024A75E File Offset: 0x0024895E
		public override Color LabOutlineColor
		{
			get
			{
				return StsColors.blue;
			}
		}

		// Token: 0x06005666 RID: 22118 RVA: 0x0024A768 File Offset: 0x00248968
		protected override IEnumerable<RelicModel> GenerateAllRelics()
		{
			return new <>z__ReadOnlyArray<RelicModel>(new RelicModel[]
			{
				ModelDb.Relic<CrackedCore>(),
				ModelDb.Relic<DataDisk>(),
				ModelDb.Relic<EmotionChip>(),
				ModelDb.Relic<GoldPlatedCables>(),
				ModelDb.Relic<PowerCell>(),
				ModelDb.Relic<Metronome>(),
				ModelDb.Relic<RunicCapacitor>(),
				ModelDb.Relic<SymbioticVirus>()
			});
		}

		// Token: 0x06005667 RID: 22119 RVA: 0x0024A7C0 File Offset: 0x002489C0
		public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
		{
			List<RelicModel> list = base.AllRelics.ToList<RelicModel>();
			if (!unlockState.IsEpochRevealed<Defect3Epoch>())
			{
				list.RemoveAll((RelicModel r) => Defect3Epoch.Relics.Any<RelicModel>((RelicModel relic) => relic.Id == r.Id));
			}
			if (!unlockState.IsEpochRevealed<Defect6Epoch>())
			{
				list.RemoveAll((RelicModel r) => Defect6Epoch.Relics.Any<RelicModel>((RelicModel relic) => relic.Id == r.Id));
			}
			return list;
		}
	}
}
