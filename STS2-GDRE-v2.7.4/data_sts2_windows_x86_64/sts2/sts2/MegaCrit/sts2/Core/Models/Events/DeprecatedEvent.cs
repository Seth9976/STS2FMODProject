using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Events;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000835 RID: 2101
	public sealed class DeprecatedEvent : EventModel
	{
		// Token: 0x060067C1 RID: 26561 RVA: 0x002741B3 File Offset: 0x002723B3
		[NullableContext(1)]
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return Array.Empty<EventOption>();
		}
	}
}
