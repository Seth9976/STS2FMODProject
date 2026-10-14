using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000859 RID: 2137
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TeaMaster : EventModel
	{
		// Token: 0x0600690E RID: 26894 RVA: 0x0027A66B File Offset: 0x0027886B
		public override bool IsAllowed(IRunState runState)
		{
			if (runState.CurrentActIndex < 2)
			{
				return runState.Players.All<Player>((Player p) => p.Gold >= 150);
			}
			return false;
		}

		// Token: 0x170019D5 RID: 6613
		// (get) Token: 0x0600690F RID: 26895 RVA: 0x0027A6A4 File Offset: 0x002788A4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("BoneTeaCost", 50m),
					new DynamicVar("EmberTeaCost", 150m),
					new StringVar("BoneTeaDescription", ModelDb.Relic<BoneTea>().DynamicDescription.GetFormattedText()),
					new StringVar("EmberTeaDescription", ModelDb.Relic<EmberTea>().DynamicDescription.GetFormattedText()),
					new StringVar("TeaOfDiscourtesyDescription", ModelDb.Relic<TeaOfDiscourtesy>().DynamicDescription.GetFormattedText())
				});
			}
		}

		// Token: 0x06006910 RID: 26896 RVA: 0x0027A73C File Offset: 0x0027893C
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			List<EventOption> list = new List<EventOption>();
			if (base.Owner.Gold >= base.DynamicVars["BoneTeaCost"].BaseValue)
			{
				list.Add(new EventOption(this, new Func<Task>(this.BoneTea), "TEA_MASTER.pages.INITIAL.options.BONE_TEA", HoverTipFactory.FromRelicExcludingItself<BoneTea>()));
			}
			else
			{
				list.Add(new EventOption(this, null, "TEA_MASTER.pages.INITIAL.options.BONE_TEA_LOCKED", Array.Empty<IHoverTip>()));
			}
			if (base.Owner.Gold >= base.DynamicVars["EmberTeaCost"].BaseValue)
			{
				list.Add(new EventOption(this, new Func<Task>(this.EmberTea), "TEA_MASTER.pages.INITIAL.options.EMBER_TEA", HoverTipFactory.FromRelicExcludingItself<EmberTea>()));
			}
			else
			{
				list.Add(new EventOption(this, null, "TEA_MASTER.pages.INITIAL.options.EMBER_TEA_LOCKED", Array.Empty<IHoverTip>()));
			}
			list.Add(new EventOption(this, new Func<Task>(this.TeaOfDiscourtesy), "TEA_MASTER.pages.INITIAL.options.TEA_OF_DISCOURTESY", HoverTipFactory.FromRelicExcludingItself<TeaOfDiscourtesy>()));
			return list;
		}

		// Token: 0x06006911 RID: 26897 RVA: 0x0027A840 File Offset: 0x00278A40
		private async Task BoneTea()
		{
			await PlayerCmd.LoseGold(base.DynamicVars["BoneTeaCost"].BaseValue, base.Owner, GoldLossType.Spent);
			await RelicCmd.Obtain<BoneTea>(base.Owner);
			base.SetEventFinished(base.L10NLookup("TEA_MASTER.pages.DONE.description"));
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x0027A884 File Offset: 0x00278A84
		private async Task EmberTea()
		{
			await PlayerCmd.LoseGold(base.DynamicVars["EmberTeaCost"].BaseValue, base.Owner, GoldLossType.Spent);
			await RelicCmd.Obtain<EmberTea>(base.Owner);
			base.SetEventFinished(base.L10NLookup("TEA_MASTER.pages.DONE.description"));
		}

		// Token: 0x06006913 RID: 26899 RVA: 0x0027A8C8 File Offset: 0x00278AC8
		private async Task TeaOfDiscourtesy()
		{
			await RelicCmd.Obtain<TeaOfDiscourtesy>(base.Owner);
			base.SetEventFinished(base.L10NLookup("TEA_MASTER.pages.TEA_OF_DISCOURTESY.description"));
		}

		// Token: 0x040027CB RID: 10187
		private const string _boneTeaCostKey = "BoneTeaCost";

		// Token: 0x040027CC RID: 10188
		private const string _emberTeaCostKey = "EmberTeaCost";

		// Token: 0x040027CD RID: 10189
		private const int _boneTeaCost = 50;

		// Token: 0x040027CE RID: 10190
		private const int _emberTeaCost = 150;
	}
}
