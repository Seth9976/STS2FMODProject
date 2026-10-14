using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens
{
	/// <summary>
	/// Handles the deck card selection screen (removal, transform, etc).
	/// The screen has two confirm buttons: one for the main grid, and one in the preview container.
	/// When selecting exactly MaxSelect cards, the screen auto-shows the preview, so we need to click
	/// the preview confirm button, not the main one.
	/// </summary>
	// Token: 0x02000D7A RID: 3450
	public class DeckCardSelectScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x17002638 RID: 9784
		// (get) Token: 0x06008B22 RID: 35618 RVA: 0x002E77D7 File Offset: 0x002E59D7
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NDeckCardSelectScreen);
			}
		}

		// Token: 0x17002639 RID: 9785
		// (get) Token: 0x06008B23 RID: 35619 RVA: 0x002E77E3 File Offset: 0x002E59E3
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B24 RID: 35620 RVA: 0x002E77F0 File Offset: 0x002E59F0
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			DeckCardSelectScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals1 = new DeckCardSelectScreenHandler.<>c__DisplayClass4_0();
			AutoSlayLog.EnterScreen("NDeckCardSelectScreen");
			CS$<>8__locals1.screen = AutoSlayer.GetCurrentScreen<NDeckCardSelectScreen>();
			CS$<>8__locals1.cards = new List<NGridCardHolder>();
			await WaitHelper.Until(delegate
			{
				CS$<>8__locals1.cards = UiHelper.FindAll<NGridCardHolder>(CS$<>8__locals1.screen);
				return CS$<>8__locals1.cards.Count > 0;
			}, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "No cards found in card select screen");
			await Task.Delay(300, ct);
			int maxSelections = Math.Min(CS$<>8__locals1.cards.Count, 5);
			CS$<>8__locals1.selectedCards = new List<NGridCardHolder>();
			NConfirmButton nconfirmButton;
			for (int i = 0; i < maxSelections; i++)
			{
				CS$<>8__locals1.previewContainer = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%PreviewContainer");
				nconfirmButton = CS$<>8__locals1.screen.GetNodeOrNull<NConfirmButton>("%Confirm");
				Control previewContainer = CS$<>8__locals1.previewContainer;
				if ((previewContainer != null && previewContainer.Visible) || (nconfirmButton != null && nconfirmButton.IsEnabled))
				{
					break;
				}
				IEnumerable<NGridCardHolder> cards = CS$<>8__locals1.cards;
				Func<NGridCardHolder, bool> func;
				if ((func = CS$<>8__locals1.<>9__4) == null)
				{
					DeckCardSelectScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals2 = CS$<>8__locals1;
					Func<NGridCardHolder, bool> func2 = (NGridCardHolder c) => !CS$<>8__locals1.selectedCards.Contains(c);
					CS$<>8__locals2.<>9__4 = func2;
					func = func2;
				}
				List<NGridCardHolder> list = cards.Where<NGridCardHolder>(func).ToList<NGridCardHolder>();
				if (list.Count == 0)
				{
					break;
				}
				NGridCardHolder ngridCardHolder = random.NextItem<NGridCardHolder>(list);
				CS$<>8__locals1.selectedCards.Add(ngridCardHolder);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Selecting card ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i + 1);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.cards.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" cards available)");
				AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
				ngridCardHolder.EmitSignal(NCardHolder.SignalName.Pressed, new Variant[] { ngridCardHolder });
				await Task.Delay(200, ct);
			}
			CS$<>8__locals1.previewContainer = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%PreviewContainer");
			nconfirmButton = CS$<>8__locals1.screen.GetNodeOrNull<NConfirmButton>("%Confirm");
			Control previewContainer2 = CS$<>8__locals1.previewContainer;
			if ((previewContainer2 == null || !previewContainer2.Visible) && nconfirmButton != null && nconfirmButton.IsEnabled)
			{
				AutoSlayLog.Action("Clicking main confirm button to show preview");
				await UiHelper.Click(nconfirmButton, 100);
				await Task.Delay(200, ct);
			}
			await WaitHelper.Until(delegate
			{
				CS$<>8__locals1.previewContainer = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%PreviewContainer");
				Control previewContainer4 = CS$<>8__locals1.previewContainer;
				return previewContainer4 != null && previewContainer4.Visible;
			}, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Preview container did not appear");
			await Task.Delay(500, ct);
			DeckCardSelectScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals3 = CS$<>8__locals1;
			Control previewContainer3 = CS$<>8__locals1.previewContainer;
			CS$<>8__locals3.confirmButton = ((previewContainer3 != null) ? previewContainer3.GetNodeOrNull<NConfirmButton>("%PreviewConfirm") : null);
			if (CS$<>8__locals1.confirmButton == null)
			{
				CS$<>8__locals1.confirmButton = UiHelper.FindAll<NConfirmButton>(CS$<>8__locals1.screen).FirstOrDefault<NConfirmButton>((NConfirmButton b) => b.IsEnabled);
			}
			if (CS$<>8__locals1.confirmButton != null)
			{
				await WaitHelper.Until(() => CS$<>8__locals1.confirmButton.IsEnabled, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Card select confirm button did not become enabled");
				AutoSlayLog.Action("Confirming selection");
				await UiHelper.Click(CS$<>8__locals1.confirmButton, 100);
				await WaitHelper.Until(() => !GodotObject.IsInstanceValid(CS$<>8__locals1.screen) || !CS$<>8__locals1.screen.IsVisibleInTree(), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Card select screen did not close after confirmation");
			}
			else
			{
				AutoSlayLog.Error("No confirm button found on card select screen");
			}
			AutoSlayLog.ExitScreen("NDeckCardSelectScreen");
		}
	}
}
