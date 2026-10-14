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
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary
{
	/// <summary>
	/// Screen that has a list of monsters that you can click on to view their name, description, hp, some stats, and
	/// a list of their moves which you can click on to play the associated animation and sfx.
	/// </summary>
	// Token: 0x0200035D RID: 861
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiary.cs")]
	public class NBestiary : NSubmenu
	{
		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060031D0 RID: 12752 RVA: 0x00171A4E File Offset: 0x0016FC4E
		// (set) Token: 0x060031D1 RID: 12753 RVA: 0x00171A55 File Offset: 0x0016FC55
		[Nullable(2)]
		public static NBestiary Instance
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060031D2 RID: 12754 RVA: 0x00171A5D File Offset: 0x0016FC5D
		public static string[] AssetPaths
		{
			get
			{
				List<string> list = new List<string>();
				list.Add(NBestiary._scenePath);
				list.AddRange(NBestiaryEntry.AssetPaths);
				return list.ToArray();
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060031D3 RID: 12755 RVA: 0x00171A7F File Offset: 0x0016FC7F
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._bestiaryList.GetChildren(false).OfType<NBestiaryEntry>().FirstOrDefault<NBestiaryEntry>();
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060031D4 RID: 12756 RVA: 0x00171A97 File Offset: 0x0016FC97
		// (set) Token: 0x060031D5 RID: 12757 RVA: 0x00171A9F File Offset: 0x0016FC9F
		public Control BackVfxContainer { get; private set; }

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x060031D6 RID: 12758 RVA: 0x00171AA8 File Offset: 0x0016FCA8
		// (set) Token: 0x060031D7 RID: 12759 RVA: 0x00171AB0 File Offset: 0x0016FCB0
		public Control VfxContainer { get; private set; }

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060031D8 RID: 12760 RVA: 0x00171AB9 File Offset: 0x0016FCB9
		[Nullable(2)]
		public Control Layout
		{
			[NullableContext(2)]
			get
			{
				return this._currentLayout;
			}
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x00171AC1 File Offset: 0x0016FCC1
		[NullableContext(2)]
		public static NBestiary Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NBestiary._scenePath).Instantiate<NBestiary>(0L);
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x00171AE4 File Offset: 0x0016FCE4
		public override void _Ready()
		{
			this.ConnectSignals();
			base.GetNode<MegaLabel>("%MoveHeader").SetTextAutoSize(new LocString("bestiary", "ACTIONS.header").GetFormattedText());
			base.GetNode<MegaRichTextLabel>("%ConstructionLabel").SetTextAutoSize(new LocString("bestiary", "UNDER_CONSTRUCTION").GetRawText());
			this._sidebar = base.GetNode<NScrollableContainer>("%Sidebar");
			this._bestiaryList = base.GetNode<VBoxContainer>("%BestiaryList");
			this._monsterNameLabel = base.GetNode<MegaRichTextLabel>("%MonsterName");
			this._layoutContainer = base.GetNode<Control>("%LayoutContainer");
			this._epithet = base.GetNode<MegaLabel>("%Epithet");
			this._characterIcon = base.GetNode<Control>("%CharacterIcon");
			this._iconTexture = base.GetNode<TextureRect>("%Icon");
			this._iconOutlineTexture = base.GetNode<TextureRect>("%Outline");
			this._dialogueLine = base.GetNode<Control>("%DialogueLine");
			this._dialogueLabel = base.GetNode<MegaRichTextLabel>("%DialogueText");
			this._dialogueBubble = base.GetNode<Control>("%Bubble");
			this._dialogueTail = base.GetNode<TextureRect>("%DialogueTail");
			this._dialogueTailShadow = base.GetNode<TextureRect>("%DialogueTailShadow");
			this._modeButton = base.GetNode<NBestiaryModeButton>("%ModeButton");
			this._modeButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ToggleMode)), 0U);
			this._modeLabel = base.GetNode<MegaLabel>("%ModeLabel");
			this._pageLeftIcon = base.GetNode<TextureRect>("%PageLeftIcon");
			this._pageRightIcon = base.GetNode<TextureRect>("%PageRightIcon");
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdatePageIcons)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdatePageIcons)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdatePageIcons)), 0U);
			this._moveContainer = base.GetNode<Control>("%MoveContainer");
			this._statsContainer = base.GetNode<Control>("%StatsContainer");
			this._selectionArrow = base.GetNode<Control>("%SelectionArrow");
			this._moveList = base.GetNode<Control>("%MoveList");
			this._filterContainer = base.GetNode<Control>("%PoolFilters");
			this._statsLabel = base.GetNode<MegaRichTextLabel>("%StatisticsFull");
			this.VfxContainer = base.GetNode<Control>("%VfxContainer");
			this.BackVfxContainer = base.GetNode<Control>("%BackVfxContainer");
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x00171DF8 File Offset: 0x0016FFF8
		private void ToggleMode(NButton _)
		{
			this._isStatsMode = !this._isStatsMode;
			if (this._isStatsMode)
			{
				this._modeLabel.SetTextAutoSize(new LocString("bestiary", "MODE.viewActions").GetRawText());
				this.ShowStatsPanel();
				this.DisplayCharacterData();
				this.EnableStatsModeHotkeys();
				this.DisableMoveButtonHotkeys();
			}
			else
			{
				this._modeLabel.SetTextAutoSize(new LocString("bestiary", "MODE.viewStats").GetRawText());
				this.ShowMovesPanel();
				this.HideDialogue();
				this.DisableStatsModeHotkeys();
				this.EnableMoveButtonHotkeys();
			}
			this.UpdatePageIcons();
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x00171E94 File Offset: 0x00170094
		private void EnableMoveButtonHotkeys()
		{
			foreach (Node node in this._moveList.GetChildren(false))
			{
				((NBestiaryMoveButton)node).Enable();
			}
		}

		/// <summary>
		/// We need to disable these hotkeys when we go to Stats mode because otherwise
		/// paginating through the characters will make the monster perform actions.
		/// </summary>
		// Token: 0x060031DD RID: 12765 RVA: 0x00171EEC File Offset: 0x001700EC
		private void DisableMoveButtonHotkeys()
		{
			foreach (Node node in this._moveList.GetChildren(false))
			{
				((NBestiaryMoveButton)node).Disable();
			}
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x00171F44 File Offset: 0x00170144
		private void ShowMovesPanel()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._statsContainer.Visible = false;
			this._moveContainer.Visible = true;
			CanvasItem moveContainer = this._moveContainer;
			Color modulate = base.Modulate;
			modulate.A = 0f;
			moveContainer.Modulate = modulate;
			this._tween.TweenProperty(this._moveContainer, "position:x", 242f, 0.5).SetEase(1L).SetTrans(5L)
				.From(210f);
			this._tween.TweenProperty(this._moveContainer, "modulate:a", 1f, 0.5);
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x00172028 File Offset: 0x00170228
		private void ShowStatsPanel()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._moveContainer.Visible = false;
			this._statsContainer.Visible = true;
			CanvasItem statsContainer = this._statsContainer;
			Color modulate = base.Modulate;
			modulate.A = 0f;
			statsContainer.Modulate = modulate;
			this._tween.TweenProperty(this._statsContainer, "position:x", 242f, 0.5).SetEase(1L).SetTrans(5L)
				.From(210f);
			this._tween.TweenProperty(this._statsContainer, "modulate:a", 1f, 0.5);
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x0017210C File Offset: 0x0017030C
		private void RefreshStatisticsText()
		{
			if (this._selectedEntry == null)
			{
				Log.Error("How did this happen?", 2);
				return;
			}
			BestiaryEntry entry = this._selectedEntry.Entry;
			EnemyStats enemyStats = null;
			if (entry.monsterModel != null)
			{
				enemyStats = this._progress.EnemyStats.FirstOrDefault<EnemyStats>((EnemyStats e) => e.Id == entry.monsterModel.Id);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Need to handle special case: ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(entry.encounterModel.Id);
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			foreach (Node node in this._filterContainer.GetChildren(false))
			{
				NBestiaryCharacterFilter filter = node as NBestiaryCharacterFilter;
				if (filter != null && enemyStats != null)
				{
					if (filter.character != null)
					{
						FightStats fightStats = enemyStats.FightStats.FirstOrDefault<FightStats>((FightStats f) => f.Character == filter.character.Id);
						filter.kills = ((fightStats != null) ? fightStats.Wins : 0);
						filter.deaths = ((fightStats != null) ? fightStats.Losses : 0);
					}
					else
					{
						filter.kills = enemyStats.TotalWins;
						filter.deaths = enemyStats.TotalLosses;
					}
					filter.IsLocked = filter.kills + filter.deaths <= 0;
				}
			}
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x001722C8 File Offset: 0x001704C8
		private void DisplayCharacterData()
		{
			if (this._selectedEntry == null)
			{
				Log.Error("How did this happen?", 2);
				return;
			}
			BestiaryEntry entry = this._selectedEntry.Entry;
			if (entry.monsterModel == null)
			{
				return;
			}
			LocString locString = new LocString("bestiary", "STATS.layout");
			if (this._currentFilter.Total == 0)
			{
				locString.Add("total", 0m);
				locString.Add("kills", 0m);
				locString.Add("deaths", 0m);
				locString.Add("winrate", "--");
			}
			else
			{
				locString.Add("total", this._currentFilter.Total);
				locString.Add("kills", this._currentFilter.kills);
				locString.Add("deaths", this._currentFilter.deaths);
				locString.Add("winrate", this._currentFilter.WinRate);
			}
			this._statsLabel.SetTextAutoSize(locString.GetFormattedText());
			if (this._currentFilter.kills <= 0)
			{
				this._dialogueLabel.SetTextAutoSize(this._currentFilter.BestiarySeenQuote);
			}
			else
			{
				LocString bestiaryKillQuote = this._currentFilter.BestiaryKillQuote;
				if (bestiaryKillQuote == null)
				{
					this._dialogueLabel.SetTextAutoSize(new LocString("bestiary", "QUOTE_PLACEHOLDER").GetFormattedText());
				}
				else
				{
					this._dialogueLabel.SetTextAutoSize(bestiaryKillQuote.GetFormattedText());
				}
			}
			if (this._currentFilter.character == null)
			{
				this.HideDialogue();
			}
			else
			{
				this.ShowDialogue();
			}
			this.UpdateDialogueBubbleStyle();
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x00172460 File Offset: 0x00170660
		private void UpdateDialogueBubbleStyle()
		{
			CharacterModel character = this._currentFilter.character;
			Color color = ((character != null) ? character.DialogueColor : StsColors.transparentWhite);
			this._characterIcon.Modulate = ((character == null) ? StsColors.transparentWhite : Colors.White);
			if (character != null)
			{
				this._iconTexture.Texture = character.IconTexture;
				this._iconOutlineTexture.Texture = character.IconOutlineTexture;
			}
			this._dialogueBubble.SelfModulate = color;
			this._dialogueTail.SelfModulate = color;
			string text = ((character is Silent) ? "res://images/ui/thought_tail.png" : "res://images/ui/dialogue_tail.png");
			this._dialogueTail.Texture = PreloadManager.Cache.GetCompressedTexture2D(text);
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x0017250C File Offset: 0x0017070C
		private void ShowDialogue()
		{
			Tween dialogueTween = this._dialogueTween;
			if (dialogueTween != null)
			{
				dialogueTween.Kill();
			}
			this._dialogueTween = base.CreateTween().SetParallel(true);
			this._dialogueLine.Modulate = StsColors.transparentWhite;
			this._dialogueTween.TweenProperty(this._dialogueLine, "modulate", Colors.White, 0.1).SetDelay(0.1);
			this._dialogueTween.TweenProperty(this._dialogueLine, "position:x", 560f, 0.4).SetEase(1L).SetTrans(5L)
				.From(528f)
				.SetDelay(0.1);
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x001725E4 File Offset: 0x001707E4
		private void HideDialogue()
		{
			Tween dialogueTween = this._dialogueTween;
			if (dialogueTween != null)
			{
				dialogueTween.Kill();
			}
			this._dialogueTween = base.CreateTween();
			this._dialogueTween.TweenProperty(this._dialogueLine, "modulate", StsColors.transparentWhite, 0.1);
		}

		/// <summary>
		/// On screen open. When the player opens the Bestiary.
		/// </summary>
		// Token: 0x060031E5 RID: 12773 RVA: 0x00172640 File Offset: 0x00170840
		public override void OnSubmenuOpened()
		{
			NBestiary.Instance = this;
			this._progress = SaveManager.Instance.Progress.ToSerializable();
			NGame instance = NGame.Instance;
			this._previousScreenshakeTarget = ((instance != null) ? instance.ScreenshakeTarget : null);
			this._isStatsMode = !SaveManager.Instance.PrefsSave.IsBestiaryActionsPreferred;
			if (this._isStatsMode)
			{
				this._modeLabel.SetTextAutoSize(new LocString("bestiary", "MODE.viewActions").GetRawText());
			}
			else
			{
				this._modeLabel.SetTextAutoSize(new LocString("bestiary", "MODE.viewStats").GetRawText());
			}
			this.CreateFilters();
			this.CreateEntries();
			if (this._isStatsMode)
			{
				this.DisplayCharacterData();
				this.EnableStatsModeHotkeys();
				this.DisableMoveButtonHotkeys();
			}
			this.UpdatePageIcons();
		}

		/// <summary>
		/// Called when the Bestiary is closed (Back button)
		/// </summary>
		// Token: 0x060031E6 RID: 12774 RVA: 0x0017270C File Offset: 0x0017090C
		public override void OnSubmenuClosed()
		{
			this.DisableStatsModeHotkeys();
			this.DisableMoveButtonHotkeys();
			this._initSelectionArrow = true;
			this._selectedEntry = null;
			NBestiary.Instance = null;
			SaveManager.Instance.PrefsSave.IsBestiaryActionsPreferred = !this._isStatsMode;
			SaveManager.Instance.SavePrefsFile();
			NBestiaryLayout currentLayout = this._currentLayout;
			if (currentLayout != null)
			{
				currentLayout.Cleanup();
			}
			if (this._previousScreenshakeTarget != null)
			{
				if (this._previousScreenshakeTarget.IsValid())
				{
					NGame instance = NGame.Instance;
					if (instance != null)
					{
						instance.SetScreenShakeTarget(this._previousScreenshakeTarget);
					}
				}
				else
				{
					Log.Warn("The screenshake target is no longer valid. This should never happen.", 2);
					this._previousScreenshakeTarget = null;
				}
			}
			else
			{
				NGame instance2 = NGame.Instance;
				if (instance2 != null)
				{
					instance2.ClearScreenShakeTarget();
				}
			}
			this._bestiaryList.FreeChildren();
			this._lastFocusedControl = null;
		}

		/// <summary>
		/// Initializes the list of monsters based on your save file.
		/// </summary>
		// Token: 0x060031E7 RID: 12775 RVA: 0x001727D0 File Offset: 0x001709D0
		private void CreateEntries()
		{
			this._discoveredMonsterIds = (from e in SaveManager.Instance.Progress.EnemyStats.Values
				where e.TotalWins > 0
				select e.Id).ToHashSet<ModelId>();
			this._discoveredEncounterIds = (from e in SaveManager.Instance.Progress.EncounterStats.Values
				where e.TotalWins > 0
				select e.Id).ToHashSet<ModelId>();
			foreach (ActModel actModel in ModelDb.Acts)
			{
				this.AddAct(actModel);
			}
			this.AddEvents();
			Control node = this._sidebar.GetNode<Control>("Content");
			Control control = node;
			Vector2 position = node.Position;
			position.Y = 0f;
			control.Position = position;
			this._sidebar.InstantlyScrollToTop();
			NBestiaryEntry nbestiaryEntry = this._bestiaryList.GetChildren(false).OfType<NBestiaryEntry>().FirstOrDefault<NBestiaryEntry>((NBestiaryEntry e) => e.IsDiscovered && e.IsEnabled);
			if (nbestiaryEntry == null)
			{
				Log.Error("Should not be possible as the Compendium + Bestiary isn't unlocked by default!", 2);
				return;
			}
			this.SelectMonster(nbestiaryEntry);
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x0017297C File Offset: 0x00170B7C
		private void CreateFilters()
		{
			this._filterContainer.FreeChildren();
			this.AddFilter(null);
			this.AddFilter(ModelDb.Character<Ironclad>());
			this.AddFilter(ModelDb.Character<Silent>());
			this.AddFilter(ModelDb.Character<Regent>());
			this.AddFilter(ModelDb.Character<Necrobinder>());
			this.AddFilter(ModelDb.Character<Defect>());
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x001729D4 File Offset: 0x00170BD4
		[NullableContext(2)]
		private void AddFilter(CharacterModel character)
		{
			NBestiaryCharacterFilter nbestiaryCharacterFilter = NBestiaryCharacterFilter.Create(character);
			nbestiaryCharacterFilter.Connect(NBestiaryCharacterFilter.SignalName.Toggled, Callable.From<NBestiaryCharacterFilter>(new Action<NBestiaryCharacterFilter>(this.OnCharacterFilterSelected)), 0U);
			this._filterContainer.AddChildSafely(nbestiaryCharacterFilter);
			if (character == null)
			{
				this._currentFilter = nbestiaryCharacterFilter;
				this._currentFilter.IsSelected = true;
			}
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x00172A28 File Offset: 0x00170C28
		private void OnCharacterFilterSelected(NBestiaryCharacterFilter selectedFilter)
		{
			this._currentFilter = selectedFilter;
			foreach (Node node in this._filterContainer.GetChildren(false))
			{
				if (!node.Equals(selectedFilter))
				{
					((NBestiaryCharacterFilter)node).Deselect();
				}
			}
			if (this._isStatsMode)
			{
				this.DisplayCharacterData();
			}
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x00172AA0 File Offset: 0x00170CA0
		private void AddAct(ActModel act)
		{
			if (!SaveManager.Instance.Progress.DiscoveredActs.Contains(act.Id))
			{
				return;
			}
			this._bestiaryList.AddChildSafely(NBestiaryLabelDivider.Create(act));
			HashSet<ModelId> hashSet = new HashSet<ModelId>();
			List<BestiaryEntry> list = new List<BestiaryEntry>();
			foreach (EncounterModel encounterModel in act.AllEncounters)
			{
				foreach (MonsterModel monsterModel in encounterModel.AllPossibleMonsters)
				{
					if (hashSet.Add(monsterModel.Id) && monsterModel.ShouldShowInCompendium)
					{
						list.Add(BestiaryEntry.FromMonster(monsterModel, encounterModel, encounterModel.RoomType));
					}
				}
			}
			if (act is Hive)
			{
				list.Add(BestiaryEntry.FromEncounter(ModelDb.Encounter<DecimillipedeElite>(), RoomType.Elite));
			}
			this.AddEntries(list);
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x00172BA8 File Offset: 0x00170DA8
		private void AddEvents()
		{
			this._bestiaryList.AddChildSafely(NBestiaryLabelDivider.Create(new LocString("bestiary", "EVENTS.title")));
			HashSet<ModelId> hashSet = new HashSet<ModelId>();
			List<BestiaryEntry> list = new List<BestiaryEntry>();
			foreach (EncounterModel encounterModel in ModelDb.EventEncounters)
			{
				foreach (MonsterModel monsterModel in encounterModel.AllPossibleMonsters)
				{
					if (hashSet.Add(monsterModel.Id) && monsterModel.ShouldShowInCompendium)
					{
						list.Add(BestiaryEntry.FromMonster(monsterModel, encounterModel, encounterModel.RoomType));
					}
				}
			}
			this.AddEntries(list);
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x00172C88 File Offset: 0x00170E88
		private void AddEntries(List<BestiaryEntry> entries)
		{
			entries.Sort(delegate(BestiaryEntry e1, BestiaryEntry e2)
			{
				if (e1.roomType != e2.roomType)
				{
					return e1.roomType.CompareTo(e2.roomType);
				}
				if (e1.roomType == RoomType.Boss)
				{
					int num = string.Compare(e1.GetEncounterTitle(), e2.GetEncounterTitle(), StringComparison.CurrentCulture);
					if (num != 0)
					{
						return num;
					}
				}
				return string.Compare(e1.GetEntryTitle(), e2.GetEntryTitle(), StringComparison.CurrentCulture);
			});
			foreach (BestiaryEntry bestiaryEntry in entries)
			{
				NBestiaryEntry nbestiaryEntry = NBestiaryEntry.Create(bestiaryEntry, bestiaryEntry.IsDiscovered(this._discoveredMonsterIds, this._discoveredEncounterIds));
				this._bestiaryList.AddChildSafely(nbestiaryEntry);
				nbestiaryEntry.Connect(NClickableControl.SignalName.Released, Callable.From<NBestiaryEntry>(new Action<NBestiaryEntry>(this.OnMonsterClicked)), 0U);
			}
		}

		/// <summary>
		/// A player clicked on a monster in the list on the right.
		/// </summary>
		// Token: 0x060031EE RID: 12782 RVA: 0x00172D38 File Offset: 0x00170F38
		private void OnMonsterClicked(NBestiaryEntry entry)
		{
			this.SelectMonster(entry);
		}

		/// <summary>
		/// Loads a specific monster's bestiary entry.
		/// </summary>
		// Token: 0x060031EF RID: 12783 RVA: 0x00172D44 File Offset: 0x00170F44
		private void SelectMonster(NBestiaryEntry entry)
		{
			if (entry == this._selectedEntry)
			{
				return;
			}
			this._moveList.FreeChildren();
			this._selectedEntry = entry;
			if (entry.IsUnderConstruction)
			{
				this._monsterNameLabel.Text = entry.Entry.GetEntryTitle();
				NBestiaryLayout currentLayout = this._currentLayout;
				if (currentLayout != null)
				{
					currentLayout.Cleanup();
				}
				NBestiaryLayout currentLayout2 = this._currentLayout;
				if (currentLayout2 != null)
				{
					currentLayout2.QueueFreeSafely();
				}
			}
			else if (!entry.IsDiscovered)
			{
				this._monsterNameLabel.Text = NBestiary._locked.GetFormattedText();
				NBestiaryLayout currentLayout3 = this._currentLayout;
				if (currentLayout3 != null)
				{
					currentLayout3.Cleanup();
				}
				NBestiaryLayout currentLayout4 = this._currentLayout;
				if (currentLayout4 != null)
				{
					currentLayout4.QueueFreeSafely();
				}
			}
			else
			{
				Tween tween = this._tween;
				if (tween != null)
				{
					tween.Kill();
				}
				this._tween = base.CreateTween().SetParallel(true);
				this._monsterNameLabel.Text = entry.Entry.GetEntryTitle();
				this._monsterNameLabel.SelfModulate = StsColors.transparentWhite;
				this._epithet.Modulate = StsColors.transparentWhite;
				this._tween.TweenProperty(this._monsterNameLabel, "position:y", 88f, 0.5).SetEase(1L).SetTrans(5L)
					.From(24f);
				this._tween.TweenProperty(this._monsterNameLabel, "self_modulate:a", 1f, 0.5);
				this._tween.TweenProperty(this._epithet, "modulate:a", 1f, 0.5).SetDelay(0.2);
				this._tween.TweenProperty(this._dialogueLabel, "position:y", 894f, 0.5).SetEase(1L).SetTrans(5L)
					.From(958f);
				this._tween.TweenProperty(this._dialogueLabel, "modulate:a", 1f, 0.5);
				if (this._isStatsMode)
				{
					this.ShowStatsPanel();
				}
				else
				{
					this.ShowMovesPanel();
				}
				this.RefreshStatisticsText();
				NBestiaryLayout currentLayout5 = this._currentLayout;
				if (currentLayout5 != null)
				{
					currentLayout5.Cleanup();
				}
				if (!entry.Entry.CanReuseLayout(this._currentLayout))
				{
					NBestiaryLayout currentLayout6 = this._currentLayout;
					if (currentLayout6 != null)
					{
						currentLayout6.QueueFreeSafely();
					}
					this._currentLayout = entry.Entry.CreateLayoutNode(this);
					this._layoutContainer.AddChildSafely(this._currentLayout);
					NGame instance = NGame.Instance;
					if (instance != null)
					{
						instance.SetScreenShakeTarget(this._currentLayout);
					}
				}
				List<BestiaryMonsterMove> list = this._currentLayout.Setup(entry.Entry, this._tween);
				for (int i = 0; i < list.Count; i++)
				{
					if (i >= 9)
					{
						Log.Error("Hotkeys for monster Actions beyond 9 are not supported!", 2);
					}
					NBestiaryMoveButton nbestiaryMoveButton = this.CreateBestiaryMoveButton(list[i], i + 1);
					this._moveList.AddChildSafely(nbestiaryMoveButton);
					nbestiaryMoveButton.Connect(NClickableControl.SignalName.Released, Callable.From<NBestiaryMoveButton>(new Action<NBestiaryMoveButton>(this.OnMoveButtonClicked)), 0U);
				}
				if (this._isStatsMode)
				{
					this.DisableMoveButtonHotkeys();
					if (this._currentFilter.IsLocked)
					{
						NBestiaryCharacterFilter child = this._filterContainer.GetChild<NBestiaryCharacterFilter>(0, false);
						child.IsSelected = true;
						this.OnCharacterFilterSelected(child);
					}
					this.DisplayCharacterData();
				}
			}
			Color color;
			if (this._initSelectionArrow)
			{
				CanvasItem selectionArrow = this._selectionArrow;
				color = this._selectionArrow.Modulate;
				color.A = 0f;
				selectionArrow.Modulate = color;
				this._initSelectionArrow = false;
				TaskHelper.RunSafely(this.InitializeSelectorArrow(entry));
				return;
			}
			CanvasItem selectionArrow2 = this._selectionArrow;
			color = this._selectionArrow.Modulate;
			color.A = 1f;
			selectionArrow2.Modulate = color;
			Tween arrowTween = this._arrowTween;
			if (arrowTween != null)
			{
				arrowTween.Kill();
			}
			this._arrowTween = base.CreateTween().SetParallel(true);
			this._arrowTween.TweenProperty(this._selectionArrow, "position", entry.Position + NBestiary._arrowOffset, 0.25).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x00173198 File Offset: 0x00171398
		private NBestiaryMoveButton CreateBestiaryMoveButton(BestiaryMonsterMove move, int moveIndex)
		{
			NControllerManager instance = NControllerManager.Instance;
			bool? flag = ((instance != null) ? new bool?(instance.IsUsingController) : null);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (flag != null && flag.GetValueOrDefault())
			{
				NBestiaryMoveButton nbestiaryMoveButton;
				switch (moveIndex)
				{
				case 1:
					nbestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.viewDeckAndTabLeft);
					break;
				case 2:
					nbestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.viewExhaustPileAndTabRight);
					break;
				case 3:
					nbestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.viewDiscardPile);
					break;
				case 4:
					nbestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.viewDrawPile);
					break;
				case 5:
					nbestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.topPanel);
					break;
				case 6:
					nbestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.altUp);
					break;
				case 7:
					nbestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.altDown);
					break;
				case 8:
					nbestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.altLeft);
					break;
				case 9:
					nbestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.altRight);
					break;
				default:
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
					defaultInterpolatedStringHandler.AppendFormatted<int>(moveIndex);
					nbestiaryMoveButton = NBestiaryMoveButton.Create(move, defaultInterpolatedStringHandler.ToStringAndClear());
					break;
				}
				return nbestiaryMoveButton;
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
			defaultInterpolatedStringHandler.AppendLiteral("mega_select_card_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(moveIndex);
			return NBestiaryMoveButton.Create(move, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x001732E8 File Offset: 0x001714E8
		private async Task InitializeSelectorArrow(NBestiaryEntry entry)
		{
			await base.ToSignal(base.GetTree(), SceneTree.SignalName.ProcessFrame);
			this._selectionArrow.Position = entry.Position + NBestiary._arrowOffset;
			Tween arrowTween = this._arrowTween;
			if (arrowTween != null)
			{
				arrowTween.Kill();
			}
			this._arrowTween = base.CreateTween().SetParallel(true);
			this._arrowTween.TweenProperty(this._selectionArrow, "modulate:a", 1f, 0.2);
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x00173334 File Offset: 0x00171534
		private void OnMoveButtonClicked(NButton button)
		{
			NBestiaryMoveButton nbestiaryMoveButton = (NBestiaryMoveButton)button;
			NBestiaryLayout currentLayout = this._currentLayout;
			NBestiary.PlayMoveAnim(((currentLayout != null) ? currentLayout.GetCreatures() : null) ?? Array.Empty<NCreature>(), nbestiaryMoveButton.Move);
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x00173370 File Offset: 0x00171570
		private static void PlayMoveAnim(IEnumerable<NCreature> creatures, BestiaryMonsterMove move)
		{
			foreach (NCreature ncreature in creatures)
			{
				if (move.stateId != null)
				{
					MonsterModel monster = ncreature.Entity.Monster;
					if (monster == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Non-monster creature ");
						defaultInterpolatedStringHandler.AppendFormatted<NCreature>(ncreature);
						defaultInterpolatedStringHandler.AppendLiteral(" is in the bestiary!");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					monster.SetMoveImmediate((MoveState)monster.MoveStateMachine.States[move.stateId], true);
					TaskHelper.RunSafely(monster.PerformMove());
				}
				else if (move.nonStateMove != null)
				{
					TaskHelper.RunSafely(move.nonStateMove(Array.Empty<Creature>()));
				}
				else if (move.action != null)
				{
					TaskHelper.RunSafely(move.action());
				}
				else if (move.animId != null)
				{
					MegaSprite spineBody = ncreature.Visuals.SpineBody;
					if (spineBody != null)
					{
						spineBody.GetAnimationState().SetAnimation(move.animId, false, 0);
					}
					if (move.animId != "die")
					{
						MegaSprite spineBody2 = ncreature.Visuals.SpineBody;
						if (spineBody2 != null)
						{
							spineBody2.GetAnimationState().AddAnimation("idle_loop", 0f, true, 0);
						}
					}
					if (move.sfx != null)
					{
						NAudioManager.Instance.PlayOneShot(move.sfx, 1f);
					}
				}
				if (move.stopSfxLoops)
				{
					ncreature.StopAllSfxLoops();
				}
			}
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x00173514 File Offset: 0x00171714
		[NullableContext(2)]
		public NCreature GetCreatureNode(Creature creature)
		{
			NBestiaryLayout currentLayout = this._currentLayout;
			foreach (NCreature ncreature in (((currentLayout != null) ? currentLayout.GetCreatures() : null) ?? Array.Empty<NCreature>()))
			{
				if (ncreature.Entity == creature)
				{
					return ncreature;
				}
			}
			return null;
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x00173580 File Offset: 0x00171780
		public Vector2 GetSideCenter()
		{
			if (this._currentLayout == null)
			{
				Log.Error("Tried to get current side center, but we're not showing anything!", 2);
				return Vector2.Zero;
			}
			Vector2 vector = Vector2.Zero;
			int num = 0;
			foreach (NCreature ncreature in this._currentLayout.GetCreatures())
			{
				vector += ncreature.VfxSpawnPosition;
				num++;
			}
			return vector / (float)num;
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x00173608 File Offset: 0x00171808
		public Vector2 GetSideFloor()
		{
			if (this._currentLayout == null)
			{
				Log.Error("Tried to get current side floor, but we're not showing anything!", 2);
				return Vector2.Zero;
			}
			Vector2 vector = Vector2.Zero;
			int num = 0;
			foreach (NCreature ncreature in this._currentLayout.GetCreatures())
			{
				vector += ncreature.GetBottomOfHitbox();
				num++;
			}
			return vector / (float)num;
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x00173690 File Offset: 0x00171890
		private void EnableStatsModeHotkeys()
		{
			NHotkeyManager.Instance.PushHotkeyPressedBinding(NBestiary._filterLeftHotkey, new Action(this.FilterLeft));
			NHotkeyManager.Instance.PushHotkeyPressedBinding(NBestiary._filterRightHotkey, new Action(this.FilterRight));
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x001736E0 File Offset: 0x001718E0
		private void DisableStatsModeHotkeys()
		{
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(NBestiary._filterLeftHotkey, new Action(this.FilterLeft));
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(NBestiary._filterRightHotkey, new Action(this.FilterRight));
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x00173730 File Offset: 0x00171930
		private void FilterLeft()
		{
			List<NBestiaryCharacterFilter> list = this._filterContainer.GetChildren(false).OfType<NBestiaryCharacterFilter>().ToList<NBestiaryCharacterFilter>();
			int num = list.IndexOf(this._currentFilter);
			for (int i = 1; i <= list.Count; i++)
			{
				int num2 = (num - i + list.Count) % list.Count;
				if (!list[num2].IsLocked)
				{
					this.SelectFilter(list[num2]);
					return;
				}
			}
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x001737A0 File Offset: 0x001719A0
		private void FilterRight()
		{
			List<NBestiaryCharacterFilter> list = this._filterContainer.GetChildren(false).OfType<NBestiaryCharacterFilter>().ToList<NBestiaryCharacterFilter>();
			int num = list.IndexOf(this._currentFilter);
			for (int i = 1; i <= list.Count; i++)
			{
				int num2 = (num + i) % list.Count;
				if (!list[num2].IsLocked)
				{
					this.SelectFilter(list[num2]);
					return;
				}
			}
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x00173809 File Offset: 0x00171A09
		private void SelectFilter(NBestiaryCharacterFilter filter)
		{
			filter.IsSelected = true;
			this.OnCharacterFilterSelected(filter);
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x0017381C File Offset: 0x00171A1C
		private void UpdatePageIcons()
		{
			bool flag = this._isStatsMode && NControllerManager.Instance.IsUsingController;
			this._pageLeftIcon.Visible = flag;
			this._pageRightIcon.Visible = flag;
			if (flag)
			{
				this._pageLeftIcon.Texture = NInputManager.Instance.GetHotkeyIcon(MegaInput.viewDeckAndTabLeft);
				this._pageRightIcon.Texture = NInputManager.Instance.GetHotkeyIcon(MegaInput.viewExhaustPileAndTabRight);
			}
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x00173898 File Offset: 0x00171A98
		public static bool CanBeShown()
		{
			if (SaveManager.Instance.Progress.DiscoveredActs.Count == 0)
			{
				return false;
			}
			return SaveManager.Instance.Progress.EnemyStats.Values.Any<EnemyStats>((EnemyStats e) => e.TotalWins > 0);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060031FE RID: 12798 RVA: 0x001738F8 File Offset: 0x00171AF8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(30)
			{
				new MethodInfo(NBestiary.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NBestiary.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.ToggleMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NBestiary.MethodName.EnableMoveButtonHotkeys, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.DisableMoveButtonHotkeys, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.ShowMovesPanel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.ShowStatsPanel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.RefreshStatisticsText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.DisplayCharacterData, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.UpdateDialogueBubbleStyle, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.ShowDialogue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.HideDialogue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.CreateEntries, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.CreateFilters, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.OnCharacterFilterSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "selectedFilter", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NBestiary.MethodName.AddEvents, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.OnMonsterClicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "entry", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NBestiary.MethodName.SelectMonster, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "entry", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NBestiary.MethodName.OnMoveButtonClicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NBestiary.MethodName.GetSideCenter, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.GetSideFloor, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.EnableStatsModeHotkeys, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.DisableStatsModeHotkeys, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.FilterLeft, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.FilterRight, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.SelectFilter, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "filter", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NBestiary.MethodName.UpdatePageIcons, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiary.MethodName.CanBeShown, new PropertyInfo(1L, "", 0L, "", 6L, false), 33L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060031FF RID: 12799 RVA: 0x00173FC0 File Offset: 0x001721C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiary.MethodName.Create && args.Count == 0)
			{
				NBestiary nbestiary = NBestiary.Create();
				ret = VariantUtils.CreateFrom<NBestiary>(ref nbestiary);
				return true;
			}
			if ((ref method) == NBestiary.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.ToggleMode && args.Count == 1)
			{
				this.ToggleMode(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.EnableMoveButtonHotkeys && args.Count == 0)
			{
				this.EnableMoveButtonHotkeys();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.DisableMoveButtonHotkeys && args.Count == 0)
			{
				this.DisableMoveButtonHotkeys();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.ShowMovesPanel && args.Count == 0)
			{
				this.ShowMovesPanel();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.ShowStatsPanel && args.Count == 0)
			{
				this.ShowStatsPanel();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.RefreshStatisticsText && args.Count == 0)
			{
				this.RefreshStatisticsText();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.DisplayCharacterData && args.Count == 0)
			{
				this.DisplayCharacterData();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.UpdateDialogueBubbleStyle && args.Count == 0)
			{
				this.UpdateDialogueBubbleStyle();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.ShowDialogue && args.Count == 0)
			{
				this.ShowDialogue();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.HideDialogue && args.Count == 0)
			{
				this.HideDialogue();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.CreateEntries && args.Count == 0)
			{
				this.CreateEntries();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.CreateFilters && args.Count == 0)
			{
				this.CreateFilters();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.OnCharacterFilterSelected && args.Count == 1)
			{
				this.OnCharacterFilterSelected(VariantUtils.ConvertTo<NBestiaryCharacterFilter>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.AddEvents && args.Count == 0)
			{
				this.AddEvents();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.OnMonsterClicked && args.Count == 1)
			{
				this.OnMonsterClicked(VariantUtils.ConvertTo<NBestiaryEntry>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.SelectMonster && args.Count == 1)
			{
				this.SelectMonster(VariantUtils.ConvertTo<NBestiaryEntry>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.OnMoveButtonClicked && args.Count == 1)
			{
				this.OnMoveButtonClicked(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.GetSideCenter && args.Count == 0)
			{
				Vector2 sideCenter = this.GetSideCenter();
				ret = VariantUtils.CreateFrom<Vector2>(ref sideCenter);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.GetSideFloor && args.Count == 0)
			{
				Vector2 sideFloor = this.GetSideFloor();
				ret = VariantUtils.CreateFrom<Vector2>(ref sideFloor);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.EnableStatsModeHotkeys && args.Count == 0)
			{
				this.EnableStatsModeHotkeys();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.DisableStatsModeHotkeys && args.Count == 0)
			{
				this.DisableStatsModeHotkeys();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.FilterLeft && args.Count == 0)
			{
				this.FilterLeft();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.FilterRight && args.Count == 0)
			{
				this.FilterRight();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.SelectFilter && args.Count == 1)
			{
				this.SelectFilter(VariantUtils.ConvertTo<NBestiaryCharacterFilter>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.UpdatePageIcons && args.Count == 0)
			{
				this.UpdatePageIcons();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.CanBeShown && args.Count == 0)
			{
				bool flag = NBestiary.CanBeShown();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06003200 RID: 12800 RVA: 0x0017449C File Offset: 0x0017269C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiary.MethodName.Create && args.Count == 0)
			{
				NBestiary nbestiary = NBestiary.Create();
				ret = VariantUtils.CreateFrom<NBestiary>(ref nbestiary);
				return true;
			}
			if ((ref method) == NBestiary.MethodName.CanBeShown && args.Count == 0)
			{
				bool flag = NBestiary.CanBeShown();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003201 RID: 12801 RVA: 0x00174508 File Offset: 0x00172708
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBestiary.MethodName.Create || (ref method) == NBestiary.MethodName._Ready || (ref method) == NBestiary.MethodName.ToggleMode || (ref method) == NBestiary.MethodName.EnableMoveButtonHotkeys || (ref method) == NBestiary.MethodName.DisableMoveButtonHotkeys || (ref method) == NBestiary.MethodName.ShowMovesPanel || (ref method) == NBestiary.MethodName.ShowStatsPanel || (ref method) == NBestiary.MethodName.RefreshStatisticsText || (ref method) == NBestiary.MethodName.DisplayCharacterData || (ref method) == NBestiary.MethodName.UpdateDialogueBubbleStyle || (ref method) == NBestiary.MethodName.ShowDialogue || (ref method) == NBestiary.MethodName.HideDialogue || (ref method) == NBestiary.MethodName.OnSubmenuOpened || (ref method) == NBestiary.MethodName.OnSubmenuClosed || (ref method) == NBestiary.MethodName.CreateEntries || (ref method) == NBestiary.MethodName.CreateFilters || (ref method) == NBestiary.MethodName.OnCharacterFilterSelected || (ref method) == NBestiary.MethodName.AddEvents || (ref method) == NBestiary.MethodName.OnMonsterClicked || (ref method) == NBestiary.MethodName.SelectMonster || (ref method) == NBestiary.MethodName.OnMoveButtonClicked || (ref method) == NBestiary.MethodName.GetSideCenter || (ref method) == NBestiary.MethodName.GetSideFloor || (ref method) == NBestiary.MethodName.EnableStatsModeHotkeys || (ref method) == NBestiary.MethodName.DisableStatsModeHotkeys || (ref method) == NBestiary.MethodName.FilterLeft || (ref method) == NBestiary.MethodName.FilterRight || (ref method) == NBestiary.MethodName.SelectFilter || (ref method) == NBestiary.MethodName.UpdatePageIcons || (ref method) == NBestiary.MethodName.CanBeShown || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003202 RID: 12802 RVA: 0x001746E0 File Offset: 0x001728E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBestiary.PropertyName.BackVfxContainer)
			{
				this.BackVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName.VfxContainer)
			{
				this.VfxContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._monsterNameLabel)
			{
				this._monsterNameLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._epithet)
			{
				this._epithet = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._sidebar)
			{
				this._sidebar = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._bestiaryList)
			{
				this._bestiaryList = VariantUtils.ConvertTo<VBoxContainer>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._selectionArrow)
			{
				this._selectionArrow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._arrowTween)
			{
				this._arrowTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._initSelectionArrow)
			{
				this._initSelectionArrow = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._layoutContainer)
			{
				this._layoutContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._currentLayout)
			{
				this._currentLayout = VariantUtils.ConvertTo<NBestiaryLayout>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._characterIcon)
			{
				this._characterIcon = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._iconTexture)
			{
				this._iconTexture = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._iconOutlineTexture)
			{
				this._iconOutlineTexture = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueLine)
			{
				this._dialogueLine = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueLabel)
			{
				this._dialogueLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueBubble)
			{
				this._dialogueBubble = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueTail)
			{
				this._dialogueTail = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueTailShadow)
			{
				this._dialogueTailShadow = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._modeButton)
			{
				this._modeButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._modeLabel)
			{
				this._modeLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._isStatsMode)
			{
				this._isStatsMode = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._pageLeftIcon)
			{
				this._pageLeftIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._pageRightIcon)
			{
				this._pageRightIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._moveList)
			{
				this._moveList = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._moveContainer)
			{
				this._moveContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._statsContainer)
			{
				this._statsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._filterContainer)
			{
				this._filterContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._statsLabel)
			{
				this._statsLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._currentFilter)
			{
				this._currentFilter = VariantUtils.ConvertTo<NBestiaryCharacterFilter>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._selectedEntry)
			{
				this._selectedEntry = VariantUtils.ConvertTo<NBestiaryEntry>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._previousScreenshakeTarget)
			{
				this._previousScreenshakeTarget = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueTween)
			{
				this._dialogueTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003203 RID: 12803 RVA: 0x00174A8C File Offset: 0x00172C8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBestiary.PropertyName.InitialFocusedControl)
			{
				Control control = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName.BackVfxContainer)
			{
				Control control = this.BackVfxContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName.VfxContainer)
			{
				Control control = this.VfxContainer;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName.Layout)
			{
				Control control = this.Layout;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._monsterNameLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._monsterNameLabel);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._epithet)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._epithet);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._sidebar)
			{
				value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._sidebar);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._bestiaryList)
			{
				value = VariantUtils.CreateFrom<VBoxContainer>(ref this._bestiaryList);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._selectionArrow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._selectionArrow);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._arrowTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._arrowTween);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._initSelectionArrow)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._initSelectionArrow);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._layoutContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._layoutContainer);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._currentLayout)
			{
				value = VariantUtils.CreateFrom<NBestiaryLayout>(ref this._currentLayout);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._characterIcon)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._characterIcon);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._iconTexture)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._iconTexture);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._iconOutlineTexture)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._iconOutlineTexture);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueLine)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._dialogueLine);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._dialogueLabel);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueBubble)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._dialogueBubble);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueTail)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._dialogueTail);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueTailShadow)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._dialogueTailShadow);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._modeButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._modeButton);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._modeLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._modeLabel);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._isStatsMode)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isStatsMode);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._pageLeftIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._pageLeftIcon);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._pageRightIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._pageRightIcon);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._moveList)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._moveList);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._moveContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._moveContainer);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._statsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._statsContainer);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._filterContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._filterContainer);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._statsLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._statsLabel);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._currentFilter)
			{
				value = VariantUtils.CreateFrom<NBestiaryCharacterFilter>(ref this._currentFilter);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._selectedEntry)
			{
				value = VariantUtils.CreateFrom<NBestiaryEntry>(ref this._selectedEntry);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._previousScreenshakeTarget)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._previousScreenshakeTarget);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NBestiary.PropertyName._dialogueTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._dialogueTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003204 RID: 12804 RVA: 0x00174F30 File Offset: 0x00173130
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NBestiary.PropertyName._monsterNameLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._epithet, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName.InitialFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._sidebar, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._bestiaryList, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._selectionArrow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._arrowTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NBestiary.PropertyName._initSelectionArrow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._layoutContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._currentLayout, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._characterIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._iconTexture, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._iconOutlineTexture, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._dialogueLine, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._dialogueLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._dialogueBubble, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._dialogueTail, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._dialogueTailShadow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._modeButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._modeLabel, 0L, "", 4096L, false),
				new PropertyInfo(1L, NBestiary.PropertyName._isStatsMode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._pageLeftIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._pageRightIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._moveList, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._moveContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._statsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._filterContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._statsLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._currentFilter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._selectedEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._previousScreenshakeTarget, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName._dialogueTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName.BackVfxContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName.VfxContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiary.PropertyName.Layout, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003205 RID: 12805 RVA: 0x001753E8 File Offset: 0x001735E8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName backVfxContainer = NBestiary.PropertyName.BackVfxContainer;
			Control control = this.BackVfxContainer;
			info.AddProperty(backVfxContainer, Variant.From<Control>(ref control));
			StringName vfxContainer = NBestiary.PropertyName.VfxContainer;
			control = this.VfxContainer;
			info.AddProperty(vfxContainer, Variant.From<Control>(ref control));
			info.AddProperty(NBestiary.PropertyName._monsterNameLabel, Variant.From<MegaRichTextLabel>(ref this._monsterNameLabel));
			info.AddProperty(NBestiary.PropertyName._epithet, Variant.From<MegaLabel>(ref this._epithet));
			info.AddProperty(NBestiary.PropertyName._sidebar, Variant.From<NScrollableContainer>(ref this._sidebar));
			info.AddProperty(NBestiary.PropertyName._bestiaryList, Variant.From<VBoxContainer>(ref this._bestiaryList));
			info.AddProperty(NBestiary.PropertyName._selectionArrow, Variant.From<Control>(ref this._selectionArrow));
			info.AddProperty(NBestiary.PropertyName._arrowTween, Variant.From<Tween>(ref this._arrowTween));
			info.AddProperty(NBestiary.PropertyName._initSelectionArrow, Variant.From<bool>(ref this._initSelectionArrow));
			info.AddProperty(NBestiary.PropertyName._layoutContainer, Variant.From<Control>(ref this._layoutContainer));
			info.AddProperty(NBestiary.PropertyName._currentLayout, Variant.From<NBestiaryLayout>(ref this._currentLayout));
			info.AddProperty(NBestiary.PropertyName._characterIcon, Variant.From<Control>(ref this._characterIcon));
			info.AddProperty(NBestiary.PropertyName._iconTexture, Variant.From<TextureRect>(ref this._iconTexture));
			info.AddProperty(NBestiary.PropertyName._iconOutlineTexture, Variant.From<TextureRect>(ref this._iconOutlineTexture));
			info.AddProperty(NBestiary.PropertyName._dialogueLine, Variant.From<Control>(ref this._dialogueLine));
			info.AddProperty(NBestiary.PropertyName._dialogueLabel, Variant.From<MegaRichTextLabel>(ref this._dialogueLabel));
			info.AddProperty(NBestiary.PropertyName._dialogueBubble, Variant.From<Control>(ref this._dialogueBubble));
			info.AddProperty(NBestiary.PropertyName._dialogueTail, Variant.From<TextureRect>(ref this._dialogueTail));
			info.AddProperty(NBestiary.PropertyName._dialogueTailShadow, Variant.From<TextureRect>(ref this._dialogueTailShadow));
			info.AddProperty(NBestiary.PropertyName._modeButton, Variant.From<NButton>(ref this._modeButton));
			info.AddProperty(NBestiary.PropertyName._modeLabel, Variant.From<MegaLabel>(ref this._modeLabel));
			info.AddProperty(NBestiary.PropertyName._isStatsMode, Variant.From<bool>(ref this._isStatsMode));
			info.AddProperty(NBestiary.PropertyName._pageLeftIcon, Variant.From<TextureRect>(ref this._pageLeftIcon));
			info.AddProperty(NBestiary.PropertyName._pageRightIcon, Variant.From<TextureRect>(ref this._pageRightIcon));
			info.AddProperty(NBestiary.PropertyName._moveList, Variant.From<Control>(ref this._moveList));
			info.AddProperty(NBestiary.PropertyName._moveContainer, Variant.From<Control>(ref this._moveContainer));
			info.AddProperty(NBestiary.PropertyName._statsContainer, Variant.From<Control>(ref this._statsContainer));
			info.AddProperty(NBestiary.PropertyName._filterContainer, Variant.From<Control>(ref this._filterContainer));
			info.AddProperty(NBestiary.PropertyName._statsLabel, Variant.From<MegaRichTextLabel>(ref this._statsLabel));
			info.AddProperty(NBestiary.PropertyName._currentFilter, Variant.From<NBestiaryCharacterFilter>(ref this._currentFilter));
			info.AddProperty(NBestiary.PropertyName._selectedEntry, Variant.From<NBestiaryEntry>(ref this._selectedEntry));
			info.AddProperty(NBestiary.PropertyName._previousScreenshakeTarget, Variant.From<Control>(ref this._previousScreenshakeTarget));
			info.AddProperty(NBestiary.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NBestiary.PropertyName._dialogueTween, Variant.From<Tween>(ref this._dialogueTween));
		}

		/// <inheritdoc />
		// Token: 0x06003206 RID: 12806 RVA: 0x001756F0 File Offset: 0x001738F0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBestiary.PropertyName.BackVfxContainer, ref variant))
			{
				this.BackVfxContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBestiary.PropertyName.VfxContainer, ref variant2))
			{
				this.VfxContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBestiary.PropertyName._monsterNameLabel, ref variant3))
			{
				this._monsterNameLabel = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBestiary.PropertyName._epithet, ref variant4))
			{
				this._epithet = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NBestiary.PropertyName._sidebar, ref variant5))
			{
				this._sidebar = variant5.As<NScrollableContainer>();
			}
			Variant variant6;
			if (info.TryGetProperty(NBestiary.PropertyName._bestiaryList, ref variant6))
			{
				this._bestiaryList = variant6.As<VBoxContainer>();
			}
			Variant variant7;
			if (info.TryGetProperty(NBestiary.PropertyName._selectionArrow, ref variant7))
			{
				this._selectionArrow = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NBestiary.PropertyName._arrowTween, ref variant8))
			{
				this._arrowTween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NBestiary.PropertyName._initSelectionArrow, ref variant9))
			{
				this._initSelectionArrow = variant9.As<bool>();
			}
			Variant variant10;
			if (info.TryGetProperty(NBestiary.PropertyName._layoutContainer, ref variant10))
			{
				this._layoutContainer = variant10.As<Control>();
			}
			Variant variant11;
			if (info.TryGetProperty(NBestiary.PropertyName._currentLayout, ref variant11))
			{
				this._currentLayout = variant11.As<NBestiaryLayout>();
			}
			Variant variant12;
			if (info.TryGetProperty(NBestiary.PropertyName._characterIcon, ref variant12))
			{
				this._characterIcon = variant12.As<Control>();
			}
			Variant variant13;
			if (info.TryGetProperty(NBestiary.PropertyName._iconTexture, ref variant13))
			{
				this._iconTexture = variant13.As<TextureRect>();
			}
			Variant variant14;
			if (info.TryGetProperty(NBestiary.PropertyName._iconOutlineTexture, ref variant14))
			{
				this._iconOutlineTexture = variant14.As<TextureRect>();
			}
			Variant variant15;
			if (info.TryGetProperty(NBestiary.PropertyName._dialogueLine, ref variant15))
			{
				this._dialogueLine = variant15.As<Control>();
			}
			Variant variant16;
			if (info.TryGetProperty(NBestiary.PropertyName._dialogueLabel, ref variant16))
			{
				this._dialogueLabel = variant16.As<MegaRichTextLabel>();
			}
			Variant variant17;
			if (info.TryGetProperty(NBestiary.PropertyName._dialogueBubble, ref variant17))
			{
				this._dialogueBubble = variant17.As<Control>();
			}
			Variant variant18;
			if (info.TryGetProperty(NBestiary.PropertyName._dialogueTail, ref variant18))
			{
				this._dialogueTail = variant18.As<TextureRect>();
			}
			Variant variant19;
			if (info.TryGetProperty(NBestiary.PropertyName._dialogueTailShadow, ref variant19))
			{
				this._dialogueTailShadow = variant19.As<TextureRect>();
			}
			Variant variant20;
			if (info.TryGetProperty(NBestiary.PropertyName._modeButton, ref variant20))
			{
				this._modeButton = variant20.As<NButton>();
			}
			Variant variant21;
			if (info.TryGetProperty(NBestiary.PropertyName._modeLabel, ref variant21))
			{
				this._modeLabel = variant21.As<MegaLabel>();
			}
			Variant variant22;
			if (info.TryGetProperty(NBestiary.PropertyName._isStatsMode, ref variant22))
			{
				this._isStatsMode = variant22.As<bool>();
			}
			Variant variant23;
			if (info.TryGetProperty(NBestiary.PropertyName._pageLeftIcon, ref variant23))
			{
				this._pageLeftIcon = variant23.As<TextureRect>();
			}
			Variant variant24;
			if (info.TryGetProperty(NBestiary.PropertyName._pageRightIcon, ref variant24))
			{
				this._pageRightIcon = variant24.As<TextureRect>();
			}
			Variant variant25;
			if (info.TryGetProperty(NBestiary.PropertyName._moveList, ref variant25))
			{
				this._moveList = variant25.As<Control>();
			}
			Variant variant26;
			if (info.TryGetProperty(NBestiary.PropertyName._moveContainer, ref variant26))
			{
				this._moveContainer = variant26.As<Control>();
			}
			Variant variant27;
			if (info.TryGetProperty(NBestiary.PropertyName._statsContainer, ref variant27))
			{
				this._statsContainer = variant27.As<Control>();
			}
			Variant variant28;
			if (info.TryGetProperty(NBestiary.PropertyName._filterContainer, ref variant28))
			{
				this._filterContainer = variant28.As<Control>();
			}
			Variant variant29;
			if (info.TryGetProperty(NBestiary.PropertyName._statsLabel, ref variant29))
			{
				this._statsLabel = variant29.As<MegaRichTextLabel>();
			}
			Variant variant30;
			if (info.TryGetProperty(NBestiary.PropertyName._currentFilter, ref variant30))
			{
				this._currentFilter = variant30.As<NBestiaryCharacterFilter>();
			}
			Variant variant31;
			if (info.TryGetProperty(NBestiary.PropertyName._selectedEntry, ref variant31))
			{
				this._selectedEntry = variant31.As<NBestiaryEntry>();
			}
			Variant variant32;
			if (info.TryGetProperty(NBestiary.PropertyName._previousScreenshakeTarget, ref variant32))
			{
				this._previousScreenshakeTarget = variant32.As<Control>();
			}
			Variant variant33;
			if (info.TryGetProperty(NBestiary.PropertyName._tween, ref variant33))
			{
				this._tween = variant33.As<Tween>();
			}
			Variant variant34;
			if (info.TryGetProperty(NBestiary.PropertyName._dialogueTween, ref variant34))
			{
				this._dialogueTween = variant34.As<Tween>();
			}
		}

		// Token: 0x0400135E RID: 4958
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary");

		// Token: 0x0400135F RID: 4959
		private SerializableProgress _progress;

		// Token: 0x04001360 RID: 4960
		private MegaRichTextLabel _monsterNameLabel;

		// Token: 0x04001361 RID: 4961
		private MegaLabel _epithet;

		// Token: 0x04001362 RID: 4962
		private NScrollableContainer _sidebar;

		// Token: 0x04001363 RID: 4963
		private VBoxContainer _bestiaryList;

		// Token: 0x04001364 RID: 4964
		private static readonly LocString _locked = new LocString("bestiary", "LOCKED.monsterTitle");

		// Token: 0x04001365 RID: 4965
		private Control _selectionArrow;

		// Token: 0x04001366 RID: 4966
		[Nullable(2)]
		private Tween _arrowTween;

		// Token: 0x04001367 RID: 4967
		private static readonly Vector2 _arrowOffset = new Vector2(-42f, 119f);

		// Token: 0x04001368 RID: 4968
		private bool _initSelectionArrow = true;

		// Token: 0x04001369 RID: 4969
		private Control _layoutContainer;

		// Token: 0x0400136A RID: 4970
		[Nullable(2)]
		private NBestiaryLayout _currentLayout;

		// Token: 0x0400136B RID: 4971
		private Control _characterIcon;

		// Token: 0x0400136C RID: 4972
		private TextureRect _iconTexture;

		// Token: 0x0400136D RID: 4973
		private TextureRect _iconOutlineTexture;

		// Token: 0x0400136E RID: 4974
		private Control _dialogueLine;

		// Token: 0x0400136F RID: 4975
		private MegaRichTextLabel _dialogueLabel;

		// Token: 0x04001370 RID: 4976
		private Control _dialogueBubble;

		// Token: 0x04001371 RID: 4977
		private TextureRect _dialogueTail;

		// Token: 0x04001372 RID: 4978
		private TextureRect _dialogueTailShadow;

		// Token: 0x04001373 RID: 4979
		private const string _dialogueTailPath = "res://images/ui/dialogue_tail.png";

		// Token: 0x04001374 RID: 4980
		private const string _thoughtTailPath = "res://images/ui/thought_tail.png";

		// Token: 0x04001375 RID: 4981
		private NButton _modeButton;

		// Token: 0x04001376 RID: 4982
		private MegaLabel _modeLabel;

		// Token: 0x04001377 RID: 4983
		private bool _isStatsMode;

		// Token: 0x04001378 RID: 4984
		private TextureRect _pageLeftIcon;

		// Token: 0x04001379 RID: 4985
		private TextureRect _pageRightIcon;

		// Token: 0x0400137A RID: 4986
		private static readonly StringName _filterLeftHotkey = MegaInput.viewDeckAndTabLeft;

		// Token: 0x0400137B RID: 4987
		private static readonly StringName _filterRightHotkey = MegaInput.viewExhaustPileAndTabRight;

		// Token: 0x0400137C RID: 4988
		private Control _moveList;

		// Token: 0x0400137D RID: 4989
		private Control _moveContainer;

		// Token: 0x0400137E RID: 4990
		private Control _statsContainer;

		// Token: 0x0400137F RID: 4991
		private Control _filterContainer;

		// Token: 0x04001380 RID: 4992
		private MegaRichTextLabel _statsLabel;

		// Token: 0x04001381 RID: 4993
		private NBestiaryCharacterFilter _currentFilter;

		// Token: 0x04001382 RID: 4994
		private HashSet<ModelId> _discoveredMonsterIds;

		// Token: 0x04001383 RID: 4995
		private HashSet<ModelId> _discoveredEncounterIds;

		// Token: 0x04001384 RID: 4996
		[Nullable(2)]
		private NBestiaryEntry _selectedEntry;

		// Token: 0x04001385 RID: 4997
		[Nullable(2)]
		private Control _previousScreenshakeTarget;

		// Token: 0x04001386 RID: 4998
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001387 RID: 4999
		[Nullable(2)]
		private Tween _dialogueTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001651 RID: 5713
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040056E2 RID: 22242
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040056E3 RID: 22243
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ToggleMode' method.
			/// </summary>
			// Token: 0x040056E4 RID: 22244
			public static readonly StringName ToggleMode = "ToggleMode";

			/// <summary>
			/// Cached name for the 'EnableMoveButtonHotkeys' method.
			/// </summary>
			// Token: 0x040056E5 RID: 22245
			public static readonly StringName EnableMoveButtonHotkeys = "EnableMoveButtonHotkeys";

			/// <summary>
			/// Cached name for the 'DisableMoveButtonHotkeys' method.
			/// </summary>
			// Token: 0x040056E6 RID: 22246
			public static readonly StringName DisableMoveButtonHotkeys = "DisableMoveButtonHotkeys";

			/// <summary>
			/// Cached name for the 'ShowMovesPanel' method.
			/// </summary>
			// Token: 0x040056E7 RID: 22247
			public static readonly StringName ShowMovesPanel = "ShowMovesPanel";

			/// <summary>
			/// Cached name for the 'ShowStatsPanel' method.
			/// </summary>
			// Token: 0x040056E8 RID: 22248
			public static readonly StringName ShowStatsPanel = "ShowStatsPanel";

			/// <summary>
			/// Cached name for the 'RefreshStatisticsText' method.
			/// </summary>
			// Token: 0x040056E9 RID: 22249
			public static readonly StringName RefreshStatisticsText = "RefreshStatisticsText";

			/// <summary>
			/// Cached name for the 'DisplayCharacterData' method.
			/// </summary>
			// Token: 0x040056EA RID: 22250
			public static readonly StringName DisplayCharacterData = "DisplayCharacterData";

			/// <summary>
			/// Cached name for the 'UpdateDialogueBubbleStyle' method.
			/// </summary>
			// Token: 0x040056EB RID: 22251
			public static readonly StringName UpdateDialogueBubbleStyle = "UpdateDialogueBubbleStyle";

			/// <summary>
			/// Cached name for the 'ShowDialogue' method.
			/// </summary>
			// Token: 0x040056EC RID: 22252
			public static readonly StringName ShowDialogue = "ShowDialogue";

			/// <summary>
			/// Cached name for the 'HideDialogue' method.
			/// </summary>
			// Token: 0x040056ED RID: 22253
			public static readonly StringName HideDialogue = "HideDialogue";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x040056EE RID: 22254
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x040056EF RID: 22255
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'CreateEntries' method.
			/// </summary>
			// Token: 0x040056F0 RID: 22256
			public static readonly StringName CreateEntries = "CreateEntries";

			/// <summary>
			/// Cached name for the 'CreateFilters' method.
			/// </summary>
			// Token: 0x040056F1 RID: 22257
			public static readonly StringName CreateFilters = "CreateFilters";

			/// <summary>
			/// Cached name for the 'OnCharacterFilterSelected' method.
			/// </summary>
			// Token: 0x040056F2 RID: 22258
			public static readonly StringName OnCharacterFilterSelected = "OnCharacterFilterSelected";

			/// <summary>
			/// Cached name for the 'AddEvents' method.
			/// </summary>
			// Token: 0x040056F3 RID: 22259
			public static readonly StringName AddEvents = "AddEvents";

			/// <summary>
			/// Cached name for the 'OnMonsterClicked' method.
			/// </summary>
			// Token: 0x040056F4 RID: 22260
			public static readonly StringName OnMonsterClicked = "OnMonsterClicked";

			/// <summary>
			/// Cached name for the 'SelectMonster' method.
			/// </summary>
			// Token: 0x040056F5 RID: 22261
			public static readonly StringName SelectMonster = "SelectMonster";

			/// <summary>
			/// Cached name for the 'OnMoveButtonClicked' method.
			/// </summary>
			// Token: 0x040056F6 RID: 22262
			public static readonly StringName OnMoveButtonClicked = "OnMoveButtonClicked";

			/// <summary>
			/// Cached name for the 'GetSideCenter' method.
			/// </summary>
			// Token: 0x040056F7 RID: 22263
			public static readonly StringName GetSideCenter = "GetSideCenter";

			/// <summary>
			/// Cached name for the 'GetSideFloor' method.
			/// </summary>
			// Token: 0x040056F8 RID: 22264
			public static readonly StringName GetSideFloor = "GetSideFloor";

			/// <summary>
			/// Cached name for the 'EnableStatsModeHotkeys' method.
			/// </summary>
			// Token: 0x040056F9 RID: 22265
			public static readonly StringName EnableStatsModeHotkeys = "EnableStatsModeHotkeys";

			/// <summary>
			/// Cached name for the 'DisableStatsModeHotkeys' method.
			/// </summary>
			// Token: 0x040056FA RID: 22266
			public static readonly StringName DisableStatsModeHotkeys = "DisableStatsModeHotkeys";

			/// <summary>
			/// Cached name for the 'FilterLeft' method.
			/// </summary>
			// Token: 0x040056FB RID: 22267
			public static readonly StringName FilterLeft = "FilterLeft";

			/// <summary>
			/// Cached name for the 'FilterRight' method.
			/// </summary>
			// Token: 0x040056FC RID: 22268
			public static readonly StringName FilterRight = "FilterRight";

			/// <summary>
			/// Cached name for the 'SelectFilter' method.
			/// </summary>
			// Token: 0x040056FD RID: 22269
			public static readonly StringName SelectFilter = "SelectFilter";

			/// <summary>
			/// Cached name for the 'UpdatePageIcons' method.
			/// </summary>
			// Token: 0x040056FE RID: 22270
			public static readonly StringName UpdatePageIcons = "UpdatePageIcons";

			/// <summary>
			/// Cached name for the 'CanBeShown' method.
			/// </summary>
			// Token: 0x040056FF RID: 22271
			public static readonly StringName CanBeShown = "CanBeShown";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001652 RID: 5714
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04005700 RID: 22272
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the 'BackVfxContainer' property.
			/// </summary>
			// Token: 0x04005701 RID: 22273
			public static readonly StringName BackVfxContainer = "BackVfxContainer";

			/// <summary>
			/// Cached name for the 'VfxContainer' property.
			/// </summary>
			// Token: 0x04005702 RID: 22274
			public static readonly StringName VfxContainer = "VfxContainer";

			/// <summary>
			/// Cached name for the 'Layout' property.
			/// </summary>
			// Token: 0x04005703 RID: 22275
			public static readonly StringName Layout = "Layout";

			/// <summary>
			/// Cached name for the '_monsterNameLabel' field.
			/// </summary>
			// Token: 0x04005704 RID: 22276
			public static readonly StringName _monsterNameLabel = "_monsterNameLabel";

			/// <summary>
			/// Cached name for the '_epithet' field.
			/// </summary>
			// Token: 0x04005705 RID: 22277
			public static readonly StringName _epithet = "_epithet";

			/// <summary>
			/// Cached name for the '_sidebar' field.
			/// </summary>
			// Token: 0x04005706 RID: 22278
			public static readonly StringName _sidebar = "_sidebar";

			/// <summary>
			/// Cached name for the '_bestiaryList' field.
			/// </summary>
			// Token: 0x04005707 RID: 22279
			public static readonly StringName _bestiaryList = "_bestiaryList";

			/// <summary>
			/// Cached name for the '_selectionArrow' field.
			/// </summary>
			// Token: 0x04005708 RID: 22280
			public static readonly StringName _selectionArrow = "_selectionArrow";

			/// <summary>
			/// Cached name for the '_arrowTween' field.
			/// </summary>
			// Token: 0x04005709 RID: 22281
			public static readonly StringName _arrowTween = "_arrowTween";

			/// <summary>
			/// Cached name for the '_initSelectionArrow' field.
			/// </summary>
			// Token: 0x0400570A RID: 22282
			public static readonly StringName _initSelectionArrow = "_initSelectionArrow";

			/// <summary>
			/// Cached name for the '_layoutContainer' field.
			/// </summary>
			// Token: 0x0400570B RID: 22283
			public static readonly StringName _layoutContainer = "_layoutContainer";

			/// <summary>
			/// Cached name for the '_currentLayout' field.
			/// </summary>
			// Token: 0x0400570C RID: 22284
			public static readonly StringName _currentLayout = "_currentLayout";

			/// <summary>
			/// Cached name for the '_characterIcon' field.
			/// </summary>
			// Token: 0x0400570D RID: 22285
			public static readonly StringName _characterIcon = "_characterIcon";

			/// <summary>
			/// Cached name for the '_iconTexture' field.
			/// </summary>
			// Token: 0x0400570E RID: 22286
			public static readonly StringName _iconTexture = "_iconTexture";

			/// <summary>
			/// Cached name for the '_iconOutlineTexture' field.
			/// </summary>
			// Token: 0x0400570F RID: 22287
			public static readonly StringName _iconOutlineTexture = "_iconOutlineTexture";

			/// <summary>
			/// Cached name for the '_dialogueLine' field.
			/// </summary>
			// Token: 0x04005710 RID: 22288
			public static readonly StringName _dialogueLine = "_dialogueLine";

			/// <summary>
			/// Cached name for the '_dialogueLabel' field.
			/// </summary>
			// Token: 0x04005711 RID: 22289
			public static readonly StringName _dialogueLabel = "_dialogueLabel";

			/// <summary>
			/// Cached name for the '_dialogueBubble' field.
			/// </summary>
			// Token: 0x04005712 RID: 22290
			public static readonly StringName _dialogueBubble = "_dialogueBubble";

			/// <summary>
			/// Cached name for the '_dialogueTail' field.
			/// </summary>
			// Token: 0x04005713 RID: 22291
			public static readonly StringName _dialogueTail = "_dialogueTail";

			/// <summary>
			/// Cached name for the '_dialogueTailShadow' field.
			/// </summary>
			// Token: 0x04005714 RID: 22292
			public static readonly StringName _dialogueTailShadow = "_dialogueTailShadow";

			/// <summary>
			/// Cached name for the '_modeButton' field.
			/// </summary>
			// Token: 0x04005715 RID: 22293
			public static readonly StringName _modeButton = "_modeButton";

			/// <summary>
			/// Cached name for the '_modeLabel' field.
			/// </summary>
			// Token: 0x04005716 RID: 22294
			public static readonly StringName _modeLabel = "_modeLabel";

			/// <summary>
			/// Cached name for the '_isStatsMode' field.
			/// </summary>
			// Token: 0x04005717 RID: 22295
			public static readonly StringName _isStatsMode = "_isStatsMode";

			/// <summary>
			/// Cached name for the '_pageLeftIcon' field.
			/// </summary>
			// Token: 0x04005718 RID: 22296
			public static readonly StringName _pageLeftIcon = "_pageLeftIcon";

			/// <summary>
			/// Cached name for the '_pageRightIcon' field.
			/// </summary>
			// Token: 0x04005719 RID: 22297
			public static readonly StringName _pageRightIcon = "_pageRightIcon";

			/// <summary>
			/// Cached name for the '_moveList' field.
			/// </summary>
			// Token: 0x0400571A RID: 22298
			public static readonly StringName _moveList = "_moveList";

			/// <summary>
			/// Cached name for the '_moveContainer' field.
			/// </summary>
			// Token: 0x0400571B RID: 22299
			public static readonly StringName _moveContainer = "_moveContainer";

			/// <summary>
			/// Cached name for the '_statsContainer' field.
			/// </summary>
			// Token: 0x0400571C RID: 22300
			public static readonly StringName _statsContainer = "_statsContainer";

			/// <summary>
			/// Cached name for the '_filterContainer' field.
			/// </summary>
			// Token: 0x0400571D RID: 22301
			public static readonly StringName _filterContainer = "_filterContainer";

			/// <summary>
			/// Cached name for the '_statsLabel' field.
			/// </summary>
			// Token: 0x0400571E RID: 22302
			public static readonly StringName _statsLabel = "_statsLabel";

			/// <summary>
			/// Cached name for the '_currentFilter' field.
			/// </summary>
			// Token: 0x0400571F RID: 22303
			public static readonly StringName _currentFilter = "_currentFilter";

			/// <summary>
			/// Cached name for the '_selectedEntry' field.
			/// </summary>
			// Token: 0x04005720 RID: 22304
			public static readonly StringName _selectedEntry = "_selectedEntry";

			/// <summary>
			/// Cached name for the '_previousScreenshakeTarget' field.
			/// </summary>
			// Token: 0x04005721 RID: 22305
			public static readonly StringName _previousScreenshakeTarget = "_previousScreenshakeTarget";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005722 RID: 22306
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_dialogueTween' field.
			/// </summary>
			// Token: 0x04005723 RID: 22307
			public static readonly StringName _dialogueTween = "_dialogueTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001653 RID: 5715
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}
	}
}
