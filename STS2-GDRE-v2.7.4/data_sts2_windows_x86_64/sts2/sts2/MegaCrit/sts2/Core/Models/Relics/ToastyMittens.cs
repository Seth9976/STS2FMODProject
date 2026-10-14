using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005FD RID: 1533
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ToastyMittens : RelicModel
	{
		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x06005582 RID: 21890 RVA: 0x00248AF8 File Offset: 0x00246CF8
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x06005583 RID: 21891 RVA: 0x00248AFB File Offset: 0x00246CFB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<StrengthPower>(1m));
			}
		}

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x06005584 RID: 21892 RVA: 0x00248B0C File Offset: 0x00246D0C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
					HoverTipFactory.FromPower<StrengthPower>(null)
				});
			}
		}

		// Token: 0x06005585 RID: 21893 RVA: 0x00248B40 File Offset: 0x00246D40
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner.Creature.Player)
			{
				base.Flash();
				await CardPileCmd.ShuffleIfNecessary(choiceContext, base.Owner);
				IReadOnlyList<CardModel> cards = PileType.Draw.GetPile(player).Cards;
				CardModel cardModel = null;
				if (base.Owner.PlayerCombatState.TurnNumber == 1)
				{
					cardModel = cards.FirstOrDefault<CardModel>((CardModel c) => !c.Keywords.Contains(CardKeyword.Innate));
				}
				if (cardModel == null)
				{
					cardModel = cards.FirstOrDefault<CardModel>();
				}
				if (cardModel != null)
				{
					await CardCmd.Exhaust(choiceContext, cardModel, false, false);
				}
				await PowerCmd.Apply<StrengthPower>(choiceContext, player.Creature, base.DynamicVars.Strength.BaseValue, player.Creature, null, false);
			}
		}
	}
}
