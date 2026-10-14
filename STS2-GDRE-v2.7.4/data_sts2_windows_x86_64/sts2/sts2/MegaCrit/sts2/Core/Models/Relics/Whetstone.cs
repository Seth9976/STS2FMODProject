using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000614 RID: 1556
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Whetstone : RelicModel
	{
		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x06005628 RID: 22056 RVA: 0x00249FCC File Offset: 0x002481CC
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x06005629 RID: 22057 RVA: 0x00249FCF File Offset: 0x002481CF
		public override bool HasUponPickupEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x0600562A RID: 22058 RVA: 0x00249FD2 File Offset: 0x002481D2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x00249FE0 File Offset: 0x002481E0
		public override Task AfterObtained()
		{
			IEnumerable<CardModel> enumerable = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c != null && c.Type == CardType.Attack && c.IsUpgradable).ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.Niche)
				.Take<CardModel>(base.DynamicVars.Cards.IntValue);
			foreach (CardModel cardModel in enumerable)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
			return Task.CompletedTask;
		}
	}
}
