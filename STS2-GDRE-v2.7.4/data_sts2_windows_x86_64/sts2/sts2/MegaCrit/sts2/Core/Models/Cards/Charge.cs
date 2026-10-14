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
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200094B RID: 2379
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Charge : CardModel
	{
		// Token: 0x06006F2A RID: 28458 RVA: 0x00286DD6 File Offset: 0x00284FD6
		public Charge()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CDD RID: 7389
		// (get) Token: 0x06006F2B RID: 28459 RVA: 0x00286DE3 File Offset: 0x00284FE3
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<MinionDiveBomb>(base.IsUpgraded));
			}
		}

		// Token: 0x17001CDE RID: 7390
		// (get) Token: 0x06006F2C RID: 28460 RVA: 0x00286DF5 File Offset: 0x00284FF5
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x06006F2D RID: 28461 RVA: 0x00286E04 File Offset: 0x00285004
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(base.Owner), base.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, base.DynamicVars.Cards.IntValue));
			List<CardModel> selection = enumerable.ToList<CardModel>();
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			foreach (CardModel cardModel in selection)
			{
				CardPileAddResult? cardPileAddResult = await CardCmd.TransformTo<MinionDiveBomb>(cardModel, CardPreviewStyle.HorizontalLayout);
				if (base.IsUpgraded && cardPileAddResult != null)
				{
					CardCmd.Upgrade(cardPileAddResult.Value.cardAdded, CardPreviewStyle.HorizontalLayout);
				}
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
		}
	}
}
