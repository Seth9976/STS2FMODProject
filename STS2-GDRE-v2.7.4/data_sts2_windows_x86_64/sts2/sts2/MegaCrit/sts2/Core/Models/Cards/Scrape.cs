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
	// Token: 0x02000AAF RID: 2735
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Scrape : CardModel
	{
		// Token: 0x060076AF RID: 30383 RVA: 0x0029634C File Offset: 0x0029454C
		public Scrape()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002015 RID: 8213
		// (get) Token: 0x060076B0 RID: 30384 RVA: 0x00296359 File Offset: 0x00294559
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(7m, ValueProp.Move),
					new CardsVar(4)
				});
			}
		}

		// Token: 0x060076B1 RID: 30385 RVA: 0x00296380 File Offset: 0x00294580
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			IEnumerable<CardModel> enumerable = await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner, false);
			IEnumerable<CardModel> enumerable2 = enumerable.Where<CardModel>((CardModel c) => c.EnergyCost.GetWithModifiers(CostModifiers.All) != 0 || c.EnergyCost.CostsX);
			await CardCmd.Discard(choiceContext, enumerable2);
		}

		// Token: 0x060076B2 RID: 30386 RVA: 0x002963D3 File Offset: 0x002945D3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
