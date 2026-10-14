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
	/// Handles in-combat card selection screens (e.g., from Attack Potion).
	/// </summary>
	// Token: 0x02000D77 RID: 3447
	public class ChooseACardScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x17002632 RID: 9778
		// (get) Token: 0x06008B16 RID: 35606 RVA: 0x002E769B File Offset: 0x002E589B
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NChooseACardSelectionScreen);
			}
		}

		// Token: 0x17002633 RID: 9779
		// (get) Token: 0x06008B17 RID: 35607 RVA: 0x002E76A7 File Offset: 0x002E58A7
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B18 RID: 35608 RVA: 0x002E76B4 File Offset: 0x002E58B4
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			AutoSlayLog.EnterScreen("NChooseACardSelectionScreen");
			NChooseACardSelectionScreen currentScreen = AutoSlayer.GetCurrentScreen<NChooseACardSelectionScreen>();
			List<NCardHolder> list = UiHelper.FindAll<NCardHolder>(currentScreen);
			if (list.Count == 0)
			{
				AutoSlayLog.Warn("No card holders found in choose-a-card screen");
			}
			else
			{
				NCardHolder ncardHolder = random.NextItem<NCardHolder>(list);
				AutoSlayLog.Action("Selecting card");
				ncardHolder.EmitSignal(NCardHolder.SignalName.Pressed, new Variant[] { ncardHolder });
				await Task.Delay(100, ct);
				AutoSlayLog.ExitScreen("NChooseACardSelectionScreen");
			}
		}
	}
}
