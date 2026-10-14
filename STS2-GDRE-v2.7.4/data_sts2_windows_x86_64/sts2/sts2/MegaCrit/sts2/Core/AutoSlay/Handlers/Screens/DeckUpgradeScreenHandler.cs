using System;
using System.Collections.Generic;
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
	/// Handles the deck upgrade selection screen.
	/// </summary>
	// Token: 0x02000D7D RID: 3453
	public class DeckUpgradeScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x1700263E RID: 9790
		// (get) Token: 0x06008B2E RID: 35630 RVA: 0x002E791B File Offset: 0x002E5B1B
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NDeckUpgradeSelectScreen);
			}
		}

		// Token: 0x1700263F RID: 9791
		// (get) Token: 0x06008B2F RID: 35631 RVA: 0x002E7927 File Offset: 0x002E5B27
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B30 RID: 35632 RVA: 0x002E7934 File Offset: 0x002E5B34
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			DeckUpgradeScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals1 = new DeckUpgradeScreenHandler.<>c__DisplayClass4_0();
			AutoSlayLog.EnterScreen("NDeckUpgradeSelectScreen");
			CS$<>8__locals1.screen = AutoSlayer.GetCurrentScreen<NDeckUpgradeSelectScreen>();
			List<NGridCardHolder> cards = UiHelper.FindAll<NGridCardHolder>(CS$<>8__locals1.screen);
			if (cards.Count == 0)
			{
				AutoSlayLog.Warn("No cards found in upgrade screen");
			}
			else
			{
				int maxSelections = Math.Min(cards.Count, 5);
				int i = 0;
				while (i < maxSelections && GodotObject.IsInstanceValid(CS$<>8__locals1.screen) && CS$<>8__locals1.screen.IsVisibleInTree())
				{
					Control nodeOrNull = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%UpgradeSinglePreviewContainer");
					Control nodeOrNull2 = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%UpgradeMultiPreviewContainer");
					if ((nodeOrNull != null && nodeOrNull.Visible) || (nodeOrNull2 != null && nodeOrNull2.Visible))
					{
						break;
					}
					NGridCardHolder ngridCardHolder = random.NextItem<NGridCardHolder>(cards);
					AutoSlayLog.Action("Selecting card to upgrade");
					ngridCardHolder.EmitSignal(NCardHolder.SignalName.Pressed, new Variant[] { ngridCardHolder });
					cards.Remove(ngridCardHolder);
					await Task.Delay(300, ct);
					i++;
				}
				CS$<>8__locals1.visiblePreview = null;
				await WaitHelper.Until(delegate
				{
					Control nodeOrNull3 = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%UpgradeSinglePreviewContainer");
					Control nodeOrNull4 = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%UpgradeMultiPreviewContainer");
					if (nodeOrNull3 != null && nodeOrNull3.Visible)
					{
						CS$<>8__locals1.visiblePreview = nodeOrNull3;
						return true;
					}
					if (nodeOrNull4 != null && nodeOrNull4.Visible)
					{
						CS$<>8__locals1.visiblePreview = nodeOrNull4;
						return true;
					}
					return !GodotObject.IsInstanceValid(CS$<>8__locals1.screen) || !CS$<>8__locals1.screen.IsVisibleInTree();
				}, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Upgrade preview did not appear");
				if (!GodotObject.IsInstanceValid(CS$<>8__locals1.screen) || !CS$<>8__locals1.screen.IsVisibleInTree())
				{
					AutoSlayLog.ExitScreen("NDeckUpgradeSelectScreen");
				}
				else
				{
					DeckUpgradeScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals2 = CS$<>8__locals1;
					Control visiblePreview = CS$<>8__locals1.visiblePreview;
					CS$<>8__locals2.confirmButton = ((visiblePreview != null) ? visiblePreview.GetNodeOrNull<NConfirmButton>("Confirm") : null);
					if (CS$<>8__locals1.confirmButton == null)
					{
						AutoSlayLog.Error("Preview confirm button not found");
						AutoSlayLog.ExitScreen("NDeckUpgradeSelectScreen");
					}
					else
					{
						await WaitHelper.Until(() => CS$<>8__locals1.confirmButton.IsEnabled, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Upgrade confirm button did not become enabled");
						AutoSlayLog.Action("Confirming upgrade");
						await UiHelper.Click(CS$<>8__locals1.confirmButton, 100);
						await WaitHelper.Until(() => !GodotObject.IsInstanceValid(CS$<>8__locals1.screen) || !CS$<>8__locals1.screen.IsVisibleInTree(), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Upgrade screen did not close after confirmation");
						AutoSlayLog.ExitScreen("NDeckUpgradeSelectScreen");
					}
				}
			}
		}
	}
}
