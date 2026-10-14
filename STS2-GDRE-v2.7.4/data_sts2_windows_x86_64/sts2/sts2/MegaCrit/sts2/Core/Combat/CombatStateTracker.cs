using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Combat
{
	// Token: 0x02000D43 RID: 3395
	[NullableContext(1)]
	[Nullable(0)]
	public class CombatStateTracker
	{
		/// <summary>
		/// Fired whenever any part of the combat's state changes (card changes piles, power applied/removed, damage dealt,
		/// block gained/lost, etc.).
		/// </summary>
		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06008956 RID: 35158 RVA: 0x002E2954 File Offset: 0x002E0B54
		// (remove) Token: 0x06008957 RID: 35159 RVA: 0x002E298C File Offset: 0x002E0B8C
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CombatState> CombatStateChanged;

		// Token: 0x06008958 RID: 35160 RVA: 0x002E29C4 File Offset: 0x002E0BC4
		public CombatStateTracker(CombatManager combatManager)
		{
			this._combatManager = combatManager;
			this._combatManager.History.Changed += this.OnCombatHistoryChanged;
			this._combatManager.CombatBegan += this.OnCombatBegan;
			this._combatManager.CreaturesChanged += this.OnCreaturesChanged;
			this._combatManager.TurnStarted += this.OnTurnStarted;
			this._combatManager.TurnEnded += this.OnTurnEnded;
		}

		// Token: 0x06008959 RID: 35161 RVA: 0x002E2A58 File Offset: 0x002E0C58
		~CombatStateTracker()
		{
			this._combatManager.History.Changed -= this.OnCombatHistoryChanged;
			this._combatManager.CombatBegan -= this.OnCombatBegan;
			this._combatManager.CreaturesChanged -= this.OnCreaturesChanged;
			this._combatManager.TurnStarted -= this.OnTurnStarted;
			this._combatManager.TurnEnded -= this.OnTurnEnded;
		}

		// Token: 0x0600895A RID: 35162 RVA: 0x002E2AF8 File Offset: 0x002E0CF8
		public void SetState(CombatState state)
		{
			this._state = state;
		}

		// Token: 0x0600895B RID: 35163 RVA: 0x002E2B04 File Offset: 0x002E0D04
		public void Subscribe(CardModel card)
		{
			card.AfflictionChanged += this.OnCardValueChanged;
			card.EnchantmentChanged += this.OnCardValueChanged;
			card.EnergyCostChanged += this.OnCardValueChanged;
			card.ReplayCountChanged += this.OnCardValueChanged;
			card.Played += this.OnCardValueChanged;
			card.Drawn += this.OnCardValueChanged;
			card.StarCostChanged += this.OnCardValueChanged;
			card.Upgraded += this.OnCardValueChanged;
			card.Forged += this.OnCardValueChanged;
		}

		// Token: 0x0600895C RID: 35164 RVA: 0x002E2BB4 File Offset: 0x002E0DB4
		public void Unsubscribe(CardModel card)
		{
			card.AfflictionChanged -= this.OnCardValueChanged;
			card.EnchantmentChanged -= this.OnCardValueChanged;
			card.EnergyCostChanged -= this.OnCardValueChanged;
			card.ReplayCountChanged -= this.OnCardValueChanged;
			card.Played -= this.OnCardValueChanged;
			card.Drawn -= this.OnCardValueChanged;
			card.StarCostChanged -= this.OnCardValueChanged;
			card.Upgraded -= this.OnCardValueChanged;
			card.Forged -= this.OnCardValueChanged;
		}

		// Token: 0x0600895D RID: 35165 RVA: 0x002E2C63 File Offset: 0x002E0E63
		public void Subscribe(CardPile pile)
		{
			pile.ContentsChanged += this.OnCardPileContentsChanged;
		}

		// Token: 0x0600895E RID: 35166 RVA: 0x002E2C77 File Offset: 0x002E0E77
		public void Unsubscribe(CardPile pile)
		{
			pile.ContentsChanged -= this.OnCardPileContentsChanged;
		}

		// Token: 0x0600895F RID: 35167 RVA: 0x002E2C8C File Offset: 0x002E0E8C
		public void Subscribe(Creature creature)
		{
			creature.BlockChanged += this.OnCreatureValueChanged;
			creature.CurrentHpChanged += this.OnCreatureValueChanged;
			creature.MaxHpChanged += this.OnCreatureValueChanged;
			creature.PowerApplied += this.OnPowerAppliedOrRemoved;
			creature.PowerIncreased += this.OnPowerIncreased;
			creature.PowerDecreased += this.OnPowerDecreased;
			creature.PowerRemoved += this.OnPowerAppliedOrRemoved;
			creature.Died += this.OnCreatureChanged;
		}

		// Token: 0x06008960 RID: 35168 RVA: 0x002E2D2C File Offset: 0x002E0F2C
		public void Unsubscribe(Creature creature)
		{
			creature.BlockChanged -= this.OnCreatureValueChanged;
			creature.CurrentHpChanged -= this.OnCreatureValueChanged;
			creature.MaxHpChanged -= this.OnCreatureValueChanged;
			creature.PowerApplied -= this.OnPowerAppliedOrRemoved;
			creature.PowerIncreased -= this.OnPowerIncreased;
			creature.PowerDecreased -= this.OnPowerDecreased;
			creature.PowerRemoved -= this.OnPowerAppliedOrRemoved;
			creature.Died -= this.OnCreatureChanged;
		}

		// Token: 0x06008961 RID: 35169 RVA: 0x002E2DC9 File Offset: 0x002E0FC9
		public void Subscribe(PlayerCombatState combatState)
		{
			combatState.EnergyChanged += this.OnPlayerCombatStateValueChanged;
			combatState.PlayerTurnPhaseChanged += this.OnPlayerStateChanged;
			combatState.StarsChanged += this.OnPlayerCombatStateValueChanged;
		}

		// Token: 0x06008962 RID: 35170 RVA: 0x002E2E01 File Offset: 0x002E1001
		public void Unsubscribe(PlayerCombatState combatState)
		{
			combatState.EnergyChanged -= this.OnPlayerCombatStateValueChanged;
			combatState.PlayerTurnPhaseChanged -= this.OnPlayerStateChanged;
			combatState.StarsChanged -= this.OnPlayerCombatStateValueChanged;
		}

		// Token: 0x06008963 RID: 35171 RVA: 0x002E2E39 File Offset: 0x002E1039
		private void OnCombatBegan(CombatState _)
		{
			this.NotifyCombatStateChanged("OnCombatBegan");
		}

		// Token: 0x06008964 RID: 35172 RVA: 0x002E2E46 File Offset: 0x002E1046
		private void OnCardPileContentsChanged()
		{
			this.NotifyCombatStateChanged("OnCardPileContentsChanged");
		}

		// Token: 0x06008965 RID: 35173 RVA: 0x002E2E53 File Offset: 0x002E1053
		private void OnCardValueChanged()
		{
			this.NotifyCombatStateChanged("OnCardValueChanged");
		}

		// Token: 0x06008966 RID: 35174 RVA: 0x002E2E60 File Offset: 0x002E1060
		private void OnCombatHistoryChanged()
		{
			this.NotifyCombatStateChanged("OnCombatHistoryChanged");
		}

		// Token: 0x06008967 RID: 35175 RVA: 0x002E2E6D File Offset: 0x002E106D
		private void OnCreatureValueChanged(int _, int __)
		{
			this.NotifyCombatStateChanged("OnCreatureValueChanged");
		}

		// Token: 0x06008968 RID: 35176 RVA: 0x002E2E7A File Offset: 0x002E107A
		private void OnCreaturesChanged(CombatState _)
		{
			this.NotifyCombatStateChanged("OnCreatureChanged");
		}

		// Token: 0x06008969 RID: 35177 RVA: 0x002E2E87 File Offset: 0x002E1087
		private void OnCreatureChanged(Creature _)
		{
			this.NotifyCombatStateChanged("OnCreaturesChanged");
		}

		// Token: 0x0600896A RID: 35178 RVA: 0x002E2E94 File Offset: 0x002E1094
		private void OnPlayerStateChanged()
		{
			this.NotifyCombatStateChanged("OnPlayerStateChanged");
		}

		// Token: 0x0600896B RID: 35179 RVA: 0x002E2EA1 File Offset: 0x002E10A1
		private void OnPlayerCombatStateValueChanged(int _, int __)
		{
			this.NotifyCombatStateChanged("OnPlayerCombatStateValueChanged");
		}

		// Token: 0x0600896C RID: 35180 RVA: 0x002E2EAE File Offset: 0x002E10AE
		private void OnPowerAppliedOrRemoved(PowerModel _)
		{
			this.NotifyCombatStateChanged("OnPowerAppliedOrRemoved");
		}

		// Token: 0x0600896D RID: 35181 RVA: 0x002E2EBB File Offset: 0x002E10BB
		private void OnPowerDecreased(PowerModel _, bool __)
		{
			this.NotifyCombatStateChanged("OnPowerDecreased");
		}

		// Token: 0x0600896E RID: 35182 RVA: 0x002E2EC8 File Offset: 0x002E10C8
		private void OnPowerIncreased(PowerModel _, int __, bool ___)
		{
			this.NotifyCombatStateChanged("OnPowerIncreased");
		}

		// Token: 0x0600896F RID: 35183 RVA: 0x002E2ED5 File Offset: 0x002E10D5
		private void OnTurnStarted(CombatState _)
		{
			this.NotifyCombatStateChanged("OnTurnStarted");
		}

		// Token: 0x06008970 RID: 35184 RVA: 0x002E2EE2 File Offset: 0x002E10E2
		private void OnTurnEnded(CombatState _)
		{
			this.NotifyCombatStateChanged("OnTurnEnded");
		}

		/// <summary>
		/// Notify the world that something in the combat state changed.
		/// </summary>
		/// <param name="caller">The name of the method that triggered this.</param>
		// Token: 0x06008971 RID: 35185 RVA: 0x002E2EF0 File Offset: 0x002E10F0
		private void NotifyCombatStateChanged(string caller)
		{
			if (!TestMode.IsOn)
			{
				if (this._combatStateChangedDeferredTask == null || this._combatStateChangedDeferredTask.IsCompleted)
				{
					this._combatStateChangedDeferredTask = TaskHelper.RunSafely(this.CallCombatStateChangedDeferred());
				}
				return;
			}
			if (this.CombatStateChanged != null)
			{
				throw new InvalidOperationException("Backend should not be subscribing to CombatStateChanged!");
			}
		}

		// Token: 0x06008972 RID: 35186 RVA: 0x002E2F40 File Offset: 0x002E1140
		private async Task CallCombatStateChangedDeferred()
		{
			Node instance = NRun.Instance;
			if (((instance != null) ? instance.GetTreeOrNull() : null) != null)
			{
				await instance.AwaitProcessFrame(default(CancellationToken));
			}
			CombatState state = this._state;
			if (state != null)
			{
				IReadOnlyList<Creature> creatures = state.Creatures;
				if (creatures != null && creatures.Count > 0)
				{
					Player me = LocalContext.GetMe(this._state);
					if (me != null)
					{
						PlayerCombatState playerCombatState = me.PlayerCombatState;
						if (playerCombatState != null)
						{
							playerCombatState.RecalculateCardValues();
						}
					}
					Action<CombatState> combatStateChanged = this.CombatStateChanged;
					if (combatStateChanged != null)
					{
						combatStateChanged(this._state);
					}
				}
			}
		}

		// Token: 0x04002FD6 RID: 12246
		private readonly CombatManager _combatManager;

		// Token: 0x04002FD7 RID: 12247
		[Nullable(2)]
		private Task _combatStateChangedDeferredTask;

		// Token: 0x04002FD8 RID: 12248
		[Nullable(2)]
		private CombatState _state;
	}
}
