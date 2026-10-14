using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200092A RID: 2346
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BoostAway : CardModel
	{
		// Token: 0x06006E8E RID: 28302 RVA: 0x002859D5 File Offset: 0x00283BD5
		public BoostAway()
			: base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001CA1 RID: 7329
		// (get) Token: 0x06006E8F RID: 28303 RVA: 0x002859E2 File Offset: 0x00283BE2
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CA2 RID: 7330
		// (get) Token: 0x06006E90 RID: 28304 RVA: 0x002859E5 File Offset: 0x00283BE5
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Dazed>(false));
			}
		}

		// Token: 0x17001CA3 RID: 7331
		// (get) Token: 0x06006E91 RID: 28305 RVA: 0x002859F2 File Offset: 0x00283BF2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x06006E92 RID: 28306 RVA: 0x00285A08 File Offset: 0x00283C08
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			CardModel cardModel = base.CombatState.CreateCard<Dazed>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Discard, base.Owner, CardPilePosition.Bottom);
			CardCmd.PreviewCardPileAdd(cardPileAddResult, 1.2f, CardPreviewStyle.HorizontalLayout);
			await Cmd.Wait(0.5f, false);
		}

		// Token: 0x06006E93 RID: 28307 RVA: 0x00285A53 File Offset: 0x00283C53
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
