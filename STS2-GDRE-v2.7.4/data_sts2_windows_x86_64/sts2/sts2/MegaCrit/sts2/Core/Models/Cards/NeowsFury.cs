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
	// Token: 0x02000A50 RID: 2640
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NeowsFury : CardModel
	{
		// Token: 0x060074BB RID: 29883 RVA: 0x002921F0 File Offset: 0x002903F0
		public NeowsFury()
			: base(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F46 RID: 8006
		// (get) Token: 0x060074BC RID: 29884 RVA: 0x002921FD File Offset: 0x002903FD
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001F47 RID: 8007
		// (get) Token: 0x060074BD RID: 29885 RVA: 0x00292200 File Offset: 0x00290400
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001F48 RID: 8008
		// (get) Token: 0x060074BE RID: 29886 RVA: 0x00292208 File Offset: 0x00290408
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(10m, ValueProp.Move),
					new CardsVar(2)
				});
			}
		}

		// Token: 0x060074BF RID: 29887 RVA: 0x00292230 File Offset: 0x00290430
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			int num = Math.Min(base.DynamicVars.Cards.IntValue, CardPile.MaxCardsInHand - PileType.Hand.GetPile(base.Owner).Cards.Count);
			if (num > 0)
			{
				await CardPileCmd.Add(await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(base.Owner), base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, 0, num)), PileType.Hand, CardPilePosition.Bottom, null, false);
			}
		}

		// Token: 0x060074C0 RID: 29888 RVA: 0x00292283 File Offset: 0x00290483
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
