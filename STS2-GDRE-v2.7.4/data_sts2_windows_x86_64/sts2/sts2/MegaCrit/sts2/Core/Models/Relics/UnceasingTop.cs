using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000607 RID: 1543
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UnceasingTop : RelicModel
	{
		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x060055D2 RID: 21970 RVA: 0x002494FB File Offset: 0x002476FB
		public override string FlashSfx
		{
			get
			{
				return "event:/sfx/ui/relic_activate_draw";
			}
		}

		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x060055D3 RID: 21971 RVA: 0x00249502 File Offset: 0x00247702
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x060055D4 RID: 21972 RVA: 0x00249508 File Offset: 0x00247708
		public override async Task AfterHandEmptied(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner)
			{
				if (UnceasingTop.IsValidPhase(player.PlayerCombatState.Phase))
				{
					base.Flash();
					await CardPileCmd.Draw(choiceContext, player);
				}
			}
		}

		/// <remarks>
		/// Don't trigger before hand draw or after hand flush, because if a card is autoplayed during this time, the
		/// player's hand will always be empty, so Unceasing Top will always draw.
		/// </remarks>
		// Token: 0x060055D5 RID: 21973 RVA: 0x0024955C File Offset: 0x0024775C
		private static bool IsValidPhase(PlayerTurnPhase phase)
		{
			return phase - PlayerTurnPhase.AutoPrePlay <= 2;
		}
	}
}
