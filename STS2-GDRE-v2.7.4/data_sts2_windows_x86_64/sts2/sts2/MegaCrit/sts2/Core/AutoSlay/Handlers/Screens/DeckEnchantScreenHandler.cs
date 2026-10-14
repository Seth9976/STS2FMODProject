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
	/// Handles the deck enchant selection screen.
	/// </summary>
	// Token: 0x02000D7B RID: 3451
	public class DeckEnchantScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x1700263A RID: 9786
		// (get) Token: 0x06008B26 RID: 35622 RVA: 0x002E7843 File Offset: 0x002E5A43
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NDeckEnchantSelectScreen);
			}
		}

		// Token: 0x1700263B RID: 9787
		// (get) Token: 0x06008B27 RID: 35623 RVA: 0x002E784F File Offset: 0x002E5A4F
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B28 RID: 35624 RVA: 0x002E785C File Offset: 0x002E5A5C
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			DeckEnchantScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals1 = new DeckEnchantScreenHandler.<>c__DisplayClass4_0();
			AutoSlayLog.EnterScreen("NDeckEnchantSelectScreen");
			CS$<>8__locals1.screen = AutoSlayer.GetCurrentScreen<NDeckEnchantSelectScreen>();
			CS$<>8__locals1.cards = new List<NGridCardHolder>();
			await WaitHelper.Until(delegate
			{
				CS$<>8__locals1.cards = UiHelper.FindAll<NGridCardHolder>(CS$<>8__locals1.screen);
				return CS$<>8__locals1.cards.Count > 0;
			}, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "No cards found in enchant screen");
			await Task.Delay(300, ct);
			int maxSelections = Math.Min(CS$<>8__locals1.cards.Count, 5);
			CS$<>8__locals1.selectedCards = new List<NGridCardHolder>();
			NConfirmButton nconfirmButton;
			for (int i = 0; i < maxSelections; i++)
			{
				CS$<>8__locals1.singlePreviewContainer = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%EnchantSinglePreviewContainer");
				CS$<>8__locals1.multiPreviewContainer = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%EnchantMultiPreviewContainer");
				nconfirmButton = CS$<>8__locals1.screen.GetNodeOrNull<NConfirmButton>("Confirm");
				Control singlePreviewContainer = CS$<>8__locals1.singlePreviewContainer;
				if (singlePreviewContainer != null && singlePreviewContainer.Visible)
				{
					break;
				}
				Control multiPreviewContainer = CS$<>8__locals1.multiPreviewContainer;
				if ((multiPreviewContainer != null && multiPreviewContainer.Visible) || (nconfirmButton != null && nconfirmButton.IsEnabled))
				{
					break;
				}
				IEnumerable<NGridCardHolder> cards = CS$<>8__locals1.cards;
				Func<NGridCardHolder, bool> func;
				if ((func = CS$<>8__locals1.<>9__4) == null)
				{
					DeckEnchantScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals2 = CS$<>8__locals1;
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
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Selecting card to enchant (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.cards.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" cards available)");
				AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
				ngridCardHolder.EmitSignal(NCardHolder.SignalName.Pressed, new Variant[] { ngridCardHolder });
				await Task.Delay(200, ct);
			}
			CS$<>8__locals1.singlePreviewContainer = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%EnchantSinglePreviewContainer");
			CS$<>8__locals1.multiPreviewContainer = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%EnchantMultiPreviewContainer");
			nconfirmButton = CS$<>8__locals1.screen.GetNodeOrNull<NConfirmButton>("Confirm");
			Control singlePreviewContainer2 = CS$<>8__locals1.singlePreviewContainer;
			if (singlePreviewContainer2 == null || !singlePreviewContainer2.Visible)
			{
				Control multiPreviewContainer2 = CS$<>8__locals1.multiPreviewContainer;
				if ((multiPreviewContainer2 == null || !multiPreviewContainer2.Visible) && nconfirmButton != null && nconfirmButton.IsEnabled)
				{
					AutoSlayLog.Action("Clicking main confirm button to show preview");
					await UiHelper.Click(nconfirmButton, 100);
					await Task.Delay(200, ct);
				}
			}
			CS$<>8__locals1.visiblePreviewContainer = null;
			await WaitHelper.Until(delegate
			{
				CS$<>8__locals1.singlePreviewContainer = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%EnchantSinglePreviewContainer");
				CS$<>8__locals1.multiPreviewContainer = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%EnchantMultiPreviewContainer");
				Control singlePreviewContainer3 = CS$<>8__locals1.singlePreviewContainer;
				if (singlePreviewContainer3 != null && singlePreviewContainer3.Visible)
				{
					CS$<>8__locals1.visiblePreviewContainer = CS$<>8__locals1.singlePreviewContainer;
					return true;
				}
				Control multiPreviewContainer3 = CS$<>8__locals1.multiPreviewContainer;
				if (multiPreviewContainer3 != null && multiPreviewContainer3.Visible)
				{
					CS$<>8__locals1.visiblePreviewContainer = CS$<>8__locals1.multiPreviewContainer;
					return true;
				}
				return false;
			}, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Enchant preview container did not appear");
			await Task.Delay(500, ct);
			DeckEnchantScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals3 = CS$<>8__locals1;
			Control visiblePreviewContainer = CS$<>8__locals1.visiblePreviewContainer;
			CS$<>8__locals3.confirmButton = ((visiblePreviewContainer != null) ? visiblePreviewContainer.GetNodeOrNull<NConfirmButton>("Confirm") : null);
			if (CS$<>8__locals1.confirmButton == null)
			{
				CS$<>8__locals1.confirmButton = UiHelper.FindAll<NConfirmButton>(CS$<>8__locals1.screen).FirstOrDefault<NConfirmButton>((NConfirmButton b) => b.IsEnabled);
			}
			if (CS$<>8__locals1.confirmButton != null)
			{
				await WaitHelper.Until(() => CS$<>8__locals1.confirmButton.IsEnabled, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Enchant confirm button did not become enabled");
				AutoSlayLog.Action("Confirming enchant");
				await UiHelper.Click(CS$<>8__locals1.confirmButton, 100);
				await WaitHelper.Until(() => !GodotObject.IsInstanceValid(CS$<>8__locals1.screen) || !CS$<>8__locals1.screen.IsVisibleInTree(), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Enchant screen did not close after confirmation");
			}
			else
			{
				AutoSlayLog.Error("No confirm button found on enchant screen");
			}
			AutoSlayLog.ExitScreen("NDeckEnchantSelectScreen");
		}
	}
}
