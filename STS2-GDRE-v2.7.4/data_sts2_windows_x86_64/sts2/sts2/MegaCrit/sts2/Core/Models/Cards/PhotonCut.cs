using System;
using System.Collections.Generic;
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
	// Token: 0x02000A71 RID: 2673
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PhotonCut : CardModel
	{
		// Token: 0x06007569 RID: 30057 RVA: 0x00293854 File Offset: 0x00291A54
		public PhotonCut()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F90 RID: 8080
		// (get) Token: 0x0600756A RID: 30058 RVA: 0x00293861 File Offset: 0x00291A61
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(10m, ValueProp.Move),
					new CardsVar(1),
					new DynamicVar("PutBack", 1m)
				});
			}
		}

		// Token: 0x0600756B RID: 30059 RVA: 0x0029389C File Offset: 0x00291A9C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, "dagger_throw.mp3")
				.Execute(choiceContext);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, base.DynamicVars["PutBack"].IntValue);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, cardSelectorPrefs, null, this);
			await CardPileCmd.Add(enumerable, PileType.Draw, CardPilePosition.Top, null, false);
		}

		// Token: 0x0600756C RID: 30060 RVA: 0x002938EF File Offset: 0x00291AEF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}

		// Token: 0x040028AE RID: 10414
		private const string _putBackKey = "PutBack";
	}
}
