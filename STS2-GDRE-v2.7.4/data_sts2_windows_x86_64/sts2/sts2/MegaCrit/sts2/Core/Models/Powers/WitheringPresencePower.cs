using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000730 RID: 1840
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WitheringPresencePower : PowerModel
	{
		// Token: 0x17001471 RID: 5233
		// (get) Token: 0x06005CB0 RID: 23728 RVA: 0x00256EFC File Offset: 0x002550FC
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001472 RID: 5234
		// (get) Token: 0x06005CB1 RID: 23729 RVA: 0x00256EFF File Offset: 0x002550FF
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001473 RID: 5235
		// (get) Token: 0x06005CB2 RID: 23730 RVA: 0x00256F02 File Offset: 0x00255102
		public override int DisplayAmount
		{
			get
			{
				return base.DynamicVars["CardsLeft"].IntValue;
			}
		}

		// Token: 0x17001474 RID: 5236
		// (get) Token: 0x06005CB3 RID: 23731 RVA: 0x00256F19 File Offset: 0x00255119
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x17001475 RID: 5237
		// (get) Token: 0x06005CB4 RID: 23732 RVA: 0x00256F1C File Offset: 0x0025511C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("CardsLeft", 6m));
			}
		}

		// Token: 0x17001476 RID: 5238
		// (get) Token: 0x06005CB5 RID: 23733 RVA: 0x00256F34 File Offset: 0x00255134
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				Wither wither = (Wither)ModelDb.Card<Wither>().ToMutable();
				Aeonglass aeonglass = base.Owner.Monster as Aeonglass;
				if (aeonglass != null)
				{
					aeonglass.MatchWitherToUpgradeCount(wither);
				}
				List<IHoverTip> list = new List<IHoverTip>();
				list.Add(HoverTipFactory.FromCard(wither, false));
				list.AddRange(ModelDb.Card<Wither>().HoverTips);
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x06005CB6 RID: 23734 RVA: 0x00256F94 File Offset: 0x00255194
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Target.Player)
			{
				DynamicVar dynamicVar = base.DynamicVars["CardsLeft"];
				decimal baseValue = dynamicVar.BaseValue;
				dynamicVar.BaseValue = baseValue - 1m;
				base.InvokeDisplayAmountChanged();
				if (base.DynamicVars["CardsLeft"].IntValue <= 0)
				{
					await Cmd.Wait(0.5f, false);
					await CardPileCmd.AddToCombatAndPreview<Wither>(cardPlay.Card.Owner.Creature, PileType.Hand, 1, null, CardPilePosition.Bottom);
					base.Flash();
					base.DynamicVars["CardsLeft"].BaseValue = 6m;
					base.InvokeDisplayAmountChanged();
				}
			}
		}

		// Token: 0x04002528 RID: 9512
		private const int _baseCardsLeft = 6;

		// Token: 0x04002529 RID: 9513
		private const string _cardsLeftKey = "CardsLeft";
	}
}
