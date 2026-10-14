using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Combat.History
{
	/// <summary>
	/// A history of most events that have happened in combat.
	/// Each entry should be logged immediately after the event occurs, but before its AfterX hook is executed.
	/// For example, a CardPlayedEntry is logged immediately after the card is played, but before AfterCardPlayed is
	/// executed.
	/// </summary>
	// Token: 0x02000D47 RID: 3399
	[NullableContext(1)]
	[Nullable(0)]
	public class CombatHistory
	{
		// Token: 0x140000DE RID: 222
		// (add) Token: 0x060089C8 RID: 35272 RVA: 0x002E3164 File Offset: 0x002E1364
		// (remove) Token: 0x060089C9 RID: 35273 RVA: 0x002E319C File Offset: 0x002E139C
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action Changed;

		// Token: 0x170025C2 RID: 9666
		// (get) Token: 0x060089CA RID: 35274 RVA: 0x002E31D1 File Offset: 0x002E13D1
		public IEnumerable<CombatHistoryEntry> Entries
		{
			get
			{
				return this._entries;
			}
		}

		// Token: 0x170025C3 RID: 9667
		// (get) Token: 0x060089CB RID: 35275 RVA: 0x002E31D9 File Offset: 0x002E13D9
		public IEnumerable<CardPlayStartedEntry> CardPlaysStarted
		{
			get
			{
				return this.Entries.OfType<CardPlayStartedEntry>();
			}
		}

		// Token: 0x170025C4 RID: 9668
		// (get) Token: 0x060089CC RID: 35276 RVA: 0x002E31E6 File Offset: 0x002E13E6
		public IEnumerable<CardPlayFinishedEntry> CardPlaysFinished
		{
			get
			{
				return this.Entries.OfType<CardPlayFinishedEntry>();
			}
		}

		// Token: 0x060089CD RID: 35277 RVA: 0x002E31F3 File Offset: 0x002E13F3
		public void Clear()
		{
			this._entries.Clear();
			Action changed = this.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060089CE RID: 35278 RVA: 0x002E3210 File Offset: 0x002E1410
		public void CardPlayStarted(ICombatState combatState, CardPlay cardPlay)
		{
			this.Add(combatState, new CardPlayStartedEntry(cardPlay, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089CF RID: 35279 RVA: 0x002E3232 File Offset: 0x002E1432
		public void CardPlayFinished(ICombatState combatState, CardPlay cardPlay)
		{
			this.Add(combatState, new CardPlayFinishedEntry(cardPlay, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089D0 RID: 35280 RVA: 0x002E3254 File Offset: 0x002E1454
		public void CardAfflicted(ICombatState combatState, CardModel card, AfflictionModel affliction)
		{
			this.Add(combatState, new CardAfflictedEntry(card, affliction, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089D1 RID: 35281 RVA: 0x002E3277 File Offset: 0x002E1477
		public void CardDiscarded(ICombatState combatState, CardModel card)
		{
			this.Add(combatState, new CardDiscardedEntry(card, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089D2 RID: 35282 RVA: 0x002E3299 File Offset: 0x002E1499
		public void CardDrawn(ICombatState combatState, CardModel card, bool fromHandDraw)
		{
			this.Add(combatState, new CardDrawnEntry(card, combatState.RoundNumber, combatState.CurrentSide, fromHandDraw, this, combatState.Players));
		}

		// Token: 0x060089D3 RID: 35283 RVA: 0x002E32BC File Offset: 0x002E14BC
		public void CardExhausted(ICombatState combatState, CardModel card)
		{
			this.Add(combatState, new CardExhaustedEntry(card, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089D4 RID: 35284 RVA: 0x002E32DE File Offset: 0x002E14DE
		public void CardGenerated(ICombatState combatState, CardModel card, [Nullable(2)] Player creator)
		{
			this.Add(combatState, new CardGeneratedEntry(card, creator, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089D5 RID: 35285 RVA: 0x002E3301 File Offset: 0x002E1501
		public void CreatureAttacked(ICombatState combatState, Creature attacker, IReadOnlyList<DamageResult> damageResults)
		{
			this.Add(combatState, new CreatureAttackedEntry(attacker, damageResults, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089D6 RID: 35286 RVA: 0x002E3324 File Offset: 0x002E1524
		public void DamageReceived(ICombatState combatState, Creature receiver, [Nullable(2)] Creature dealer, DamageResult result, [Nullable(2)] CardModel cardSource)
		{
			this.Add(combatState, new DamageReceivedEntry(result, receiver, dealer, cardSource, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089D7 RID: 35287 RVA: 0x002E3358 File Offset: 0x002E1558
		public void BlockGained(ICombatState combatState, Creature receiver, int amount, ValueProp props, [Nullable(2)] CardPlay cardPlay)
		{
			this.Add(combatState, new BlockGainedEntry(amount, props, cardPlay, receiver, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089D8 RID: 35288 RVA: 0x002E338A File Offset: 0x002E158A
		public void EnergySpent(ICombatState combatState, int amount, Player player)
		{
			this.Add(combatState, new EnergySpentEntry(amount, player, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089D9 RID: 35289 RVA: 0x002E33B0 File Offset: 0x002E15B0
		public void MonsterPerformedMove(ICombatState combatState, MonsterModel monster, MoveState move, [Nullable(new byte[] { 2, 1 })] IEnumerable<Creature> targets)
		{
			this.Add(combatState, new MonsterPerformedMoveEntry(monster, move, targets, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089DA RID: 35290 RVA: 0x002E33E0 File Offset: 0x002E15E0
		public void OrbChanneled(ICombatState combatState, OrbModel orb)
		{
			this.Add(combatState, new OrbChanneledEntry(orb, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089DB RID: 35291 RVA: 0x002E3402 File Offset: 0x002E1602
		public void PotionUsed(ICombatState combatState, PotionModel potion, [Nullable(2)] Creature target)
		{
			this.Add(combatState, new PotionUsedEntry(potion, target, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089DC RID: 35292 RVA: 0x002E3428 File Offset: 0x002E1628
		public void PowerReceived(ICombatState combatState, PowerModel power, decimal amount, [Nullable(2)] Creature applier)
		{
			this.Add(combatState, new PowerReceivedEntry(power, amount, applier, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089DD RID: 35293 RVA: 0x002E3458 File Offset: 0x002E1658
		public void StarsModified(ICombatState combatState, int amount, Player player)
		{
			this.Add(combatState, new StarsModifiedEntry(amount, player, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089DE RID: 35294 RVA: 0x002E347B File Offset: 0x002E167B
		public void Summoned(ICombatState combatState, int amount, Player player)
		{
			this.Add(combatState, new SummonedEntry(amount, player, combatState.RoundNumber, combatState.CurrentSide, this, combatState.Players));
		}

		// Token: 0x060089DF RID: 35295 RVA: 0x002E349E File Offset: 0x002E169E
		private void Add(ICombatState combatState, CombatHistoryEntry entry)
		{
			if (!combatState.IsLiveCombat())
			{
				return;
			}
			this._entries.Add(entry);
			Action changed = this.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x04002FED RID: 12269
		private readonly List<CombatHistoryEntry> _entries = new List<CombatHistoryEntry>();
	}
}
