using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms
{
	/// <summary>
	/// Handles post-combat victory rooms where the proceed button is displayed.
	/// </summary>
	// Token: 0x02000D87 RID: 3463
	public class VictoryRoomHandler : IHandler
	{
		// Token: 0x17002652 RID: 9810
		// (get) Token: 0x06008B60 RID: 35680 RVA: 0x002E80FB File Offset: 0x002E62FB
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B61 RID: 35681 RVA: 0x002E8108 File Offset: 0x002E6308
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			AutoSlayLog.Action("Waiting for victory room");
			await WaitHelper.Until(() => NCombatRoom.Instance != null, ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Victory room not ready");
			NCombatRoom instance = NCombatRoom.Instance;
			NProceedButton proceedButton = instance.ProceedButton;
			await WaitHelper.Until(() => proceedButton.IsEnabled, ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Proceed button not enabled");
			AutoSlayLog.Action("Clicking proceed on victory");
			await UiHelper.Click(proceedButton, 100);
			AutoSlayLog.Action("Victory room completed");
		}
	}
}
