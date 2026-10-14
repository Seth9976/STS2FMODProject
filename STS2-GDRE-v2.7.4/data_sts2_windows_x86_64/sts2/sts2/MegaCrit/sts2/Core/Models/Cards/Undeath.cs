using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B27 RID: 2855
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Undeath : CardModel
	{
		// Token: 0x06007940 RID: 31040 RVA: 0x0029B6FF File Offset: 0x002998FF
		public Undeath()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x1700211D RID: 8477
		// (get) Token: 0x06007941 RID: 31041 RVA: 0x0029B70C File Offset: 0x0029990C
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700211E RID: 8478
		// (get) Token: 0x06007942 RID: 31042 RVA: 0x0029B70F File Offset: 0x0029990F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(7m, ValueProp.Move));
			}
		}

		// Token: 0x06007943 RID: 31043 RVA: 0x0029B724 File Offset: 0x00299924
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			CardModel cardModel = base.CreateClone();
			CardPileAddResult cardPileAddResult = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Discard, base.Owner, CardPilePosition.Bottom);
			CardCmd.PreviewCardPileAdd(cardPileAddResult, 2.2f, CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x06007944 RID: 31044 RVA: 0x0029B76F File Offset: 0x0029996F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
		}
	}
}
