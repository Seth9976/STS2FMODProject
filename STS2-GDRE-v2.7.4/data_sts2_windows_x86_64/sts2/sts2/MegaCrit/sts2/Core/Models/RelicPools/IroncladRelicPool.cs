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
	// Token: 0x02000621 RID: 1569
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IroncladRelicPool : RelicPoolModel
	{
		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x06005672 RID: 22130 RVA: 0x0024ADBE File Offset: 0x00248FBE
		public override string EnergyColorName
		{
			get
			{
				return "ironclad";
			}
		}

		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x06005673 RID: 22131 RVA: 0x0024ADC5 File Offset: 0x00248FC5
		public override Color LabOutlineColor
		{
			get
			{
				return StsColors.red;
			}
		}

		// Token: 0x06005674 RID: 22132 RVA: 0x0024ADCC File Offset: 0x00248FCC
		protected override IEnumerable<RelicModel> GenerateAllRelics()
		{
			return new <>z__ReadOnlyArray<RelicModel>(new RelicModel[]
			{
				ModelDb.Relic<Brimstone>(),
				ModelDb.Relic<BurningBlood>(),
				ModelDb.Relic<CharonsAshes>(),
				ModelDb.Relic<DemonTongue>(),
				ModelDb.Relic<PaperPhrog>(),
				ModelDb.Relic<RedSkull>(),
				ModelDb.Relic<RuinedHelmet>(),
				ModelDb.Relic<SelfFormingClay>()
			});
		}

		// Token: 0x06005675 RID: 22133 RVA: 0x0024AE24 File Offset: 0x00249024
		public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
		{
			List<RelicModel> list = base.AllRelics.ToList<RelicModel>();
			if (!unlockState.IsEpochRevealed<Ironclad3Epoch>())
			{
				list.RemoveAll((RelicModel r) => Ironclad3Epoch.Relics.Any<RelicModel>((RelicModel relic) => relic.Id == r.Id));
			}
			if (!unlockState.IsEpochRevealed<Ironclad6Epoch>())
			{
				list.RemoveAll((RelicModel r) => Ironclad6Epoch.Relics.Any<RelicModel>((RelicModel relic) => relic.Id == r.Id));
			}
			return list;
		}
	}
}
