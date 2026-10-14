using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A02 RID: 2562
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HiddenGem : CardModel
	{
		// Token: 0x06007307 RID: 29447 RVA: 0x0028E93B File Offset: 0x0028CB3B
		public HiddenGem()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001E90 RID: 7824
		// (get) Token: 0x06007308 RID: 29448 RVA: 0x0028E948 File Offset: 0x0028CB48
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001E91 RID: 7825
		// (get) Token: 0x06007309 RID: 29449 RVA: 0x0028E94B File Offset: 0x0028CB4B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new IntVar("Replay", 2m));
			}
		}

		// Token: 0x17001E92 RID: 7826
		// (get) Token: 0x0600730A RID: 29450 RVA: 0x0028E962 File Offset: 0x0028CB62
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.ReplayStatic, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x0600730B RID: 29451 RVA: 0x0028E978 File Offset: 0x0028CB78
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			List<CardModel> list = PileType.Draw.GetPile(base.Owner).Cards.ToList<CardModel>();
			if (list.Count != 0)
			{
				List<CardModel> list2 = list.Where<CardModel>(delegate(CardModel c)
				{
					bool flag = !c.Keywords.Contains(CardKeyword.Unplayable);
					bool flag2 = flag;
					if (flag2)
					{
						CardType type = c.Type;
						bool flag3 = type - CardType.Curse <= 1;
						flag2 = !flag3;
					}
					return flag2 && c.GetEnchantedReplayCount() < 1;
				}).ToList<CardModel>();
				List<CardModel> list3 = list2.Where<CardModel>(delegate(CardModel c)
				{
					CardType type2 = c.Type;
					return type2 - CardType.Attack <= 2;
				}).ToList<CardModel>();
				IEnumerable<CardModel> enumerable;
				if (list3.Count != 0)
				{
					enumerable = list3;
				}
				else
				{
					enumerable = list2;
				}
				CardModel cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem<CardModel>(enumerable);
				if (cardModel != null)
				{
					cardModel.BaseReplayCount += base.DynamicVars["Replay"].IntValue;
					CardCmd.Preview(cardModel, 1.2f, CardPreviewStyle.HorizontalLayout);
				}
			}
		}

		// Token: 0x0600730C RID: 29452 RVA: 0x0028E9BB File Offset: 0x0028CBBB
		protected override void OnUpgrade()
		{
			base.DynamicVars["Replay"].UpgradeValueBy(1m);
		}

		// Token: 0x04002881 RID: 10369
		private const string _replayKey = "Replay";
	}
}
