using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MegaCrit.Sts2.Core.Models.CardPools
{
	// Token: 0x02000B50 RID: 2896
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EventCardPool : CardPoolModel
	{
		// Token: 0x170021A0 RID: 8608
		// (get) Token: 0x06007A60 RID: 31328 RVA: 0x0029DA9E File Offset: 0x0029BC9E
		public override string Title
		{
			get
			{
				return "event";
			}
		}

		// Token: 0x170021A1 RID: 8609
		// (get) Token: 0x06007A61 RID: 31329 RVA: 0x0029DAA5 File Offset: 0x0029BCA5
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x170021A2 RID: 8610
		// (get) Token: 0x06007A62 RID: 31330 RVA: 0x0029DAAC File Offset: 0x0029BCAC
		public override string CardFrameMaterialPath
		{
			get
			{
				return "card_frame_colorless";
			}
		}

		// Token: 0x170021A3 RID: 8611
		// (get) Token: 0x06007A63 RID: 31331 RVA: 0x0029DAB3 File Offset: 0x0029BCB3
		public override Color DeckEntryCardColor
		{
			get
			{
				return new Color("A3A3A3FF");
			}
		}

		// Token: 0x170021A4 RID: 8612
		// (get) Token: 0x06007A64 RID: 31332 RVA: 0x0029DABF File Offset: 0x0029BCBF
		public override bool IsColorless
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007A65 RID: 31333 RVA: 0x0029DAC4 File Offset: 0x0029BCC4
		protected override CardModel[] GenerateAllCards()
		{
			return new CardModel[]
			{
				ModelDb.Card<Abundance>(),
				ModelDb.Card<Apotheosis>(),
				ModelDb.Card<Apparition>(),
				ModelDb.Card<BrightestFlame>(),
				ModelDb.Card<ByrdSwoop>(),
				ModelDb.Card<Caltrops>(),
				ModelDb.Card<Clash>(),
				ModelDb.Card<Distraction>(),
				ModelDb.Card<DualWield>(),
				ModelDb.Card<Enlightenment>(),
				ModelDb.Card<Entrench>(),
				ModelDb.Card<Exterminate>(),
				ModelDb.Card<FeedingFrenzy>(),
				ModelDb.Card<HelloWorld>(),
				ModelDb.Card<MadScience>(),
				ModelDb.Card<Maul>(),
				ModelDb.Card<Metamorphosis>(),
				ModelDb.Card<NeowsFury>(),
				ModelDb.Card<Outmaneuver>(),
				ModelDb.Card<Peck>(),
				ModelDb.Card<Rebound>(),
				ModelDb.Card<Relax>(),
				ModelDb.Card<RipAndTear>(),
				ModelDb.Card<Squash>(),
				ModelDb.Card<Stack>(),
				ModelDb.Card<ToricToughness>(),
				ModelDb.Card<Wish>(),
				ModelDb.Card<Whistle>()
			};
		}
	}
}
