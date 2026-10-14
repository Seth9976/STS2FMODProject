using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens
{
	/// <summary>
	/// Handles the Crystal Sphere minigame event screen.
	/// Uses divinations by clicking cells, then clicks proceed.
	/// Note: When divinations run out, a rewards screen opens automatically.
	/// The drain loop will handle that, then this handler runs again to click proceed.
	/// </summary>
	// Token: 0x02000D79 RID: 3449
	public class CrystalSphereScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x17002636 RID: 9782
		// (get) Token: 0x06008B1E RID: 35614 RVA: 0x002E776B File Offset: 0x002E596B
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NCrystalSphereScreen);
			}
		}

		// Token: 0x17002637 RID: 9783
		// (get) Token: 0x06008B1F RID: 35615 RVA: 0x002E7777 File Offset: 0x002E5977
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(120L);
			}
		}

		// Token: 0x06008B20 RID: 35616 RVA: 0x002E7784 File Offset: 0x002E5984
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			AutoSlayLog.EnterScreen("NCrystalSphereScreen");
			NCrystalSphereScreen screen = AutoSlayer.GetCurrentScreen<NCrystalSphereScreen>();
			await Task.Delay(1000, ct);
			NProceedButton proceedButton = screen.GetNodeOrNull<NProceedButton>("%ProceedButton");
			NProceedButton proceedButton4 = proceedButton;
			if (proceedButton4 != null && proceedButton4.IsEnabled)
			{
				AutoSlayLog.Action("Clicking Crystal Sphere proceed button");
				await UiHelper.Click(proceedButton, 100);
				await WaitHelper.Until(delegate
				{
					if (GodotObject.IsInstanceValid(screen) && screen.IsVisibleInTree())
					{
						NMapScreen instance7 = NMapScreen.Instance;
						return instance7 != null && instance7.IsVisibleInTree();
					}
					return true;
				}, ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Crystal Sphere screen did not close after clicking proceed");
				if (GodotObject.IsInstanceValid(screen) && screen.IsVisibleInTree())
				{
					NMapScreen instance = NMapScreen.Instance;
					if (instance != null && instance.IsVisibleInTree())
					{
						AutoSlayLog.Info("Map opened, manually removing Crystal Sphere screen from overlay stack");
						NOverlayStack instance2 = NOverlayStack.Instance;
						if (instance2 != null)
						{
							instance2.Remove(screen);
						}
						await Task.Delay(100, ct);
					}
				}
				AutoSlayLog.ExitScreen("NCrystalSphereScreen");
			}
			else
			{
				int maxClicks = 20;
				int clicks = 0;
				int lastClickableCount = int.MaxValue;
				int noProgressCount = 0;
				while (clicks < maxClicks)
				{
					ct.ThrowIfCancellationRequested();
					if (!GodotObject.IsInstanceValid(screen) || !screen.IsVisibleInTree())
					{
						break;
					}
					NOverlayStack instance3 = NOverlayStack.Instance;
					IOverlayScreen overlayScreen = ((instance3 != null) ? instance3.Peek() : null);
					if (overlayScreen != null && overlayScreen != screen)
					{
						AutoSlayLog.Info("Child screen appeared (rewards), returning to drain loop");
						AutoSlayLog.ExitScreen("NCrystalSphereScreen");
						return;
					}
					proceedButton = screen.GetNodeOrNull<NProceedButton>("%ProceedButton");
					NProceedButton proceedButton2 = proceedButton;
					if (proceedButton2 != null && proceedButton2.IsEnabled)
					{
						break;
					}
					Control nodeOrNull = screen.GetNodeOrNull<Control>("%Cells");
					if (nodeOrNull == null)
					{
						await Task.Delay(100, ct);
					}
					else
					{
						List<NCrystalSphereCell> list = (from c in UiHelper.FindAll<NCrystalSphereCell>(nodeOrNull)
							where c.Visible && c.Entity.IsHidden
							select c).ToList<NCrystalSphereCell>();
						if (list.Count == 0)
						{
							AutoSlayLog.Info("No more clickable cells, waiting for rewards or proceed");
							break;
						}
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						if (list.Count >= lastClickableCount)
						{
							noProgressCount++;
							if (noProgressCount > 5)
							{
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
								defaultInterpolatedStringHandler.AppendLiteral("No progress after ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(noProgressCount);
								defaultInterpolatedStringHandler.AppendLiteral(" clicks, divinations likely exhausted");
								AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
								break;
							}
						}
						else
						{
							noProgressCount = 0;
						}
						lastClickableCount = list.Count;
						NCrystalSphereCell ncrystalSphereCell = random.NextItem<NCrystalSphereCell>(list);
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 4);
						defaultInterpolatedStringHandler.AppendLiteral("Clicking crystal sphere cell at (");
						defaultInterpolatedStringHandler.AppendFormatted<int>(ncrystalSphereCell.Entity.X);
						defaultInterpolatedStringHandler.AppendLiteral(", ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(ncrystalSphereCell.Entity.Y);
						defaultInterpolatedStringHandler.AppendLiteral("), click ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(clicks + 1);
						defaultInterpolatedStringHandler.AppendLiteral(", ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count);
						defaultInterpolatedStringHandler.AppendLiteral(" clickable");
						AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
						ncrystalSphereCell.EmitSignal(NClickableControl.SignalName.Released, new Variant[] { ncrystalSphereCell });
						await Task.Delay(500, ct);
						clicks++;
					}
				}
				await WaitHelper.Until(delegate
				{
					proceedButton = screen.GetNodeOrNull<NProceedButton>("%ProceedButton");
					NProceedButton proceedButton3 = proceedButton;
					if (proceedButton3 != null && proceedButton3.IsEnabled)
					{
						return true;
					}
					if (!GodotObject.IsInstanceValid(screen) || !screen.IsVisibleInTree())
					{
						return true;
					}
					NOverlayStack instance8 = NOverlayStack.Instance;
					IOverlayScreen overlayScreen3 = ((instance8 != null) ? instance8.Peek() : null);
					return overlayScreen3 != null && overlayScreen3 != screen;
				}, ct, new TimeSpan?(TimeSpan.FromSeconds(15L)), "Crystal Sphere: neither proceed button nor rewards screen appeared");
				NOverlayStack instance4 = NOverlayStack.Instance;
				IOverlayScreen overlayScreen2 = ((instance4 != null) ? instance4.Peek() : null);
				if (overlayScreen2 != null && overlayScreen2 != screen)
				{
					AutoSlayLog.Info("Rewards screen appeared, returning to drain loop");
					AutoSlayLog.ExitScreen("NCrystalSphereScreen");
				}
				else if (!GodotObject.IsInstanceValid(screen) || !screen.IsVisibleInTree())
				{
					AutoSlayLog.ExitScreen("NCrystalSphereScreen");
				}
				else
				{
					proceedButton = screen.GetNodeOrNull<NProceedButton>("%ProceedButton");
					if (proceedButton != null && proceedButton.IsEnabled)
					{
						AutoSlayLog.Action("Clicking Crystal Sphere proceed button");
						await UiHelper.Click(proceedButton, 100);
						await WaitHelper.Until(delegate
						{
							if (GodotObject.IsInstanceValid(screen) && screen.IsVisibleInTree())
							{
								NMapScreen instance9 = NMapScreen.Instance;
								return instance9 != null && instance9.IsVisibleInTree();
							}
							return true;
						}, ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Crystal Sphere screen did not close after clicking proceed");
						if (GodotObject.IsInstanceValid(screen) && screen.IsVisibleInTree())
						{
							NMapScreen instance5 = NMapScreen.Instance;
							if (instance5 != null && instance5.IsVisibleInTree())
							{
								AutoSlayLog.Info("Map opened, manually removing Crystal Sphere screen from overlay stack");
								NOverlayStack instance6 = NOverlayStack.Instance;
								if (instance6 != null)
								{
									instance6.Remove(screen);
								}
								await Task.Delay(100, ct);
							}
						}
					}
					AutoSlayLog.ExitScreen("NCrystalSphereScreen");
				}
			}
		}
	}
}
