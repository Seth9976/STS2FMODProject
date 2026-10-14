using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Commands
{
	// Token: 0x02000D37 RID: 3383
	[NullableContext(1)]
	[Nullable(0)]
	public static class RelicSelectCmd
	{
		// Token: 0x0600886D RID: 34925 RVA: 0x002DF26F File Offset: 0x002DD46F
		private static bool ShouldSelectLocalRelic(Player player)
		{
			return LocalContext.IsMe(player) && RunManager.Instance.NetService.Type != NetGameType.Replay;
		}

		// Token: 0x0600886E RID: 34926 RVA: 0x002DF290 File Offset: 0x002DD490
		[return: Nullable(new byte[] { 1, 2 })]
		public static async Task<RelicModel> FromChooseARelicScreen(Player player, IReadOnlyList<RelicModel> relics)
		{
			uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
			RelicModel relicModel2;
			if (RelicSelectCmd.ShouldSelectLocalRelic(player))
			{
				NChooseARelicSelection nchooseARelicSelection = NChooseARelicSelection.ShowScreen(relics);
				if (LocalContext.IsMe(player))
				{
					foreach (RelicModel relicModel in relics)
					{
						SaveManager.Instance.MarkRelicAsSeen(relicModel);
					}
				}
				IEnumerable<RelicModel> enumerable = await nchooseARelicSelection.RelicsSelected();
				relicModel2 = enumerable.FirstOrDefault<RelicModel>();
				int num = relics.IndexOf(relicModel2);
				PlayerChoiceResult playerChoiceResult = PlayerChoiceResult.FromIndex(new int?(num));
				RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, playerChoiceResult);
			}
			else
			{
				int num2 = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndex();
				relicModel2 = ((num2 < 0) ? null : relics[num2]);
			}
			return relicModel2;
		}
	}
}
