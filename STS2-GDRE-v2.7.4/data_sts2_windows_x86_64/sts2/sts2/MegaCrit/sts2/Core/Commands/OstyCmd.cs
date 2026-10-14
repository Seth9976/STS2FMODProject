using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D32 RID: 3378
	public static class OstyCmd
	{
		/// <summary>
		/// Summon Osty with the specified number of HP. If the specified creature already owns an instance of Osty, raise
		/// Osty's max HP by the specified number instead.
		/// </summary>
		/// <param name="choiceContext">The context with which to handle player choices.</param>
		/// <param name="summoner">The player who is summoning.</param>
		/// <param name="amount">
		/// The number of HP that Osty should be summoned with (or that should be added to the existing Osty instance).
		/// </param>
		/// <param name="source">
		/// The model that this Summon came from. For example, <see cref="T:MegaCrit.Sts2.Core.Models.Cards.Bodyguard" /> and <see cref="T:MegaCrit.Sts2.Core.Models.Relics.BoundPhylactery" />
		/// pass themselves here.
		/// Null if the Summon did not come from any model (generally only relevant in tests).
		/// </param>
		/// <returns>The result of the summon.</returns>
		// Token: 0x0600884B RID: 34891 RVA: 0x002DE82C File Offset: 0x002DCA2C
		[NullableContext(1)]
		public static async Task<SummonResult> Summon(PlayerChoiceContext choiceContext, Player summoner, decimal amount, [Nullable(2)] AbstractModel source)
		{
			ICombatState combatState = summoner.Creature.CombatState;
			amount = Hook.ModifySummonAmount(combatState, summoner, amount, source);
			SummonResult summonResult;
			if (amount == 0m)
			{
				summonResult = new SummonResult(summoner.Osty, 0m);
			}
			else
			{
				if (CombatManager.Instance.IsInProgress)
				{
					SfxCmd.Play("event:/sfx/characters/necrobinder/necrobinder_summon", 1f);
				}
				Creature osty = combatState.Allies.FirstOrDefault<Creature>((Creature c) => c.Monster is Osty && c.PetOwner == summoner);
				if (summoner.IsOstyAlive)
				{
					await CreatureCmd.GainMaxHp(summoner.Osty, amount);
				}
				else
				{
					bool isReviving = osty != null;
					if (isReviving)
					{
						if (osty.IsAlive)
						{
							throw new InvalidOperationException("We shouldn't make it here if Osty is still alive!");
						}
						summoner.PlayerCombatState.AddPetInternal(osty);
					}
					else
					{
						osty = await PlayerCmd.AddPet<Osty>(summoner);
						NCombatRoom instance = NCombatRoom.Instance;
						NCreature ostyNode = ((instance != null) ? instance.GetCreatureNode(osty) : null);
						if (ostyNode != null && source is CardModel)
						{
							ostyNode.Modulate = Colors.Transparent;
							ostyNode.CreateTween().TweenProperty(ostyNode, "modulate", Colors.White, 0.3499999940395355).SetDelay(0.10000000149011612);
							ostyNode.StartReviveAnim();
						}
						await PowerCmd.Apply<DieForYouPower>(choiceContext, osty, 1m, null, null, false);
						NCreature ncreature = ostyNode;
						if (ncreature != null)
						{
							ncreature.TrackBlockStatus(summoner.Creature);
						}
						ostyNode = null;
					}
					await CreatureCmd.SetMaxHp(osty, amount);
					await CreatureCmd.Heal(osty, amount, isReviving);
					if (isReviving)
					{
						await Hook.AfterOstyRevived(combatState, osty);
					}
				}
				if (osty != null)
				{
					NCombatRoom instance2 = NCombatRoom.Instance;
					if (instance2 != null)
					{
						NCreature creatureNode = instance2.GetCreatureNode(osty);
						if (creatureNode != null)
						{
							creatureNode.OstyScaleToSize((float)osty.MaxHp, 0.75);
						}
					}
				}
				CombatManager.Instance.History.Summoned(combatState, (int)amount, summoner);
				await Hook.AfterSummon(combatState, choiceContext, summoner, amount);
				summonResult = new SummonResult(summoner.Osty, amount);
			}
			return summonResult;
		}
	}
}
