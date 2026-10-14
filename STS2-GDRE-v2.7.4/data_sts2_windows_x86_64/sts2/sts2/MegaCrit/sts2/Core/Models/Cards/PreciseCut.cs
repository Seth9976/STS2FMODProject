using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A7C RID: 2684
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PreciseCut : CardModel
	{
		// Token: 0x060075A1 RID: 30113 RVA: 0x00293FCF File Offset: 0x002921CF
		public PreciseCut()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001FA5 RID: 8101
		// (get) Token: 0x060075A2 RID: 30114 RVA: 0x00293FDC File Offset: 0x002921DC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(13m);
				array[1] = new ExtraDamageVar(2m);
				array[2] = new CalculatedDamageVar(ValueProp.Move).WithMultiplier(delegate(CardModel card, [Nullable(2)] Creature _)
				{
					int num = PileType.Hand.GetPile(card.Owner).Cards.Count;
					CardPile pile = card.Pile;
					if (pile != null && pile.Type == PileType.Hand)
					{
						num--;
					}
					return -num;
				});
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x060075A3 RID: 30115 RVA: 0x00294040 File Offset: 0x00292240
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, "dagger_throw.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x060075A4 RID: 30116 RVA: 0x00294093 File Offset: 0x00292293
		protected override void OnUpgrade()
		{
			base.DynamicVars.CalculationBase.UpgradeValueBy(3m);
		}
	}
}
