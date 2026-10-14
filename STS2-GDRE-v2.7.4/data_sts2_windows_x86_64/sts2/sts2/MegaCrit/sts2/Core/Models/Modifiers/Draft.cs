using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Modifiers
{
	// Token: 0x02000819 RID: 2073
	[NullableContext(1)]
	[Nullable(0)]
	public class Draft : ModifierModel
	{
		// Token: 0x1700195C RID: 6492
		// (get) Token: 0x06006738 RID: 26424 RVA: 0x0027201B File Offset: 0x0027021B
		public override bool ClearsPlayerDeck
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006739 RID: 26425 RVA: 0x00272020 File Offset: 0x00270220
		public override Func<Task> GenerateNeowOption(EventModel eventModel)
		{
			return () => Draft.OfferRewards(eventModel.Owner);
		}

		// Token: 0x0600673A RID: 26426 RVA: 0x00272048 File Offset: 0x00270248
		private static async Task OfferRewards(Player player)
		{
			CardCreationOptions creationOptions = new CardCreationOptions(new <>z__ReadOnlySingleElementList<CardPoolModel>(player.Character.CardPool), CardCreationSource.Other, CardRarityOddsType.RegularEncounter, null).WithFlags(CardCreationFlags.NoUpgradeRoll);
			for (int i = 0; i < 10; i++)
			{
				CardReward cardReward = new CardReward(creationOptions, 3, player, null)
				{
					CanSkip = false
				};
				cardReward.Populate();
				await cardReward.SelectUnsynchronized();
			}
			foreach (Player player2 in player.RunState.Players)
			{
				player2.RelicGrabBag.Remove<PandorasBox>();
			}
			player.RunState.SharedRelicGrabBag.Remove<PandorasBox>();
		}
	}
}
