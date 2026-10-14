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
	// Token: 0x02000623 RID: 1571
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RegentRelicPool : RelicPoolModel
	{
		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x0600567C RID: 22140 RVA: 0x0024AF86 File Offset: 0x00249186
		public override string EnergyColorName
		{
			get
			{
				return "regent";
			}
		}

		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x0600567D RID: 22141 RVA: 0x0024AF8D File Offset: 0x0024918D
		public override Color LabOutlineColor
		{
			get
			{
				return StsColors.orange;
			}
		}

		// Token: 0x0600567E RID: 22142 RVA: 0x0024AF94 File Offset: 0x00249194
		protected override IEnumerable<RelicModel> GenerateAllRelics()
		{
			return new <>z__ReadOnlyArray<RelicModel>(new RelicModel[]
			{
				ModelDb.Relic<DivineRight>(),
				ModelDb.Relic<FencingManual>(),
				ModelDb.Relic<GalacticDust>(),
				ModelDb.Relic<LunarPastry>(),
				ModelDb.Relic<MiniRegent>(),
				ModelDb.Relic<OrangeDough>(),
				ModelDb.Relic<Regalite>(),
				ModelDb.Relic<VitruvianMinion>()
			});
		}

		// Token: 0x0600567F RID: 22143 RVA: 0x0024AFEC File Offset: 0x002491EC
		public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
		{
			List<RelicModel> list = base.AllRelics.ToList<RelicModel>();
			if (!unlockState.IsEpochRevealed<Regent3Epoch>())
			{
				list.RemoveAll((RelicModel r) => Regent3Epoch.Relics.Any<RelicModel>((RelicModel relic) => relic.Id == r.Id));
			}
			if (!unlockState.IsEpochRevealed<Regent6Epoch>())
			{
				list.RemoveAll((RelicModel r) => Regent6Epoch.Relics.Any<RelicModel>((RelicModel relic) => relic.Id == r.Id));
			}
			return list;
		}
	}
}
