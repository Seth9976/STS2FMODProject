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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000641 RID: 1601
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CallOfTheVoidPower : PowerModel
	{
		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x06005722 RID: 22306 RVA: 0x0024C49E File Offset: 0x0024A69E
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x06005723 RID: 22307 RVA: 0x0024C4A1 File Offset: 0x0024A6A1
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x06005724 RID: 22308 RVA: 0x0024C4A4 File Offset: 0x0024A6A4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
			}
		}

		// Token: 0x06005725 RID: 22309 RVA: 0x0024C4B4 File Offset: 0x0024A6B4
		public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
		{
			if (player == base.Owner.Player)
			{
				IReadOnlyList<CardModel> readOnlyList = base.Owner.Player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where<CardModel>(delegate(CardModel c)
				{
					CardRarity rarity = c.Rarity;
					bool flag = rarity == CardRarity.Basic || rarity == CardRarity.Ancient;
					return !flag;
				}).ToList<CardModel>();
				if (readOnlyList.Count > 0)
				{
					CardModel[] array = new CardModel[base.Amount];
					Rng combatCardGeneration = base.Owner.Player.RunState.Rng.CombatCardGeneration;
					for (int i = 0; i < base.Amount; i++)
					{
						CardModel cardModel = CardFactory.GetDistinctForCombat(player, readOnlyList, 1, combatCardGeneration).First<CardModel>();
						array[i] = cardModel;
						CardCmd.ApplyKeyword(cardModel, new CardKeyword[] { CardKeyword.Ethereal });
					}
					base.Flash();
					await CardPileCmd.AddGeneratedCardsToCombat(array, PileType.Hand, base.Owner.Player, CardPilePosition.Bottom);
				}
			}
		}
	}
}
