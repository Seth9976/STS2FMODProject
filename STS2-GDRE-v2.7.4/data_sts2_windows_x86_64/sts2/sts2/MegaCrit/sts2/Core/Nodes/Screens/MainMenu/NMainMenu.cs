using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Connection;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x02000307 RID: 775
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMainMenu.cs")]
	public class NMainMenu : Control, IScreenContext
	{
		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002A86 RID: 10886 RVA: 0x001290F8 File Offset: 0x001272F8
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/screens/main_menu.tscn");
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002A87 RID: 10887 RVA: 0x00129104 File Offset: 0x00127304
		// (set) Token: 0x06002A88 RID: 10888 RVA: 0x0012910C File Offset: 0x0012730C
		public NPatchNotesScreen PatchNotesScreen { get; private set; }

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002A89 RID: 10889 RVA: 0x00129115 File Offset: 0x00127315
		// (set) Token: 0x06002A8A RID: 10890 RVA: 0x0012911D File Offset: 0x0012731D
		public Control BlurBackstop { get; private set; }

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002A8B RID: 10891 RVA: 0x00129128 File Offset: 0x00127328
		private NButton[] MainMenuButtons
		{
			get
			{
				return new NButton[] { this._continueButton, this._abandonRunButton, this._singleplayerButton, this._multiplayerButton, this._timelineButton, this._settingsButton, this._compendiumButton, this._quitButton };
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002A8C RID: 10892 RVA: 0x00129183 File Offset: 0x00127383
		// (set) Token: 0x06002A8D RID: 10893 RVA: 0x0012918B File Offset: 0x0012738B
		public NMainMenuSubmenuStack SubmenuStack { get; private set; }

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002A8E RID: 10894 RVA: 0x00129194 File Offset: 0x00127394
		public NContinueRunInfo ContinueRunInfo
		{
			get
			{
				return this._runInfo;
			}
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x0012919C File Offset: 0x0012739C
		public static NMainMenu Create(bool openTimeline)
		{
			NMainMenu nmainMenu = PreloadManager.Cache.GetScene("res://scenes/screens/main_menu.tscn").Instantiate<NMainMenu>(0L);
			nmainMenu._openTimeline = openTimeline;
			return nmainMenu;
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x001291C8 File Offset: 0x001273C8
		public override void _Ready()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
			defaultInterpolatedStringHandler.AppendLiteral("[Startup] Time to main menu (Godot ticks): ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(Time.GetTicksMsec());
			defaultInterpolatedStringHandler.AppendLiteral("ms");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			this._window = base.GetTree().Root;
			NGame.Instance.Connect(NGame.SignalName.WindowChange, Callable.From<bool>(new Action<bool>(this.OnWindowChange)), 0U);
			if (SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto)
			{
				this.OnWindowChange(true);
			}
			this._continueButton = base.GetNode<NMainMenuTextButton>("MainMenuTextButtons/ContinueButton");
			this._continueButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnContinueButtonPressed)), 0U);
			this._continueButton.SetLocalization("CONTINUE");
			this._abandonRunButton = base.GetNode<NMainMenuTextButton>("MainMenuTextButtons/AbandonRunButton");
			this._abandonRunButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnAbandonRunButtonPressed)), 0U);
			this._abandonRunButton.SetLocalization("ABANDON_RUN");
			this._singleplayerButton = base.GetNode<NMainMenuTextButton>("MainMenuTextButtons/SingleplayerButton");
			this._singleplayerButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.SingleplayerButtonPressed)), 0U);
			this._singleplayerButton.SetLocalization("SINGLE_PLAYER");
			this._multiplayerButton = base.GetNode<NMainMenuTextButton>("MainMenuTextButtons/MultiplayerButton");
			this._multiplayerButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenMultiplayerSubmenu)), 0U);
			this._multiplayerButton.SetLocalization("MULTIPLAYER");
			this._compendiumButton = base.GetNode<NMainMenuTextButton>("MainMenuTextButtons/CompendiumButton");
			this._compendiumButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenCompendiumSubmenu)), 0U);
			this._compendiumButton.SetLocalization("COMPENDIUM");
			this._timelineButton = base.GetNode<NMainMenuTextButton>("MainMenuTextButtons/TimelineButton");
			this._timelineButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenTimelineScreen)), 0U);
			this._timelineButton.SetLocalization("TIMELINE");
			this._timelineNotificationDot = base.GetNode<Control>("%TimelineNotificationDot");
			this._timelineNotificationDot.Visible = SaveManager.Instance.GetDiscoveredEpochCount() > 0;
			this._settingsButton = base.GetNode<NMainMenuTextButton>("MainMenuTextButtons/SettingsButton");
			this._settingsButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenSettingsMenu)), 0U);
			this._settingsButton.SetLocalization("SETTINGS");
			this._quitButton = base.GetNode<NMainMenuTextButton>("MainMenuTextButtons/QuitButton");
			GodotObject quitButton = this._quitButton;
			StringName released = NClickableControl.SignalName.Released;
			Action<NButton> action;
			if ((action = NMainMenu.<>O.<0>__Quit) == null)
			{
				action = (NMainMenu.<>O.<0>__Quit = new Action<NButton>(NMainMenu.Quit));
			}
			quitButton.Connect(released, Callable.From<NButton>(action), 0U);
			this._quitButton.SetLocalization("QUIT");
			this._buttonReticleLeft = base.GetNode<Control>("%ButtonReticleLeft");
			this._buttonReticleRight = base.GetNode<Control>("%ButtonReticleRight");
			this.ConnectMainMenuTextButtonFocusLogic();
			this.PatchNotesScreen = base.GetNode<NPatchNotesScreen>("%PatchNotesScreen");
			this.SubmenuStack = base.GetNode<NMainMenuSubmenuStack>("%Submenus");
			this._runInfo = base.GetNode<NContinueRunInfo>("%ContinueRunInfo");
			this._patchNotesButtonNode = base.GetNode<NPatchNotesButton>("%PatchNotesButton");
			this._patchNotesButtonNode.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenPatchNotes)), 0U);
			this._openProfileScreenButton = base.GetNode<NOpenProfileScreenButton>("%ChangeProfileButton");
			this._bg = base.GetNode<NMainMenuBg>("%MainMenuBg");
			this.BlurBackstop = base.GetNode<Control>("BlurBackstop");
			this._blur = (ShaderMaterial)this.BlurBackstop.Material;
			this._timelineButton.Visible = SaveManager.Instance.Progress.Epochs.Count > 0;
			NGame.Instance.SetScreenShakeTarget(this);
			NAudioManager instance = NAudioManager.Instance;
			if (instance != null)
			{
				instance.PlayMusic("event:/music/menu_update");
			}
			this.SubmenuStack.InitializeForMainMenu(this);
			this.SubmenuStack.Connect(NSubmenuStack.SignalName.StackModified, Callable.From(new Action(this.OnSubmenuStackChanged)), 0U);
			this.OnSubmenuStackChanged();
			ActiveScreenContext.Instance.Update();
			this.RefreshButtons();
			this.CheckCommandLineArgs();
			if (SaveManager.Instance.SettingsSave.ModSettings == null && ModManager.Mods.Count > 0)
			{
				NModalContainer.Instance.Add(NConfirmModLoadingPopup.Create(), true);
			}
			TaskHelper.RunSafely(NGame.Instance.Transition.FadeIn(2f, "res://materials/transitions/fade_transition_mat.tres", null));
			PlatformUtil.SetRichPresence("MAIN_MENU", null, null);
			if (this._openTimeline)
			{
				TaskHelper.RunSafely(this.OpenTimelineFromGameOverScreen());
			}
			if (NGame.IsReleaseGame() && !SaveManager.Instance.SettingsSave.SeenEaDisclaimer)
			{
				NModalContainer.Instance.Add(NEarlyAccessDisclaimer.Create(), true);
			}
		}

		/// <summary>
		/// Helper to connect Focused/Unfocused signals to main menu text buttons.
		/// </summary>
		// Token: 0x06002A91 RID: 10897 RVA: 0x0012971C File Offset: 0x0012791C
		private void ConnectMainMenuTextButtonFocusLogic()
		{
			foreach (NMainMenuTextButton nmainMenuTextButton in base.GetNode<Control>("%MainMenuTextButtons").GetChildren(false).OfType<NMainMenuTextButton>())
			{
				nmainMenuTextButton.Connect(NClickableControl.SignalName.Unfocused, Callable.From<NMainMenuTextButton>(new Action<NMainMenuTextButton>(this.MainMenuButtonUnfocused)), 0U);
				nmainMenuTextButton.Connect(NClickableControl.SignalName.Focused, Callable.From<NMainMenuTextButton>(delegate(NMainMenuTextButton b)
				{
					Callable.From(delegate
					{
						this.MainMenuButtonFocused(b);
					}).CallDeferred(Array.Empty<Variant>());
				}), 0U);
			}
		}

		/// <summary>
		/// This handles the little carets animation for the main menu text buttons.
		/// </summary>
		// Token: 0x06002A92 RID: 10898 RVA: 0x001297B4 File Offset: 0x001279B4
		private void MainMenuButtonFocused(NMainMenuTextButton button)
		{
			Tween reticleTween = this._reticleTween;
			if (reticleTween != null)
			{
				reticleTween.Kill();
			}
			this._reticleTween = base.CreateTween().SetParallel(true);
			this._buttonReticleLeft.GlobalPosition = new Vector2(0f, button.GlobalPosition.Y + 5f);
			this._buttonReticleRight.GlobalPosition = new Vector2(0f, button.GlobalPosition.Y + 5f);
			MegaLabel label = button.label;
			float num = ((label != null) ? label.GlobalPosition.X : 0f);
			MegaLabel label2 = button.label;
			float num2 = ((label2 != null) ? label2.Size.X : 0f);
			float num3 = num - 20f - 6f;
			float num4 = num + num2 - 20f + 6f;
			this._reticleTween.TweenProperty(this._buttonReticleLeft, "global_position:x", num3 - 28f, 0.25).SetEase(1L).SetTrans(10L)
				.From(num3);
			this._reticleTween.TweenProperty(this._buttonReticleLeft, "modulate", StsColors.gold, 0.05).From(StsColors.transparentWhite);
			this._reticleTween.TweenProperty(this._buttonReticleRight, "global_position:x", num4 + 28f, 0.25).SetEase(1L).SetTrans(10L)
				.From(num4);
			this._reticleTween.TweenProperty(this._buttonReticleRight, "modulate", StsColors.gold, 0.05).From(StsColors.transparentWhite);
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x00129998 File Offset: 0x00127B98
		private void MainMenuButtonUnfocused(NMainMenuTextButton obj)
		{
			Tween reticleTween = this._reticleTween;
			if (reticleTween != null)
			{
				reticleTween.Kill();
			}
			this._reticleTween = base.CreateTween().SetParallel(true);
			this._reticleTween.TweenProperty(this._buttonReticleLeft, "modulate", StsColors.transparentWhite, 0.25);
			this._reticleTween.TweenProperty(this._buttonReticleRight, "modulate", StsColors.transparentWhite, 0.25);
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x00129A28 File Offset: 0x00127C28
		private async Task OpenTimelineFromGameOverScreen()
		{
			if (SaveManager.Instance.PrefsSave.FastMode != FastModeType.Instant)
			{
				await Task.Delay(500);
			}
			this.SubmenuStack.PushSubmenuType<NTimelineScreen>();
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x00129A6C File Offset: 0x00127C6C
		public void EnableBackstop()
		{
			this._bg.HideLogo();
			this.BlurBackstop.MouseFilter = 0L;
			Tween backstopTween = this._backstopTween;
			if (backstopTween != null)
			{
				backstopTween.Kill();
			}
			this._backstopTween = base.CreateTween().SetParallel(true);
			this._backstopTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderLod)), this._blur.GetShaderParameter(NMainMenu._lod), 3f, 0.25);
			this._backstopTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderMix)), this._blur.GetShaderParameter(NMainMenu._mixPercentage), 0.7f, 0.25);
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x00129B34 File Offset: 0x00127D34
		public void DisableBackstop()
		{
			this._bg.ShowLogo();
			this.BlurBackstop.MouseFilter = 2L;
			Tween backstopTween = this._backstopTween;
			if (backstopTween != null)
			{
				backstopTween.Kill();
			}
			this._backstopTween = base.CreateTween().SetParallel(true);
			this._backstopTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderLod)), this._blur.GetShaderParameter(NMainMenu._lod), 0f, 0.15);
			this._backstopTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderMix)), this._blur.GetShaderParameter(NMainMenu._mixPercentage), 0f, 0.15);
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x00129BFC File Offset: 0x00127DFC
		public void DisableBackstopInstantly()
		{
			Tween backstopTween = this._backstopTween;
			if (backstopTween != null)
			{
				backstopTween.Kill();
			}
			this._blur.SetShaderParameter(NMainMenu._lod, 0f);
			this._blur.SetShaderParameter(NMainMenu._mixPercentage, 0f);
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x00129C50 File Offset: 0x00127E50
		public void EnableBackstopInstantly()
		{
			Tween backstopTween = this._backstopTween;
			if (backstopTween != null)
			{
				backstopTween.Kill();
			}
			this._blur.SetShaderParameter(NMainMenu._lod, 3f);
			this._blur.SetShaderParameter(NMainMenu._mixPercentage, 0.7f);
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x00129CA2 File Offset: 0x00127EA2
		private void UpdateShaderMix(float obj)
		{
			this._blur.SetShaderParameter(NMainMenu._mixPercentage, obj);
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x00129CBA File Offset: 0x00127EBA
		private void UpdateShaderLod(float obj)
		{
			this._blur.SetShaderParameter(NMainMenu._lod, obj);
		}

		/// <summary>
		/// Re-checks the visibility of text buttons after we abandon a run from the main menu or
		/// close the Timeline screen. Useful because we may have unlocked new features.
		/// </summary>
		// Token: 0x06002A9B RID: 10907 RVA: 0x00129CD4 File Offset: 0x00127ED4
		public void RefreshButtons()
		{
			if (SaveManager.Instance.HasRunSave)
			{
				this._readRunSaveResult = SaveManager.Instance.LoadRunSave();
				this._singleplayerButton.Visible = false;
				this._abandonRunButton.Visible = true;
				this._continueButton.Visible = true;
				this._continueButton.SetEnabled(true);
				this._runInfo.SetResult(this._readRunSaveResult);
			}
			else
			{
				this._readRunSaveResult = null;
				this._singleplayerButton.Visible = true;
				this._abandonRunButton.Visible = false;
				this._continueButton.Visible = false;
				this._continueButton.SetEnabled(false);
				this._runInfo.SetResult(null);
			}
			this.UpdateTimelineButtonBehavior();
			this._compendiumButton.Visible = SaveManager.Instance.IsCompendiumAvailable();
			ActiveScreenContext.Instance.Update();
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x00129DA8 File Offset: 0x00127FA8
		private void UpdateTimelineButtonBehavior()
		{
			if (!DebugSettings.DevSkip && SaveManager.Instance.GetDiscoveredEpochCount() > 0 && !SaveManager.Instance.HasRunSave)
			{
				this._timelineButton.Enable();
				this._singleplayerButton.Disable();
				this._multiplayerButton.Disable();
				this._compendiumButton.Disable();
				this._timelineButton.Visible = true;
				this._timelineNotificationDot.Visible = true;
				return;
			}
			if (SaveManager.Instance.Progress.Epochs.Count > 1 && SaveManager.Instance.IsEpochRevealed<NeowEpoch>())
			{
				this._timelineButton.Visible = true;
				if (SaveManager.Instance.GetDiscoveredEpochCount() == 0)
				{
					this._timelineButton.Enable();
				}
				else
				{
					this._timelineButton.Disable();
				}
				this._timelineNotificationDot.Visible = false;
				this._singleplayerButton.Enable();
				this._multiplayerButton.Enable();
				this._compendiumButton.Enable();
				return;
			}
			if (SaveManager.Instance.Progress.Epochs.Count > 1)
			{
				this._timelineButton.Disable();
				return;
			}
			this._timelineButton.Visible = false;
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002A9D RID: 10909 RVA: 0x00129ECC File Offset: 0x001280CC
		public Control DefaultFocusedControl
		{
			get
			{
				if (this._lastHitButton == null || !this._lastHitButton.IsVisible())
				{
					return this.MainMenuButtons.First<NButton>((NButton b) => b.IsEnabled && b.IsVisible());
				}
				return this._lastHitButton;
			}
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x00129F20 File Offset: 0x00128120
		private void OnSubmenuStackChanged()
		{
			base.GetNode<Control>("MainMenuTextButtons").Visible = !this.SubmenuStack.SubmenusOpen;
			this._patchNotesButtonNode.Visible = !this.SubmenuStack.SubmenusOpen;
			if (this.SubmenuStack.SubmenusOpen)
			{
				this._openProfileScreenButton.Visible = false;
				this._patchNotesButtonNode.Visible = false;
				this._openProfileScreenButton.Disable();
				this._patchNotesButtonNode.Disable();
				return;
			}
			this._openProfileScreenButton.Visible = true;
			this._patchNotesButtonNode.Visible = true;
			this._openProfileScreenButton.Enable();
			this._patchNotesButtonNode.Enable();
			NAudioManager instance = NAudioManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.UpdateMusicParameter("menu_progress", "main");
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x00129FEC File Offset: 0x001281EC
		private void OnContinueButtonPressed(NButton _)
		{
			if (this._readRunSaveResult == null || !this._readRunSaveResult.Success || this._readRunSaveResult.SaveData == null)
			{
				this.DisplayLoadSaveError();
				return;
			}
			TaskHelper.RunSafely(this.OnContinueButtonPressedAsync());
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x0012A024 File Offset: 0x00128224
		private async Task OnContinueButtonPressedAsync()
		{
			try
			{
				this._continueButton.Disable();
				NAudioManager instance = NAudioManager.Instance;
				if (instance != null)
				{
					instance.StopMusic();
				}
				SerializableRun serializableRun = this._readRunSaveResult.SaveData;
				RunState runState = RunState.FromSerializable(serializableRun);
				await RunManager.Instance.SetUpSavedSingleplayer(runState, serializableRun);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Continuing run with character: ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(serializableRun.Players[0].CharacterId);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SfxCmd.Play(runState.Players[0].Character.CharacterTransitionSfx, 1f);
				await NGame.Instance.Transition.FadeOut(0.8f, runState.Players[0].Character.CharacterSelectTransitionPath, null);
				NGame.Instance.ReactionContainer.InitializeNetworking(new NetSingleplayerGameService());
				await NGame.Instance.LoadRun(runState, serializableRun.PreFinishedRoom);
				await NGame.Instance.Transition.FadeIn(0.8f, "res://materials/transitions/fade_transition_mat.tres", null);
				serializableRun = null;
				runState = null;
			}
			catch (Exception)
			{
				this.DisplayLoadSaveError();
				RunManager.Instance.CleanUp(true);
				throw;
			}
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x0012A068 File Offset: 0x00128268
		private void DisplayLoadSaveError()
		{
			NErrorPopup nerrorPopup = NErrorPopup.Create(new LocString("main_menu_ui", "INVALID_SAVE_POPUP.title"), new LocString("main_menu_ui", "INVALID_SAVE_POPUP.description_run"), new LocString("main_menu_ui", "INVALID_SAVE_POPUP.dismiss"), true);
			NModalContainer.Instance.Add(nerrorPopup, true);
			NModalContainer.Instance.ShowBackstop();
			this._continueButton.Disable();
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x0012A0CA File Offset: 0x001282CA
		private void OnAbandonRunButtonPressed(NButton _)
		{
			NModalContainer.Instance.Add(NAbandonRunConfirmPopup.Create(this), true);
			this._lastHitButton = this._abandonRunButton;
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x0012A0EC File Offset: 0x001282EC
		public void AbandonRun()
		{
			if (this._readRunSaveResult == null)
			{
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this._readRunSaveResult.Success && this._readRunSaveResult.SaveData != null)
			{
				try
				{
					Log.Info("Abandoning run from main menu", 2);
					SerializableRun saveData = this._readRunSaveResult.SaveData;
					SaveManager.Instance.UpdateProgressWithRunData(saveData, false);
					RunHistoryUtilities.CreateRunHistoryEntry(saveData, false, true, saveData.PlatformType);
					if (saveData.DailyTime != null)
					{
						int num = ScoreUtility.CalculateDailyScore(saveData, saveData.Players.First<SerializablePlayer>().NetId, false);
						TaskHelper.RunSafely(DailyRunUtility.UploadScore(saveData.DailyTime.Value, num, saveData.Players));
					}
					goto IL_0113;
				}
				catch (Exception ex)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
					defaultInterpolatedStringHandler.AppendLiteral("ERROR: Failed to upload run history/metrics: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					goto IL_0113;
				}
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Abandoning run with invalid save (status=");
			defaultInterpolatedStringHandler.AppendFormatted<ReadSaveStatus>(this._readRunSaveResult.Status);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			IL_0113:
			SaveManager.Instance.DeleteCurrentRun();
			this.RefreshButtons();
			GC.Collect();
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x0012A234 File Offset: 0x00128434
		private void SingleplayerButtonPressed(NButton _)
		{
			if (SaveManager.Instance.Progress.NumberOfRuns > 0)
			{
				this.OpenSingleplayerSubmenu();
				return;
			}
			NCharacterSelectScreen submenuType = this.SubmenuStack.GetSubmenuType<NCharacterSelectScreen>();
			submenuType.InitializeSingleplayer();
			this.SubmenuStack.Push(submenuType);
			this._lastHitButton = this._singleplayerButton;
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x0012A285 File Offset: 0x00128485
		public NSingleplayerSubmenu OpenSingleplayerSubmenu()
		{
			this._lastHitButton = this._singleplayerButton;
			return this.SubmenuStack.PushSubmenuType<NSingleplayerSubmenu>();
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x0012A29E File Offset: 0x0012849E
		public void OpenMultiplayerSubmenu(NButton _)
		{
			this.OpenMultiplayerSubmenu();
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x0012A2A7 File Offset: 0x001284A7
		public NMultiplayerSubmenu OpenMultiplayerSubmenu()
		{
			this._lastHitButton = this._multiplayerButton;
			return this.SubmenuStack.PushSubmenuType<NMultiplayerSubmenu>();
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x0012A2C0 File Offset: 0x001284C0
		private void OpenCompendiumSubmenu(NButton _)
		{
			this.OpenCompendiumSubmenu();
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x0012A2C9 File Offset: 0x001284C9
		public NCompendiumSubmenu OpenCompendiumSubmenu()
		{
			this._lastHitButton = this._compendiumButton;
			return this.SubmenuStack.PushSubmenuType<NCompendiumSubmenu>();
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x0012A2E2 File Offset: 0x001284E2
		private void OpenTimelineScreen(NButton obj)
		{
			this._lastHitButton = this._timelineButton;
			NAudioManager instance = NAudioManager.Instance;
			if (instance != null)
			{
				instance.UpdateMusicParameter("menu_progress", "timeline");
			}
			this.SubmenuStack.PushSubmenuType<NTimelineScreen>();
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x0012A316 File Offset: 0x00128516
		private void OpenSettingsMenu(NButton _)
		{
			this.OpenSettingsMenu();
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x0012A31E File Offset: 0x0012851E
		public void OpenProfileScreen()
		{
			this.SubmenuStack.PushSubmenuType<NProfileScreen>();
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x0012A32C File Offset: 0x0012852C
		public void OpenSettingsMenu()
		{
			this._lastHitButton = this._settingsButton;
			this.SubmenuStack.PushSubmenuType<NSettingsScreen>();
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x0012A346 File Offset: 0x00128546
		private void OpenPatchNotes(NButton _)
		{
			this.PatchNotesScreen.Open();
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x0012A354 File Offset: 0x00128554
		public async Task JoinGame(IClientConnectionInitializer connInitializer)
		{
			NMultiplayerSubmenu nmultiplayerSubmenu = this.OpenMultiplayerSubmenu();
			NJoinFriendScreen njoinFriendScreen = nmultiplayerSubmenu.OnJoinFriendsPressed();
			await njoinFriendScreen.JoinGameAsync(connInitializer);
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x0012A39F File Offset: 0x0012859F
		private static void Quit(NButton _)
		{
			Log.Info("Quit button pressed", 2);
			TaskHelper.RunSafely(NMainMenu.ConfirmAndQuit());
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x0012A3B8 File Offset: 0x001285B8
		private static async Task ConfirmAndQuit()
		{
			NGenericPopup ngenericPopup = NGenericPopup.Create();
			NModalContainer.Instance.Add(ngenericPopup, true);
			bool flag = await ngenericPopup.WaitForConfirmation(new LocString("main_menu_ui", "QUIT_CONFIRM_POPUP.body"), new LocString("main_menu_ui", "QUIT_CONFIRM_POPUP.header"), new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), new LocString("main_menu_ui", "GENERIC_POPUP.confirm"));
			bool flag2 = flag;
			if (flag2)
			{
				Log.Info("Quit confirmed", 2);
				NGame instance = NGame.Instance;
				if (instance != null)
				{
					instance.Quit();
				}
			}
			else
			{
				Log.Info("Quit cancelled", 2);
			}
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x0012A3F4 File Offset: 0x001285F4
		private void OnWindowChange(bool isAspectRatioAuto)
		{
			if (!isAspectRatioAuto)
			{
				return;
			}
			float num = (float)this._window.Size.X / (float)this._window.Size.Y;
			if (num > 2.3888888f)
			{
				this._window.ContentScaleAspect = 2L;
				this._window.ContentScaleSize = new Vector2I(2580, 1080);
				return;
			}
			if (num < 1.3333334f)
			{
				this._window.ContentScaleAspect = 3L;
				this._window.ContentScaleSize = new Vector2I(1680, 1260);
				return;
			}
			this._window.ContentScaleAspect = 4L;
			this._window.ContentScaleSize = new Vector2I(1680, 1080);
		}

		/// <summary>
		/// For getting into multiplayer testing quickly. Checks some command line args and skips through menu clicking
		/// based on the args.
		/// </summary>
		// Token: 0x06002AB3 RID: 10931 RVA: 0x0012A4B0 File Offset: 0x001286B0
		private void CheckCommandLineArgs()
		{
			string text;
			if (CommandLineHelper.TryGetValue("fastmp", out text))
			{
				NMultiplayerSubmenu nmultiplayerSubmenu = this.OpenMultiplayerSubmenu();
				if (!(text == "host") && !(text == "host_standard") && !(text == "host_daily") && !(text == "host_custom"))
				{
					if (!(text == "load"))
					{
						if (text == "join")
						{
							nmultiplayerSubmenu.OnJoinFriendsPressed();
							return;
						}
						Log.Error("fastmp command line argument passed with invalid value: " + text + ". Expected host, load, or join", 2);
					}
					else
					{
						PlatformType platformType = (SteamInitializer.Initialized ? PlatformType.Steam : PlatformType.None);
						ulong localPlayerId = PlatformUtil.GetLocalPlayerId(platformType);
						ReadSaveResult<SerializableRun> readSaveResult = SaveManager.Instance.LoadAndCanonicalizeMultiplayerRunSave(localPlayerId);
						if (readSaveResult.SaveData != null)
						{
							nmultiplayerSubmenu.StartHost(readSaveResult.SaveData);
							return;
						}
						Log.Error("Failed to load multiplayer save", 2);
						return;
					}
				}
				else
				{
					GameMode gameMode;
					if (!(text == "host_standard"))
					{
						if (!(text == "host_daily"))
						{
							if (!(text == "host_custom"))
							{
								gameMode = GameMode.None;
							}
							else
							{
								gameMode = GameMode.Custom;
							}
						}
						else
						{
							gameMode = GameMode.Daily;
						}
					}
					else
					{
						gameMode = GameMode.Standard;
					}
					GameMode gameMode2 = gameMode;
					if (gameMode2 != GameMode.None)
					{
						nmultiplayerSubmenu.FastHost(gameMode2);
						return;
					}
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002AB4 RID: 10932 RVA: 0x0012A5D8 File Offset: 0x001287D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(32)
			{
				new MethodInfo(NMainMenu.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "openTimeline", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMainMenu.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.ConnectMainMenuTextButtonFocusLogic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.MainMenuButtonFocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.MainMenuButtonUnfocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "obj", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.EnableBackstop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.DisableBackstop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.DisableBackstopInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.EnableBackstopInstantly, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.UpdateShaderMix, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "obj", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.UpdateShaderLod, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "obj", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.RefreshButtons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.UpdateTimelineButtonBehavior, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.OnSubmenuStackChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.OnContinueButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.DisplayLoadSaveError, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.OnAbandonRunButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.AbandonRun, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.SingleplayerButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.OpenSingleplayerSubmenu, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.OpenMultiplayerSubmenu, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.OpenMultiplayerSubmenu, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.OpenCompendiumSubmenu, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.OpenCompendiumSubmenu, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.OpenTimelineScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "obj", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.OpenSettingsMenu, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.OpenProfileScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.OpenSettingsMenu, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenu.MethodName.OpenPatchNotes, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.Quit, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isAspectRatioAuto", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMainMenu.MethodName.CheckCommandLineArgs, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002AB5 RID: 10933 RVA: 0x0012AEA0 File Offset: 0x001290A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMainMenu.MethodName.Create && args.Count == 1)
			{
				NMainMenu nmainMenu = NMainMenu.Create(VariantUtils.ConvertTo<bool>(args[0]));
				ret = VariantUtils.CreateFrom<NMainMenu>(ref nmainMenu);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.ConnectMainMenuTextButtonFocusLogic && args.Count == 0)
			{
				this.ConnectMainMenuTextButtonFocusLogic();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.MainMenuButtonFocused && args.Count == 1)
			{
				this.MainMenuButtonFocused(VariantUtils.ConvertTo<NMainMenuTextButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.MainMenuButtonUnfocused && args.Count == 1)
			{
				this.MainMenuButtonUnfocused(VariantUtils.ConvertTo<NMainMenuTextButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.EnableBackstop && args.Count == 0)
			{
				this.EnableBackstop();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.DisableBackstop && args.Count == 0)
			{
				this.DisableBackstop();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.DisableBackstopInstantly && args.Count == 0)
			{
				this.DisableBackstopInstantly();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.EnableBackstopInstantly && args.Count == 0)
			{
				this.EnableBackstopInstantly();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.UpdateShaderMix && args.Count == 1)
			{
				this.UpdateShaderMix(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.UpdateShaderLod && args.Count == 1)
			{
				this.UpdateShaderLod(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.RefreshButtons && args.Count == 0)
			{
				this.RefreshButtons();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.UpdateTimelineButtonBehavior && args.Count == 0)
			{
				this.UpdateTimelineButtonBehavior();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OnSubmenuStackChanged && args.Count == 0)
			{
				this.OnSubmenuStackChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OnContinueButtonPressed && args.Count == 1)
			{
				this.OnContinueButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.DisplayLoadSaveError && args.Count == 0)
			{
				this.DisplayLoadSaveError();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OnAbandonRunButtonPressed && args.Count == 1)
			{
				this.OnAbandonRunButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.AbandonRun && args.Count == 0)
			{
				this.AbandonRun();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.SingleplayerButtonPressed && args.Count == 1)
			{
				this.SingleplayerButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OpenSingleplayerSubmenu && args.Count == 0)
			{
				NSingleplayerSubmenu nsingleplayerSubmenu = this.OpenSingleplayerSubmenu();
				ret = VariantUtils.CreateFrom<NSingleplayerSubmenu>(ref nsingleplayerSubmenu);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OpenMultiplayerSubmenu && args.Count == 1)
			{
				this.OpenMultiplayerSubmenu(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OpenMultiplayerSubmenu && args.Count == 0)
			{
				NMultiplayerSubmenu nmultiplayerSubmenu = this.OpenMultiplayerSubmenu();
				ret = VariantUtils.CreateFrom<NMultiplayerSubmenu>(ref nmultiplayerSubmenu);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OpenCompendiumSubmenu && args.Count == 1)
			{
				this.OpenCompendiumSubmenu(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OpenCompendiumSubmenu && args.Count == 0)
			{
				NCompendiumSubmenu ncompendiumSubmenu = this.OpenCompendiumSubmenu();
				ret = VariantUtils.CreateFrom<NCompendiumSubmenu>(ref ncompendiumSubmenu);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OpenTimelineScreen && args.Count == 1)
			{
				this.OpenTimelineScreen(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OpenSettingsMenu && args.Count == 1)
			{
				this.OpenSettingsMenu(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OpenProfileScreen && args.Count == 0)
			{
				this.OpenProfileScreen();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OpenSettingsMenu && args.Count == 0)
			{
				this.OpenSettingsMenu();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OpenPatchNotes && args.Count == 1)
			{
				this.OpenPatchNotes(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.Quit && args.Count == 1)
			{
				NMainMenu.Quit(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.OnWindowChange && args.Count == 1)
			{
				this.OnWindowChange(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.CheckCommandLineArgs && args.Count == 0)
			{
				this.CheckCommandLineArgs();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x0012B444 File Offset: 0x00129644
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMainMenu.MethodName.Create && args.Count == 1)
			{
				NMainMenu nmainMenu = NMainMenu.Create(VariantUtils.ConvertTo<bool>(args[0]));
				ret = VariantUtils.CreateFrom<NMainMenu>(ref nmainMenu);
				return true;
			}
			if ((ref method) == NMainMenu.MethodName.Quit && args.Count == 1)
			{
				NMainMenu.Quit(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002AB7 RID: 10935 RVA: 0x0012B4C4 File Offset: 0x001296C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMainMenu.MethodName.Create || (ref method) == NMainMenu.MethodName._Ready || (ref method) == NMainMenu.MethodName.ConnectMainMenuTextButtonFocusLogic || (ref method) == NMainMenu.MethodName.MainMenuButtonFocused || (ref method) == NMainMenu.MethodName.MainMenuButtonUnfocused || (ref method) == NMainMenu.MethodName.EnableBackstop || (ref method) == NMainMenu.MethodName.DisableBackstop || (ref method) == NMainMenu.MethodName.DisableBackstopInstantly || (ref method) == NMainMenu.MethodName.EnableBackstopInstantly || (ref method) == NMainMenu.MethodName.UpdateShaderMix || (ref method) == NMainMenu.MethodName.UpdateShaderLod || (ref method) == NMainMenu.MethodName.RefreshButtons || (ref method) == NMainMenu.MethodName.UpdateTimelineButtonBehavior || (ref method) == NMainMenu.MethodName.OnSubmenuStackChanged || (ref method) == NMainMenu.MethodName.OnContinueButtonPressed || (ref method) == NMainMenu.MethodName.DisplayLoadSaveError || (ref method) == NMainMenu.MethodName.OnAbandonRunButtonPressed || (ref method) == NMainMenu.MethodName.AbandonRun || (ref method) == NMainMenu.MethodName.SingleplayerButtonPressed || (ref method) == NMainMenu.MethodName.OpenSingleplayerSubmenu || (ref method) == NMainMenu.MethodName.OpenMultiplayerSubmenu || (ref method) == NMainMenu.MethodName.OpenCompendiumSubmenu || (ref method) == NMainMenu.MethodName.OpenTimelineScreen || (ref method) == NMainMenu.MethodName.OpenSettingsMenu || (ref method) == NMainMenu.MethodName.OpenProfileScreen || (ref method) == NMainMenu.MethodName.OpenPatchNotes || (ref method) == NMainMenu.MethodName.Quit || (ref method) == NMainMenu.MethodName.OnWindowChange || (ref method) == NMainMenu.MethodName.CheckCommandLineArgs || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002AB8 RID: 10936 RVA: 0x0012B68C File Offset: 0x0012988C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMainMenu.PropertyName.PatchNotesScreen)
			{
				this.PatchNotesScreen = VariantUtils.ConvertTo<NPatchNotesScreen>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName.BlurBackstop)
			{
				this.BlurBackstop = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName.SubmenuStack)
			{
				this.SubmenuStack = VariantUtils.ConvertTo<NMainMenuSubmenuStack>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._window)
			{
				this._window = VariantUtils.ConvertTo<Window>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._continueButton)
			{
				this._continueButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._abandonRunButton)
			{
				this._abandonRunButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._singleplayerButton)
			{
				this._singleplayerButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._compendiumButton)
			{
				this._compendiumButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._timelineButton)
			{
				this._timelineButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._settingsButton)
			{
				this._settingsButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._quitButton)
			{
				this._quitButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._multiplayerButton)
			{
				this._multiplayerButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._buttonReticleLeft)
			{
				this._buttonReticleLeft = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._buttonReticleRight)
			{
				this._buttonReticleRight = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._reticleTween)
			{
				this._reticleTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._patchNotesButtonNode)
			{
				this._patchNotesButtonNode = VariantUtils.ConvertTo<NPatchNotesButton>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._openProfileScreenButton)
			{
				this._openProfileScreenButton = VariantUtils.ConvertTo<NOpenProfileScreenButton>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._lastHitButton)
			{
				this._lastHitButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._runInfo)
			{
				this._runInfo = VariantUtils.ConvertTo<NContinueRunInfo>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._timelineNotificationDot)
			{
				this._timelineNotificationDot = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._backstopTween)
			{
				this._backstopTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._bg)
			{
				this._bg = VariantUtils.ConvertTo<NMainMenuBg>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._blur)
			{
				this._blur = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._openTimeline)
			{
				this._openTimeline = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002AB9 RID: 10937 RVA: 0x0012B92C File Offset: 0x00129B2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMainMenu.PropertyName.PatchNotesScreen)
			{
				NPatchNotesScreen patchNotesScreen = this.PatchNotesScreen;
				value = VariantUtils.CreateFrom<NPatchNotesScreen>(ref patchNotesScreen);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName.BlurBackstop)
			{
				Control control = this.BlurBackstop;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName.MainMenuButtons)
			{
				GodotObject[] mainMenuButtons = this.MainMenuButtons;
				value = VariantUtils.CreateFromSystemArrayOfGodotObject(mainMenuButtons);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName.SubmenuStack)
			{
				NMainMenuSubmenuStack submenuStack = this.SubmenuStack;
				value = VariantUtils.CreateFrom<NMainMenuSubmenuStack>(ref submenuStack);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName.ContinueRunInfo)
			{
				NContinueRunInfo continueRunInfo = this.ContinueRunInfo;
				value = VariantUtils.CreateFrom<NContinueRunInfo>(ref continueRunInfo);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._window)
			{
				value = VariantUtils.CreateFrom<Window>(ref this._window);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._continueButton)
			{
				value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._continueButton);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._abandonRunButton)
			{
				value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._abandonRunButton);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._singleplayerButton)
			{
				value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._singleplayerButton);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._compendiumButton)
			{
				value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._compendiumButton);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._timelineButton)
			{
				value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._timelineButton);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._settingsButton)
			{
				value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._settingsButton);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._quitButton)
			{
				value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._quitButton);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._multiplayerButton)
			{
				value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._multiplayerButton);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._buttonReticleLeft)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._buttonReticleLeft);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._buttonReticleRight)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._buttonReticleRight);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._reticleTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._reticleTween);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._patchNotesButtonNode)
			{
				value = VariantUtils.CreateFrom<NPatchNotesButton>(ref this._patchNotesButtonNode);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._openProfileScreenButton)
			{
				value = VariantUtils.CreateFrom<NOpenProfileScreenButton>(ref this._openProfileScreenButton);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._lastHitButton)
			{
				value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._lastHitButton);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._runInfo)
			{
				value = VariantUtils.CreateFrom<NContinueRunInfo>(ref this._runInfo);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._timelineNotificationDot)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._timelineNotificationDot);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._backstopTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._backstopTween);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._bg)
			{
				value = VariantUtils.CreateFrom<NMainMenuBg>(ref this._bg);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._blur)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._blur);
				return true;
			}
			if ((ref name) == NMainMenu.PropertyName._openTimeline)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._openTimeline);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002ABA RID: 10938 RVA: 0x0012BCB4 File Offset: 0x00129EB4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMainMenu.PropertyName.PatchNotesScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._window, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._continueButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._abandonRunButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._singleplayerButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._compendiumButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._timelineButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._settingsButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._quitButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._multiplayerButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._buttonReticleLeft, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._buttonReticleRight, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._reticleTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._patchNotesButtonNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName.BlurBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._openProfileScreenButton, 0L, "", 4096L, false),
				new PropertyInfo(28L, NMainMenu.PropertyName.MainMenuButtons, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._lastHitButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._runInfo, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._timelineNotificationDot, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._backstopTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._bg, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName._blur, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMainMenu.PropertyName._openTimeline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName.SubmenuStack, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName.ContinueRunInfo, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMainMenu.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002ABB RID: 10939 RVA: 0x0012C044 File Offset: 0x0012A244
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName patchNotesScreen = NMainMenu.PropertyName.PatchNotesScreen;
			NPatchNotesScreen patchNotesScreen2 = this.PatchNotesScreen;
			info.AddProperty(patchNotesScreen, Variant.From<NPatchNotesScreen>(ref patchNotesScreen2));
			StringName blurBackstop = NMainMenu.PropertyName.BlurBackstop;
			Control blurBackstop2 = this.BlurBackstop;
			info.AddProperty(blurBackstop, Variant.From<Control>(ref blurBackstop2));
			StringName submenuStack = NMainMenu.PropertyName.SubmenuStack;
			NMainMenuSubmenuStack submenuStack2 = this.SubmenuStack;
			info.AddProperty(submenuStack, Variant.From<NMainMenuSubmenuStack>(ref submenuStack2));
			info.AddProperty(NMainMenu.PropertyName._window, Variant.From<Window>(ref this._window));
			info.AddProperty(NMainMenu.PropertyName._continueButton, Variant.From<NMainMenuTextButton>(ref this._continueButton));
			info.AddProperty(NMainMenu.PropertyName._abandonRunButton, Variant.From<NMainMenuTextButton>(ref this._abandonRunButton));
			info.AddProperty(NMainMenu.PropertyName._singleplayerButton, Variant.From<NMainMenuTextButton>(ref this._singleplayerButton));
			info.AddProperty(NMainMenu.PropertyName._compendiumButton, Variant.From<NMainMenuTextButton>(ref this._compendiumButton));
			info.AddProperty(NMainMenu.PropertyName._timelineButton, Variant.From<NMainMenuTextButton>(ref this._timelineButton));
			info.AddProperty(NMainMenu.PropertyName._settingsButton, Variant.From<NMainMenuTextButton>(ref this._settingsButton));
			info.AddProperty(NMainMenu.PropertyName._quitButton, Variant.From<NMainMenuTextButton>(ref this._quitButton));
			info.AddProperty(NMainMenu.PropertyName._multiplayerButton, Variant.From<NMainMenuTextButton>(ref this._multiplayerButton));
			info.AddProperty(NMainMenu.PropertyName._buttonReticleLeft, Variant.From<Control>(ref this._buttonReticleLeft));
			info.AddProperty(NMainMenu.PropertyName._buttonReticleRight, Variant.From<Control>(ref this._buttonReticleRight));
			info.AddProperty(NMainMenu.PropertyName._reticleTween, Variant.From<Tween>(ref this._reticleTween));
			info.AddProperty(NMainMenu.PropertyName._patchNotesButtonNode, Variant.From<NPatchNotesButton>(ref this._patchNotesButtonNode));
			info.AddProperty(NMainMenu.PropertyName._openProfileScreenButton, Variant.From<NOpenProfileScreenButton>(ref this._openProfileScreenButton));
			info.AddProperty(NMainMenu.PropertyName._lastHitButton, Variant.From<NMainMenuTextButton>(ref this._lastHitButton));
			info.AddProperty(NMainMenu.PropertyName._runInfo, Variant.From<NContinueRunInfo>(ref this._runInfo));
			info.AddProperty(NMainMenu.PropertyName._timelineNotificationDot, Variant.From<Control>(ref this._timelineNotificationDot));
			info.AddProperty(NMainMenu.PropertyName._backstopTween, Variant.From<Tween>(ref this._backstopTween));
			info.AddProperty(NMainMenu.PropertyName._bg, Variant.From<NMainMenuBg>(ref this._bg));
			info.AddProperty(NMainMenu.PropertyName._blur, Variant.From<ShaderMaterial>(ref this._blur));
			info.AddProperty(NMainMenu.PropertyName._openTimeline, Variant.From<bool>(ref this._openTimeline));
		}

		/// <inheritdoc />
		// Token: 0x06002ABC RID: 10940 RVA: 0x0012C274 File Offset: 0x0012A474
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMainMenu.PropertyName.PatchNotesScreen, ref variant))
			{
				this.PatchNotesScreen = variant.As<NPatchNotesScreen>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMainMenu.PropertyName.BlurBackstop, ref variant2))
			{
				this.BlurBackstop = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMainMenu.PropertyName.SubmenuStack, ref variant3))
			{
				this.SubmenuStack = variant3.As<NMainMenuSubmenuStack>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMainMenu.PropertyName._window, ref variant4))
			{
				this._window = variant4.As<Window>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMainMenu.PropertyName._continueButton, ref variant5))
			{
				this._continueButton = variant5.As<NMainMenuTextButton>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMainMenu.PropertyName._abandonRunButton, ref variant6))
			{
				this._abandonRunButton = variant6.As<NMainMenuTextButton>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMainMenu.PropertyName._singleplayerButton, ref variant7))
			{
				this._singleplayerButton = variant7.As<NMainMenuTextButton>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMainMenu.PropertyName._compendiumButton, ref variant8))
			{
				this._compendiumButton = variant8.As<NMainMenuTextButton>();
			}
			Variant variant9;
			if (info.TryGetProperty(NMainMenu.PropertyName._timelineButton, ref variant9))
			{
				this._timelineButton = variant9.As<NMainMenuTextButton>();
			}
			Variant variant10;
			if (info.TryGetProperty(NMainMenu.PropertyName._settingsButton, ref variant10))
			{
				this._settingsButton = variant10.As<NMainMenuTextButton>();
			}
			Variant variant11;
			if (info.TryGetProperty(NMainMenu.PropertyName._quitButton, ref variant11))
			{
				this._quitButton = variant11.As<NMainMenuTextButton>();
			}
			Variant variant12;
			if (info.TryGetProperty(NMainMenu.PropertyName._multiplayerButton, ref variant12))
			{
				this._multiplayerButton = variant12.As<NMainMenuTextButton>();
			}
			Variant variant13;
			if (info.TryGetProperty(NMainMenu.PropertyName._buttonReticleLeft, ref variant13))
			{
				this._buttonReticleLeft = variant13.As<Control>();
			}
			Variant variant14;
			if (info.TryGetProperty(NMainMenu.PropertyName._buttonReticleRight, ref variant14))
			{
				this._buttonReticleRight = variant14.As<Control>();
			}
			Variant variant15;
			if (info.TryGetProperty(NMainMenu.PropertyName._reticleTween, ref variant15))
			{
				this._reticleTween = variant15.As<Tween>();
			}
			Variant variant16;
			if (info.TryGetProperty(NMainMenu.PropertyName._patchNotesButtonNode, ref variant16))
			{
				this._patchNotesButtonNode = variant16.As<NPatchNotesButton>();
			}
			Variant variant17;
			if (info.TryGetProperty(NMainMenu.PropertyName._openProfileScreenButton, ref variant17))
			{
				this._openProfileScreenButton = variant17.As<NOpenProfileScreenButton>();
			}
			Variant variant18;
			if (info.TryGetProperty(NMainMenu.PropertyName._lastHitButton, ref variant18))
			{
				this._lastHitButton = variant18.As<NMainMenuTextButton>();
			}
			Variant variant19;
			if (info.TryGetProperty(NMainMenu.PropertyName._runInfo, ref variant19))
			{
				this._runInfo = variant19.As<NContinueRunInfo>();
			}
			Variant variant20;
			if (info.TryGetProperty(NMainMenu.PropertyName._timelineNotificationDot, ref variant20))
			{
				this._timelineNotificationDot = variant20.As<Control>();
			}
			Variant variant21;
			if (info.TryGetProperty(NMainMenu.PropertyName._backstopTween, ref variant21))
			{
				this._backstopTween = variant21.As<Tween>();
			}
			Variant variant22;
			if (info.TryGetProperty(NMainMenu.PropertyName._bg, ref variant22))
			{
				this._bg = variant22.As<NMainMenuBg>();
			}
			Variant variant23;
			if (info.TryGetProperty(NMainMenu.PropertyName._blur, ref variant23))
			{
				this._blur = variant23.As<ShaderMaterial>();
			}
			Variant variant24;
			if (info.TryGetProperty(NMainMenu.PropertyName._openTimeline, ref variant24))
			{
				this._openTimeline = variant24.As<bool>();
			}
		}

		// Token: 0x04000FE2 RID: 4066
		private static readonly StringName _lod = new StringName("lod");

		// Token: 0x04000FE3 RID: 4067
		private static readonly StringName _mixPercentage = new StringName("mix_percentage");

		// Token: 0x04000FE4 RID: 4068
		private const string _scenePath = "res://scenes/screens/main_menu.tscn";

		// Token: 0x04000FE5 RID: 4069
		private const string _menuMusicParam = "menu_progress";

		// Token: 0x04000FE7 RID: 4071
		private Window _window;

		// Token: 0x04000FE8 RID: 4072
		private NMainMenuTextButton _continueButton;

		// Token: 0x04000FE9 RID: 4073
		private NMainMenuTextButton _abandonRunButton;

		// Token: 0x04000FEA RID: 4074
		private NMainMenuTextButton _singleplayerButton;

		// Token: 0x04000FEB RID: 4075
		private NMainMenuTextButton _compendiumButton;

		// Token: 0x04000FEC RID: 4076
		private NMainMenuTextButton _timelineButton;

		// Token: 0x04000FED RID: 4077
		private NMainMenuTextButton _settingsButton;

		// Token: 0x04000FEE RID: 4078
		private NMainMenuTextButton _quitButton;

		// Token: 0x04000FEF RID: 4079
		private NMainMenuTextButton _multiplayerButton;

		// Token: 0x04000FF0 RID: 4080
		private const float _reticleYOffset = 5f;

		// Token: 0x04000FF1 RID: 4081
		private const float _reticlePadding = 28f;

		// Token: 0x04000FF2 RID: 4082
		private Control _buttonReticleLeft;

		// Token: 0x04000FF3 RID: 4083
		private Control _buttonReticleRight;

		// Token: 0x04000FF4 RID: 4084
		[Nullable(2)]
		private Tween _reticleTween;

		// Token: 0x04000FF5 RID: 4085
		private NPatchNotesButton _patchNotesButtonNode;

		// Token: 0x04000FF7 RID: 4087
		private NOpenProfileScreenButton _openProfileScreenButton;

		// Token: 0x04000FF8 RID: 4088
		[Nullable(2)]
		private NMainMenuTextButton _lastHitButton;

		// Token: 0x04000FF9 RID: 4089
		private NContinueRunInfo _runInfo;

		// Token: 0x04000FFA RID: 4090
		private Control _timelineNotificationDot;

		// Token: 0x04000FFB RID: 4091
		[Nullable(2)]
		private Tween _backstopTween;

		// Token: 0x04000FFC RID: 4092
		private NMainMenuBg _bg;

		// Token: 0x04000FFD RID: 4093
		private ShaderMaterial _blur;

		// Token: 0x04000FFE RID: 4094
		private bool _openTimeline;

		// Token: 0x04000FFF RID: 4095
		[Nullable(new byte[] { 2, 1 })]
		private ReadSaveResult<SerializableRun> _readRunSaveResult;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001503 RID: 5379
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005002 RID: 20482
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005003 RID: 20483
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectMainMenuTextButtonFocusLogic' method.
			/// </summary>
			// Token: 0x04005004 RID: 20484
			public static readonly StringName ConnectMainMenuTextButtonFocusLogic = "ConnectMainMenuTextButtonFocusLogic";

			/// <summary>
			/// Cached name for the 'MainMenuButtonFocused' method.
			/// </summary>
			// Token: 0x04005005 RID: 20485
			public static readonly StringName MainMenuButtonFocused = "MainMenuButtonFocused";

			/// <summary>
			/// Cached name for the 'MainMenuButtonUnfocused' method.
			/// </summary>
			// Token: 0x04005006 RID: 20486
			public static readonly StringName MainMenuButtonUnfocused = "MainMenuButtonUnfocused";

			/// <summary>
			/// Cached name for the 'EnableBackstop' method.
			/// </summary>
			// Token: 0x04005007 RID: 20487
			public static readonly StringName EnableBackstop = "EnableBackstop";

			/// <summary>
			/// Cached name for the 'DisableBackstop' method.
			/// </summary>
			// Token: 0x04005008 RID: 20488
			public static readonly StringName DisableBackstop = "DisableBackstop";

			/// <summary>
			/// Cached name for the 'DisableBackstopInstantly' method.
			/// </summary>
			// Token: 0x04005009 RID: 20489
			public static readonly StringName DisableBackstopInstantly = "DisableBackstopInstantly";

			/// <summary>
			/// Cached name for the 'EnableBackstopInstantly' method.
			/// </summary>
			// Token: 0x0400500A RID: 20490
			public static readonly StringName EnableBackstopInstantly = "EnableBackstopInstantly";

			/// <summary>
			/// Cached name for the 'UpdateShaderMix' method.
			/// </summary>
			// Token: 0x0400500B RID: 20491
			public static readonly StringName UpdateShaderMix = "UpdateShaderMix";

			/// <summary>
			/// Cached name for the 'UpdateShaderLod' method.
			/// </summary>
			// Token: 0x0400500C RID: 20492
			public static readonly StringName UpdateShaderLod = "UpdateShaderLod";

			/// <summary>
			/// Cached name for the 'RefreshButtons' method.
			/// </summary>
			// Token: 0x0400500D RID: 20493
			public static readonly StringName RefreshButtons = "RefreshButtons";

			/// <summary>
			/// Cached name for the 'UpdateTimelineButtonBehavior' method.
			/// </summary>
			// Token: 0x0400500E RID: 20494
			public static readonly StringName UpdateTimelineButtonBehavior = "UpdateTimelineButtonBehavior";

			/// <summary>
			/// Cached name for the 'OnSubmenuStackChanged' method.
			/// </summary>
			// Token: 0x0400500F RID: 20495
			public static readonly StringName OnSubmenuStackChanged = "OnSubmenuStackChanged";

			/// <summary>
			/// Cached name for the 'OnContinueButtonPressed' method.
			/// </summary>
			// Token: 0x04005010 RID: 20496
			public static readonly StringName OnContinueButtonPressed = "OnContinueButtonPressed";

			/// <summary>
			/// Cached name for the 'DisplayLoadSaveError' method.
			/// </summary>
			// Token: 0x04005011 RID: 20497
			public static readonly StringName DisplayLoadSaveError = "DisplayLoadSaveError";

			/// <summary>
			/// Cached name for the 'OnAbandonRunButtonPressed' method.
			/// </summary>
			// Token: 0x04005012 RID: 20498
			public static readonly StringName OnAbandonRunButtonPressed = "OnAbandonRunButtonPressed";

			/// <summary>
			/// Cached name for the 'AbandonRun' method.
			/// </summary>
			// Token: 0x04005013 RID: 20499
			public static readonly StringName AbandonRun = "AbandonRun";

			/// <summary>
			/// Cached name for the 'SingleplayerButtonPressed' method.
			/// </summary>
			// Token: 0x04005014 RID: 20500
			public static readonly StringName SingleplayerButtonPressed = "SingleplayerButtonPressed";

			/// <summary>
			/// Cached name for the 'OpenSingleplayerSubmenu' method.
			/// </summary>
			// Token: 0x04005015 RID: 20501
			public static readonly StringName OpenSingleplayerSubmenu = "OpenSingleplayerSubmenu";

			/// <summary>
			/// Cached name for the 'OpenMultiplayerSubmenu' method.
			/// </summary>
			// Token: 0x04005016 RID: 20502
			public static readonly StringName OpenMultiplayerSubmenu = "OpenMultiplayerSubmenu";

			/// <summary>
			/// Cached name for the 'OpenCompendiumSubmenu' method.
			/// </summary>
			// Token: 0x04005017 RID: 20503
			public static readonly StringName OpenCompendiumSubmenu = "OpenCompendiumSubmenu";

			/// <summary>
			/// Cached name for the 'OpenTimelineScreen' method.
			/// </summary>
			// Token: 0x04005018 RID: 20504
			public static readonly StringName OpenTimelineScreen = "OpenTimelineScreen";

			/// <summary>
			/// Cached name for the 'OpenSettingsMenu' method.
			/// </summary>
			// Token: 0x04005019 RID: 20505
			public static readonly StringName OpenSettingsMenu = "OpenSettingsMenu";

			/// <summary>
			/// Cached name for the 'OpenProfileScreen' method.
			/// </summary>
			// Token: 0x0400501A RID: 20506
			public static readonly StringName OpenProfileScreen = "OpenProfileScreen";

			/// <summary>
			/// Cached name for the 'OpenPatchNotes' method.
			/// </summary>
			// Token: 0x0400501B RID: 20507
			public static readonly StringName OpenPatchNotes = "OpenPatchNotes";

			/// <summary>
			/// Cached name for the 'Quit' method.
			/// </summary>
			// Token: 0x0400501C RID: 20508
			public static readonly StringName Quit = "Quit";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x0400501D RID: 20509
			public static readonly StringName OnWindowChange = "OnWindowChange";

			/// <summary>
			/// Cached name for the 'CheckCommandLineArgs' method.
			/// </summary>
			// Token: 0x0400501E RID: 20510
			public static readonly StringName CheckCommandLineArgs = "CheckCommandLineArgs";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001504 RID: 5380
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'PatchNotesScreen' property.
			/// </summary>
			// Token: 0x0400501F RID: 20511
			public static readonly StringName PatchNotesScreen = "PatchNotesScreen";

			/// <summary>
			/// Cached name for the 'BlurBackstop' property.
			/// </summary>
			// Token: 0x04005020 RID: 20512
			public static readonly StringName BlurBackstop = "BlurBackstop";

			/// <summary>
			/// Cached name for the 'MainMenuButtons' property.
			/// </summary>
			// Token: 0x04005021 RID: 20513
			public static readonly StringName MainMenuButtons = "MainMenuButtons";

			/// <summary>
			/// Cached name for the 'SubmenuStack' property.
			/// </summary>
			// Token: 0x04005022 RID: 20514
			public static readonly StringName SubmenuStack = "SubmenuStack";

			/// <summary>
			/// Cached name for the 'ContinueRunInfo' property.
			/// </summary>
			// Token: 0x04005023 RID: 20515
			public static readonly StringName ContinueRunInfo = "ContinueRunInfo";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005024 RID: 20516
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_window' field.
			/// </summary>
			// Token: 0x04005025 RID: 20517
			public static readonly StringName _window = "_window";

			/// <summary>
			/// Cached name for the '_continueButton' field.
			/// </summary>
			// Token: 0x04005026 RID: 20518
			public static readonly StringName _continueButton = "_continueButton";

			/// <summary>
			/// Cached name for the '_abandonRunButton' field.
			/// </summary>
			// Token: 0x04005027 RID: 20519
			public static readonly StringName _abandonRunButton = "_abandonRunButton";

			/// <summary>
			/// Cached name for the '_singleplayerButton' field.
			/// </summary>
			// Token: 0x04005028 RID: 20520
			public static readonly StringName _singleplayerButton = "_singleplayerButton";

			/// <summary>
			/// Cached name for the '_compendiumButton' field.
			/// </summary>
			// Token: 0x04005029 RID: 20521
			public static readonly StringName _compendiumButton = "_compendiumButton";

			/// <summary>
			/// Cached name for the '_timelineButton' field.
			/// </summary>
			// Token: 0x0400502A RID: 20522
			public static readonly StringName _timelineButton = "_timelineButton";

			/// <summary>
			/// Cached name for the '_settingsButton' field.
			/// </summary>
			// Token: 0x0400502B RID: 20523
			public static readonly StringName _settingsButton = "_settingsButton";

			/// <summary>
			/// Cached name for the '_quitButton' field.
			/// </summary>
			// Token: 0x0400502C RID: 20524
			public static readonly StringName _quitButton = "_quitButton";

			/// <summary>
			/// Cached name for the '_multiplayerButton' field.
			/// </summary>
			// Token: 0x0400502D RID: 20525
			public static readonly StringName _multiplayerButton = "_multiplayerButton";

			/// <summary>
			/// Cached name for the '_buttonReticleLeft' field.
			/// </summary>
			// Token: 0x0400502E RID: 20526
			public static readonly StringName _buttonReticleLeft = "_buttonReticleLeft";

			/// <summary>
			/// Cached name for the '_buttonReticleRight' field.
			/// </summary>
			// Token: 0x0400502F RID: 20527
			public static readonly StringName _buttonReticleRight = "_buttonReticleRight";

			/// <summary>
			/// Cached name for the '_reticleTween' field.
			/// </summary>
			// Token: 0x04005030 RID: 20528
			public static readonly StringName _reticleTween = "_reticleTween";

			/// <summary>
			/// Cached name for the '_patchNotesButtonNode' field.
			/// </summary>
			// Token: 0x04005031 RID: 20529
			public static readonly StringName _patchNotesButtonNode = "_patchNotesButtonNode";

			/// <summary>
			/// Cached name for the '_openProfileScreenButton' field.
			/// </summary>
			// Token: 0x04005032 RID: 20530
			public static readonly StringName _openProfileScreenButton = "_openProfileScreenButton";

			/// <summary>
			/// Cached name for the '_lastHitButton' field.
			/// </summary>
			// Token: 0x04005033 RID: 20531
			public static readonly StringName _lastHitButton = "_lastHitButton";

			/// <summary>
			/// Cached name for the '_runInfo' field.
			/// </summary>
			// Token: 0x04005034 RID: 20532
			public static readonly StringName _runInfo = "_runInfo";

			/// <summary>
			/// Cached name for the '_timelineNotificationDot' field.
			/// </summary>
			// Token: 0x04005035 RID: 20533
			public static readonly StringName _timelineNotificationDot = "_timelineNotificationDot";

			/// <summary>
			/// Cached name for the '_backstopTween' field.
			/// </summary>
			// Token: 0x04005036 RID: 20534
			public static readonly StringName _backstopTween = "_backstopTween";

			/// <summary>
			/// Cached name for the '_bg' field.
			/// </summary>
			// Token: 0x04005037 RID: 20535
			public static readonly StringName _bg = "_bg";

			/// <summary>
			/// Cached name for the '_blur' field.
			/// </summary>
			// Token: 0x04005038 RID: 20536
			public static readonly StringName _blur = "_blur";

			/// <summary>
			/// Cached name for the '_openTimeline' field.
			/// </summary>
			// Token: 0x04005039 RID: 20537
			public static readonly StringName _openTimeline = "_openTimeline";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001505 RID: 5381
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}

		// Token: 0x02001506 RID: 5382
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400503A RID: 20538
			[Nullable(new byte[] { 0, 1 })]
			public static Action<NButton> <0>__Quit;
		}
	}
}
