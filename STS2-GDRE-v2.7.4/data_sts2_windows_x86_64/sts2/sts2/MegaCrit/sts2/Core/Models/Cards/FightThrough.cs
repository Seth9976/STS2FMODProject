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
	// Token: 0x020009C2 RID: 2498
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FightThrough : CardModel
	{
		// Token: 0x060071B1 RID: 29105 RVA: 0x0028BCCF File Offset: 0x00289ECF
		public FightThrough()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E03 RID: 7683
		// (get) Token: 0x060071B2 RID: 29106 RVA: 0x0028BCDC File Offset: 0x00289EDC
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E04 RID: 7684
		// (get) Token: 0x060071B3 RID: 29107 RVA: 0x0028BCDF File Offset: 0x00289EDF
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Wound>(false));
			}
		}

		// Token: 0x17001E05 RID: 7685
		// (get) Token: 0x060071B4 RID: 29108 RVA: 0x0028BCEC File Offset: 0x00289EEC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(13m, ValueProp.Move));
			}
		}

		// Token: 0x060071B5 RID: 29109 RVA: 0x0028BD00 File Offset: 0x00289F00
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			for (int i = 0; i < 2; i++)
			{
				CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(base.CombatState.CreateCard<Wound>(base.Owner), PileType.Discard, base.Owner, CardPilePosition.Bottom), 1.2f, CardPreviewStyle.HorizontalLayout);
			}
		}

		// Token: 0x060071B6 RID: 29110 RVA: 0x0028BD4B File Offset: 0x00289F4B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(4m);
		}
	}
}
