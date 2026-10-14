using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Entities.Players
{
	// Token: 0x02000C7D RID: 3197
	[NullableContext(1)]
	[Nullable(0)]
	public class Player
	{
		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x060082B3 RID: 33459 RVA: 0x002C223C File Offset: 0x002C043C
		// (remove) Token: 0x060082B4 RID: 33460 RVA: 0x002C2274 File Offset: 0x002C0474
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<RelicModel> RelicObtained;

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x060082B5 RID: 33461 RVA: 0x002C22AC File Offset: 0x002C04AC
		// (remove) Token: 0x060082B6 RID: 33462 RVA: 0x002C22E4 File Offset: 0x002C04E4
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<RelicModel> RelicRemoved;

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x060082B7 RID: 33463 RVA: 0x002C231C File Offset: 0x002C051C
		// (remove) Token: 0x060082B8 RID: 33464 RVA: 0x002C2354 File Offset: 0x002C0554
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<int> MaxPotionCountChanged;

		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x060082B9 RID: 33465 RVA: 0x002C238C File Offset: 0x002C058C
		// (remove) Token: 0x060082BA RID: 33466 RVA: 0x002C23C4 File Offset: 0x002C05C4
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<PotionModel> PotionProcured;

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x060082BB RID: 33467 RVA: 0x002C23FC File Offset: 0x002C05FC
		// (remove) Token: 0x060082BC RID: 33468 RVA: 0x002C2434 File Offset: 0x002C0634
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<PotionModel> PotionDiscarded;

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x060082BD RID: 33469 RVA: 0x002C246C File Offset: 0x002C066C
		// (remove) Token: 0x060082BE RID: 33470 RVA: 0x002C24A4 File Offset: 0x002C06A4
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<PotionModel> UsedPotionRemoved;

		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x060082BF RID: 33471 RVA: 0x002C24DC File Offset: 0x002C06DC
		// (remove) Token: 0x060082C0 RID: 33472 RVA: 0x002C2514 File Offset: 0x002C0714
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action AddPotionFailed;

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x060082C1 RID: 33473 RVA: 0x002C254C File Offset: 0x002C074C
		// (remove) Token: 0x060082C2 RID: 33474 RVA: 0x002C2584 File Offset: 0x002C0784
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action GoldChanged;

		// Token: 0x170023B4 RID: 9140
		// (get) Token: 0x060082C3 RID: 33475 RVA: 0x002C25B9 File Offset: 0x002C07B9
		public int MaxPotionCount
		{
			get
			{
				return this._potionSlots.Count;
			}
		}

		// Token: 0x170023B5 RID: 9141
		// (get) Token: 0x060082C4 RID: 33476 RVA: 0x002C25C6 File Offset: 0x002C07C6
		public CharacterModel Character { get; }

		// Token: 0x170023B6 RID: 9142
		// (get) Token: 0x060082C5 RID: 33477 RVA: 0x002C25CE File Offset: 0x002C07CE
		public Creature Creature { get; }

		// Token: 0x170023B7 RID: 9143
		// (get) Token: 0x060082C6 RID: 33478 RVA: 0x002C25D6 File Offset: 0x002C07D6
		public ulong NetId { get; }

		/// <summary>
		/// Player-scoped RNG set.
		/// Note that this state is not deterministic outside of combat, events and rest sites
		/// This is initialized with a seed of 0, but is updated to a real seed when added to a <see cref="P:MegaCrit.Sts2.Core.Entities.Players.Player.RunState" />.
		/// </summary>
		// Token: 0x170023B8 RID: 9144
		// (get) Token: 0x060082C7 RID: 33479 RVA: 0x002C25DE File Offset: 0x002C07DE
		// (set) Token: 0x060082C8 RID: 33480 RVA: 0x002C25E6 File Offset: 0x002C07E6
		public PlayerRngSet PlayerRng { get; private set; }

		/// <summary>
		/// Player-scoped odds set.
		/// Note that this state is not deterministic outside of combat, events and rest sites.
		/// This is initialized with a seed of 0, but is updated to a real seed when added to a <see cref="P:MegaCrit.Sts2.Core.Entities.Players.Player.RunState" />.
		/// </summary>
		// Token: 0x170023B9 RID: 9145
		// (get) Token: 0x060082C9 RID: 33481 RVA: 0x002C25EF File Offset: 0x002C07EF
		// (set) Token: 0x060082CA RID: 33482 RVA: 0x002C25F7 File Offset: 0x002C07F7
		public PlayerOddsSet PlayerOdds { get; private set; }

		/// <summary>
		/// Player-scoped relic grab bag.
		/// See <see cref="P:MegaCrit.Sts2.Core.Runs.IRunState.SharedRelicGrabBag" /> for the difference between this and the shared grab bag.
		/// Note that this state is not deterministic outside of combat, events and rest sites.
		/// </summary>
		// Token: 0x170023BA RID: 9146
		// (get) Token: 0x060082CB RID: 33483 RVA: 0x002C2600 File Offset: 0x002C0800
		public RelicGrabBag RelicGrabBag { get; }

		/// <summary>
		/// The set of unlocks the player entered the run with.
		/// This serves several purposes:
		/// - In multiplayer, this keeps track of each player's unlocks so that we generate only cards/potions/relics that
		///   the player has unlocked.
		/// - In both singleplayer and multiplayer, unlocking epochs outside of the run does not change the unlocks in a
		///   run that you load. The unlocks are saved into the run save file.
		/// </summary>
		// Token: 0x170023BB RID: 9147
		// (get) Token: 0x060082CC RID: 33484 RVA: 0x002C2608 File Offset: 0x002C0808
		public UnlockState UnlockState { get; }

		/// <summary>
		/// The state of the run that the player is in.
		/// Will usually be an instance of <see cref="P:MegaCrit.Sts2.Core.Entities.Players.Player.RunState" />, but will be <see cref="T:MegaCrit.Sts2.Core.Runs.NullRunState" /> in some spots
		/// for testing, debugging, or one-off combat scenarios. You should never have to check for this though, as
		/// <see cref="T:MegaCrit.Sts2.Core.Runs.NullRunState" /> should always behave appropriately.
		/// </summary>
		// Token: 0x170023BC RID: 9148
		// (get) Token: 0x060082CD RID: 33485 RVA: 0x002C2610 File Offset: 0x002C0810
		// (set) Token: 0x060082CE RID: 33486 RVA: 0x002C2618 File Offset: 0x002C0818
		public IRunState RunState
		{
			get
			{
				return this._runState;
			}
			set
			{
				if (!(this._runState is NullRunState))
				{
					throw new InvalidOperationException("RunState has already been set.");
				}
				this._runState = value;
			}
		}

		/// <summary>
		/// This is almost equivalent to Creature.IsAlive, except for a brief period of time between when the player's HP
		/// reaches zero and when DieInternal is called.
		/// It is used to allow hooks that prevent death to run before the player is fully considered dead.
		/// This is on Player and not Creature because monsters use a different mechanism for death prevention. Since
		/// creatures are always removed from combat after they are fully dead, their powers are always iterated, and we
		/// rely on creature removal to remove their powers from Hook. Players are different - they stick around after they
		/// are fully dead, and so we rely on this flag to stop model iteration.
		/// </summary>
		// Token: 0x170023BD RID: 9149
		// (get) Token: 0x060082CF RID: 33487 RVA: 0x002C2639 File Offset: 0x002C0839
		// (set) Token: 0x060082D0 RID: 33488 RVA: 0x002C2641 File Offset: 0x002C0841
		public bool IsActiveForHooks { get; private set; }

		// Token: 0x170023BE RID: 9150
		// (get) Token: 0x060082D1 RID: 33489 RVA: 0x002C264A File Offset: 0x002C084A
		// (set) Token: 0x060082D2 RID: 33490 RVA: 0x002C2652 File Offset: 0x002C0852
		[Nullable(2)]
		public PlayerCombatState PlayerCombatState
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x170023BF RID: 9151
		// (get) Token: 0x060082D3 RID: 33491 RVA: 0x002C265B File Offset: 0x002C085B
		// (set) Token: 0x060082D4 RID: 33492 RVA: 0x002C2663 File Offset: 0x002C0863
		public ExtraPlayerFields ExtraFields { get; private set; } = new ExtraPlayerFields();

		// Token: 0x170023C0 RID: 9152
		// (get) Token: 0x060082D5 RID: 33493 RVA: 0x002C266C File Offset: 0x002C086C
		public IReadOnlyList<RelicModel> Relics
		{
			get
			{
				return this._relics;
			}
		}

		// Token: 0x170023C1 RID: 9153
		// (get) Token: 0x060082D6 RID: 33494 RVA: 0x002C2674 File Offset: 0x002C0874
		[Nullable(new byte[] { 1, 2 })]
		public IReadOnlyList<PotionModel> PotionSlots
		{
			[return: Nullable(new byte[] { 1, 2 })]
			get
			{
				return this._potionSlots;
			}
		}

		// Token: 0x170023C2 RID: 9154
		// (get) Token: 0x060082D7 RID: 33495 RVA: 0x002C267C File Offset: 0x002C087C
		public IEnumerable<PotionModel> Potions
		{
			get
			{
				return this._potionSlots.Where<PotionModel>((PotionModel p) => p != null).OfType<PotionModel>();
			}
		}

		/// <summary>
		/// Get this player's Osty pet Creature.
		/// If Osty is not in combat, this will return null.
		/// If Osty is dead, this will return the dead Osty Creature instance.
		///
		/// Note: If you want to check that Osty is both present in combat and alive, use <see cref="P:MegaCrit.Sts2.Core.Entities.Players.Player.IsOstyAlive" />.
		/// If you want to check that Osty is missing from combat or dead, use <see cref="P:MegaCrit.Sts2.Core.Entities.Players.Player.IsOstyMissing" />.
		/// </summary>
		// Token: 0x170023C3 RID: 9155
		// (get) Token: 0x060082D8 RID: 33496 RVA: 0x002C26AD File Offset: 0x002C08AD
		[Nullable(2)]
		public Creature Osty
		{
			[NullableContext(2)]
			get
			{
				PlayerCombatState playerCombatState = this.PlayerCombatState;
				if (playerCombatState == null)
				{
					return null;
				}
				return playerCombatState.GetPet<Osty>();
			}
		}

		/// <summary>
		/// Is Osty present in combat and alive?
		/// </summary>
		// Token: 0x170023C4 RID: 9156
		// (get) Token: 0x060082D9 RID: 33497 RVA: 0x002C26C0 File Offset: 0x002C08C0
		public bool IsOstyAlive
		{
			get
			{
				Creature osty = this.Osty;
				return osty != null && osty.IsAlive;
			}
		}

		/// <summary>
		/// Is Osty missing from combat or dead?
		/// </summary>
		// Token: 0x170023C5 RID: 9157
		// (get) Token: 0x060082DA RID: 33498 RVA: 0x002C26DF File Offset: 0x002C08DF
		public bool IsOstyMissing
		{
			get
			{
				return !this.IsOstyAlive;
			}
		}

		// Token: 0x060082DB RID: 33499 RVA: 0x002C26EC File Offset: 0x002C08EC
		public bool HasEventPet()
		{
			if (!this.Relics.Any<RelicModel>((RelicModel r) => r.AddsPet))
			{
				return this.Deck.Cards.Any<CardModel>((CardModel c) => c is ByrdonisEgg);
			}
			return true;
		}

		// Token: 0x170023C6 RID: 9158
		// (get) Token: 0x060082DC RID: 33500 RVA: 0x002C2756 File Offset: 0x002C0956
		// (set) Token: 0x060082DD RID: 33501 RVA: 0x002C275E File Offset: 0x002C095E
		public int Gold
		{
			get
			{
				return this._gold;
			}
			set
			{
				if (value == this.Gold)
				{
					return;
				}
				this._gold = value;
				Action goldChanged = this.GoldChanged;
				if (goldChanged == null)
				{
					return;
				}
				goldChanged();
			}
		}

		/// <summary>
		/// The player's character's max ascension level when the run started.
		/// We need to keep track of this in order to properly show the "ascension unlocked" message when winning a run.
		/// </summary>
		// Token: 0x170023C7 RID: 9159
		// (get) Token: 0x060082DE RID: 33502 RVA: 0x002C2781 File Offset: 0x002C0981
		public int MaxAscensionWhenRunStarted { get; }

		// Token: 0x170023C8 RID: 9160
		// (get) Token: 0x060082DF RID: 33503 RVA: 0x002C2789 File Offset: 0x002C0989
		public bool HasOpenPotionSlots
		{
			get
			{
				return this._potionSlots.Any<PotionModel>((PotionModel p) => p == null);
			}
		}

		// Token: 0x140000BA RID: 186
		// (add) Token: 0x060082E0 RID: 33504 RVA: 0x002C27B8 File Offset: 0x002C09B8
		// (remove) Token: 0x060082E1 RID: 33505 RVA: 0x002C27F0 File Offset: 0x002C09F0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action CanUseOrRemovePotionsChanged;

		/// <summary>
		/// Can this player use or remove potions?
		/// Usually true, but set to false during some sensitive timing spots.
		/// </summary>
		// Token: 0x170023C9 RID: 9161
		// (get) Token: 0x060082E2 RID: 33506 RVA: 0x002C2825 File Offset: 0x002C0A25
		// (set) Token: 0x060082E3 RID: 33507 RVA: 0x002C282D File Offset: 0x002C0A2D
		public bool CanUseOrRemovePotions
		{
			get
			{
				return this._canUseOrRemovePotions;
			}
			set
			{
				this._canUseOrRemovePotions = value;
				Action canUseOrRemovePotionsChanged = this.CanUseOrRemovePotionsChanged;
				if (canUseOrRemovePotionsChanged == null)
				{
					return;
				}
				canUseOrRemovePotionsChanged();
			}
		}

		/// <summary>
		/// Has this player's inventory been populated?
		/// A player object is initially created unpopulated, but the various populating methods should be called before
		/// the run they're in has fully been launched.
		/// </summary>
		// Token: 0x170023CA RID: 9162
		// (get) Token: 0x060082E4 RID: 33508 RVA: 0x002C2846 File Offset: 0x002C0A46
		private bool IsInventoryPopulated
		{
			get
			{
				return this.Deck.Cards.Any<CardModel>() || this.Relics.Any<RelicModel>() || this.Potions.Any<PotionModel>();
			}
		}

		// Token: 0x170023CB RID: 9163
		// (get) Token: 0x060082E5 RID: 33509 RVA: 0x002C2874 File Offset: 0x002C0A74
		public CardPile Deck { get; } = new CardPile(PileType.Deck);

		// Token: 0x170023CC RID: 9164
		// (get) Token: 0x060082E6 RID: 33510 RVA: 0x002C287C File Offset: 0x002C0A7C
		// (set) Token: 0x060082E7 RID: 33511 RVA: 0x002C2884 File Offset: 0x002C0A84
		public int MaxEnergy { get; set; }

		// Token: 0x170023CD RID: 9165
		// (get) Token: 0x060082E8 RID: 33512 RVA: 0x002C288D File Offset: 0x002C0A8D
		// (set) Token: 0x060082E9 RID: 33513 RVA: 0x002C2895 File Offset: 0x002C0A95
		public List<ModelId> DiscoveredCards { get; set; }

		// Token: 0x170023CE RID: 9166
		// (get) Token: 0x060082EA RID: 33514 RVA: 0x002C289E File Offset: 0x002C0A9E
		// (set) Token: 0x060082EB RID: 33515 RVA: 0x002C28A6 File Offset: 0x002C0AA6
		public List<ModelId> DiscoveredRelics { get; set; }

		// Token: 0x170023CF RID: 9167
		// (get) Token: 0x060082EC RID: 33516 RVA: 0x002C28AF File Offset: 0x002C0AAF
		// (set) Token: 0x060082ED RID: 33517 RVA: 0x002C28B7 File Offset: 0x002C0AB7
		public List<ModelId> DiscoveredPotions { get; set; }

		// Token: 0x170023D0 RID: 9168
		// (get) Token: 0x060082EE RID: 33518 RVA: 0x002C28C0 File Offset: 0x002C0AC0
		// (set) Token: 0x060082EF RID: 33519 RVA: 0x002C28C8 File Offset: 0x002C0AC8
		public List<ModelId> DiscoveredEnemies { get; set; }

		// Token: 0x170023D1 RID: 9169
		// (get) Token: 0x060082F0 RID: 33520 RVA: 0x002C28D1 File Offset: 0x002C0AD1
		// (set) Token: 0x060082F1 RID: 33521 RVA: 0x002C28D9 File Offset: 0x002C0AD9
		public List<string> DiscoveredEpochs { get; set; }

		// Token: 0x170023D2 RID: 9170
		// (get) Token: 0x060082F2 RID: 33522 RVA: 0x002C28E2 File Offset: 0x002C0AE2
		// (set) Token: 0x060082F3 RID: 33523 RVA: 0x002C28EA File Offset: 0x002C0AEA
		public int BaseOrbSlotCount { get; set; }

		// Token: 0x170023D3 RID: 9171
		// (get) Token: 0x060082F4 RID: 33524 RVA: 0x002C28F4 File Offset: 0x002C0AF4
		public IEnumerable<CardPile> Piles
		{
			get
			{
				if (this._runPiles == null)
				{
					this._runPiles = new CardPile[] { this.Deck };
				}
				PlayerCombatState playerCombatState = this.PlayerCombatState;
				return (((playerCombatState != null) ? playerCombatState.AllPiles : null) ?? Array.Empty<CardPile>()).Concat<CardPile>(this._runPiles);
			}
		}

		// Token: 0x060082F5 RID: 33525 RVA: 0x002C2944 File Offset: 0x002C0B44
		private Player(CharacterModel character, ulong netId, int currentHp, int maxHp, int maxEnergy, int gold, int potionSlotCount, int orbSlotCount, RelicGrabBag sharedRelicGrabBag, UnlockState unlockState, [Nullable(new byte[] { 2, 1 })] List<ModelId> discoveredCards = null, [Nullable(new byte[] { 2, 1 })] List<ModelId> discoveredEnemies = null, [Nullable(new byte[] { 2, 1 })] List<string> discoveredEpochs = null, [Nullable(new byte[] { 2, 1 })] List<ModelId> discoveredPotions = null, [Nullable(new byte[] { 2, 1 })] List<ModelId> discoveredRelics = null)
		{
			this.RunState = NullRunState.Instance;
			this.Character = character;
			this.NetId = netId;
			this.Creature = new Creature(this, currentHp, maxHp);
			this.MaxEnergy = maxEnergy;
			this.Gold = gold;
			this.SetMaxPotionCountInternal(potionSlotCount);
			this.BaseOrbSlotCount = orbSlotCount;
			this.RelicGrabBag = sharedRelicGrabBag;
			this.UnlockState = unlockState;
			this.PlayerRng = new PlayerRngSet(0UL);
			this.PlayerOdds = new PlayerOddsSet(this.PlayerRng);
			this.DiscoveredCards = discoveredCards ?? new List<ModelId>();
			this.DiscoveredEnemies = discoveredEnemies ?? new List<ModelId>();
			this.DiscoveredEpochs = discoveredEpochs ?? new List<string>();
			this.DiscoveredPotions = discoveredPotions ?? new List<ModelId>();
			this.DiscoveredRelics = discoveredRelics ?? new List<ModelId>();
			this.IsActiveForHooks = this.Creature.IsAlive;
			SaveManager instance = SaveManager.Instance;
			CharacterStats characterStats = ((instance != null) ? instance.Progress.GetStatsForCharacter(this.Character.Id) : null);
			this.MaxAscensionWhenRunStarted = ((characterStats != null) ? characterStats.MaxAscension : 0);
		}

		/// <summary>
		/// Create a new player for use at the start of a new run.
		/// The player's inventory will be populated with the chosen character's starting cards, relics, etc., but these
		/// models will not work properly until the player is added to a <see cref="P:MegaCrit.Sts2.Core.Entities.Players.Player.RunState" /> and/or
		/// <see cref="T:MegaCrit.Sts2.Core.Combat.CombatState" />.
		/// </summary>
		/// <param name="unlockState">The set of unlocks the player entered the run with.</param>
		/// <param name="netId">ID for uniquely identifying this player in multiplayer games.</param>
		/// <typeparam name="T">The type of the character that the player is playing as.</typeparam>
		/// <returns>A new player with an empty inventory.</returns>
		// Token: 0x060082F6 RID: 33526 RVA: 0x002C2AA6 File Offset: 0x002C0CA6
		public static Player CreateForNewRun<[Nullable(0)] T>(UnlockState unlockState, ulong netId) where T : CharacterModel
		{
			return Player.CreateForNewRun(ModelDb.Character<T>(), unlockState, netId);
		}

		/// <summary>
		/// Create a new player for use at the start of a new run.
		/// The player's inventory will be populated with the chosen character's starting cards, relics, etc., but these
		/// models will not work properly until the player is added to a <see cref="P:MegaCrit.Sts2.Core.Entities.Players.Player.RunState" /> and/or
		/// <see cref="T:MegaCrit.Sts2.Core.Combat.CombatState" />.
		/// </summary>
		/// <param name="character">The character that the player is playing as.</param>
		/// <param name="unlockState">The set of unlocks the player entered the run with.</param>
		/// <param name="netId">ID for uniquely identifying this player in multiplayer games.</param>
		/// <returns>A new player with an empty inventory.</returns>
		// Token: 0x060082F7 RID: 33527 RVA: 0x002C2ABC File Offset: 0x002C0CBC
		public static Player CreateForNewRun(CharacterModel character, UnlockState unlockState, ulong netId)
		{
			Player player = new Player(character, netId, character.StartingHp, character.StartingHp, character.MaxEnergy, character.StartingGold, 3, character.BaseOrbSlotCount, new RelicGrabBag(), unlockState, null, null, null, null, null);
			player.PopulateStartingInventory();
			return player;
		}

		/// <summary>
		/// Load a player from a SerializablePlayer.
		/// The player's inventory will be populated with the cards, relics, etc. from the SerializablePlayer., but these
		/// models will not work properly until the player is added to a <see cref="P:MegaCrit.Sts2.Core.Entities.Players.Player.RunState" /> and/or
		/// <see cref="T:MegaCrit.Sts2.Core.Combat.CombatState" />.
		/// </summary>
		// Token: 0x060082F8 RID: 33528 RVA: 0x002C2B04 File Offset: 0x002C0D04
		public static Player FromSerializable(SerializablePlayer save)
		{
			Player player = new Player(ModelDb.GetById<CharacterModel>(save.CharacterId), save.NetId, save.CurrentHp, save.MaxHp, save.MaxEnergy, save.Gold, save.MaxPotionSlotCount, save.BaseOrbSlotCount, RelicGrabBag.FromSerializable(save.RelicGrabBag), UnlockState.FromSerializable(save.UnlockState), save.DiscoveredCards.ToList<ModelId>(), save.DiscoveredEnemies.ToList<ModelId>(), save.DiscoveredEpochs.ToList<string>(), save.DiscoveredPotions.ToList<ModelId>(), save.DiscoveredRelics.ToList<ModelId>());
			player.PlayerRng = PlayerRngSet.FromSerializable(save.Rng);
			player.PlayerOdds = PlayerOddsSet.FromSerializable(save.Odds, player.PlayerRng);
			player.ExtraFields = ExtraPlayerFields.FromSerializable(save.ExtraFields);
			player.LoadInventory(save);
			return player;
		}

		// Token: 0x060082F9 RID: 33529 RVA: 0x002C2BDA File Offset: 0x002C0DDA
		public void InitializeSeed(string seed)
		{
			this.PlayerRng = new PlayerRngSet(StringHelper.GetDeterministicHashCode(seed) + (ulong)((long)this._runState.GetPlayerSlotIndex(this)));
			this.PlayerOdds = new PlayerOddsSet(this.PlayerRng);
		}

		// Token: 0x060082FA RID: 33530 RVA: 0x002C2C0C File Offset: 0x002C0E0C
		private void PopulateStartingInventory()
		{
			if (this.IsInventoryPopulated)
			{
				throw new InvalidOperationException("Inventory is already populated.");
			}
			if (!(this.RunState is NullRunState))
			{
				throw new InvalidOperationException("A player's starting inventory must be populated before being added to a run.");
			}
			this.PopulateStartingDeck();
			this.PopulateStartingRelics();
			foreach (PotionModel potionModel in this.Character.StartingPotions.Select<PotionModel, PotionModel>((PotionModel p) => p.ToMutable()))
			{
				this.AddPotionInternal(potionModel, -1, false);
			}
		}

		// Token: 0x060082FB RID: 33531 RVA: 0x002C2CBC File Offset: 0x002C0EBC
		private void LoadInventory(SerializablePlayer save)
		{
			if (this.IsInventoryPopulated)
			{
				throw new InvalidOperationException("Inventory is already populated.");
			}
			if (!(this.RunState is NullRunState))
			{
				throw new InvalidOperationException("A player's inventory must be loaded before being added to a run.");
			}
			IEnumerable<SerializableCard> deck = save.Deck;
			Func<SerializableCard, CardModel> func;
			if ((func = Player.<>O.<0>__FromSerializable) == null)
			{
				func = (Player.<>O.<0>__FromSerializable = new Func<SerializableCard, CardModel>(CardModel.FromSerializable));
			}
			this.PopulateDeck(deck.Select<SerializableCard, CardModel>(func), false);
			this.LoadPotions(save.Potions, false);
			IEnumerable<SerializableRelic> relics = save.Relics;
			Func<SerializableRelic, RelicModel> func2;
			if ((func2 = Player.<>O.<1>__FromSerializable) == null)
			{
				func2 = (Player.<>O.<1>__FromSerializable = new Func<SerializableRelic, RelicModel>(RelicModel.FromSerializable));
			}
			this.PopulateRelics(relics.Select<SerializableRelic, RelicModel>(func2), false);
		}

		// Token: 0x060082FC RID: 33532 RVA: 0x002C2D5B File Offset: 0x002C0F5B
		public void PopulateRelicGrabBagIfNecessary(Rng rng)
		{
			if (this.RelicGrabBag.IsPopulated)
			{
				return;
			}
			this.RelicGrabBag.Populate(this, rng);
		}

		// Token: 0x060082FD RID: 33533 RVA: 0x002C2D78 File Offset: 0x002C0F78
		public SerializablePlayer ToSerializable()
		{
			SerializablePlayer serializablePlayer = new SerializablePlayer();
			serializablePlayer.CharacterId = this.Character.Id;
			serializablePlayer.CurrentHp = this.Creature.CurrentHp;
			serializablePlayer.MaxHp = this.Creature.MaxHp;
			serializablePlayer.MaxEnergy = this.MaxEnergy;
			serializablePlayer.MaxPotionSlotCount = this.MaxPotionCount;
			serializablePlayer.BaseOrbSlotCount = this.BaseOrbSlotCount;
			serializablePlayer.NetId = this.NetId;
			serializablePlayer.Gold = this.Gold;
			serializablePlayer.Rng = this.PlayerRng.ToSerializable();
			serializablePlayer.Odds = this.PlayerOdds.ToSerializable();
			serializablePlayer.RelicGrabBag = this.RelicGrabBag.ToSerializable();
			serializablePlayer.Deck = this.Deck.Cards.Select<CardModel, SerializableCard>((CardModel c) => c.ToSerializable()).ToList<SerializableCard>();
			serializablePlayer.Relics = this.Relics.Select<RelicModel, SerializableRelic>((RelicModel r) => r.ToSerializable()).ToList<SerializableRelic>();
			serializablePlayer.Potions = this.PotionSlots.Select<PotionModel, SerializablePotion>(delegate(PotionModel p, int i)
			{
				if (p == null)
				{
					return null;
				}
				return p.ToSerializable(i);
			}).OfType<SerializablePotion>().ToList<SerializablePotion>();
			serializablePlayer.ExtraFields = this.ExtraFields.ToSerializable();
			serializablePlayer.UnlockState = this.UnlockState.ToSerializable();
			serializablePlayer.DiscoveredCards = this.DiscoveredCards.ToList<ModelId>();
			serializablePlayer.DiscoveredEnemies = this.DiscoveredEnemies.ToList<ModelId>();
			serializablePlayer.DiscoveredEpochs = this.DiscoveredEpochs.ToList<string>();
			serializablePlayer.DiscoveredPotions = this.DiscoveredPotions.ToList<ModelId>();
			serializablePlayer.DiscoveredRelics = this.DiscoveredRelics.ToList<ModelId>();
			return serializablePlayer;
		}

		// Token: 0x060082FE RID: 33534 RVA: 0x002C2F4C File Offset: 0x002C114C
		public void SyncWithSerializedPlayer(SerializablePlayer player)
		{
			if (player.NetId != this.NetId)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(79, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to sync player that has net ID ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" with SerializablePlayer that has net ID ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (player.CharacterId != this.Character.Id)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Character changed for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.NetId);
				defaultInterpolatedStringHandler.AppendLiteral("! This is not allowed");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Creature.SetMaxHpInternal(player.MaxHp);
			this.Creature.SetCurrentHpInternal(player.CurrentHp);
			this.MaxEnergy = player.MaxEnergy;
			this.Gold = player.Gold;
			this.SetMaxPotionCountInternal(player.MaxPotionSlotCount);
			this.Deck.Clear(true);
			foreach (RelicModel relicModel in this._relics.ToList<RelicModel>())
			{
				this.RemoveRelicInternal(relicModel, true);
			}
			foreach (PotionModel potionModel in this._potionSlots.ToList<PotionModel>())
			{
				if (potionModel != null)
				{
					this.DiscardPotionInternal(potionModel, true);
				}
			}
			this.PopulateDeck(player.Deck.Select<SerializableCard, CardModel>((SerializableCard c) => this.RunState.LoadCard(c, this)), true);
			IEnumerable<SerializableRelic> relics = player.Relics;
			Func<SerializableRelic, RelicModel> func;
			if ((func = Player.<>O.<1>__FromSerializable) == null)
			{
				func = (Player.<>O.<1>__FromSerializable = new Func<SerializableRelic, RelicModel>(RelicModel.FromSerializable));
			}
			this.PopulateRelics(relics.Select<SerializableRelic, RelicModel>(func), true);
			this.LoadPotions(player.Potions, true);
			this.PlayerRng.LoadFromSerializable(player.Rng);
			this.PlayerOdds.LoadFromSerializable(player.Odds);
			this.RelicGrabBag.LoadFromSerializable(player.RelicGrabBag);
			this.DiscoveredCards = player.DiscoveredCards.ToList<ModelId>();
			this.DiscoveredEnemies = player.DiscoveredEnemies.ToList<ModelId>();
			this.DiscoveredEpochs = player.DiscoveredEpochs.ToList<string>();
			this.DiscoveredPotions = player.DiscoveredPotions.ToList<ModelId>();
			this.DiscoveredRelics = player.DiscoveredRelics.ToList<ModelId>();
			this.ExtraFields = ExtraPlayerFields.FromSerializable(player.ExtraFields);
			this.IsActiveForHooks = this.Creature.IsAlive;
		}

		/// <summary>
		/// NEVER CALL THIS!
		/// Only RelicCmd.Obtain and save/load stuff should be calling this.
		/// </summary>
		/// <param name="relic">Relic to add.</param>
		/// <param name="index">Index at which relic should be inserted. -1 means at the end.</param>
		/// <param name="silent">If true, RelicObtained will not be called.</param>
		// Token: 0x060082FF RID: 33535 RVA: 0x002C3208 File Offset: 0x002C1408
		public void AddRelicInternal(RelicModel relic, int index = -1, bool silent = false)
		{
			relic.AssertMutable();
			relic.Owner = this;
			if (index == -1)
			{
				this._relics.Add(relic);
			}
			else
			{
				this._relics.Insert(index, relic);
			}
			if (relic != null && !relic.IsMelted && relic.ShouldFlashOnPlayer)
			{
				relic.Flashed += this.OnRelicFlashed;
			}
			if (!silent)
			{
				Action<RelicModel> relicObtained = this.RelicObtained;
				if (relicObtained == null)
				{
					return;
				}
				relicObtained(relic);
			}
		}

		/// <summary>
		/// NEVER CALL THIS!
		/// ONLY RelicCmd.Remove should be calling this.
		/// </summary>
		/// <param name="relic">Relic to remove.</param>
		/// <param name="silent">If true, RelicRemoved will not be called.</param>
		// Token: 0x06008300 RID: 33536 RVA: 0x002C327C File Offset: 0x002C147C
		public void RemoveRelicInternal(RelicModel relic, bool silent = false)
		{
			if (!this._relics.Contains(relic))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Player does not have relic ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(relic.Id);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._relics.Remove(relic);
			relic.RemoveInternal();
			if (relic.ShouldFlashOnPlayer)
			{
				relic.Flashed -= this.OnRelicFlashed;
			}
			if (!silent)
			{
				Action<RelicModel> relicRemoved = this.RelicRemoved;
				if (relicRemoved == null)
				{
					return;
				}
				relicRemoved(relic);
			}
		}

		/// <summary>
		/// NEVER CALL THIS!
		/// ONLY RelicCmd.Melt should be calling this.
		/// </summary>
		/// <param name="relic">Relic to melt.</param>
		// Token: 0x06008301 RID: 33537 RVA: 0x002C3308 File Offset: 0x002C1508
		public void MeltRelicInternal(RelicModel relic)
		{
			if (!relic.IsWax)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(relic.Id);
				defaultInterpolatedStringHandler.AppendLiteral(" is not wax.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (relic.IsMelted)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(relic.Id);
				defaultInterpolatedStringHandler.AppendLiteral(" is already melted.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (!this._relics.Contains(relic))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Player does not have relic ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(relic.Id);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (relic.ShouldFlashOnPlayer)
			{
				relic.Flashed -= this.OnRelicFlashed;
			}
			relic.IsMelted = true;
			relic.Status = RelicStatus.Disabled;
		}

		/// <summary>
		/// Get one of this player's relics.
		/// If the player has multiple of the same type of relic, just get the first one.
		/// If the player has none of this type of relic, returns null.
		/// </summary>
		/// <typeparam name="T">Type of relic to get.</typeparam>
		/// <returns>Matching relic.</returns>
		// Token: 0x06008302 RID: 33538 RVA: 0x002C33EB File Offset: 0x002C15EB
		[NullableContext(0)]
		[return: Nullable(2)]
		public T GetRelic<T>() where T : RelicModel
		{
			return this.Relics.FirstOrDefault<RelicModel>((RelicModel r) => r is T) as T;
		}

		// Token: 0x06008303 RID: 33539 RVA: 0x002C3424 File Offset: 0x002C1624
		[return: Nullable(2)]
		public RelicModel GetRelicById(ModelId id)
		{
			return this.Relics.FirstOrDefault<RelicModel>((RelicModel r) => r.Id == id);
		}

		/// <summary>
		/// Returns the slot index of the potion in the player's belt, or -1 if it is not in the belt.
		/// </summary>
		// Token: 0x06008304 RID: 33540 RVA: 0x002C3455 File Offset: 0x002C1655
		public int GetPotionSlotIndex(PotionModel model)
		{
			return this._potionSlots.IndexOf(model);
		}

		/// <summary>
		/// Returns the potion at the slot index, or throws if the index is out of range.
		/// </summary>
		// Token: 0x06008305 RID: 33541 RVA: 0x002C3464 File Offset: 0x002C1664
		[NullableContext(2)]
		public PotionModel GetPotionAtSlotIndex(int index)
		{
			if (index < 0 || index >= this._potionSlots.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(index);
				defaultInterpolatedStringHandler.AppendLiteral(" is not a valid potion slot index! Player has ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._potionSlots.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" potion slots");
				throw new IndexOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this._potionSlots[index];
		}

		/// <summary>
		/// Increases the maximum amount of potions the player can hold.
		/// </summary>
		/// <param name="maxPotionCountIncrease">The increased count of maximum amount of potions the player can carry.</param>
		// Token: 0x06008306 RID: 33542 RVA: 0x002C34E4 File Offset: 0x002C16E4
		public void AddToMaxPotionCount(int maxPotionCountIncrease)
		{
			this.SetMaxPotionCountInternal(this._potionSlots.Count + maxPotionCountIncrease);
		}

		/// <summary>
		/// Decreases the maximum amount of potions the player can hold.
		/// </summary>
		/// <param name="maxPotionCountDecrease">The decreased count of maximum amount of potions the player can carry.</param>
		// Token: 0x06008307 RID: 33543 RVA: 0x002C34F9 File Offset: 0x002C16F9
		public void SubtractFromMaxPotionCount(int maxPotionCountDecrease)
		{
			this.SetMaxPotionCountInternal(this._potionSlots.Count - maxPotionCountDecrease);
		}

		/// <summary>
		/// NEVER CALL THIS!
		/// ONLY save/load stuff should be calling this.
		/// </summary>
		/// <param name="newMaxPotionCount">The new maximum amount of potions the player can carry.</param>
		// Token: 0x06008308 RID: 33544 RVA: 0x002C3510 File Offset: 0x002C1710
		private void SetMaxPotionCountInternal(int newMaxPotionCount)
		{
			if (newMaxPotionCount <= this._potionSlots.Count)
			{
				if (newMaxPotionCount < this._potionSlots.Count)
				{
					for (int i = this._potionSlots.Count - 1; i >= newMaxPotionCount; i--)
					{
						if (this._potionSlots[i] != null)
						{
							int num = this._potionSlots.IndexOf(null);
							if (num < newMaxPotionCount)
							{
								this._potionSlots[num] = this._potionSlots[i];
							}
							else
							{
								this.DiscardPotionInternal(this._potionSlots[i], false);
							}
						}
						this._potionSlots.RemoveAt(i);
					}
					Action<int> maxPotionCountChanged = this.MaxPotionCountChanged;
					if (maxPotionCountChanged == null)
					{
						return;
					}
					maxPotionCountChanged(this.MaxPotionCount);
				}
				return;
			}
			for (int j = this._potionSlots.Count; j < newMaxPotionCount; j++)
			{
				this._potionSlots.Add(null);
			}
			Action<int> maxPotionCountChanged2 = this.MaxPotionCountChanged;
			if (maxPotionCountChanged2 == null)
			{
				return;
			}
			maxPotionCountChanged2(this.MaxPotionCount);
		}

		/// <summary>
		/// NEVER CALL THIS!
		/// ONLY PotionCmd.Procure and save/load stuff should be calling this.
		/// </summary>
		/// <param name="potion">Potion to add.</param>
		/// <param name="slotIndex">Slot at which to add the potion. If -1, the potion will be added in the first available slot.</param>
		/// <param name="silent">If true, no events will be called.</param>
		// Token: 0x06008309 RID: 33545 RVA: 0x002C35FC File Offset: 0x002C17FC
		public PotionProcureResult AddPotionInternal(PotionModel potion, int slotIndex = -1, bool silent = false)
		{
			potion.AssertMutable();
			PotionProcureResult potionProcureResult = new PotionProcureResult
			{
				potion = potion
			};
			if (slotIndex < 0)
			{
				slotIndex = this._potionSlots.IndexOf(null);
			}
			if (slotIndex >= 0)
			{
				if (this._potionSlots[slotIndex] != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to add potion ");
					defaultInterpolatedStringHandler.AppendFormatted<PotionModel>(potion);
					defaultInterpolatedStringHandler.AppendLiteral(" at slot index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(slotIndex);
					defaultInterpolatedStringHandler.AppendLiteral(" which is already filled with potion ");
					defaultInterpolatedStringHandler.AppendFormatted<PotionModel>(this._potionSlots[slotIndex]);
					defaultInterpolatedStringHandler.AppendLiteral("!");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					if (!silent)
					{
						Action addPotionFailed = this.AddPotionFailed;
						if (addPotionFailed != null)
						{
							addPotionFailed();
						}
					}
					potionProcureResult.success = false;
					potionProcureResult.failureReason = PotionProcureFailureReason.TooFull;
					return potionProcureResult;
				}
				potion.Owner = this;
				this._potionSlots[slotIndex] = potion;
				if (!silent)
				{
					Action<PotionModel> potionProcured = this.PotionProcured;
					if (potionProcured != null)
					{
						potionProcured(potion);
					}
				}
				potionProcureResult.success = true;
			}
			else
			{
				if (!silent)
				{
					Action addPotionFailed2 = this.AddPotionFailed;
					if (addPotionFailed2 != null)
					{
						addPotionFailed2();
					}
				}
				potionProcureResult.success = false;
				potionProcureResult.failureReason = PotionProcureFailureReason.TooFull;
			}
			return potionProcureResult;
		}

		/// <summary>
		/// NEVER CALL THIS!
		/// ONLY PotionModel.Discard should be calling this.
		/// </summary>
		/// <param name="potion">Potion to discard.</param>
		/// <param name="silent">If true, PotionDiscarded will not be called.</param>
		// Token: 0x0600830A RID: 33546 RVA: 0x002C3729 File Offset: 0x002C1929
		public void DiscardPotionInternal(PotionModel potion, bool silent = false)
		{
			this.RemovePotionInternal(potion);
			if (!silent)
			{
				Action<PotionModel> potionDiscarded = this.PotionDiscarded;
				if (potionDiscarded == null)
				{
					return;
				}
				potionDiscarded(potion);
			}
		}

		/// <summary>
		/// NEVER CALL THIS!
		/// ONLY PotionModel.Remove should be calling this.
		/// </summary>
		/// <param name="potion">Used potion to remove.</param>
		// Token: 0x0600830B RID: 33547 RVA: 0x002C3746 File Offset: 0x002C1946
		public void RemoveUsedPotionInternal(PotionModel potion)
		{
			this.RemovePotionInternal(potion);
			Action<PotionModel> usedPotionRemoved = this.UsedPotionRemoved;
			if (usedPotionRemoved == null)
			{
				return;
			}
			usedPotionRemoved(potion);
		}

		// Token: 0x0600830C RID: 33548 RVA: 0x002C3760 File Offset: 0x002C1960
		private void RemovePotionInternal(PotionModel potion)
		{
			int num = this._potionSlots.IndexOf(potion);
			if (num < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to remove potion you don't have: ");
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(potion.Id);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._potionSlots[num] = null;
		}

		/// <summary>
		/// Populates the character's starting deck to start a new game.
		/// </summary>
		// Token: 0x0600830D RID: 33549 RVA: 0x002C37BC File Offset: 0x002C19BC
		private void PopulateStartingDeck()
		{
			List<CardModel> list = new List<CardModel>();
			foreach (CardModel cardModel in this.Character.StartingDeck)
			{
				CardModel cardModel2 = cardModel.ToMutable();
				cardModel2.FloorAddedToDeck = new int?(1);
				list.Add(cardModel2);
			}
			this.PopulateDeck(list, false);
		}

		/// <summary>
		/// Populates a player's existing deck.
		/// This can be from a multiplayer sync or from a save file.
		/// </summary>
		/// <param name="cards">Cards to load.</param>
		/// <param name="silent">
		/// Whether or not to emit events. If loading from a multiplayer sync, we don't want to emit events.
		/// </param>
		// Token: 0x0600830E RID: 33550 RVA: 0x002C3830 File Offset: 0x002C1A30
		private void PopulateDeck(IEnumerable<CardModel> cards, bool silent = false)
		{
			if (this.Deck.Cards.Any<CardModel>())
			{
				throw new InvalidOperationException("Deck has already been populated.");
			}
			foreach (CardModel cardModel in cards)
			{
				this.Deck.AddInternal(cardModel, -1, silent);
			}
		}

		// Token: 0x0600830F RID: 33551 RVA: 0x002C389C File Offset: 0x002C1A9C
		private void PopulateStartingRelics()
		{
			List<RelicModel> list = this.Character.StartingRelics.Select<RelicModel, RelicModel>((RelicModel r) => r.ToMutable()).ToList<RelicModel>();
			foreach (RelicModel relicModel in list)
			{
				relicModel.FloorAddedToDeck = 1;
				SaveManager.Instance.MarkRelicAsSeen(relicModel);
			}
			this.PopulateRelics(list, false);
		}

		/// <summary>
		/// Loads a player's existing relic set.
		/// This can be from the player's starting relics, from a multiplayer sync, or from a save file.
		/// </summary>
		/// <param name="relics">Relics to load.</param>
		/// <param name="silent">
		/// Whether or not to emit events. If loading from a multiplayer sync, we don't want to emit events.
		/// </param>
		// Token: 0x06008310 RID: 33552 RVA: 0x002C3934 File Offset: 0x002C1B34
		private void PopulateRelics(IEnumerable<RelicModel> relics, bool silent = false)
		{
			if (this.Relics.Any<RelicModel>())
			{
				throw new InvalidOperationException("Relics have already been populated.");
			}
			foreach (RelicModel relicModel in relics)
			{
				this.AddRelicInternal(relicModel, -1, silent);
			}
		}

		/// <summary>
		/// Loads a player's existing potion set.
		/// This can be from a multiplayer sync or from a save file.
		/// </summary>
		/// <param name="serializablePotions">Cards to load.</param>
		/// <param name="silent">Whether or not to emit events. If loading from a multiplayer sync, we don't want to emit events.</param>
		// Token: 0x06008311 RID: 33553 RVA: 0x002C3998 File Offset: 0x002C1B98
		private void LoadPotions(List<SerializablePotion> serializablePotions, bool silent = false)
		{
			if (this.Potions.Any<PotionModel>())
			{
				throw new InvalidOperationException("Potions have already been populated.");
			}
			foreach (SerializablePotion serializablePotion in serializablePotions)
			{
				this.AddPotionInternal(PotionModel.FromSerializable(serializablePotion), serializablePotion.SlotIndex, silent);
			}
		}

		/// <summary>
		/// Resets the player's combat state to an empty state.
		/// This will leave the player with no cards in combat, so you should usually call <see cref="M:MegaCrit.Sts2.Core.Entities.Players.Player.PopulateCombatState(MegaCrit.Sts2.Core.Random.Rng,MegaCrit.Sts2.Core.Combat.CombatState)" />
		/// after.
		/// </summary>
		// Token: 0x06008312 RID: 33554 RVA: 0x002C3A0C File Offset: 0x002C1C0C
		public void ResetCombatState()
		{
			this.PlayerCombatState = new PlayerCombatState(this);
		}

		/// <summary>
		/// Populates the player's combat state with everything they should get at the start of combat.
		/// For example, this clones all the cards from their deck into their draw pile in a random order.
		/// </summary>
		// Token: 0x06008313 RID: 33555 RVA: 0x002C3A1C File Offset: 0x002C1C1C
		public void PopulateCombatState(Rng rng, CombatState state)
		{
			foreach (CardModel cardModel in this.Deck.Cards.ToList<CardModel>())
			{
				CardModel cardModel2 = state.CloneCard(cardModel);
				cardModel2.DeckVersion = cardModel;
				this.PlayerCombatState.DrawPile.AddInternal(cardModel2, -1, false);
			}
			this.PlayerCombatState.DrawPile.RandomizeOrderInternal(this, rng, state);
		}

		/// <summary>
		/// Revives the player before the combat ends, in multiplayer only. Should only trigger when combat ends with other
		/// players alive.
		/// It is very important to do this _before_ combat ends instead of after. If the player is still dead during
		/// HookBus.AfterCombatEnd, their relics will not be subscribed to the HookBus, and relics which rely on AfterCombatEnd
		/// to reset state will not be reset for the next combat.
		/// See: Centennial Puzzle, Captain's Wheel, or any other relics that use AfterCombatEnd.
		/// </summary>
		// Token: 0x06008314 RID: 33556 RVA: 0x002C3AA8 File Offset: 0x002C1CA8
		public async Task ReviveBeforeCombatEnd()
		{
			if (this.Creature.IsDead)
			{
				await CreatureCmd.Heal(this.Creature, 1m, true);
			}
		}

		/// <summary>
		/// Called after combat ends, giving the player the opportunity to do things like clear out their combat state and
		/// other combat teardown stuff.
		/// </summary>
		// Token: 0x06008315 RID: 33557 RVA: 0x002C3AEB File Offset: 0x002C1CEB
		public void AfterCombatEnd()
		{
			this.Creature.RemoveAllPowersInternalExcept(null);
			PlayerCombatState playerCombatState = this.PlayerCombatState;
			if (playerCombatState != null)
			{
				playerCombatState.AfterCombatEnd();
			}
			this.Creature.LoseBlockInternal(this.Creature.Block);
		}

		// Token: 0x06008316 RID: 33558 RVA: 0x002C3B28 File Offset: 0x002C1D28
		private void OnRelicFlashed(RelicModel relic, IEnumerable<Creature> targets)
		{
			SfxCmd.Play(relic.FlashSfx, 1f);
			foreach (Creature creature in targets)
			{
				Control vfxContainer = creature.GetVfxContainer();
				if (vfxContainer != null)
				{
					vfxContainer.AddChildSafely(NRelicFlashVfx.Create(relic, creature));
				}
			}
		}

		// Token: 0x06008317 RID: 33559 RVA: 0x002C3B94 File Offset: 0x002C1D94
		public void OnSideSwitch()
		{
		}

		/// <summary>
		/// Called from Creature when the player reaches zero health, after all hooks that prevent death are called, as well
		/// as <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.AfterDeath(MegaCrit.Sts2.Core.Runs.IRunState,MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Boolean,System.Single)" />.
		/// </summary>
		// Token: 0x06008318 RID: 33560 RVA: 0x002C3B96 File Offset: 0x002C1D96
		public void DeactivateHooks()
		{
			this.IsActiveForHooks = false;
		}

		/// <summary>
		/// Called from Creature when the player changes from a dead state to a non-dead state.
		/// This is _not_ called in the scenario when death is prevented, e.g. by Fairy in a Bottle.
		/// Likely only called in multiplayer scenarios - players cannot revive in singleplayer (remember that death
		/// prevention is different).
		/// </summary>
		// Token: 0x06008319 RID: 33561 RVA: 0x002C3B9F File Offset: 0x002C1D9F
		public void ActivateHooks()
		{
			this.IsActiveForHooks = true;
		}

		// Token: 0x04002C38 RID: 11320
		public const int initialMaxPotionSlotCount = 3;

		// Token: 0x04002C41 RID: 11329
		[Nullable(new byte[] { 2, 1 })]
		private CardPile[] _runPiles;

		// Token: 0x04002C42 RID: 11330
		private readonly List<RelicModel> _relics = new List<RelicModel>();

		// Token: 0x04002C43 RID: 11331
		[Nullable(new byte[] { 1, 2 })]
		private readonly List<PotionModel> _potionSlots = new List<PotionModel>();

		// Token: 0x04002C4B RID: 11339
		private IRunState _runState = NullRunState.Instance;

		// Token: 0x04002C4F RID: 11343
		private int _gold;

		// Token: 0x04002C52 RID: 11346
		private bool _canUseOrRemovePotions = true;

		// Token: 0x0200239D RID: 9117
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040096DF RID: 38623
			[Nullable(0)]
			public static Func<SerializableCard, CardModel> <0>__FromSerializable;

			// Token: 0x040096E0 RID: 38624
			[Nullable(0)]
			public static Func<SerializableRelic, RelicModel> <1>__FromSerializable;
		}
	}
}
