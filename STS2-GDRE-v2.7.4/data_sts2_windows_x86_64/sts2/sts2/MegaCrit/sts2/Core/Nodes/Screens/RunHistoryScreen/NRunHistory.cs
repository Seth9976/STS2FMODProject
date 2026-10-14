using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen
{
	/// <summary>
	/// Rename to NRunHistoryScreen someday.
	/// </summary>
	// Token: 0x020002D3 RID: 723
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NRunHistory.cs")]
	public class NRunHistory : NSubmenu
	{
		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002658 RID: 9816 RVA: 0x001032C0 File Offset: 0x001014C0
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002659 RID: 9817 RVA: 0x001032C3 File Offset: 0x001014C3
		public static string[] AssetPaths
		{
			get
			{
				return new string[] { NRunHistory._scenePath };
			}
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x001032D3 File Offset: 0x001014D3
		[NullableContext(2)]
		public static NRunHistory Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NRunHistory._scenePath).Instantiate<NRunHistory>(0L);
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x001032F4 File Offset: 0x001014F4
		public override void _Ready()
		{
			this.ConnectSignals();
			this._screenContents = base.GetNode<NScrollableContainer>("ScreenContents");
			this._playerIconContainer = base.GetNode<Control>("%PlayerIconContainer");
			this._hpLabel = base.GetNode<MegaLabel>("%HpLabel");
			this._goldLabel = base.GetNode<MegaLabel>("%GoldLabel");
			this._potionHolder = base.GetNode<Control>("%PotionHolders");
			this._floorLabel = base.GetNode<MegaLabel>("%FloorNumLabel");
			this._timeLabel = base.GetNode<MegaLabel>("%RunTimeLabel");
			this._dateLabel = base.GetNode<MegaRichTextLabel>("%DateLabel");
			this._seedLabel = base.GetNode<MegaRichTextLabel>("%SeedLabel");
			this._gameModeLabel = base.GetNode<MegaRichTextLabel>("%GameModeLabel");
			this._buildLabel = base.GetNode<MegaRichTextLabel>("%BuildLabel");
			this._deathQuoteLabel = base.GetNode<MegaRichTextLabel>("%DeathQuoteLabel");
			this._badgeContainer = base.GetNode<Control>("%BadgeContainer");
			this._mapPointHistory = base.GetNode<NMapPointHistory>("%MapPointHistory");
			this._relicHistory = base.GetNode<NRelicHistory>("%RelicHistory");
			this._deckHistory = base.GetNode<NDeckHistory>("%DeckHistory");
			this._outOfDateVisual = base.GetNode<Control>("%OutOfDateVisual");
			this._prevButton = base.GetNode<NRunHistoryArrowButton>("LeftArrow");
			this._prevButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnLeftButtonButtonReleased)), 0U);
			this._nextButton = base.GetNode<NRunHistoryArrowButton>("RightArrow");
			this._nextButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnRightButtonButtonReleased)), 0U);
			this._prevButton.IsLeft = true;
			this._mapPointHistory.SetDeckHistory(this._deckHistory);
			this._mapPointHistory.SetRelicHistory(this._relicHistory);
			this._screenContents.DisableScrollingIfContentFits();
			this._screenContents.UpdatePadding(0f, 25f);
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x00103540 File Offset: 0x00101740
		private void OnLeftButtonButtonReleased(NButton _)
		{
			TaskHelper.RunSafely(this.RefreshAndSelectRun(this._index + 1));
			Tween screenTween = this._screenTween;
			if (screenTween != null)
			{
				screenTween.Kill();
			}
			this._screenTween = base.CreateTween().SetParallel(true);
			this._screenTween.TweenProperty(this._screenContents, "position", Vector2.Zero, 0.5).SetTrans(5L).SetEase(1L)
				.From(Vector2.Zero + new Vector2(-1000f, 0f));
			this._screenTween.TweenProperty(this._screenContents, "modulate:a", 1f, 0.4).SetTrans(0L).From(0f);
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x0010362C File Offset: 0x0010182C
		private void OnRightButtonButtonReleased(NButton _)
		{
			TaskHelper.RunSafely(this.RefreshAndSelectRun(this._index - 1));
			Tween screenTween = this._screenTween;
			if (screenTween != null)
			{
				screenTween.Kill();
			}
			this._screenTween = base.CreateTween().SetParallel(true);
			this._screenTween.TweenProperty(this._screenContents, "position", Vector2.Zero, 0.5).SetTrans(5L).SetEase(1L)
				.From(Vector2.Zero + new Vector2(1000f, 0f));
			this._screenTween.TweenProperty(this._screenContents, "modulate:a", 1f, 0.4).SetTrans(0L).From(0f);
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x00103715 File Offset: 0x00101915
		public static bool CanBeShown()
		{
			return SaveManager.Instance.GetRunHistoryCount() > 0;
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x00103724 File Offset: 0x00101924
		public override void OnSubmenuOpened()
		{
			this._runNames.Clear();
			this._runNames.AddRange(SaveManager.Instance.GetAllRunHistoryNames());
			this._runNames.Reverse();
			TaskHelper.RunSafely(this.RefreshAndSelectRun(0));
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x00103760 File Offset: 0x00101960
		protected override void OnSubmenuShown()
		{
			if (!NRunHistory.CanBeShown())
			{
				throw new InvalidOperationException("Tried to show run history screen with no runs!");
			}
			Tween screenTween = this._screenTween;
			if (screenTween != null)
			{
				screenTween.Kill();
			}
			this._screenTween = base.CreateTween();
			this._screenTween.TweenProperty(this._screenContents, "modulate:a", 1f, 0.4).From(0f);
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x001037DA File Offset: 0x001019DA
		protected override void OnSubmenuHidden()
		{
			Tween screenTween = this._screenTween;
			if (screenTween == null)
			{
				return;
			}
			screenTween.Kill();
		}

		/// <summary>
		/// Called whenever we want to load a run history file!
		/// </summary>
		// Token: 0x06002662 RID: 9826 RVA: 0x001037EC File Offset: 0x001019EC
		private Task RefreshAndSelectRun(int index)
		{
			if (index < 0 || index >= this._runNames.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid run index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(index);
				defaultInterpolatedStringHandler.AppendLiteral(", valid range is 0-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._runNames.Count - 1);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return Task.CompletedTask;
			}
			this._prevButton.Disable();
			this._nextButton.Disable();
			this._outOfDateVisual.Visible = false;
			try
			{
				ReadSaveResult<RunHistory> readSaveResult = SaveManager.Instance.LoadRunHistory(this._runNames[index]);
				if (readSaveResult.Success)
				{
					this.DisplayRun(readSaveResult.SaveData);
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Could not load run ");
					defaultInterpolatedStringHandler.AppendFormatted(this._runNames[index]);
					defaultInterpolatedStringHandler.AppendLiteral(" at index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(index);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(readSaveResult.ErrorMessage);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted<ReadSaveStatus>(readSaveResult.Status);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					this._outOfDateVisual.Visible = true;
				}
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Exception ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				defaultInterpolatedStringHandler.AppendLiteral(" while loading run at index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(index);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				this._outOfDateVisual.Visible = true;
				throw;
			}
			finally
			{
				this._index = index;
				if (index < this._runNames.Count - 1)
				{
					this._prevButton.Enable();
				}
				if (index > 0)
				{
					this._nextButton.Enable();
				}
				this._prevButton.Visible = index < this._runNames.Count - 1;
				this._nextButton.Visible = index > 0;
			}
			return Task.CompletedTask;
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x00103A28 File Offset: 0x00101C28
		public override void _Input(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (NDevConsole.IsConsoleVisible)
			{
				return;
			}
			if (!NControllerManager.Instance.IsUsingController)
			{
				return;
			}
			Control control = base.GetViewport().GuiGetFocusOwner();
			bool flag = control is TextEdit || control is LineEdit;
			if (flag)
			{
				return;
			}
			if (!ActiveScreenContext.Instance.IsCurrent(this))
			{
				return;
			}
			Control control2 = base.GetViewport().GuiGetFocusOwner();
			if (control2 != null && base.IsAncestorOf(control2))
			{
				return;
			}
			if (inputEvent.IsActionPressed(MegaInput.left, false, false) || inputEvent.IsActionPressed(MegaInput.right, false, false) || inputEvent.IsActionPressed(MegaInput.up, false, false) || inputEvent.IsActionPressed(MegaInput.down, false, false) || inputEvent.IsActionPressed(MegaInput.select, false, false))
			{
				Viewport viewport = base.GetViewport();
				if (viewport != null)
				{
					viewport.SetInputAsHandled();
				}
				Control defaultFocusedControl = this._mapPointHistory.DefaultFocusedControl;
				if (defaultFocusedControl == null)
				{
					return;
				}
				defaultFocusedControl.TryGrabFocus();
			}
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x00103B14 File Offset: 0x00101D14
		private void DisplayRun(RunHistory history)
		{
			NRunHistoryPlayerIcon selectedPlayerIcon = this._selectedPlayerIcon;
			if (selectedPlayerIcon != null)
			{
				selectedPlayerIcon.Deselect();
			}
			this._selectedPlayerIcon = null;
			foreach (NRunHistoryPlayerIcon nrunHistoryPlayerIcon in this._playerIconContainer.GetChildren(false).OfType<NRunHistoryPlayerIcon>())
			{
				nrunHistoryPlayerIcon.QueueFreeSafely();
			}
			this._history = history;
			ulong localPlayerId = PlatformUtil.GetLocalPlayerId(history.PlatformType);
			this.LoadPlayerFloor(history);
			this.LoadGameModeDetails(history);
			this.LoadTimeDetails(history);
			this._mapPointHistory.LoadHistory(history);
			bool flag = false;
			NRunHistoryPlayerIcon nrunHistoryPlayerIcon2 = null;
			foreach (RunHistoryPlayer runHistoryPlayer in history.Players)
			{
				NRunHistoryPlayerIcon playerIcon = PreloadManager.Cache.GetScene(NRunHistoryPlayerIcon.scenePath).Instantiate<NRunHistoryPlayerIcon>(0L);
				if (nrunHistoryPlayerIcon2 == null)
				{
					nrunHistoryPlayerIcon2 = playerIcon;
				}
				this._playerIconContainer.AddChildSafely(playerIcon);
				playerIcon.LoadRun(runHistoryPlayer, history);
				playerIcon.Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(delegate(NClickableControl _)
				{
					this.SelectPlayer(playerIcon);
				}), 0U);
				if (runHistoryPlayer.Id == localPlayerId)
				{
					flag = true;
					this.SelectPlayer(playerIcon);
				}
			}
			for (int i = 0; i < this._playerIconContainer.GetChildCount(false); i++)
			{
				this._playerIconContainer.GetChild<Control>(i, false).FocusNeighborLeft = ((i > 0) ? this._playerIconContainer.GetChild<Control>(i - 1, false).GetPath() : this._playerIconContainer.GetChild<Control>(this._playerIconContainer.GetChildCount(false) - 1, false).GetPath());
				this._playerIconContainer.GetChild<Control>(i, false).FocusNeighborRight = ((i < this._playerIconContainer.GetChildCount(false) - 1) ? this._playerIconContainer.GetChild<Control>(i + 1, false).GetPath() : this._playerIconContainer.GetChild<Control>(0, false).GetPath());
			}
			if (!flag)
			{
				if (history.Players.Count > 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(91, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Local player with ID ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(localPlayerId);
					defaultInterpolatedStringHandler.AppendLiteral(" not found in multiplayer run history file! Defaulting to first player");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				this.SelectPlayer(nrunHistoryPlayerIcon2);
			}
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x00103DA4 File Offset: 0x00101FA4
		private void SelectPlayer(NRunHistoryPlayerIcon playerIcon)
		{
			NRunHistoryPlayerIcon selectedPlayerIcon = this._selectedPlayerIcon;
			if (selectedPlayerIcon != null)
			{
				selectedPlayerIcon.Deselect();
			}
			this._selectedPlayerIcon = playerIcon;
			playerIcon.Select();
			if (this._history.Players.Count != 1)
			{
				LocString locString = new LocString("run_history", "PLAYER_NAME");
				locString.Add("PlayerName", PlatformUtil.GetPlayerName(this._history.PlatformType, playerIcon.Player.Id));
			}
			UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
			Player player = Player.CreateForNewRun(SaveUtil.CharacterOrDeprecated(playerIcon.Player.Character), unlockState, playerIcon.Player.Id);
			this.LoadGoldHpAndPotionInfo(playerIcon);
			this.LoadDeathQuote(this._history, playerIcon.Player.Character);
			this.LoadBadges(this._history.Players.FirstOrDefault<RunHistoryPlayer>((RunHistoryPlayer p) => p.Id == player.NetId).Badges.ToList<SerializableBadge>());
			this._mapPointHistory.SetPlayer(playerIcon.Player);
			this._relicHistory.LoadRelics(player, playerIcon.Player.Relics);
			this._deckHistory.LoadDeck(player, playerIcon.Player.Deck);
			TaskHelper.RunSafely(this.ResizeScreen());
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x00103EF0 File Offset: 0x001020F0
		private async Task ResizeScreen()
		{
			await this.AwaitProcessFrame(default(CancellationToken));
			this._screenContents.GetNode<Control>("Content").ResetSize();
			this._screenContents.InstantlyScrollToTop();
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x00103F34 File Offset: 0x00102134
		private void LoadGoldHpAndPotionInfo(NRunHistoryPlayerIcon icon)
		{
			if (!this._history.MapPointHistory.Any<List<MapPointHistoryEntry>>())
			{
				CharacterModel characterModel = SaveUtil.CharacterOrDeprecated(icon.Player.Character);
				MegaLabel hpLabel = this._hpLabel;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(characterModel.StartingHp);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<int>(characterModel.StartingHp);
				hpLabel.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
				MegaLabel goldLabel = this._goldLabel;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(characterModel.StartingGold);
				goldLabel.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			else
			{
				MapPointHistoryEntry mapPointHistoryEntry = this._history.MapPointHistory.Last<List<MapPointHistoryEntry>>().Last<MapPointHistoryEntry>();
				PlayerMapPointHistoryEntry playerMapPointHistoryEntry = mapPointHistoryEntry.PlayerStats.First<PlayerMapPointHistoryEntry>((PlayerMapPointHistoryEntry stat) => stat.PlayerId == icon.Player.Id);
				MegaLabel hpLabel2 = this._hpLabel;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(playerMapPointHistoryEntry.CurrentHp);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<int>(playerMapPointHistoryEntry.MaxHp);
				hpLabel2.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
				MegaLabel goldLabel2 = this._goldLabel;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(playerMapPointHistoryEntry.CurrentGold);
				goldLabel2.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._potionHolder.FreeChildren();
			RunHistoryPlayer runHistoryPlayer = this._history.Players.First<RunHistoryPlayer>((RunHistoryPlayer player) => player.Id == icon.Player.Id);
			IEnumerable<SerializablePotion> potions = runHistoryPlayer.Potions;
			Func<SerializablePotion, PotionModel> func;
			if ((func = NRunHistory.<>O.<0>__FromSerializable) == null)
			{
				func = (NRunHistory.<>O.<0>__FromSerializable = new Func<SerializablePotion, PotionModel>(PotionModel.FromSerializable));
			}
			List<PotionModel> list = potions.Select<SerializablePotion, PotionModel>(func).ToList<PotionModel>();
			List<NPotionHolder> list2 = new List<NPotionHolder>();
			for (int i = 0; i < runHistoryPlayer.MaxPotionSlotCount; i++)
			{
				NPotionHolder npotionHolder = NPotionHolder.Create(false);
				this._potionHolder.AddChildSafely(npotionHolder);
				list2.Add(npotionHolder);
			}
			UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
			Player player2 = Player.CreateForNewRun(SaveUtil.CharacterOrDeprecated(icon.Player.Character), unlockState, icon.Player.Id);
			int num = 0;
			while (num < list.Count && num < runHistoryPlayer.MaxPotionSlotCount)
			{
				NPotion npotion = NPotion.Create(list[num]);
				npotion.Model.Owner = player2;
				list2[num].AddPotion(npotion);
				npotion.Position = Vector2.Zero;
				num++;
			}
		}

		/// <summary>
		/// Counts the total number of MapPointHistoryEntry objects.
		/// This is how many floors the player has traversed I think.
		/// </summary>
		// Token: 0x06002668 RID: 9832 RVA: 0x001041A8 File Offset: 0x001023A8
		private void LoadPlayerFloor(RunHistory history)
		{
			int num = history.MapPointHistory.Sum<List<MapPointHistoryEntry>>((List<MapPointHistoryEntry> rooms) => rooms.Count);
			MegaLabel floorLabel = this._floorLabel;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			floorLabel.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		/// <summary>
		/// Sets the text in the top right that is like Singleplayer: Standard or Multiplayer: Custom, etc.
		/// </summary>
		/// <param name="history"></param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
		// Token: 0x06002669 RID: 9833 RVA: 0x00104204 File Offset: 0x00102404
		private void LoadGameModeDetails(RunHistory history)
		{
			LocString locString = new LocString("run_history", "GAME_MODE.title");
			if (history.Players.Count > 1)
			{
				locString.Add("PlayerCount", new LocString("run_history", "PLAYER_COUNT.multiplayer"));
			}
			else
			{
				locString.Add("PlayerCount", new LocString("run_history", "PLAYER_COUNT.singleplayer"));
			}
			switch (history.GameMode)
			{
			case GameMode.Standard:
				locString.Add("GameMode", new LocString("run_history", "GAME_MODE.standard"));
				goto IL_00E1;
			case GameMode.Daily:
				locString.Add("GameMode", new LocString("run_history", "GAME_MODE.daily"));
				goto IL_00E1;
			case GameMode.Custom:
				locString.Add("GameMode", new LocString("run_history", "GAME_MODE.custom"));
				goto IL_00E1;
			}
			locString.Add("GameMode", new LocString("run_history", "GAME_MODE.unknown"));
			IL_00E1:
			this._gameModeLabel.Text = locString.GetFormattedText() ?? "";
		}

		/// <summary>
		/// Given a <see cref="T:MegaCrit.Sts2.Core.Runs.RunHistory" /> entry, returns how the run ended (aka the <see cref="T:MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.GameOverType" />).
		/// </summary>
		// Token: 0x0600266A RID: 9834 RVA: 0x0010430C File Offset: 0x0010250C
		public static GameOverType GetGameOverType(RunHistory history)
		{
			if (history.Win)
			{
				return GameOverType.FalseVictory;
			}
			if (history.WasAbandoned)
			{
				return GameOverType.AbandonedRun;
			}
			if (history.KilledByEncounter != ModelId.none)
			{
				return GameOverType.CombatDeath;
			}
			if (history.KilledByEvent != ModelId.none)
			{
				return GameOverType.EventDeath;
			}
			Log.Warn("How did the game end??", 2);
			return GameOverType.None;
		}

		/// <summary>
		/// Helper method to generate a death quote given a run history entry, characterId, and how they died.
		/// </summary>
		// Token: 0x0600266B RID: 9835 RVA: 0x00104364 File Offset: 0x00102564
		public static string GetDeathQuote(RunHistory history, ModelId characterId, GameOverType gameOverType)
		{
			CharacterModel characterModel = SaveUtil.CharacterOrDeprecated(characterId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(NRunHistory._leftQuote.GetRawText());
			Rng rng = new Rng(StringHelper.GetDeterministicHashCode(history.Seed));
			switch (gameOverType)
			{
			case GameOverType.None:
			case GameOverType.TrueVictory:
			{
				LocString deathDebugMessage = NRunHistory.GetDeathDebugMessage();
				characterModel.AddDetailsTo(deathDebugMessage);
				stringBuilder.Append(deathDebugMessage.GetFormattedText());
				break;
			}
			case GameOverType.AbandonedRun:
			{
				LocString randomWithPrefix = LocString.GetRandomWithPrefix("run_history", "MAP_POINT_HISTORY.abandon", rng);
				characterModel.AddDetailsTo(randomWithPrefix);
				stringBuilder.Append(randomWithPrefix.GetFormattedText());
				break;
			}
			case GameOverType.EventDeath:
			{
				EventModel eventModel;
				try
				{
					eventModel = ModelDb.GetById<EventModel>(history.KilledByEvent);
				}
				catch (ModelNotFoundException)
				{
					eventModel = ModelDb.Event<DeprecatedEvent>();
				}
				LocString locString = LocString.GetIfExists(eventModel.LocTable, eventModel.Id.Entry + ".loss");
				if (locString == null)
				{
					locString = NRunHistory.GetDeathDebugMessage();
				}
				characterModel.AddDetailsTo(locString);
				locString.Add("event", eventModel.Title);
				stringBuilder.Append(locString.GetFormattedText());
				break;
			}
			case GameOverType.CombatDeath:
			{
				EncounterModel encounterModel = SaveUtil.EncounterOrDeprecated(history.KilledByEncounter);
				LocString lossMessageFor = encounterModel.GetLossMessageFor(characterModel);
				stringBuilder.Append(lossMessageFor.GetFormattedText());
				break;
			}
			case GameOverType.FalseVictory:
			{
				LocString randomWithPrefix2 = LocString.GetRandomWithPrefix("run_history", "MAP_POINT_HISTORY.falseVictory", rng);
				characterModel.AddDetailsTo(randomWithPrefix2);
				stringBuilder.Append(randomWithPrefix2.GetFormattedText());
				break;
			}
			default:
				Log.Error("Unimplemented GameOverType: " + gameOverType.ToString(), 2);
				throw new ArgumentOutOfRangeException("gameOverType", gameOverType, null);
			}
			stringBuilder.Append(NRunHistory._rightQuote.GetRawText());
			return stringBuilder.ToString();
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x0010452C File Offset: 0x0010272C
		private void LoadDeathQuote(RunHistory history, ModelId characterId)
		{
			CharacterModel characterModel = SaveUtil.CharacterOrDeprecated(characterId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(NRunHistory._leftQuote.GetRawText());
			Rng rng = new Rng(StringHelper.GetDeterministicHashCode(history.Seed));
			if (history.Win)
			{
				this._deathQuoteLabel.AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, StsColors.green);
				LocString randomWithPrefix = LocString.GetRandomWithPrefix("run_history", "MAP_POINT_HISTORY.falseVictory", rng);
				characterModel.AddDetailsTo(randomWithPrefix);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder3 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(randomWithPrefix.GetFormattedText());
				stringBuilder3.Append(ref appendInterpolatedStringHandler);
			}
			else if (history.WasAbandoned)
			{
				this._deathQuoteLabel.AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, StsColors.red);
				LocString randomWithPrefix2 = LocString.GetRandomWithPrefix("run_history", "MAP_POINT_HISTORY.abandon", rng);
				characterModel.AddDetailsTo(randomWithPrefix2);
				stringBuilder.Append(randomWithPrefix2.GetFormattedText());
			}
			else if (history.KilledByEncounter != ModelId.none)
			{
				this._deathQuoteLabel.AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, StsColors.red);
				EncounterModel encounterModel = SaveUtil.EncounterOrDeprecated(history.KilledByEncounter);
				LocString lossMessageFor = encounterModel.GetLossMessageFor(characterModel);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(lossMessageFor.GetFormattedText());
				stringBuilder4.Append(ref appendInterpolatedStringHandler);
			}
			else if (history.KilledByEvent != ModelId.none)
			{
				this._deathQuoteLabel.AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, StsColors.red);
				EventModel eventModel;
				try
				{
					eventModel = ModelDb.GetById<EventModel>(history.KilledByEvent);
				}
				catch (ModelNotFoundException)
				{
					eventModel = ModelDb.Event<DeprecatedEvent>();
				}
				string text = eventModel.Id.Entry + ".loss";
				LocString locString;
				if (LocString.Exists("events", text))
				{
					locString = new LocString("events", text);
				}
				else
				{
					locString = new LocString("run_history", "DEFAULT_EVENT_LOSS_MESSAGE");
				}
				characterModel.AddDetailsTo(locString);
				locString.Add("event", eventModel.Title);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder5 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(locString.GetFormattedText());
				stringBuilder5.Append(ref appendInterpolatedStringHandler);
			}
			stringBuilder.Append(NRunHistory._rightQuote.GetRawText());
			this._deathQuoteLabel.Text = stringBuilder.ToString();
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x00104778 File Offset: 0x00102978
		private static LocString GetDeathDebugMessage()
		{
			return new LocString("run_history", "MAP_POINT_HISTORY.debug");
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x0010478C File Offset: 0x0010298C
		private void LoadBadges(List<SerializableBadge> badges)
		{
			if (badges.Count == 0)
			{
				this._badgeContainer.Visible = false;
				return;
			}
			this._badgeContainer.FreeChildren();
			this._badgeContainer.Visible = true;
			foreach (SerializableBadge serializableBadge in badges)
			{
				NBadge nbadge = NBadge.Create(serializableBadge.Id, serializableBadge.Rarity);
				if (nbadge != null)
				{
					this._badgeContainer.AddChildSafely(nbadge);
					nbadge.Modulate = Colors.White;
				}
			}
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x0010482C File Offset: 0x00102A2C
		private void LoadTimeDetails(RunHistory history)
		{
			DateTimeFormatInfo dateTimeFormat = LocManager.Instance.CultureInfo.DateTimeFormat;
			DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeSeconds(history.StartTime).UtcDateTime, TimeZoneInfo.Local);
			string text = dateTime.ToString(this._dateFormat.GetRawText(), dateTimeFormat);
			string text2 = dateTime.ToString(this._timeFormat.GetRawText(), dateTimeFormat);
			this._dateTimeLocString.Add("Date", text);
			this._dateTimeLocString.Add("Time", text2);
			this._seedLocString.Add("Seed", history.Seed);
			this._dateLabel.Text = this._dateTimeLocString.GetFormattedText();
			this._seedLabel.Text = this._seedLocString.GetFormattedText();
			this._buildLabel.Text = history.BuildId;
			this._timeLabel.SetTextAutoSize(TimeFormatting.Format(history.RunTime));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002670 RID: 9840 RVA: 0x0010491C File Offset: 0x00102B1C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NRunHistory.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NRunHistory.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunHistory.MethodName.OnLeftButtonButtonReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRunHistory.MethodName.OnRightButtonButtonReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRunHistory.MethodName.CanBeShown, new PropertyInfo(1L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NRunHistory.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunHistory.MethodName.OnSubmenuShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunHistory.MethodName.OnSubmenuHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunHistory.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NRunHistory.MethodName.SelectPlayer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "playerIcon", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NRunHistory.MethodName.LoadGoldHpAndPotionInfo, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "icon", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002671 RID: 9841 RVA: 0x00104C34 File Offset: 0x00102E34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunHistory.MethodName.Create && args.Count == 0)
			{
				NRunHistory nrunHistory = NRunHistory.Create();
				ret = VariantUtils.CreateFrom<NRunHistory>(ref nrunHistory);
				return true;
			}
			if ((ref method) == NRunHistory.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistory.MethodName.OnLeftButtonButtonReleased && args.Count == 1)
			{
				this.OnLeftButtonButtonReleased(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistory.MethodName.OnRightButtonButtonReleased && args.Count == 1)
			{
				this.OnRightButtonButtonReleased(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistory.MethodName.CanBeShown && args.Count == 0)
			{
				bool flag = NRunHistory.CanBeShown();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NRunHistory.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistory.MethodName.OnSubmenuShown && args.Count == 0)
			{
				this.OnSubmenuShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistory.MethodName.OnSubmenuHidden && args.Count == 0)
			{
				this.OnSubmenuHidden();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistory.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistory.MethodName.SelectPlayer && args.Count == 1)
			{
				this.SelectPlayer(VariantUtils.ConvertTo<NRunHistoryPlayerIcon>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunHistory.MethodName.LoadGoldHpAndPotionInfo && args.Count == 1)
			{
				this.LoadGoldHpAndPotionInfo(VariantUtils.ConvertTo<NRunHistoryPlayerIcon>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x00104E34 File Offset: 0x00103034
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunHistory.MethodName.Create && args.Count == 0)
			{
				NRunHistory nrunHistory = NRunHistory.Create();
				ret = VariantUtils.CreateFrom<NRunHistory>(ref nrunHistory);
				return true;
			}
			if ((ref method) == NRunHistory.MethodName.CanBeShown && args.Count == 0)
			{
				bool flag = NRunHistory.CanBeShown();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002673 RID: 9843 RVA: 0x00104EA0 File Offset: 0x001030A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRunHistory.MethodName.Create || (ref method) == NRunHistory.MethodName._Ready || (ref method) == NRunHistory.MethodName.OnLeftButtonButtonReleased || (ref method) == NRunHistory.MethodName.OnRightButtonButtonReleased || (ref method) == NRunHistory.MethodName.CanBeShown || (ref method) == NRunHistory.MethodName.OnSubmenuOpened || (ref method) == NRunHistory.MethodName.OnSubmenuShown || (ref method) == NRunHistory.MethodName.OnSubmenuHidden || (ref method) == NRunHistory.MethodName._Input || (ref method) == NRunHistory.MethodName.SelectPlayer || (ref method) == NRunHistory.MethodName.LoadGoldHpAndPotionInfo || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002674 RID: 9844 RVA: 0x00104F5C File Offset: 0x0010315C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRunHistory.PropertyName._screenContents)
			{
				this._screenContents = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._playerIconContainer)
			{
				this._playerIconContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._hpLabel)
			{
				this._hpLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._goldLabel)
			{
				this._goldLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._potionHolder)
			{
				this._potionHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._floorLabel)
			{
				this._floorLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._timeLabel)
			{
				this._timeLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._dateLabel)
			{
				this._dateLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._seedLabel)
			{
				this._seedLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._gameModeLabel)
			{
				this._gameModeLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._buildLabel)
			{
				this._buildLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._badgeContainer)
			{
				this._badgeContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._deathQuoteLabel)
			{
				this._deathQuoteLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._mapPointHistory)
			{
				this._mapPointHistory = VariantUtils.ConvertTo<NMapPointHistory>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._relicHistory)
			{
				this._relicHistory = VariantUtils.ConvertTo<NRelicHistory>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._deckHistory)
			{
				this._deckHistory = VariantUtils.ConvertTo<NDeckHistory>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._outOfDateVisual)
			{
				this._outOfDateVisual = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._index)
			{
				this._index = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._prevButton)
			{
				this._prevButton = VariantUtils.ConvertTo<NRunHistoryArrowButton>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._nextButton)
			{
				this._nextButton = VariantUtils.ConvertTo<NRunHistoryArrowButton>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._selectedPlayerIcon)
			{
				this._selectedPlayerIcon = VariantUtils.ConvertTo<NRunHistoryPlayerIcon>(ref value);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._screenTween)
			{
				this._screenTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002675 RID: 9845 RVA: 0x001051C4 File Offset: 0x001033C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRunHistory.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._screenContents)
			{
				value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._screenContents);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._playerIconContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._playerIconContainer);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._hpLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._hpLabel);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._goldLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._goldLabel);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._potionHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._potionHolder);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._floorLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._floorLabel);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._timeLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._timeLabel);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._dateLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._dateLabel);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._seedLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._seedLabel);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._gameModeLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._gameModeLabel);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._buildLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._buildLabel);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._badgeContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._badgeContainer);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._deathQuoteLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._deathQuoteLabel);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._mapPointHistory)
			{
				value = VariantUtils.CreateFrom<NMapPointHistory>(ref this._mapPointHistory);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._relicHistory)
			{
				value = VariantUtils.CreateFrom<NRelicHistory>(ref this._relicHistory);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._deckHistory)
			{
				value = VariantUtils.CreateFrom<NDeckHistory>(ref this._deckHistory);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._outOfDateVisual)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._outOfDateVisual);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._index)
			{
				value = VariantUtils.CreateFrom<int>(ref this._index);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._prevButton)
			{
				value = VariantUtils.CreateFrom<NRunHistoryArrowButton>(ref this._prevButton);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._nextButton)
			{
				value = VariantUtils.CreateFrom<NRunHistoryArrowButton>(ref this._nextButton);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._selectedPlayerIcon)
			{
				value = VariantUtils.CreateFrom<NRunHistoryPlayerIcon>(ref this._selectedPlayerIcon);
				return true;
			}
			if ((ref name) == NRunHistory.PropertyName._screenTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._screenTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002676 RID: 9846 RVA: 0x001054BC File Offset: 0x001036BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRunHistory.PropertyName.InitialFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._screenContents, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._playerIconContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._hpLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._goldLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._potionHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._floorLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._timeLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._dateLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._seedLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._gameModeLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._buildLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._badgeContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._deathQuoteLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._mapPointHistory, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._relicHistory, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._deckHistory, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._outOfDateVisual, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRunHistory.PropertyName._index, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._prevButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._nextButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._selectedPlayerIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunHistory.PropertyName._screenTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002677 RID: 9847 RVA: 0x001057C8 File Offset: 0x001039C8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRunHistory.PropertyName._screenContents, Variant.From<NScrollableContainer>(ref this._screenContents));
			info.AddProperty(NRunHistory.PropertyName._playerIconContainer, Variant.From<Control>(ref this._playerIconContainer));
			info.AddProperty(NRunHistory.PropertyName._hpLabel, Variant.From<MegaLabel>(ref this._hpLabel));
			info.AddProperty(NRunHistory.PropertyName._goldLabel, Variant.From<MegaLabel>(ref this._goldLabel));
			info.AddProperty(NRunHistory.PropertyName._potionHolder, Variant.From<Control>(ref this._potionHolder));
			info.AddProperty(NRunHistory.PropertyName._floorLabel, Variant.From<MegaLabel>(ref this._floorLabel));
			info.AddProperty(NRunHistory.PropertyName._timeLabel, Variant.From<MegaLabel>(ref this._timeLabel));
			info.AddProperty(NRunHistory.PropertyName._dateLabel, Variant.From<MegaRichTextLabel>(ref this._dateLabel));
			info.AddProperty(NRunHistory.PropertyName._seedLabel, Variant.From<MegaRichTextLabel>(ref this._seedLabel));
			info.AddProperty(NRunHistory.PropertyName._gameModeLabel, Variant.From<MegaRichTextLabel>(ref this._gameModeLabel));
			info.AddProperty(NRunHistory.PropertyName._buildLabel, Variant.From<MegaRichTextLabel>(ref this._buildLabel));
			info.AddProperty(NRunHistory.PropertyName._badgeContainer, Variant.From<Control>(ref this._badgeContainer));
			info.AddProperty(NRunHistory.PropertyName._deathQuoteLabel, Variant.From<MegaRichTextLabel>(ref this._deathQuoteLabel));
			info.AddProperty(NRunHistory.PropertyName._mapPointHistory, Variant.From<NMapPointHistory>(ref this._mapPointHistory));
			info.AddProperty(NRunHistory.PropertyName._relicHistory, Variant.From<NRelicHistory>(ref this._relicHistory));
			info.AddProperty(NRunHistory.PropertyName._deckHistory, Variant.From<NDeckHistory>(ref this._deckHistory));
			info.AddProperty(NRunHistory.PropertyName._outOfDateVisual, Variant.From<Control>(ref this._outOfDateVisual));
			info.AddProperty(NRunHistory.PropertyName._index, Variant.From<int>(ref this._index));
			info.AddProperty(NRunHistory.PropertyName._prevButton, Variant.From<NRunHistoryArrowButton>(ref this._prevButton));
			info.AddProperty(NRunHistory.PropertyName._nextButton, Variant.From<NRunHistoryArrowButton>(ref this._nextButton));
			info.AddProperty(NRunHistory.PropertyName._selectedPlayerIcon, Variant.From<NRunHistoryPlayerIcon>(ref this._selectedPlayerIcon));
			info.AddProperty(NRunHistory.PropertyName._screenTween, Variant.From<Tween>(ref this._screenTween));
		}

		/// <inheritdoc />
		// Token: 0x06002678 RID: 9848 RVA: 0x001059C0 File Offset: 0x00103BC0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRunHistory.PropertyName._screenContents, ref variant))
			{
				this._screenContents = variant.As<NScrollableContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRunHistory.PropertyName._playerIconContainer, ref variant2))
			{
				this._playerIconContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRunHistory.PropertyName._hpLabel, ref variant3))
			{
				this._hpLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRunHistory.PropertyName._goldLabel, ref variant4))
			{
				this._goldLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRunHistory.PropertyName._potionHolder, ref variant5))
			{
				this._potionHolder = variant5.As<Control>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRunHistory.PropertyName._floorLabel, ref variant6))
			{
				this._floorLabel = variant6.As<MegaLabel>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRunHistory.PropertyName._timeLabel, ref variant7))
			{
				this._timeLabel = variant7.As<MegaLabel>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRunHistory.PropertyName._dateLabel, ref variant8))
			{
				this._dateLabel = variant8.As<MegaRichTextLabel>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRunHistory.PropertyName._seedLabel, ref variant9))
			{
				this._seedLabel = variant9.As<MegaRichTextLabel>();
			}
			Variant variant10;
			if (info.TryGetProperty(NRunHistory.PropertyName._gameModeLabel, ref variant10))
			{
				this._gameModeLabel = variant10.As<MegaRichTextLabel>();
			}
			Variant variant11;
			if (info.TryGetProperty(NRunHistory.PropertyName._buildLabel, ref variant11))
			{
				this._buildLabel = variant11.As<MegaRichTextLabel>();
			}
			Variant variant12;
			if (info.TryGetProperty(NRunHistory.PropertyName._badgeContainer, ref variant12))
			{
				this._badgeContainer = variant12.As<Control>();
			}
			Variant variant13;
			if (info.TryGetProperty(NRunHistory.PropertyName._deathQuoteLabel, ref variant13))
			{
				this._deathQuoteLabel = variant13.As<MegaRichTextLabel>();
			}
			Variant variant14;
			if (info.TryGetProperty(NRunHistory.PropertyName._mapPointHistory, ref variant14))
			{
				this._mapPointHistory = variant14.As<NMapPointHistory>();
			}
			Variant variant15;
			if (info.TryGetProperty(NRunHistory.PropertyName._relicHistory, ref variant15))
			{
				this._relicHistory = variant15.As<NRelicHistory>();
			}
			Variant variant16;
			if (info.TryGetProperty(NRunHistory.PropertyName._deckHistory, ref variant16))
			{
				this._deckHistory = variant16.As<NDeckHistory>();
			}
			Variant variant17;
			if (info.TryGetProperty(NRunHistory.PropertyName._outOfDateVisual, ref variant17))
			{
				this._outOfDateVisual = variant17.As<Control>();
			}
			Variant variant18;
			if (info.TryGetProperty(NRunHistory.PropertyName._index, ref variant18))
			{
				this._index = variant18.As<int>();
			}
			Variant variant19;
			if (info.TryGetProperty(NRunHistory.PropertyName._prevButton, ref variant19))
			{
				this._prevButton = variant19.As<NRunHistoryArrowButton>();
			}
			Variant variant20;
			if (info.TryGetProperty(NRunHistory.PropertyName._nextButton, ref variant20))
			{
				this._nextButton = variant20.As<NRunHistoryArrowButton>();
			}
			Variant variant21;
			if (info.TryGetProperty(NRunHistory.PropertyName._selectedPlayerIcon, ref variant21))
			{
				this._selectedPlayerIcon = variant21.As<NRunHistoryPlayerIcon>();
			}
			Variant variant22;
			if (info.TryGetProperty(NRunHistory.PropertyName._screenTween, ref variant22))
			{
				this._screenTween = variant22.As<Tween>();
			}
		}

		// Token: 0x04000E2A RID: 3626
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/run_history_screen/run_history");

		/// <summary>
		/// Padding at the bottom of the screen so that the scrollbar shows up if the contents don't quite touch the bottom
		/// </summary>
		// Token: 0x04000E2B RID: 3627
		private const float _bottomScrollbarPadding = 25f;

		// Token: 0x04000E2C RID: 3628
		public const string locTable = "run_history";

		// Token: 0x04000E2D RID: 3629
		private static readonly LocString _leftQuote = new LocString("game_over_screen", "ENCOUNTER_QUOTE_LEFT");

		// Token: 0x04000E2E RID: 3630
		private static readonly LocString _rightQuote = new LocString("game_over_screen", "ENCOUNTER_QUOTE_RIGHT");

		// Token: 0x04000E2F RID: 3631
		private readonly LocString _dateFormat = new LocString("run_history", "INFO.DATE_FORMAT");

		// Token: 0x04000E30 RID: 3632
		private readonly LocString _timeFormat = new LocString("run_history", "INFO.TIME_FORMAT");

		// Token: 0x04000E31 RID: 3633
		private readonly LocString _dateTimeLocString = new LocString("run_history", "INFO.DATE_TIME");

		// Token: 0x04000E32 RID: 3634
		private readonly LocString _seedLocString = new LocString("run_history", "INFO.SEED");

		// Token: 0x04000E33 RID: 3635
		private NScrollableContainer _screenContents;

		// Token: 0x04000E34 RID: 3636
		private Control _playerIconContainer;

		// Token: 0x04000E35 RID: 3637
		private MegaLabel _hpLabel;

		// Token: 0x04000E36 RID: 3638
		private MegaLabel _goldLabel;

		// Token: 0x04000E37 RID: 3639
		private Control _potionHolder;

		// Token: 0x04000E38 RID: 3640
		private MegaLabel _floorLabel;

		// Token: 0x04000E39 RID: 3641
		private MegaLabel _timeLabel;

		// Token: 0x04000E3A RID: 3642
		private MegaRichTextLabel _dateLabel;

		// Token: 0x04000E3B RID: 3643
		private MegaRichTextLabel _seedLabel;

		// Token: 0x04000E3C RID: 3644
		private MegaRichTextLabel _gameModeLabel;

		// Token: 0x04000E3D RID: 3645
		private MegaRichTextLabel _buildLabel;

		// Token: 0x04000E3E RID: 3646
		private Control _badgeContainer;

		// Token: 0x04000E3F RID: 3647
		private MegaRichTextLabel _deathQuoteLabel;

		// Token: 0x04000E40 RID: 3648
		private NMapPointHistory _mapPointHistory;

		// Token: 0x04000E41 RID: 3649
		private NRelicHistory _relicHistory;

		// Token: 0x04000E42 RID: 3650
		private NDeckHistory _deckHistory;

		// Token: 0x04000E43 RID: 3651
		private Control _outOfDateVisual;

		// Token: 0x04000E44 RID: 3652
		private readonly List<string> _runNames = new List<string>();

		// Token: 0x04000E45 RID: 3653
		private int _index;

		// Token: 0x04000E46 RID: 3654
		private RunHistory _history;

		// Token: 0x04000E47 RID: 3655
		private NRunHistoryArrowButton _prevButton;

		// Token: 0x04000E48 RID: 3656
		private NRunHistoryArrowButton _nextButton;

		// Token: 0x04000E49 RID: 3657
		[Nullable(2)]
		private NRunHistoryPlayerIcon _selectedPlayerIcon;

		// Token: 0x04000E4A RID: 3658
		[Nullable(2)]
		private Tween _screenTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200143D RID: 5181
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004CCB RID: 19659
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004CCC RID: 19660
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnLeftButtonButtonReleased' method.
			/// </summary>
			// Token: 0x04004CCD RID: 19661
			public static readonly StringName OnLeftButtonButtonReleased = "OnLeftButtonButtonReleased";

			/// <summary>
			/// Cached name for the 'OnRightButtonButtonReleased' method.
			/// </summary>
			// Token: 0x04004CCE RID: 19662
			public static readonly StringName OnRightButtonButtonReleased = "OnRightButtonButtonReleased";

			/// <summary>
			/// Cached name for the 'CanBeShown' method.
			/// </summary>
			// Token: 0x04004CCF RID: 19663
			public static readonly StringName CanBeShown = "CanBeShown";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04004CD0 RID: 19664
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuShown' method.
			/// </summary>
			// Token: 0x04004CD1 RID: 19665
			public new static readonly StringName OnSubmenuShown = "OnSubmenuShown";

			/// <summary>
			/// Cached name for the 'OnSubmenuHidden' method.
			/// </summary>
			// Token: 0x04004CD2 RID: 19666
			public new static readonly StringName OnSubmenuHidden = "OnSubmenuHidden";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04004CD3 RID: 19667
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'SelectPlayer' method.
			/// </summary>
			// Token: 0x04004CD4 RID: 19668
			public static readonly StringName SelectPlayer = "SelectPlayer";

			/// <summary>
			/// Cached name for the 'LoadGoldHpAndPotionInfo' method.
			/// </summary>
			// Token: 0x04004CD5 RID: 19669
			public static readonly StringName LoadGoldHpAndPotionInfo = "LoadGoldHpAndPotionInfo";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200143E RID: 5182
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04004CD6 RID: 19670
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_screenContents' field.
			/// </summary>
			// Token: 0x04004CD7 RID: 19671
			public static readonly StringName _screenContents = "_screenContents";

			/// <summary>
			/// Cached name for the '_playerIconContainer' field.
			/// </summary>
			// Token: 0x04004CD8 RID: 19672
			public static readonly StringName _playerIconContainer = "_playerIconContainer";

			/// <summary>
			/// Cached name for the '_hpLabel' field.
			/// </summary>
			// Token: 0x04004CD9 RID: 19673
			public static readonly StringName _hpLabel = "_hpLabel";

			/// <summary>
			/// Cached name for the '_goldLabel' field.
			/// </summary>
			// Token: 0x04004CDA RID: 19674
			public static readonly StringName _goldLabel = "_goldLabel";

			/// <summary>
			/// Cached name for the '_potionHolder' field.
			/// </summary>
			// Token: 0x04004CDB RID: 19675
			public static readonly StringName _potionHolder = "_potionHolder";

			/// <summary>
			/// Cached name for the '_floorLabel' field.
			/// </summary>
			// Token: 0x04004CDC RID: 19676
			public static readonly StringName _floorLabel = "_floorLabel";

			/// <summary>
			/// Cached name for the '_timeLabel' field.
			/// </summary>
			// Token: 0x04004CDD RID: 19677
			public static readonly StringName _timeLabel = "_timeLabel";

			/// <summary>
			/// Cached name for the '_dateLabel' field.
			/// </summary>
			// Token: 0x04004CDE RID: 19678
			public static readonly StringName _dateLabel = "_dateLabel";

			/// <summary>
			/// Cached name for the '_seedLabel' field.
			/// </summary>
			// Token: 0x04004CDF RID: 19679
			public static readonly StringName _seedLabel = "_seedLabel";

			/// <summary>
			/// Cached name for the '_gameModeLabel' field.
			/// </summary>
			// Token: 0x04004CE0 RID: 19680
			public static readonly StringName _gameModeLabel = "_gameModeLabel";

			/// <summary>
			/// Cached name for the '_buildLabel' field.
			/// </summary>
			// Token: 0x04004CE1 RID: 19681
			public static readonly StringName _buildLabel = "_buildLabel";

			/// <summary>
			/// Cached name for the '_badgeContainer' field.
			/// </summary>
			// Token: 0x04004CE2 RID: 19682
			public static readonly StringName _badgeContainer = "_badgeContainer";

			/// <summary>
			/// Cached name for the '_deathQuoteLabel' field.
			/// </summary>
			// Token: 0x04004CE3 RID: 19683
			public static readonly StringName _deathQuoteLabel = "_deathQuoteLabel";

			/// <summary>
			/// Cached name for the '_mapPointHistory' field.
			/// </summary>
			// Token: 0x04004CE4 RID: 19684
			public static readonly StringName _mapPointHistory = "_mapPointHistory";

			/// <summary>
			/// Cached name for the '_relicHistory' field.
			/// </summary>
			// Token: 0x04004CE5 RID: 19685
			public static readonly StringName _relicHistory = "_relicHistory";

			/// <summary>
			/// Cached name for the '_deckHistory' field.
			/// </summary>
			// Token: 0x04004CE6 RID: 19686
			public static readonly StringName _deckHistory = "_deckHistory";

			/// <summary>
			/// Cached name for the '_outOfDateVisual' field.
			/// </summary>
			// Token: 0x04004CE7 RID: 19687
			public static readonly StringName _outOfDateVisual = "_outOfDateVisual";

			/// <summary>
			/// Cached name for the '_index' field.
			/// </summary>
			// Token: 0x04004CE8 RID: 19688
			public static readonly StringName _index = "_index";

			/// <summary>
			/// Cached name for the '_prevButton' field.
			/// </summary>
			// Token: 0x04004CE9 RID: 19689
			public static readonly StringName _prevButton = "_prevButton";

			/// <summary>
			/// Cached name for the '_nextButton' field.
			/// </summary>
			// Token: 0x04004CEA RID: 19690
			public static readonly StringName _nextButton = "_nextButton";

			/// <summary>
			/// Cached name for the '_selectedPlayerIcon' field.
			/// </summary>
			// Token: 0x04004CEB RID: 19691
			public static readonly StringName _selectedPlayerIcon = "_selectedPlayerIcon";

			/// <summary>
			/// Cached name for the '_screenTween' field.
			/// </summary>
			// Token: 0x04004CEC RID: 19692
			public static readonly StringName _screenTween = "_screenTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200143F RID: 5183
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}

		// Token: 0x02001440 RID: 5184
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04004CED RID: 19693
			[Nullable(0)]
			public static Func<SerializablePotion, PotionModel> <0>__FromSerializable;
		}
	}
}
