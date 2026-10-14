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
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005BE RID: 1470
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PowerCell : RelicModel
	{
		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06005401 RID: 21505 RVA: 0x00245D47 File Offset: 0x00243F47
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Rare;
			}
		}

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x06005402 RID: 21506 RVA: 0x00245D4A File Offset: 0x00243F4A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x00245D58 File Offset: 0x00243F58
		public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					base.Flash();
					IEnumerable<CardModel> enumerable = PileType.Draw.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => !c.EnergyCost.CostsX && c.EnergyCost.GetWithModifiers(CostModifiers.Local) == 0).ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.CombatCardSelection)
						.Take<CardModel>(base.DynamicVars.Cards.IntValue);
					await CardPileCmd.Add(enumerable, PileType.Hand, CardPilePosition.Bottom, null, false);
				}
			}
		}
	}
}
