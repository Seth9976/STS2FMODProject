using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.CardPools
{
	// Token: 0x02000B4C RID: 2892
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CurseCardPool : CardPoolModel
	{
		// Token: 0x1700218A RID: 8586
		// (get) Token: 0x06007A40 RID: 31296 RVA: 0x0029D4A3 File Offset: 0x0029B6A3
		public override string Title
		{
			get
			{
				return "curse";
			}
		}

		// Token: 0x1700218B RID: 8587
		// (get) Token: 0x06007A41 RID: 31297 RVA: 0x0029D4AA File Offset: 0x0029B6AA
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x1700218C RID: 8588
		// (get) Token: 0x06007A42 RID: 31298 RVA: 0x0029D4B1 File Offset: 0x0029B6B1
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_curse";
			}
		}

		// Token: 0x1700218D RID: 8589
		// (get) Token: 0x06007A43 RID: 31299 RVA: 0x0029D4B8 File Offset: 0x0029B6B8
		public override Color DeckEntryCardColor
		{
			get
			{
				return new Color("585B61FF");
			}
		}

		// Token: 0x1700218E RID: 8590
		// (get) Token: 0x06007A44 RID: 31300 RVA: 0x0029D4C4 File Offset: 0x0029B6C4
		public override bool IsColorless
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007A45 RID: 31301 RVA: 0x0029D4C8 File Offset: 0x0029B6C8
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<AscendersBane>(),
				ModelDb.Card<BadLuck>(),
				ModelDb.Card<Clumsy>(),
				ModelDb.Card<CurseOfTheBell>(),
				ModelDb.Card<Debt>(),
				ModelDb.Card<Decay>(),
				ModelDb.Card<Doubt>(),
				ModelDb.Card<Enthralled>(),
				ModelDb.Card<Folly>(),
				ModelDb.Card<Greed>(),
				ModelDb.Card<Guilty>(),
				ModelDb.Card<Injury>(),
				ModelDb.Card<Normality>(),
				ModelDb.Card<PoorSleep>(),
				ModelDb.Card<Regret>(),
				ModelDb.Card<Shame>(),
				ModelDb.Card<SporeMind>(),
				ModelDb.Card<Writhe>()
			};
		}
	}
}
