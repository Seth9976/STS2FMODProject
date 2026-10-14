using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000975 RID: 2421
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DarkShackles : CardModel
	{
		// Token: 0x06007004 RID: 28676 RVA: 0x002889AB File Offset: 0x00286BAB
		public DarkShackles()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D39 RID: 7481
		// (get) Token: 0x06007005 RID: 28677 RVA: 0x002889B8 File Offset: 0x00286BB8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("StrengthLoss", 9m));
			}
		}

		// Token: 0x17001D3A RID: 7482
		// (get) Token: 0x06007006 RID: 28678 RVA: 0x002889D0 File Offset: 0x00286BD0
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001D3B RID: 7483
		// (get) Token: 0x06007007 RID: 28679 RVA: 0x002889D8 File Offset: 0x00286BD8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06007008 RID: 28680 RVA: 0x002889F8 File Offset: 0x00286BF8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<DarkShacklesPower>(choiceContext, cardPlay.Target, base.DynamicVars["StrengthLoss"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007009 RID: 28681 RVA: 0x00288A4B File Offset: 0x00286C4B
		protected override void OnUpgrade()
		{
			base.DynamicVars["StrengthLoss"].UpgradeValueBy(6m);
		}

		// Token: 0x0400285C RID: 10332
		private const string _strengthLossKey = "StrengthLoss";
	}
}
