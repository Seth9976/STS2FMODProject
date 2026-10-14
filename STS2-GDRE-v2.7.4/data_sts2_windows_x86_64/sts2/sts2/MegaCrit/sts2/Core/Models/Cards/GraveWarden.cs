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
	// Token: 0x020009E9 RID: 2537
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GraveWarden : CardModel
	{
		// Token: 0x0600728B RID: 29323 RVA: 0x0028D92E File Offset: 0x0028BB2E
		public GraveWarden()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001E5D RID: 7773
		// (get) Token: 0x0600728C RID: 29324 RVA: 0x0028D93B File Offset: 0x0028BB3B
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E5E RID: 7774
		// (get) Token: 0x0600728D RID: 29325 RVA: 0x0028D93E File Offset: 0x0028BB3E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(8m, ValueProp.Move),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x17001E5F RID: 7775
		// (get) Token: 0x0600728E RID: 29326 RVA: 0x0028D963 File Offset: 0x0028BB63
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>(false));
			}
		}

		// Token: 0x0600728F RID: 29327 RVA: 0x0028D970 File Offset: 0x0028BB70
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			IReadOnlyList<CardPileAddResult> readOnlyList = await CardPileCmd.AddGeneratedCardsToCombat(Soul.Create(base.Owner, base.DynamicVars.Cards.IntValue, base.CombatState), PileType.Draw, base.Owner, CardPilePosition.Random);
			CardCmd.PreviewCardPileAdd(readOnlyList, 1.2f, CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x06007290 RID: 29328 RVA: 0x0028D9BB File Offset: 0x0028BBBB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
