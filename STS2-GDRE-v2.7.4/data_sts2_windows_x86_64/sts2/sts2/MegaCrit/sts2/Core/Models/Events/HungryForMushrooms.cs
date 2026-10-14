using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models.Relics;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200083D RID: 2109
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HungryForMushrooms : EventModel
	{
		// Token: 0x0600680F RID: 26639 RVA: 0x00275678 File Offset: 0x00273878
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				base.RelicOption<BigMushroom>(new Func<Task>(this.BigMushroom), "INITIAL"),
				base.RelicOption<FragrantMushroom>(new Func<Task>(this.FragrantMushroom), "INITIAL").ThatDoesDamage(15m)
			});
		}

		// Token: 0x06006810 RID: 26640 RVA: 0x002756D0 File Offset: 0x002738D0
		private async Task BigMushroom()
		{
			await RelicCmd.Obtain<BigMushroom>(base.Owner);
			base.SetEventFinished(base.L10NLookup("HUNGRY_FOR_MUSHROOMS.pages.BIG_MUSHROOM.description"));
		}

		// Token: 0x06006811 RID: 26641 RVA: 0x00275714 File Offset: 0x00273914
		private async Task FragrantMushroom()
		{
			await RelicCmd.Obtain<FragrantMushroom>(base.Owner);
			base.SetEventFinished(base.L10NLookup("HUNGRY_FOR_MUSHROOMS.pages.FRAGRANT_MUSHROOM.description"));
		}
	}
}
