using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B35 RID: 2869
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Volley : CardModel
	{
		// Token: 0x06007987 RID: 31111 RVA: 0x0029BF6C File Offset: 0x0029A16C
		public Volley()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy, true)
		{
		}

		// Token: 0x1700213A RID: 8506
		// (get) Token: 0x06007988 RID: 31112 RVA: 0x0029BF79 File Offset: 0x0029A179
		protected override bool HasEnergyCostX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700213B RID: 8507
		// (get) Token: 0x06007989 RID: 31113 RVA: 0x0029BF7C File Offset: 0x0029A17C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(10m, ValueProp.Move));
			}
		}

		// Token: 0x0600798A RID: 31114 RVA: 0x0029BF90 File Offset: 0x0029A190
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.ResolveEnergyXValue()).FromCard(this, cardPlay)
				.TargetingRandomOpponents(base.CombatState, true)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x0600798B RID: 31115 RVA: 0x0029BFE3 File Offset: 0x0029A1E3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
