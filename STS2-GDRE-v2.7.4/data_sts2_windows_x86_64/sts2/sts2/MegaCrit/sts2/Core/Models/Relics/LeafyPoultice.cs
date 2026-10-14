using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000578 RID: 1400
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LeafyPoultice : RelicModel
	{
		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06005216 RID: 21014 RVA: 0x0024243B File Offset: 0x0024063B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06005217 RID: 21015 RVA: 0x0024243E File Offset: 0x0024063E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new MaxHpVar(12m));
			}
		}

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06005218 RID: 21016 RVA: 0x00242451 File Offset: 0x00240651
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Transform, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x00242464 File Offset: 0x00240664
		public override async Task AfterObtained()
		{
			await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue, false);
			List<CardModel> list = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.Rarity == CardRarity.Basic).ToList<CardModel>();
			CardModel cardModel = list.FirstOrDefault<CardModel>((CardModel c) => c.Tags.Contains(CardTag.Strike));
			CardModel cardModel2 = list.FirstOrDefault<CardModel>((CardModel c) => c.Tags.Contains(CardTag.Defend));
			List<CardTransformation> list2 = new List<CardTransformation>();
			if (cardModel != null)
			{
				list2.Add(new CardTransformation(cardModel));
			}
			if (cardModel2 != null)
			{
				list2.Add(new CardTransformation(cardModel2));
			}
			await CardCmd.Transform(list2, base.Owner.PlayerRng.Transformations, CardPreviewStyle.HorizontalLayout);
		}
	}
}
