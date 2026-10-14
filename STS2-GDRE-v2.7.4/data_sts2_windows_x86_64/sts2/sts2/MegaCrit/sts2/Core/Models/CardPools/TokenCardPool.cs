using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.CardPools
{
	// Token: 0x02000B58 RID: 2904
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TokenCardPool : CardPoolModel
	{
		// Token: 0x170021CE RID: 8654
		// (get) Token: 0x06007AA5 RID: 31397 RVA: 0x0029EDE3 File Offset: 0x0029CFE3
		public override string Title
		{
			get
			{
				return "token";
			}
		}

		// Token: 0x170021CF RID: 8655
		// (get) Token: 0x06007AA6 RID: 31398 RVA: 0x0029EDEA File Offset: 0x0029CFEA
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x170021D0 RID: 8656
		// (get) Token: 0x06007AA7 RID: 31399 RVA: 0x0029EDF1 File Offset: 0x0029CFF1
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_colorless";
			}
		}

		// Token: 0x170021D1 RID: 8657
		// (get) Token: 0x06007AA8 RID: 31400 RVA: 0x0029EDF8 File Offset: 0x0029CFF8
		public override Color DeckEntryCardColor
		{
			get
			{
				return Colors.White;
			}
		}

		// Token: 0x170021D2 RID: 8658
		// (get) Token: 0x06007AA9 RID: 31401 RVA: 0x0029EDFF File Offset: 0x0029CFFF
		public override bool IsColorless
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007AAA RID: 31402 RVA: 0x0029EE04 File Offset: 0x0029D004
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<Disintegration>(),
				ModelDb.Card<Fuel>(),
				ModelDb.Card<GiantRock>(),
				ModelDb.Card<Luminesce>(),
				ModelDb.Card<MindRot>(),
				ModelDb.Card<MinionDiveBomb>(),
				ModelDb.Card<MinionSacrifice>(),
				ModelDb.Card<MinionStrike>(),
				ModelDb.Card<Shiv>(),
				ModelDb.Card<Sloth>(),
				ModelDb.Card<Soul>(),
				ModelDb.Card<SovereignBlade>(),
				ModelDb.Card<SweepingGaze>(),
				ModelDb.Card<WasteAway>()
			};
		}
	}
}
