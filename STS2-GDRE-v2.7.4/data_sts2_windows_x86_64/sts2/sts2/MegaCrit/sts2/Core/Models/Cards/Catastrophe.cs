using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000948 RID: 2376
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Catastrophe : CardModel
	{
		// Token: 0x06006F1D RID: 28445 RVA: 0x00286C2B File Offset: 0x00284E2B
		public Catastrophe()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001CD9 RID: 7385
		// (get) Token: 0x06006F1E RID: 28446 RVA: 0x00286C38 File Offset: 0x00284E38
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x06006F1F RID: 28447 RVA: 0x00286C48 File Offset: 0x00284E48
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
			{
				CardModel cardModel = PileType.Draw.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => !c.Keywords.Contains(CardKeyword.Unplayable)).ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.Shuffle)
					.FirstOrDefault<CardModel>();
				if (cardModel == null)
				{
					cardModel = PileType.Draw.GetPile(base.Owner).Cards.ToList<CardModel>().StableShuffle<CardModel>(base.Owner.RunState.Rng.Shuffle).FirstOrDefault<CardModel>();
				}
				if (cardModel != null)
				{
					await CardCmd.AutoPlay(choiceContext, cardModel, null, AutoPlayType.Default, false, false);
				}
			}
		}

		// Token: 0x06006F20 RID: 28448 RVA: 0x00286C93 File Offset: 0x00284E93
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
