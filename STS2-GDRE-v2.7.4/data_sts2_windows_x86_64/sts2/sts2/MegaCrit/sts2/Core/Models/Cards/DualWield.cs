using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009A0 RID: 2464
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DualWield : CardModel
	{
		// Token: 0x060070F3 RID: 28915 RVA: 0x0028A511 File Offset: 0x00288711
		public DualWield()
			: base(1, CardType.Skill, CardRarity.Event, TargetType.Self, true)
		{
		}

		// Token: 0x17001DAC RID: 7596
		// (get) Token: 0x060070F4 RID: 28916 RVA: 0x0028A51E File Offset: 0x0028871E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));
			}
		}

		// Token: 0x17001DAD RID: 7597
		// (get) Token: 0x060070F5 RID: 28917 RVA: 0x0028A52B File Offset: 0x0028872B
		public override CardPoolModel VisualCardPool
		{
			get
			{
				return ModelDb.CardPool<IroncladCardPool>();
			}
		}

		// Token: 0x060070F6 RID: 28918 RVA: 0x0028A534 File Offset: 0x00288734
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, cardSelectorPrefs, delegate(CardModel c)
			{
				CardType type = c.Type;
				return type == CardType.Attack || type == CardType.Power;
			}, this);
			IEnumerable<CardModel> enumerable2 = enumerable;
			CardModel selection = enumerable2.FirstOrDefault<CardModel>();
			if (selection != null)
			{
				for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
				{
					await CardPileCmd.AddGeneratedCardToCombat(selection.CreateClone(), PileType.Hand, base.Owner, CardPilePosition.Bottom);
				}
			}
		}

		// Token: 0x060070F7 RID: 28919 RVA: 0x0028A57F File Offset: 0x0028877F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
