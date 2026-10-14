using System;
using System.Collections.Generic;
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
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005C6 RID: 1478
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RadiantPearl : RelicModel
	{
		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x06005430 RID: 21552 RVA: 0x0024619F File Offset: 0x0024439F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x06005431 RID: 21553 RVA: 0x002461A2 File Offset: 0x002443A2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));
			}
		}

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06005432 RID: 21554 RVA: 0x002461AF File Offset: 0x002443AF
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				List<IHoverTip> list = new List<IHoverTip>();
				list.Add(HoverTipFactory.ForEnergy(this));
				list.AddRange(HoverTipFactory.FromCardWithCardHoverTips<Luminesce>(false));
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x06005433 RID: 21555 RVA: 0x002461D4 File Offset: 0x002443D4
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner)
			{
				if (base.Owner.PlayerCombatState.TurnNumber == 1)
				{
					List<CardModel> list = new List<CardModel>();
					for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
					{
						list.Add(base.Owner.Creature.CombatState.CreateCard<Luminesce>(base.Owner));
					}
					await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, base.Owner, CardPilePosition.Bottom);
				}
			}
		}
	}
}
