using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.GameActions
{
	/// <summary>
	/// This action is enqueued when the player chooses to discard a potion from their belt. It runs both in combat and
	/// outside of combat.
	/// </summary>
	// Token: 0x02000C1D RID: 3101
	[NullableContext(1)]
	[Nullable(0)]
	public class DiscardPotionGameAction : GameAction
	{
		// Token: 0x1700230F RID: 8975
		// (get) Token: 0x06008015 RID: 32789 RVA: 0x002B7B7F File Offset: 0x002B5D7F
		public override ulong OwnerId
		{
			get
			{
				return this._player.NetId;
			}
		}

		// Token: 0x17002310 RID: 8976
		// (get) Token: 0x06008016 RID: 32790 RVA: 0x002B7B8C File Offset: 0x002B5D8C
		public override GameActionType ActionType
		{
			get
			{
				if (!this.WasEnqueuedInCombat)
				{
					return GameActionType.NonCombat;
				}
				return GameActionType.CombatPlayPhaseOnly;
			}
		}

		/// <summary>
		/// True if the player discarded a potion in combat, false otherwise.
		/// The player may discard a potion at any time. However, there's a specific situation in which order of GameAction
		/// enqueues matters when out-of-combat: if the player discards a potion post-combat while another player is still
		/// executing the player turn. If the action is set to GameActionType.Any, it will be executed during combat, before
		/// the enemy turn.
		/// </summary>
		// Token: 0x17002311 RID: 8977
		// (get) Token: 0x06008017 RID: 32791 RVA: 0x002B7B99 File Offset: 0x002B5D99
		public bool WasEnqueuedInCombat { get; }

		// Token: 0x06008018 RID: 32792 RVA: 0x002B7BA1 File Offset: 0x002B5DA1
		public DiscardPotionGameAction(Player player, uint potionSlotIndex, bool isCombatInProgress)
		{
			this._player = player;
			this._potionSlotIndex = potionSlotIndex;
			this.WasEnqueuedInCombat = isCombatInProgress;
		}

		// Token: 0x06008019 RID: 32793 RVA: 0x002B7BC0 File Offset: 0x002B5DC0
		protected override async Task ExecuteAction()
		{
			if ((ulong)this._potionSlotIndex >= (ulong)((long)this._player.PotionSlots.Count))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to discard potion at slot index ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(this._potionSlotIndex);
				defaultInterpolatedStringHandler.AppendLiteral(", but player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" only has ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._player.PotionSlots.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" potion slots!");
				throw new IndexOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			PotionModel potionModel = this._player.PotionSlots[(int)this._potionSlotIndex];
			if (potionModel == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 3);
				defaultInterpolatedStringHandler.AppendFormatted("DiscardPotionGameAction");
				defaultInterpolatedStringHandler.AppendLiteral(": potion at slot index ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(this._potionSlotIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" is null for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(", canceling");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				base.Cancel();
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(potionModel.Owner.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(" discarding potion ");
				defaultInterpolatedStringHandler.AppendFormatted(potionModel.Id.Entry);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				await PotionCmd.Discard(potionModel);
			}
		}

		// Token: 0x0600801A RID: 32794 RVA: 0x002B7C04 File Offset: 0x002B5E04
		protected override void CancelAction()
		{
			PotionModel potionAtSlotIndex = this._player.GetPotionAtSlotIndex((int)this._potionSlotIndex);
			if (TestMode.IsOff && NRun.Instance != null && LocalContext.IsMe(this._player) && potionAtSlotIndex != null)
			{
				NRun.Instance.GlobalUi.TopBar.PotionContainer.OnPotionUseOrDiscardCanceled(potionAtSlotIndex);
			}
			if (potionAtSlotIndex != null)
			{
				potionAtSlotIndex.AfterUsageCanceled();
			}
		}

		// Token: 0x0600801B RID: 32795 RVA: 0x002B7C64 File Offset: 0x002B5E64
		public override INetAction ToNetAction()
		{
			return new NetDiscardPotionGameAction
			{
				potionSlotIndex = this._potionSlotIndex,
				wasEnqueuedInCombat = this.WasEnqueuedInCombat
			};
		}

		// Token: 0x0600801C RID: 32796 RVA: 0x002B7C9C File Offset: 0x002B5E9C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 4);
			defaultInterpolatedStringHandler.AppendFormatted("NetDiscardPotionGameAction");
			defaultInterpolatedStringHandler.AppendLiteral(" for player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" potion slot: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this._potionSlotIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" in combat: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.WasEnqueuedInCombat);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002AD8 RID: 10968
		private readonly Player _player;

		// Token: 0x04002AD9 RID: 10969
		private readonly uint _potionSlotIndex;
	}
}
