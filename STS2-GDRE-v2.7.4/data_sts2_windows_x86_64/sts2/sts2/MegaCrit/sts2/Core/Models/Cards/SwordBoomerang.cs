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
	// Token: 0x02000B00 RID: 2816
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SwordBoomerang : CardModel
	{
		// Token: 0x0600786B RID: 30827 RVA: 0x00299C97 File Offset: 0x00297E97
		public SwordBoomerang()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy, true)
		{
		}

		// Token: 0x170020C8 RID: 8392
		// (get) Token: 0x0600786C RID: 30828 RVA: 0x00299CA4 File Offset: 0x00297EA4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(3m, ValueProp.Move),
					new RepeatVar(3)
				});
			}
		}

		// Token: 0x0600786D RID: 30829 RVA: 0x00299CCC File Offset: 0x00297ECC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue).FromCard(this, cardPlay)
				.TargetingRandomOpponents(base.CombatState, true)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x0600786E RID: 30830 RVA: 0x00299D1F File Offset: 0x00297F1F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Repeat.UpgradeValueBy(1m);
		}
	}
}
