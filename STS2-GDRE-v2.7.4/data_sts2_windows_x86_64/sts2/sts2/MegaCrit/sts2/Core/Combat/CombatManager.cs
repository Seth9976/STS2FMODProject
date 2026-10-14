using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Combat
{
	// Token: 0x02000D3F RID: 3391
	[NullableContext(1)]
	[Nullable(0)]
	public class CombatManager
	{
		// Token: 0x17002586 RID: 9606
		// (get) Token: 0x060088C3 RID: 35011 RVA: 0x002E061B File Offset: 0x002DE81B
		public static CombatManager Instance { get; } = new CombatManager();

		/// <summary>
		/// Fired after combat is set up.
		/// Note that this happens a little bit before combat actually begins.
		/// </summary>
		// Token: 0x140000CF RID: 207
		// (add) Token: 0x060088C4 RID: 35012 RVA: 0x002E0624 File Offset: 0x002DE824
		// (remove) Token: 0x060088C5 RID: 35013 RVA: 0x002E065C File Offset: 0x002DE85C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CombatState> CombatSetUp;

		/// <summary>
		/// Fired whenever a new combat begins, after IsInProgress is set to true.
		/// </summary>
		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x060088C6 RID: 35014 RVA: 0x002E0694 File Offset: 0x002DE894
		// (remove) Token: 0x060088C7 RID: 35015 RVA: 0x002E06CC File Offset: 0x002DE8CC
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CombatState> CombatBegan;

		/// <summary>
		/// Fired when combat ends.
		/// </summary>
		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x060088C8 RID: 35016 RVA: 0x002E0704 File Offset: 0x002DE904
		// (remove) Token: 0x060088C9 RID: 35017 RVA: 0x002E073C File Offset: 0x002DE93C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CombatRoom> CombatEnded;

		/// <summary>
		/// Fired when combat is won.
		/// </summary>
		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x060088CA RID: 35018 RVA: 0x002E0774 File Offset: 0x002DE974
		// (remove) Token: 0x060088CB RID: 35019 RVA: 0x002E07AC File Offset: 0x002DE9AC
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CombatRoom> CombatWon;

		/// <summary>
		/// Fired whenever the arrangement of creatures in the combat changes. Specifically, when:
		/// * A creature is added.
		/// * A creature is removed.
		/// * A creature's position changes.
		/// </summary>
		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x060088CC RID: 35020 RVA: 0x002E07E4 File Offset: 0x002DE9E4
		// (remove) Token: 0x060088CD RID: 35021 RVA: 0x002E081C File Offset: 0x002DEA1C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CombatState> CreaturesChanged;

		/// <summary>
		/// Fired whenever a new turn starts.
		/// </summary>
		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x060088CE RID: 35022 RVA: 0x002E0854 File Offset: 0x002DEA54
		// (remove) Token: 0x060088CF RID: 35023 RVA: 0x002E088C File Offset: 0x002DEA8C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CombatState> TurnStarted;

		/// <summary>
		/// Fired whenever a turn ends.
		/// </summary>
		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x060088D0 RID: 35024 RVA: 0x002E08C4 File Offset: 0x002DEAC4
		// (remove) Token: 0x060088D1 RID: 35025 RVA: 0x002E08FC File Offset: 0x002DEAFC
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CombatState> TurnEnded;

		/// <summary>
		/// Fired whenever a player ends their turn. Remember that, in multiplayer, this is not the same as switching to the
		/// enemy's turn.
		/// </summary>
		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x060088D2 RID: 35026 RVA: 0x002E0934 File Offset: 0x002DEB34
		// (remove) Token: 0x060088D3 RID: 35027 RVA: 0x002E096C File Offset: 0x002DEB6C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<Player, bool> PlayerEndedTurn;

		/// <summary>
		/// Fired whenever a player un-does the end of their turn.
		/// </summary>
		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x060088D4 RID: 35028 RVA: 0x002E09A4 File Offset: 0x002DEBA4
		// (remove) Token: 0x060088D5 RID: 35029 RVA: 0x002E09DC File Offset: 0x002DEBDC
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<Player> PlayerUnendedTurn;

		/// <summary>
		/// Fired when all players have fully committed to ending turn and all player actions are done (including end of turn
		/// hooks like Well-Laid Plans), but before the player hand flush.
		/// </summary>
		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x060088D6 RID: 35030 RVA: 0x002E0A14 File Offset: 0x002DEC14
		// (remove) Token: 0x060088D7 RID: 35031 RVA: 0x002E0A4C File Offset: 0x002DEC4C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CombatState> AboutToSwitchToEnemyTurn;

		/// <summary>
		/// Fired when the local player's actions become disabled or enabled.
		/// </summary>
		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x060088D8 RID: 35032 RVA: 0x002E0A84 File Offset: 0x002DEC84
		// (remove) Token: 0x060088D9 RID: 35033 RVA: 0x002E0ABC File Offset: 0x002DECBC
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CombatState> PlayerActionsDisabledChanged;

		// Token: 0x17002587 RID: 9607
		// (get) Token: 0x060088DA RID: 35034 RVA: 0x002E0AF4 File Offset: 0x002DECF4
		private CancellationToken CombatCt
		{
			get
			{
				CancellationTokenSource combatCts = this._combatCts;
				if (combatCts == null)
				{
					return default(CancellationToken);
				}
				return combatCts.Token;
			}
		}

		/// <summary>
		/// WARNING: ONLY USE THIS IN TESTS!
		/// See <see cref="M:MegaCrit.Sts2.Core.Combat.CombatManager.DebugForceTopCardOnNextShuffle(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x17002588 RID: 9608
		// (get) Token: 0x060088DB RID: 35035 RVA: 0x002E0B1A File Offset: 0x002DED1A
		// (set) Token: 0x060088DC RID: 35036 RVA: 0x002E0B22 File Offset: 0x002DED22
		[Nullable(2)]
		public CardModel DebugForcedTopCardOnNextShuffle
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		/// <summary>
		/// THIS IS TEMPORARY AND SHOULD ONLY BE USED IN TESTS
		/// </summary>
		/// <returns></returns>
		// Token: 0x060088DD RID: 35037 RVA: 0x002E0B2B File Offset: 0x002DED2B
		[NullableContext(2)]
		public CombatState DebugOnlyGetState()
		{
			return this._state;
		}

		// Token: 0x17002589 RID: 9609
		// (get) Token: 0x060088DE RID: 35038 RVA: 0x002E0B33 File Offset: 0x002DED33
		// (set) Token: 0x060088DF RID: 35039 RVA: 0x002E0B3B File Offset: 0x002DED3B
		public bool IsPaused { get; private set; }

		// Token: 0x1700258A RID: 9610
		// (get) Token: 0x060088E0 RID: 35040 RVA: 0x002E0B44 File Offset: 0x002DED44
		// (set) Token: 0x060088E1 RID: 35041 RVA: 0x002E0B4C File Offset: 0x002DED4C
		public bool PlayerActionsDisabled
		{
			get
			{
				return this._playerActionsDisabled;
			}
			private set
			{
				if (this._playerActionsDisabled != value)
				{
					this._playerActionsDisabled = value;
					Action<CombatState> playerActionsDisabledChanged = this.PlayerActionsDisabledChanged;
					if (playerActionsDisabledChanged == null)
					{
						return;
					}
					playerActionsDisabledChanged(this._state);
				}
			}
		}

		/// <summary>
		/// The list of players in the current turn that are taking an extra turn.
		/// Normally empty; only non-empty if there are players that used extra-turn-taking effects like
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Relics.PaelsEye" />.
		/// Returns a snapshot copy for thread safety.
		/// </summary>
		// Token: 0x1700258B RID: 9611
		// (get) Token: 0x060088E2 RID: 35042 RVA: 0x002E0B74 File Offset: 0x002DED74
		public IReadOnlyList<Player> PlayersTakingExtraTurn
		{
			get
			{
				IReadOnlyList<Player> readOnlyList;
				using (this._playerReadyLock.EnterScope())
				{
					readOnlyList = this._playersTakingExtraTurn.ToList<Player>();
				}
				return readOnlyList;
			}
		}

		/// <summary>
		/// Sets <see cref="P:MegaCrit.Sts2.Core.Entities.Players.PlayerCombatState.Phase" /> to the same value for all players.
		/// </summary>
		// Token: 0x060088E3 RID: 35043 RVA: 0x002E0BB4 File Offset: 0x002DEDB4
		private void SetPhaseForAllPlayers(PlayerTurnPhase phase)
		{
			if (this._state == null)
			{
				return;
			}
			foreach (Player player in this._state.Players)
			{
				if (player.PlayerCombatState != null)
				{
					player.PlayerCombatState.Phase = phase;
				}
			}
		}

		/// <summary>
		/// True when the enemy turn has started (TurnStarted has fired for the enemy side).
		/// Set right before TurnStarted fires for enemy turns, cleared when switching to player turn.
		/// </summary>
		// Token: 0x1700258C RID: 9612
		// (get) Token: 0x060088E4 RID: 35044 RVA: 0x002E0C1C File Offset: 0x002DEE1C
		// (set) Token: 0x060088E5 RID: 35045 RVA: 0x002E0C24 File Offset: 0x002DEE24
		public bool IsEnemyTurnStarted { get; private set; }

		/// <summary>
		/// Set to true in the time between when all players are ready to begin the enemy turn and when the enemy turn begins.
		/// </summary>
		// Token: 0x1700258D RID: 9613
		// (get) Token: 0x060088E6 RID: 35046 RVA: 0x002E0C2D File Offset: 0x002DEE2D
		// (set) Token: 0x060088E7 RID: 35047 RVA: 0x002E0C35 File Offset: 0x002DEE35
		public bool EndingPlayerTurnPhaseTwo { get; private set; }

		/// <summary>
		/// Set to true in the time during phase one of the end of the player's turn.
		/// </summary>
		// Token: 0x1700258E RID: 9614
		// (get) Token: 0x060088E8 RID: 35048 RVA: 0x002E0C3E File Offset: 0x002DEE3E
		// (set) Token: 0x060088E9 RID: 35049 RVA: 0x002E0C46 File Offset: 0x002DEE46
		public bool EndingPlayerTurnPhaseOne { get; private set; }

		// Token: 0x1700258F RID: 9615
		// (get) Token: 0x060088EA RID: 35050 RVA: 0x002E0C4F File Offset: 0x002DEE4F
		public CombatStateTracker StateTracker { get; }

		// Token: 0x17002590 RID: 9616
		// (get) Token: 0x060088EB RID: 35051 RVA: 0x002E0C57 File Offset: 0x002DEE57
		public CombatHistory History { get; }

		/// <summary>
		/// Is the combat currently in progress?
		/// True when the combat is done being initialized and has fully started.
		/// False when:
		/// * The combat is first being initialized.
		/// * The combat is ending (the last monster has been killed).
		/// * We're in a non-combat room.
		/// </summary>
		// Token: 0x17002591 RID: 9617
		// (get) Token: 0x060088EC RID: 35052 RVA: 0x002E0C5F File Offset: 0x002DEE5F
		// (set) Token: 0x060088ED RID: 35053 RVA: 0x002E0C67 File Offset: 0x002DEE67
		public bool IsInProgress { get; private set; }

		/// <summary>
		/// Is a new combat currently being set up?
		/// True from the start of <see cref="M:MegaCrit.Sts2.Core.Combat.CombatManager.SetUpCombat(MegaCrit.Sts2.Core.Combat.CombatState)" /> until <see cref="P:MegaCrit.Sts2.Core.Combat.CombatManager.IsInProgress" /> flips true in
		/// <see cref="M:MegaCrit.Sts2.Core.Combat.CombatManager.StartCombatInternal" />. During this window <see cref="P:MegaCrit.Sts2.Core.Combat.CombatManager.IsInProgress" /> is still false, so it lets
		/// callers distinguish "combat is starting" (where combat hooks that run during setup, like the initial deck
		/// shuffle, must still fire) from "combat is over or ending".
		/// </summary>
		// Token: 0x17002592 RID: 9618
		// (get) Token: 0x060088EE RID: 35054 RVA: 0x002E0C70 File Offset: 0x002DEE70
		// (set) Token: 0x060088EF RID: 35055 RVA: 0x002E0C78 File Offset: 0x002DEE78
		public bool IsStarting { get; private set; }

		/// <summary>
		/// True while a <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" /> or a <see cref="M:MegaCrit.Sts2.Core.Models.PotionModel.OnUse(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Creatures.Creature)" /> effect body is currently
		/// executing for <paramref name="player" />, including nested auto-plays (e.g. a Sly card auto-played when
		/// discarded). Used to avoid premature hand-empty triggers while that player's effect is mid-resolution.
		/// </summary>
		// Token: 0x060088F0 RID: 35056 RVA: 0x002E0C81 File Offset: 0x002DEE81
		public bool IsExecutingCardOrPotionEffect(Player player)
		{
			return this._cardOrPotionEffectDepth.GetValueOrDefault(player) > 0;
		}

		/// <summary>
		/// Marks the start of a <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" /> or <see cref="M:MegaCrit.Sts2.Core.Models.PotionModel.OnUse(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Creatures.Creature)" /> effect body for
		/// <paramref name="player" />, incrementing their effect-nesting depth. Must be paired with a
		/// <see cref="M:MegaCrit.Sts2.Core.Combat.CombatManager.EndCardOrPotionEffect(MegaCrit.Sts2.Core.Entities.Players.Player)" /> in a finally block so the depth stays balanced even if the effect throws or
		/// the player dies mid-play.
		/// </summary>
		// Token: 0x060088F1 RID: 35057 RVA: 0x002E0C92 File Offset: 0x002DEE92
		public void BeginCardOrPotionEffect(Player player)
		{
			this._cardOrPotionEffectDepth[player] = this._cardOrPotionEffectDepth.GetValueOrDefault(player) + 1;
		}

		/// <summary>
		/// Marks the end of a <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" /> or <see cref="M:MegaCrit.Sts2.Core.Models.PotionModel.OnUse(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Creatures.Creature)" /> effect body for
		/// <paramref name="player" />, decrementing their effect-nesting depth (and removing the entry once it reaches
		/// zero). Pairs with <see cref="M:MegaCrit.Sts2.Core.Combat.CombatManager.BeginCardOrPotionEffect(MegaCrit.Sts2.Core.Entities.Players.Player)" />; call it from a finally block.
		/// </summary>
		// Token: 0x060088F2 RID: 35058 RVA: 0x002E0CB0 File Offset: 0x002DEEB0
		public async Task EndCardOrPotionEffect(Player player)
		{
			int num = this._cardOrPotionEffectDepth.GetValueOrDefault(player) - 1;
			if (num <= 0)
			{
				this._cardOrPotionEffectDepth.Remove(player);
				if (player.Creature.IsDead)
				{
					await this.RemoveDeadPlayerCardsFromCombat(player);
				}
			}
			else
			{
				this._cardOrPotionEffectDepth[player] = num;
			}
		}

		/// <summary>
		/// Is combat about to end due to player death?
		/// True when LoseCombat() has been called but the loss hasn't been processed yet.
		/// This allows effects to bail out early while still letting the current action complete.
		/// </summary>
		// Token: 0x17002593 RID: 9619
		// (get) Token: 0x060088F3 RID: 35059 RVA: 0x002E0CFB File Offset: 0x002DEEFB
		public bool IsAboutToLose
		{
			get
			{
				return this._pendingLoss != null;
			}
		}

		/// <summary>
		/// Is the combat in the process of ending (but still in progress)?
		/// True when combat is in progress but all the enemies are dead, and there is nothing stopping combat from ending
		/// (e.g. Phrog Parasite spawning in new enemies).
		/// Also true when a pending loss is waiting to be processed.
		/// False when
		/// * Combat is in progress and 1+ primary enemies are still alive.
		/// * Combat is not in progress.
		/// </summary>
		// Token: 0x17002594 RID: 9620
		// (get) Token: 0x060088F4 RID: 35060 RVA: 0x002E0D0C File Offset: 0x002DEF0C
		public bool IsEnding
		{
			get
			{
				if (!this.IsInProgress)
				{
					return false;
				}
				if (this._pendingLoss != null)
				{
					return true;
				}
				if (this._state != null)
				{
					if (this._state.Enemies.Any<Creature>((Creature e) => e != null && e.IsAlive && e.IsPrimaryEnemy))
					{
						return false;
					}
				}
				return !Hook.ShouldStopCombatFromEnding(this._state);
			}
		}

		/// <summary>
		/// Has this combat ended (or is it in the process of ending)?
		/// When you want to skip/cancel an effect because combat is not in progress, you should usually use this instead of
		/// <see cref="P:MegaCrit.Sts2.Core.Combat.CombatManager.IsEnding" /> or !<see cref="P:MegaCrit.Sts2.Core.Combat.CombatManager.IsInProgress" />, because they can return unexpected results at certain
		/// boundary points.
		/// </summary>
		// Token: 0x17002595 RID: 9621
		// (get) Token: 0x060088F5 RID: 35061 RVA: 0x002E0D7E File Offset: 0x002DEF7E
		public bool IsOverOrEnding
		{
			get
			{
				return this.IsEnding || !this.IsInProgress;
			}
		}

		// Token: 0x060088F6 RID: 35062 RVA: 0x002E0D94 File Offset: 0x002DEF94
		private CombatManager()
		{
			this.History = new CombatHistory();
			this.StateTracker = new CombatStateTracker(this);
		}

		// Token: 0x060088F7 RID: 35063 RVA: 0x002E0DF8 File Offset: 0x002DEFF8
		public void SetUpCombat(CombatState state)
		{
			if (this._state != null)
			{
				throw new InvalidOperationException("Make sure to reset the combat before setting up a new one.");
			}
			this.IsStarting = true;
			this._state = state;
			MultiplayerScalingModel multiplayerScalingModel = this._state.MultiplayerScalingModel;
			if (multiplayerScalingModel != null)
			{
				multiplayerScalingModel.OnCombatEntered(this._state);
			}
			this.StateTracker.SetState(state);
			using (this._playerReadyLock.EnterScope())
			{
				this._playersTakingExtraTurn.Clear();
			}
			foreach (Player player in state.Players)
			{
				player.ResetCombatState();
			}
			foreach (Player player2 in state.Players)
			{
				player2.PopulateCombatState(player2.RunState.Rng.Shuffle, state);
			}
			NetCombatCardDb.Instance.StartCombat(state.Players);
			foreach (Creature creature in state.Creatures)
			{
				this.AddCreature(creature);
			}
			Action<CombatState> combatSetUp = this.CombatSetUp;
			if (combatSetUp == null)
			{
				return;
			}
			combatSetUp(state);
		}

		// Token: 0x060088F8 RID: 35064 RVA: 0x002E0F70 File Offset: 0x002DF170
		public void AfterCombatRoomLoaded()
		{
			CancellationTokenSource combatCts = this._combatCts;
			if (combatCts != null)
			{
				combatCts.Cancel();
			}
			this._combatCts = new CancellationTokenSource();
			RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.PreCombatSetup);
			TaskHelper.RunSafely(this.StartCombatInternal());
		}

		// Token: 0x060088F9 RID: 35065 RVA: 0x002E0FAC File Offset: 0x002DF1AC
		public async Task StartCombatInternal()
		{
			RunManager.Instance.ActionExecutor.Unpause();
			await RunManager.Instance.ActionExecutor.FinishedExecutingActions();
			RunManager.Instance.ActionExecutor.Pause();
			if (this._state.Encounter.HasBgm)
			{
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.PlayCustomMusic(this._state.Encounter.CustomBgm);
				}
			}
			foreach (Creature creature in this._state.Creatures)
			{
				await this.AfterCreatureAdded(creature);
				this.CombatCt.ThrowIfCancellationRequested();
			}
			IEnumerator<Creature> enumerator = null;
			RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.NotPlayPhase);
			this.IsInProgress = true;
			this.IsStarting = false;
			await Hook.BeforeCombatStart(this._state.RunState, this._state);
			this.CombatCt.ThrowIfCancellationRequested();
			Action<CombatState> combatBegan = this.CombatBegan;
			if (combatBegan != null)
			{
				combatBegan(this._state);
			}
			NRunMusicController instance2 = NRunMusicController.Instance;
			if (instance2 != null)
			{
				instance2.UpdateTrack();
			}
			NCombatRulesFtue ftue = null;
			if (SaveManager.Instance.SeenFtue("combat_rules_ftue"))
			{
				NCombatRoom instance3 = NCombatRoom.Instance;
				if (instance3 != null)
				{
					instance3.AddChildSafely(NCombatStartBanner.Create());
				}
			}
			else
			{
				ftue = NCombatRulesFtue.Create();
				NModalContainer instance4 = NModalContainer.Instance;
				if (instance4 != null)
				{
					instance4.Add(ftue, false);
				}
			}
			await Cmd.CustomScaledWait(0.5f, 1f, false, default(CancellationToken));
			this.CombatCt.ThrowIfCancellationRequested();
			await this.StartTurn(null);
			NCombatRulesFtue ncombatRulesFtue = ftue;
			if (ncombatRulesFtue != null)
			{
				ncombatRulesFtue.Start();
			}
		}

		// Token: 0x060088FA RID: 35066 RVA: 0x002E0FF0 File Offset: 0x002DF1F0
		private async Task StartTurn([Nullable(new byte[] { 2, 1 })] Func<Task> actionDuringEnemyTurn = null)
		{
			if (this.IsInProgress)
			{
				this.CombatCt.ThrowIfCancellationRequested();
				this.SetPhaseForAllPlayers(PlayerTurnPhase.None);
				bool isExtraPlayerTurn;
				List<Creature> creaturesStartingTurn;
				List<Player> playersStartingTurn;
				using (this._playerReadyLock.EnterScope())
				{
					isExtraPlayerTurn = this._playersTakingExtraTurn.Count > 0;
					CombatState state = this._state;
					if (state != null && state.CurrentSide == CombatSide.Player && isExtraPlayerTurn)
					{
						creaturesStartingTurn = this._playersTakingExtraTurn.Select<Player, Creature>((Player p) => p.Creature).ToList<Creature>();
						playersStartingTurn = this._playersTakingExtraTurn.ToList<Player>();
					}
					else
					{
						CombatState state2 = this._state;
						creaturesStartingTurn = ((state2 != null) ? state2.CreaturesOnCurrentSide.ToList<Creature>() : null) ?? new List<Creature>();
						CombatState state3 = this._state;
						List<Player> list;
						if (state3 == null || state3.CurrentSide != CombatSide.Player)
						{
							list = new List<Player>();
						}
						else
						{
							CombatState state4 = this._state;
							list = ((state4 != null) ? state4.Players.ToList<Player>() : null) ?? new List<Player>();
						}
						playersStartingTurn = list;
					}
				}
				foreach (Creature creature in creaturesStartingTurn)
				{
					if (this._state != null)
					{
						creature.BeforeTurnStart(this._state.CurrentSide);
					}
				}
				if (this._state != null)
				{
					await Hook.BeforeSideTurnStart(this._state, this._state.CurrentSide, creaturesStartingTurn);
					this.CombatCt.ThrowIfCancellationRequested();
				}
				CombatState state5 = this._state;
				if (state5 != null && state5.CurrentSide == CombatSide.Player)
				{
					this.SetPhaseForAllPlayers(PlayerTurnPhase.Start);
					this.PlayerActionsDisabled = false;
					using (this._playerReadyLock.EnterScope())
					{
						this._playersReadyToEndTurn.Clear();
						this._playersReadyToBeginEnemyTurn.Clear();
						this._playerToEnemyTransitionFired = false;
						this._inPlayerTurnSetup = true;
						this._deferredEndTurnTransition = null;
					}
					Player me = LocalContext.GetMe(playersStartingTurn);
					int? num;
					if (me == null)
					{
						num = null;
					}
					else
					{
						PlayerCombatState playerCombatState = me.PlayerCombatState;
						num = ((playerCombatState != null) ? new int?(playerCombatState.TurnNumber) : null);
					}
					int? num2 = num;
					int valueOrDefault = num2.GetValueOrDefault(-1);
					if (valueOrDefault > 1)
					{
						NCombatRoom instance = NCombatRoom.Instance;
						if (instance != null)
						{
							instance.AddChildSafely(NPlayerTurnBanner.Create(valueOrDefault));
						}
					}
					if (isExtraPlayerTurn)
					{
						goto IL_0378;
					}
					using (IEnumerator<Creature> enumerator2 = this._state.Enemies.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Creature creature2 = enumerator2.Current;
							creature2.PrepareForNextTurn(this._state.PlayerCreatures, true);
						}
						goto IL_0378;
					}
				}
				NCombatRoom instance2 = NCombatRoom.Instance;
				if (instance2 != null)
				{
					instance2.AddChildSafely(NEnemyTurnBanner.Create());
				}
				IL_0378:
				await Cmd.CustomScaledWait(0.5f, 0.8f, false, default(CancellationToken));
				this.CombatCt.ThrowIfCancellationRequested();
				foreach (Creature creature3 in creaturesStartingTurn)
				{
					if (this._state != null)
					{
						await creature3.AfterTurnStart(this._state.CurrentSide);
						this.CombatCt.ThrowIfCancellationRequested();
					}
				}
				List<Creature>.Enumerator enumerator3 = default(List<Creature>.Enumerator);
				foreach (Creature creature4 in creaturesStartingTurn)
				{
					if (this._state != null)
					{
						await Hook.AfterBlockCleared(this._state, creature4);
						this.CombatCt.ThrowIfCancellationRequested();
					}
				}
				enumerator3 = default(List<Creature>.Enumerator);
				List<ValueTuple<HookPlayerChoiceContext, Task>> setupPlayerTurnContext = new List<ValueTuple<HookPlayerChoiceContext, Task>>();
				foreach (Player player in playersStartingTurn)
				{
					if (LocalContext.NetId != null)
					{
						HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(player, LocalContext.NetId.Value, GameActionType.CombatPlayPhaseOnly);
						Task task = this.SetupPlayerTurn(player, playerChoiceContext);
						await playerChoiceContext.WaitForPauseOrCompletionWithoutAssigningTask(task);
						this.CombatCt.ThrowIfCancellationRequested();
						setupPlayerTurnContext.Add(new ValueTuple<HookPlayerChoiceContext, Task>(playerChoiceContext, task));
						playerChoiceContext = null;
						task = null;
					}
				}
				List<Player>.Enumerator enumerator4 = default(List<Player>.Enumerator);
				if (this._state != null)
				{
					await Hook.AfterSideTurnStart(this._state, this._state.CurrentSide, creaturesStartingTurn);
					this.CombatCt.ThrowIfCancellationRequested();
				}
				CombatState state6 = this._state;
				if (state6 != null && state6.CurrentSide == CombatSide.Player)
				{
					foreach (Player player2 in playersStartingTurn)
					{
						if (player2.PlayerCombatState != null && LocalContext.NetId != null)
						{
							HookPlayerChoiceContext hookPlayerChoiceContext = new HookPlayerChoiceContext(player2, LocalContext.NetId.Value, GameActionType.CombatPlayPhaseOnly);
							await hookPlayerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(player2.PlayerCombatState.OrbQueue.AfterTurnStart(hookPlayerChoiceContext));
							this.CombatCt.ThrowIfCancellationRequested();
						}
					}
					enumerator4 = default(List<Player>.Enumerator);
					RunManager.Instance.ChecksumTracker.GenerateChecksum("After player turn start", null);
					if (this._state != null)
					{
						foreach (Player player3 in this._state.Players)
						{
							if (player3.Creature.IsDead || !playersStartingTurn.Contains(player3))
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Setting player ");
								defaultInterpolatedStringHandler.AppendFormatted<ulong>(player3.NetId);
								defaultInterpolatedStringHandler.AppendLiteral(" to ready at start of turn. IsDead: ");
								defaultInterpolatedStringHandler.AppendFormatted<bool>(player3.Creature.IsDead);
								defaultInterpolatedStringHandler.AppendLiteral(". ");
								defaultInterpolatedStringHandler.AppendLiteral("IsStartingTurn: ");
								defaultInterpolatedStringHandler.AppendFormatted<bool>(playersStartingTurn.Contains(player3));
								Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
								this.SetReadyToEndTurn(player3, false, null);
								if (this.AllPlayersReadyToEndTurn())
								{
									this.ReleaseDeferredEndTurnTransitionIfNeeded();
									return;
								}
							}
						}
						foreach (ValueTuple<Player, ValueTuple<HookPlayerChoiceContext, Task>> valueTuple in playersStartingTurn.Zip<Player, ValueTuple<HookPlayerChoiceContext, Task>>(setupPlayerTurnContext))
						{
							ValueTuple<HookPlayerChoiceContext, Task> item = valueTuple.Item2;
							Player item2 = valueTuple.Item1;
							HookPlayerChoiceContext item3 = item.Item1;
							Task item4 = item.Item2;
							if (this._state == null)
							{
								this.ReleaseDeferredEndTurnTransitionIfNeeded();
								return;
							}
							if (!item2.Creature.IsDead)
							{
								await item3.AssignTaskAndWaitForPauseOrCompletion(this.RunAutoPrePlayPhase(item3, item4, item2));
								this.CombatCt.ThrowIfCancellationRequested();
							}
						}
						IEnumerator<ValueTuple<Player, ValueTuple<HookPlayerChoiceContext, Task>>> enumerator6 = null;
						await this.CheckWinCondition();
						this.CombatCt.ThrowIfCancellationRequested();
						if (this.IsInProgress)
						{
							RunManager.Instance.ActionExecutor.Unpause();
							RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.PlayPhase);
							this.IsEnemyTurnStarted = false;
							using (this._playerReadyLock.EnterScope())
							{
								this._inPlayerTurnSetup = false;
							}
							Action<CombatState> turnStarted = this.TurnStarted;
							if (turnStarted != null)
							{
								turnStarted(this._state);
							}
						}
						this.ReleaseDeferredEndTurnTransitionIfNeeded();
					}
				}
				else
				{
					this.IsEnemyTurnStarted = true;
					if (this._state != null)
					{
						Action<CombatState> turnStarted2 = this.TurnStarted;
						if (turnStarted2 != null)
						{
							turnStarted2(this._state);
						}
					}
					RunManager.Instance.ChecksumTracker.GenerateChecksum("After enemy turn start", null);
					await this.WaitForUnpause();
					this.CombatCt.ThrowIfCancellationRequested();
					await this.CheckWinCondition();
					this.CombatCt.ThrowIfCancellationRequested();
					if (this.IsInProgress)
					{
						await this.ExecuteEnemyTurn(actionDuringEnemyTurn);
					}
				}
			}
		}

		/// <summary>
		/// Awaits the player's setup task, then runs the auto-pre-play hooks, transitioning the player's phase
		/// from <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.Start" /> -&gt; <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.AutoPrePlay" /> -&gt; <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.Play" />.
		/// The setup await ensures a player whose setup is paused (making a <see cref="T:MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext" />)
		/// stays in <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.Start" /> until their setup actually completes.
		/// </summary>
		// Token: 0x060088FB RID: 35067 RVA: 0x002E103C File Offset: 0x002DF23C
		private async Task RunAutoPrePlayPhase(HookPlayerChoiceContext playerChoiceContext, Task setupPlayerTurnTask, Player player)
		{
			await setupPlayerTurnTask;
			player.PlayerCombatState.Phase = PlayerTurnPhase.AutoPrePlay;
			await this.CheckForEmptyHand(playerChoiceContext, player);
			await Hook.AfterAutoPrePlayPhaseEntered(playerChoiceContext, this._state, player);
			player.PlayerCombatState.Phase = PlayerTurnPhase.Play;
		}

		/// <summary>
		/// Sets up a player's turn by resetting energy, drawing cards, and firing start-of-turn hooks.
		/// If the player's turn start executes a player choice (e.g. Mayhem plays Cosmic Indifference), then the entire
		/// sequence is paused for this player. However, other players' turn start sequences may continue, and they may
		/// play cards while this is occuring.
		/// </summary>
		/// <param name="player">The player whose turn to setup.</param>
		/// <param name="playerChoiceContext">The player choice context to pass to hooks that take it.</param>
		// Token: 0x060088FC RID: 35068 RVA: 0x002E1098 File Offset: 0x002DF298
		private async Task SetupPlayerTurn(Player player, HookPlayerChoiceContext playerChoiceContext)
		{
			if (!player.Creature.IsDead)
			{
				if (this._state == null || player.PlayerCombatState == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(100, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Combat state is null. Assuming that the run has been cleaned up. (CombatState: ");
					defaultInterpolatedStringHandler.AppendFormatted<CombatState>(this._state);
					defaultInterpolatedStringHandler.AppendLiteral(" PlayerCombatState: ");
					defaultInterpolatedStringHandler.AppendFormatted<PlayerCombatState>(player.PlayerCombatState);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				else
				{
					CombatState state = this._state;
					if (Hook.ShouldPlayerResetEnergy(state, player))
					{
						SfxCmd.Play("event:/sfx/ui/gain_energy", 1f);
						player.PlayerCombatState.ResetEnergy();
					}
					else
					{
						player.PlayerCombatState.AddMaxEnergyToCurrent();
					}
					await Hook.AfterEnergyReset(state, player);
					this.CombatCt.ThrowIfCancellationRequested();
					await Hook.BeforeHandDraw(state, player, playerChoiceContext);
					this.CombatCt.ThrowIfCancellationRequested();
					IEnumerable<AbstractModel> enumerable;
					decimal handDraw = Hook.ModifyHandDraw(state, player, 5m, out enumerable);
					await Hook.AfterModifyingHandDraw(state, enumerable);
					this.CombatCt.ThrowIfCancellationRequested();
					if (player.PlayerCombatState.TurnNumber == 1)
					{
						CardPile pile = PileType.Draw.GetPile(player);
						List<CardModel> list = pile.Cards.Where<CardModel>(delegate(CardModel c)
						{
							EnchantmentModel enchantment = c.Enchantment;
							return enchantment != null && enchantment.ShouldStartAtBottomOfDrawPile;
						}).ToList<CardModel>();
						foreach (CardModel cardModel in list)
						{
							pile.MoveToBottomInternal(cardModel);
						}
						List<CardModel> list2 = pile.Cards.Where<CardModel>((CardModel c) => c.Keywords.Contains(CardKeyword.Innate)).Except<CardModel>(list).ToList<CardModel>();
						foreach (CardModel cardModel2 in list2)
						{
							pile.MoveToTopInternal(cardModel2);
						}
						handDraw = Math.Max(handDraw, list2.Count);
						handDraw = Math.Min(handDraw, CardPile.MaxCardsInHand);
					}
					await CardPileCmd.Draw(playerChoiceContext, handDraw, player, true);
					this.CombatCt.ThrowIfCancellationRequested();
					await Hook.AfterPlayerTurnStart(state, playerChoiceContext, player);
				}
			}
		}

		/// <summary>
		/// Called in EndPlayerTurnAction to indicate that the player is ready to execute end-of-turn events.
		/// </summary>
		/// <param name="player">The player that readied up.</param>
		/// <param name="canBackOut">In multiplayer, notes if the player is allowed to back out of ending their turn.</param>
		/// <param name="actionDuringEnemyTurn">Optional action to execute during the enemy turn. This is useful for tests.</param>
		// Token: 0x060088FD RID: 35069 RVA: 0x002E10EC File Offset: 0x002DF2EC
		public void SetReadyToEndTurn(Player player, bool canBackOut, [Nullable(new byte[] { 2, 1 })] Func<Task> actionDuringEnemyTurn = null)
		{
			CombatManager.<>c__DisplayClass109_0 CS$<>8__locals1 = new CombatManager.<>c__DisplayClass109_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.player = player;
			CS$<>8__locals1.actionDuringEnemyTurn = actionDuringEnemyTurn;
			using (this._playerReadyLock.EnterScope())
			{
				if (this._playersReadyToEndTurn.Contains(CS$<>8__locals1.player))
				{
					return;
				}
				this._playersReadyToEndTurn.Add(CS$<>8__locals1.player);
			}
			Action<Player, bool> playerEndedTurn = this.PlayerEndedTurn;
			if (playerEndedTurn != null)
			{
				playerEndedTurn(CS$<>8__locals1.player, canBackOut);
			}
			if (this.AllPlayersReadyToEndTurn())
			{
				Log.Debug("All players ready to end turn", 2);
				CS$<>8__locals1.runningAction = RunManager.Instance.ActionExecutor.CurrentlyRunningAction;
				CS$<>8__locals1.scheduledCombat = this._state;
				CombatManager.<>c__DisplayClass109_0 CS$<>8__locals2 = CS$<>8__locals1;
				PlayerCombatState playerCombatState = CS$<>8__locals1.player.PlayerCombatState;
				CS$<>8__locals2.scheduledTurnNumber = ((playerCombatState != null) ? playerCombatState.TurnNumber : (-1));
				Func<Task> func;
				if (CS$<>8__locals1.runningAction != null && ActionQueueSet.IsGameActionPlayerDriven(CS$<>8__locals1.runningAction))
				{
					func = () => CS$<>8__locals1.<>4__this.WaitForActionThenEndTurn(CS$<>8__locals1.runningAction, CS$<>8__locals1.scheduledCombat, CS$<>8__locals1.scheduledTurnNumber, CS$<>8__locals1.player, CS$<>8__locals1.actionDuringEnemyTurn);
				}
				else
				{
					func = () => CS$<>8__locals1.<>4__this.AfterAllPlayersReadyToEndTurn(CS$<>8__locals1.scheduledCombat, CS$<>8__locals1.scheduledTurnNumber, CS$<>8__locals1.player, CS$<>8__locals1.actionDuringEnemyTurn);
				}
				bool inPlayerTurnSetup;
				using (this._playerReadyLock.EnterScope())
				{
					inPlayerTurnSetup = this._inPlayerTurnSetup;
					if (inPlayerTurnSetup)
					{
						this._deferredEndTurnTransition = func;
					}
				}
				if (!inPlayerTurnSetup)
				{
					TaskHelper.RunSafely(func());
				}
			}
		}

		/// <summary>
		/// Runs any end-of-turn transition that was deferred while the player turn was being set up (see
		/// <see cref="M:MegaCrit.Sts2.Core.Combat.CombatManager.SetReadyToEndTurn(MegaCrit.Sts2.Core.Entities.Players.Player,System.Boolean,System.Func{System.Threading.Tasks.Task})" />). Must be called on every exit path of the player-turn setup in
		/// <see cref="M:MegaCrit.Sts2.Core.Combat.CombatManager.StartTurn(System.Func{System.Threading.Tasks.Task})" /> so a deferred transition is never dropped.
		/// </summary>
		// Token: 0x060088FE RID: 35070 RVA: 0x002E1244 File Offset: 0x002DF444
		private void ReleaseDeferredEndTurnTransitionIfNeeded()
		{
			Func<Task> deferredEndTurnTransition;
			using (this._playerReadyLock.EnterScope())
			{
				this._inPlayerTurnSetup = false;
				deferredEndTurnTransition = this._deferredEndTurnTransition;
				this._deferredEndTurnTransition = null;
			}
			if (deferredEndTurnTransition != null)
			{
				TaskHelper.RunSafely(deferredEndTurnTransition());
			}
		}

		// Token: 0x060088FF RID: 35071 RVA: 0x002E129C File Offset: 0x002DF49C
		public void UndoReadyToEndTurn(Player player)
		{
			using (this._playerReadyLock.EnterScope())
			{
				this._playersReadyToEndTurn.Remove(player);
			}
			if (LocalContext.IsMe(player))
			{
				this.PlayerActionsDisabled = false;
			}
			Action<Player> playerUnendedTurn = this.PlayerUnendedTurn;
			if (playerUnendedTurn == null)
			{
				return;
			}
			playerUnendedTurn(player);
		}

		/// <summary>
		/// Call this when the end turn button is pressed to disable local player actions until the start of the next turn.
		/// In multiplayer, this prevents the player from playing cards after they have ended turn.
		/// In both SP and MP, this prevents the player from playing cards before the AfterTurnStart hook has run.
		/// It's important that we do this when the end turn button is pressed, instead of when the EndTurnAction is
		/// processed, because the player might try to execute actions while the end turn action is waiting in the queue.
		/// This is a little fragile; if actions do slip through in MP, it has the potential to cause a state divergence.
		/// Revisit if needed - we might need to discard actions on the host side (which ends up being way more complicated).
		/// </summary>
		// Token: 0x06008900 RID: 35072 RVA: 0x002E12FC File Offset: 0x002DF4FC
		public void OnEndedTurnLocally()
		{
			this.PlayerActionsDisabled = true;
		}

		/// <summary>
		/// Called in ReadyToBeginEnemyTurnAction to indicate that the player is ready to switch to the monster turn (or
		/// extra player turn, if necessary). Note that this is called automatically, and is not player-driven.
		/// </summary>
		/// <param name="player">The player that is ready to switch sides.</param>
		/// <param name="actionDuringEnemyTurn">Optional action to execute during the enemy turn. This is useful for tests.</param>
		// Token: 0x06008901 RID: 35073 RVA: 0x002E1308 File Offset: 0x002DF508
		public void SetReadyToBeginEnemyTurn(Player player, [Nullable(new byte[] { 2, 1 })] Func<Task> actionDuringEnemyTurn = null)
		{
			if (!this.IsInProgress)
			{
				Log.Error("Trying to set player ready to begin enemy turn, but combat is over!", 2);
				return;
			}
			bool flag3;
			using (this._playerReadyLock.EnterScope())
			{
				if (!this._playersReadyToBeginEnemyTurn.Add(player))
				{
					return;
				}
				bool flag = this._state.CurrentSide == CombatSide.Player;
				bool flag2 = (this._playersReadyToBeginEnemyTurn.Count == this._state.Players.Count && flag) || (flag && RunManager.Instance.NetService.Type == NetGameType.Singleplayer);
				flag3 = flag2 && !this._playerToEnemyTransitionFired;
				if (flag3)
				{
					this._playerToEnemyTransitionFired = true;
				}
				else if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(117, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Ignoring ready-to-begin-enemy-turn for player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
					defaultInterpolatedStringHandler.AppendLiteral(": a player-to-enemy transition has already been launched for this turn.");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
			}
			if (flag3)
			{
				TaskHelper.RunSafely(this.AfterAllPlayersReadyToBeginEnemyTurn(actionDuringEnemyTurn));
			}
		}

		/// <returns>
		/// True if the passed player has hit the end turn button, and the next player turn has not yet begun.
		/// </returns>
		// Token: 0x06008902 RID: 35074 RVA: 0x002E1420 File Offset: 0x002DF620
		public bool IsPlayerReadyToEndTurn(Player player)
		{
			bool flag;
			using (this._playerReadyLock.EnterScope())
			{
				flag = this._playersReadyToEndTurn.Contains(player);
			}
			return flag;
		}

		// Token: 0x06008903 RID: 35075 RVA: 0x002E1464 File Offset: 0x002DF664
		public bool AllPlayersReadyToEndTurn()
		{
			bool flag;
			using (this._playerReadyLock.EnterScope())
			{
				flag = this._playersReadyToEndTurn.Count == this._state.Players.Count;
			}
			return RunManager.Instance.IsSingleplayerOrFakeMultiplayer || (flag && this._state.CurrentSide == CombatSide.Player);
		}

		// Token: 0x06008904 RID: 35076 RVA: 0x002E14D8 File Offset: 0x002DF6D8
		private Task EndEnemyTurn(CancellationToken? combatCt = null)
		{
			CombatManager.<EndEnemyTurn>d__116 <EndEnemyTurn>d__;
			<EndEnemyTurn>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<EndEnemyTurn>d__.<>4__this = this;
			<EndEnemyTurn>d__.combatCt = combatCt;
			<EndEnemyTurn>d__.<>1__state = -1;
			<EndEnemyTurn>d__.<>t__builder.Start<CombatManager.<EndEnemyTurn>d__116>(ref <EndEnemyTurn>d__);
			return <EndEnemyTurn>d__.<>t__builder.Task;
		}

		// Token: 0x06008905 RID: 35077 RVA: 0x002E1524 File Offset: 0x002DF724
		public void AddCreature(Creature creature)
		{
			if (!this._state.ContainsCreature(creature))
			{
				throw new InvalidOperationException("CombatState must already contain creature.");
			}
			MonsterModel monster = creature.Monster;
			if (monster != null)
			{
				monster.SetUpForCombat();
			}
			if (creature.SlotName != null)
			{
				this._state.SortEnemiesBySlotName();
			}
			this.StateTracker.Subscribe(creature);
			Action<CombatState> creaturesChanged = this.CreaturesChanged;
			if (creaturesChanged == null)
			{
				return;
			}
			creaturesChanged(this._state);
		}

		/// <summary>
		/// Called after both the Creature has been added to the room _and_ the NCreature is spawned.
		/// </summary>
		/// <param name="creature"></param>
		// Token: 0x06008906 RID: 35078 RVA: 0x002E1590 File Offset: 0x002DF790
		public async Task AfterCreatureAdded(Creature creature)
		{
			await creature.AfterAddedToRoom();
			if (creature.IsEnemy && this._state.CurrentSide == CombatSide.Player)
			{
				creature.Monster.RollMove(this._state.Players.Select<Player, Creature>((Player p) => p.Creature));
			}
		}

		/// <summary>
		/// Check for the player's hand to be empty and run the appropriate hooks if it is.
		///
		/// We can't just do this check every time the hand size changes, because sometimes we're in the middle of a
		/// sequence of effects and we want to wait to check until they're all done.
		///
		/// For example, if we have <see cref="T:MegaCrit.Sts2.Core.Models.Relics.UnceasingTop" /> and the last card in our hand is <see cref="T:MegaCrit.Sts2.Core.Models.Cards.PommelStrike" />
		/// and we play it, we have to wait to check hand size until Pommel Strike is done being played, otherwise we'll
		/// draw two cards (one when your hand becomes "empty" immediately after Pommel Strike moves to the Play pile, and
		/// another after Pommel Strike's draw command executes).
		///
		/// So, instead of automatically doing this check every time the hand size changes, we manually check after a card
		/// is played, and after a potion is used, since these are the two ways a player can manually interact with combat
		/// state (besides ending turn, which should not trigger an empty hand check). We also check once at the start of
		/// the play-capable phase (see <see cref="M:MegaCrit.Sts2.Core.Combat.CombatManager.RunAutoPrePlayPhase(MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext,System.Threading.Tasks.Task,MegaCrit.Sts2.Core.Entities.Players.Player)" />), to catch a hand draw that ends empty because
		/// every card was auto-played as it was drawn. If we ever add more ways, we should add this check in those too,
		/// and update this comment.
		/// </summary>
		/// <param name="choiceContext">Object that keeps context of the action this is called from.</param>
		/// <param name="player">Player whose hand we want to check.</param>
		// Token: 0x06008907 RID: 35079 RVA: 0x002E15DC File Offset: 0x002DF7DC
		public async Task CheckForEmptyHand(PlayerChoiceContext choiceContext, Player player)
		{
			if (this.IsInProgress)
			{
				if (!this.IsExecutingCardOrPotionEffect(player))
				{
					if (!PileType.Hand.GetPile(player).Cards.Any<CardModel>())
					{
						await Hook.AfterHandEmptied(this._state, choiceContext, player);
					}
				}
			}
		}

		/// <summary>
		/// Reset the combat manager to prepare for the next combat.
		/// </summary>
		/// <param name="graceful">Usually true. Only pass false if we're exiting the game completely.</param>
		// Token: 0x06008908 RID: 35080 RVA: 0x002E1630 File Offset: 0x002DF830
		public void Reset(bool graceful)
		{
			CancellationTokenSource combatCts = this._combatCts;
			if (combatCts != null)
			{
				combatCts.Cancel();
			}
			if (graceful && this._state != null)
			{
				this.SetPhaseForAllPlayers(PlayerTurnPhase.None);
				foreach (Creature creature in this._state.Creatures.ToList<Creature>())
				{
					creature.Reset();
					this.RemoveCreature(creature);
					this._state.RemoveCreature(creature, true);
				}
				this._state = null;
			}
			this._pendingLoss = null;
			this.DebugForcedTopCardOnNextShuffle = null;
			this.IsInProgress = false;
			this.IsStarting = false;
			this.IsEnemyTurnStarted = false;
			this.EndingPlayerTurnPhaseOne = false;
			this.EndingPlayerTurnPhaseTwo = false;
			using (this._playerReadyLock.EnterScope())
			{
				this._playersReadyToEndTurn.Clear();
				this._playersReadyToBeginEnemyTurn.Clear();
				this._playerToEnemyTransitionFired = false;
			}
			this.History.Clear();
			this._cardOrPotionEffectDepth.Clear();
			RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.NotInCombat);
		}

		// Token: 0x06008909 RID: 35081 RVA: 0x002E1760 File Offset: 0x002DF960
		public async Task HandlePlayerDeath(Player player)
		{
			if (this.IsInProgress)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" died, doing death handling");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				if (!this.IsExecutingCardOrPotionEffect(player))
				{
					await this.RemoveDeadPlayerCardsFromCombat(player);
				}
				await PlayerCmd.SetEnergy(0m, player);
				await PlayerCmd.SetStars(0m, player);
			}
		}

		/// <summary>
		/// Removes a dead player's cards from combat. Runs from <see cref="M:MegaCrit.Sts2.Core.Combat.CombatManager.HandlePlayerDeath(MegaCrit.Sts2.Core.Entities.Players.Player)" /> when the player is not
		/// mid-effect, otherwise deferred to when their outermost card or potion effect ends.
		/// </summary>
		// Token: 0x0600890A RID: 35082 RVA: 0x002E17AC File Offset: 0x002DF9AC
		public async Task RemoveDeadPlayerCardsFromCombat(Player player)
		{
			if (this.IsInProgress)
			{
				if (player.PlayerCombatState != null)
				{
					if (this._state != null)
					{
						if (!this._state.Players.All<Player>((Player p) => p.Creature.IsDead))
						{
							List<CardModel> list = new List<CardModel>();
							list.AddRange(player.PlayerCombatState.Hand.Cards);
							list.AddRange(player.PlayerCombatState.DrawPile.Cards);
							list.AddRange(player.PlayerCombatState.DiscardPile.Cards);
							list.AddRange(player.PlayerCombatState.ExhaustPile.Cards);
							list.AddRange(player.PlayerCombatState.PlayPile.Cards);
							CardModel[] array = list.ToArray();
							await CardPileCmd.RemoveFromCombat(array, false);
						}
					}
				}
			}
		}

		/// <summary>
		/// Marks combat as pending loss. The actual loss processing happens at the next safe point
		/// (in CheckWinCondition) to avoid race conditions where effects try to run after IsInProgress is false.
		/// </summary>
		// Token: 0x0600890B RID: 35083 RVA: 0x002E17F7 File Offset: 0x002DF9F7
		public void LoseCombat()
		{
			if (this._pendingLoss != null)
			{
				return;
			}
			this._pendingLoss = new CombatManager.PendingLossState(this._state, (CombatRoom)this._state.RunState.CurrentRoom);
		}

		/// <summary>
		/// Processes a pending combat loss. Called from CheckWinCondition at safe points.
		/// </summary>
		// Token: 0x0600890C RID: 35084 RVA: 0x002E1830 File Offset: 0x002DFA30
		private void ProcessPendingLoss()
		{
			if (this._pendingLoss == null)
			{
				return;
			}
			CombatManager.PendingLossState pendingLoss = this._pendingLoss;
			this._pendingLoss = null;
			this.IsInProgress = false;
			Action<CombatRoom> combatEnded = this.CombatEnded;
			if (combatEnded == null)
			{
				return;
			}
			combatEnded(pendingLoss.Room);
		}

		/// <summary>
		/// DO NOT CALL THIS unless you're in this class or ModelTest.
		/// </summary>
		// Token: 0x0600890D RID: 35085 RVA: 0x002E1878 File Offset: 0x002DFA78
		public async Task EndCombatInternal()
		{
			CombatState combatState = this._state;
			Player localPlayer = LocalContext.GetMe(combatState);
			int turnsTaken = localPlayer.PlayerCombatState.TurnNumber;
			IRunState runState = combatState.RunState;
			CombatRoom room = (CombatRoom)runState.CurrentRoom;
			this.IsInProgress = false;
			this.SetPhaseForAllPlayers(PlayerTurnPhase.None);
			this.PlayerActionsDisabled = false;
			using (this._playerReadyLock.EnterScope())
			{
				this._playersTakingExtraTurn.Clear();
			}
			foreach (Player player in combatState.Players)
			{
				await player.ReviveBeforeCombatEnd();
			}
			IEnumerator<Player> enumerator = null;
			await Hook.AfterCombatEnd(runState, combatState, room);
			this.History.Clear();
			room.OnCombatEnded();
			if (RunManager.Instance.NetService.Type != NetGameType.Replay)
			{
				RunManager.Instance.WriteReplay(true);
			}
			foreach (Player player2 in combatState.Players)
			{
				player2.AfterCombatEnd();
			}
			await Hook.AfterCombatVictory(runState, combatState, room);
			NHoverTipSet.Clear();
			if (runState.CurrentMapPointHistoryEntry != null)
			{
				runState.CurrentMapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().TurnsTaken = turnsTaken;
			}
			bool flag = runState.Map.SecondBossMapPoint != null && runState.CurrentMapCoord == runState.Map.SecondBossMapPoint.coord;
			bool flag2 = runState.Map.SecondBossMapPoint == null && runState.CurrentMapCoord == runState.Map.BossMapPoint.coord;
			if (room.RoomType == RoomType.Boss && runState.CurrentActIndex == runState.Acts.Count - 1 && (flag || flag2))
			{
				RunManager.Instance.WinTime = RunManager.Instance.RunTime;
			}
			room.MarkPreFinished();
			await SaveManager.Instance.SaveRun(room, false);
			NMapScreen instance = NMapScreen.Instance;
			if (instance != null)
			{
				instance.SetTravelEnabled(true);
			}
			SaveManager.Instance.UpdateProgressAfterCombatWon(localPlayer, room);
			AchievementsHelper.CheckForDefeatedAllEnemiesAchievement(runState.Act, localPlayer);
			SaveManager.Instance.SaveProgressFile();
			if (room.RoomType == RoomType.Boss)
			{
				AchievementsHelper.AfterBossDefeated(localPlayer);
			}
			MultiplayerScalingModel multiplayerScalingModel = combatState.MultiplayerScalingModel;
			if (multiplayerScalingModel != null)
			{
				multiplayerScalingModel.OnCombatFinished();
			}
			if (this._state != null)
			{
				Action<CombatRoom> combatWon = this.CombatWon;
				if (combatWon != null)
				{
					combatWon(room);
				}
			}
			RunManager.Instance.ActionExecutor.Unpause();
			RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.NotInCombat);
			NRunMusicController instance2 = NRunMusicController.Instance;
			if (instance2 != null)
			{
				instance2.UpdateTrack();
			}
			if (this._state != null)
			{
				Action<CombatRoom> combatEnded = this.CombatEnded;
				if (combatEnded != null)
				{
					combatEnded(room);
				}
			}
		}

		// Token: 0x0600890E RID: 35086 RVA: 0x002E18BC File Offset: 0x002DFABC
		public void RemoveCreature(Creature creature)
		{
			if (creature.IsMonster)
			{
				creature.Monster.BeforeRemovedFromRoom();
				creature.Monster.ResetStateMachine();
			}
			this.StateTracker.Unsubscribe(creature);
			Action<CombatState> creaturesChanged = this.CreaturesChanged;
			if (creaturesChanged == null)
			{
				return;
			}
			creaturesChanged(this._state);
		}

		// Token: 0x0600890F RID: 35087 RVA: 0x002E190C File Offset: 0x002DFB0C
		public async Task<bool> CheckWinCondition()
		{
			bool flag;
			if (this._pendingLoss != null)
			{
				this.ProcessPendingLoss();
				flag = true;
			}
			else if (this.IsEnding)
			{
				await this.EndCombatInternal();
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06008910 RID: 35088 RVA: 0x002E1950 File Offset: 0x002DFB50
		private Task ExecuteEnemyTurn([Nullable(new byte[] { 2, 1 })] Func<Task> actionDuringEnemyTurn = null)
		{
			CombatManager.<ExecuteEnemyTurn>d__128 <ExecuteEnemyTurn>d__;
			<ExecuteEnemyTurn>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ExecuteEnemyTurn>d__.<>4__this = this;
			<ExecuteEnemyTurn>d__.actionDuringEnemyTurn = actionDuringEnemyTurn;
			<ExecuteEnemyTurn>d__.<>1__state = -1;
			<ExecuteEnemyTurn>d__.<>t__builder.Start<CombatManager.<ExecuteEnemyTurn>d__128>(ref <ExecuteEnemyTurn>d__);
			return <ExecuteEnemyTurn>d__.<>t__builder.Task;
		}

		// Token: 0x06008911 RID: 35089 RVA: 0x002E199C File Offset: 0x002DFB9C
		private async Task WaitForActionThenEndTurn(GameAction action, [Nullable(2)] CombatState scheduledCombat, int scheduledTurnNumber, Player scheduledPlayer, [Nullable(new byte[] { 2, 1 })] Func<Task> actionDuringEnemyTurn)
		{
			await action.CompletionTask;
			await this.AfterAllPlayersReadyToEndTurn(scheduledCombat, scheduledTurnNumber, scheduledPlayer, actionDuringEnemyTurn);
		}

		// Token: 0x06008912 RID: 35090 RVA: 0x002E1A0C File Offset: 0x002DFC0C
		private async Task AfterAllPlayersReadyToEndTurn([Nullable(2)] CombatState scheduledCombat, int scheduledTurnNumber, Player scheduledPlayer, [Nullable(new byte[] { 2, 1 })] Func<Task> actionDuringEnemyTurn = null)
		{
			if (this.IsInProgress)
			{
				if (this._state == scheduledCombat)
				{
					PlayerCombatState playerCombatState = scheduledPlayer.PlayerCombatState;
					if (((playerCombatState != null) ? playerCombatState.TurnNumber : (-1)) == scheduledTurnNumber)
					{
						this.CombatCt.ThrowIfCancellationRequested();
						this.EndingPlayerTurnPhaseOne = true;
						RunManager.Instance.ActionQueueSynchronizer.SetCombatState(ActionSynchronizerCombatState.EndTurnPhaseOne);
						await this.WaitUntilQueueIsEmptyOrWaitingOnNonPlayerDrivenAction();
						await this.EndPlayerTurnPhaseOneInternal();
						if (this.IsInProgress && RunManager.Instance.NetService.Type != NetGameType.Replay)
						{
							RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new ReadyToBeginEnemyTurnAction(LocalContext.GetMe(this._state), actionDuringEnemyTurn));
						}
						this.EndingPlayerTurnPhaseOne = false;
						return;
					}
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(104, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Dropping stale player-turn-end transition for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(scheduledPlayer.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(": the combat or turn it was scheduled for has ended");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		// Token: 0x06008913 RID: 35091 RVA: 0x002E1A70 File Offset: 0x002DFC70
		private async Task WaitUntilQueueIsEmptyOrWaitingOnNonPlayerDrivenAction()
		{
			CombatManager.<>c__DisplayClass131_0 CS$<>8__locals1 = new CombatManager.<>c__DisplayClass131_0();
			GameAction currentlyRunningAction = RunManager.Instance.ActionExecutor.CurrentlyRunningAction;
			if (currentlyRunningAction != null && ActionQueueSet.IsGameActionPlayerDriven(currentlyRunningAction))
			{
				CS$<>8__locals1.completionSource = new TaskCompletionSource();
				RunManager.Instance.ActionExecutor.AfterActionExecuted += CS$<>8__locals1.<WaitUntilQueueIsEmptyOrWaitingOnNonPlayerDrivenAction>g__AfterActionExecuted|0;
				await CS$<>8__locals1.completionSource.Task;
				RunManager.Instance.ActionExecutor.AfterActionExecuted -= CS$<>8__locals1.<WaitUntilQueueIsEmptyOrWaitingOnNonPlayerDrivenAction>g__AfterActionExecuted|0;
			}
		}

		/// <summary>
		/// DO NOT CALL THIS unless you're in this class or ModelTest.
		/// This calls all end-of-turn hooks that could require player choices to be made.
		/// </summary>
		// Token: 0x06008914 RID: 35092 RVA: 0x002E1AAC File Offset: 0x002DFCAC
		public async Task EndPlayerTurnPhaseOneInternal()
		{
			if (this._state != null)
			{
				CombatState state = this._state;
				if (state == null || state.CurrentSide != CombatSide.Player)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
					defaultInterpolatedStringHandler.AppendLiteral("EndPlayerTurn called while the current side is ");
					CombatState state2 = this._state;
					defaultInterpolatedStringHandler.AppendFormatted<CombatSide?>((state2 != null) ? new CombatSide?(state2.CurrentSide) : null);
					defaultInterpolatedStringHandler.AppendLiteral("!");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				await this.WaitForUnpause();
				List<Player> playersEndingTurn;
				using (this._playerReadyLock.EnterScope())
				{
					List<Player> list;
					if (this._playersTakingExtraTurn.Count <= 0)
					{
						CombatState state3 = this._state;
						list = ((state3 != null) ? state3.Players.ToList<Player>() : null) ?? new List<Player>();
					}
					else
					{
						list = this._playersTakingExtraTurn.ToList<Player>();
					}
					playersEndingTurn = list;
				}
				List<ValueTuple<Player, HookPlayerChoiceContext>> autoPostPlayContexts = new List<ValueTuple<Player, HookPlayerChoiceContext>>();
				foreach (Player player in playersEndingTurn)
				{
					if (this._state != null && LocalContext.NetId != null)
					{
						Player player;
						player.PlayerCombatState.Phase = PlayerTurnPhase.AutoPostPlay;
						HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(player, LocalContext.NetId.Value, GameActionType.CombatPlayPhaseOnly);
						await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(Hook.AfterAutoPostPlayPhaseEntered(playerChoiceContext, this._state, player));
						autoPostPlayContexts.Add(new ValueTuple<Player, HookPlayerChoiceContext>(player, playerChoiceContext));
						playerChoiceContext = null;
						player = null;
					}
				}
				List<Player>.Enumerator enumerator = default(List<Player>.Enumerator);
				foreach (ValueTuple<Player, HookPlayerChoiceContext> valueTuple in autoPostPlayContexts)
				{
					Player player = valueTuple.Item1;
					await valueTuple.Item2.WaitForCompletion();
					player.PlayerCombatState.Phase = PlayerTurnPhase.End;
					player = null;
				}
				List<ValueTuple<Player, HookPlayerChoiceContext>>.Enumerator enumerator2 = default(List<ValueTuple<Player, HookPlayerChoiceContext>>.Enumerator);
				if (this._state != null)
				{
					await Hook.BeforeSideTurnEnd(this._state, this._state.CurrentSide, playersEndingTurn.Select<Player, Creature>((Player p) => p.Creature));
				}
				if (!(await this.CheckWinCondition()))
				{
					List<HookPlayerChoiceContext> playerEndContexts = new List<HookPlayerChoiceContext>();
					foreach (Player player2 in playersEndingTurn)
					{
						if (LocalContext.NetId != null)
						{
							HookPlayerChoiceContext playerChoiceContext = new HookPlayerChoiceContext(player2, LocalContext.NetId.Value, GameActionType.Combat);
							await playerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(this.DoTurnEnd(player2, playerChoiceContext));
							playerEndContexts.Add(playerChoiceContext);
							playerChoiceContext = null;
						}
					}
					enumerator = default(List<Player>.Enumerator);
					foreach (HookPlayerChoiceContext hookPlayerChoiceContext in playerEndContexts)
					{
						await hookPlayerChoiceContext.WaitForCompletion();
					}
					List<HookPlayerChoiceContext>.Enumerator enumerator3 = default(List<HookPlayerChoiceContext>.Enumerator);
					if (!(await this.CheckWinCondition()))
					{
						if (this._state != null)
						{
							foreach (Player player3 in playersEndingTurn)
							{
								await Hook.BeforeFlush(this._state, player3);
							}
							enumerator = default(List<Player>.Enumerator);
						}
						RunManager.Instance.ChecksumTracker.GenerateChecksum("After player turn phase one end", null);
						await this.CheckWinCondition();
					}
				}
			}
		}

		/// <summary>
		/// Executes turn end hooks for a player.
		/// If player choice occurs during this method, it uses the passed choice context. This way, each player's turn end
		/// runs independently of all others.
		/// </summary>
		// Token: 0x06008915 RID: 35093 RVA: 0x002E1AF0 File Offset: 0x002DFCF0
		private async Task DoTurnEnd(Player player, PlayerChoiceContext choiceContext)
		{
			await player.PlayerCombatState.OrbQueue.BeforeTurnEnd(choiceContext);
			if (!this.IsOverOrEnding)
			{
				CardPile pile = PileType.Hand.GetPile(player);
				List<CardModel> turnEndCards = new List<CardModel>();
				List<CardModel> list = new List<CardModel>();
				foreach (CardModel cardModel in pile.Cards)
				{
					if (cardModel.HasTurnEndInHandEffect)
					{
						turnEndCards.Add(cardModel);
					}
					else if (cardModel.Keywords.Contains(CardKeyword.Ethereal) && Hook.ShouldEtherealTrigger(player.Creature.CombatState, cardModel))
					{
						list.Add(cardModel);
					}
				}
				foreach (CardModel cardModel2 in list)
				{
					await CardCmd.Exhaust(choiceContext, cardModel2, true, false);
				}
				List<CardModel>.Enumerator enumerator2 = default(List<CardModel>.Enumerator);
				foreach (CardModel cardModel3 in turnEndCards)
				{
					await cardModel3.OnTurnEndInHandWrapper(choiceContext);
				}
				enumerator2 = default(List<CardModel>.Enumerator);
			}
		}

		// Token: 0x06008916 RID: 35094 RVA: 0x002E1B44 File Offset: 0x002DFD44
		private async Task EndEnemyTurnInternal()
		{
			List<Creature> enemies = this._state.CreaturesOnCurrentSide.ToList<Creature>();
			await Hook.BeforeSideTurnEnd(this._state, this._state.CurrentSide, enemies);
			foreach (Player player in this._state.Players)
			{
				player.PlayerCombatState.EndOfTurnCleanup();
			}
			await Hook.AfterSideTurnEnd(this._state, this._state.CurrentSide, enemies);
		}

		// Token: 0x06008917 RID: 35095 RVA: 0x002E1B88 File Offset: 0x002DFD88
		private Task AfterAllPlayersReadyToBeginEnemyTurn([Nullable(new byte[] { 2, 1 })] Func<Task> actionDuringEnemyTurn = null)
		{
			CombatManager.<AfterAllPlayersReadyToBeginEnemyTurn>d__135 <AfterAllPlayersReadyToBeginEnemyTurn>d__;
			<AfterAllPlayersReadyToBeginEnemyTurn>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AfterAllPlayersReadyToBeginEnemyTurn>d__.<>4__this = this;
			<AfterAllPlayersReadyToBeginEnemyTurn>d__.actionDuringEnemyTurn = actionDuringEnemyTurn;
			<AfterAllPlayersReadyToBeginEnemyTurn>d__.<>1__state = -1;
			<AfterAllPlayersReadyToBeginEnemyTurn>d__.<>t__builder.Start<CombatManager.<AfterAllPlayersReadyToBeginEnemyTurn>d__135>(ref <AfterAllPlayersReadyToBeginEnemyTurn>d__);
			return <AfterAllPlayersReadyToBeginEnemyTurn>d__.<>t__builder.Task;
		}

		/// <summary>
		/// DO NOT CALL THIS unless you're in this class or ModelTest.
		/// This does all the player state cleanup for the end of their turn. It must not call any hooks that might cause
		/// player choices to occur.
		/// </summary>
		// Token: 0x06008918 RID: 35096 RVA: 0x002E1BD4 File Offset: 0x002DFDD4
		public Task EndPlayerTurnPhaseTwoInternal(CancellationToken? combatCt = null)
		{
			CombatManager.<EndPlayerTurnPhaseTwoInternal>d__136 <EndPlayerTurnPhaseTwoInternal>d__;
			<EndPlayerTurnPhaseTwoInternal>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<EndPlayerTurnPhaseTwoInternal>d__.<>4__this = this;
			<EndPlayerTurnPhaseTwoInternal>d__.combatCt = combatCt;
			<EndPlayerTurnPhaseTwoInternal>d__.<>1__state = -1;
			<EndPlayerTurnPhaseTwoInternal>d__.<>t__builder.Start<CombatManager.<EndPlayerTurnPhaseTwoInternal>d__136>(ref <EndPlayerTurnPhaseTwoInternal>d__);
			return <EndPlayerTurnPhaseTwoInternal>d__.<>t__builder.Task;
		}

		// Token: 0x06008919 RID: 35097 RVA: 0x002E1C20 File Offset: 0x002DFE20
		private async Task FlushPlayerHand(Player player, HookPlayerChoiceContext playerChoiceContext)
		{
			if (!player.Creature.IsDead)
			{
				if (this._state == null || player.PlayerCombatState == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(100, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Combat state is null. Assuming that the run has been cleaned up. (CombatState: ");
					defaultInterpolatedStringHandler.AppendFormatted<CombatState>(this._state);
					defaultInterpolatedStringHandler.AppendLiteral(" PlayerCombatState: ");
					defaultInterpolatedStringHandler.AppendFormatted<PlayerCombatState>(player.PlayerCombatState);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				else
				{
					CombatState state = this._state;
					List<CardModel> cardsToFlush = new List<CardModel>();
					List<CardModel> cardsToRetain = new List<CardModel>();
					bool flag = Hook.ShouldFlush(state, player);
					foreach (CardModel cardModel in PileType.Hand.GetPile(player).Cards)
					{
						if (!flag || cardModel.ShouldRetainThisTurn)
						{
							cardsToRetain.Add(cardModel);
						}
						else
						{
							cardsToFlush.Add(cardModel);
						}
					}
					if (cardsToFlush.Count > 0)
					{
						await CardPileCmd.Add(cardsToFlush, PileType.Discard, CardPilePosition.Bottom, null, false);
						this.CombatCt.ThrowIfCancellationRequested();
					}
					await Hook.AfterFlush(state, player, playerChoiceContext, cardsToFlush, cardsToRetain);
					this.CombatCt.ThrowIfCancellationRequested();
					player.PlayerCombatState.EndOfTurnCleanup();
				}
			}
		}

		/// <summary>
		/// DO NOT CALL THIS unless you're in this class or ModelTest.
		/// This switches from the player side to the enemy side, handling extra player turns if necessary.
		/// </summary>
		/// <param name="actionDuringEnemyTurn">Optional action to execute during the enemy turn. This is useful for tests.</param>
		// Token: 0x0600891A RID: 35098 RVA: 0x002E1C74 File Offset: 0x002DFE74
		public async Task SwitchFromPlayerToEnemySide([Nullable(new byte[] { 2, 1 })] Func<Task> actionDuringEnemyTurn = null)
		{
			if (this._state != null)
			{
				List<Player> list;
				using (this._playerReadyLock.EnterScope())
				{
					this._playersTakingExtraTurn.Clear();
					foreach (Player player in this._state.Players)
					{
						if (Hook.ShouldTakeExtraTurn(this._state, player))
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Player ");
							defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
							defaultInterpolatedStringHandler.AppendLiteral(" (");
							defaultInterpolatedStringHandler.AppendFormatted(player.Character.Id.Entry);
							defaultInterpolatedStringHandler.AppendLiteral(") is taking an extra turn");
							Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							this._playersTakingExtraTurn.Add(player);
						}
					}
					list = this._playersTakingExtraTurn.ToList<Player>();
				}
				this.SwitchSides();
				foreach (Player player2 in list)
				{
					if (this._state == null)
					{
						return;
					}
					await Hook.AfterTakingExtraTurn(this._state, player2);
				}
				List<Player>.Enumerator enumerator2 = default(List<Player>.Enumerator);
				await this.WaitForUnpause();
				await this.StartTurn(actionDuringEnemyTurn);
			}
		}

		// Token: 0x0600891B RID: 35099 RVA: 0x002E1CC0 File Offset: 0x002DFEC0
		private void SwitchSides()
		{
			if (this._state == null)
			{
				return;
			}
			bool flag;
			using (this._playerReadyLock.EnterScope())
			{
				flag = this._playersTakingExtraTurn.Count > 0;
			}
			if (this._state.CurrentSide == CombatSide.Player && !flag)
			{
				this._state.CurrentSide = CombatSide.Enemy;
			}
			else
			{
				this._state.CurrentSide = CombatSide.Player;
				IReadOnlyList<Player> readOnlyList;
				if (flag)
				{
					readOnlyList = this._playersTakingExtraTurn;
				}
				else
				{
					readOnlyList = this._state.Players;
					CombatState state = this._state;
					int roundNumber = state.RoundNumber;
					state.RoundNumber = roundNumber + 1;
				}
				foreach (Player player in readOnlyList)
				{
					player.PlayerCombatState.IncrementTurnNumber();
				}
			}
			foreach (Creature creature in this._state.Creatures)
			{
				creature.OnSideSwitch();
			}
			Action<CombatState> turnEnded = this.TurnEnded;
			if (turnEnded == null)
			{
				return;
			}
			turnEnded(this._state);
		}

		/// <summary>
		/// Pause combat.
		/// </summary>
		// Token: 0x0600891C RID: 35100 RVA: 0x002E1E04 File Offset: 0x002E0004
		public void Pause()
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			if (!this.IsInProgress)
			{
				return;
			}
			this.IsPaused = true;
		}

		/// <summary>
		/// Un-pause combat.
		/// </summary>
		// Token: 0x0600891D RID: 35101 RVA: 0x002E1E1E File Offset: 0x002E001E
		public void Unpause()
		{
			if (NonInteractiveMode.IsActive)
			{
				return;
			}
			this.IsPaused = false;
		}

		/// <summary>
		/// Returns true if the passed player is taking part in the current player turn.
		/// Returns false if some player is taking an extra turn, and it's not us.
		/// If it is not the player turn, then this returns false.
		/// </summary>
		// Token: 0x0600891E RID: 35102 RVA: 0x002E1E2F File Offset: 0x002E002F
		public bool IsPartOfPlayerTurn(Player player)
		{
			CombatState state = this._state;
			return state != null && state.CurrentSide == CombatSide.Player && (this._playersTakingExtraTurn.Count == 0 || this._playersTakingExtraTurn.Contains(player));
		}

		// Token: 0x0600891F RID: 35103 RVA: 0x002E1E68 File Offset: 0x002E0068
		public async Task WaitForUnpause()
		{
			if (!NonInteractiveMode.IsActive)
			{
				while (this.IsPaused && this.IsInProgress && this._state != null)
				{
					await NGame.Instance.AwaitProcessFrame(default(CancellationToken));
				}
			}
		}

		/// <summary>
		/// WARNING: ONLY CALL THIS IN TESTS!
		/// Force the specified card to be moved to the top of the next shuffle.
		/// Useful for tests for shuffle tests where the first card drawn afterwards matters.
		/// </summary>
		/// <param name="card">Card to force to the top.</param>
		// Token: 0x06008920 RID: 35104 RVA: 0x002E1EAB File Offset: 0x002E00AB
		public void DebugForceTopCardOnNextShuffle(CardModel card)
		{
			card.AssertMutable();
			this.DebugForcedTopCardOnNextShuffle = card;
		}

		/// <summary>
		/// WARNING: ONLY CALL THIS IN TESTS!
		/// Clear the forced specified card to be moved to the top of the next shuffle.
		/// Useful for tests for shuffle tests where the first card drawn afterwards matters.
		/// </summary>
		// Token: 0x06008921 RID: 35105 RVA: 0x002E1EBA File Offset: 0x002E00BA
		public void DebugClearForcedTopCardOnNextShuffle()
		{
			this.DebugForcedTopCardOnNextShuffle = null;
		}

		// Token: 0x04002FA2 RID: 12194
		public const int baseHandDrawCount = 5;

		// Token: 0x04002FAF RID: 12207
		private readonly Lock _playerReadyLock = new Lock();

		// Token: 0x04002FB0 RID: 12208
		private readonly HashSet<Player> _playersReadyToEndTurn = new HashSet<Player>();

		// Token: 0x04002FB1 RID: 12209
		private readonly HashSet<Player> _playersReadyToBeginEnemyTurn = new HashSet<Player>();

		// Token: 0x04002FB2 RID: 12210
		private readonly List<Player> _playersTakingExtraTurn = new List<Player>();

		/// <summary>
		/// True once a player-to-enemy transition has been launched for the current player turn. Guarded by
		/// <see cref="F:MegaCrit.Sts2.Core.Combat.CombatManager._playerReadyLock" />; cleared at each player turn start and on Reset. This is the single-flight gate
		/// that makes concurrent turn transitions structurally impossible: however a duplicate or stale ready delivery
		/// slips past the per-delivery guards, only the first can launch the transition for a given turn. Two concurrent
		/// transitions otherwise race the side switch on thread pool threads, and the loser re-runs the player-turn-end
		/// on the enemy side (throwing on a swallowed background task and leaving the action synchronizer paused).
		/// </summary>
		// Token: 0x04002FB3 RID: 12211
		private bool _playerToEnemyTransitionFired;

		/// <summary>
		/// True while <see cref="M:MegaCrit.Sts2.Core.Combat.CombatManager.StartTurn(System.Func{System.Threading.Tasks.Task})" /> is setting up the player turn but has not yet moved to the Play phase.
		/// Guarded by <see cref="F:MegaCrit.Sts2.Core.Combat.CombatManager._playerReadyLock" />.
		/// </summary>
		// Token: 0x04002FB4 RID: 12212
		private bool _inPlayerTurnSetup;

		/// <summary>
		/// If a card ends the turn while <see cref="F:MegaCrit.Sts2.Core.Combat.CombatManager._inPlayerTurnSetup" /> is true (e.g. Void Form auto-played by
		/// Whispering Earring during the AutoPrePlay phase), the end-of-turn transition is stored in this field and run
		/// only once <see cref="M:MegaCrit.Sts2.Core.Combat.CombatManager.StartTurn(System.Func{System.Threading.Tasks.Task})" /> has caused us to move to the Play phase, so it never runs concurrently with
		/// the end of StartTurn (this would be a race condition, and could leave the turn-transition sequence stuck
		/// mid-way, never advancing to the next turn).
		/// A normal end-turn happens after the Play phase has started, so it is never deferred.
		/// Guarded by <see cref="F:MegaCrit.Sts2.Core.Combat.CombatManager._playerReadyLock" />.
		/// </summary>
		// Token: 0x04002FB5 RID: 12213
		[Nullable(new byte[] { 2, 1 })]
		private Func<Task> _deferredEndTurnTransition;

		// Token: 0x04002FB6 RID: 12214
		[Nullable(2)]
		private CombatState _state;

		// Token: 0x04002FB7 RID: 12215
		[Nullable(2)]
		private CancellationTokenSource _combatCts;

		// Token: 0x04002FB8 RID: 12216
		[Nullable(2)]
		private CombatManager.PendingLossState _pendingLoss;

		/// <summary>
		/// Set to true when the player should not be able to interact with their hand or any potions.
		/// </summary>
		// Token: 0x04002FBB RID: 12219
		private bool _playerActionsDisabled;

		// Token: 0x04002FC3 RID: 12227
		private readonly Dictionary<Player, int> _cardOrPotionEffectDepth = new Dictionary<Player, int>();

		// Token: 0x020024DB RID: 9435
		[Nullable(0)]
		private sealed class PendingLossState : IEquatable<CombatManager.PendingLossState>
		{
			// Token: 0x0600C48C RID: 50316 RVA: 0x0043B906 File Offset: 0x00439B06
			public PendingLossState(CombatState State, CombatRoom Room)
			{
				this.State = State;
				this.Room = Room;
				base..ctor();
			}

			// Token: 0x170026DC RID: 9948
			// (get) Token: 0x0600C48D RID: 50317 RVA: 0x0043B91C File Offset: 0x00439B1C
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(CombatManager.PendingLossState);
				}
			}

			// Token: 0x170026DD RID: 9949
			// (get) Token: 0x0600C48E RID: 50318 RVA: 0x0043B928 File Offset: 0x00439B28
			// (set) Token: 0x0600C48F RID: 50319 RVA: 0x0043B930 File Offset: 0x00439B30
			public CombatState State { get; set; }

			// Token: 0x170026DE RID: 9950
			// (get) Token: 0x0600C490 RID: 50320 RVA: 0x0043B939 File Offset: 0x00439B39
			// (set) Token: 0x0600C491 RID: 50321 RVA: 0x0043B941 File Offset: 0x00439B41
			public CombatRoom Room { get; set; }

			// Token: 0x0600C492 RID: 50322 RVA: 0x0043B94C File Offset: 0x00439B4C
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("PendingLossState");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600C493 RID: 50323 RVA: 0x0043B998 File Offset: 0x00439B98
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("State = ");
				builder.Append(this.State);
				builder.Append(", Room = ");
				builder.Append(this.Room);
				return true;
			}

			// Token: 0x0600C494 RID: 50324 RVA: 0x0043B9D2 File Offset: 0x00439BD2
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(CombatManager.PendingLossState left, CombatManager.PendingLossState right)
			{
				return !(left == right);
			}

			// Token: 0x0600C495 RID: 50325 RVA: 0x0043B9DE File Offset: 0x00439BDE
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(CombatManager.PendingLossState left, CombatManager.PendingLossState right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600C496 RID: 50326 RVA: 0x0043B9F2 File Offset: 0x00439BF2
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<CombatState>.Default.GetHashCode(this.<State>k__BackingField)) * -1521134295 + EqualityComparer<CombatRoom>.Default.GetHashCode(this.<Room>k__BackingField);
			}

			// Token: 0x0600C497 RID: 50327 RVA: 0x0043BA32 File Offset: 0x00439C32
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as CombatManager.PendingLossState);
			}

			// Token: 0x0600C498 RID: 50328 RVA: 0x0043BA40 File Offset: 0x00439C40
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(CombatManager.PendingLossState other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<CombatState>.Default.Equals(this.<State>k__BackingField, other.<State>k__BackingField) && EqualityComparer<CombatRoom>.Default.Equals(this.<Room>k__BackingField, other.<Room>k__BackingField));
			}

			// Token: 0x0600C49A RID: 50330 RVA: 0x0043BAA1 File Offset: 0x00439CA1
			[CompilerGenerated]
			private PendingLossState(CombatManager.PendingLossState original)
			{
				this.State = original.<State>k__BackingField;
				this.Room = original.<Room>k__BackingField;
			}

			// Token: 0x0600C49B RID: 50331 RVA: 0x0043BAC1 File Offset: 0x00439CC1
			[CompilerGenerated]
			public void Deconstruct(out CombatState State, out CombatRoom Room)
			{
				State = this.State;
				Room = this.Room;
			}
		}
	}
}
