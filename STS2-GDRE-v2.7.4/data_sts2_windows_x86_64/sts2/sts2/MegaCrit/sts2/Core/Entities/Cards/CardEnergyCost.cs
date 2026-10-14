using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	/// <summary>
	/// Encapsulates all energy cost related properties and methods for a CardModel.
	/// This class handles base energy costs, local and global modifications, and energy cost calculations.
	/// </summary>
	// Token: 0x02000CAA RID: 3242
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CardEnergyCost
	{
		/// <summary>
		/// This card's "official" starting energy cost.
		/// This is what would appear on the card if it was printed out on paper.
		/// </summary>
		// Token: 0x17002433 RID: 9267
		// (get) Token: 0x06008479 RID: 33913 RVA: 0x002C9E31 File Offset: 0x002C8031
		public int Canonical { get; }

		/// <summary>
		/// Whether this card has an energy cost of X.
		/// X-cost-cards automatically spend all of the player's remaining energy when played, and their effect is
		/// multiplied by the amount spent.
		/// </summary>
		// Token: 0x17002434 RID: 9268
		// (get) Token: 0x0600847A RID: 33914 RVA: 0x002C9E39 File Offset: 0x002C8039
		public bool CostsX { get; }

		/// <summary>
		/// Was this card's energy cost just recently upgraded?
		/// This is mainly used to show upgrade preview values in green.
		/// This should be cleared after the upgrade is complete.
		/// </summary>
		// Token: 0x17002435 RID: 9269
		// (get) Token: 0x0600847B RID: 33915 RVA: 0x002C9E41 File Offset: 0x002C8041
		// (set) Token: 0x0600847C RID: 33916 RVA: 0x002C9E49 File Offset: 0x002C8049
		public bool WasJustUpgraded { get; private set; }

		/// <summary>
		/// Does this energy cost have any local modifiers?
		/// See <see cref="F:MegaCrit.Sts2.Core.Entities.Cards.CostModifiers.Local" /> for details.
		/// </summary>
		// Token: 0x17002436 RID: 9270
		// (get) Token: 0x0600847D RID: 33917 RVA: 0x002C9E52 File Offset: 0x002C8052
		public bool HasLocalModifiers
		{
			get
			{
				return this._localModifiers.Count > 0;
			}
		}

		// Token: 0x0600847E RID: 33918 RVA: 0x002C9E62 File Offset: 0x002C8062
		public CardEnergyCost(CardModel card, int canonicalCost, bool costsX)
		{
			this._card = card;
			this.CostsX = costsX;
			this.Canonical = (this.CostsX ? 0 : canonicalCost);
			this._base = this.Canonical;
		}

		/// <summary>
		/// Get this card's energy cost, including the specified modifier types.
		/// See <see cref="T:MegaCrit.Sts2.Core.Entities.Cards.CostModifiers" /> for details on what types are available.
		/// </summary>
		// Token: 0x0600847F RID: 33919 RVA: 0x002C9EA4 File Offset: 0x002C80A4
		public int GetWithModifiers(CostModifiers modifiers)
		{
			int num = this._base;
			if (this._card.IsCanonical)
			{
				return num;
			}
			if (this._base < 0)
			{
				return num;
			}
			if (this.CostsX)
			{
				return num;
			}
			if (modifiers.HasFlag(CostModifiers.Local))
			{
				foreach (LocalCostModifier localCostModifier in this._localModifiers)
				{
					num = localCostModifier.Modify(num);
				}
			}
			if (modifiers.HasFlag(CostModifiers.Global) && this._card.CombatState != null)
			{
				num = (int)Hook.ModifyEnergyCostInCombat(this._card.CombatState, this._card, num);
			}
			return Math.Max(0, num);
		}

		/// <summary>
		/// The amount of energy most recently spent to play this X-cost card.
		/// Used when duplicating X-cost cards, to make sure the duplicates are played with the same value.
		///
		/// WARNING: Only use this for calculations related to energy spent. If you're using this to calculate a
		/// cost-X-energy card's effect, use <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.ResolveEnergyXValue" /> instead, as it will take X-value
		/// modifications (like <see cref="T:MegaCrit.Sts2.Core.Models.Relics.ChemicalX" />) into account.
		/// </summary>
		// Token: 0x17002437 RID: 9271
		// (get) Token: 0x06008480 RID: 33920 RVA: 0x002C9F80 File Offset: 0x002C8180
		// (set) Token: 0x06008481 RID: 33921 RVA: 0x002C9F9B File Offset: 0x002C819B
		public int CapturedXValue
		{
			get
			{
				if (!this.CostsX)
				{
					throw new InvalidOperationException("Only X-cost cards have a captured value.");
				}
				return this._capturedXValue;
			}
			set
			{
				this._card.AssertMutable();
				if (!this.CostsX)
				{
					throw new InvalidOperationException("Only X-cost cards have a captured value.");
				}
				this._capturedXValue = value;
			}
		}

		/// <summary>
		/// Get the amount of energy that should be spent to play this card.
		///
		/// * For X-cost cards, this is the amount of energy that its owner has.
		/// * For normal cards, this is the current cost including all modifiers
		///   (see <see cref="M:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost.GetWithModifiers(MegaCrit.Sts2.Core.Entities.Cards.CostModifiers)" /> with <see cref="F:MegaCrit.Sts2.Core.Entities.Cards.CostModifiers.All" />) clamped to 0.
		///
		/// The game uses this value when actually spending the energy to play the card.
		/// Additionally, this is useful for effects that need to know how much WOULD be spent to play the card without
		/// actually playing it, such as <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Scavenge" />.
		/// </summary>
		// Token: 0x06008482 RID: 33922 RVA: 0x002C9FC2 File Offset: 0x002C81C2
		public int GetAmountToSpend()
		{
			if (!this.CostsX)
			{
				return Math.Max(0, this.GetWithModifiers(CostModifiers.All));
			}
			PlayerCombatState playerCombatState = this._card.Owner.PlayerCombatState;
			if (playerCombatState == null)
			{
				return 0;
			}
			return playerCombatState.Energy;
		}

		/// <summary>
		/// Get the "resolved" cost of this card. This can mean one of two things:
		///
		/// * For X-cost cards, this is the captured X-cost value (see <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost.CapturedXValue" />).
		/// * For normal cards, this is the current cost including all modifiers
		///   (see <see cref="M:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost.GetWithModifiers(MegaCrit.Sts2.Core.Entities.Cards.CostModifiers)" /> with <see cref="F:MegaCrit.Sts2.Core.Entities.Cards.CostModifiers.All" />) clamped to 0.
		///
		/// This is useful for effects that need to know the card's cost AFTER it was played, such as
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Relics.IntimidatingHelmet" />. For normal cards, these effects just care about the card's current cost
		/// (including all modifiers). For X-cost cards, these effects care about the X-value that was set for the card when
		/// it was played.
		/// </summary>
		// Token: 0x06008483 RID: 33923 RVA: 0x002C9FF5 File Offset: 0x002C81F5
		public int GetResolved()
		{
			if (this.CostsX)
			{
				return this.CapturedXValue;
			}
			return Math.Max(0, this.GetWithModifiers(CostModifiers.All));
		}

		/// <summary>
		/// Set this cost to the specified amount until the card is played.
		/// </summary>
		/// <example>
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Eidolon" /> says "Reduce the cost of all cards in your Discard Pile to 0 until played."
		/// </example>
		/// <param name="cost">New cost.</param>
		/// <param name="reduceOnly">
		/// Whether this modifier should only be included in the cost calculation if it would lower the current cost.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.LocalCostModifier.IsReduceOnly" /> for details.
		/// </param>
		// Token: 0x06008484 RID: 33924 RVA: 0x002CA013 File Offset: 0x002C8213
		public void SetUntilPlayed(int cost, bool reduceOnly = false)
		{
			if (cost == 0 && this.Canonical < 0)
			{
				return;
			}
			this._localModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.WhenPlayed, reduceOnly));
		}

		/// <summary>
		/// Set this cost to the specified amount until the end of the current turn OR until the card is played, whichever
		/// comes first.
		/// Note that the text of these effects will just say "this turn"; the "or until played" part is left implicit
		/// because it's wordy and rarely relevant.
		/// </summary>
		/// <example>
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Cards.BulletTime" /> says "Reduce the cost of ALL cards in your Hand to 0 this turn."
		/// </example>
		/// <param name="cost">New cost.</param>
		/// <param name="reduceOnly">
		/// Whether this modifier should only be included in the cost calculation if it would lower the current cost.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.LocalCostModifier.IsReduceOnly" /> for details.
		/// </param>
		// Token: 0x06008485 RID: 33925 RVA: 0x002CA036 File Offset: 0x002C8236
		public void SetThisTurnOrUntilPlayed(int cost, bool reduceOnly = false)
		{
			if (cost == 0 && this.Canonical < 0)
			{
				return;
			}
			this._localModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.EndOfTurn | LocalCostModifierExpiration.WhenPlayed, reduceOnly));
		}

		/// <summary>
		/// BE CAREFUL USING THIS! You usually want <see cref="M:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost.SetThisTurnOrUntilPlayed(System.Int32,System.Boolean)" /> instead.
		/// Set this cost to the specified amount until the end of the current turn.
		/// Note that most effects that say "this turn" really mean "this turn or until played".
		/// This method should only be used for the few effects that should last for multiple plays in the same turn.
		/// </summary>
		/// <example>
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Invoke" /> says "This card costs 0 if Osty has attacked this turn."
		/// </example>
		/// <param name="cost">New cost.</param>
		/// <param name="reduceOnly">
		/// Whether this modifier should only be included in the cost calculation if it would lower the current cost.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.LocalCostModifier.IsReduceOnly" /> for details.
		/// </param>
		// Token: 0x06008486 RID: 33926 RVA: 0x002CA059 File Offset: 0x002C8259
		public void SetThisTurn(int cost, bool reduceOnly = false)
		{
			if (cost == 0 && this.Canonical < 0)
			{
				return;
			}
			this._localModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.EndOfTurn, reduceOnly));
		}

		/// <summary>
		/// Set this cost to the specified amount for the rest of the combat.
		/// </summary>
		/// <example>
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Enlightenment" />+ says "Reduce the cost of ALL cards in your Hand to 1 this combat."
		/// </example>
		/// <param name="cost">New cost.</param>
		/// <param name="reduceOnly">
		/// Whether this modifier should only be included in the cost calculation if it would lower the current cost.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.LocalCostModifier.IsReduceOnly" /> for details.
		/// </param>
		// Token: 0x06008487 RID: 33927 RVA: 0x002CA07C File Offset: 0x002C827C
		public void SetThisCombat(int cost, bool reduceOnly = false)
		{
			if (cost == 0 && this.Canonical < 0)
			{
				return;
			}
			this._localModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.EndOfCombat, reduceOnly));
		}

		/// <summary>
		/// Add the specified amount to this cost until the card is played.
		/// </summary>
		/// <example>
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Enchantments.SlumberingEssence" /> says "If this card is in your hand at the end of turn, reduce its cost by 1
		/// until it is played."
		/// </example>
		/// <param name="amount">Amount to add to the cost.</param>
		/// <param name="reduceOnly">
		/// Whether this modifier should only be included in the cost calculation if it would lower the current cost.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.LocalCostModifier.IsReduceOnly" /> for details.
		/// </param>
		// Token: 0x06008488 RID: 33928 RVA: 0x002CA09F File Offset: 0x002C829F
		public void AddUntilPlayed(int amount, bool reduceOnly = false)
		{
			if (amount == 0)
			{
				return;
			}
			this._localModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.WhenPlayed, reduceOnly));
		}

		/// <summary>
		/// Add the specified amount to this cost until the end of the current turn OR until the card is played, whichever
		/// comes first.
		/// Note that the text of these effects will just say "this turn"; the "or until played" part is left implicit
		/// because it's wordy and rarely relevant.
		/// </summary>
		/// <example>None yet. Update this if we add one!</example>
		/// <param name="amount">Amount to add to the cost.</param>
		/// <param name="reduceOnly">
		/// Whether this modifier should only be included in the cost calculation if it would lower the current cost.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.LocalCostModifier.IsReduceOnly" /> for details.
		/// </param>
		// Token: 0x06008489 RID: 33929 RVA: 0x002CA0B9 File Offset: 0x002C82B9
		public void AddThisTurnOrUntilPlayed(int amount, bool reduceOnly = false)
		{
			if (amount == 0)
			{
				return;
			}
			this._localModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.EndOfTurn | LocalCostModifierExpiration.WhenPlayed, reduceOnly));
		}

		/// <summary>
		/// BE CAREFUL USING THIS! You usually want <see cref="M:MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost.AddThisTurnOrUntilPlayed(System.Int32,System.Boolean)" /> instead.
		/// Add the specified amount to this cost until the end of the current turn.
		/// Note that most effects that say "this turn" really mean "this turn or until played".
		/// This method should only be used for the few effects that should last for multiple plays in the same turn.
		/// </summary>
		/// <example>
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Pinpoint" /> says "Costs 1 less for each Skill played this turn."
		/// </example>
		/// <param name="amount">Amount to add to the cost.</param>
		/// <param name="reduceOnly">
		/// Whether this modifier should only be included in the cost calculation if it would lower the current cost.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.LocalCostModifier.IsReduceOnly" /> for details.
		/// </param>
		// Token: 0x0600848A RID: 33930 RVA: 0x002CA0D3 File Offset: 0x002C82D3
		public void AddThisTurn(int amount, bool reduceOnly = false)
		{
			if (amount == 0)
			{
				return;
			}
			this._localModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.EndOfTurn, reduceOnly));
		}

		/// <summary>
		/// Add the specified amount to this cost for the rest of the combat.
		/// </summary>
		/// <example>
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Cards.KinglyKick" /> says "Whenever you draw this card, lower its cost by 1 this combat."
		/// </example>
		/// <param name="amount">Amount to add to the cost.</param>
		/// <param name="reduceOnly">
		/// Whether this modifier should only be included in the cost calculation if it would lower the current cost.
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Cards.LocalCostModifier.IsReduceOnly" /> for details.
		/// </param>
		// Token: 0x0600848B RID: 33931 RVA: 0x002CA0ED File Offset: 0x002C82ED
		public void AddThisCombat(int amount, bool reduceOnly = false)
		{
			if (amount == 0)
			{
				return;
			}
			this._localModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.EndOfCombat, reduceOnly));
		}

		/// <summary>
		/// Clear local cost modifiers that should last until the end of the turn.
		/// </summary>
		/// <returns>True if any modifiers were cleared and EnergyCostChanged should be invoked.</returns>
		// Token: 0x0600848C RID: 33932 RVA: 0x002CA107 File Offset: 0x002C8307
		public bool EndOfTurnCleanup()
		{
			this._card.AssertMutable();
			return this._localModifiers.RemoveAll((LocalCostModifier m) => m.Expiration.HasFlag(LocalCostModifierExpiration.EndOfTurn)) > 0;
		}

		/// <summary>
		/// Clear local cost modifiers that should last until the card is played.
		/// </summary>
		/// <returns>True if any modifiers were cleared and EnergyCostChanged should be invoked.</returns>
		// Token: 0x0600848D RID: 33933 RVA: 0x002CA141 File Offset: 0x002C8341
		public bool AfterCardPlayedCleanup()
		{
			this._card.AssertMutable();
			return this._localModifiers.RemoveAll((LocalCostModifier m) => m.Expiration.HasFlag(LocalCostModifierExpiration.WhenPlayed)) > 0;
		}

		/// <summary>
		/// Upgrade the energy cost of this card by the specified amount.
		/// </summary>
		/// <param name="addend">Amount to add to the current cost (usually negative).</param>
		// Token: 0x0600848E RID: 33934 RVA: 0x002CA17C File Offset: 0x002C837C
		public void UpgradeBy(int addend)
		{
			this._card.AssertMutable();
			if (this.CostsX)
			{
				return;
			}
			if (addend == 0)
			{
				return;
			}
			int @base = this._base;
			int num = Math.Max(this._base + addend, 0);
			this.WasJustUpgraded = true;
			if (num < @base)
			{
				foreach (LocalCostModifier localCostModifier in this._localModifiers)
				{
					if (localCostModifier.Type == LocalCostType.Absolute && localCostModifier.Amount > num)
					{
						localCostModifier.Amount = num;
					}
				}
			}
			this.SetCustomBaseCost(num);
		}

		/// <summary>
		/// Finalize an upgrade after calling UpgradeEnergyCostBy. This clears out state that is used for displaying an upgrade
		/// preview.
		/// </summary>
		// Token: 0x0600848F RID: 33935 RVA: 0x002CA224 File Offset: 0x002C8424
		public void FinalizeUpgrade()
		{
			this._card.AssertMutable();
			this.WasJustUpgraded = false;
		}

		/// <summary>
		/// Reset energy cost to base values during downgrade.
		/// </summary>
		// Token: 0x06008490 RID: 33936 RVA: 0x002CA238 File Offset: 0x002C8438
		public void ResetForDowngrade()
		{
			this._card.AssertMutable();
			this._base = this.Canonical;
			this._card.InvokeEnergyCostChanged();
		}

		/// <summary>
		/// BE VERY CAREFUL USING THIS!
		/// This is mainly meant for internal usage. The only external usage of this should be in <see cref="T:MegaCrit.Sts2.Core.Models.Cards.MadScience" />.
		/// </summary>
		/// <param name="newBaseCost"></param>
		// Token: 0x06008491 RID: 33937 RVA: 0x002CA25C File Offset: 0x002C845C
		public void SetCustomBaseCost(int newBaseCost)
		{
			this._card.AssertMutable();
			this._base = newBaseCost;
			this._card.InvokeEnergyCostChanged();
		}

		/// <summary>
		/// Create a deep clone of this EnergyCostInfo for the specified card.
		/// </summary>
		/// <param name="newCard">The card that will own the cloned EnergyCostInfo.</param>
		/// <returns>A deep clone of this EnergyCostInfo.</returns>
		// Token: 0x06008492 RID: 33938 RVA: 0x002CA27C File Offset: 0x002C847C
		public CardEnergyCost Clone(CardModel newCard)
		{
			List<LocalCostModifier> list = this._localModifiers.Select<LocalCostModifier, LocalCostModifier>((LocalCostModifier m) => m.Clone()).ToList<LocalCostModifier>();
			return new CardEnergyCost(newCard, newCard.EnergyCost.Canonical, newCard.EnergyCost.CostsX)
			{
				_base = this._base,
				_capturedXValue = this._capturedXValue,
				WasJustUpgraded = this.WasJustUpgraded,
				_localModifiers = list
			};
		}

		// Token: 0x04002D78 RID: 11640
		private readonly CardModel _card;

		// Token: 0x04002D79 RID: 11641
		private int _base;

		// Token: 0x04002D7A RID: 11642
		private int _capturedXValue;

		/// <summary>
		/// This card's local cost modifiers.
		/// See <see cref="T:MegaCrit.Sts2.Core.Entities.Cards.LocalCostModifier" /> for details on how this works.
		/// </summary>
		// Token: 0x04002D7B RID: 11643
		private List<LocalCostModifier> _localModifiers = new List<LocalCostModifier>();
	}
}
