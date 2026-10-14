using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D33 RID: 3379
	[NullableContext(1)]
	[Nullable(0)]
	public static class PlayerCmd
	{
		/// <summary>
		/// Increase the current amount of energy that the player has.
		/// </summary>
		/// <param name="amount">Amount of energy to give.</param>
		/// <param name="player">Player to give the energy to.</param>
		// Token: 0x0600884C RID: 34892 RVA: 0x002DE888 File Offset: 0x002DCA88
		public static async Task GainEnergy(decimal amount, Player player)
		{
			if (!(amount <= 0m))
			{
				if (!CombatManager.Instance.IsEnding)
				{
					ICombatState combatState = player.Creature.CombatState;
					IEnumerable<AbstractModel> enumerable;
					decimal finalAmount = Hook.ModifyEnergyGain(combatState, player, amount, out enumerable);
					await Hook.AfterModifyingEnergyGain(combatState, enumerable);
					if (finalAmount > 0m)
					{
						SfxCmd.Play("event:/sfx/ui/gain_energy", 1f);
						player.PlayerCombatState.GainEnergy(finalAmount);
					}
				}
			}
		}

		/// <summary>
		/// Decrease the current amount of energy that the player has.
		/// </summary>
		/// <param name="amount">Amount of energy to remove.</param>
		/// <param name="player">Player to remove the energy from.</param>
		// Token: 0x0600884D RID: 34893 RVA: 0x002DE8D3 File Offset: 0x002DCAD3
		public static Task LoseEnergy(decimal amount, Player player)
		{
			if (amount <= 0m)
			{
				return Task.CompletedTask;
			}
			if (CombatManager.Instance.IsEnding)
			{
				return Task.CompletedTask;
			}
			player.PlayerCombatState.LoseEnergy(amount);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Set the player to have a specific amount of energy.
		/// </summary>
		/// <param name="amount">New amount of energy.</param>
		/// <param name="player">Player whose energy we're setting.</param>
		// Token: 0x0600884E RID: 34894 RVA: 0x002DE90C File Offset: 0x002DCB0C
		public static async Task SetEnergy(decimal amount, Player player)
		{
			if (!CombatManager.Instance.IsEnding)
			{
				int energy = player.PlayerCombatState.Energy;
				if (energy < amount)
				{
					await PlayerCmd.GainEnergy(amount - energy, player);
				}
				else if (energy > amount)
				{
					await PlayerCmd.LoseEnergy(energy - amount, player);
				}
			}
		}

		/// <summary>
		/// Increase the current amount of stars that the player has.
		/// </summary>
		/// <param name="amount">Amount of stars to give.</param>
		/// <param name="player">Player to give the stars to.</param>
		// Token: 0x0600884F RID: 34895 RVA: 0x002DE958 File Offset: 0x002DCB58
		public static async Task GainStars(decimal amount, Player player)
		{
			if (!CombatManager.Instance.IsEnding)
			{
				if (Hook.ShouldGainStars(player.Creature.CombatState, amount, player))
				{
					player.PlayerCombatState.GainStars(amount);
					await Hook.AfterStarsGained(player.Creature.CombatState, (int)amount, player);
				}
			}
		}

		/// <summary>
		/// Decrease the current amount of stars that the player has.
		/// </summary>
		/// <param name="amount">Amount of stars to remove.</param>
		/// <param name="player">Player to remove the stars from.</param>
		// Token: 0x06008850 RID: 34896 RVA: 0x002DE9A3 File Offset: 0x002DCBA3
		public static Task LoseStars(decimal amount, Player player)
		{
			if (CombatManager.Instance.IsEnding)
			{
				return Task.CompletedTask;
			}
			player.PlayerCombatState.LoseStars(amount);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Set the player to have a specific amount of stars.
		/// </summary>
		/// <param name="amount">New amount of stars.</param>
		/// <param name="player">Player whose stars we're setting.</param>
		// Token: 0x06008851 RID: 34897 RVA: 0x002DE9C8 File Offset: 0x002DCBC8
		public static async Task SetStars(decimal amount, Player player)
		{
			if (!CombatManager.Instance.IsEnding)
			{
				int stars = player.PlayerCombatState.Stars;
				if (stars < amount)
				{
					await PlayerCmd.GainStars(amount - stars, player);
				}
				else if (stars > amount)
				{
					await PlayerCmd.LoseStars(stars - amount, player);
				}
			}
		}

		/// <summary>
		/// Increase the current amount of gold that the player has.
		/// </summary>
		/// <param name="amount">Amount of gold to give.</param>
		/// <param name="player">Player to give the gold to.</param>
		/// <param name="wasStolenBack">Was the gold stolen back from an enemy.</param>
		// Token: 0x06008852 RID: 34898 RVA: 0x002DEA14 File Offset: 0x002DCC14
		public static async Task GainGold(decimal amount, Player player, bool wasStolenBack = false)
		{
			IRunState runState = player.RunState;
			IEnumerable<AbstractModel> enumerable;
			amount = Hook.ModifyGoldGained(runState, player.Creature.CombatState, amount, player, out enumerable);
			await Hook.AfterModifyingGoldGained(runState, player.Creature.CombatState, enumerable, player, amount);
			if (amount > 0m)
			{
				if (player == LocalContext.GetMe(runState))
				{
					SfxCmd.Play((amount >= 100m) ? "event:/sfx/ui/gold/gold_3" : ((amount > 30m) ? "event:/sfx/ui/gold/gold_2" : "event:/sfx/ui/gold/gold_1"), 1f);
				}
				MapPointHistoryEntry currentMapPointHistoryEntry = runState.CurrentMapPointHistoryEntry;
				PlayerMapPointHistoryEntry playerMapPointHistoryEntry = ((currentMapPointHistoryEntry != null) ? currentMapPointHistoryEntry.GetEntry(player.NetId) : null);
				if (playerMapPointHistoryEntry != null)
				{
					if (wasStolenBack)
					{
						playerMapPointHistoryEntry.GoldStolen -= (int)amount;
					}
					else
					{
						playerMapPointHistoryEntry.GoldGained += (int)amount;
					}
				}
				player.Gold += (int)amount;
				await Hook.AfterGoldGained(runState, player);
			}
		}

		/// <summary>
		/// Decrease the current amount of gold that the player has.
		/// </summary>
		/// <param name="amount">Amount of gold to lose.</param>
		/// <param name="player">Player to take the gold from.</param>
		/// <param name="goldLossType">How the player lost the gold</param>
		// Token: 0x06008853 RID: 34899 RVA: 0x002DEA68 File Offset: 0x002DCC68
		public static Task LoseGold(decimal amount, Player player, GoldLossType goldLossType = GoldLossType.Lost)
		{
			SfxCmd.Play("event:/sfx/ui/gold/gold_1", 1f);
			MapPointHistoryEntry currentMapPointHistoryEntry = player.RunState.CurrentMapPointHistoryEntry;
			PlayerMapPointHistoryEntry playerMapPointHistoryEntry = ((currentMapPointHistoryEntry != null) ? currentMapPointHistoryEntry.GetEntry(player.NetId) : null);
			if (playerMapPointHistoryEntry != null)
			{
				switch (goldLossType)
				{
				case GoldLossType.Spent:
					playerMapPointHistoryEntry.GoldSpent += (int)amount;
					break;
				case GoldLossType.Lost:
					playerMapPointHistoryEntry.GoldLost += (int)amount;
					break;
				case GoldLossType.Stolen:
					playerMapPointHistoryEntry.GoldStolen += (int)amount;
					playerMapPointHistoryEntry.MarkLootStolen((int)amount);
					break;
				}
			}
			player.Gold = int.Max(0, player.Gold - (int)amount);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Set the player to have a specific amount of gold.
		/// </summary>
		/// <param name="amount">New amount of gold.</param>
		/// <param name="player">Player whose gold we're setting.</param>
		// Token: 0x06008854 RID: 34900 RVA: 0x002DEB24 File Offset: 0x002DCD24
		public static async Task SetGold(decimal amount, Player player)
		{
			int gold = player.Gold;
			if (gold < amount)
			{
				await PlayerCmd.GainGold(amount - gold, player, false);
			}
			else if (gold > amount)
			{
				await PlayerCmd.LoseGold(gold - amount, player, GoldLossType.Lost);
			}
		}

		// Token: 0x06008855 RID: 34901 RVA: 0x002DEB6F File Offset: 0x002DCD6F
		public static Task GainMaxPotionCount(int amount, Player player)
		{
			player.AddToMaxPotionCount(amount);
			return Task.CompletedTask;
		}

		// Token: 0x06008856 RID: 34902 RVA: 0x002DEB7D File Offset: 0x002DCD7D
		public static Task LoseMaxPotionCount(int amount, Player player)
		{
			player.SubtractFromMaxPotionCount(amount);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Give the player a new pet (aww).
		/// </summary>
		/// <param name="player">Player to give the pet to.</param>
		/// <typeparam name="T">Type of pet to give them.</typeparam>
		// Token: 0x06008857 RID: 34903 RVA: 0x002DEB8C File Offset: 0x002DCD8C
		public static async Task<Creature> AddPet<[Nullable(0)] T>(Player player) where T : MonsterModel
		{
			Creature pet = player.Creature.CombatState.CreateCreature((T)((object)ModelDb.Monster<T>().ToMutable()), player.Creature.Side, null);
			await PlayerCmd.AddPet(pet, player);
			return pet;
		}

		/// <summary>
		/// Give the player a new pet (aww).
		/// </summary>
		/// <param name="pet">Pet creature to give to the player.</param>
		/// <param name="player">Player to give the pet to.</param>
		// Token: 0x06008858 RID: 34904 RVA: 0x002DEBD0 File Offset: 0x002DCDD0
		public static async Task AddPet(Creature pet, Player player)
		{
			if (pet.CombatState == null)
			{
				throw new InvalidOperationException("Pet must already be added to a combat state.");
			}
			player.PlayerCombatState.AddPetInternal(pet);
			await CreatureCmd.Add(pet);
		}

		/// <summary>
		/// Heal the player as if they were resting at a rest site.
		/// </summary>
		/// <param name="player">Player to heal.</param>
		/// <param name="playSfx">If true, we'll play the default rest site SFX.</param>
		// Token: 0x06008859 RID: 34905 RVA: 0x002DEC1C File Offset: 0x002DCE1C
		public static async Task MimicRestSiteHeal(Player player, bool playSfx = true)
		{
			if (playSfx)
			{
				HealRestSiteOption.PlayRestSiteHealSfx();
			}
			await HealRestSiteOption.ExecuteRestSiteHeal(player, true);
		}

		/// <summary>
		/// Ends the turn for a given player.
		/// </summary>
		/// <param name="player">Player who is ending their turn</param>
		/// <param name="canBackOut">If the player is allowed to un-end their turn, particularly in multiplayer.</param>
		/// <param name="actionDuringEnemyTurn">Optional action to execute during the enemy turn. This is useful for tests.</param>
		// Token: 0x0600885A RID: 34906 RVA: 0x002DEC67 File Offset: 0x002DCE67
		public static void EndTurn(Player player, bool canBackOut, [Nullable(new byte[] { 2, 1 })] Func<Task> actionDuringEnemyTurn = null)
		{
			if (CombatManager.Instance.IsPlayerReadyToEndTurn(player))
			{
				return;
			}
			if (LocalContext.IsMe(player))
			{
				CombatManager.Instance.OnEndedTurnLocally();
			}
			CombatManager.Instance.SetReadyToEndTurn(player, canBackOut, actionDuringEnemyTurn);
		}

		// Token: 0x0600885B RID: 34907 RVA: 0x002DEC98 File Offset: 0x002DCE98
		public static void CompleteQuest(CardModel questCard)
		{
			MapPointHistoryEntry currentMapPointHistoryEntry = questCard.Owner.RunState.CurrentMapPointHistoryEntry;
			if (currentMapPointHistoryEntry != null)
			{
				currentMapPointHistoryEntry.GetEntry(questCard.Owner.NetId).CompletedQuests.Add(questCard.Id);
			}
		}

		// Token: 0x04002F5D RID: 12125
		public const string goldSmallSfx = "event:/sfx/ui/gold/gold_1";

		// Token: 0x04002F5E RID: 12126
		public const string goldMediumSfx = "event:/sfx/ui/gold/gold_2";

		// Token: 0x04002F5F RID: 12127
		public const string goldLargeSfx = "event:/sfx/ui/gold/gold_3";
	}
}
