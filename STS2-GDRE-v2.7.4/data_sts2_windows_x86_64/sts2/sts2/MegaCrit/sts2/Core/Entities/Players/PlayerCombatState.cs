using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Players
{
	// Token: 0x02000C7E RID: 3198
	[NullableContext(1)]
	[Nullable(0)]
	public class PlayerCombatState
	{
		// Token: 0x170023D4 RID: 9172
		// (get) Token: 0x0600831B RID: 33563 RVA: 0x002C3BB7 File Offset: 0x002C1DB7
		public IReadOnlyList<Creature> Pets
		{
			get
			{
				return this._pets;
			}
		}

		/// <summary>
		/// The turn number that this player is currently on.
		/// This is different from <see cref="P:MegaCrit.Sts2.Core.Combat.ICombatState.RoundNumber" />; if a player takes an extra turn after their
		/// current one, this will be incremented, while the round number will not.
		/// This number can also be different between players in multiplayer. If one player takes an extra turn (due to an
		/// effect like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.PaelsEye" />), their turn number will increment, but other players' will not.
		/// This starts at 1, so it should never be 0.
		/// </summary>
		// Token: 0x170023D5 RID: 9173
		// (get) Token: 0x0600831C RID: 33564 RVA: 0x002C3BBF File Offset: 0x002C1DBF
		// (set) Token: 0x0600831D RID: 33565 RVA: 0x002C3BC7 File Offset: 0x002C1DC7
		public int TurnNumber { get; private set; } = 1;

		/// <summary>
		/// The current phase of this player's turn. <see cref="F:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase.None" /> when combat is not in progress
		/// and during the enemy's turn. See <see cref="T:MegaCrit.Sts2.Core.Combat.PlayerTurnPhase" /> for the meaning of each phase.
		/// Managed by <see cref="T:MegaCrit.Sts2.Core.Combat.CombatManager" /> and <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.AfterAutoPrePlayPhaseEntered(MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext,MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.Entities.Players.Player)" />.
		/// </summary>
		// Token: 0x170023D6 RID: 9174
		// (get) Token: 0x0600831E RID: 33566 RVA: 0x002C3BD0 File Offset: 0x002C1DD0
		// (set) Token: 0x0600831F RID: 33567 RVA: 0x002C3BD8 File Offset: 0x002C1DD8
		public PlayerTurnPhase Phase
		{
			get
			{
				return this._phase;
			}
			set
			{
				if (value != this._phase)
				{
					this._phase = value;
					Action playerTurnPhaseChanged = this.PlayerTurnPhaseChanged;
					if (playerTurnPhaseChanged == null)
					{
						return;
					}
					playerTurnPhaseChanged();
				}
			}
		}

		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06008320 RID: 33568 RVA: 0x002C3BFC File Offset: 0x002C1DFC
		// (remove) Token: 0x06008321 RID: 33569 RVA: 0x002C3C34 File Offset: 0x002C1E34
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action PlayerTurnPhaseChanged;

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06008322 RID: 33570 RVA: 0x002C3C6C File Offset: 0x002C1E6C
		// (remove) Token: 0x06008323 RID: 33571 RVA: 0x002C3CA4 File Offset: 0x002C1EA4
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<int, int> EnergyChanged;

		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06008324 RID: 33572 RVA: 0x002C3CDC File Offset: 0x002C1EDC
		// (remove) Token: 0x06008325 RID: 33573 RVA: 0x002C3D14 File Offset: 0x002C1F14
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<int, int> StarsChanged;

		// Token: 0x170023D7 RID: 9175
		// (get) Token: 0x06008326 RID: 33574 RVA: 0x002C3D49 File Offset: 0x002C1F49
		public CardPile Hand { get; } = new CardPile(PileType.Hand);

		// Token: 0x170023D8 RID: 9176
		// (get) Token: 0x06008327 RID: 33575 RVA: 0x002C3D51 File Offset: 0x002C1F51
		public CardPile DrawPile { get; } = new CardPile(PileType.Draw);

		// Token: 0x170023D9 RID: 9177
		// (get) Token: 0x06008328 RID: 33576 RVA: 0x002C3D59 File Offset: 0x002C1F59
		public CardPile DiscardPile { get; } = new CardPile(PileType.Discard);

		// Token: 0x170023DA RID: 9178
		// (get) Token: 0x06008329 RID: 33577 RVA: 0x002C3D61 File Offset: 0x002C1F61
		public CardPile ExhaustPile { get; } = new CardPile(PileType.Exhaust);

		// Token: 0x170023DB RID: 9179
		// (get) Token: 0x0600832A RID: 33578 RVA: 0x002C3D69 File Offset: 0x002C1F69
		public CardPile PlayPile { get; } = new CardPile(PileType.Play);

		// Token: 0x170023DC RID: 9180
		// (get) Token: 0x0600832B RID: 33579 RVA: 0x002C3D74 File Offset: 0x002C1F74
		public IReadOnlyList<CardPile> AllPiles
		{
			get
			{
				if (this._piles == null)
				{
					this._piles = new CardPile[] { this.Hand, this.DrawPile, this.DiscardPile, this.ExhaustPile, this.PlayPile };
				}
				return this._piles;
			}
		}

		// Token: 0x170023DD RID: 9181
		// (get) Token: 0x0600832C RID: 33580 RVA: 0x002C3DC8 File Offset: 0x002C1FC8
		public IEnumerable<CardModel> AllCards
		{
			get
			{
				return this.AllPiles.SelectMany<CardPile, CardModel>((CardPile p) => p.Cards);
			}
		}

		// Token: 0x170023DE RID: 9182
		// (get) Token: 0x0600832D RID: 33581 RVA: 0x002C3DF4 File Offset: 0x002C1FF4
		// (set) Token: 0x0600832E RID: 33582 RVA: 0x002C3DFC File Offset: 0x002C1FFC
		public int Energy
		{
			get
			{
				return this._energy;
			}
			set
			{
				if (this._energy == value)
				{
					return;
				}
				int energy = this._energy;
				this._energy = value;
				Action<int, int> energyChanged = this.EnergyChanged;
				if (energyChanged == null)
				{
					return;
				}
				energyChanged(energy, this._energy);
			}
		}

		// Token: 0x170023DF RID: 9183
		// (get) Token: 0x0600832F RID: 33583 RVA: 0x002C3E38 File Offset: 0x002C2038
		public int MaxEnergy
		{
			get
			{
				return (int)Hook.ModifyMaxEnergy(this._player.Creature.CombatState, this._player, this._player.MaxEnergy);
			}
		}

		// Token: 0x170023E0 RID: 9184
		// (get) Token: 0x06008330 RID: 33584 RVA: 0x002C3E6A File Offset: 0x002C206A
		// (set) Token: 0x06008331 RID: 33585 RVA: 0x002C3E74 File Offset: 0x002C2074
		public int Stars
		{
			get
			{
				return this._stars;
			}
			set
			{
				if (this._stars == value)
				{
					return;
				}
				int stars = this._stars;
				this._stars = value;
				CombatManager.Instance.History.StarsModified(this._player.Creature.CombatState, this._stars - stars, this._player);
				Action<int, int> starsChanged = this.StarsChanged;
				if (starsChanged == null)
				{
					return;
				}
				starsChanged(stars, this._stars);
			}
		}

		// Token: 0x170023E1 RID: 9185
		// (get) Token: 0x06008332 RID: 33586 RVA: 0x002C3EDD File Offset: 0x002C20DD
		public OrbQueue OrbQueue { get; }

		// Token: 0x06008333 RID: 33587 RVA: 0x002C3EE8 File Offset: 0x002C20E8
		public PlayerCombatState(Player player)
		{
			this._player = player;
			CombatManager.Instance.StateTracker.Subscribe(this);
			foreach (CardPile cardPile in this.AllPiles)
			{
				CombatManager.Instance.StateTracker.Subscribe(cardPile);
			}
			this.OrbQueue = new OrbQueue(player);
			this.OrbQueue.Clear();
			this.OrbQueue.AddCapacity(player.BaseOrbSlotCount);
		}

		// Token: 0x06008334 RID: 33588 RVA: 0x002C3FD4 File Offset: 0x002C21D4
		public void AfterCombatEnd()
		{
			CombatManager.Instance.StateTracker.Unsubscribe(this);
			foreach (CardPile cardPile in this.AllPiles)
			{
				cardPile.Clear(false);
				CombatManager.Instance.StateTracker.Unsubscribe(cardPile);
			}
			this._pets.Clear();
		}

		/// <summary>
		/// Increment this player's current turn number.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Players.PlayerCombatState.TurnNumber" /> for details on turn number versus <see cref="P:MegaCrit.Sts2.Core.Combat.ICombatState.RoundNumber" />.
		/// </summary>
		// Token: 0x06008335 RID: 33589 RVA: 0x002C404C File Offset: 0x002C224C
		public void IncrementTurnNumber()
		{
			int turnNumber = this.TurnNumber;
			this.TurnNumber = turnNumber + 1;
		}

		// Token: 0x06008336 RID: 33590 RVA: 0x002C4069 File Offset: 0x002C2269
		public void ResetEnergy()
		{
			this.Energy = this.MaxEnergy;
		}

		// Token: 0x06008337 RID: 33591 RVA: 0x002C4077 File Offset: 0x002C2277
		public void AddMaxEnergyToCurrent()
		{
			this.Energy += this.MaxEnergy;
		}

		// Token: 0x06008338 RID: 33592 RVA: 0x002C408C File Offset: 0x002C228C
		public void LoseEnergy(decimal amount)
		{
			if (amount < 0m)
			{
				throw new ArgumentException("Must not be negative.", "amount");
			}
			this.Energy = (int)Math.Clamp(this.Energy - amount, 0m, 999999999m);
		}

		// Token: 0x06008339 RID: 33593 RVA: 0x002C40E8 File Offset: 0x002C22E8
		public void GainEnergy(decimal amount)
		{
			if (amount < 0m)
			{
				throw new ArgumentException("Must not be negative.", "amount");
			}
			this.Energy = (int)Math.Clamp(this.Energy + amount, 0m, 999999999m);
		}

		// Token: 0x0600833A RID: 33594 RVA: 0x002C4144 File Offset: 0x002C2344
		public bool HasEnoughResourcesFor(CardModel card, out UnplayableReason reason)
		{
			int num = Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.All));
			int num2 = Math.Max(0, card.GetStarCostWithModifiers());
			if (num > this.Energy && card.CombatState != null && Hook.ShouldPayExcessEnergyCostWithStars(card.CombatState, this._player))
			{
				num2 += (num - this.Energy) * 2;
				num = this.Energy;
			}
			reason = UnplayableReason.None;
			if (num > this.Energy)
			{
				reason |= UnplayableReason.EnergyCostTooHigh;
			}
			if (num2 > this.Stars)
			{
				reason |= UnplayableReason.StarCostTooHigh;
			}
			return reason == UnplayableReason.None;
		}

		// Token: 0x0600833B RID: 33595 RVA: 0x002C41D4 File Offset: 0x002C23D4
		public void LoseStars(decimal amount)
		{
			if (amount < 0m)
			{
				throw new ArgumentException("Must not be negative.", "amount");
			}
			this.Stars = (int)Math.Max(this.Stars - amount, 0m);
		}

		// Token: 0x0600833C RID: 33596 RVA: 0x002C4224 File Offset: 0x002C2424
		public void GainStars(decimal amount)
		{
			if (amount < 0m)
			{
				throw new ArgumentException("Must not be negative.", "amount");
			}
			this.Stars = (int)Math.Max(this.Stars + amount, 0m);
		}

		/// <summary>
		/// NEVER CALL THIS!
		/// ONLY <see cref="M:MegaCrit.Sts2.Core.Commands.PlayerCmd.AddPet``1(MegaCrit.Sts2.Core.Entities.Players.Player)" /> and save/load stuff should be calling this.
		/// </summary>
		/// <param name="pet">Pet to add.</param>
		// Token: 0x0600833D RID: 33597 RVA: 0x002C4274 File Offset: 0x002C2474
		public void AddPetInternal(Creature pet)
		{
			pet.Monster.AssertMutable();
			if (this._pets.Contains(pet))
			{
				return;
			}
			if (pet.PetOwner != this._player)
			{
				pet.PetOwner = this._player;
			}
			pet.Died += this.OnPetDied;
			this._pets.Add(pet);
		}

		/// <summary>
		/// Get one of this player's pets.
		/// If the player has multiple of the same type of pet, just get the first one.
		/// If the player has none of this type of pet, returns null.
		/// </summary>
		/// <typeparam name="T">Type of pet to get.</typeparam>
		/// <returns>Matching pet.</returns>
		// Token: 0x0600833E RID: 33598 RVA: 0x002C42D3 File Offset: 0x002C24D3
		[NullableContext(0)]
		[return: Nullable(2)]
		public Creature GetPet<T>() where T : MonsterModel
		{
			return this.Pets.FirstOrDefault<Creature>((Creature p) => p.Monster is T);
		}

		// Token: 0x0600833F RID: 33599 RVA: 0x002C4300 File Offset: 0x002C2500
		public void RecalculateCardValues()
		{
			foreach (CardModel cardModel in this.AllCards)
			{
				EnchantmentModel enchantment = cardModel.Enchantment;
				if (enchantment != null)
				{
					enchantment.RecalculateValues();
				}
			}
		}

		// Token: 0x06008340 RID: 33600 RVA: 0x002C4358 File Offset: 0x002C2558
		public void EndOfTurnCleanup()
		{
			foreach (CardModel cardModel in this.AllCards)
			{
				cardModel.EndOfTurnCleanup();
			}
		}

		// Token: 0x06008341 RID: 33601 RVA: 0x002C43A4 File Offset: 0x002C25A4
		public bool HasCardsToPlay()
		{
			return this.Hand.Cards.Any<CardModel>((CardModel c) => c.CanPlay());
		}

		// Token: 0x06008342 RID: 33602 RVA: 0x002C43D8 File Offset: 0x002C25D8
		private void OnPetDied(Creature pet)
		{
			if (!this._pets.Contains(pet))
			{
				throw new InvalidOperationException("Player does not have pet " + pet.LogName);
			}
			if (!Hook.ShouldCreatureBeRemovedFromCombatAfterDeath(pet.CombatState, pet))
			{
				return;
			}
			pet.Died -= this.OnPetDied;
			this._pets.Remove(pet);
		}

		// Token: 0x04002C5B RID: 11355
		private readonly Player _player;

		// Token: 0x04002C5C RID: 11356
		private readonly List<Creature> _pets = new List<Creature>();

		// Token: 0x04002C5D RID: 11357
		[Nullable(new byte[] { 2, 1 })]
		private CardPile[] _piles;

		// Token: 0x04002C5E RID: 11358
		private int _energy;

		// Token: 0x04002C5F RID: 11359
		private int _stars;

		// Token: 0x04002C61 RID: 11361
		private PlayerTurnPhase _phase;
	}
}
