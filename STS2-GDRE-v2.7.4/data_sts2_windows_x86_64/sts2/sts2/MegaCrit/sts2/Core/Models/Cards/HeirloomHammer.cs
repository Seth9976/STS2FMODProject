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
	// Token: 0x020009FA RID: 2554
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HeirloomHammer : CardModel
	{
		// Token: 0x060072E3 RID: 29411 RVA: 0x0028E3FC File Offset: 0x0028C5FC
		public HeirloomHammer()
			: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E84 RID: 7812
		// (get) Token: 0x060072E4 RID: 29412 RVA: 0x0028E409 File Offset: 0x0028C609
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(20m, ValueProp.Move),
					new RepeatVar(1)
				});
			}
		}

		// Token: 0x060072E5 RID: 29413 RVA: 0x0028E430 File Offset: 0x0028C630
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			CardSelectorPrefs cardSelectorPrefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromHand(choiceContext, base.Owner, cardSelectorPrefs, (CardModel c) => c.VisualCardPool.IsColorless, this);
			CardModel selection = enumerable.FirstOrDefault<CardModel>();
			if (selection != null)
			{
				for (int i = 0; i < base.DynamicVars.Repeat.IntValue; i++)
				{
					await CardPileCmd.AddGeneratedCardToCombat(selection.CreateClone(), PileType.Hand, base.Owner, CardPilePosition.Bottom);
				}
			}
		}

		// Token: 0x060072E6 RID: 29414 RVA: 0x0028E483 File Offset: 0x0028C683
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(5m);
		}
	}
}
