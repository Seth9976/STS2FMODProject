using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AA1 RID: 2721
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Restlessness : CardModel
	{
		// Token: 0x06007661 RID: 30305 RVA: 0x00295963 File Offset: 0x00293B63
		public Restlessness()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001FF3 RID: 8179
		// (get) Token: 0x06007662 RID: 30306 RVA: 0x00295970 File Offset: 0x00293B70
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new CardsVar(2),
					new EnergyVar(2)
				});
			}
		}

		// Token: 0x17001FF4 RID: 8180
		// (get) Token: 0x06007663 RID: 30307 RVA: 0x0029598F File Offset: 0x00293B8F
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x17001FF5 RID: 8181
		// (get) Token: 0x06007664 RID: 30308 RVA: 0x0029599C File Offset: 0x00293B9C
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);
			}
		}

		// Token: 0x17001FF6 RID: 8182
		// (get) Token: 0x06007665 RID: 30309 RVA: 0x002959A4 File Offset: 0x00293BA4
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return this.IsOnlyCardInHand;
			}
		}

		// Token: 0x06007666 RID: 30310 RVA: 0x002959AC File Offset: 0x00293BAC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (this.IsOnlyCardInHand)
			{
				for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
				{
					await CardPileCmd.Draw(choiceContext, base.Owner);
				}
				await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
			}
		}

		// Token: 0x06007667 RID: 30311 RVA: 0x002959F7 File Offset: 0x00293BF7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}

		// Token: 0x17001FF7 RID: 8183
		// (get) Token: 0x06007668 RID: 30312 RVA: 0x00295A23 File Offset: 0x00293C23
		private bool IsOnlyCardInHand
		{
			get
			{
				return !PileType.Hand.GetPile(base.Owner).Cards.Except<CardModel>(new <>z__ReadOnlySingleElementList<CardModel>(this)).Any<CardModel>();
			}
		}
	}
}
