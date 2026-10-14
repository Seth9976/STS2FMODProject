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
	// Token: 0x02000AA2 RID: 2722
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Ricochet : CardModel
	{
		// Token: 0x06007669 RID: 30313 RVA: 0x00295A49 File Offset: 0x00293C49
		public Ricochet()
			: base(2, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy, true)
		{
		}

		// Token: 0x17001FF8 RID: 8184
		// (get) Token: 0x0600766A RID: 30314 RVA: 0x00295A56 File Offset: 0x00293C56
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(3m, ValueProp.Move),
					new RepeatVar(4)
				});
			}
		}

		// Token: 0x17001FF9 RID: 8185
		// (get) Token: 0x0600766B RID: 30315 RVA: 0x00295A7B File Offset: 0x00293C7B
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Sly);
			}
		}

		// Token: 0x0600766C RID: 30316 RVA: 0x00295A84 File Offset: 0x00293C84
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue).FromCard(this, cardPlay)
				.TargetingRandomOpponents(base.CombatState, true)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x0600766D RID: 30317 RVA: 0x00295AD7 File Offset: 0x00293CD7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Repeat.UpgradeValueBy(1m);
		}
	}
}
