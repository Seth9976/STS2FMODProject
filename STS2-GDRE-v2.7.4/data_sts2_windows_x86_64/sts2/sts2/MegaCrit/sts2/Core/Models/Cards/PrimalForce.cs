using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A80 RID: 2688
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PrimalForce : CardModel
	{
		// Token: 0x060075B2 RID: 30130 RVA: 0x00294260 File Offset: 0x00292460
		public PrimalForce()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001FAA RID: 8106
		// (get) Token: 0x060075B3 RID: 30131 RVA: 0x0029426D File Offset: 0x0029246D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<GiantRock>(base.IsUpgraded));
			}
		}

		// Token: 0x060075B4 RID: 30132 RVA: 0x00294280 File Offset: 0x00292480
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			List<CardModel> list = PileType.Hand.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c != null && c.IsTransformable && c.Type == CardType.Attack).ToList<CardModel>();
			foreach (CardModel cardModel in list)
			{
				CardModel cardModel2 = base.CombatState.CreateCard<GiantRock>(base.Owner);
				if (base.IsUpgraded)
				{
					CardCmd.Upgrade(cardModel2, CardPreviewStyle.HorizontalLayout);
				}
				await CardCmd.Transform(cardModel, cardModel2, CardPreviewStyle.HorizontalLayout);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
		}
	}
}
