using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Combat
{
	/// <summary>
	/// An object containing all of the state that represents the current combat.
	///
	/// Some state details are derived from higher-level objects. For example:
	/// * All players in the combat state are derived by checking the Player property of all the creatures.
	/// * All cards in the combat state are derived by checking the combat piles of all those players.
	/// </summary>
	// Token: 0x02000D42 RID: 3394
	[NullableContext(1)]
	[Nullable(0)]
	public class CombatState : ICombatState, ICardScope
	{
		/// <summary>
		/// The state of the run that this combat exists in.
		/// Will be <see cref="P:MegaCrit.Sts2.Core.Combat.CombatState.RunState" /> in gameplay and <see cref="T:MegaCrit.Sts2.Core.Runs.NullRunState" /> in some test/debug scenarios.
		/// </summary>
		// Token: 0x17002596 RID: 9622
		// (get) Token: 0x06008924 RID: 35108 RVA: 0x002E1F10 File Offset: 0x002E0110
		public IRunState RunState { get; }

		/// <summary>
		/// Fired whenever the arrangement of creatures in the combat changes. Specifically, when:
		/// * A creature is added.
		/// * A creature is removed.
		/// * A creature's index changes.
		/// </summary>
		// Token: 0x140000DA RID: 218
		// (add) Token: 0x06008925 RID: 35109 RVA: 0x002E1F18 File Offset: 0x002E0118
		// (remove) Token: 0x06008926 RID: 35110 RVA: 0x002E1F50 File Offset: 0x002E0150
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<ICombatState> CreaturesChanged;

		/// <summary>
		/// Get all creatures on the Allies side.
		/// </summary>
		// Token: 0x17002597 RID: 9623
		// (get) Token: 0x06008927 RID: 35111 RVA: 0x002E1F85 File Offset: 0x002E0185
		public IReadOnlyList<Creature> Allies
		{
			get
			{
				return this._allies;
			}
		}

		/// <summary>
		/// Get all creatures on the Enemies side.
		/// </summary>
		// Token: 0x17002598 RID: 9624
		// (get) Token: 0x06008928 RID: 35112 RVA: 0x002E1F8D File Offset: 0x002E018D
		public IReadOnlyList<Creature> Enemies
		{
			get
			{
				return this._enemies;
			}
		}

		/// <summary>
		/// Get all creatures in the combat on all sides.
		/// </summary>
		// Token: 0x17002599 RID: 9625
		// (get) Token: 0x06008929 RID: 35113 RVA: 0x002E1F95 File Offset: 0x002E0195
		public IReadOnlyList<Creature> Creatures
		{
			get
			{
				return this._allies.Concat<Creature>(this._enemies).ToList<Creature>();
			}
		}

		/// <summary>
		/// Get all the player creatures in the combat.
		/// </summary>
		// Token: 0x1700259A RID: 9626
		// (get) Token: 0x0600892A RID: 35114 RVA: 0x002E1FAD File Offset: 0x002E01AD
		public IReadOnlyList<Creature> PlayerCreatures
		{
			get
			{
				return this.Creatures.Where<Creature>((Creature c) => c.IsPlayer).ToList<Creature>();
			}
		}

		/// <summary>
		/// Get all players in the combat.
		/// </summary>
		// Token: 0x1700259B RID: 9627
		// (get) Token: 0x0600892B RID: 35115 RVA: 0x002E1FDE File Offset: 0x002E01DE
		public IReadOnlyList<Player> Players
		{
			get
			{
				return this.PlayerCreatures.Select<Creature, Player>((Creature c) => c.Player).ToList<Player>();
			}
		}

		/// <summary>
		/// List of custom modifiers applied to this combat, usually via daily or custom runs.
		/// </summary>
		// Token: 0x1700259C RID: 9628
		// (get) Token: 0x0600892C RID: 35116 RVA: 0x002E200F File Offset: 0x002E020F
		public IReadOnlyList<ModifierModel> Modifiers { get; }

		/// <summary>
		/// List badge models, used when unlocking badges.
		/// </summary>
		// Token: 0x1700259D RID: 9629
		// (get) Token: 0x0600892D RID: 35117 RVA: 0x002E2017 File Offset: 0x002E0217
		public IReadOnlyList<BadgeModel> BadgeModels { get; }

		/// <summary>
		/// The model used to scale various things (block, power application) in multiplayer.
		/// </summary>
		// Token: 0x1700259E RID: 9630
		// (get) Token: 0x0600892E RID: 35118 RVA: 0x002E201F File Offset: 0x002E021F
		// (set) Token: 0x0600892F RID: 35119 RVA: 0x002E2027 File Offset: 0x002E0227
		[Nullable(2)]
		public MultiplayerScalingModel MultiplayerScalingModel
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		/// <summary>
		/// The round of combat that we're on in this combat.
		/// A "round" encompasses both the player's turn and the enemy's turn.
		/// This starts at 1, so it should never be 0.
		/// </summary>
		// Token: 0x1700259F RID: 9631
		// (get) Token: 0x06008930 RID: 35120 RVA: 0x002E2030 File Offset: 0x002E0230
		// (set) Token: 0x06008931 RID: 35121 RVA: 0x002E2038 File Offset: 0x002E0238
		public int RoundNumber { get; set; }

		/// <summary>
		/// The side that is active in this combat.
		/// A round starts with the player side being active, then switches to the enemy side after all players have ended
		/// their turn.
		/// When the enemy turn ends, the current side changes back to the player side, and the round number is incremented.
		/// </summary>
		// Token: 0x170025A0 RID: 9632
		// (get) Token: 0x06008932 RID: 35122 RVA: 0x002E2041 File Offset: 0x002E0241
		// (set) Token: 0x06008933 RID: 35123 RVA: 0x002E2049 File Offset: 0x002E0249
		public CombatSide CurrentSide { get; set; }

		// Token: 0x170025A1 RID: 9633
		// (get) Token: 0x06008934 RID: 35124 RVA: 0x002E2052 File Offset: 0x002E0252
		// (set) Token: 0x06008935 RID: 35125 RVA: 0x002E205A File Offset: 0x002E025A
		[Nullable(2)]
		public EncounterModel Encounter
		{
			[NullableContext(2)]
			get
			{
				return this._encounter;
			}
			[NullableContext(2)]
			private set
			{
				if (value != null)
				{
					value.AssertMutable();
				}
				this._encounter = value;
			}
		}

		/// <summary>
		/// A list of creatures that escaped (were removed without dying) during the encounter. Used when rewards are given.
		/// </summary>
		// Token: 0x170025A2 RID: 9634
		// (get) Token: 0x06008936 RID: 35126 RVA: 0x002E206C File Offset: 0x002E026C
		public IReadOnlyList<Creature> EscapedCreatures
		{
			get
			{
				return this._escapedCreatures;
			}
		}

		// Token: 0x06008937 RID: 35127 RVA: 0x002E2074 File Offset: 0x002E0274
		[NullableContext(2)]
		public CombatState(EncounterModel encounter = null, IRunState runState = null, [Nullable(new byte[] { 2, 1 })] IReadOnlyList<ModifierModel> modifiers = null, [Nullable(new byte[] { 2, 1 })] IReadOnlyList<BadgeModel> badgeModels = null, MultiplayerScalingModel multiplayerScalingModel = null)
		{
			if (encounter != null)
			{
				encounter.AssertMutable();
			}
			this.Encounter = encounter;
			this.RoundNumber = 1;
			this.CurrentSide = CombatSide.Player;
			this.RunState = runState ?? NullRunState.Instance;
			this.Modifiers = modifiers ?? Array.Empty<ModifierModel>();
			this.BadgeModels = badgeModels ?? Array.Empty<BadgeModel>();
			this.MultiplayerScalingModel = multiplayerScalingModel;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.CreateCard``1(MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06008938 RID: 35128 RVA: 0x002E210A File Offset: 0x002E030A
		public T CreateCard<[Nullable(0)] T>(Player owner) where T : CardModel
		{
			return (T)((object)this.CreateCard(ModelDb.Card<T>(), owner));
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.CreateCard(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x06008939 RID: 35129 RVA: 0x002E2124 File Offset: 0x002E0324
		public CardModel CreateCard(CardModel canonicalCard, Player owner)
		{
			CardModel cardModel = canonicalCard.ToMutable();
			this.AddCard(cardModel, owner);
			cardModel.AfterCreated();
			return cardModel;
		}

		/// <summary>
		/// WARNING: If you're specifically intending to create a clone in combat for an effect like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.DualWield" />,
		/// you should use <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.CreateClone" /> instead.
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.CloneCard(MegaCrit.Sts2.Core.Models.CardModel)" />.
		/// </summary>
		// Token: 0x0600893A RID: 35130 RVA: 0x002E2148 File Offset: 0x002E0348
		public CardModel CloneCard(CardModel mutableCard)
		{
			CardModel cardModel = (CardModel)mutableCard.ClonePreservingMutability();
			this.AddCard(cardModel);
			return cardModel;
		}

		/// <summary>
		/// See <see cref="M:MegaCrit.Sts2.Core.Runs.ICardScope.AddCard(MegaCrit.Sts2.Core.Models.CardModel,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x0600893B RID: 35131 RVA: 0x002E2169 File Offset: 0x002E0369
		public void AddCard(CardModel card, Player owner)
		{
			card.Owner = owner;
			this.AddCard(card);
		}

		// Token: 0x0600893C RID: 35132 RVA: 0x002E2179 File Offset: 0x002E0379
		public void RemoveCard(CardModel card)
		{
			this._allCards.Remove(card);
			card.Owner = null;
		}

		/// <summary>
		/// Does this state contain the specified card?
		/// </summary>
		// Token: 0x0600893D RID: 35133 RVA: 0x002E218F File Offset: 0x002E038F
		public bool ContainsCard(CardModel card)
		{
			return this._allCards.Contains(card);
		}

		/// <summary>
		/// Add a player's creature on the Allies side.
		/// </summary>
		// Token: 0x0600893E RID: 35134 RVA: 0x002E219D File Offset: 0x002E039D
		public void AddPlayer(Player player)
		{
			this.AttachCreature(player.Creature);
			this.AddCreature(player.Creature);
		}

		/// <summary>
		/// Creates a new creature from a monster in this combat state.
		/// After you create the creature, you should call AddCreature to add it to combat. This can be done immediately
		/// or after some time.
		/// </summary>
		// Token: 0x0600893F RID: 35135 RVA: 0x002E21B8 File Offset: 0x002E03B8
		public Creature CreateCreature(MonsterModel monster, CombatSide side, [Nullable(2)] string slot)
		{
			monster.AssertMutable();
			monster.RunRng = this.RunState.Rng;
			Creature creature = new Creature(monster, side, slot);
			List<Creature> list = ((side == CombatSide.Player) ? this._allies : this._enemies);
			if (side == CombatSide.Enemy)
			{
				creature.SetUniqueMonsterHpValue(list, this.RunState.Rng.Niche);
				creature.ScaleMonsterHpForMultiplayer(this.Encounter, this.Players.Count, this.RunState.CurrentActIndex);
			}
			this.AttachCreature(creature);
			ulong seed = this.RunState.Rng.Seed;
			MapCoord? mapCoord = this.RunState.CurrentMapCoord;
			ulong num = seed + (ulong)((long)((mapCoord != null) ? mapCoord.GetValueOrDefault().col : 0));
			mapCoord = this.RunState.CurrentMapCoord;
			monster.Rng = new Rng(num + (ulong)((long)((mapCoord != null) ? mapCoord.GetValueOrDefault().row : 0)) + (ulong)((long)this.RunState.CurrentActIndex) + (ulong)creature.CombatId.Value);
			EncounterModel encounter = this._encounter;
			if (encounter != null)
			{
				encounter.OnCreatureSpawned(creature);
			}
			return creature;
		}

		/// <summary>
		/// Attaches an existing creature to this combat state without adding it to the combat.
		/// This sets up the creature to be added to combat without actually putting it into the combat (i.e. it's not
		/// targeted by cards that hit all enemies, powers don't proc on it, etc).
		/// For players and almost all monsters, this is called just before adding their creature to the combat. Currently,
		/// the Doormaker is the only creature that doesn't get added to combat immediately after this is called.
		/// </summary>
		// Token: 0x06008940 RID: 35136 RVA: 0x002E22D3 File Offset: 0x002E04D3
		private void AttachCreature(Creature creature)
		{
			creature.CombatState = this;
			creature.CombatId = new uint?(this._nextCreatureId);
			this._nextCreatureId += 1U;
		}

		/// <summary>
		/// Call this to remove a creature that escaped rather than dying.
		/// </summary>
		// Token: 0x06008941 RID: 35137 RVA: 0x002E22FB File Offset: 0x002E04FB
		public void CreatureEscaped(Creature creature)
		{
			this._escapedCreatures.Add(creature);
			this.RemoveCreature(creature, true);
		}

		/// <summary>
		/// Removes the creature from combat.
		/// </summary>
		/// <param name="creature">The creature that will be removed.</param>
		/// <param name="unattach">If true, then the creature cannot be re-added to the combat state.</param>
		// Token: 0x06008942 RID: 35138 RVA: 0x002E2314 File Offset: 0x002E0514
		public void RemoveCreature(Creature creature, bool unattach = true)
		{
			if (creature.CombatState == null)
			{
				return;
			}
			if (creature.CombatState != this)
			{
				throw new InvalidOperationException("Creature is in a different combat.");
			}
			if (this._enemies.Contains(creature))
			{
				this._enemies.Remove(creature);
			}
			else
			{
				if (!this._allies.Contains(creature))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Removed creature '");
					defaultInterpolatedStringHandler.AppendFormatted<Creature>(creature);
					defaultInterpolatedStringHandler.AppendLiteral("' was not found.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this._allies.Remove(creature);
			}
			if (unattach)
			{
				creature.CombatState = null;
			}
			Action<ICombatState> creaturesChanged = this.CreaturesChanged;
			if (creaturesChanged == null)
			{
				return;
			}
			creaturesChanged(this);
		}

		// Token: 0x06008943 RID: 35139 RVA: 0x002E23CA File Offset: 0x002E05CA
		public bool ContainsCreature(Creature creature)
		{
			return this._allies.Contains(creature) || this._enemies.Contains(creature);
		}

		// Token: 0x06008944 RID: 35140 RVA: 0x002E23E8 File Offset: 0x002E05E8
		[NullableContext(0)]
		public bool ContainsMonster<T>() where T : MonsterModel
		{
			return this._enemies.Any<Creature>((Creature c) => c.Monster is T);
		}

		/// <summary>
		/// Get the creature with the specified combat ID. Null if not found.
		/// </summary>
		// Token: 0x06008945 RID: 35141 RVA: 0x002E2414 File Offset: 0x002E0614
		[NullableContext(2)]
		public Creature GetCreature(uint? combatId)
		{
			if (combatId == null)
			{
				return null;
			}
			return this.Creatures.FirstOrDefault<Creature>(delegate(Creature c)
			{
				uint? combatId2 = c.CombatId;
				uint? combatId3 = combatId;
				return (combatId2.GetValueOrDefault() == combatId3.GetValueOrDefault()) & (combatId2 != null == (combatId3 != null));
			});
		}

		/// <summary>
		/// Get the creature with the specified combat ID.
		/// If the creature doesn't exist, keep checking for a while before timing out and returning null.
		/// </summary>
		/// <param name="combatId">Combat ID of the creature to get.</param>
		/// <param name="timeoutSec">How long to wait for the creature to appear.</param>
		/// <returns>Specified Creature, or null if it doesn't exist and we've waited long enough.</returns>
		// Token: 0x06008946 RID: 35142 RVA: 0x002E2454 File Offset: 0x002E0654
		[return: Nullable(new byte[] { 1, 2 })]
		public async Task<Creature> GetCreatureAsync(uint? combatId, double timeoutSec)
		{
			CombatState.<>c__DisplayClass60_0 CS$<>8__locals1 = new CombatState.<>c__DisplayClass60_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.combatId = combatId;
			Creature creature;
			if (CS$<>8__locals1.combatId == null)
			{
				creature = null;
			}
			else
			{
				Creature creature2 = this.GetCreature(CS$<>8__locals1.combatId);
				if (creature2 != null)
				{
					creature = creature2;
				}
				else
				{
					uint? combatId2 = CS$<>8__locals1.combatId;
					uint nextCreatureId = this._nextCreatureId;
					if ((combatId2.GetValueOrDefault() < nextCreatureId) & (combatId2 != null))
					{
						creature = null;
					}
					else
					{
						CS$<>8__locals1.completionSource = new TaskCompletionSource<Creature>();
						this.CreaturesChanged += CS$<>8__locals1.<GetCreatureAsync>g__OnCreaturesChanged|0;
						Task timeoutTask = CombatState.GodotTimerTask(timeoutSec);
						Task task = await Task.WhenAny(CS$<>8__locals1.completionSource.Task, timeoutTask);
						Task task2 = task;
						this.CreaturesChanged -= CS$<>8__locals1.<GetCreatureAsync>g__OnCreaturesChanged|0;
						if (task2 == timeoutTask)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Timed out waiting for creature with target index ");
							defaultInterpolatedStringHandler.AppendFormatted<uint?>(CS$<>8__locals1.combatId);
							defaultInterpolatedStringHandler.AppendLiteral(" to spawn!");
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						creature = await CS$<>8__locals1.completionSource.Task;
					}
				}
			}
			return creature;
		}

		/// <summary>
		/// Get all the creatures on the specified side.
		/// </summary>
		// Token: 0x06008947 RID: 35143 RVA: 0x002E24A7 File Offset: 0x002E06A7
		public IReadOnlyList<Creature> GetCreaturesOnSide(CombatSide side)
		{
			if (side != CombatSide.Enemy)
			{
				return this.Allies;
			}
			return this.Enemies;
		}

		/// <summary>
		/// Get all the creatures on the currently-active side.
		/// </summary>
		// Token: 0x170025A3 RID: 9635
		// (get) Token: 0x06008948 RID: 35144 RVA: 0x002E24BA File Offset: 0x002E06BA
		public IReadOnlyList<Creature> CreaturesOnCurrentSide
		{
			get
			{
				return this.GetCreaturesOnSide(this.CurrentSide);
			}
		}

		/// <summary>
		/// Get all opponents of a creature.
		/// </summary>
		// Token: 0x06008949 RID: 35145 RVA: 0x002E24C8 File Offset: 0x002E06C8
		public IReadOnlyList<Creature> GetOpponentsOf(Creature creature)
		{
			return this.GetCreaturesOnSide(creature.Side.GetOppositeSide());
		}

		/// <summary>
		/// Get all teammates of a creature, including the creature itself.
		/// </summary>
		// Token: 0x0600894A RID: 35146 RVA: 0x002E24DB File Offset: 0x002E06DB
		public IReadOnlyList<Creature> GetTeammatesOf(Creature creature)
		{
			return this.GetCreaturesOnSide(creature.Side);
		}

		/// <summary>
		/// Get all hittable enemies.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Creatures.Creature.IsHittable" /> for a definition.
		///
		/// NOTE: We shouldn't add too many methods like this, this one is just extremely common because it's used for AOE
		/// and random attack targeting.
		/// </summary>
		// Token: 0x170025A4 RID: 9636
		// (get) Token: 0x0600894B RID: 35147 RVA: 0x002E24E9 File Offset: 0x002E06E9
		public IReadOnlyList<Creature> HittableEnemies
		{
			get
			{
				return this.Enemies.Where<Creature>((Creature e) => e.IsHittable).ToList<Creature>();
			}
		}

		// Token: 0x0600894C RID: 35148 RVA: 0x002E251C File Offset: 0x002E071C
		[NullableContext(2)]
		public Player GetPlayer(ulong playerId)
		{
			return this.Players.FirstOrDefault<Player>((Player p) => p.NetId == playerId);
		}

		/// <summary>
		/// Get all the models that should have combat hooks called on them.
		/// </summary>
		// Token: 0x0600894D RID: 35149 RVA: 0x002E254D File Offset: 0x002E074D
		public IEnumerable<AbstractModel> IterateHookListeners()
		{
			CombatState.<IterateHookListeners>d__69 <IterateHookListeners>d__ = new CombatState.<IterateHookListeners>d__69(-2);
			<IterateHookListeners>d__.<>4__this = this;
			return <IterateHookListeners>d__;
		}

		// Token: 0x0600894E RID: 35150 RVA: 0x002E255D File Offset: 0x002E075D
		public void SortEnemiesBySlotName()
		{
			if (this.Encounter == null)
			{
				return;
			}
			this._enemies.Sort((Creature a, Creature b) => this.Encounter.Slots.IndexOf(a.SlotName) - this.Encounter.Slots.IndexOf(b.SlotName));
		}

		// Token: 0x0600894F RID: 35151 RVA: 0x002E2580 File Offset: 0x002E0780
		public void SetEnemyIndex(Creature creature, int index)
		{
			if (this.Encounter.Slots.Any<string>())
			{
				throw new InvalidOperationException("Cannot modify turn order of a combat with pre-set slots");
			}
			if (!this.Enemies.Contains(creature))
			{
				throw new ArgumentException("Creature must be a valid enemy to change its turn order.");
			}
			this._enemies.Remove(creature);
			this._enemies.Insert(Math.Min(index, this._enemies.Count - 1), creature);
		}

		// Token: 0x06008950 RID: 35152 RVA: 0x002E25F0 File Offset: 0x002E07F0
		private void AddCard(CardModel card)
		{
			card.AssertMutable();
			if (card.CombatState != null && card.CombatState != this)
			{
				throw new InvalidOperationException("Card " + card.Id.Entry + " combat state is set to a different combat.");
			}
			this._allCards.Add(card);
		}

		/// <summary>
		/// Adds a creature to the combat.
		/// This should almost always be called right after CreateCreature. Until this is called, creatures cannot be targeted
		/// and their powers will not be triggered. It is separate so that creatures can be removed from combat and re-added
		/// to it later. Currently, this is only used in the Doormaker combat.
		/// </summary>
		/// <param name="creature">The creature to add to the combat.</param>
		// Token: 0x06008951 RID: 35153 RVA: 0x002E2640 File Offset: 0x002E0840
		public void AddCreature(Creature creature)
		{
			if (creature.CombatState != this)
			{
				throw new InvalidOperationException("Creature was created for a different combat.");
			}
			List<Creature> list = ((creature.Side == CombatSide.Player) ? this._allies : this._enemies);
			if (this.ContainsCreature(creature))
			{
				throw new InvalidOperationException("Creature is already in this combat, but AddCreature was called on it again.");
			}
			list.Add(creature);
			Action<ICombatState> creaturesChanged = this.CreaturesChanged;
			if (creaturesChanged == null)
			{
				return;
			}
			creaturesChanged(this);
		}

		// Token: 0x06008952 RID: 35154 RVA: 0x002E26A8 File Offset: 0x002E08A8
		private bool Contains(AbstractModel model)
		{
			PowerModel powerModel = model as PowerModel;
			bool flag;
			if (powerModel == null)
			{
				RelicModel relicModel = model as RelicModel;
				if (relicModel == null)
				{
					PotionModel potionModel = model as PotionModel;
					if (potionModel == null)
					{
						CardModel cardModel = model as CardModel;
						if (cardModel == null)
						{
							AfflictionModel afflictionModel = model as AfflictionModel;
							if (afflictionModel == null)
							{
								EnchantmentModel enchantmentModel = model as EnchantmentModel;
								if (enchantmentModel == null)
								{
									OrbModel orbModel = model as OrbModel;
									if (orbModel == null)
									{
										MonsterModel monsterModel = model as MonsterModel;
										if (monsterModel == null)
										{
											if (!(model is AchievementModel))
											{
												if (!(model is BadgeModel))
												{
													if (!(model is ModifierModel))
													{
														if (!(model is MultiplayerScalingModel))
														{
															string text = "model";
															DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
															defaultInterpolatedStringHandler.AppendLiteral("Invalid model type ");
															defaultInterpolatedStringHandler.AppendFormatted<Type>(model.GetType());
															defaultInterpolatedStringHandler.AppendLiteral(" (");
															defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>(model);
															defaultInterpolatedStringHandler.AppendLiteral(")");
															throw new ArgumentOutOfRangeException(text, model, defaultInterpolatedStringHandler.ToStringAndClear());
														}
														flag = true;
													}
													else
													{
														flag = true;
													}
												}
												else
												{
													flag = true;
												}
											}
											else
											{
												flag = true;
											}
										}
										else
										{
											flag = monsterModel.Creature.CombatState != null;
										}
									}
									else
									{
										flag = !orbModel.HasBeenRemovedFromState && orbModel.Owner.IsActiveForHooks;
									}
								}
								else
								{
									flag = enchantmentModel.HasCard && !enchantmentModel.Card.HasBeenRemovedFromState && enchantmentModel.Card.Owner.IsActiveForHooks;
								}
							}
							else
							{
								flag = afflictionModel.HasCard && !afflictionModel.Card.HasBeenRemovedFromState && afflictionModel.Card.Owner.IsActiveForHooks;
							}
						}
						else
						{
							flag = !cardModel.HasBeenRemovedFromState && cardModel.Owner.IsActiveForHooks;
						}
					}
					else
					{
						flag = !potionModel.HasBeenRemovedFromState && potionModel.Owner.IsActiveForHooks;
					}
				}
				else
				{
					flag = !relicModel.HasBeenRemovedFromState && relicModel.Owner.IsActiveForHooks;
				}
			}
			else
			{
				bool flag2;
				if (powerModel.Owner.CombatState != null)
				{
					Player player = powerModel.Owner.Player;
					flag2 = player == null || player.IsActiveForHooks;
				}
				else
				{
					flag2 = false;
				}
				flag = flag2;
			}
			return flag;
		}

		// Token: 0x06008953 RID: 35155 RVA: 0x002E28DC File Offset: 0x002E0ADC
		private static async Task GodotTimerTask(double timeSec)
		{
			SceneTreeTimer sceneTreeTimer = ((SceneTree)Engine.GetMainLoop()).CreateTimer(timeSec, true, false, false);
			await sceneTreeTimer.ToSignal(sceneTreeTimer, SceneTreeTimer.SignalName.Timeout);
		}

		// Token: 0x06008954 RID: 35156 RVA: 0x002E291F File Offset: 0x002E0B1F
		public bool IsLiveCombat()
		{
			return true;
		}

		// Token: 0x04002FCA RID: 12234
		private readonly List<Creature> _allies = new List<Creature>();

		// Token: 0x04002FCB RID: 12235
		private readonly List<Creature> _enemies = new List<Creature>();

		/// <summary>
		/// This is the ID that will be assigned to the next spawned creature's CombatId field.
		/// If we receive an action targeting a creature with an ID less than this one, then it must either be in the creature
		/// list or it had died at some point in the past.
		/// If we receive an action targeting a creature with an ID greater than or equal to this one, then it has yet to
		/// spawn (or there is some other error).
		/// </summary>
		// Token: 0x04002FCC RID: 12236
		private uint _nextCreatureId;

		// Token: 0x04002FD0 RID: 12240
		[Nullable(2)]
		private readonly EncounterModel _encounter;

		// Token: 0x04002FD3 RID: 12243
		private readonly List<Creature> _escapedCreatures = new List<Creature>();

		/// <summary>
		/// All cards that have been created within this state.
		/// This allows us to keep track of "floating" cards that have not been added to any piles (like fake cards in
		/// upgrade previews).
		/// </summary>
		// Token: 0x04002FD4 RID: 12244
		private readonly List<CardModel> _allCards = new List<CardModel>();
	}
}
