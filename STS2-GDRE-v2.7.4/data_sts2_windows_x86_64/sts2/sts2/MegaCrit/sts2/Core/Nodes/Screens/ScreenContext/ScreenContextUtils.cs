using System;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext
{
	// Token: 0x020002CB RID: 715
	public static class ScreenContextUtils
	{
		// Token: 0x060025CD RID: 9677 RVA: 0x000FF7A2 File Offset: 0x000FD9A2
		[NullableContext(1)]
		public static void UpdateControllerNavEnabled<[Nullable(0)] T>(this T screenContext) where T : Control, IScreenContext
		{
			if (ActiveScreenContext.Instance.IsCurrent(screenContext))
			{
				screenContext.FocusBehaviorRecursive = 2L;
				return;
			}
			screenContext.FocusBehaviorRecursive = 1L;
		}
	}
}
