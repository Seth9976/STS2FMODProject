using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000525 RID: 1317
	public sealed class Crossbow : RelicModel
	{
		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06005009 RID: 20489 RVA: 0x0023E777 File Offset: 0x0023C977
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x0023E77C File Offset: 0x0023C97C
		[NullableContext(1)]
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				IReadOnlyList<CardModel> readOnlyList = (from c in base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
					where c.Type == CardType.Attack
					select c).ToList<CardModel>();
				if (readOnlyList.Count != 0)
				{
					base.Flash();
					List<CardModel> list = CardFactory.GetDistinctForCombat(base.Owner, readOnlyList, 1, base.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
					foreach (CardModel cardModel in list)
					{
						cardModel.SetToFreeThisTurn();
					}
					await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, base.Owner, CardPilePosition.Bottom);
				}
			}
		}
	}
}
