using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C2E RID: 3118
	[NullableContext(1)]
	[Nullable(0)]
	public class UsePotionAction : GameAction
	{
		// Token: 0x17002333 RID: 9011
		// (get) Token: 0x060080B5 RID: 32949 RVA: 0x002B9EB2 File Offset: 0x002B80B2
		public override ulong OwnerId
		{
			get
			{
				return this.Player.NetId;
			}
		}

		// Token: 0x17002334 RID: 9012
		// (get) Token: 0x060080B6 RID: 32950 RVA: 0x002B9EBF File Offset: 0x002B80BF
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

		// Token: 0x17002335 RID: 9013
		// (get) Token: 0x060080B7 RID: 32951 RVA: 0x002B9ECC File Offset: 0x002B80CC
		public Player Player { get; }

		// Token: 0x17002336 RID: 9014
		// (get) Token: 0x060080B8 RID: 32952 RVA: 0x002B9ED4 File Offset: 0x002B80D4
		public uint PotionIndex { get; }

		// Token: 0x17002337 RID: 9015
		// (get) Token: 0x060080B9 RID: 32953 RVA: 0x002B9EDC File Offset: 0x002B80DC
		public uint? TargetId { get; }

		// Token: 0x17002338 RID: 9016
		// (get) Token: 0x060080BA RID: 32954 RVA: 0x002B9EE4 File Offset: 0x002B80E4
		public bool WasEnqueuedInCombat { get; }

		// Token: 0x17002339 RID: 9017
		// (get) Token: 0x060080BB RID: 32955 RVA: 0x002B9EEC File Offset: 0x002B80EC
		private ulong? TargetPlayerId { get; }

		// Token: 0x1700233A RID: 9018
		// (get) Token: 0x060080BC RID: 32956 RVA: 0x002B9EF4 File Offset: 0x002B80F4
		// (set) Token: 0x060080BD RID: 32957 RVA: 0x002B9EFC File Offset: 0x002B80FC
		[Nullable(2)]
		public PlayerChoiceContext PlayerChoiceContext
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		/// <summary>
		/// Constructor to use for constructing the UsePotionAction outside of serialization.
		/// </summary>
		/// <param name="potion">The potion which will be used.</param>
		/// <param name="target">The target which the player is using the potion on. Pass null if the potion is not targeted.</param>
		/// <param name="isCombatInProgress">Pass true if the potion was played during combat. The GameAction will be cancelled at
		/// the end of combat if it is still enqueued.</param>
		// Token: 0x060080BE RID: 32958 RVA: 0x002B9F08 File Offset: 0x002B8108
		public UsePotionAction(PotionModel potion, [Nullable(2)] Creature target, bool isCombatInProgress)
		{
			if (potion.Owner == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot enqueue UsePotionAction for potion ");
				defaultInterpolatedStringHandler.AppendFormatted<PotionModel>(potion);
				defaultInterpolatedStringHandler.AppendLiteral(" without an owner!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int potionSlotIndex = potion.Owner.GetPotionSlotIndex(potion);
			if (potionSlotIndex < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Potion ");
				defaultInterpolatedStringHandler.AppendFormatted<PotionModel>(potion);
				defaultInterpolatedStringHandler.AppendLiteral(" has owner ");
				defaultInterpolatedStringHandler.AppendFormatted<Player>(this.Player);
				defaultInterpolatedStringHandler.AppendLiteral(", but the owner's potion list does not contain it!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Player = potion.Owner;
			this.PotionIndex = (uint)potionSlotIndex;
			this.WasEnqueuedInCombat = isCombatInProgress;
			if (target != null)
			{
				if (target.CombatId == null)
				{
					if (CombatManager.Instance.IsInProgress)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(81, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Trying to target potion ");
						defaultInterpolatedStringHandler.AppendFormatted<PotionModel>(potion);
						defaultInterpolatedStringHandler.AppendLiteral(" at target ");
						defaultInterpolatedStringHandler.AppendFormatted<Creature>(target);
						defaultInterpolatedStringHandler.AppendLiteral(" that has no combat ID assigned during combat!");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					if (!target.IsPlayer)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Trying to target potion ");
						defaultInterpolatedStringHandler.AppendFormatted<PotionModel>(potion);
						defaultInterpolatedStringHandler.AppendLiteral(" at target ");
						defaultInterpolatedStringHandler.AppendFormatted<Creature>(target);
						defaultInterpolatedStringHandler.AppendLiteral(" outside of combat that is not a player!");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				this.TargetId = target.CombatId;
				Player player = target.Player;
				this.TargetPlayerId = ((player != null) ? new ulong?(player.NetId) : null);
			}
		}

		/// <summary>
		/// Constructor to use when deserializing a NetUsePotionAction. Should not be used in other circumstances.
		/// </summary>
		/// <param name="player">The player who sent us the action.</param>
		/// <param name="potionIndex">The index of the potion that will be used.</param>
		/// <param name="targetId">The combat ID of the target.</param>
		/// <param name="targetPlayerId">The NetID of the player that is targeted, if the potion was used outside of combat.</param>
		/// <param name="isCombatInProgress">Whether or not combat was in progress when the potion was used.</param>
		// Token: 0x060080BF RID: 32959 RVA: 0x002BA0C9 File Offset: 0x002B82C9
		public UsePotionAction(Player player, uint potionIndex, uint? targetId, ulong? targetPlayerId, bool isCombatInProgress)
		{
			this.Player = player;
			this.PotionIndex = potionIndex;
			this.TargetId = targetId;
			this.TargetPlayerId = targetPlayerId;
			this.WasEnqueuedInCombat = isCombatInProgress;
		}

		// Token: 0x060080C0 RID: 32960 RVA: 0x002BA0F8 File Offset: 0x002B82F8
		protected override async Task ExecuteAction()
		{
			PotionModel potion = this.Player.GetPotionAtSlotIndex((int)this.PotionIndex);
			if (potion == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
				defaultInterpolatedStringHandler.AppendFormatted("UsePotionAction");
				defaultInterpolatedStringHandler.AppendLiteral(": potion at index ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(this.PotionIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" is null for player ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.Player.NetId);
				defaultInterpolatedStringHandler.AppendLiteral(", canceling");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				base.Cancel();
			}
			else
			{
				Creature creature = null;
				if (CombatManager.Instance.IsInProgress)
				{
					if (this.TargetId == null && potion.TargetType.IsSingleTarget())
					{
						creature = this.Player.Creature;
					}
					else
					{
						Creature creature2 = await this.Player.Creature.CombatState.GetCreatureAsync(this.TargetId, 10.0);
						creature = creature2;
					}
				}
				else if (this.TargetPlayerId == null && potion.TargetType != TargetType.TargetedNoCreature)
				{
					creature = this.Player.Creature;
				}
				else if (this.TargetPlayerId != null)
				{
					creature = this.Player.RunState.GetPlayer(this.TargetPlayerId.Value).Creature;
				}
				if (!potion.IsValidTarget(creature))
				{
					base.Cancel();
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					string text;
					if (creature == null)
					{
						text = "no target";
					}
					else
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
						defaultInterpolatedStringHandler.AppendLiteral("targeting ");
						defaultInterpolatedStringHandler.AppendFormatted(creature.LogName);
						defaultInterpolatedStringHandler.AppendLiteral(" (index ");
						ICombatState combatState = this.Player.Creature.CombatState;
						defaultInterpolatedStringHandler.AppendFormatted<int?>((combatState != null) ? new int?(combatState.Creatures.IndexOf(creature)) : null);
						defaultInterpolatedStringHandler.AppendLiteral(")");
						text = defaultInterpolatedStringHandler.ToStringAndClear();
					}
					string text2 = text;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(potion.Owner.NetId);
					defaultInterpolatedStringHandler.AppendLiteral(" using potion ");
					defaultInterpolatedStringHandler.AppendFormatted(potion.Id.Entry);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted(text2);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					this.PlayerChoiceContext = new GameActionPlayerChoiceContext(this);
					await potion.OnUseWrapper(this.PlayerChoiceContext, creature);
				}
			}
		}

		// Token: 0x060080C1 RID: 32961 RVA: 0x002BA13C File Offset: 0x002B833C
		protected override void CancelAction()
		{
			PotionModel potionAtSlotIndex = this.Player.GetPotionAtSlotIndex((int)this.PotionIndex);
			if (TestMode.IsOff && NRun.Instance != null && LocalContext.IsMe(this.Player) && potionAtSlotIndex != null)
			{
				NRun.Instance.GlobalUi.TopBar.PotionContainer.OnPotionUseOrDiscardCanceled(potionAtSlotIndex);
			}
			if (potionAtSlotIndex != null)
			{
				potionAtSlotIndex.AfterUsageCanceled();
			}
		}

		// Token: 0x060080C2 RID: 32962 RVA: 0x002BA19C File Offset: 0x002B839C
		public override INetAction ToNetAction()
		{
			return new NetUsePotionAction
			{
				potionIndex = this.PotionIndex,
				targetId = this.TargetId,
				targetPlayerId = this.TargetPlayerId,
				enqueuedInCombat = this.WasEnqueuedInCombat
			};
		}

		// Token: 0x060080C3 RID: 32963 RVA: 0x002BA1EC File Offset: 0x002B83EC
		public override string ToString()
		{
			PotionModel potionAtSlotIndex = this.Player.GetPotionAtSlotIndex((int)this.PotionIndex);
			ICombatState combatState = this.Player.Creature.CombatState;
			Creature creature = ((combatState != null) ? combatState.GetCreature(this.TargetId) : null);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 7);
			defaultInterpolatedStringHandler.AppendFormatted("UsePotionAction");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.Player.NetId);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<PotionModel>(potionAtSlotIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" index: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.PotionIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendLiteral("target: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint?>(this.TargetId);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<Creature>(creature);
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			defaultInterpolatedStringHandler.AppendLiteral("combat: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.WasEnqueuedInCombat);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
