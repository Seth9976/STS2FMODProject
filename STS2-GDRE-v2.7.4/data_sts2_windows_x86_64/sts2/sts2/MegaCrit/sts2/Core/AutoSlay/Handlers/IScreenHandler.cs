using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers
{
	/// <summary>
	/// Handler for a specific overlay screen type.
	/// </summary>
	// Token: 0x02000D74 RID: 3444
	[NullableContext(1)]
	public interface IScreenHandler : IHandler
	{
		/// <summary>The screen type this handler can handle.</summary>
		// Token: 0x1700262D RID: 9773
		// (get) Token: 0x06008B0D RID: 35597
		Type ScreenType { get; }
	}
}
