using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens
{
	/// <summary>
	/// Handles the card reward selection screen after combat.
	/// </summary>
	// Token: 0x02000D75 RID: 3445
	public class CardRewardScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x1700262E RID: 9774
		// (get) Token: 0x06008B0E RID: 35598 RVA: 0x002E75C4 File Offset: 0x002E57C4
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NCardRewardSelectionScreen);
			}
		}

		// Token: 0x1700262F RID: 9775
		// (get) Token: 0x06008B0F RID: 35599 RVA: 0x002E75D0 File Offset: 0x002E57D0
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B10 RID: 35600 RVA: 0x002E75DC File Offset: 0x002E57DC
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			AutoSlayLog.EnterScreen("NCardRewardSelectionScreen");
			NCardRewardSelectionScreen screen = AutoSlayer.GetCurrentScreen<NCardRewardSelectionScreen>();
			await Task.Delay(400, ct);
			List<NCardHolder> list = UiHelper.FindAll<NCardHolder>(screen);
			if (list.Count == 0)
			{
				AutoSlayLog.Warn("No card holders found in card reward screen");
			}
			else
			{
				NCardHolder ncardHolder = random.NextItem<NCardHolder>(list);
				AutoSlayLog.Action("Selecting card reward");
				ncardHolder.EmitSignal(NCardHolder.SignalName.Pressed, new Variant[] { ncardHolder });
				await WaitHelper.Until(() => !GodotObject.IsInstanceValid(screen) || !screen.IsVisibleInTree(), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Card reward screen did not close after selection");
				AutoSlayLog.ExitScreen("NCardRewardSelectionScreen");
			}
		}
	}
}
