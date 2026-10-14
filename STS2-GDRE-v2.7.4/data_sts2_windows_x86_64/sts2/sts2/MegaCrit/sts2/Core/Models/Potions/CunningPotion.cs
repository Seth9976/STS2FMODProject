using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Potions
{
	// Token: 0x0200074E RID: 1870
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CunningPotion : PotionModel
	{
		// Token: 0x170014E3 RID: 5347
		// (get) Token: 0x06005D72 RID: 23922 RVA: 0x00257C3B File Offset: 0x00255E3B
		public override PotionRarity Rarity
		{
			get
			{
				return PotionRarity.Uncommon;
			}
		}

		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x06005D73 RID: 23923 RVA: 0x00257C3E File Offset: 0x00255E3E
		public override PotionUsage Usage
		{
			get
			{
				return PotionUsage.CombatOnly;
			}
		}

		// Token: 0x170014E5 RID: 5349
		// (get) Token: 0x06005D74 RID: 23924 RVA: 0x00257C41 File Offset: 0x00255E41
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyPlayer;
			}
		}

		// Token: 0x170014E6 RID: 5350
		// (get) Token: 0x06005D75 RID: 23925 RVA: 0x00257C44 File Offset: 0x00255E44
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(3));
			}
		}

		// Token: 0x170014E7 RID: 5351
		// (get) Token: 0x06005D76 RID: 23926 RVA: 0x00257C51 File Offset: 0x00255E51
		public override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(true));
			}
		}

		// Token: 0x06005D77 RID: 23927 RVA: 0x00257C60 File Offset: 0x00255E60
		protected override async Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			PotionModel.AssertValidForTargetedPotion(target);
			Player player = target.Player;
			IEnumerable<CardModel> enumerable = await Shiv.CreateInHand(player, base.DynamicVars.Cards.IntValue, target.CombatState, base.Owner);
			IEnumerable<CardModel> enumerable2 = enumerable;
			foreach (CardModel cardModel in enumerable2)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
		}
	}
}
