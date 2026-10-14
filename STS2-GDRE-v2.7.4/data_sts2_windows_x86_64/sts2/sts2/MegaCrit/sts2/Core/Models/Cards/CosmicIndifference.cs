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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000967 RID: 2407
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CosmicIndifference : CardModel
	{
		// Token: 0x06006FC2 RID: 28610 RVA: 0x002880BD File Offset: 0x002862BD
		public CosmicIndifference()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001D21 RID: 7457
		// (get) Token: 0x06006FC3 RID: 28611 RVA: 0x002880CA File Offset: 0x002862CA
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D22 RID: 7458
		// (get) Token: 0x06006FC4 RID: 28612 RVA: 0x002880CD File Offset: 0x002862CD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x06006FC5 RID: 28613 RVA: 0x002880E0 File Offset: 0x002862E0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
			CardPile pile = PileType.Discard.GetPile(base.Owner);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(base.Owner), base.Owner, cardSelectorPrefs);
			CardModel cardModel = enumerable.FirstOrDefault<CardModel>();
			bool flag = cardModel != null;
			if (flag)
			{
				CardPile pile2 = cardModel.Pile;
				PileType? pileType = ((pile2 != null) ? new PileType?(pile2.Type) : null);
				bool flag2;
				if (pileType != null)
				{
					PileType valueOrDefault = pileType.GetValueOrDefault();
					if (valueOrDefault == PileType.Draw || valueOrDefault == PileType.Discard)
					{
						flag2 = true;
						goto IL_0191;
					}
				}
				flag2 = false;
				IL_0191:
				flag = flag2;
			}
			if (flag)
			{
				await CardPileCmd.Add(cardModel, PileType.Draw, CardPilePosition.Top, null, false);
			}
		}

		// Token: 0x06006FC6 RID: 28614 RVA: 0x00288133 File Offset: 0x00286333
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}
	}
}
