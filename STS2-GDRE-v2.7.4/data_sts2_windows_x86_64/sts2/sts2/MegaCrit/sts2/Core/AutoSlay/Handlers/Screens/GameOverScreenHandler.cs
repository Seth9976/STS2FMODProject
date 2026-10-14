using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens
{
	/// <summary>
	/// Handles the game over screen (shown after victory or death).
	/// Clicks through the summary to return to main menu.
	/// </summary>
	// Token: 0x02000D7E RID: 3454
	public class GameOverScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x17002640 RID: 9792
		// (get) Token: 0x06008B32 RID: 35634 RVA: 0x002E7987 File Offset: 0x002E5B87
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NGameOverScreen);
			}
		}

		// Token: 0x17002641 RID: 9793
		// (get) Token: 0x06008B33 RID: 35635 RVA: 0x002E7993 File Offset: 0x002E5B93
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromMinutes(2L);
			}
		}

		// Token: 0x06008B34 RID: 35636 RVA: 0x002E799C File Offset: 0x002E5B9C
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			AutoSlayLog.EnterScreen("NGameOverScreen");
			NGameOverScreen screen = AutoSlayer.GetCurrentScreen<NGameOverScreen>();
			NGameOverContinueButton continueButton = UiHelper.FindFirst<NGameOverContinueButton>(screen);
			if (continueButton == null)
			{
				AutoSlayLog.Error("Continue button not found on game over screen");
			}
			else
			{
				await WaitHelper.Until(() => continueButton.IsEnabled, ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Continue button did not become enabled");
				AutoSlayLog.Action("Clicking continue button");
				await UiHelper.Click(continueButton, 100);
				NReturnToMainMenuButton mainMenuButton = null;
				int waitCycles = 0;
				await WaitHelper.Until(delegate
				{
					if (!GodotObject.IsInstanceValid(screen) || !screen.IsVisibleInTree())
					{
						return true;
					}
					mainMenuButton = UiHelper.FindFirst<NReturnToMainMenuButton>(screen);
					int waitCycles2 = waitCycles;
					waitCycles = waitCycles2 + 1;
					if (waitCycles % 20 == 0)
					{
						bool flag = mainMenuButton != null;
						NReturnToMainMenuButton mainMenuButton4 = mainMenuButton;
						bool flag2 = mainMenuButton4 != null && mainMenuButton4.Visible;
						NReturnToMainMenuButton mainMenuButton2 = mainMenuButton;
						bool flag3 = mainMenuButton2 != null && mainMenuButton2.IsEnabled;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Waiting for main menu button: found=");
						defaultInterpolatedStringHandler.AppendFormatted<bool>(flag);
						defaultInterpolatedStringHandler.AppendLiteral(", visible=");
						defaultInterpolatedStringHandler.AppendFormatted<bool>(flag2);
						defaultInterpolatedStringHandler.AppendLiteral(", enabled=");
						defaultInterpolatedStringHandler.AppendFormatted<bool>(flag3);
						AutoSlayLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
						Watchdog currentWatchdog = AutoSlayer.CurrentWatchdog;
						if (currentWatchdog != null)
						{
							currentWatchdog.Reset("Waiting for game over summary animation");
						}
					}
					NReturnToMainMenuButton mainMenuButton3 = mainMenuButton;
					return mainMenuButton3 != null && mainMenuButton3.Visible && mainMenuButton.IsEnabled;
				}, ct, new TimeSpan?(TimeSpan.FromSeconds(90L)), "Main menu button did not become enabled");
				if (!GodotObject.IsInstanceValid(screen) || !screen.IsVisibleInTree())
				{
					AutoSlayLog.Action("Game over screen closed automatically");
					AutoSlayLog.ExitScreen("NGameOverScreen");
				}
				else
				{
					AutoSlayLog.Action("Clicking main menu button");
					await UiHelper.Click(mainMenuButton, 100);
					await WaitHelper.Until(() => !GodotObject.IsInstanceValid(screen) || !screen.IsVisibleInTree(), ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Game over screen did not close");
					AutoSlayLog.ExitScreen("NGameOverScreen");
				}
			}
		}
	}
}
