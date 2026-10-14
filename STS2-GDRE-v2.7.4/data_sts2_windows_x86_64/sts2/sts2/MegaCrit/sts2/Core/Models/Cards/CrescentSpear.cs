using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x0200096B RID: 2411
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CrescentSpear : CardModel
	{
		// Token: 0x06006FD5 RID: 28629 RVA: 0x0028830D File Offset: 0x0028650D
		public CrescentSpear()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D28 RID: 7464
		// (get) Token: 0x06006FD6 RID: 28630 RVA: 0x0028831A File Offset: 0x0028651A
		public override int CanonicalStarCost
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001D29 RID: 7465
		// (get) Token: 0x06006FD7 RID: 28631 RVA: 0x00288320 File Offset: 0x00286520
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(8m);
				array[1] = new ExtraDamageVar(2m);
				array[2] = new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, [Nullable(2)] Creature _) => card.Owner.PlayerCombatState.AllCards.Count<CardModel>((CardModel c) => c.CanonicalStarCost >= 0 || c.HasStarCostX));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x06006FD8 RID: 28632 RVA: 0x00288384 File Offset: 0x00286584
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_starry_impact", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06006FD9 RID: 28633 RVA: 0x002883D7 File Offset: 0x002865D7
		protected override void OnUpgrade()
		{
			base.DynamicVars.ExtraDamage.UpgradeValueBy(1m);
		}
	}
}
