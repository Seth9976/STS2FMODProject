using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A73 RID: 2675
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Pillage : CardModel
	{
		// Token: 0x06007573 RID: 30067 RVA: 0x002939D0 File Offset: 0x00291BD0
		public Pillage()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F94 RID: 8084
		// (get) Token: 0x06007574 RID: 30068 RVA: 0x002939DD File Offset: 0x00291BDD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x06007575 RID: 30069 RVA: 0x002939F0 File Offset: 0x00291BF0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			CardModel cardModel;
			do
			{
				cardModel = await CardPileCmd.Draw(choiceContext, base.Owner);
			}
			while (cardModel != null && cardModel.Type == CardType.Attack && CardPile.GetCards(base.Owner, new PileType[] { PileType.Hand }).Count<CardModel>() < CardPile.MaxCardsInHand);
		}

		// Token: 0x06007576 RID: 30070 RVA: 0x00293A43 File Offset: 0x00291C43
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
