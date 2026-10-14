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
	/// Handles the deck transform selection screen.
	/// </summary>
	// Token: 0x02000D7C RID: 3452
	public class DeckTransformScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x1700263C RID: 9788
		// (get) Token: 0x06008B2A RID: 35626 RVA: 0x002E78AF File Offset: 0x002E5AAF
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NDeckTransformSelectScreen);
			}
		}

		// Token: 0x1700263D RID: 9789
		// (get) Token: 0x06008B2B RID: 35627 RVA: 0x002E78BB File Offset: 0x002E5ABB
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B2C RID: 35628 RVA: 0x002E78C8 File Offset: 0x002E5AC8
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			DeckTransformScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals1 = new DeckTransformScreenHandler.<>c__DisplayClass4_0();
			AutoSlayLog.EnterScreen("NDeckTransformSelectScreen");
			CS$<>8__locals1.screen = AutoSlayer.GetCurrentScreen<NDeckTransformSelectScreen>();
			NCardGrid grid = UiHelper.FindFirst<NCardGrid>(CS$<>8__locals1.screen);
			if (grid == null)
			{
				AutoSlayLog.Error("Card grid not found in transform screen");
			}
			else
			{
				CS$<>8__locals1.previewContainer = CS$<>8__locals1.screen.GetNodeOrNull<Control>("%PreviewContainer");
				NConfirmButton mainConfirmButton = CS$<>8__locals1.screen.GetNodeOrNull<NConfirmButton>("Confirm");
				CS$<>8__locals1.selectedCards = new HashSet<NGridCardHolder>();
				for (int i = 0; i < 10; i++)
				{
					ct.ThrowIfCancellationRequested();
					Control previewContainer = CS$<>8__locals1.previewContainer;
					if (previewContainer != null && previewContainer.Visible)
					{
						AutoSlayLog.Info("Preview container appeared after selecting cards");
						break;
					}
					if (mainConfirmButton != null && mainConfirmButton.IsEnabled)
					{
						AutoSlayLog.Action("Clicking main confirm button");
						await UiHelper.Click(mainConfirmButton, 100);
						await Task.Delay(300, ct);
						Func<bool> func;
						if ((func = CS$<>8__locals1.<>9__3) == null)
						{
							DeckTransformScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals2 = CS$<>8__locals1;
							Func<bool> func2 = delegate
							{
								Control previewContainer3 = CS$<>8__locals1.previewContainer;
								return previewContainer3 != null && previewContainer3.Visible;
							};
							CS$<>8__locals2.<>9__3 = func2;
							func = func2;
						}
						await WaitHelper.Until(func, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Preview container did not appear after confirm");
						break;
					}
					IEnumerable<NGridCardHolder> enumerable = UiHelper.FindAll<NGridCardHolder>(CS$<>8__locals1.screen);
					Func<NGridCardHolder, bool> func3;
					if ((func3 = CS$<>8__locals1.<>9__4) == null)
					{
						DeckTransformScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals3 = CS$<>8__locals1;
						Func<NGridCardHolder, bool> func4 = (NGridCardHolder c) => !CS$<>8__locals1.selectedCards.Contains(c);
						CS$<>8__locals3.<>9__4 = func4;
						func3 = func4;
					}
					List<NGridCardHolder> list = enumerable.Where<NGridCardHolder>(func3).ToList<NGridCardHolder>();
					if (list.Count == 0)
					{
						AutoSlayLog.Warn("No more cards available to select");
						break;
					}
					NGridCardHolder ngridCardHolder = random.NextItem<NGridCardHolder>(list);
					CS$<>8__locals1.selectedCards.Add(ngridCardHolder);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Selecting card to transform (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.selectedCards.Count);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
					grid.EmitSignal(NCardGrid.SignalName.HolderPressed, new Variant[] { ngridCardHolder });
					await Task.Delay(300, ct);
				}
				await WaitHelper.Until(delegate
				{
					Control previewContainer4 = CS$<>8__locals1.previewContainer;
					return previewContainer4 != null && previewContainer4.Visible;
				}, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Preview container did not appear");
				await Task.Delay(500, ct);
				DeckTransformScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals4 = CS$<>8__locals1;
				Control previewContainer2 = CS$<>8__locals1.previewContainer;
				CS$<>8__locals4.previewConfirmButton = ((previewContainer2 != null) ? previewContainer2.GetNodeOrNull<NConfirmButton>("Confirm") : null);
				if (CS$<>8__locals1.previewConfirmButton == null)
				{
					AutoSlayLog.Error("Preview confirm button not found");
				}
				else
				{
					await WaitHelper.Until(() => CS$<>8__locals1.previewConfirmButton.IsEnabled, ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Preview confirm button did not become enabled");
					AutoSlayLog.Action("Confirming transform");
					await UiHelper.Click(CS$<>8__locals1.previewConfirmButton, 100);
					await WaitHelper.Until(() => !GodotObject.IsInstanceValid(CS$<>8__locals1.screen) || !CS$<>8__locals1.screen.IsVisibleInTree(), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Transform screen did not close after confirmation");
					AutoSlayLog.ExitScreen("NDeckTransformSelectScreen");
				}
			}
		}
	}
}
