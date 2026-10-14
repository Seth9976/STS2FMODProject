using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200074D RID: 1869
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CosmicConcoction : PotionModel
	{
		// Token: 0x170014DF RID: 5343
		// (get) Token: 0x06005D6C RID: 23916 RVA: 0x00257BCF File Offset: 0x00255DCF
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Rare;
			}
		}

		// Token: 0x170014E0 RID: 5344
		// (get) Token: 0x06005D6D RID: 23917 RVA: 0x00257BD2 File Offset: 0x00255DD2
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014E1 RID: 5345
		// (get) Token: 0x06005D6E RID: 23918 RVA: 0x00257BD5 File Offset: 0x00255DD5
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014E2 RID: 5346
		// (get) Token: 0x06005D6F RID: 23919 RVA: 0x00257BD8 File Offset: 0x00255DD8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x06005D70 RID: 23920 RVA: 0x00257BE8 File Offset: 0x00255DE8
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			Player player = target.Player;
			IEnumerable<CardModel> distinctForCombat = CardFactory.GetDistinctForCombat(player, ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint), base.DynamicVars.Cards.IntValue, player.RunState.Rng.CombatCardGeneration);
			foreach (CardModel cardModel in distinctForCombat)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
				await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner, CardPilePosition.Bottom);
			}
			IEnumerator<CardModel> enumerator = null;
		}
	}
}
