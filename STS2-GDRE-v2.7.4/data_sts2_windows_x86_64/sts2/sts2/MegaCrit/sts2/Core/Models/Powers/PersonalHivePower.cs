using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006C7 RID: 1735
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PersonalHivePower : PowerModel
	{
		// Token: 0x1700132D RID: 4909
		// (get) Token: 0x06005A16 RID: 23062 RVA: 0x002518E7 File Offset: 0x0024FAE7
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x06005A17 RID: 23063 RVA: 0x002518EA File Offset: 0x0024FAEA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700132F RID: 4911
		// (get) Token: 0x06005A18 RID: 23064 RVA: 0x002518ED File Offset: 0x0024FAED
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Dazed>(false));
			}
		}

		// Token: 0x06005A19 RID: 23065 RVA: 0x002518FC File Offset: 0x0024FAFC
		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult _, ValueProp props, [Nullable(2)] Creature dealer, [Nullable(2)] CardModel cardSource)
		{
			if (target == base.Owner)
			{
				if (dealer != null)
				{
					if (props.IsPoweredAttack())
					{
						if (dealer.Monster is Osty)
						{
							dealer = dealer.PetOwner.Creature;
						}
						CardPileAddResult[] statusCards = new CardPileAddResult[base.Amount];
						for (int i = 0; i < base.Amount; i++)
						{
							CardModel cardModel = base.CombatState.CreateCard<Dazed>(dealer.Player);
							CardPileAddResult[] array = statusCards;
							int num = i;
							CardPileAddResult cardPileAddResult = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Draw, null, CardPilePosition.Random);
							array[num] = cardPileAddResult;
							array = null;
						}
						CardCmd.PreviewCardPileAdd(statusCards, 1.2f, CardPreviewStyle.HorizontalLayout);
						await Cmd.Wait(0.5f, false);
					}
				}
			}
		}
	}
}
