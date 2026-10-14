using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.CardPools
{
	// Token: 0x02000B57 RID: 2903
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StatusCardPool : CardPoolModel
	{
		// Token: 0x170021C9 RID: 8649
		// (get) Token: 0x06007A9E RID: 31390 RVA: 0x0029ED43 File Offset: 0x0029CF43
		public override string Title
		{
			get
			{
				return "status";
			}
		}

		// Token: 0x170021CA RID: 8650
		// (get) Token: 0x06007A9F RID: 31391 RVA: 0x0029ED4A File Offset: 0x0029CF4A
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x170021CB RID: 8651
		// (get) Token: 0x06007AA0 RID: 31392 RVA: 0x0029ED51 File Offset: 0x0029CF51
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_colorless";
			}
		}

		// Token: 0x170021CC RID: 8652
		// (get) Token: 0x06007AA1 RID: 31393 RVA: 0x0029ED58 File Offset: 0x0029CF58
		public override Color DeckEntryCardColor
		{
			get
			{
				return Colors.White;
			}
		}

		// Token: 0x170021CD RID: 8653
		// (get) Token: 0x06007AA2 RID: 31394 RVA: 0x0029ED5F File Offset: 0x0029CF5F
		public override bool IsColorless
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007AA3 RID: 31395 RVA: 0x0029ED64 File Offset: 0x0029CF64
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<Beckon>(),
				ModelDb.Card<Burn>(),
				ModelDb.Card<Dazed>(),
				ModelDb.Card<Debris>(),
				ModelDb.Card<FranticEscape>(),
				ModelDb.Card<Infection>(),
				ModelDb.Card<Wither>(),
				ModelDb.Card<Slimed>(),
				ModelDb.Card<Soot>(),
				ModelDb.Card<Toxic>(),
				ModelDb.Card<Void>(),
				ModelDb.Card<Wound>()
			};
		}
	}
}
