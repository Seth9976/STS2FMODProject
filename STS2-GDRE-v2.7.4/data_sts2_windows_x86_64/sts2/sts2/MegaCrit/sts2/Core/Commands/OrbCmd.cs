using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D31 RID: 3377
	[NullableContext(1)]
	[Nullable(0)]
	public static class OrbCmd
	{
		/// <summary>
		/// Add orb slots to a creature.
		/// </summary>
		/// <param name="player">Player to add orb slots to.</param>
		/// <param name="amount">Number of orb slots to add.</param>
		// Token: 0x06008843 RID: 34883 RVA: 0x002DE54C File Offset: 0x002DC74C
		public static Task AddSlots(Player player, int amount)
		{
			if (CombatManager.Instance.IsOverOrEnding)
			{
				return Task.CompletedTask;
			}
			amount = Math.Min(10 - player.PlayerCombatState.OrbQueue.Capacity, amount);
			player.PlayerCombatState.OrbQueue.AddCapacity(amount);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				NOrbManager orbManager = instance.GetCreatureNode(player.Creature).OrbManager;
				if (orbManager != null)
				{
					orbManager.AddSlotAnim(amount);
				}
			}
			return Task.CompletedTask;
		}

		/// <summary>
		/// Remove orb slots from the creature. Starts from the back of the list.
		/// Orb slots with orbs already in them are also removed.
		/// </summary>
		/// <param name="player">Player to remove orb slots from.</param>
		/// <param name="amount">Number of orb slots to remove.</param>
		// Token: 0x06008844 RID: 34884 RVA: 0x002DE5C4 File Offset: 0x002DC7C4
		public static void RemoveSlots(Player player, int amount)
		{
			if (CombatManager.Instance.IsOverOrEnding)
			{
				return;
			}
			amount = Math.Min(player.PlayerCombatState.OrbQueue.Capacity, amount);
			player.PlayerCombatState.OrbQueue.RemoveCapacity(amount);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance == null)
			{
				return;
			}
			NOrbManager orbManager = instance.GetCreatureNode(player.Creature).OrbManager;
			if (orbManager == null)
			{
				return;
			}
			orbManager.RemoveSlotAnim(amount);
		}

		/// <summary>
		/// Channel an orb of the specified type.
		/// </summary>
		/// <param name="choiceContext">The context with which to handle player choices.</param>
		/// <param name="player">Player who is channeling the orb.</param>
		/// <typeparam name="T">Type of orb to channel.</typeparam>
		// Token: 0x06008845 RID: 34885 RVA: 0x002DE62C File Offset: 0x002DC82C
		public static async Task Channel<[Nullable(0)] T>(PlayerChoiceContext choiceContext, Player player) where T : OrbModel
		{
			await OrbCmd.Channel(choiceContext, ModelDb.Orb<T>().ToMutable(0), player);
		}

		/// <summary>
		/// Channel an orb.
		/// </summary>
		/// <param name="choiceContext">The context with which to handle player choices.</param>
		/// <param name="orb">Orb to channel.</param>
		/// <param name="player">Player who is channeling the orb.</param>
		// Token: 0x06008846 RID: 34886 RVA: 0x002DE678 File Offset: 0x002DC878
		public static async Task Channel(PlayerChoiceContext choiceContext, OrbModel orb, Player player)
		{
			if (!CombatManager.Instance.IsOverOrEnding)
			{
				ICombatState combatState = player.Creature.CombatState;
				OrbQueue orbQueue = player.PlayerCombatState.OrbQueue;
				if (player.Character.BaseOrbSlotCount == 0 && orbQueue.Capacity == 0)
				{
					await OrbCmd.AddSlots(player, 1);
				}
				orb.AssertMutable();
				orb.Owner = player;
				if (orbQueue.Orbs.Count >= orbQueue.Capacity)
				{
					await OrbCmd.EvokeNext(choiceContext, player, true);
				}
				bool flag = await player.PlayerCombatState.OrbQueue.TryEnqueue(orb);
				if (flag)
				{
					CombatManager.Instance.History.OrbChanneled(combatState, orb);
					orb.PlayChannelSfx();
					NCombatRoom instance = NCombatRoom.Instance;
					if (instance != null)
					{
						NCreature creatureNode = instance.GetCreatureNode(player.Creature);
						if (creatureNode != null)
						{
							NOrbManager orbManager = creatureNode.OrbManager;
							if (orbManager != null)
							{
								orbManager.AddOrbAnim();
							}
						}
					}
					await Hook.AfterOrbChanneled(combatState, choiceContext, player, orb);
				}
			}
		}

		// Token: 0x06008847 RID: 34887 RVA: 0x002DE6CC File Offset: 0x002DC8CC
		public static async Task EvokeNext(PlayerChoiceContext choiceContext, Player player, bool dequeue = true)
		{
			OrbQueue orbQueue = player.PlayerCombatState.OrbQueue;
			if (orbQueue.Orbs.Count > 0)
			{
				OrbModel orb = orbQueue.Orbs.First<OrbModel>();
				choiceContext.PushModel(orb);
				await OrbCmd.Evoke(choiceContext, player, orb, dequeue);
				choiceContext.PopModel(orb);
			}
		}

		// Token: 0x06008848 RID: 34888 RVA: 0x002DE720 File Offset: 0x002DC920
		public static async Task EvokeLast(PlayerChoiceContext choiceContext, Player player, bool dequeue = true)
		{
			OrbQueue orbQueue = player.PlayerCombatState.OrbQueue;
			if (orbQueue.Orbs.Count > 0)
			{
				OrbModel orb = orbQueue.Orbs.Last<OrbModel>();
				choiceContext.PushModel(orb);
				await OrbCmd.Evoke(choiceContext, player, orb, dequeue);
				choiceContext.PopModel(orb);
			}
		}

		/// <summary>
		/// Evoke the orb.
		/// </summary>
		/// <param name="choiceContext">The context with which to handle player choices.</param>
		/// <param name="player">Player whose next orb we're evoking.</param>
		/// <param name="evokedOrb">orb being evoked.</param>
		/// <param name="dequeue">Whether or not to dequeue the orb from the creature's orb queue after evoking (usually true).</param>
		// Token: 0x06008849 RID: 34889 RVA: 0x002DE774 File Offset: 0x002DC974
		private static async Task Evoke(PlayerChoiceContext choiceContext, Player player, OrbModel evokedOrb, bool dequeue = true)
		{
			if (!CombatManager.Instance.IsOverOrEnding)
			{
				OrbQueue orbQueue = player.PlayerCombatState.OrbQueue;
				if (orbQueue.Orbs.Count > 0)
				{
					bool removed = false;
					if (dequeue)
					{
						removed = orbQueue.Remove(evokedOrb);
						NCombatRoom instance = NCombatRoom.Instance;
						if (instance != null)
						{
							NCreature creatureNode = instance.GetCreatureNode(player.Creature);
							if (creatureNode != null)
							{
								NOrbManager orbManager = creatureNode.OrbManager;
								if (orbManager != null)
								{
									orbManager.EvokeOrbAnim(evokedOrb);
								}
							}
						}
					}
					choiceContext.PushModel(evokedOrb);
					IEnumerable<Creature> enumerable = await evokedOrb.Evoke(choiceContext);
					IEnumerable<Creature> enumerable2 = enumerable;
					choiceContext.PopModel(evokedOrb);
					if (player.Creature.CombatState != null)
					{
						await Hook.AfterOrbEvoked(choiceContext, player.Creature.CombatState, evokedOrb, enumerable2);
						if (removed)
						{
							evokedOrb.RemoveInternal();
						}
					}
				}
			}
		}

		// Token: 0x0600884A RID: 34890 RVA: 0x002DE7D0 File Offset: 0x002DC9D0
		public static async Task Passive(PlayerChoiceContext choiceContext, OrbModel orb, [Nullable(2)] Creature target, bool countAffectedByHooks = false)
		{
			if (!CombatManager.Instance.IsOverOrEnding)
			{
				choiceContext.PushModel(orb);
				if (countAffectedByHooks)
				{
					await orb.TriggerPassive(choiceContext, target);
				}
				else
				{
					await orb.Passive(choiceContext, target);
				}
				choiceContext.PopModel(orb);
			}
		}
	}
}
