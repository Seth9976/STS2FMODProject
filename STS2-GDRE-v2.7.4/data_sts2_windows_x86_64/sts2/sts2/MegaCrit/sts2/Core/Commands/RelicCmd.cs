using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D36 RID: 3382
	[NullableContext(1)]
	[Nullable(0)]
	public static class RelicCmd
	{
		/// <summary>
		/// Give a relic to the player.
		/// </summary>
		/// <param name="player">Player to give the relic to.</param>
		/// <typeparam name="T">Type of relic to obtain.</typeparam>
		/// <returns>RelicModel</returns>
		// Token: 0x06008868 RID: 34920 RVA: 0x002DF104 File Offset: 0x002DD304
		public static async Task<T> Obtain<[Nullable(0)] T>(Player player) where T : RelicModel
		{
			RelicModel relicModel = await RelicCmd.Obtain(ModelDb.Relic<T>().ToMutable(), player, -1);
			return (T)((object)relicModel);
		}

		/// <summary>
		/// Give a relic to the player.
		/// </summary>
		/// <param name="relic">Relic to obtain.</param>
		/// <param name="player">Player to give the relic to.</param>
		/// <param name="index">index in the relic list we want to add our relic to.
		/// Default of -1 means that we append it to the end of the relic list.</param>
		/// <returns>RelicModel</returns>
		// Token: 0x06008869 RID: 34921 RVA: 0x002DF148 File Offset: 0x002DD348
		public static async Task<RelicModel> Obtain(RelicModel relic, Player player, int index = -1)
		{
			relic.AssertMutable();
			IRunState runState = player.RunState;
			MapPointHistoryEntry currentMapPointHistoryEntry = runState.CurrentMapPointHistoryEntry;
			if (currentMapPointHistoryEntry != null)
			{
				currentMapPointHistoryEntry.GetEntry(player.NetId).RelicChoices.Add(new ModelChoiceHistoryEntry(relic.Id, true));
			}
			player.AddRelicInternal(relic, index, false);
			if (!relic.IsStackable)
			{
				player.RelicGrabBag.Remove(relic);
				runState.SharedRelicGrabBag.Remove(relic);
			}
			if (LocalContext.IsMe(player))
			{
				NRun instance = NRun.Instance;
				if (instance != null)
				{
					instance.GlobalUi.RelicInventory.AnimateRelic(relic, null, null);
				}
				NDebugAudioManager instance2 = NDebugAudioManager.Instance;
				if (instance2 != null)
				{
					instance2.Play("relic_get.mp3", 1f, PitchVariance.None);
				}
				SaveManager.Instance.MarkRelicAsSeen(relic);
			}
			relic.FloorAddedToDeck = runState.TotalFloor;
			await relic.AfterObtained();
			return relic;
		}

		/// <summary>
		/// Remove a relic from its owner.
		/// </summary>
		/// <param name="relic">Relic to remove.</param>
		// Token: 0x0600886A RID: 34922 RVA: 0x002DF19C File Offset: 0x002DD39C
		public static async Task Remove(RelicModel relic)
		{
			MapPointHistoryEntry currentMapPointHistoryEntry = relic.Owner.RunState.CurrentMapPointHistoryEntry;
			if (currentMapPointHistoryEntry != null)
			{
				currentMapPointHistoryEntry.GetEntry(relic.Owner.NetId).RelicsRemoved.Add(relic.Id);
			}
			relic.Owner.RemoveRelicInternal(relic, false);
			await relic.AfterRemoved();
		}

		/// <summary>
		/// Replace a relic with another relic
		/// </summary>
		/// <param name="original">Relic to remove.</param>
		/// <param name="replace">Relic to obtain.</param>
		/// <returns>RelicModel</returns>
		// Token: 0x0600886B RID: 34923 RVA: 0x002DF1E0 File Offset: 0x002DD3E0
		public static async Task<RelicModel> Replace(RelicModel original, RelicModel replace)
		{
			original.AssertMutable();
			replace.AssertMutable();
			Player player = original.Owner;
			int indexOfOriginal = player.Relics.IndexOf(original);
			await RelicCmd.Remove(original);
			return await RelicCmd.Obtain(replace, player, indexOfOriginal);
		}

		/// <summary>
		/// Melts a relic.
		/// A melted relic remains in your inventory, but is unusable.
		/// </summary>
		/// <param name="relic">Relic to melt.</param>
		// Token: 0x0600886C RID: 34924 RVA: 0x002DF22C File Offset: 0x002DD42C
		public static async Task Melt(RelicModel relic)
		{
			relic.Owner.MeltRelicInternal(relic);
			await relic.AfterRemoved();
		}
	}
}
