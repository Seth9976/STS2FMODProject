using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020004FD RID: 1277
	public sealed class Bellows : RelicModel
	{
		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06004F0C RID: 20236 RVA: 0x0023C937 File Offset: 0x0023AB37
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x0023C93C File Offset: 0x0023AB3C
		[NullableContext(1)]
		public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (base.Owner.PlayerCombatState.TurnNumber > 1)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			CardCmd.Upgrade(PileType.Hand.GetPile(base.Owner).Cards, CardPreviewStyle.HorizontalLayout);
			return Task.CompletedTask;
		}
	}
}
