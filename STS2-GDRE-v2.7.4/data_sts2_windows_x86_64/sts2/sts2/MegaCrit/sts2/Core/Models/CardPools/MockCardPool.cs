using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Models.CardPools
{
	// Token: 0x02000B52 RID: 2898
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockCardPool : CardPoolModel
	{
		// Token: 0x170021AB RID: 8619
		// (get) Token: 0x06007A70 RID: 31344 RVA: 0x0029DFE3 File Offset: 0x0029C1E3
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021AC RID: 8620
		// (get) Token: 0x06007A71 RID: 31345 RVA: 0x0029DFE6 File Offset: 0x0029C1E6
		public override string Title
		{
			get
			{
				return "test";
			}
		}

		// Token: 0x170021AD RID: 8621
		// (get) Token: 0x06007A72 RID: 31346 RVA: 0x0029DFED File Offset: 0x0029C1ED
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x170021AE RID: 8622
		// (get) Token: 0x06007A73 RID: 31347 RVA: 0x0029DFF4 File Offset: 0x0029C1F4
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_colorless";
			}
		}

		// Token: 0x170021AF RID: 8623
		// (get) Token: 0x06007A74 RID: 31348 RVA: 0x0029DFFB File Offset: 0x0029C1FB
		public override Color DeckEntryCardColor
		{
			get
			{
				return Colors.White;
			}
		}

		// Token: 0x170021B0 RID: 8624
		// (get) Token: 0x06007A75 RID: 31349 RVA: 0x0029E002 File Offset: 0x0029C202
		public override bool IsColorless
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007A76 RID: 31350 RVA: 0x0029E005 File Offset: 0x0029C205
		protected override CardModel[] GenerateAllCards()
		{
			List<CardModel> customCards = this._customCards;
			return ((customCards != null) ? customCards.ToArray() : null) ?? Array.Empty<CardModel>();
		}

		// Token: 0x06007A77 RID: 31351 RVA: 0x0029E022 File Offset: 0x0029C222
		protected override void DeepCloneFields()
		{
			base.DeepCloneFields();
			this._customCards = new List<CardModel>();
		}

		// Token: 0x06007A78 RID: 31352 RVA: 0x0029E035 File Offset: 0x0029C235
		public void Add(CardModel card)
		{
			base.AssertMutable();
			card.AssertCanonical();
			this._customCards.Add(card);
			base.InvalidateCardCache();
		}

		// Token: 0x06007A79 RID: 31353 RVA: 0x0029E058 File Offset: 0x0029C258
		public static MockCardPool Create(params CardModel[] cards)
		{
			MockCardPool mockCardPool = (MockCardPool)ModelDb.CardPool<MockCardPool>().ToMutable();
			foreach (CardModel cardModel in cards)
			{
				mockCardPool.Add(cardModel);
			}
			return mockCardPool;
		}

		// Token: 0x040028F2 RID: 10482
		[Nullable(new byte[] { 2, 1 })]
		private List<CardModel> _customCards;
	}
}
