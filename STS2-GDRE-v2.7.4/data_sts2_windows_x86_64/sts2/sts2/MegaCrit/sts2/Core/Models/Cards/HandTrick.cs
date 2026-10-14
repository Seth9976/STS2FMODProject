using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009F2 RID: 2546
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HandTrick : CardModel
	{
		// Token: 0x060072BA RID: 29370 RVA: 0x0028DE5F File Offset: 0x0028C05F
		public HandTrick()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E74 RID: 7796
		// (get) Token: 0x060072BB RID: 29371 RVA: 0x0028DE6C File Offset: 0x0028C06C
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E75 RID: 7797
		// (get) Token: 0x060072BC RID: 29372 RVA: 0x0028DE6F File Offset: 0x0028C06F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(7m, ValueProp.Move));
			}
		}

		// Token: 0x17001E76 RID: 7798
		// (get) Token: 0x060072BD RID: 29373 RVA: 0x0028DE82 File Offset: 0x0028C082
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Sly));
			}
		}

		// Token: 0x060072BE RID: 29374 RVA: 0x0028DE90 File Offset: 0x0028C090
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, cardSelectorPrefs, (CardModel card) => card.Type == CardType.Skill && !card.IsSlyThisTurn, this);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.ApplySingleTurnSly(cardModel);
			}
		}

		// Token: 0x060072BF RID: 29375 RVA: 0x0028DEE3 File Offset: 0x0028C0E3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
