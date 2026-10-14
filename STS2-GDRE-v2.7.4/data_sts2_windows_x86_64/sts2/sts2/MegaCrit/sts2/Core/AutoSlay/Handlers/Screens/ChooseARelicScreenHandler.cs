using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens
{
	/// <summary>
	/// Handles relic selection screens (e.g., from Jewelry Box event).
	/// </summary>
	// Token: 0x02000D78 RID: 3448
	public class ChooseARelicScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x17002634 RID: 9780
		// (get) Token: 0x06008B1A RID: 35610 RVA: 0x002E7707 File Offset: 0x002E5907
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NChooseARelicSelection);
			}
		}

		// Token: 0x17002635 RID: 9781
		// (get) Token: 0x06008B1B RID: 35611 RVA: 0x002E7713 File Offset: 0x002E5913
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B1C RID: 35612 RVA: 0x002E7720 File Offset: 0x002E5920
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			AutoSlayLog.EnterScreen("NChooseARelicSelection");
			NChooseARelicSelection currentScreen = AutoSlayer.GetCurrentScreen<NChooseARelicSelection>();
			List<NClickableControl> list = UiHelper.FindAll<NClickableControl>(currentScreen);
			if (list.Count == 0)
			{
				AutoSlayLog.Warn("No clickable elements found in relic selection screen");
			}
			else
			{
				NClickableControl nclickableControl = random.NextItem<NClickableControl>(list);
				AutoSlayLog.Action("Selecting relic");
				await UiHelper.Click(nclickableControl, 100);
				AutoSlayLog.ExitScreen("NChooseARelicSelection");
			}
		}
	}
}
