using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x0200085E RID: 2142
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheLegendsWereTrue : EventModel
	{
		// Token: 0x06006959 RID: 26969 RVA: 0x0027BFEC File Offset: 0x0027A1EC
		public override bool IsAllowed(IRunState runState)
		{
			if (runState.CurrentActIndex == 0)
			{
				if (runState.Players.All<Player>((Player p) => p.Deck.Cards.Count > 0))
				{
					return runState.Players.All<Player>((Player p) => p.Creature.CurrentHp >= 10);
				}
			}
			return false;
		}

		// Token: 0x170019F1 RID: 6641
		// (get) Token: 0x0600695A RID: 26970 RVA: 0x0027C059 File Offset: 0x0027A259
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(8m, ValueProp.Unblockable | ValueProp.Unpowered));
			}
		}

		// Token: 0x0600695B RID: 26971 RVA: 0x0027C06C File Offset: 0x0027A26C
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.NabTheMap), "THE_LEGENDS_WERE_TRUE.pages.INITIAL.options.NAB_THE_MAP", HoverTipFactory.FromCardWithCardHoverTips<SpoilsMap>(false)),
				new EventOption(this, new Func<Task>(this.SlowlyFindAnExit), "THE_LEGENDS_WERE_TRUE.pages.INITIAL.options.SLOWLY_FIND_AN_EXIT", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars.Damage.BaseValue)
			});
		}

		// Token: 0x0600695C RID: 26972 RVA: 0x0027C0D8 File Offset: 0x0027A2D8
		private async Task NabTheMap()
		{
			CardModel cardModel = base.Owner.RunState.CreateCard<SpoilsMap>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, null, false);
			CardPileAddResult cardPileAddResult2 = cardPileAddResult;
			CardCmd.PreviewCardPileAdd(cardPileAddResult2, 1.2f, CardPreviewStyle.HorizontalLayout);
			await Cmd.CustomScaledWait(0.5f, 1.2f, false, default(CancellationToken));
			base.SetEventFinished(base.L10NLookup("THE_LEGENDS_WERE_TRUE.pages.NAB_THE_MAP.description"));
		}

		// Token: 0x0600695D RID: 26973 RVA: 0x0027C11C File Offset: 0x0027A31C
		private async Task SlowlyFindAnExit()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Damage, null, null, null);
			IEnumerable<PotionModel> enumerable = base.Owner.Character.PotionPool.GetUnlockedPotions(base.Owner.UnlockState).Concat<PotionModel>(ModelDb.PotionPool<SharedPotionPool>().GetUnlockedPotions(base.Owner.UnlockState));
			PotionModel potionModel = base.Owner.PlayerRng.Rewards.NextItem<PotionModel>(enumerable);
			if (potionModel != null)
			{
				await RewardsCmd.OfferCustom(base.Owner, new List<Reward>(1)
				{
					new PotionReward(potionModel.ToMutable(), base.Owner)
				});
			}
			base.SetEventFinished(base.L10NLookup("THE_LEGENDS_WERE_TRUE.pages.SLOWLY_FIND_AN_EXIT.description"));
		}
	}
}
