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
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Rooms
{
	/// <summary>
	/// Manages the UI flow for a rest site.
	/// </summary>
	// Token: 0x0200036F RID: 879
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Rooms/NRestSiteRoom.cs")]
	public class NRestSiteRoom : Control, IScreenContext, IRoomWithProceedButton
	{
		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06003382 RID: 13186 RVA: 0x0017F49B File Offset: 0x0017D69B
		[Nullable(2)]
		public static NRestSiteRoom Instance
		{
			[NullableContext(2)]
			get
			{
				NRun instance = NRun.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.RestSiteRoom;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06003383 RID: 13187 RVA: 0x0017F4AD File Offset: 0x0017D6AD
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/rooms/rest_site_room.tscn");
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06003384 RID: 13188 RVA: 0x0017F4B9 File Offset: 0x0017D6B9
		public NProceedButton ProceedButton
		{
			get
			{
				return this._proceedButton;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06003385 RID: 13189 RVA: 0x0017F4C1 File Offset: 0x0017D6C1
		// (set) Token: 0x06003386 RID: 13190 RVA: 0x0017F4C9 File Offset: 0x0017D6C9
		private MegaLabel Header { get; set; }

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06003387 RID: 13191 RVA: 0x0017F4D2 File Offset: 0x0017D6D2
		// (set) Token: 0x06003388 RID: 13192 RVA: 0x0017F4DA File Offset: 0x0017D6DA
		private MegaRichTextLabel Description { get; set; }

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06003389 RID: 13193 RVA: 0x0017F4E3 File Offset: 0x0017D6E3
		// (set) Token: 0x0600338A RID: 13194 RVA: 0x0017F4EB File Offset: 0x0017D6EB
		private Control BgContainer { get; set; }

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x0600338B RID: 13195 RVA: 0x0017F4F4 File Offset: 0x0017D6F4
		public IReadOnlyList<RestSiteOption> Options
		{
			get
			{
				return this._room.Options;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x0600338C RID: 13196 RVA: 0x0017F501 File Offset: 0x0017D701
		public List<NRestSiteCharacter> Characters { get; } = new List<NRestSiteCharacter>();

		// Token: 0x0600338D RID: 13197 RVA: 0x0017F50C File Offset: 0x0017D70C
		[return: Nullable(2)]
		public static NRestSiteRoom Create(RestSiteRoom room, IRunState runState)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NRestSiteRoom nrestSiteRoom = PreloadManager.Cache.GetScene("res://scenes/rooms/rest_site_room.tscn").Instantiate<NRestSiteRoom>(0L);
			nrestSiteRoom._room = room;
			nrestSiteRoom._runState = runState;
			return nrestSiteRoom;
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x0017F548 File Offset: 0x0017D748
		public override void _Ready()
		{
			this.Header = base.GetNode<MegaLabel>("%Header");
			this.Description = base.GetNode<MegaRichTextLabel>("%Description");
			this.Description.Modulate = Colors.Transparent;
			this._choicesContainer = base.GetNode<Control>("%ChoicesContainer");
			this._choicesScreen = base.GetNode<Control>("%ChoicesScreen");
			this._proceedButton = base.GetNode<NProceedButton>("%ProceedButton");
			this.BgContainer = base.GetNode<Control>("BgContainer");
			this._characterContainers.Add(base.GetNode<Control>("BgContainer/Character_1"));
			this._characterContainers.Add(base.GetNode<Control>("BgContainer/Character_2"));
			this._characterContainers.Add(base.GetNode<Control>("BgContainer/Character_3"));
			this._characterContainers.Add(base.GetNode<Control>("BgContainer/Character_4"));
			Control control = this._runState.Act.CreateRestSiteBackground();
			this.BgContainer.AddChildSafely(control);
			this.BgContainer.MoveChildSafely(control, 0);
			this._restSiteLighting = control.GetNode<Control>("%RestSiteLighting");
			this.Header.SetTextAutoSize(new LocString("rest_site_ui", "PROMPT").GetFormattedText());
			this._proceedButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnProceedButtonReleased)), 0U);
			this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
			if (NRestSiteRoom._isDebugUiVisible)
			{
				this._choicesScreen.Modulate = Colors.Transparent;
			}
			NGame.Instance.SetScreenShakeTarget(this);
			this._proceedButton.Disable();
			for (int i = 0; i < this._runState.Players.Count; i++)
			{
				NRestSiteCharacter nrestSiteCharacter = NRestSiteCharacter.Create(this._runState.Players[i], i);
				this.characterAnims.Add(nrestSiteCharacter);
				this._characterContainers[i].AddChildSafely(nrestSiteCharacter);
				nrestSiteCharacter.Position = Vector2.Zero;
				if (i % 2 == 1)
				{
					nrestSiteCharacter.FlipX();
				}
				this.Characters.Add(nrestSiteCharacter);
			}
			this._originalDescriptionYPos = this.Description.Position.Y;
			this.UpdateRestSiteOptions();
			TaskHelper.RunSafely(this.ShowFtueIfNeeded());
			RunManager.Instance.RestSiteSynchronizer.PlayerHoverChanged += this.OnPlayerChangedHoveredRestSiteOption;
			RunManager.Instance.RestSiteSynchronizer.BeforePlayerOptionChosen += this.OnBeforePlayerSelectedRestSiteOption;
			RunManager.Instance.RestSiteSynchronizer.AfterPlayerOptionChosen += this.OnAfterPlayerSelectedRestSiteOption;
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x0017F801 File Offset: 0x0017DA01
		public override void _EnterTree()
		{
			ActiveScreenContext.Instance.Updated += this.OnActiveScreenUpdated;
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x0017F81C File Offset: 0x0017DA1C
		public override void _ExitTree()
		{
			this._cts.Cancel();
			Tween descriptionTween = this._descriptionTween;
			if (descriptionTween != null)
			{
				descriptionTween.Kill();
			}
			Tween descriptionPositionTween = this._descriptionPositionTween;
			if (descriptionPositionTween != null)
			{
				descriptionPositionTween.Kill();
			}
			Tween choicesTween = this._choicesTween;
			if (choicesTween != null)
			{
				choicesTween.Kill();
			}
			RunManager.Instance.RestSiteSynchronizer.PlayerHoverChanged -= this.OnPlayerChangedHoveredRestSiteOption;
			RunManager.Instance.RestSiteSynchronizer.BeforePlayerOptionChosen -= this.OnBeforePlayerSelectedRestSiteOption;
			RunManager.Instance.RestSiteSynchronizer.AfterPlayerOptionChosen -= this.OnAfterPlayerSelectedRestSiteOption;
			ActiveScreenContext.Instance.Updated -= this.OnActiveScreenUpdated;
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x0017F8CE File Offset: 0x0017DACE
		public void AfterSelectingOption(RestSiteOption option)
		{
			TaskHelper.RunSafely(this.AfterSelectingOptionAsync(option));
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x0017F8E0 File Offset: 0x0017DAE0
		private async Task ShowFtueIfNeeded()
		{
			if (!SaveManager.Instance.SeenFtue("rest_site_ftue"))
			{
				this._choicesContainer.Visible = false;
				this.Header.Visible = false;
				await Cmd.Wait(0.5f, this._cts.Token, false);
				this._choicesContainer.Visible = true;
				this.Header.Visible = true;
				CanvasItem choicesContainer = this._choicesContainer;
				Color color = this._choicesContainer.Modulate;
				color.A = 0f;
				choicesContainer.Modulate = color;
				CanvasItem header = this.Header;
				color = this.Header.Modulate;
				color.A = 0f;
				header.Modulate = color;
				Tween tween = base.CreateTween();
				tween.TweenProperty(this._choicesContainer, "modulate:a", 1f, 0.5);
				tween.TweenProperty(this.Header, "modulate:a", 1f, 0.5);
				NModalContainer.Instance.Add(NRestSiteFtue.Create(this._choicesContainer), true);
				SaveManager.Instance.MarkFtueAsComplete("rest_site_ftue");
			}
		}

		/// <summary>
		/// Called before the room exit begins to disable all buttons.
		/// </summary>
		// Token: 0x06003393 RID: 13203 RVA: 0x0017F923 File Offset: 0x0017DB23
		public void BeforeExitingRoom()
		{
			this._roomExiting = true;
			this.DisableOptions();
		}

		/// <summary>
		/// Grays out options so that they can't be selected while an option is being executed.
		/// </summary>
		// Token: 0x06003394 RID: 13204 RVA: 0x0017F934 File Offset: 0x0017DB34
		public void DisableOptions()
		{
			foreach (NRestSiteButton nrestSiteButton in this._choicesContainer.GetChildren(false).OfType<NRestSiteButton>())
			{
				nrestSiteButton.Disable();
			}
		}

		/// <summary>
		/// Re-enables options so that they can be selected after an option is executed.
		/// This is if a relic allows you to choose multiple options.
		/// </summary>
		// Token: 0x06003395 RID: 13205 RVA: 0x0017F98C File Offset: 0x0017DB8C
		public void EnableOptions()
		{
			if (this._roomExiting)
			{
				return;
			}
			foreach (NRestSiteButton nrestSiteButton in this._choicesContainer.GetChildren(false).OfType<NRestSiteButton>())
			{
				nrestSiteButton.Enable();
			}
			Control defaultFocusedControl = this.DefaultFocusedControl;
			if (defaultFocusedControl == null)
			{
				return;
			}
			defaultFocusedControl.TryGrabFocus();
		}

		/// <summary>
		/// Animates the description text down below the characters.
		/// If a player is doing the mend target selection, the arrow gets in the way of the text.
		/// </summary>
		// Token: 0x06003396 RID: 13206 RVA: 0x0017F9FC File Offset: 0x0017DBFC
		public void AnimateDescriptionDown()
		{
			Tween descriptionPositionTween = this._descriptionPositionTween;
			if (descriptionPositionTween != null)
			{
				descriptionPositionTween.Kill();
			}
			this._descriptionPositionTween = base.CreateTween();
			this._descriptionPositionTween.TweenProperty(this.Description, "position:y", 885f, 0.800000011920929).SetTrans(5L).SetEase(1L);
		}

		/// <summary>
		/// Animates the description text back up to its original position.
		/// If a player is doing the mend target selection, the arrow gets in the way of the text.
		/// </summary>
		// Token: 0x06003397 RID: 13207 RVA: 0x0017FA64 File Offset: 0x0017DC64
		public void AnimateDescriptionUp()
		{
			Tween descriptionPositionTween = this._descriptionPositionTween;
			if (descriptionPositionTween != null)
			{
				descriptionPositionTween.Kill();
			}
			this._descriptionPositionTween = base.CreateTween();
			this._descriptionPositionTween.TweenProperty(this.Description, "position:y", this._originalDescriptionYPos, 0.800000011920929).SetTrans(5L).SetEase(1L);
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x0017FACC File Offset: 0x0017DCCC
		private void UpdateRestSiteOptions()
		{
			if (!this.IsValid() || !base.IsInsideTree())
			{
				return;
			}
			foreach (Node node in this._choicesContainer.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
			List<NRestSiteButton> list = new List<NRestSiteButton>();
			foreach (RestSiteOption restSiteOption in this.Options)
			{
				NRestSiteButton nrestSiteButton = NRestSiteButton.Create(restSiteOption);
				this._choicesContainer.AddChildSafely(nrestSiteButton);
				nrestSiteButton.Connect(NClickableControl.SignalName.Focused, Callable.From<NRestSiteButton>(new Action<NRestSiteButton>(this.RestSiteButtonHovered)), 0U);
				nrestSiteButton.Connect(NClickableControl.SignalName.Unfocused, Callable.From<NRestSiteButton>(new Action<NRestSiteButton>(this.RestSiteButtonUnhovered)), 0U);
				list.Add(nrestSiteButton);
			}
			for (int i = 0; i < list.Count; i++)
			{
				NRestSiteButton nrestSiteButton2 = list[i];
				Control control = nrestSiteButton2;
				NodePath nodePath;
				if (i <= 0)
				{
					List<NRestSiteButton> list2 = list;
					nodePath = list2[list2.Count - 1].GetPath();
				}
				else
				{
					nodePath = list[i - 1].GetPath();
				}
				control.FocusNeighborLeft = nodePath;
				nrestSiteButton2.FocusNeighborRight = ((i < list.Count - 1) ? list[i + 1].GetPath() : list[0].GetPath());
				nrestSiteButton2.FocusNeighborTop = nrestSiteButton2.GetPath();
				nrestSiteButton2.FocusNeighborBottom = nrestSiteButton2.GetPath();
			}
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x0017FC68 File Offset: 0x0017DE68
		private void RestSiteButtonHovered(NRestSiteButton button)
		{
			RunManager.Instance.RestSiteSynchronizer.LocalOptionHovered(button.Option);
			this._lastFocused = button;
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x0017FC86 File Offset: 0x0017DE86
		private void RestSiteButtonUnhovered(NRestSiteButton button)
		{
			RunManager.Instance.RestSiteSynchronizer.LocalOptionHovered(null);
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x0017FC98 File Offset: 0x0017DE98
		private void OnPlayerChangedHoveredRestSiteOption(ulong playerId)
		{
			if (this._runState.Players.Count <= 1)
			{
				return;
			}
			NRestSiteCharacter nrestSiteCharacter = this.Characters.First<NRestSiteCharacter>((NRestSiteCharacter c) => c.Player.NetId == playerId);
			int? hoveredOptionIndex = RunManager.Instance.RestSiteSynchronizer.GetHoveredOptionIndex(playerId);
			RestSiteOption restSiteOption = ((hoveredOptionIndex == null) ? null : RunManager.Instance.RestSiteSynchronizer.GetOptionsForPlayer(playerId)[hoveredOptionIndex.Value]);
			nrestSiteCharacter.ShowHoveredRestSiteOption(restSiteOption);
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x0017FD2C File Offset: 0x0017DF2C
		private void OnBeforePlayerSelectedRestSiteOption(RestSiteOption option, ulong playerId)
		{
			if (this._runState.Players.Count <= 1)
			{
				return;
			}
			NRestSiteCharacter nrestSiteCharacter = this.Characters.First<NRestSiteCharacter>((NRestSiteCharacter c) => c.Player.NetId == playerId);
			nrestSiteCharacter.SetSelectingRestSiteOption(option);
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x0017FD7C File Offset: 0x0017DF7C
		private void OnAfterPlayerSelectedRestSiteOption(RestSiteOption option, bool success, ulong playerId)
		{
			if (this._runState.Players.Count <= 1)
			{
				return;
			}
			NRestSiteCharacter nrestSiteCharacter = this.Characters.First<NRestSiteCharacter>((NRestSiteCharacter c) => c.Player.NetId == playerId);
			nrestSiteCharacter.SetSelectingRestSiteOption(null);
			if (success)
			{
				nrestSiteCharacter.ShowSelectedRestSiteOption(option);
				if (!LocalContext.IsMe(nrestSiteCharacter.Player))
				{
					TaskHelper.RunSafely(option.DoRemotePostSelectVfx());
				}
			}
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x0017FDEC File Offset: 0x0017DFEC
		[return: Nullable(2)]
		public NRestSiteButton GetButtonForOption(RestSiteOption option)
		{
			foreach (NRestSiteButton nrestSiteButton in this._choicesContainer.GetChildren(false).OfType<NRestSiteButton>())
			{
				if (nrestSiteButton.Option == option)
				{
					return nrestSiteButton;
				}
			}
			return null;
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x0017FE54 File Offset: 0x0017E054
		[return: Nullable(2)]
		public NRestSiteCharacter GetCharacterForPlayer(Player player)
		{
			foreach (NRestSiteCharacter nrestSiteCharacter in this.characterAnims)
			{
				if (nrestSiteCharacter.Player == player)
				{
					return nrestSiteCharacter;
				}
			}
			return null;
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x0017FEB0 File Offset: 0x0017E0B0
		private unsafe async Task AfterSelectingOptionAsync(RestSiteOption option)
		{
			Task task = this.HideChoices(this._cts.Token);
			Task task2 = option.DoLocalPostSelectVfx(this._cts.Token);
			this.ExtinguishFireIfAble();
			<>y__InlineArray2<Task> <>y__InlineArray = default(<>y__InlineArray2<Task>);
			*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray2<Task>, Task>(ref <>y__InlineArray, 0) = task;
			*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray2<Task>, Task>(ref <>y__InlineArray, 1) = task2;
			await Task.WhenAll(<PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<<>y__InlineArray2<Task>, Task>(in <>y__InlineArray, 2));
			this.UpdateRestSiteOptions();
			this.ShowProceedButton();
			if (this.Options.Count > 0)
			{
				await this.ShowChoices(this._cts.Token);
				Control defaultFocusedControl = this.DefaultFocusedControl;
				if (defaultFocusedControl != null)
				{
					defaultFocusedControl.TryGrabFocus();
				}
			}
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x0017FEFB File Offset: 0x0017E0FB
		private void ShowProceedButton()
		{
			if (this._proceedButton.IsEnabled)
			{
				return;
			}
			this._proceedButton.Enable();
			NMapScreen.Instance.SetTravelEnabled(true);
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x0017FF21 File Offset: 0x0017E121
		private void OnProceedButtonReleased(NButton _)
		{
			NMapScreen.Instance.Open(false);
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x0017FF2F File Offset: 0x0017E12F
		public void SetText(string formattedText)
		{
			Tween descriptionTween = this._descriptionTween;
			if (descriptionTween != null)
			{
				descriptionTween.Kill();
			}
			this.Description.Modulate = Colors.White;
			this.Description.Text = formattedText;
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x0017FF60 File Offset: 0x0017E160
		public void FadeOutOptionDescription()
		{
			Tween descriptionTween = this._descriptionTween;
			if (descriptionTween != null)
			{
				descriptionTween.Kill();
			}
			this._descriptionTween = base.CreateTween();
			this._descriptionTween.TweenProperty(this.Description, "modulate:a", 0f, 1.0).SetEase(1L).SetTrans(5L)
				.From(1f);
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x0017FFD8 File Offset: 0x0017E1D8
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionReleased(DebugHotkey.hideRestSite, false))
			{
				NRestSiteRoom._isDebugUiVisible = !NRestSiteRoom._isDebugUiVisible;
				this._choicesScreen.Modulate = (NRestSiteRoom._isDebugUiVisible ? Colors.Transparent : Colors.White);
				NGame.Instance.AddChildSafely(NFullscreenTextVfx.Create(NRestSiteRoom._isDebugUiVisible ? "Hide RestSite UI" : "Show RestSite UI"));
			}
		}

		/// <summary>
		/// Disables the fire vfx in the rest site if there are no more options remaining.
		/// </summary>
		// Token: 0x060033A6 RID: 13222 RVA: 0x00180040 File Offset: 0x0017E240
		private void ExtinguishFireIfAble()
		{
			if (RunManager.Instance.RestSiteSynchronizer.GetLocalOptions().Count > 0)
			{
				return;
			}
			if (!this._restSiteLighting.Visible)
			{
				return;
			}
			foreach (NRestSiteCharacter nrestSiteCharacter in this.characterAnims)
			{
				nrestSiteCharacter.HideFlameGlow();
			}
			foreach (Control control in this._characterContainers)
			{
				control.Modulate = Colors.DarkGray;
			}
			this._restSiteLighting.Visible = false;
			if (!this._runState.IsGameOver)
			{
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance == null)
				{
					return;
				}
				instance.TriggerCampfireGoingOut();
			}
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x00180128 File Offset: 0x0017E328
		private async Task ShowChoices(CancellationToken ct)
		{
			Tween choicesTween = this._choicesTween;
			if (choicesTween != null)
			{
				choicesTween.Kill();
			}
			this._choicesTween = base.CreateTween();
			this._choicesTween.TweenProperty(this._choicesScreen, "modulate:a", 1f, 0.5).SetEase(1L).SetTrans(7L);
			await this._choicesTween.AwaitFinished(ct);
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x00180174 File Offset: 0x0017E374
		private async Task HideChoices(CancellationToken ct)
		{
			foreach (NButton nbutton in this._choicesContainer.GetChildren(false).OfType<NButton>())
			{
				nbutton.Disable();
			}
			Tween choicesTween = this._choicesTween;
			if (choicesTween != null)
			{
				choicesTween.Kill();
			}
			this._choicesTween = base.CreateTween();
			this._choicesTween.TweenProperty(this._choicesScreen, "modulate:a", 0f, 0.5);
			this._lastFocused = null;
			await this._choicesTween.AwaitFinished(ct);
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060033A9 RID: 13225 RVA: 0x001801BF File Offset: 0x0017E3BF
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._lastFocused != null)
				{
					return this._lastFocused;
				}
				if (this._choicesContainer.GetChildCount(false) <= 0)
				{
					return null;
				}
				return this._choicesContainer.GetChild<NRestSiteButton>(0, false);
			}
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x001801F0 File Offset: 0x0017E3F0
		private void OnActiveScreenUpdated()
		{
			this.UpdateControllerNavEnabled<NRestSiteRoom>();
			if (ActiveScreenContext.Instance.IsCurrent(this) && this.Options.Count == 0)
			{
				this.ShowProceedButton();
				return;
			}
			if (!this._proceedButton.IsEnabled)
			{
				this._proceedButton.Disable();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060033AB RID: 13227 RVA: 0x0018023C File Offset: 0x0017E43C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(19)
			{
				new MethodInfo(NRestSiteRoom.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName.BeforeExitingRoom, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName.DisableOptions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName.EnableOptions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName.AnimateDescriptionDown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName.AnimateDescriptionUp, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName.UpdateRestSiteOptions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName.RestSiteButtonHovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRestSiteRoom.MethodName.RestSiteButtonUnhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRestSiteRoom.MethodName.OnPlayerChangedHoveredRestSiteOption, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRestSiteRoom.MethodName.ShowProceedButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName.OnProceedButtonReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRestSiteRoom.MethodName.SetText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "formattedText", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRestSiteRoom.MethodName.FadeOutOptionDescription, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NRestSiteRoom.MethodName.ExtinguishFireIfAble, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSiteRoom.MethodName.OnActiveScreenUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060033AC RID: 13228 RVA: 0x001806DC File Offset: 0x0017E8DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRestSiteRoom.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.BeforeExitingRoom && args.Count == 0)
			{
				this.BeforeExitingRoom();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.DisableOptions && args.Count == 0)
			{
				this.DisableOptions();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.EnableOptions && args.Count == 0)
			{
				this.EnableOptions();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.AnimateDescriptionDown && args.Count == 0)
			{
				this.AnimateDescriptionDown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.AnimateDescriptionUp && args.Count == 0)
			{
				this.AnimateDescriptionUp();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.UpdateRestSiteOptions && args.Count == 0)
			{
				this.UpdateRestSiteOptions();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.RestSiteButtonHovered && args.Count == 1)
			{
				this.RestSiteButtonHovered(VariantUtils.ConvertTo<NRestSiteButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.RestSiteButtonUnhovered && args.Count == 1)
			{
				this.RestSiteButtonUnhovered(VariantUtils.ConvertTo<NRestSiteButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.OnPlayerChangedHoveredRestSiteOption && args.Count == 1)
			{
				this.OnPlayerChangedHoveredRestSiteOption(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.ShowProceedButton && args.Count == 0)
			{
				this.ShowProceedButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.OnProceedButtonReleased && args.Count == 1)
			{
				this.OnProceedButtonReleased(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.SetText && args.Count == 1)
			{
				this.SetText(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.FadeOutOptionDescription && args.Count == 0)
			{
				this.FadeOutOptionDescription();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.ExtinguishFireIfAble && args.Count == 0)
			{
				this.ExtinguishFireIfAble();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSiteRoom.MethodName.OnActiveScreenUpdated && args.Count == 0)
			{
				this.OnActiveScreenUpdated();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060033AD RID: 13229 RVA: 0x00180A08 File Offset: 0x0017EC08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRestSiteRoom.MethodName._Ready || (ref method) == NRestSiteRoom.MethodName._EnterTree || (ref method) == NRestSiteRoom.MethodName._ExitTree || (ref method) == NRestSiteRoom.MethodName.BeforeExitingRoom || (ref method) == NRestSiteRoom.MethodName.DisableOptions || (ref method) == NRestSiteRoom.MethodName.EnableOptions || (ref method) == NRestSiteRoom.MethodName.AnimateDescriptionDown || (ref method) == NRestSiteRoom.MethodName.AnimateDescriptionUp || (ref method) == NRestSiteRoom.MethodName.UpdateRestSiteOptions || (ref method) == NRestSiteRoom.MethodName.RestSiteButtonHovered || (ref method) == NRestSiteRoom.MethodName.RestSiteButtonUnhovered || (ref method) == NRestSiteRoom.MethodName.OnPlayerChangedHoveredRestSiteOption || (ref method) == NRestSiteRoom.MethodName.ShowProceedButton || (ref method) == NRestSiteRoom.MethodName.OnProceedButtonReleased || (ref method) == NRestSiteRoom.MethodName.SetText || (ref method) == NRestSiteRoom.MethodName.FadeOutOptionDescription || (ref method) == NRestSiteRoom.MethodName._Input || (ref method) == NRestSiteRoom.MethodName.ExtinguishFireIfAble || (ref method) == NRestSiteRoom.MethodName.OnActiveScreenUpdated || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060033AE RID: 13230 RVA: 0x00180B3C File Offset: 0x0017ED3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRestSiteRoom.PropertyName.Header)
			{
				this.Header = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName.Description)
			{
				this.Description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName.BgContainer)
			{
				this.BgContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._choicesContainer)
			{
				this._choicesContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._choicesScreen)
			{
				this._choicesScreen = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._proceedButton)
			{
				this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._restSiteLighting)
			{
				this._restSiteLighting = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._descriptionTween)
			{
				this._descriptionTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._descriptionPositionTween)
			{
				this._descriptionPositionTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._choicesTween)
			{
				this._choicesTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._originalDescriptionYPos)
			{
				this._originalDescriptionYPos = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._roomExiting)
			{
				this._roomExiting = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._lastFocused)
			{
				this._lastFocused = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060033AF RID: 13231 RVA: 0x00180CB0 File Offset: 0x0017EEB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRestSiteRoom.PropertyName.ProceedButton)
			{
				NProceedButton proceedButton = this.ProceedButton;
				value = VariantUtils.CreateFrom<NProceedButton>(ref proceedButton);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName.Header)
			{
				MegaLabel header = this.Header;
				value = VariantUtils.CreateFrom<MegaLabel>(ref header);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName.Description)
			{
				MegaRichTextLabel description = this.Description;
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref description);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName.BgContainer)
			{
				Control control = this.BgContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._choicesContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._choicesContainer);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._choicesScreen)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._choicesScreen);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._proceedButton)
			{
				value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._restSiteLighting)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._restSiteLighting);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._descriptionTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._descriptionTween);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._descriptionPositionTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._descriptionPositionTween);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._choicesTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._choicesTween);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._originalDescriptionYPos)
			{
				value = VariantUtils.CreateFrom<float>(ref this._originalDescriptionYPos);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._roomExiting)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._roomExiting);
				return true;
			}
			if ((ref name) == NRestSiteRoom.PropertyName._lastFocused)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._lastFocused);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060033B0 RID: 13232 RVA: 0x00180EB4 File Offset: 0x0017F0B4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRestSiteRoom.PropertyName._choicesContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName._choicesScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName._proceedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName.ProceedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName._restSiteLighting, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName._descriptionTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName._descriptionPositionTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName._choicesTween, 0L, "", 4096L, false),
				new PropertyInfo(3L, NRestSiteRoom.PropertyName._originalDescriptionYPos, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRestSiteRoom.PropertyName._roomExiting, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName._lastFocused, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName.Header, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName.Description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName.BgContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRestSiteRoom.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060033B1 RID: 13233 RVA: 0x001810B8 File Offset: 0x0017F2B8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName header = NRestSiteRoom.PropertyName.Header;
			MegaLabel header2 = this.Header;
			info.AddProperty(header, Variant.From<MegaLabel>(ref header2));
			StringName description = NRestSiteRoom.PropertyName.Description;
			MegaRichTextLabel description2 = this.Description;
			info.AddProperty(description, Variant.From<MegaRichTextLabel>(ref description2));
			StringName bgContainer = NRestSiteRoom.PropertyName.BgContainer;
			Control bgContainer2 = this.BgContainer;
			info.AddProperty(bgContainer, Variant.From<Control>(ref bgContainer2));
			info.AddProperty(NRestSiteRoom.PropertyName._choicesContainer, Variant.From<Control>(ref this._choicesContainer));
			info.AddProperty(NRestSiteRoom.PropertyName._choicesScreen, Variant.From<Control>(ref this._choicesScreen));
			info.AddProperty(NRestSiteRoom.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
			info.AddProperty(NRestSiteRoom.PropertyName._restSiteLighting, Variant.From<Control>(ref this._restSiteLighting));
			info.AddProperty(NRestSiteRoom.PropertyName._descriptionTween, Variant.From<Tween>(ref this._descriptionTween));
			info.AddProperty(NRestSiteRoom.PropertyName._descriptionPositionTween, Variant.From<Tween>(ref this._descriptionPositionTween));
			info.AddProperty(NRestSiteRoom.PropertyName._choicesTween, Variant.From<Tween>(ref this._choicesTween));
			info.AddProperty(NRestSiteRoom.PropertyName._originalDescriptionYPos, Variant.From<float>(ref this._originalDescriptionYPos));
			info.AddProperty(NRestSiteRoom.PropertyName._roomExiting, Variant.From<bool>(ref this._roomExiting));
			info.AddProperty(NRestSiteRoom.PropertyName._lastFocused, Variant.From<Control>(ref this._lastFocused));
		}

		/// <inheritdoc />
		// Token: 0x060033B2 RID: 13234 RVA: 0x001811F4 File Offset: 0x0017F3F4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName.Header, ref variant))
			{
				this.Header = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName.Description, ref variant2))
			{
				this.Description = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName.BgContainer, ref variant3))
			{
				this.BgContainer = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName._choicesContainer, ref variant4))
			{
				this._choicesContainer = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName._choicesScreen, ref variant5))
			{
				this._choicesScreen = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName._proceedButton, ref variant6))
			{
				this._proceedButton = variant6.As<NProceedButton>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName._restSiteLighting, ref variant7))
			{
				this._restSiteLighting = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName._descriptionTween, ref variant8))
			{
				this._descriptionTween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName._descriptionPositionTween, ref variant9))
			{
				this._descriptionPositionTween = variant9.As<Tween>();
			}
			Variant variant10;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName._choicesTween, ref variant10))
			{
				this._choicesTween = variant10.As<Tween>();
			}
			Variant variant11;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName._originalDescriptionYPos, ref variant11))
			{
				this._originalDescriptionYPos = variant11.As<float>();
			}
			Variant variant12;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName._roomExiting, ref variant12))
			{
				this._roomExiting = variant12.As<bool>();
			}
			Variant variant13;
			if (info.TryGetProperty(NRestSiteRoom.PropertyName._lastFocused, ref variant13))
			{
				this._lastFocused = variant13.As<Control>();
			}
		}

		// Token: 0x040013F3 RID: 5107
		public readonly List<NRestSiteCharacter> characterAnims = new List<NRestSiteCharacter>();

		// Token: 0x040013F4 RID: 5108
		private const float _lowDescriptionYPos = 885f;

		// Token: 0x040013F5 RID: 5109
		private const string _scenePath = "res://scenes/rooms/rest_site_room.tscn";

		// Token: 0x040013F6 RID: 5110
		private static bool _isDebugUiVisible;

		// Token: 0x040013F7 RID: 5111
		private RestSiteRoom _room;

		// Token: 0x040013F8 RID: 5112
		private IRunState _runState;

		// Token: 0x040013F9 RID: 5113
		private Control _choicesContainer;

		// Token: 0x040013FA RID: 5114
		private Control _choicesScreen;

		// Token: 0x040013FB RID: 5115
		private NProceedButton _proceedButton;

		// Token: 0x040013FC RID: 5116
		private Control _restSiteLighting;

		// Token: 0x040013FD RID: 5117
		private readonly List<Control> _characterContainers = new List<Control>();

		// Token: 0x040013FE RID: 5118
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x040013FF RID: 5119
		[Nullable(2)]
		private Tween _descriptionTween;

		// Token: 0x04001400 RID: 5120
		[Nullable(2)]
		private Tween _descriptionPositionTween;

		// Token: 0x04001401 RID: 5121
		[Nullable(2)]
		private Tween _choicesTween;

		// Token: 0x04001402 RID: 5122
		private float _originalDescriptionYPos;

		// Token: 0x04001403 RID: 5123
		private bool _roomExiting;

		// Token: 0x04001404 RID: 5124
		[Nullable(2)]
		private Control _lastFocused;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200169B RID: 5787
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005805 RID: 22533
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005806 RID: 22534
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005807 RID: 22535
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'BeforeExitingRoom' method.
			/// </summary>
			// Token: 0x04005808 RID: 22536
			public static readonly StringName BeforeExitingRoom = "BeforeExitingRoom";

			/// <summary>
			/// Cached name for the 'DisableOptions' method.
			/// </summary>
			// Token: 0x04005809 RID: 22537
			public static readonly StringName DisableOptions = "DisableOptions";

			/// <summary>
			/// Cached name for the 'EnableOptions' method.
			/// </summary>
			// Token: 0x0400580A RID: 22538
			public static readonly StringName EnableOptions = "EnableOptions";

			/// <summary>
			/// Cached name for the 'AnimateDescriptionDown' method.
			/// </summary>
			// Token: 0x0400580B RID: 22539
			public static readonly StringName AnimateDescriptionDown = "AnimateDescriptionDown";

			/// <summary>
			/// Cached name for the 'AnimateDescriptionUp' method.
			/// </summary>
			// Token: 0x0400580C RID: 22540
			public static readonly StringName AnimateDescriptionUp = "AnimateDescriptionUp";

			/// <summary>
			/// Cached name for the 'UpdateRestSiteOptions' method.
			/// </summary>
			// Token: 0x0400580D RID: 22541
			public static readonly StringName UpdateRestSiteOptions = "UpdateRestSiteOptions";

			/// <summary>
			/// Cached name for the 'RestSiteButtonHovered' method.
			/// </summary>
			// Token: 0x0400580E RID: 22542
			public static readonly StringName RestSiteButtonHovered = "RestSiteButtonHovered";

			/// <summary>
			/// Cached name for the 'RestSiteButtonUnhovered' method.
			/// </summary>
			// Token: 0x0400580F RID: 22543
			public static readonly StringName RestSiteButtonUnhovered = "RestSiteButtonUnhovered";

			/// <summary>
			/// Cached name for the 'OnPlayerChangedHoveredRestSiteOption' method.
			/// </summary>
			// Token: 0x04005810 RID: 22544
			public static readonly StringName OnPlayerChangedHoveredRestSiteOption = "OnPlayerChangedHoveredRestSiteOption";

			/// <summary>
			/// Cached name for the 'ShowProceedButton' method.
			/// </summary>
			// Token: 0x04005811 RID: 22545
			public static readonly StringName ShowProceedButton = "ShowProceedButton";

			/// <summary>
			/// Cached name for the 'OnProceedButtonReleased' method.
			/// </summary>
			// Token: 0x04005812 RID: 22546
			public static readonly StringName OnProceedButtonReleased = "OnProceedButtonReleased";

			/// <summary>
			/// Cached name for the 'SetText' method.
			/// </summary>
			// Token: 0x04005813 RID: 22547
			public static readonly StringName SetText = "SetText";

			/// <summary>
			/// Cached name for the 'FadeOutOptionDescription' method.
			/// </summary>
			// Token: 0x04005814 RID: 22548
			public static readonly StringName FadeOutOptionDescription = "FadeOutOptionDescription";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005815 RID: 22549
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'ExtinguishFireIfAble' method.
			/// </summary>
			// Token: 0x04005816 RID: 22550
			public static readonly StringName ExtinguishFireIfAble = "ExtinguishFireIfAble";

			/// <summary>
			/// Cached name for the 'OnActiveScreenUpdated' method.
			/// </summary>
			// Token: 0x04005817 RID: 22551
			public static readonly StringName OnActiveScreenUpdated = "OnActiveScreenUpdated";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200169C RID: 5788
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ProceedButton' property.
			/// </summary>
			// Token: 0x04005818 RID: 22552
			public static readonly StringName ProceedButton = "ProceedButton";

			/// <summary>
			/// Cached name for the 'Header' property.
			/// </summary>
			// Token: 0x04005819 RID: 22553
			public static readonly StringName Header = "Header";

			/// <summary>
			/// Cached name for the 'Description' property.
			/// </summary>
			// Token: 0x0400581A RID: 22554
			public static readonly StringName Description = "Description";

			/// <summary>
			/// Cached name for the 'BgContainer' property.
			/// </summary>
			// Token: 0x0400581B RID: 22555
			public static readonly StringName BgContainer = "BgContainer";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x0400581C RID: 22556
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_choicesContainer' field.
			/// </summary>
			// Token: 0x0400581D RID: 22557
			public static readonly StringName _choicesContainer = "_choicesContainer";

			/// <summary>
			/// Cached name for the '_choicesScreen' field.
			/// </summary>
			// Token: 0x0400581E RID: 22558
			public static readonly StringName _choicesScreen = "_choicesScreen";

			/// <summary>
			/// Cached name for the '_proceedButton' field.
			/// </summary>
			// Token: 0x0400581F RID: 22559
			public static readonly StringName _proceedButton = "_proceedButton";

			/// <summary>
			/// Cached name for the '_restSiteLighting' field.
			/// </summary>
			// Token: 0x04005820 RID: 22560
			public static readonly StringName _restSiteLighting = "_restSiteLighting";

			/// <summary>
			/// Cached name for the '_descriptionTween' field.
			/// </summary>
			// Token: 0x04005821 RID: 22561
			public static readonly StringName _descriptionTween = "_descriptionTween";

			/// <summary>
			/// Cached name for the '_descriptionPositionTween' field.
			/// </summary>
			// Token: 0x04005822 RID: 22562
			public static readonly StringName _descriptionPositionTween = "_descriptionPositionTween";

			/// <summary>
			/// Cached name for the '_choicesTween' field.
			/// </summary>
			// Token: 0x04005823 RID: 22563
			public static readonly StringName _choicesTween = "_choicesTween";

			/// <summary>
			/// Cached name for the '_originalDescriptionYPos' field.
			/// </summary>
			// Token: 0x04005824 RID: 22564
			public static readonly StringName _originalDescriptionYPos = "_originalDescriptionYPos";

			/// <summary>
			/// Cached name for the '_roomExiting' field.
			/// </summary>
			// Token: 0x04005825 RID: 22565
			public static readonly StringName _roomExiting = "_roomExiting";

			/// <summary>
			/// Cached name for the '_lastFocused' field.
			/// </summary>
			// Token: 0x04005826 RID: 22566
			public static readonly StringName _lastFocused = "_lastFocused";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200169D RID: 5789
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
