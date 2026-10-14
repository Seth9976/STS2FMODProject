using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Events
{
	// Token: 0x020003B7 RID: 951
	[NullableContext(1)]
	public interface ICustomEventNode : IScreenContext
	{
		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600392B RID: 14635
		IScreenContext CurrentScreenContext { get; }

		// Token: 0x0600392C RID: 14636
		void Initialize(EventModel eventModel);
	}
}
