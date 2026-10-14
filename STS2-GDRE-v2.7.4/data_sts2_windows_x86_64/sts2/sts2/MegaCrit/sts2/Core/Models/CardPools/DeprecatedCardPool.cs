using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.CardPools
{
	// Token: 0x02000B4E RID: 2894
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeprecatedCardPool : CardPoolModel
	{
		// Token: 0x17002195 RID: 8597
		// (get) Token: 0x06007A50 RID: 31312 RVA: 0x0029D997 File Offset: 0x0029BB97
		public override string Title
		{
			get
			{
				return "token";
			}
		}

		// Token: 0x17002196 RID: 8598
		// (get) Token: 0x06007A51 RID: 31313 RVA: 0x0029D99E File Offset: 0x0029BB9E
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x17002197 RID: 8599
		// (get) Token: 0x06007A52 RID: 31314 RVA: 0x0029D9A5 File Offset: 0x0029BBA5
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_colorless";
			}
		}

		// Token: 0x17002198 RID: 8600
		// (get) Token: 0x06007A53 RID: 31315 RVA: 0x0029D9AC File Offset: 0x0029BBAC
		public override Color DeckEntryCardColor
		{
			get
			{
				return Colors.White;
			}
		}

		// Token: 0x17002199 RID: 8601
		// (get) Token: 0x06007A54 RID: 31316 RVA: 0x0029D9B3 File Offset: 0x0029BBB3
		public override bool IsColorless
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007A55 RID: 31317 RVA: 0x0029D9B6 File Offset: 0x0029BBB6
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[] { ModelDb.Card<DeprecatedCard>() };
		}
	}
}
