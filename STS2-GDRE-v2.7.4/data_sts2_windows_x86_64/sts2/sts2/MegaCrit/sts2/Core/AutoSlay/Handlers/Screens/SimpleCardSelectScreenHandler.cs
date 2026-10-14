using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens
{
	/// <summary>
	/// Handles simple card selection screens.
	/// Some screens require multiple card selections and/or manual confirmation.
	/// </summary>
	// Token: 0x02000D81 RID: 3457
	public class SimpleCardSelectScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x17002646 RID: 9798
		// (get) Token: 0x06008B40 RID: 35648 RVA: 0x002E7B87 File Offset: 0x002E5D87
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NSimpleCardSelectScreen);
			}
		}

		// Token: 0x17002647 RID: 9799
		// (get) Token: 0x06008B41 RID: 35649 RVA: 0x002E7B93 File Offset: 0x002E5D93
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B42 RID: 35650 RVA: 0x002E7BA0 File Offset: 0x002E5DA0
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			SimpleCardSelectScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals1 = new SimpleCardSelectScreenHandler.<>c__DisplayClass4_0();
			AutoSlayLog.EnterScreen("NSimpleCardSelectScreen");
			CS$<>8__locals1.screen = AutoSlayer.GetCurrentScreen<NSimpleCardSelectScreen>();
			NCardGrid grid = UiHelper.FindFirst<NCardGrid>(CS$<>8__locals1.screen);
			if (grid == null)
			{
				AutoSlayLog.Error("Card grid not found in simple card select screen");
			}
			else
			{
				NConfirmButton confirmButton = CS$<>8__locals1.screen.GetNodeOrNull<NConfirmButton>("%Confirm");
				CS$<>8__locals1.selectedCards = new HashSet<NGridCardHolder>();
				for (int i = 0; i < 10; i++)
				{
					ct.ThrowIfCancellationRequested();
					if (!GodotObject.IsInstanceValid(CS$<>8__locals1.screen) || !CS$<>8__locals1.screen.IsVisibleInTree())
					{
						AutoSlayLog.Info("Screen auto-closed after selections");
						break;
					}
					if (confirmButton != null && confirmButton.IsEnabled)
					{
						AutoSlayLog.Action("Clicking confirm button");
						await UiHelper.Click(confirmButton, 100);
						break;
					}
					IEnumerable<NGridCardHolder> enumerable = UiHelper.FindAll<NGridCardHolder>(CS$<>8__locals1.screen);
					Func<NGridCardHolder, bool> func;
					if ((func = CS$<>8__locals1.<>9__1) == null)
					{
						SimpleCardSelectScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals2 = CS$<>8__locals1;
						Func<NGridCardHolder, bool> func2 = (NGridCardHolder c) => !CS$<>8__locals1.selectedCards.Contains(c);
						CS$<>8__locals2.<>9__1 = func2;
						func = func2;
					}
					List<NGridCardHolder> list = enumerable.Where<NGridCardHolder>(func).ToList<NGridCardHolder>();
					if (list.Count == 0)
					{
						AutoSlayLog.Warn("No more cards available to select");
						break;
					}
					NGridCardHolder ngridCardHolder = random.NextItem<NGridCardHolder>(list);
					CS$<>8__locals1.selectedCards.Add(ngridCardHolder);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Selecting card (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.selectedCards.Count);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
					grid.EmitSignal(NCardGrid.SignalName.HolderPressed, new Variant[] { ngridCardHolder });
					await Task.Delay(300, ct);
				}
				await WaitHelper.Until(() => !GodotObject.IsInstanceValid(CS$<>8__locals1.screen) || !CS$<>8__locals1.screen.IsVisibleInTree(), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Simple card select screen did not close");
				AutoSlayLog.ExitScreen("NSimpleCardSelectScreen");
			}
		}
	}
}
