using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000869 RID: 2153
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WhisperingHollow : EventModel
	{
		// Token: 0x060069C6 RID: 27078 RVA: 0x0027E2F7 File Offset: 0x0027C4F7
		public override bool IsAllowed(IRunState runState)
		{
			return runState.Players.All<Player>((Player p) => p.Gold >= 44);
		}

		// Token: 0x17001A08 RID: 6664
		// (get) Token: 0x060069C7 RID: 27079 RVA: 0x0027E323 File Offset: 0x0027C523
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new GoldVar(35),
					new HpLossVar(9m)
				});
			}
		}

		// Token: 0x060069C8 RID: 27080 RVA: 0x0027E349 File Offset: 0x0027C549
		public override void CalculateVars()
		{
			base.DynamicVars.Gold.BaseValue += base.Rng.NextInt(-9, 10);
		}

		// Token: 0x060069C9 RID: 27081 RVA: 0x0027E37C File Offset: 0x0027C57C
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Gold), "WHISPERING_HOLLOW.pages.INITIAL.options.GOLD", Array.Empty<IHoverTip>()),
				new EventOption(this, new Func<Task>(this.Hug), "WHISPERING_HOLLOW.pages.INITIAL.options.HUG", new IHoverTip[] { HoverTipFactory.Static(StaticHoverTip.Transform, Array.Empty<DynamicVar>()) }).ThatDoesDamage(base.DynamicVars.HpLoss.IntValue)
			});
		}

		// Token: 0x060069CA RID: 27082 RVA: 0x0027E3FC File Offset: 0x0027C5FC
		private async Task Gold()
		{
			await PlayerCmd.LoseGold(base.DynamicVars.Gold.IntValue, base.Owner, GoldLossType.Spent);
			await RewardsCmd.OfferCustom(base.Owner, new List<Reward>(2)
			{
				new PotionReward(base.Owner),
				new PotionReward(base.Owner)
			});
			base.SetEventFinished(base.L10NLookup("WHISPERING_HOLLOW.pages.GOLD.description"));
		}

		// Token: 0x060069CB RID: 27083 RVA: 0x0027E440 File Offset: 0x0027C640
		private async Task Hug()
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromDeckForTransformation(base.Owner, cardSelectorPrefs, null);
			List<CardModel> list = enumerable.ToList<CardModel>();
			foreach (CardModel cardModel in list)
			{
				await CardCmd.TransformToRandom(cardModel, base.Rng, CardPreviewStyle.EventLayout);
			}
			List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null, null);
			base.SetEventFinished(base.L10NLookup("WHISPERING_HOLLOW.pages.HUG.description"));
		}

		// Token: 0x040027F5 RID: 10229
		private const int _baseGold = 35;

		// Token: 0x040027F6 RID: 10230
		private const int _goldVariance = 9;
	}
}
