using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D2D RID: 3373
	[NullableContext(1)]
	[Nullable(0)]
	public static class CreatureCmd
	{
		// Token: 0x0600881E RID: 34846 RVA: 0x002DD78C File Offset: 0x002DB98C
		public static async Task<Creature> Add<[Nullable(0)] T>(ICombatState combatState, [Nullable(2)] string slotName = null) where T : MonsterModel
		{
			Creature creature = combatState.CreateCreature(ModelDb.Monster<T>().ToMutable(), CombatSide.Enemy, slotName);
			await CreatureCmd.Add(creature);
			return creature;
		}

		// Token: 0x0600881F RID: 34847 RVA: 0x002DD7D8 File Offset: 0x002DB9D8
		public static async Task<Creature> Add(MonsterModel monster, ICombatState combatState, CombatSide side = CombatSide.Enemy, [Nullable(2)] string slotName = null)
		{
			monster.AssertMutable();
			Creature creature = combatState.CreateCreature(monster, side, slotName);
			await CreatureCmd.Add(creature);
			return creature;
		}

		// Token: 0x06008820 RID: 34848 RVA: 0x002DD834 File Offset: 0x002DBA34
		public static async Task Add(Creature creature)
		{
			if (!CombatManager.Instance.IsInProgress)
			{
				throw new InvalidOperationException("Attempted to add a creature outside of combat.");
			}
			ICombatState combatState = creature.CombatState;
			if (combatState == null)
			{
				throw new InvalidOperationException("Attempted to add a creature with no combat state.");
			}
			if (combatState.IsLiveCombat())
			{
				combatState.AddCreature(creature);
				CombatManager.Instance.AddCreature(creature);
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.AddCreature(creature);
				}
				await CombatManager.Instance.AfterCreatureAdded(creature);
				if (combatState.CurrentSide != CombatSide.Enemy && creature.IsMonster)
				{
					creature.PrepareForNextTurn(combatState.Players.Select<Player, Creature>((Player p) => p.Creature), false);
				}
				MapPointHistoryEntry currentMapPointHistoryEntry = combatState.RunState.CurrentMapPointHistoryEntry;
				MapPointRoomHistoryEntry mapPointRoomHistoryEntry = ((currentMapPointHistoryEntry != null) ? currentMapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>() : null);
				if (mapPointRoomHistoryEntry != null && creature != null && creature.Monster != null && creature.Side == CombatSide.Enemy && !mapPointRoomHistoryEntry.MonsterIds.Contains(creature.Monster.Id))
				{
					mapPointRoomHistoryEntry.MonsterIds.Add(creature.Monster.Id);
				}
				await Hook.AfterCreatureAddedToCombat(creature.CombatState, creature);
			}
		}

		/// <summary>
		/// Damage a creature.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature to damage.</param>
		/// <param name="damageVar">
		/// DamageVar containing the amount of damage they are taking and the properties of the damage.
		/// </param>
		/// <param name="cardSource">Card that dealt the damage. Optional.</param>
		/// <param name="cardPlay">The object which represents a card play, or null if this damage is not part of card play.
		/// Note that cardSource is set both when previewing AND when the card is played, but this is only set when the card
		/// is played.</param>
		/// <returns>
		/// A set of damage results. Can be multiple if another creature absorbs some damage (like Osty via DieForYou).
		/// </returns>
		// Token: 0x06008821 RID: 34849 RVA: 0x002DD878 File Offset: 0x002DBA78
		public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target, DamageVar damageVar, CardModel cardSource, [Nullable(2)] CardPlay cardPlay)
		{
			return await CreatureCmd.Damage(choiceContext, target, damageVar.BaseValue, damageVar.Props, cardSource, cardPlay);
		}

		/// <summary>
		/// Damage a creature.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature to damage.</param>
		/// <param name="amount">Amount of damage it is taking.</param>
		/// <param name="props">Properties of the damage.</param>
		/// <param name="cardSource">Card that dealt the damage. Optional.</param>
		/// <param name="cardPlay">The object which represents a card play, or null if this damage is not part of card play.
		/// Note that cardSource is set both when previewing AND when the card is played, but this is only set when the card
		/// is played.</param>
		/// <returns>
		/// A set of damage results. Can be multiple if another creature absorbs some damage (like Osty via DieForYou).
		/// </returns>
		// Token: 0x06008822 RID: 34850 RVA: 0x002DD8DC File Offset: 0x002DBADC
		public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, [Nullable(2)] CardModel cardSource, [Nullable(2)] CardPlay cardPlay)
		{
			return await CreatureCmd.Damage(choiceContext, new List<Creature> { target }, amount, props, (cardSource != null) ? cardSource.Owner.Creature : null, cardSource, cardPlay);
		}

		/// <summary>
		/// Damage a set of creatures.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="targets">Creatures to damage.</param>
		/// <param name="damageVar">
		/// DamageVar containing the amount of damage they are taking and the properties of the damage.
		/// </param>
		/// <param name="dealer">Creature who dealt the damage.</param>
		/// <returns>A set of damage results.</returns>
		// Token: 0x06008823 RID: 34851 RVA: 0x002DD94C File Offset: 0x002DBB4C
		public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, IEnumerable<Creature> targets, DamageVar damageVar, Creature dealer)
		{
			return await CreatureCmd.Damage(choiceContext, targets, damageVar.BaseValue, damageVar.Props, dealer);
		}

		/// <summary>
		/// Damage a set of creatures.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="targets">Creatures to damage.</param>
		/// <param name="amount">Amount of damage they are taking.</param>
		/// <param name="props">Properties of the damage.</param>
		/// <param name="dealer">Creature who dealt the damage.</param>
		/// <returns>A set of damage results.</returns>
		// Token: 0x06008824 RID: 34852 RVA: 0x002DD9A8 File Offset: 0x002DBBA8
		public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, IEnumerable<Creature> targets, decimal amount, ValueProp props, Creature dealer)
		{
			return await CreatureCmd.Damage(choiceContext, targets, amount, props, dealer, null, null);
		}

		/// <summary>
		/// Damage a creature.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature to damage.</param>
		/// <param name="damageVar">
		/// DamageVar containing the amount of damage they are taking and the properties of the damage.
		/// </param>
		/// <param name="dealer">Creature who dealt the damage. Optional.</param>
		/// <returns>
		/// A set of damage results. Can be multiple if another creature absorbs some damage (like Osty via DieForYou).
		/// </returns>
		// Token: 0x06008825 RID: 34853 RVA: 0x002DDA0C File Offset: 0x002DBC0C
		public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target, DamageVar damageVar, Creature dealer)
		{
			return await CreatureCmd.Damage(choiceContext, target, damageVar.BaseValue, damageVar.Props, dealer);
		}

		/// <summary>
		/// Damage a creature.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature to damage.</param>
		/// <param name="amount">Amount of damage it is taking.</param>
		/// <param name="props">Properties of the damage.</param>
		/// <param name="dealer">Creature who dealt the damage. Optional.</param>
		/// <returns>
		/// A set of damage results. Can be multiple if another creature absorbs some damage (like Osty via DieForYou).
		/// </returns>
		// Token: 0x06008826 RID: 34854 RVA: 0x002DDA68 File Offset: 0x002DBC68
		public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature dealer)
		{
			return await CreatureCmd.Damage(choiceContext, new <>z__ReadOnlySingleElementList<Creature>(target), amount, props, dealer, null, null);
		}

		/// <summary>
		/// Damage a creature.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature to damage.</param>
		/// <param name="damageVar">
		/// DamageVar containing the amount of damage they are taking and the properties of the damage.
		/// </param>
		/// <param name="dealer">Creature who dealt the damage. Optional.</param>
		/// <param name="cardSource">Card that dealt the damage. Optional.</param>
		/// <param name="cardPlay">The object which represents a card play, or null if this damage is not part of card play.
		/// Note that cardSource is set both when previewing AND when the card is played, but this is only set when the card
		/// is played.</param>
		/// <returns>
		/// A set of damage results. Can be multiple if another creature absorbs some damage (like Osty via DieForYou).
		/// </returns>
		// Token: 0x06008827 RID: 34855 RVA: 0x002DDACC File Offset: 0x002DBCCC
		public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target, DamageVar damageVar, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource, [Nullable(2)] CardPlay cardPlay)
		{
			return await CreatureCmd.Damage(choiceContext, new <>z__ReadOnlySingleElementList<Creature>(target), damageVar.BaseValue, damageVar.Props, dealer, cardSource, cardPlay);
		}

		/// <summary>
		/// Damage a creature.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature to damage.</param>
		/// <param name="amount">Amount of damage it is taking.</param>
		/// <param name="props">Properties of the damage.</param>
		/// <param name="dealer">Creature who dealt the damage. Optional.</param>
		/// <param name="cardSource">Card that dealt the damage. Optional.</param>
		/// <param name="cardPlay">The object which represents a card play, or null if this damage is not part of card play.
		/// Note that cardSource is set both when previewing AND when the card is played, but this is only set when the card
		/// is played.</param>
		/// <returns>
		/// A set of damage results. Can be multiple if another creature absorbs some damage (like Osty via DieForYou).
		/// </returns>
		// Token: 0x06008828 RID: 34856 RVA: 0x002DDB3C File Offset: 0x002DBD3C
		public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource, [Nullable(2)] CardPlay cardPlay)
		{
			return await CreatureCmd.Damage(choiceContext, new <>z__ReadOnlySingleElementList<Creature>(target), amount, props, dealer, cardSource, cardPlay);
		}

		/// <summary>
		/// Damage a set of creatures.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="targets">Creatures to damage.</param>
		/// <param name="damageVar">
		/// DamageVar containing the amount of damage they are taking and the properties of the damage.
		/// </param>
		/// <param name="dealer">Creature who dealt the damage. Optional.</param>
		/// <param name="cardSource">Card that dealt the damage. Optional.</param>
		/// <param name="cardPlay">The object which represents a card play, or null if this damage is not part of card play.
		/// Note that cardSource is set both when previewing AND when the card is played, but this is only set when the card
		/// is played.</param>
		/// <returns>A set of damage results.</returns>
		// Token: 0x06008829 RID: 34857 RVA: 0x002DDBB4 File Offset: 0x002DBDB4
		public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, [Nullable(new byte[] { 2, 1 })] IEnumerable<Creature> targets, DamageVar damageVar, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource, [Nullable(2)] CardPlay cardPlay)
		{
			return await CreatureCmd.Damage(choiceContext, targets, damageVar.BaseValue, damageVar.Props, dealer, cardSource, cardPlay);
		}

		/// <summary>
		/// Damage a set of creatures.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="targets">Creatures to damage.</param>
		/// <param name="amount">Amount of damage they are taking.</param>
		/// <param name="props">Properties of the damage.</param>
		/// <param name="dealer">Creature who dealt the damage. Optional.</param>
		/// <param name="cardSource">Card that dealt the damage. Optional.</param>
		/// <param name="cardPlay">The object which represents a card play, or null if this damage is not part of card play.
		/// Note that cardSource is set both when previewing AND when the card is played, but this is only set when the card
		/// is played.</param>
		/// <returns>A set of damage results.</returns>
		// Token: 0x0600882A RID: 34858 RVA: 0x002DDC24 File Offset: 0x002DBE24
		[NullableContext(2)]
		[return: Nullable(1)]
		public static async Task<IEnumerable<DamageResult>> Damage([Nullable(1)] PlayerChoiceContext choiceContext, [Nullable(new byte[] { 2, 1 })] IEnumerable<Creature> targets, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			IEnumerable<DamageResult> enumerable;
			if (targets == null)
			{
				enumerable = Array.Empty<DamageResult>();
			}
			else if (dealer != null && dealer.IsDead)
			{
				enumerable = targets.Select<Creature, DamageResult>((Creature t) => new DamageResult(t, props)).ToList<DamageResult>();
			}
			else
			{
				List<DamageResult> results = new List<DamageResult>();
				List<Creature> targetList = targets.ToList<Creature>();
				if (targetList.Count == 0)
				{
					enumerable = results;
				}
				else
				{
					ICombatState combatState = targetList[0].CombatState;
					IRunState runState = IRunState.GetFrom(targetList.Append(dealer).OfType<Creature>());
					List<DamageResult>.Enumerator enumerator2;
					foreach (Creature originalTarget in targetList)
					{
						Creature originalTarget;
						if (!originalTarget.IsDead)
						{
							IEnumerable<AbstractModel> enumerable2;
							decimal modifiedAmount = Hook.ModifyDamage(runState, combatState, originalTarget, dealer, amount, props, cardSource, cardPlay, ModifyDamageHookType.All, CardPreviewMode.None, out enumerable2);
							await Hook.AfterModifyingDamageAmount(runState, combatState, cardSource, enumerable2);
							await Hook.BeforeDamageReceived(choiceContext, runState, combatState, originalTarget, modifiedAmount, props, dealer, cardSource);
							Player petOwner = originalTarget.PetOwner;
							Creature creature = ((petOwner != null) ? petOwner.Creature : null) ?? originalTarget;
							decimal blockedDamage = creature.DamageBlockInternal(modifiedAmount, props);
							decimal unblockedDamage = Hook.ModifyHpLost(runState, combatState, originalTarget, Math.Max(modifiedAmount - blockedDamage, 0m), props, dealer, cardSource, HpLossHookPhase.BeforeOsty, out enumerable2);
							await Hook.AfterModifyingHpLostBeforeOsty(runState, combatState, enumerable2);
							Creature unblockedDamageTarget;
							if (combatState != null)
							{
								unblockedDamageTarget = Hook.ModifyUnblockedDamageTarget(combatState, originalTarget, unblockedDamage, props, dealer);
							}
							else
							{
								unblockedDamageTarget = originalTarget;
							}
							unblockedDamage = Hook.ModifyHpLost(runState, combatState, unblockedDamageTarget, unblockedDamage, props, dealer, cardSource, HpLossHookPhase.AfterOsty, out enumerable2);
							await Hook.AfterModifyingHpLostAfterOsty(runState, combatState, enumerable2);
							DamageResult unblockedDamageResult = unblockedDamageTarget.LoseHpInternal(unblockedDamage, props);
							List<DamageResult> damageResults = new List<DamageResult>(1) { unblockedDamageResult };
							bool wasBlockBroken = originalTarget.Block <= 0 && blockedDamage > 0m;
							bool wasFullyBlocked = !props.HasFlag(ValueProp.Unblockable) && (blockedDamage > 0m || originalTarget.Block > 0) && (int)unblockedDamage == 0;
							if (originalTarget == unblockedDamageTarget)
							{
								unblockedDamageResult.BlockedDamage = (int)blockedDamage;
								unblockedDamageResult.WasBlockBroken = wasBlockBroken;
								unblockedDamageResult.WasFullyBlocked = wasFullyBlocked;
							}
							else
							{
								decimal originalTargetDamage = Hook.ModifyHpLost(runState, combatState, originalTarget, unblockedDamageResult.OverkillDamage, props, dealer, cardSource, HpLossHookPhase.AfterOsty, out enumerable2);
								await Hook.AfterModifyingHpLostAfterOsty(runState, combatState, enumerable2);
								DamageResult damageResult = ((!(originalTargetDamage > 0m)) ? new DamageResult(originalTarget, props) : originalTarget.LoseHpInternal(originalTargetDamage, props));
								damageResult.BlockedDamage = (int)blockedDamage;
								damageResult.WasBlockBroken = wasBlockBroken;
								damageResult.WasFullyBlocked = wasFullyBlocked;
								damageResults.Add(damageResult);
							}
							List<Task> hitTriggers = new List<Task>();
							foreach (DamageResult damageResult2 in damageResults)
							{
								int damage = damageResult2.UnblockedDamage + damageResult2.OverkillDamage;
								Creature receiver = damageResult2.Receiver;
								if (CombatManager.Instance.IsInProgress && !CombatManager.Instance.IsEnding)
								{
									CombatManager.Instance.History.DamageReceived(combatState, receiver, dealer, damageResult2, cardSource);
								}
								if (!damageResult2.WasFullyBlocked)
								{
									Node vfxContainer = receiver.GetVfxContainer();
									if (damage > 0 || (modifiedAmount == 0m && damageResult2.Receiver == unblockedDamageTarget))
									{
										NDamageNumVfx ndamageNumVfx = NDamageNumVfx.Create(receiver, damageResult2);
										if (ndamageNumVfx != null)
										{
											if (vfxContainer != null)
											{
												vfxContainer.AddChildSafely(ndamageNumVfx);
											}
											else
											{
												NRun.Instance.GlobalUi.AddChildSafely(ndamageNumVfx);
											}
										}
									}
									if (damage > 0)
									{
										if (vfxContainer != null)
										{
											vfxContainer.AddChildSafely(NHitSparkVfx.Create(receiver, true));
										}
										if (receiver != dealer && !props.HasFlag(ValueProp.SkipHurtAnim))
										{
											hitTriggers.Add(CreatureCmd.TriggerAnim(receiver, "Hit", 0f));
											if (receiver.IsMonster && receiver.Monster.HasHurtSfx)
											{
												SfxCmd.Play(receiver.Monster.HurtSfx, 1f);
											}
										}
										Player player = receiver.Player;
										MapPointHistoryEntry mapPointHistoryEntry = ((player != null) ? player.RunState.CurrentMapPointHistoryEntry : null);
										if (mapPointHistoryEntry != null)
										{
											mapPointHistoryEntry.GetEntry(receiver.Player.NetId).DamageTaken += damageResult2.UnblockedDamage;
										}
									}
									await Task.WhenAll(hitTriggers);
									if (damage > 0)
									{
										if (damageResults.Any<DamageResult>((DamageResult r) => r.WasBlockBroken))
										{
											SfxCmd.Play("event:/sfx/block_break", 1f);
										}
										if (LocalContext.IsMe(originalTarget) && (!CombatManager.Instance.IsInProgress || originalTarget.GetHpPercentRemaining() <= 0.25))
										{
											PlayerHurtVignetteHelper.Play();
										}
										if (originalTarget.Side == CombatSide.Enemy)
										{
											SfxCmd.PlayDamage(originalTarget.Monster, unblockedDamageResult.UnblockedDamage);
										}
										if (CombatManager.Instance.IsInProgress || LocalContext.ContainsMe(targetList))
										{
											if (unblockedDamageResult.UnblockedDamage < 6)
											{
												NGame instance = NGame.Instance;
												if (instance != null)
												{
													instance.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, -1f);
												}
											}
											else if (unblockedDamageResult.UnblockedDamage < 11)
											{
												NGame instance2 = NGame.Instance;
												if (instance2 != null)
												{
													instance2.ScreenShake(ShakeStrength.Medium, ShakeDuration.Short, -1f);
												}
											}
											else
											{
												NGame instance3 = NGame.Instance;
												if (instance3 != null)
												{
													instance3.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short, -1f);
												}
											}
										}
									}
								}
							}
							enumerator2 = default(List<DamageResult>.Enumerator);
							results.AddRange(damageResults);
							unblockedDamageTarget = null;
							unblockedDamageResult = null;
							damageResults = null;
							hitTriggers = null;
							originalTarget = null;
						}
					}
					List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
					List<Creature> killedCreatures = new List<Creature>();
					foreach (DamageResult unblockedDamageResult in results)
					{
						DamageResult unblockedDamageResult;
						Creature originalTarget = unblockedDamageResult.Receiver;
						if (unblockedDamageResult.WasBlockBroken)
						{
							await Hook.AfterBlockBroken(originalTarget.CombatState, choiceContext, originalTarget, dealer);
						}
						if (unblockedDamageResult.UnblockedDamage > 0)
						{
							await Hook.AfterCurrentHpChanged(runState, combatState, originalTarget, -unblockedDamageResult.UnblockedDamage);
						}
						if (dealer != null && dealer.Player != null && originalTarget.Player == null)
						{
							dealer.Player.ExtraFields.DamageDealt += unblockedDamageResult.UnblockedDamage;
						}
						if (combatState != null)
						{
							await Hook.AfterDamageGiven(choiceContext, combatState, dealer, unblockedDamageResult, props, originalTarget, cardSource);
						}
						if (unblockedDamageResult.WasTargetKilled && originalTarget.IsDead)
						{
							killedCreatures.Add(originalTarget);
						}
						else
						{
							await Hook.AfterDamageReceived(choiceContext, runState, combatState, originalTarget, unblockedDamageResult, props, dealer, cardSource);
						}
						if (unblockedDamageResult.WasFullyBlocked && CombatManager.Instance.IsInProgress)
						{
							SfxCmd.Play("event:/sfx/block_hit", 1f);
							Node vfxContainer2 = unblockedDamageResult.Receiver.GetVfxContainer();
							if (vfxContainer2 != null)
							{
								vfxContainer2.AddChildSafely(NBlockSparkVfx.Create(unblockedDamageResult.Receiver));
							}
							if (vfxContainer2 != null)
							{
								vfxContainer2.AddChildSafely(NDamageBlockedVfx.Create(unblockedDamageResult.Receiver));
							}
							NGame instance4 = NGame.Instance;
							if (instance4 != null)
							{
								instance4.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, -1f);
							}
						}
						originalTarget = null;
						unblockedDamageResult = null;
					}
					enumerator2 = default(List<DamageResult>.Enumerator);
					await CreatureCmd.Kill(killedCreatures, false);
					await Cmd.CustomScaledWait(0.1f, 0.2f, false, default(CancellationToken));
					enumerable = results;
				}
			}
			return enumerable;
		}

		/// <summary>
		/// Kill the specified creature.
		/// NOTE: ALL creatures on the opposing side are considered the killer of this creature, no
		/// matter who struck the killing blow.
		/// </summary>
		/// <param name="creature">Creature to kill.</param>
		/// <param name="force">
		/// Whether or not to force the death (blocking death prevention by effects like Fairy in a Bottle).
		/// Usually false, but true in certain built-in cases like abandoning a run.
		/// </param>
		// Token: 0x0600882B RID: 34859 RVA: 0x002DDC9C File Offset: 0x002DBE9C
		public static async Task Kill(Creature creature, bool force = false)
		{
			await CreatureCmd.Kill(new <>z__ReadOnlySingleElementList<Creature>(creature), force);
		}

		/// <summary>
		/// Kill the specified creatures.
		/// NOTE: ALL creatures on the opposing side are considered the killer of these creatures,
		/// no matter who struck the killing blow.
		/// </summary>
		/// <param name="creatures">Creatures to kill.</param>
		/// <param name="force">
		/// Whether or not to force the death (blocking death prevention by effects like Fairy in a Bottle).
		/// Usually false, but true in certain built-in cases like abandoning a run.
		/// </param>
		// Token: 0x0600882C RID: 34860 RVA: 0x002DDCE8 File Offset: 0x002DBEE8
		public static async Task Kill(IReadOnlyCollection<Creature> creatures, bool force = false)
		{
			if (creatures.Count != 0)
			{
				Creature creature = creatures.FirstOrDefault<Creature>((Creature c) => c.IsPlayer);
				IRunState runState2;
				if (creature == null)
				{
					runState2 = null;
				}
				else
				{
					Player player = creature.Player;
					runState2 = ((player != null) ? player.RunState : null);
				}
				IRunState runState = runState2;
				foreach (Creature creature2 in creatures.ToList<Creature>())
				{
					await CreatureCmd.KillWithoutCheckingWinCondition(creature2, force, 0);
				}
				List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
				if (RunManager.Instance.IsInProgress && !RunManager.Instance.IsCleaningUp)
				{
					if (runState != null)
					{
						if (runState.Players.All<Player>((Player p) => p.Creature.IsDead))
						{
							if (CombatManager.Instance.IsInProgress)
							{
								CombatManager.Instance.LoseCombat();
							}
							if (TestMode.IsOff)
							{
								NRun.Instance.RunMusicController.StopMusic();
								NDebugAudioManager instance = NDebugAudioManager.Instance;
								if (instance != null)
								{
									instance.StopAll();
								}
								NAudioManager.Instance.PlayMusic("event:/temp/sfx/game_over");
								SerializableRun serializableRun = RunManager.Instance.OnEnded(false);
								NRun.Instance.ShowGameOverScreen(serializableRun);
								goto IL_0263;
							}
							goto IL_0263;
						}
					}
					if (CombatManager.Instance.IsInProgress)
					{
						foreach (Creature creature3 in creatures.ToList<Creature>())
						{
							if (creature3 != null)
							{
								ICombatState combatState = creature3.CombatState;
								if (combatState != null && combatState.CurrentSide == CombatSide.Player && creature3.IsDead && creature3.IsPlayer)
								{
									PlayerCmd.EndTurn(creature3.Player, false, null);
								}
							}
						}
					}
					IL_0263:;
				}
			}
		}

		/// <summary>
		/// Kill the specified creature without checking win condition. For internal use only, when we know we will
		/// be checking the win condition later.
		/// </summary>
		// Token: 0x0600882D RID: 34861 RVA: 0x002DDD34 File Offset: 0x002DBF34
		private static async Task KillWithoutCheckingWinCondition(Creature creature, bool force, int recursion = 0)
		{
			if (creature.CombatState != null || creature.IsPlayer)
			{
				if (creature.CombatState == null || creature.CombatState.IsLiveCombat())
				{
					if (!CombatManager.Instance.IsInProgress && creature.IsPlayer && creature.Player.RunState.Players.Count > 1)
					{
						AbstractRoom currentRoom = creature.Player.RunState.CurrentRoom;
						if (currentRoom == null || !currentRoom.IsVictoryRoom)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(79, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Player ");
							defaultInterpolatedStringHandler.AppendFormatted<ulong>(creature.Player.NetId);
							defaultInterpolatedStringHandler.AppendLiteral(" has been killed outside of combat in multiplayer! This should not occur");
							Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							await CreatureCmd.Heal(creature, 1m, true);
							return;
						}
					}
					ICombatState combatState = creature.CombatState;
					IRunState runState = IRunState.GetFrom(new <>z__ReadOnlySingleElementList<Creature>(creature));
					int currentHp = creature.CurrentHp;
					if (currentHp > 0)
					{
						creature.LoseHpInternal(currentHp, ValueProp.Unblockable | ValueProp.Unpowered);
						await Hook.AfterCurrentHpChanged(runState, creature.CombatState, creature, -currentHp);
					}
					await Hook.BeforeDeath(runState, combatState, creature);
					AbstractModel preventer = null;
					bool flag = force || creature.MaxHp <= 0 || Hook.ShouldDie(runState, combatState, creature, out preventer);
					if (flag)
					{
						creature.InvokeDiedEvent();
						bool shouldRemoveFromCombat = combatState != null && Hook.ShouldCreatureBeRemovedFromCombatAfterDeath(combatState, creature);
						float num = 0f;
						NCombatRoom instance = NCombatRoom.Instance;
						NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
						if (ncreature != null)
						{
							num = ncreature.StartDeathAnim(shouldRemoveFromCombat && creature.IsMonster);
							if (shouldRemoveFromCombat && creature.IsMonster)
							{
								NCombatRoom instance2 = NCombatRoom.Instance;
								if (instance2 != null)
								{
									instance2.RemoveCreatureNode(ncreature);
								}
							}
						}
						await Hook.AfterDeath(runState, combatState, creature, false, num);
						ICombatState combatState2 = combatState;
						List<Creature> list;
						if (combatState2 == null)
						{
							list = null;
						}
						else
						{
							list = (from t in combatState2.GetTeammatesOf(creature)
								where t.IsAlive
								select t).ToList<Creature>();
						}
						List<Creature> teammates = list ?? new List<Creature>();
						if (shouldRemoveFromCombat && creature.Side == CombatSide.Enemy)
						{
							ICombatState combatState3 = combatState;
							if (combatState3 != null && combatState3.Enemies.Contains(creature))
							{
								CombatManager.Instance.RemoveCreature(creature);
								MonsterModel monster = creature.Monster;
								if (monster != null && !monster.IsPerformingMove)
								{
									combatState.RemoveCreature(creature, true);
								}
							}
						}
						bool isPrimaryEnemy = creature.IsPrimaryEnemy;
						foreach (PowerModel powerModel in creature.RemoveAllPowersAfterDeath())
						{
							await powerModel.AfterRemoved(creature);
						}
						IEnumerator<PowerModel> enumerator = null;
						if (creature.Side == CombatSide.Enemy)
						{
							if (isPrimaryEnemy && teammates.Count != 0)
							{
								if (teammates.All<Creature>((Creature t) => t.IsSecondaryEnemy))
								{
									await CreatureCmd.Kill(teammates, false);
								}
							}
						}
						else if (creature.IsPlayer)
						{
							Player player = creature.Player;
							PlayerCombatState playerCombatState = player.PlayerCombatState;
							if (playerCombatState != null)
							{
								playerCombatState.OrbQueue.Clear();
							}
							if (player.IsOstyAlive)
							{
								await CreatureCmd.Kill(player.Osty, force);
							}
							player.DeactivateHooks();
							if (combatState != null)
							{
								if (!combatState.Players.All<Player>((Player p) => p.Creature.IsDead))
								{
									await CombatManager.Instance.HandlePlayerDeath(player);
								}
							}
							player = null;
						}
						teammates = null;
					}
					else
					{
						if (recursion >= 10)
						{
							throw new InvalidOperationException("Combat is ending, but something is continually preventing the last creature from being killed!");
						}
						await Hook.AfterDeath(runState, combatState, creature, true, 0f);
						await Hook.AfterPreventingDeath(runState, combatState, preventer, creature);
						if (creature.IsDead)
						{
							await CreatureCmd.KillWithoutCheckingWinCondition(creature, force, recursion + 1);
						}
					}
				}
			}
		}

		/// <summary>
		/// Removes creature from combat without killing it and marks it escaped in the combat state.
		/// Used for things like the EscapeArtist power.
		/// </summary>
		// Token: 0x0600882E RID: 34862 RVA: 0x002DDD88 File Offset: 0x002DBF88
		public static Task Escape(Creature creature, bool removeCreatureNode = true)
		{
			if (creature.IsDead)
			{
				return Task.CompletedTask;
			}
			if (creature.CombatState == null || !creature.CombatState.IsLiveCombat())
			{
				return Task.CompletedTask;
			}
			creature.RemoveAllPowersInternalExcept(null);
			if (removeCreatureNode)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
				if (ncreature != null)
				{
					NCombatRoom.Instance.RemoveCreatureNode(ncreature);
					ncreature.ToggleIsInteractable(false);
					ncreature.Visible = false;
				}
			}
			CombatManager.Instance.RemoveCreature(creature);
			ICombatState combatState = creature.CombatState;
			if (combatState != null)
			{
				combatState.CreatureEscaped(creature);
			}
			return Task.CompletedTask;
		}

		/// <summary>
		/// Make the specified creature gain the specified amount of block.
		/// </summary>
		/// <param name="creature">Creature that should gain block.</param>
		/// <param name="blockVar">BlockVar containing the amount of block they should gain, and the properties of the block.</param>
		/// <param name="cardPlay">
		/// The CardPlay that caused the block gain.
		/// Null if it was not directly caused by a card play.
		/// </param>
		/// <param name="fast">If true, the wait that is performed after block gain is very small. Should be used in scenarios
		/// where block gain happens quickly in sequence (e.g. After Image, Barnacled Pipe).</param>
		/// <returns>The amount of block that the creature gained after all modifications were applied.</returns>
		// Token: 0x0600882F RID: 34863 RVA: 0x002DDE1C File Offset: 0x002DC01C
		public static async Task<decimal> GainBlock(Creature creature, BlockVar blockVar, [Nullable(2)] CardPlay cardPlay, bool fast = false)
		{
			return await CreatureCmd.GainBlock(creature, blockVar.BaseValue, blockVar.Props, cardPlay, fast);
		}

		/// <summary>
		/// Make this creature gain the specified amount of block.
		/// </summary>
		/// <param name="creature">Creature that should gain block.</param>
		/// <param name="amount">Amount of block they should gain.</param>
		/// <param name="props">Properties of the block.</param>
		/// <param name="cardPlay">
		/// The CardPlay that caused the block gain.
		/// Null if it was not directly caused by a card play.
		/// </param>
		/// <param name="fast">If true, the wait that is performed after block gain is very small. Should be used in scenarios
		/// where block gain happens quickly in sequence (e.g. After Image, Barnacled Pipe).</param>
		/// <returns>The amount of block that the creature gained after all modifications were applied.</returns>
		// Token: 0x06008830 RID: 34864 RVA: 0x002DDE78 File Offset: 0x002DC078
		public static async Task<decimal> GainBlock(Creature creature, decimal amount, ValueProp props, [Nullable(2)] CardPlay cardPlay, bool fast = false)
		{
			decimal num;
			if (CombatManager.Instance.IsOverOrEnding)
			{
				num = 0m;
			}
			else if (creature.IsDead)
			{
				num = 0m;
			}
			else
			{
				ICombatState combatState = creature.CombatState;
				await Hook.BeforeBlockGained(combatState, creature, amount, props, (cardPlay != null) ? cardPlay.Card : null);
				decimal modifiedAmount = amount;
				IEnumerable<AbstractModel> enumerable;
				modifiedAmount = Hook.ModifyBlock(combatState, creature, modifiedAmount, props, (cardPlay != null) ? cardPlay.Card : null, cardPlay, out enumerable);
				modifiedAmount = Math.Max(modifiedAmount, 0m);
				await Hook.AfterModifyingBlockAmount(combatState, modifiedAmount, (cardPlay != null) ? cardPlay.Card : null, cardPlay, enumerable);
				if (modifiedAmount > 0m)
				{
					SfxCmd.Play("event:/sfx/block_gain", 1f);
					VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_block");
					creature.GainBlockInternal(modifiedAmount);
					CombatManager.Instance.History.BlockGained(combatState, creature, (int)modifiedAmount, props, cardPlay);
					if (fast)
					{
						await Cmd.CustomScaledWait(0f, 0.03f, false, default(CancellationToken));
					}
					else
					{
						await Cmd.CustomScaledWait(0.1f, 0.25f, false, default(CancellationToken));
					}
				}
				await Hook.AfterBlockGained(combatState, creature, modifiedAmount, props, (cardPlay != null) ? cardPlay.Card : null);
				num = modifiedAmount;
			}
			return num;
		}

		/// <summary>
		/// Make this creature lose the specified amount of block.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="target">Creature that should lose block.</param>
		/// <param name="amount">Amount of block they should lose.</param>
		/// <param name="remover">
		/// Creature that caused the block loss.
		/// Null if no creature was involved in the block loss, like if it was removed by a power.
		/// </param>
		// Token: 0x06008831 RID: 34865 RVA: 0x002DDEDC File Offset: 0x002DC0DC
		public static async Task LoseBlock(PlayerChoiceContext choiceContext, Creature target, decimal amount, [Nullable(2)] Creature remover)
		{
			if (!CombatManager.Instance.IsOverOrEnding)
			{
				if (!target.IsDead)
				{
					if (!(amount <= 0m))
					{
						int block = target.Block;
						target.LoseBlockInternal(amount);
						if (block > 0 && target.Block <= 0)
						{
							SfxCmd.Play("event:/sfx/block_break", 1f);
							await Hook.AfterBlockBroken(target.CombatState, choiceContext, target, remover);
						}
					}
				}
			}
		}

		/// <summary>
		/// Heal a creature.
		/// </summary>
		/// <param name="creature">Creature to heal.</param>
		/// <param name="amount">Amount to heal them by.</param>
		/// <param name="playAnim">
		/// Whether or not to show the heal animation.
		/// True by default, and you should leave this true even if you're using it outside of combat (like when healing
		/// a player at the rest site). Only set it to false if you explicitly want the heal animation to not play even
		/// while in combat.
		/// </param>
		// Token: 0x06008832 RID: 34866 RVA: 0x002DDF38 File Offset: 0x002DC138
		public static async Task Heal(Creature creature, decimal amount, bool playAnim = true)
		{
			if (!CombatManager.Instance.IsEnding || creature.IsPlayer)
			{
				bool isDead = creature.IsDead;
				decimal num = Math.Min(amount, creature.MaxHp - creature.CurrentHp);
				if (creature == null || !(creature.Monster is Osty))
				{
					SfxCmd.Play("event:/sfx/heal", 1f);
				}
				creature.HealInternal(amount);
				if (playAnim)
				{
					if (!CombatManager.Instance.IsInProgress)
					{
						Player player = creature.Player;
						if (!(((player != null) ? player.RunState.CurrentRoom : null) is CombatRoom))
						{
							if (LocalContext.IsMe(creature))
							{
								if (creature.Player.RunState.CurrentRoom is EventRoom)
								{
									Player player2 = creature.Player;
									NEventRoom instance = NEventRoom.Instance;
									PlayerFullscreenHealVfx.Play(player2, amount, (instance != null) ? instance.VfxContainer : null);
									goto IL_031D;
								}
								if (creature.Player.RunState.CurrentRoom is MerchantRoom)
								{
									PlayerFullscreenHealVfx.Play(creature.Player, amount, NMerchantRoom.Instance);
									goto IL_031D;
								}
								goto IL_031D;
							}
							else
							{
								Player player3 = creature.Player;
								if (((player3 != null) ? player3.RunState.CurrentRoom : null) is RestSiteRoom && TestMode.IsOff)
								{
									NRestSiteRoom instance2 = NRestSiteRoom.Instance;
									NRestSiteCharacter nrestSiteCharacter = ((instance2 != null) ? instance2.Characters.First<NRestSiteCharacter>((NRestSiteCharacter c) => c.Player == creature.Player) : null);
									string scenePath = SceneHelper.GetScenePath("vfx/vfx_cross_heal");
									Node2D node2D = PreloadManager.Cache.GetScene(scenePath).Instantiate<Node2D>(0L);
									if (nrestSiteCharacter != null)
									{
										nrestSiteCharacter.AddChildSafely(node2D);
									}
									node2D.Position = Vector2.Zero;
									goto IL_031D;
								}
								goto IL_031D;
							}
						}
					}
					if (creature != null && creature.Monster is Osty)
					{
						VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_heal_osty");
					}
					else
					{
						VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_cross_heal");
					}
					Node vfxContainer = creature.GetVfxContainer();
					if (vfxContainer != null)
					{
						vfxContainer.AddChildSafely(NHealNumVfx.Create(creature, amount));
					}
					if (isDead)
					{
						NCombatRoom instance3 = NCombatRoom.Instance;
						if (instance3 != null)
						{
							NCreature creatureNode = instance3.GetCreatureNode(creature);
							if (creatureNode != null)
							{
								creatureNode.StartReviveAnim();
							}
						}
					}
				}
				IL_031D:
				Player player4 = creature.Player;
				MapPointHistoryEntry mapPointHistoryEntry = ((player4 != null) ? player4.RunState.CurrentMapPointHistoryEntry : null);
				if (mapPointHistoryEntry != null && num > 0m)
				{
					mapPointHistoryEntry.GetEntry(creature.Player.NetId).HpHealed += (int)num;
				}
				if (CombatManager.Instance.IsInProgress)
				{
					await Cmd.CustomScaledWait(0.1f, 0.25f, false, default(CancellationToken));
				}
				if (amount > 0m && creature.CombatState != null)
				{
					Player player5 = creature.Player;
					await Hook.AfterCurrentHpChanged(((player5 != null) ? player5.RunState : null) ?? creature.CombatState.RunState, creature.CombatState, creature, amount);
				}
			}
		}

		/// <summary>
		/// Set a creature's current HP to a new value.
		/// </summary>
		/// <param name="creature">Creature whose HP we're setting.</param>
		/// <param name="amount">New amount of HP they should have.</param>
		// Token: 0x06008833 RID: 34867 RVA: 0x002DDF8C File Offset: 0x002DC18C
		public static async Task SetCurrentHp(Creature creature, decimal amount)
		{
			bool flag = creature.IsDead && amount > 0m;
			decimal num = creature.CurrentHp;
			creature.SetCurrentHpInternal(amount);
			if (amount != num)
			{
				if (CombatManager.Instance.IsInProgress && flag)
				{
					NCombatRoom instance = NCombatRoom.Instance;
					if (instance != null)
					{
						NCreature creatureNode = instance.GetCreatureNode(creature);
						if (creatureNode != null)
						{
							creatureNode.StartReviveAnim();
						}
					}
				}
				Player player = creature.Player;
				IRunState runState;
				if ((runState = ((player != null) ? player.RunState : null)) == null)
				{
					ICombatState combatState = creature.CombatState;
					runState = ((combatState != null) ? combatState.RunState : null) ?? NullRunState.Instance;
				}
				await Hook.AfterCurrentHpChanged(runState, creature.CombatState, creature, amount - num);
			}
			if (creature.IsDead)
			{
				await CreatureCmd.Kill(creature, false);
			}
		}

		/// <summary>
		/// Increase a creature's maximum HP.
		/// </summary>
		/// <param name="creature">Creature to add max HP to.</param>
		/// <param name="amount">Amount of max HP to add.</param>
		// Token: 0x06008834 RID: 34868 RVA: 0x002DDFD8 File Offset: 0x002DC1D8
		public static async Task GainMaxHp(Creature creature, decimal amount)
		{
			if (amount < 0m)
			{
				throw new ArgumentException("amount must be non-negative. Use LoseMaxHp for max HP loss.");
			}
			decimal num = await CreatureCmd.SetMaxHp(creature, creature.MaxHp + amount);
			decimal num2 = num;
			Player player = creature.Player;
			MapPointHistoryEntry mapPointHistoryEntry = ((player != null) ? player.RunState.CurrentMapPointHistoryEntry : null);
			if (mapPointHistoryEntry != null)
			{
				mapPointHistoryEntry.GetEntry(creature.Player.NetId).MaxHpGained += (int)num2;
			}
			await CreatureCmd.Heal(creature, num2, true);
		}

		/// <summary>
		/// Decrease a creature's maximum HP.
		/// </summary>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="creature">Creature to remove max HP from.</param>
		/// <param name="amount">Amount of max HP to remove.</param>
		/// <param name="isFromCard">
		/// Whether or not this max HP loss is coming from a card.
		/// Used to determine whether effects like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Rupture" /> should trigger.
		/// </param>
		// Token: 0x06008835 RID: 34869 RVA: 0x002DE024 File Offset: 0x002DC224
		public static async Task LoseMaxHp(PlayerChoiceContext choiceContext, Creature creature, decimal amount, bool isFromCard)
		{
			if (amount < 0m)
			{
				throw new ArgumentException("amount must be non-negative. Use GainMaxHp for max HP gain.");
			}
			decimal newMaxHp = creature.MaxHp - amount;
			Player player = creature.Player;
			MapPointHistoryEntry mapPointHistoryEntry = ((player != null) ? player.RunState.CurrentMapPointHistoryEntry : null);
			if (mapPointHistoryEntry != null)
			{
				mapPointHistoryEntry.GetEntry(creature.Player.NetId).MaxHpLost += (int)amount;
			}
			if (newMaxHp < creature.CurrentHp)
			{
				await CreatureCmd.Damage(choiceContext, creature, creature.CurrentHp - newMaxHp, isFromCard ? (ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move) : (ValueProp.Unblockable | ValueProp.Unpowered), null, null);
			}
			await CreatureCmd.SetMaxHp(creature, Math.Max(1.0m, newMaxHp));
		}

		/// <summary>
		/// Set a creature's maximum HP to a new value.
		/// </summary>
		/// <param name="creature">Creature whose max HP we're setting.</param>
		/// <param name="amount">New amount of max HP they should have.</param>
		/// <returns>
		/// The amount that the creature's max HP changed by.
		/// A positive number means their max HP increased, a negative number means it decreased.
		/// </returns>
		// Token: 0x06008836 RID: 34870 RVA: 0x002DE080 File Offset: 0x002DC280
		public static async Task<decimal> SetMaxHp(Creature creature, decimal amount)
		{
			int oldMaxHp = creature.MaxHp;
			creature.SetMaxHpInternal(Math.Max(0m, amount));
			int newMaxHp = creature.MaxHp;
			if (creature.MaxHp <= 0)
			{
				await CreatureCmd.Kill(creature, false);
			}
			return newMaxHp - oldMaxHp;
		}

		/// <summary>
		/// Set a creature's maximum AND current HP to a new value.
		/// </summary>
		/// <param name="creature">Creature whose max/current HP we're setting.</param>
		/// <param name="amount">New amount of max/current HP they should have.</param>
		// Token: 0x06008837 RID: 34871 RVA: 0x002DE0CC File Offset: 0x002DC2CC
		public static async Task SetMaxAndCurrentHp(Creature creature, decimal amount)
		{
			await CreatureCmd.SetMaxHp(creature, amount);
			await CreatureCmd.SetCurrentHp(creature, amount);
		}

		/// <summary>
		/// Stun the specified creature, performing no logic during their stunned turn.
		/// </summary>
		/// <param name="creature">Creature to stun.</param>
		/// <param name="nextMoveId">
		/// ID of the move that the creature should perform the turn after they perform stunMove.
		/// If null or empty, this will default to the last move they performed.
		/// </param>
		// Token: 0x06008838 RID: 34872 RVA: 0x002DE118 File Offset: 0x002DC318
		public static async Task Stun(Creature creature, [Nullable(2)] string nextMoveId = null)
		{
			await CreatureCmd.Stun(creature, (IReadOnlyList<Creature> _) => Task.CompletedTask, nextMoveId);
		}

		/// <summary>
		/// Stun the specified creature, performing the specified logic during their stunned turn.
		/// </summary>
		/// <param name="creature">Creature to stun.</param>
		/// <param name="stunMove">Logic for the move that the stunned creature will "perform".</param>
		/// <param name="nextMoveId">
		/// ID of the move that the creature should perform the turn after they perform stunMove.
		/// If null or empty, this will default to the last move they performed.
		/// </param>
		// Token: 0x06008839 RID: 34873 RVA: 0x002DE164 File Offset: 0x002DC364
		public static Task Stun(Creature creature, Func<IReadOnlyList<Creature>, Task> stunMove, [Nullable(2)] string nextMoveId = null)
		{
			CreatureCmd.<>c__DisplayClass27_0 CS$<>8__locals1 = new CreatureCmd.<>c__DisplayClass27_0();
			CS$<>8__locals1.creature = creature;
			CS$<>8__locals1.stunMove = stunMove;
			CS$<>8__locals1.creature.StunInternal(new Func<IReadOnlyList<Creature>, Task>(CS$<>8__locals1.<Stun>g__Wrapper|0), nextMoveId);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Trigger an animation on the specified creature.
		/// </summary>
		/// <param name="creature">Creature to animate.</param>
		/// <param name="triggerName">Name of the animation trigger.</param>
		/// <param name="waitTime">Number of seconds to wait after starting the animation.</param>
		// Token: 0x0600883A RID: 34874 RVA: 0x002DE1A4 File Offset: 0x002DC3A4
		public static async Task TriggerAnim(Creature creature, string triggerName, float waitTime)
		{
			NCreature creatureNode = creature.GetCreatureNode();
			if (creatureNode == null)
			{
				if (!TestMode.IsOn)
				{
					if (CombatManager.Instance.IsInProgress)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Attempted to play animation on creature ");
						defaultInterpolatedStringHandler.AppendFormatted<Creature>(creature);
						defaultInterpolatedStringHandler.AppendLiteral(" but its creature node doesn't exist!");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					}
				}
			}
			else
			{
				if (creature.IsPlayer)
				{
					CharacterModel character = creature.Player.Character;
					if (creature.IsDead)
					{
						return;
					}
					if (!(triggerName == "Attack"))
					{
						if (!(triggerName == "Cast"))
						{
							if (triggerName == "PowerUp")
							{
								SfxCmd.Play(character.PowerUpSfx, 1f);
							}
						}
						else
						{
							SfxCmd.Play(character.CastSfx, 1f);
						}
					}
					else
					{
						SfxCmd.Play(character.AttackSfx, 1f);
					}
				}
				creatureNode.SetAnimationTrigger(triggerName);
				await Cmd.CustomScaledWait(Mathf.Min(waitTime * 0.5f, 0.25f), waitTime, false, default(CancellationToken));
			}
		}
	}
}
