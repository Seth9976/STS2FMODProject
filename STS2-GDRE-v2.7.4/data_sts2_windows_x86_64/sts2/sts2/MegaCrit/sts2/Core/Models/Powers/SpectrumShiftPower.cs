using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006FB RID: 1787
	public sealed class SpectrumShiftPower : PowerModel
	{
		// Token: 0x170013CA RID: 5066
		// (get) Token: 0x06005B62 RID: 23394 RVA: 0x0025425B File Offset: 0x0025245B
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170013CB RID: 5067
		// (get) Token: 0x06005B63 RID: 23395 RVA: 0x0025425E File Offset: 0x0025245E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005B64 RID: 23396 RVA: 0x00254264 File Offset: 0x00252464
		[NullableContext(1)]
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner.Player)
			{
				List<CardModel> list = CardFactory.GetDistinctForCombat(player, ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint), base.Amount, player.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
				await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, base.Owner.Player, CardPilePosition.Bottom);
				base.Flash();
			}
		}
	}
}
