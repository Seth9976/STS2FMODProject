using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000834 RID: 2100
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeprecatedAncientEvent : AncientEventModel
	{
		// Token: 0x060067BD RID: 26557 RVA: 0x00274196 File Offset: 0x00272396
		protected override AncientDialogueSet DefineDialogues()
		{
			throw new NotImplementedException();
		}

		// Token: 0x17001973 RID: 6515
		// (get) Token: 0x060067BE RID: 26558 RVA: 0x0027419D File Offset: 0x0027239D
		public override IEnumerable<EventOption> AllPossibleOptions
		{
			get
			{
				return Array.Empty<EventOption>();
			}
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x002741A4 File Offset: 0x002723A4
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return Array.Empty<EventOption>();
		}
	}
}
