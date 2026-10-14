using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A1F RID: 2591
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KnifeTrap : CardModel
	{
		// Token: 0x060073A4 RID: 29604 RVA: 0x0028FC74 File Offset: 0x0028DE74
		public KnifeTrap()
			: base(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001ED1 RID: 7889
		// (get) Token: 0x060073A5 RID: 29605 RVA: 0x0028FC81 File Offset: 0x0028DE81
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(base.IsUpgraded));
			}
		}

		// Token: 0x17001ED2 RID: 7890
		// (get) Token: 0x060073A6 RID: 29606 RVA: 0x0028FC94 File Offset: 0x0028DE94
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(0m);
				array[1] = new CalculationExtraVar(1m);
				array[2] = new CalculatedVar("CalculatedShivs").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => PileType.Exhaust.GetPile(card.Owner).Cards.Count<CardModel>((CardModel c) => c.Tags.Contains(CardTag.Shiv)));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x060073A7 RID: 29607 RVA: 0x0028FCF8 File Offset: 0x0028DEF8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			IEnumerable<CardModel> enumerable = PileType.Exhaust.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.Tags.Contains(CardTag.Shiv)).ToList<CardModel>();
			bool flag = true;
			foreach (CardModel cardModel in enumerable)
			{
				if (base.IsUpgraded)
				{
					CardCmd.Upgrade(cardModel, CardPreviewStyle.None);
				}
				await CardCmd.AutoPlay(choiceContext, cardModel, cardPlay.Target, AutoPlayType.Default, false, !flag);
				flag = false;
			}
			IEnumerator<CardModel> enumerator = null;
		}

		// Token: 0x04002884 RID: 10372
		private const string _calculatedShivsKey = "CalculatedShivs";
	}
}
