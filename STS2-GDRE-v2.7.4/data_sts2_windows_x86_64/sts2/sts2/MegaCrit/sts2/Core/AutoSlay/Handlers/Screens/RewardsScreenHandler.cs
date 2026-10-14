using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rewards;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens
{
	/// <summary>
	/// Handles the rewards screen after combat.
	/// </summary>
	// Token: 0x02000D80 RID: 3456
	public class RewardsScreenHandler : IScreenHandler, IHandler
	{
		// Token: 0x17002644 RID: 9796
		// (get) Token: 0x06008B3C RID: 35644 RVA: 0x002E7B23 File Offset: 0x002E5D23
		[Nullable(1)]
		public Type ScreenType
		{
			[NullableContext(1)]
			get
			{
				return typeof(NRewardsScreen);
			}
		}

		// Token: 0x17002645 RID: 9797
		// (get) Token: 0x06008B3D RID: 35645 RVA: 0x002E7B2F File Offset: 0x002E5D2F
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(30L);
			}
		}

		// Token: 0x06008B3E RID: 35646 RVA: 0x002E7B3C File Offset: 0x002E5D3C
		[NullableContext(1)]
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			RewardsScreenHandler.<>c__DisplayClass4_0 CS$<>8__locals1 = new RewardsScreenHandler.<>c__DisplayClass4_0();
			AutoSlayLog.EnterScreen("NRewardsScreen");
			CS$<>8__locals1.screen = AutoSlayer.GetCurrentScreen<NRewardsScreen>();
			CS$<>8__locals1.attemptedButtons = new HashSet<NRewardButton>();
			for (;;)
			{
				RewardsScreenHandler.<>c__DisplayClass4_1 CS$<>8__locals2 = new RewardsScreenHandler.<>c__DisplayClass4_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				ct.ThrowIfCancellationRequested();
				Player me = LocalContext.GetMe(RunManager.Instance.DebugOnlyGetState());
				CS$<>8__locals2.hasPotionSlots = me != null && me.HasOpenPotionSlots;
				NRewardButton nrewardButton = UiHelper.FindAll<NRewardButton>(CS$<>8__locals2.CS$<>8__locals1.screen).FirstOrDefault<NRewardButton>((NRewardButton b) => b.IsEnabled && !CS$<>8__locals2.CS$<>8__locals1.attemptedButtons.Contains(b) && (!(b.Reward is PotionReward) | CS$<>8__locals2.hasPotionSlots));
				if (nrewardButton == null)
				{
					goto IL_0230;
				}
				CS$<>8__locals2.CS$<>8__locals1.attemptedButtons.Add(nrewardButton);
				string text = "Clicking reward button: ";
				Reward reward = nrewardButton.Reward;
				AutoSlayLog.Action(text + (((reward != null) ? reward.GetType().Name : null) ?? "unknown"));
				await UiHelper.Click(nrewardButton, 100);
				await Task.Delay(500, ct);
				NOverlayStack instance = NOverlayStack.Instance;
				IOverlayScreen overlayScreen = ((instance != null) ? instance.Peek() : null);
				if (overlayScreen != null && overlayScreen != CS$<>8__locals2.CS$<>8__locals1.screen)
				{
					break;
				}
				CS$<>8__locals2 = null;
			}
			AutoSlayLog.Action("Child screen opened, returning to drain loop");
			AutoSlayLog.ExitScreen("NRewardsScreen");
			return;
			IL_0230:
			NProceedButton nproceedButton = UiHelper.FindFirst<NProceedButton>(CS$<>8__locals1.screen);
			if (nproceedButton != null)
			{
				AutoSlayLog.Action("Clicking proceed");
				await UiHelper.Click(nproceedButton, 100);
				await WaitHelper.Until(delegate
				{
					if (GodotObject.IsInstanceValid(CS$<>8__locals1.screen))
					{
						NOverlayStack instance2 = NOverlayStack.Instance;
						if (((instance2 != null) ? instance2.Peek() : null) == CS$<>8__locals1.screen)
						{
							NMapScreen instance3 = NMapScreen.Instance;
							return instance3 != null && instance3.IsOpen;
						}
					}
					return true;
				}, ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Rewards screen did not close or map did not open after clicking proceed");
			}
			AutoSlayLog.ExitScreen("NRewardsScreen");
		}
	}
}
