using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rewards;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens
{
	// Token: 0x0200024F RID: 591
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/NRewardsScreen.cs")]
	public class NRewardsScreen : Control, IOverlayScreen, IScreenContext
	{
		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001E71 RID: 7793 RVA: 0x000C7D90 File Offset: 0x000C5F90
		private bool CanScroll
		{
			get
			{
				return this._rewardsContainer.Size.Y >= 400f;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x000C7DAC File Offset: 0x000C5FAC
		private float ScrollLimitBottom
		{
			get
			{
				return 35f - this._rewardsContainer.Size.Y + 400f;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001E73 RID: 7795 RVA: 0x000C7DCA File Offset: 0x000C5FCA
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/rewards_screen");
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001E74 RID: 7796 RVA: 0x000C7DD6 File Offset: 0x000C5FD6
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NRewardsScreen.ScenePath);
			}
		}

		/// <summary>
		/// If the rewards screen is terminal, this is true once the player has taken all rewards or skipped them.
		/// False if the rewards screen is not terminal or the player has not taken all rewards yet.
		/// </summary>
		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001E75 RID: 7797 RVA: 0x000C7DE2 File Offset: 0x000C5FE2
		// (set) Token: 0x06001E76 RID: 7798 RVA: 0x000C7DEA File Offset: 0x000C5FEA
		public bool IsComplete { get; private set; }

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001E77 RID: 7799 RVA: 0x000C7DF3 File Offset: 0x000C5FF3
		public NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.Rewards;
			}
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000C7DF8 File Offset: 0x000C5FF8
		public static NRewardsScreen ShowScreen(RewardsSet set, bool isTerminal, IRunState runState)
		{
			NRewardsScreen nrewardsScreen = PreloadManager.Cache.GetScene(NRewardsScreen.ScenePath).Instantiate<NRewardsScreen>(0L);
			nrewardsScreen._rewardsSet = set;
			nrewardsScreen._isTerminal = isTerminal;
			nrewardsScreen._runState = runState;
			NOverlayStack.Instance.Push(nrewardsScreen);
			return nrewardsScreen;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000C7E40 File Offset: 0x000C6040
		public override void _Ready()
		{
			this._proceedButton = base.GetNode<NProceedButton>("ProceedButton");
			this._rewardContainerMask = base.GetNode<Control>("%RewardContainerMask");
			this._rewardsContainer = base.GetNode<Control>("%RewardsContainer");
			this._scrollbar = base.GetNode<NScrollbar>("%Scrollbar");
			this._headerLabel = base.GetNode<MegaLabel>("%HeaderLabel");
			this._waitingForOtherPlayersOverlay = base.GetNode<Control>("%WaitingForOtherPlayers");
			this._waitingForOtherPlayersOverlay.GetNode<MegaLabel>("Label").SetTextAutoSize(NRewardsScreen._waitingLoc.GetRawText());
			this._rewardsWindow = base.GetNode<Control>("Rewards");
			this._rewardsWindow.Modulate = StsColors.transparentBlack;
			this._proceedButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnProceedButtonPressed)), 0U);
			this._proceedButton.SetPulseState(false);
			this.TryEnableProceedButton();
			this._proceedButton.UpdateText(NProceedButton.SkipLoc);
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("victory.mp3", 1f, PitchVariance.None);
			}
			this._scrollbar.Connect(NScrollbar.SignalName.MousePressed, Callable.From<InputEvent>(delegate(InputEvent _)
			{
				this._scrollbarPressed = true;
			}), 0U);
			this._scrollbar.Connect(NScrollbar.SignalName.MouseReleased, Callable.From<InputEvent>(delegate(InputEvent _)
			{
				this._scrollbarPressed = false;
			}), 0U);
			this._targetDragPos = new Vector2(this._rewardsContainer.Position.X, 35f);
			bool flag;
			if (this._runState.CurrentRoom is CombatRoom)
			{
				MapPointHistoryEntry currentMapPointHistoryEntry = this._runState.CurrentMapPointHistoryEntry;
				flag = currentMapPointHistoryEntry != null && currentMapPointHistoryEntry.GetEntry(this._rewardsSet.Player.NetId).WasMugged;
			}
			else
			{
				flag = false;
			}
			string text = (flag ? "COMBAT_REWARD_HEADER_MUGGED" : "COMBAT_REWARD_HEADER_LOOT");
			this._headerLabel.SetTextAutoSize(new LocString("gameplay_ui", text).GetFormattedText());
			base.GetViewport().Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
			this._rewardsContainer.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
			{
				this.DefaultFocusedControl.TryGrabFocus();
			}), 0U);
			foreach (Control control in this._rewardButtons)
			{
				this.RemoveButton(control);
			}
			List<Reward> list = this._rewardsSet.Rewards.ToList<Reward>();
			this._rewardButtons.Clear();
			foreach (Reward reward in list)
			{
				LinkedRewardSet linkedRewardSet = reward as LinkedRewardSet;
				Control option;
				if (linkedRewardSet != null)
				{
					option = NLinkedRewardSet.Create(linkedRewardSet, this);
					option.Connect(NLinkedRewardSet.SignalName.RewardClaimed, Callable.From<NLinkedRewardSet>(new Action<NLinkedRewardSet>(this.RewardCollectedFrom)), 0U);
				}
				else
				{
					option = NRewardButton.Create(reward, this);
					option.Connect(NRewardButton.SignalName.RewardClaimed, Callable.From<NRewardButton>(new Action<NRewardButton>(this.RewardCollectedFrom)), 0U);
					option.Connect(NRewardButton.SignalName.RewardSkipped, Callable.From<NRewardButton>(new Action<NRewardButton>(this.RewardSkippedFrom)), 0U);
					option.Connect(Control.SignalName.FocusEntered, Callable.From<Control>(() => this._lastRewardFocused = option), 0U);
				}
				reward.MarkContentAsSeen();
				this._rewardButtons.Add(option);
				this._rewardsContainer.AddChildSafely(option);
			}
			if (this._rewardsSet.DisallowSkipping)
			{
				this._skipDisallowed = true;
				if (this._proceedButton.IsSkip)
				{
					this._proceedButton.Disable();
				}
			}
			if (this._rewardsContainer.HasFocus())
			{
				this.DefaultFocusedControl.TryGrabFocus();
			}
			TaskHelper.RunSafely(this.RelicFtueCheck());
			Callable.From(new Action(this.UpdateScreenState)).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000C82B8 File Offset: 0x000C64B8
		private async Task RelicFtueCheck()
		{
			if (!SaveManager.Instance.SeenFtue("obtain_relic_ftue"))
			{
				await this.AwaitProcessFrame(default(CancellationToken));
				await this.AwaitProcessFrame(default(CancellationToken));
				foreach (NRewardButton nrewardButton in this._rewardButtons.OfType<NRewardButton>())
				{
					if (nrewardButton.Reward is RelicReward)
					{
						NModalContainer.Instance.Add(NRelicRewardFtue.Create(nrewardButton), true);
						SaveManager.Instance.MarkFtueAsComplete("obtain_relic_ftue");
						break;
					}
				}
			}
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000C82FC File Offset: 0x000C64FC
		public void RewardCollectedFrom(Control button)
		{
			int num = this._rewardButtons.IndexOf(button);
			this.RemoveButton(button);
			this._lastRewardFocused = ((this._rewardButtons.Count > 0) ? this._rewardButtons[Mathf.Min(num, this._rewardButtons.Count - 1)] : null);
			this.UpdateScreenState();
			if (this._rewardButtons.Count > 0 || this._isTerminal)
			{
				this.TryEnableProceedButton();
				if (this._proceedButton.IsEnabled && !this._rewardButtons.Except<Control>(this._skippedRewardButtons).Any<Control>())
				{
					this._proceedButton.SetPulseState(true);
				}
			}
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000C83A5 File Offset: 0x000C65A5
		public void RewardSkippedFrom(Control button)
		{
			this._skippedRewardButtons.Add(button);
			if (!this._rewardButtons.Except<Control>(this._skippedRewardButtons).Any<Control>())
			{
				this._proceedButton.SetPulseState(true);
			}
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000C83D8 File Offset: 0x000C65D8
		private void UpdateScreenState()
		{
			if (this._rewardButtons.Count == 0)
			{
				if (!RunManager.Instance.RewardsSetSynchronizer.IsRewardsSetCompleted(this._rewardsSet))
				{
					Log.Error("All rewards have been taken, but the rewards set is not complete on the backend!", 2);
				}
				if (this._isTerminal)
				{
					Tween fadeTween = this._fadeTween;
					if (fadeTween != null)
					{
						fadeTween.Kill();
					}
					this._fadeTween = base.CreateTween().SetParallel(true);
					this._fadeTween.TweenProperty(base.GetNode<Control>("Rewards"), "modulate:a", 0f, 0.25);
					NOverlayStack.Instance.HideBackstop();
					this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
					this.TryEnableProceedButton();
					this._proceedButton.SetPulseState(true);
					this._rewardsContainer.FocusMode = 0L;
					this.IsComplete = true;
					base.EmitSignal(NRewardsScreen.SignalName.Completed, Array.Empty<Variant>());
				}
				else
				{
					NOverlayStack.Instance.Remove(this);
				}
			}
			this._rewardsContainer.ResetSize();
			this._scrollbar.Visible = this.CanScroll;
			this._scrollbar.MouseFilter = (this.CanScroll ? 0L : 2L);
			if (!this.CanScroll)
			{
				this._targetDragPos.Y = 35f;
			}
			for (int i = 0; i < this._rewardButtons.Count; i++)
			{
				this._rewardButtons[i].FocusNeighborLeft = this._rewardButtons[i].GetPath();
				this._rewardButtons[i].FocusNeighborRight = this._rewardButtons[i].GetPath();
				this._rewardButtons[i].FocusNeighborTop = ((i > 0) ? this._rewardButtons[i - 1].GetPath() : this._rewardButtons[i].GetPath());
				this._rewardButtons[i].FocusNeighborBottom = ((i < this._rewardButtons.Count - 1) ? this._rewardButtons[i + 1].GetPath() : this._rewardButtons[i].GetPath());
			}
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000C860C File Offset: 0x000C680C
		private void RemoveButton(Control button)
		{
			button.GetParent().RemoveChildSafely(button);
			button.QueueFreeSafely();
			int num = this._rewardButtons.IndexOf(button);
			this._rewardButtons.Remove(button);
			if (this._rewardButtons.Count > 0)
			{
				num = Mathf.Min(num, this._rewardButtons.Count - 1);
				this._rewardButtons[num].TryGrabFocus();
			}
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000C8678 File Offset: 0x000C6878
		private void OnProceedButtonPressed(NButton _)
		{
			if (RunManager.Instance.debugAfterCombatRewardsOverride != null && this._isTerminal)
			{
				Action debugAfterCombatRewardsOverride = RunManager.Instance.debugAfterCombatRewardsOverride;
				if (debugAfterCombatRewardsOverride == null)
				{
					return;
				}
				debugAfterCombatRewardsOverride();
				return;
			}
			else if (this._isTerminal && (this._runState.CurrentRoom.RoomType == RoomType.Boss || this._runState.CurrentRoom.IsVictoryRoom))
			{
				bool flag = this._runState.Map.SecondBossMapPoint != null && this._runState.CurrentMapCoord == this._runState.Map.BossMapPoint.coord;
				if (flag)
				{
					TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
					return;
				}
				this._proceedButton.Disable();
				this._disableProceedForever = true;
				if (RunManager.Instance.ActChangeSynchronizer.IsWaitingForOtherPlayers())
				{
					this._waitingForOtherPlayersOverlay.Visible = true;
				}
				RunManager.Instance.ActChangeSynchronizer.SetLocalPlayerReady();
				return;
			}
			else
			{
				if (!this._isTerminal)
				{
					RunManager.Instance.RewardsSetSynchronizer.SkipLocalRewardsSet();
					NOverlayStack.Instance.Remove(this);
					return;
				}
				if (!this._proceedButton.IsSkip)
				{
					if (this._runState.ActFloor > 4)
					{
						SaveManager.Instance.MarkFtueAsComplete("combat_reward_ftue");
					}
					TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
					return;
				}
				if (TestMode.IsOn || SaveManager.Instance.SeenFtue("combat_reward_ftue"))
				{
					TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
					return;
				}
				TaskHelper.RunSafely(this.RewardFtueCheck());
				return;
			}
		}

		/// <summary>
		/// Called when RewardsSetSynchronizer skips all rewards just before we exit the room.
		/// If we're still showing rewards at that time, we must hide all rewards to prevent the player from clicking them
		/// just before the transition blocks input.
		/// </summary>
		// Token: 0x06001E80 RID: 7808 RVA: 0x000C8818 File Offset: 0x000C6A18
		private void BeforeRoomExit()
		{
			this._rewardButtons.Clear();
			this.UpdateScreenState();
			if (this.IsValid())
			{
				this._proceedButton.Disable();
			}
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x000C883E File Offset: 0x000C6A3E
		public void AfterOverlayOpened()
		{
			RunManager.Instance.RewardsSetSynchronizer.RewardsSkippedDuringRoomExit += this.BeforeRoomExit;
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x000C885B File Offset: 0x000C6A5B
		public void AfterOverlayClosed()
		{
			this._proceedButton.Disable();
			RunManager.Instance.RewardsSetSynchronizer.RewardsSkippedDuringRoomExit -= this.BeforeRoomExit;
			this.QueueFreeSafely();
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000C888C File Offset: 0x000C6A8C
		private void TryEnableProceedButton()
		{
			if (this._disableProceedForever)
			{
				return;
			}
			if (this._skipDisallowed && this._proceedButton.IsSkip)
			{
				return;
			}
			if (Hook.ShouldProceedToNextMapPoint(this._runState) && !this._proceedButton.IsEnabled)
			{
				if (this._isTerminal && this._rewardButtons.Count == 0)
				{
					NOverlayStack.Instance.HideBackstop();
				}
				this._proceedButton.Enable();
			}
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x000C88FC File Offset: 0x000C6AFC
		public void AfterOverlayShown()
		{
			this.TryEnableProceedButton();
			this.UpdateScreenState();
			if (!this.IsComplete)
			{
				Tween fadeTween = this._fadeTween;
				if (fadeTween != null)
				{
					fadeTween.FastForwardToCompletion();
				}
				this._fadeTween = base.CreateTween().SetParallel(true);
				this._fadeTween.TweenProperty(this._rewardsWindow, "modulate", Colors.White, 0.5);
				this._fadeTween.TweenProperty(this._rewardsWindow, "position:y", this._rewardsWindow.Position.Y, 0.5).SetEase(1L).SetTrans(10L)
					.From(this._rewardsWindow.Position.Y + 100f);
			}
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x000C89DC File Offset: 0x000C6BDC
		public void AfterOverlayHidden()
		{
			this._proceedButton.Disable();
			if (!this.IsComplete)
			{
				Tween fadeTween = this._fadeTween;
				if (fadeTween != null)
				{
					fadeTween.FastForwardToCompletion();
				}
				this._fadeTween = base.CreateTween();
				this._fadeTween.TweenProperty(this._rewardsWindow, "modulate:a", 0, 0.25);
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x000C8A44 File Offset: 0x000C6C44
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x000C8A47 File Offset: 0x000C6C47
		public override void _GuiInput(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (!this.CanScroll)
			{
				return;
			}
			this.ProcessScrollEvent(inputEvent);
		}

		/// <summary>
		/// Detects mouse wheel up/down and updates our scroll target accordingly
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06001E88 RID: 7816 RVA: 0x000C8A62 File Offset: 0x000C6C62
		private void ProcessScrollEvent(InputEvent inputEvent)
		{
			this._targetDragPos += new Vector2(0f, ScrollHelper.GetDragForScrollEvent(inputEvent));
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x000C8A88 File Offset: 0x000C6C88
		private void ProcessGuiFocus(Control focusedControl)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (!this.CanScroll)
			{
				return;
			}
			if (!NControllerManager.Instance.IsUsingController)
			{
				return;
			}
			if (this._rewardButtons.Contains(focusedControl))
			{
				float num = -focusedControl.Position.Y + this._rewardContainerMask.Size.Y * 0.5f;
				num = Mathf.Clamp(num, this.ScrollLimitBottom, 35f);
				this._targetDragPos = new Vector2(this._targetDragPos.X, num);
			}
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x000C8B0F File Offset: 0x000C6D0F
		public override void _Process(double delta)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			this.UpdateScrollPosition(delta);
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x000C8B24 File Offset: 0x000C6D24
		private void UpdateScrollPosition(double delta)
		{
			if (!this._rewardsContainer.Position.IsEqualApprox(this._targetDragPos))
			{
				float num = (float)Mathf.Sign(this._rewardsContainer.Position.Y - this._targetDragPos.Y);
				this._rewardsContainer.Position = this._rewardsContainer.Position.Lerp(this._targetDragPos, Mathf.Clamp((float)delta * 15f, 0f, 1f));
				float num2 = (float)Mathf.Sign(this._rewardsContainer.Position.Y - this._targetDragPos.Y);
				if (Math.Abs(this._rewardsContainer.Position.Y - this._targetDragPos.Y) < 0.5f || !Mathf.IsEqualApprox(num, num2))
				{
					this._rewardsContainer.Position = this._targetDragPos;
				}
				if (!this._scrollbarPressed && this.CanScroll)
				{
					this._scrollbar.SetValueWithoutAnimation((double)(Mathf.Clamp(this._rewardsContainer.Position.Y / this.ScrollLimitBottom, 0f, 1f) * 100f));
				}
			}
			if (this._scrollbarPressed)
			{
				this._targetDragPos.Y = Mathf.Lerp(35f, this.ScrollLimitBottom, (float)this._scrollbar.Value * 0.01f);
			}
			if (this._targetDragPos.Y < Mathf.Min(this.ScrollLimitBottom, 0f))
			{
				this._targetDragPos.Y = Mathf.Lerp(this._targetDragPos.Y, this.ScrollLimitBottom, (float)delta * 12f);
				return;
			}
			if (this._targetDragPos.Y > Mathf.Max(this.ScrollLimitBottom, 0f))
			{
				this._targetDragPos.Y = Mathf.Lerp(this._targetDragPos.Y, 35f, (float)delta * 12f);
			}
		}

		/// <summary>
		/// If the player attempts to skip rewards AND they have encountered less than 3 rewards, we show the Rewards FTUE to
		/// let them know that they shouldn't skip rewards. You have to click em to get em!
		/// </summary>
		// Token: 0x06001E8C RID: 7820 RVA: 0x000C8D1C File Offset: 0x000C6F1C
		private async Task RewardFtueCheck()
		{
			this._proceedButton.Hide();
			NCombatRewardFtue ncombatRewardFtue = NCombatRewardFtue.Create(this._rewardsContainer);
			NModalContainer.Instance.Add(ncombatRewardFtue, true);
			SaveManager.Instance.MarkFtueAsComplete("combat_reward_ftue");
			await ncombatRewardFtue.WaitForPlayerToConfirm();
			this._proceedButton.Show();
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x000C8D5F File Offset: 0x000C6F5F
		public Control DefaultFocusedControl
		{
			get
			{
				if (this._rewardButtons.Count == 0)
				{
					return this._rewardsContainer;
				}
				return this._lastRewardFocused ?? this._rewardButtons[0];
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x000C8D8B File Offset: 0x000C6F8B
		public Control FocusedControlFromTopBar
		{
			get
			{
				if (this._rewardButtons.Count <= 0)
				{
					return this._rewardsContainer;
				}
				return this._rewardButtons[0];
			}
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x000C8DAE File Offset: 0x000C6FAE
		public void HideWaitingForPlayersScreen()
		{
			this._waitingForOtherPlayersOverlay.Visible = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E90 RID: 7824 RVA: 0x000C8DBC File Offset: 0x000C6FBC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(18)
			{
				new MethodInfo(NRewardsScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardsScreen.MethodName.RewardCollectedFrom, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRewardsScreen.MethodName.RewardSkippedFrom, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRewardsScreen.MethodName.UpdateScreenState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardsScreen.MethodName.RemoveButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRewardsScreen.MethodName.OnProceedButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRewardsScreen.MethodName.BeforeRoomExit, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardsScreen.MethodName.AfterOverlayOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardsScreen.MethodName.AfterOverlayClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardsScreen.MethodName.TryEnableProceedButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardsScreen.MethodName.AfterOverlayShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardsScreen.MethodName.AfterOverlayHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRewardsScreen.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NRewardsScreen.MethodName.ProcessScrollEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NRewardsScreen.MethodName.ProcessGuiFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "focusedControl", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRewardsScreen.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRewardsScreen.MethodName.UpdateScrollPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRewardsScreen.MethodName.HideWaitingForPlayersScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E91 RID: 7825 RVA: 0x000C92BC File Offset: 0x000C74BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRewardsScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.RewardCollectedFrom && args.Count == 1)
			{
				this.RewardCollectedFrom(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.RewardSkippedFrom && args.Count == 1)
			{
				this.RewardSkippedFrom(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.UpdateScreenState && args.Count == 0)
			{
				this.UpdateScreenState();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.RemoveButton && args.Count == 1)
			{
				this.RemoveButton(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.OnProceedButtonPressed && args.Count == 1)
			{
				this.OnProceedButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.BeforeRoomExit && args.Count == 0)
			{
				this.BeforeRoomExit();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.AfterOverlayOpened && args.Count == 0)
			{
				this.AfterOverlayOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.AfterOverlayClosed && args.Count == 0)
			{
				this.AfterOverlayClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.TryEnableProceedButton && args.Count == 0)
			{
				this.TryEnableProceedButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.AfterOverlayShown && args.Count == 0)
			{
				this.AfterOverlayShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.AfterOverlayHidden && args.Count == 0)
			{
				this.AfterOverlayHidden();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.ProcessScrollEvent && args.Count == 1)
			{
				this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.ProcessGuiFocus && args.Count == 1)
			{
				this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.UpdateScrollPosition && args.Count == 1)
			{
				this.UpdateScrollPosition(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRewardsScreen.MethodName.HideWaitingForPlayersScreen && args.Count == 0)
			{
				this.HideWaitingForPlayersScreen();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001E92 RID: 7826 RVA: 0x000C95EC File Offset: 0x000C77EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRewardsScreen.MethodName._Ready || (ref method) == NRewardsScreen.MethodName.RewardCollectedFrom || (ref method) == NRewardsScreen.MethodName.RewardSkippedFrom || (ref method) == NRewardsScreen.MethodName.UpdateScreenState || (ref method) == NRewardsScreen.MethodName.RemoveButton || (ref method) == NRewardsScreen.MethodName.OnProceedButtonPressed || (ref method) == NRewardsScreen.MethodName.BeforeRoomExit || (ref method) == NRewardsScreen.MethodName.AfterOverlayOpened || (ref method) == NRewardsScreen.MethodName.AfterOverlayClosed || (ref method) == NRewardsScreen.MethodName.TryEnableProceedButton || (ref method) == NRewardsScreen.MethodName.AfterOverlayShown || (ref method) == NRewardsScreen.MethodName.AfterOverlayHidden || (ref method) == NRewardsScreen.MethodName._GuiInput || (ref method) == NRewardsScreen.MethodName.ProcessScrollEvent || (ref method) == NRewardsScreen.MethodName.ProcessGuiFocus || (ref method) == NRewardsScreen.MethodName._Process || (ref method) == NRewardsScreen.MethodName.UpdateScrollPosition || (ref method) == NRewardsScreen.MethodName.HideWaitingForPlayersScreen || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001E93 RID: 7827 RVA: 0x000C9710 File Offset: 0x000C7910
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRewardsScreen.PropertyName.IsComplete)
			{
				this.IsComplete = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._proceedButton)
			{
				this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._rewardsContainer)
			{
				this._rewardsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._scrollbar)
			{
				this._scrollbar = VariantUtils.ConvertTo<NScrollbar>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._headerLabel)
			{
				this._headerLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._rewardContainerMask)
			{
				this._rewardContainerMask = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._waitingForOtherPlayersOverlay)
			{
				this._waitingForOtherPlayersOverlay = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._rewardsWindow)
			{
				this._rewardsWindow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._targetDragPos)
			{
				this._targetDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._scrollbarPressed)
			{
				this._scrollbarPressed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._fadeTween)
			{
				this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._lastRewardFocused)
			{
				this._lastRewardFocused = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._disableProceedForever)
			{
				this._disableProceedForever = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._isTerminal)
			{
				this._isTerminal = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._skipDisallowed)
			{
				this._skipDisallowed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001E94 RID: 7828 RVA: 0x000C98BC File Offset: 0x000C7ABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRewardsScreen.PropertyName.CanScroll)
			{
				bool flag = this.CanScroll;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName.ScrollLimitBottom)
			{
				float scrollLimitBottom = this.ScrollLimitBottom;
				value = VariantUtils.CreateFrom<float>(ref scrollLimitBottom);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName.IsComplete)
			{
				bool flag = this.IsComplete;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName.UseSharedBackstop)
			{
				bool flag = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName.FocusedControlFromTopBar)
			{
				Control control = this.FocusedControlFromTopBar;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._proceedButton)
			{
				value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._rewardsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rewardsContainer);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._scrollbar)
			{
				value = VariantUtils.CreateFrom<NScrollbar>(ref this._scrollbar);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._headerLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._headerLabel);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._rewardContainerMask)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rewardContainerMask);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._waitingForOtherPlayersOverlay)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._waitingForOtherPlayersOverlay);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._rewardsWindow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rewardsWindow);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._targetDragPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetDragPos);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._scrollbarPressed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._scrollbarPressed);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._fadeTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._lastRewardFocused)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._lastRewardFocused);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._disableProceedForever)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._disableProceedForever);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._isTerminal)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isTerminal);
				return true;
			}
			if ((ref name) == NRewardsScreen.PropertyName._skipDisallowed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._skipDisallowed);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E95 RID: 7829 RVA: 0x000C9B88 File Offset: 0x000C7D88
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRewardsScreen.PropertyName._proceedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardsScreen.PropertyName._rewardsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardsScreen.PropertyName._scrollbar, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardsScreen.PropertyName._headerLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardsScreen.PropertyName._rewardContainerMask, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardsScreen.PropertyName._waitingForOtherPlayersOverlay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardsScreen.PropertyName._rewardsWindow, 0L, "", 4096L, false),
				new PropertyInfo(5L, NRewardsScreen.PropertyName._targetDragPos, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRewardsScreen.PropertyName.CanScroll, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRewardsScreen.PropertyName._scrollbarPressed, 0L, "", 4096L, false),
				new PropertyInfo(3L, NRewardsScreen.PropertyName.ScrollLimitBottom, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardsScreen.PropertyName._fadeTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardsScreen.PropertyName._lastRewardFocused, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRewardsScreen.PropertyName._disableProceedForever, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRewardsScreen.PropertyName._isTerminal, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRewardsScreen.PropertyName._skipDisallowed, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRewardsScreen.PropertyName.IsComplete, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRewardsScreen.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRewardsScreen.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardsScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRewardsScreen.PropertyName.FocusedControlFromTopBar, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E96 RID: 7830 RVA: 0x000C9E48 File Offset: 0x000C8048
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isComplete = NRewardsScreen.PropertyName.IsComplete;
			bool isComplete2 = this.IsComplete;
			info.AddProperty(isComplete, Variant.From<bool>(ref isComplete2));
			info.AddProperty(NRewardsScreen.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
			info.AddProperty(NRewardsScreen.PropertyName._rewardsContainer, Variant.From<Control>(ref this._rewardsContainer));
			info.AddProperty(NRewardsScreen.PropertyName._scrollbar, Variant.From<NScrollbar>(ref this._scrollbar));
			info.AddProperty(NRewardsScreen.PropertyName._headerLabel, Variant.From<MegaLabel>(ref this._headerLabel));
			info.AddProperty(NRewardsScreen.PropertyName._rewardContainerMask, Variant.From<Control>(ref this._rewardContainerMask));
			info.AddProperty(NRewardsScreen.PropertyName._waitingForOtherPlayersOverlay, Variant.From<Control>(ref this._waitingForOtherPlayersOverlay));
			info.AddProperty(NRewardsScreen.PropertyName._rewardsWindow, Variant.From<Control>(ref this._rewardsWindow));
			info.AddProperty(NRewardsScreen.PropertyName._targetDragPos, Variant.From<Vector2>(ref this._targetDragPos));
			info.AddProperty(NRewardsScreen.PropertyName._scrollbarPressed, Variant.From<bool>(ref this._scrollbarPressed));
			info.AddProperty(NRewardsScreen.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
			info.AddProperty(NRewardsScreen.PropertyName._lastRewardFocused, Variant.From<Control>(ref this._lastRewardFocused));
			info.AddProperty(NRewardsScreen.PropertyName._disableProceedForever, Variant.From<bool>(ref this._disableProceedForever));
			info.AddProperty(NRewardsScreen.PropertyName._isTerminal, Variant.From<bool>(ref this._isTerminal));
			info.AddProperty(NRewardsScreen.PropertyName._skipDisallowed, Variant.From<bool>(ref this._skipDisallowed));
			info.AddSignalEventDelegate(NRewardsScreen.SignalName.Completed, this.backing_Completed);
		}

		/// <inheritdoc />
		// Token: 0x06001E97 RID: 7831 RVA: 0x000C9FBC File Offset: 0x000C81BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRewardsScreen.PropertyName.IsComplete, ref variant))
			{
				this.IsComplete = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._proceedButton, ref variant2))
			{
				this._proceedButton = variant2.As<NProceedButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._rewardsContainer, ref variant3))
			{
				this._rewardsContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._scrollbar, ref variant4))
			{
				this._scrollbar = variant4.As<NScrollbar>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._headerLabel, ref variant5))
			{
				this._headerLabel = variant5.As<MegaLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._rewardContainerMask, ref variant6))
			{
				this._rewardContainerMask = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._waitingForOtherPlayersOverlay, ref variant7))
			{
				this._waitingForOtherPlayersOverlay = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._rewardsWindow, ref variant8))
			{
				this._rewardsWindow = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._targetDragPos, ref variant9))
			{
				this._targetDragPos = variant9.As<Vector2>();
			}
			Variant variant10;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._scrollbarPressed, ref variant10))
			{
				this._scrollbarPressed = variant10.As<bool>();
			}
			Variant variant11;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._fadeTween, ref variant11))
			{
				this._fadeTween = variant11.As<Tween>();
			}
			Variant variant12;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._lastRewardFocused, ref variant12))
			{
				this._lastRewardFocused = variant12.As<Control>();
			}
			Variant variant13;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._disableProceedForever, ref variant13))
			{
				this._disableProceedForever = variant13.As<bool>();
			}
			Variant variant14;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._isTerminal, ref variant14))
			{
				this._isTerminal = variant14.As<bool>();
			}
			Variant variant15;
			if (info.TryGetProperty(NRewardsScreen.PropertyName._skipDisallowed, ref variant15))
			{
				this._skipDisallowed = variant15.As<bool>();
			}
			NRewardsScreen.CompletedEventHandler completedEventHandler;
			if (info.TryGetSignalEventDelegate<NRewardsScreen.CompletedEventHandler>(NRewardsScreen.SignalName.Completed, ref completedEventHandler))
			{
				this.backing_Completed = completedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E98 RID: 7832 RVA: 0x000CA18C File Offset: 0x000C838C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NRewardsScreen.SignalName.Completed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.NRewardsScreen.CompletedEventHandler" />
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06001E99 RID: 7833 RVA: 0x000CA1D0 File Offset: 0x000C83D0
		// (remove) Token: 0x06001E9A RID: 7834 RVA: 0x000CA1E9 File Offset: 0x000C83E9
		[Nullable(0)]
		public event NRewardsScreen.CompletedEventHandler Completed
		{
			[NullableContext(0)]
			add
			{
				this.backing_Completed = (NRewardsScreen.CompletedEventHandler)Delegate.Combine(this.backing_Completed, value);
			}
			[NullableContext(0)]
			remove
			{
				this.backing_Completed = (NRewardsScreen.CompletedEventHandler)Delegate.Remove(this.backing_Completed, value);
			}
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x000CA202 File Offset: 0x000C8402
		protected void EmitSignalCompleted()
		{
			base.EmitSignal(NRewardsScreen.SignalName.Completed, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06001E9C RID: 7836 RVA: 0x000CA215 File Offset: 0x000C8415
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NRewardsScreen.SignalName.Completed) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NRewardsScreen.CompletedEventHandler completedEventHandler = this.backing_Completed;
			if (completedEventHandler == null)
			{
				return;
			}
			completedEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x06001E9D RID: 7837 RVA: 0x000CA246 File Offset: 0x000C8446
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NRewardsScreen.SignalName.Completed || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000BBA RID: 3002
		private const float _scrollLimitTop = 35f;

		// Token: 0x04000BBB RID: 3003
		private const int _scrollbarThreshold = 400;

		// Token: 0x04000BBC RID: 3004
		private IRunState _runState;

		// Token: 0x04000BBD RID: 3005
		private NProceedButton _proceedButton;

		// Token: 0x04000BBE RID: 3006
		private Control _rewardsContainer;

		// Token: 0x04000BBF RID: 3007
		private NScrollbar _scrollbar;

		// Token: 0x04000BC0 RID: 3008
		private MegaLabel _headerLabel;

		// Token: 0x04000BC1 RID: 3009
		private Control _rewardContainerMask;

		// Token: 0x04000BC2 RID: 3010
		private Control _waitingForOtherPlayersOverlay;

		// Token: 0x04000BC3 RID: 3011
		private Control _rewardsWindow;

		// Token: 0x04000BC4 RID: 3012
		private Vector2 _targetDragPos;

		// Token: 0x04000BC5 RID: 3013
		private bool _scrollbarPressed;

		// Token: 0x04000BC6 RID: 3014
		[Nullable(2)]
		private Tween _fadeTween;

		// Token: 0x04000BC7 RID: 3015
		private readonly List<Control> _rewardButtons = new List<Control>();

		// Token: 0x04000BC8 RID: 3016
		private readonly List<Control> _skippedRewardButtons = new List<Control>();

		// Token: 0x04000BC9 RID: 3017
		[Nullable(2)]
		private Control _lastRewardFocused;

		// Token: 0x04000BCA RID: 3018
		private RewardsSet _rewardsSet;

		/// <summary>
		/// The proceed button is often disabled temporarily. Set this flag if it should never be re-enabled.
		/// This is used when proceed is hit after a boss fight - the player shouldn't be able to re-press it.
		/// </summary>
		// Token: 0x04000BCB RID: 3019
		private bool _disableProceedForever;

		/// <summary>
		/// Whether this screen is the last thing you'll see in the current room.
		///
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.RunManager.ProceedFromTerminalRewardsScreen" /> for more info.
		/// </summary>
		// Token: 0x04000BCC RID: 3020
		private bool _isTerminal;

		/// <summary>
		/// If this rewards screen requires the player to take all rewards, then the skip button cannot be shown.
		/// </summary>
		// Token: 0x04000BCD RID: 3021
		private bool _skipDisallowed;

		// Token: 0x04000BCE RID: 3022
		private static readonly LocString _waitingLoc = new LocString("gameplay_ui", "MULTIPLAYER_WAITING");

		// Token: 0x04000BD0 RID: 3024
		[Nullable(0)]
		private NRewardsScreen.CompletedEventHandler backing_Completed;

		// Token: 0x02001269 RID: 4713
		// (Invoke) Token: 0x06009D1B RID: 40219
		[NullableContext(0)]
		[Signal]
		public delegate void CompletedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200126A RID: 4714
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400475E RID: 18270
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RewardCollectedFrom' method.
			/// </summary>
			// Token: 0x0400475F RID: 18271
			public static readonly StringName RewardCollectedFrom = "RewardCollectedFrom";

			/// <summary>
			/// Cached name for the 'RewardSkippedFrom' method.
			/// </summary>
			// Token: 0x04004760 RID: 18272
			public static readonly StringName RewardSkippedFrom = "RewardSkippedFrom";

			/// <summary>
			/// Cached name for the 'UpdateScreenState' method.
			/// </summary>
			// Token: 0x04004761 RID: 18273
			public static readonly StringName UpdateScreenState = "UpdateScreenState";

			/// <summary>
			/// Cached name for the 'RemoveButton' method.
			/// </summary>
			// Token: 0x04004762 RID: 18274
			public static readonly StringName RemoveButton = "RemoveButton";

			/// <summary>
			/// Cached name for the 'OnProceedButtonPressed' method.
			/// </summary>
			// Token: 0x04004763 RID: 18275
			public static readonly StringName OnProceedButtonPressed = "OnProceedButtonPressed";

			/// <summary>
			/// Cached name for the 'BeforeRoomExit' method.
			/// </summary>
			// Token: 0x04004764 RID: 18276
			public static readonly StringName BeforeRoomExit = "BeforeRoomExit";

			/// <summary>
			/// Cached name for the 'AfterOverlayOpened' method.
			/// </summary>
			// Token: 0x04004765 RID: 18277
			public static readonly StringName AfterOverlayOpened = "AfterOverlayOpened";

			/// <summary>
			/// Cached name for the 'AfterOverlayClosed' method.
			/// </summary>
			// Token: 0x04004766 RID: 18278
			public static readonly StringName AfterOverlayClosed = "AfterOverlayClosed";

			/// <summary>
			/// Cached name for the 'TryEnableProceedButton' method.
			/// </summary>
			// Token: 0x04004767 RID: 18279
			public static readonly StringName TryEnableProceedButton = "TryEnableProceedButton";

			/// <summary>
			/// Cached name for the 'AfterOverlayShown' method.
			/// </summary>
			// Token: 0x04004768 RID: 18280
			public static readonly StringName AfterOverlayShown = "AfterOverlayShown";

			/// <summary>
			/// Cached name for the 'AfterOverlayHidden' method.
			/// </summary>
			// Token: 0x04004769 RID: 18281
			public static readonly StringName AfterOverlayHidden = "AfterOverlayHidden";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x0400476A RID: 18282
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'ProcessScrollEvent' method.
			/// </summary>
			// Token: 0x0400476B RID: 18283
			public static readonly StringName ProcessScrollEvent = "ProcessScrollEvent";

			/// <summary>
			/// Cached name for the 'ProcessGuiFocus' method.
			/// </summary>
			// Token: 0x0400476C RID: 18284
			public static readonly StringName ProcessGuiFocus = "ProcessGuiFocus";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x0400476D RID: 18285
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'UpdateScrollPosition' method.
			/// </summary>
			// Token: 0x0400476E RID: 18286
			public static readonly StringName UpdateScrollPosition = "UpdateScrollPosition";

			/// <summary>
			/// Cached name for the 'HideWaitingForPlayersScreen' method.
			/// </summary>
			// Token: 0x0400476F RID: 18287
			public static readonly StringName HideWaitingForPlayersScreen = "HideWaitingForPlayersScreen";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200126B RID: 4715
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CanScroll' property.
			/// </summary>
			// Token: 0x04004770 RID: 18288
			public static readonly StringName CanScroll = "CanScroll";

			/// <summary>
			/// Cached name for the 'ScrollLimitBottom' property.
			/// </summary>
			// Token: 0x04004771 RID: 18289
			public static readonly StringName ScrollLimitBottom = "ScrollLimitBottom";

			/// <summary>
			/// Cached name for the 'IsComplete' property.
			/// </summary>
			// Token: 0x04004772 RID: 18290
			public static readonly StringName IsComplete = "IsComplete";

			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x04004773 RID: 18291
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04004774 RID: 18292
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004775 RID: 18293
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the 'FocusedControlFromTopBar' property.
			/// </summary>
			// Token: 0x04004776 RID: 18294
			public static readonly StringName FocusedControlFromTopBar = "FocusedControlFromTopBar";

			/// <summary>
			/// Cached name for the '_proceedButton' field.
			/// </summary>
			// Token: 0x04004777 RID: 18295
			public static readonly StringName _proceedButton = "_proceedButton";

			/// <summary>
			/// Cached name for the '_rewardsContainer' field.
			/// </summary>
			// Token: 0x04004778 RID: 18296
			public static readonly StringName _rewardsContainer = "_rewardsContainer";

			/// <summary>
			/// Cached name for the '_scrollbar' field.
			/// </summary>
			// Token: 0x04004779 RID: 18297
			public static readonly StringName _scrollbar = "_scrollbar";

			/// <summary>
			/// Cached name for the '_headerLabel' field.
			/// </summary>
			// Token: 0x0400477A RID: 18298
			public static readonly StringName _headerLabel = "_headerLabel";

			/// <summary>
			/// Cached name for the '_rewardContainerMask' field.
			/// </summary>
			// Token: 0x0400477B RID: 18299
			public static readonly StringName _rewardContainerMask = "_rewardContainerMask";

			/// <summary>
			/// Cached name for the '_waitingForOtherPlayersOverlay' field.
			/// </summary>
			// Token: 0x0400477C RID: 18300
			public static readonly StringName _waitingForOtherPlayersOverlay = "_waitingForOtherPlayersOverlay";

			/// <summary>
			/// Cached name for the '_rewardsWindow' field.
			/// </summary>
			// Token: 0x0400477D RID: 18301
			public static readonly StringName _rewardsWindow = "_rewardsWindow";

			/// <summary>
			/// Cached name for the '_targetDragPos' field.
			/// </summary>
			// Token: 0x0400477E RID: 18302
			public static readonly StringName _targetDragPos = "_targetDragPos";

			/// <summary>
			/// Cached name for the '_scrollbarPressed' field.
			/// </summary>
			// Token: 0x0400477F RID: 18303
			public static readonly StringName _scrollbarPressed = "_scrollbarPressed";

			/// <summary>
			/// Cached name for the '_fadeTween' field.
			/// </summary>
			// Token: 0x04004780 RID: 18304
			public static readonly StringName _fadeTween = "_fadeTween";

			/// <summary>
			/// Cached name for the '_lastRewardFocused' field.
			/// </summary>
			// Token: 0x04004781 RID: 18305
			public static readonly StringName _lastRewardFocused = "_lastRewardFocused";

			/// <summary>
			/// Cached name for the '_disableProceedForever' field.
			/// </summary>
			// Token: 0x04004782 RID: 18306
			public static readonly StringName _disableProceedForever = "_disableProceedForever";

			/// <summary>
			/// Cached name for the '_isTerminal' field.
			/// </summary>
			// Token: 0x04004783 RID: 18307
			public static readonly StringName _isTerminal = "_isTerminal";

			/// <summary>
			/// Cached name for the '_skipDisallowed' field.
			/// </summary>
			// Token: 0x04004784 RID: 18308
			public static readonly StringName _skipDisallowed = "_skipDisallowed";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200126C RID: 4716
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'Completed' signal.
			/// </summary>
			// Token: 0x04004785 RID: 18309
			public static readonly StringName Completed = "Completed";
		}
	}
}
