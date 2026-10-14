using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000852 RID: 2130
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SpiritGrafter : EventModel
	{
		// Token: 0x060068D8 RID: 26840 RVA: 0x0027965C File Offset: 0x0027785C
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.LetItIn), "SPIRIT_GRAFTER.pages.INITIAL.options.LET_IT_IN", HoverTipFactory.FromCardWithCardHoverTips<Metamorphosis>(false)),
				new EventOption(this, new Func<Task>(this.Rejection), "SPIRIT_GRAFTER.pages.INITIAL.options.REJECTION", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars["RejectionHpLoss"].BaseValue)
			});
		}

		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x060068D9 RID: 26841 RVA: 0x002796CD File Offset: 0x002778CD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new HpLossVar("RejectionHpLoss", 10m),
					new HealVar("LetItInHealAmount", 25m)
				});
			}
		}

		// Token: 0x060068DA RID: 26842 RVA: 0x00279704 File Offset: 0x00277904
		private async Task LetItIn()
		{
			await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars["LetItInHealAmount"].BaseValue, true);
			CardModel cardModel = base.Owner.RunState.CreateCard<Metamorphosis>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardCmd.PreviewCardPileAdd(cardPileAddResult, 1.2f, CardPreviewStyle.HorizontalLayout);
			base.SetEventFinished(base.L10NLookup("SPIRIT_GRAFTER.pages.LET_IT_IN.description"));
		}

		// Token: 0x060068DB RID: 26843 RVA: 0x00279748 File Offset: 0x00277948
		private async Task Rejection()
		{
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForUpgrade(base.Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, 1));
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
			}
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["RejectionHpLoss"].BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
			base.SetEventFinished(base.L10NLookup("SPIRIT_GRAFTER.pages.REJECTION.description"));
		}

		// Token: 0x040027B7 RID: 10167
		private const string _rejectionHpLossKey = "RejectionHpLoss";

		// Token: 0x040027B8 RID: 10168
		private const string _letItInHealAmountKey = "LetItInHealAmount";
	}
}
