using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D34 RID: 3380
	[NullableContext(1)]
	[Nullable(0)]
	public static class PotionCmd
	{
		// Token: 0x0600885C RID: 34908 RVA: 0x002DECDC File Offset: 0x002DCEDC
		public static async Task<PotionProcureResult> TryToProcure<[Nullable(0)] T>(Player player) where T : PotionModel
		{
			return await PotionCmd.TryToProcure(ModelDb.Potion<T>().ToMutable(), player, -1);
		}

		/// <summary>
		/// We try to Procure the potion. Can fail if the potions bar is full.
		/// </summary>
		/// <param name="potion">Potion to be added</param>
		/// <param name="player">Player who is getting the potion</param>
		/// <param name="slotIndex">Slot at which to add the potion. If -1, the potion will be added in the first available slot.</param>
		/// <returns>Potion if it is added successfully. Returns null if it failed.</returns>
		// Token: 0x0600885D RID: 34909 RVA: 0x002DED20 File Offset: 0x002DCF20
		public static async Task<PotionProcureResult> TryToProcure(PotionModel potion, Player player, int slotIndex = -1)
		{
			potion.AssertMutable();
			PotionProcureResult potionProcureResult;
			if (!Hook.ShouldProcurePotion(player.RunState, player.Creature.CombatState, potion, player))
			{
				potionProcureResult = new PotionProcureResult
				{
					potion = potion,
					success = false,
					failureReason = PotionProcureFailureReason.NotAllowed
				};
			}
			else
			{
				PotionProcureResult result = player.AddPotionInternal(potion, slotIndex, false);
				if (result.success)
				{
					MapPointHistoryEntry currentMapPointHistoryEntry = player.RunState.CurrentMapPointHistoryEntry;
					if (currentMapPointHistoryEntry != null)
					{
						currentMapPointHistoryEntry.GetEntry(player.NetId).PotionChoices.Add(new ModelChoiceHistoryEntry(potion.Id, true));
					}
					NDebugAudioManager instance = NDebugAudioManager.Instance;
					if (instance != null)
					{
						instance.Play("gain_potion.mp3", 1f, PitchVariance.Small);
					}
					NRun instance2 = NRun.Instance;
					if (instance2 != null)
					{
						instance2.GlobalUi.TopBar.PotionContainer.AnimatePotion(potion, null);
					}
					await Hook.AfterPotionProcured(player.RunState, player.Creature.CombatState, potion);
					if (LocalContext.IsMe(player))
					{
						SaveManager.Instance.MarkPotionAsSeen(potion);
					}
				}
				potionProcureResult = result;
			}
			return potionProcureResult;
		}

		// Token: 0x0600885E RID: 34910 RVA: 0x002DED74 File Offset: 0x002DCF74
		public static async Task Discard(PotionModel potion)
		{
			MapPointHistoryEntry currentMapPointHistoryEntry = potion.Owner.RunState.CurrentMapPointHistoryEntry;
			if (currentMapPointHistoryEntry != null)
			{
				currentMapPointHistoryEntry.GetEntry(potion.Owner.NetId).PotionDiscarded.Add(potion.Id);
			}
			potion.Discard();
			await Hook.AfterPotionDiscarded(potion.Owner.RunState, potion.Owner.Creature.CombatState, potion);
		}
	}
}
