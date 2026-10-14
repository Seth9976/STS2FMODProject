using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.CardPools
{
	// Token: 0x02000B54 RID: 2900
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class QuestCardPool : CardPoolModel
	{
		// Token: 0x170021B7 RID: 8631
		// (get) Token: 0x06007A84 RID: 31364 RVA: 0x0029E4B3 File Offset: 0x0029C6B3
		public override string Title
		{
			get
			{
				return "quest";
			}
		}

		// Token: 0x170021B8 RID: 8632
		// (get) Token: 0x06007A85 RID: 31365 RVA: 0x0029E4BA File Offset: 0x0029C6BA
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x170021B9 RID: 8633
		// (get) Token: 0x06007A86 RID: 31366 RVA: 0x0029E4C1 File Offset: 0x0029C6C1
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_quest";
			}
		}

		// Token: 0x170021BA RID: 8634
		// (get) Token: 0x06007A87 RID: 31367 RVA: 0x0029E4C8 File Offset: 0x0029C6C8
		public override Color DeckEntryCardColor
		{
			get
			{
				return new Color("24476A");
			}
		}

		// Token: 0x170021BB RID: 8635
		// (get) Token: 0x06007A88 RID: 31368 RVA: 0x0029E4D4 File Offset: 0x0029C6D4
		public override Color EnergyOutlineColor
		{
			get
			{
				return new Color("431E14");
			}
		}

		// Token: 0x170021BC RID: 8636
		// (get) Token: 0x06007A89 RID: 31369 RVA: 0x0029E4E0 File Offset: 0x0029C6E0
		public override bool IsColorless
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007A8A RID: 31370 RVA: 0x0029E4E3 File Offset: 0x0029C6E3
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<ByrdonisEgg>(),
				ModelDb.Card<Dowsing>(),
				ModelDb.Card<LanternKey>(),
				ModelDb.Card<SpoilsMap>()
			};
		}
	}
}
