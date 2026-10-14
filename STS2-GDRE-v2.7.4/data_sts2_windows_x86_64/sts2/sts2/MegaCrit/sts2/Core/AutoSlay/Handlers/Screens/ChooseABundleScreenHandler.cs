using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens
{
	/// <summary>
	/// Handles bundle card selection screens.
	/// </summary>
	// Token: 0x02000D76 RID: 3446
	public class ChooseABundleScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x17002630 RID: 9776
		// (get) Token: 0x06008B12 RID: 35602 RVA: 0x002E762F File Offset: 0x002E582F
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NChooseABundleSelectionScreen);
			}
		}

		// Token: 0x17002631 RID: 9777
		// (get) Token: 0x06008B13 RID: 35603 RVA: 0x002E763B File Offset: 0x002E583B
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B14 RID: 35604 RVA: 0x002E7648 File Offset: 0x002E5848
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			AutoSlayLog.EnterScreen("NChooseABundleSelectionScreen");
			NChooseABundleSelectionScreen screen = AutoSlayer.GetCurrentScreen<NChooseABundleSelectionScreen>();
			List<NCardBundle> list = UiHelper.FindAll<NCardBundle>(screen);
			if (list.Count == 0)
			{
				AutoSlayLog.Warn("No bundles found in bundle selection screen");
			}
			else
			{
				NCardBundle ncardBundle = random.NextItem<NCardBundle>(list);
				AutoSlayLog.Action("Selecting card bundle");
				await UiHelper.Click(ncardBundle.Hitbox, 100);
				await Task.Delay(500, ct);
				NConfirmButton nconfirmButton = UiHelper.FindFirst<NConfirmButton>(screen);
				if (nconfirmButton != null)
				{
					AutoSlayLog.Action("Confirming bundle selection");
					await UiHelper.Click(nconfirmButton, 100);
					await WaitHelper.Until(() => !GodotObject.IsInstanceValid(screen) || !screen.IsVisibleInTree(), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Bundle selection screen did not close after confirmation");
				}
				AutoSlayLog.ExitScreen("NChooseABundleSelectionScreen");
			}
		}
	}
}
