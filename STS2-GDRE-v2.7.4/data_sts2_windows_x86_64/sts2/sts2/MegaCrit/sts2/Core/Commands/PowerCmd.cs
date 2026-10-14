using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D35 RID: 3381
	[NullableContext(1)]
	[Nullable(0)]
	public static class PowerCmd
	{
		/// <summary>
		/// Apply a power to multiple creatures.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="targets">Creatures to apply the power to.</param>
		/// <param name="amount">Amount of the power to apply.</param>
		/// <param name="applier">Creature applying the power.</param>
		/// <param name="cardSource">Optional card that is applying the power.</param>
		/// <param name="silent">Pass true if no flashes should play for this power application.</param>
		/// <typeparam name="T">Type of power to apply.</typeparam>
		/// <returns>
		/// The power that was applied.
		/// Can be null if:
		/// * Combat is over.
		/// * Something blocked application (like Artifact).
		/// * The power's amount was changed to 0 (like if you had 3 Strength and applied -3 Strength).
		/// </returns>
		// Token: 0x0600885F RID: 34911 RVA: 0x002DEDB8 File Offset: 0x002DCFB8
		public static async Task<IReadOnlyList<T>> Apply<[Nullable(0)] T>(PlayerChoiceContext choiceContext, [Nullable(new byte[] { 2, 1 })] IEnumerable<Creature> targets, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource, bool silent = false) where T : PowerModel
		{
			List<T> powers = new List<T>();
			IReadOnlyList<T> readOnlyList;
			if (targets == null)
			{
				readOnlyList = powers;
			}
			else
			{
				foreach (Creature creature in targets)
				{
					T t = await PowerCmd.Apply<T>(choiceContext, creature, amount, applier, cardSource, silent);
					T t2 = t;
					if (t2 != null)
					{
						powers.Add(t2);
					}
				}
				IEnumerator<Creature> enumerator = null;
				readOnlyList = powers;
			}
			return readOnlyList;
		}

		/// <summary>
		/// Apply a power to a creature.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature to apply the power to.</param>
		/// <param name="amount">Amount of the power to apply.</param>
		/// <param name="applier">Creature applying the power.</param>
		/// <param name="cardSource">Optional card that is applying the power.</param>
		/// <param name="silent">Pass true if no flashes should play for this power application.</param>
		/// <typeparam name="T">Type of power to apply.</typeparam>
		/// <returns>
		/// The power that was applied.
		/// Can be null if:
		/// * Combat is over.
		/// * Something blocked application (like Artifact).
		/// * The power's amount was changed to 0 (like if you had 3 Strength and applied -3 Strength).
		/// </returns>
		// Token: 0x06008860 RID: 34912 RVA: 0x002DEE28 File Offset: 0x002DD028
		[return: Nullable(new byte[] { 1, 2 })]
		public static async Task<T> Apply<[Nullable(0)] T>(PlayerChoiceContext choiceContext, Creature target, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource, bool silent = false) where T : PowerModel
		{
			T t;
			if (CombatManager.Instance.IsEnding)
			{
				t = default(T);
			}
			else if (!target.CanReceivePowers)
			{
				t = default(T);
			}
			else
			{
				PowerModel powerModel = ModelDb.Power<T>();
				PowerModel power = PowerCmd.FindExistingInstanceForStacking(powerModel, target, applier);
				if (power == null)
				{
					power = powerModel.ToMutable(0);
					await PowerCmd.Apply(choiceContext, power, target, amount, applier, cardSource, silent);
				}
				else
				{
					TaskAwaiter<int> taskAwaiter = PowerCmd.ModifyAmount(choiceContext, power, amount, applier, cardSource, silent).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<int> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<int>);
					}
					if (taskAwaiter.GetResult() == 0)
					{
						power = null;
					}
				}
				t = power as T;
			}
			return t;
		}

		/// <summary>
		/// Apply a power to a creature.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="power">Power to apply.</param>
		/// <param name="target">Creature to apply the power to.</param>
		/// <param name="amount">Amount of the power to apply.</param>
		/// <param name="applier">Creature applying the power.</param>
		/// <param name="cardSource">Optional card that is applying the power.</param>
		/// <param name="silent">Pass true if no flashes should play for this power application.</param>
		// Token: 0x06008861 RID: 34913 RVA: 0x002DEE98 File Offset: 0x002DD098
		public static async Task Apply(PlayerChoiceContext choiceContext, PowerModel power, Creature target, decimal amount, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource, bool silent = false)
		{
			if (!CombatManager.Instance.IsEnding)
			{
				if (!(amount == 0m))
				{
					if (target.CanReceivePowers)
					{
						ICombatState combatState = target.CombatState;
						if (combatState != null)
						{
							PowerModel powerModel = PowerCmd.FindExistingInstanceForStacking(power, target, applier);
							if (powerModel != null)
							{
								await PowerCmd.ModifyAmount(choiceContext, powerModel, amount, applier, cardSource, false);
							}
							else
							{
								power.AssertMutable();
								power.Applier = applier;
								await Hook.BeforePowerAmountChanged(combatState, power, amount, target, applier, cardSource);
								decimal modifiedAmount = amount;
								IEnumerable<AbstractModel> givenModifiers = null;
								if (applier != null && combatState.ContainsCreature(applier))
								{
									modifiedAmount = Hook.ModifyPowerAmountGiven(combatState, power, applier, modifiedAmount, target, cardSource, out givenModifiers);
								}
								IEnumerable<AbstractModel> receivedModifiers;
								modifiedAmount = Hook.ModifyPowerAmountReceived(combatState, power, target, modifiedAmount, applier, out receivedModifiers);
								if (combatState.Players.Count > 1 && (target.IsPrimaryEnemy || target.IsSecondaryEnemy) && power.ShouldScaleInMultiplayer)
								{
									modifiedAmount = power.GetScaledAmountForMultiplayer(combatState, applier, modifiedAmount, target, cardSource);
								}
								await power.BeforeApplied(target, modifiedAmount, applier, cardSource);
								if (target.CanReceivePowers)
								{
									power.ApplyInternal(target, modifiedAmount, silent);
									if (modifiedAmount != 0m)
									{
										CombatManager.Instance.History.PowerReceived(combatState, power, modifiedAmount, applier);
									}
									if (power.IsVisible && CombatManager.Instance.IsInProgress)
									{
										await Cmd.CustomScaledWait(0.1f, 0.25f, false, default(CancellationToken));
									}
									if (target.Side == CombatSide.Player && power.Type == PowerType.Debuff)
									{
										power.SkipNextDurationTick = true;
									}
									if (givenModifiers != null)
									{
										await Hook.AfterModifyingPowerAmountGiven(combatState, givenModifiers, power);
									}
									await Hook.AfterModifyingPowerAmountReceived(combatState, receivedModifiers, power);
									if (modifiedAmount != 0m)
									{
										await power.AfterApplied(applier, cardSource);
										await Hook.AfterPowerAmountChanged(combatState, choiceContext, power, modifiedAmount, applier, cardSource);
									}
									givenModifiers = null;
									receivedModifiers = null;
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Looks for a valid existing instance for a given power within the target
		///  for the sake of adjusting the amount on them.
		/// </summary>
		// Token: 0x06008862 RID: 34914 RVA: 0x002DEF10 File Offset: 0x002DD110
		[return: Nullable(2)]
		public static PowerModel FindExistingInstanceForStacking(PowerModel basePower, Creature target, [Nullable(2)] Creature applier)
		{
			PowerModel powerModel;
			switch (basePower.InstanceType)
			{
			case PowerInstanceType.None:
				powerModel = target.GetPower(basePower.Id);
				break;
			case PowerInstanceType.Instanced:
				powerModel = null;
				break;
			case PowerInstanceType.InstancedPerApplier:
				powerModel = target.GetPowerInstances(basePower.Id).FirstOrDefault<PowerModel>((PowerModel p) => p.Applier == applier);
				break;
			default:
				throw new ArgumentOutOfRangeException("InstanceType");
			}
			return powerModel;
		}

		/// <summary>
		/// Decrement the amount of a power by 1.
		/// </summary>
		/// <param name="power">Power to decrement.</param>
		// Token: 0x06008863 RID: 34915 RVA: 0x002DEF84 File Offset: 0x002DD184
		public static async Task Decrement(PowerModel power)
		{
			await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), power, -1m, null, null, false);
		}

		/// <summary>
		/// Tick down the amount of a duration-based power (Vulnerable, Weak, etc.).
		/// Similar to Decrement, but respects some extra rules about turn ordering.
		/// </summary>
		/// <param name="power">Power to tick down.</param>
		// Token: 0x06008864 RID: 34916 RVA: 0x002DEFC8 File Offset: 0x002DD1C8
		public static async Task TickDownDuration(PowerModel power)
		{
			if (power.SkipNextDurationTick)
			{
				power.SkipNextDurationTick = false;
			}
			else
			{
				await PowerCmd.Decrement(power);
			}
		}

		/// <summary>
		/// Modify the amount of a power.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="power">Power to modify.</param>
		/// <param name="offset">Amount to modify it by (can be positive or negative).</param>
		/// <param name="applier">Creature applying the power.</param>
		/// <param name="cardSource">Optional card that is modifying the power amount.</param>
		/// <param name="silent">Pass true if no flashes should play for this power application.</param>
		/// <returns>
		/// The new amount of the power.
		/// For example, if you have 1 Strength and modify it by 3, this will return 4.
		/// </returns>
		// Token: 0x06008865 RID: 34917 RVA: 0x002DF00C File Offset: 0x002DD20C
		public static async Task<int> ModifyAmount(PlayerChoiceContext choiceContext, PowerModel power, decimal offset, [Nullable(2)] Creature applier, [Nullable(2)] CardModel cardSource, bool silent = false)
		{
			int num;
			if (CombatManager.Instance.IsEnding)
			{
				num = 0;
			}
			else
			{
				Creature owner = power.Owner;
				ICombatState combatState = owner.CombatState;
				if (combatState == null)
				{
					num = 0;
				}
				else
				{
					await Hook.BeforePowerAmountChanged(combatState, power, offset, owner, applier, cardSource);
					decimal modifiedOffset = offset;
					IEnumerable<AbstractModel> enumerable = null;
					if (applier != null && combatState.ContainsCreature(applier))
					{
						modifiedOffset = Hook.ModifyPowerAmountGiven(combatState, power, applier, modifiedOffset, owner, cardSource, out enumerable);
					}
					IEnumerable<AbstractModel> receivedModifiers;
					modifiedOffset = Hook.ModifyPowerAmountReceived(combatState, power, owner, modifiedOffset, applier, out receivedModifiers);
					CombatManager.Instance.History.PowerReceived(combatState, power, modifiedOffset, applier);
					int newAmount = power.Amount + (int)modifiedOffset;
					power.SetAmount(newAmount, silent);
					if (enumerable != null)
					{
						await Hook.AfterModifyingPowerAmountGiven(combatState, enumerable, power);
					}
					await Hook.AfterModifyingPowerAmountReceived(combatState, receivedModifiers, power);
					if ((int)modifiedOffset != 0)
					{
						await Hook.AfterPowerAmountChanged(combatState, choiceContext, power, modifiedOffset, applier, cardSource);
					}
					if (power.ShouldRemoveDueToAmount())
					{
						await PowerCmd.Remove(power);
					}
					if (CombatManager.Instance.IsInProgress && owner != null && owner.IsMonster && owner.IsAlive)
					{
						NCombatRoom instance = NCombatRoom.Instance;
						NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(owner) : null);
						if (ncreature != null)
						{
							try
							{
								await ncreature.UpdateIntent(combatState.Allies);
							}
							catch (ObjectDisposedException ex)
							{
								Log.Error(ex.ToString(), 2);
							}
						}
					}
					if (power.IsVisible && CombatManager.Instance.IsInProgress)
					{
						await Cmd.CustomScaledWait(0.1f, 0.25f, false, default(CancellationToken));
					}
					num = newAmount;
				}
			}
			return num;
		}

		/// <summary>
		/// Remove a power.
		/// </summary>
		/// <param name="creature">Creature to remove the power from.</param>
		/// <typeparam name="T">Type of power to remove.</typeparam>
		// Token: 0x06008866 RID: 34918 RVA: 0x002DF07C File Offset: 0x002DD27C
		public static async Task Remove<[Nullable(0)] T>(Creature creature) where T : PowerModel
		{
			await PowerCmd.Remove(creature.GetPower<T>());
		}

		/// <summary>
		/// Remove a power.
		/// </summary>
		/// <param name="power">Power to remove.</param>
		// Token: 0x06008867 RID: 34919 RVA: 0x002DF0C0 File Offset: 0x002DD2C0
		public static async Task Remove([Nullable(2)] PowerModel power)
		{
			if (power != null)
			{
				power.RemoveInternal();
				await Cmd.CustomScaledWait(0.2f, 0.4f, false, default(CancellationToken));
				await power.AfterRemoved(power.Owner);
			}
		}
	}
}
