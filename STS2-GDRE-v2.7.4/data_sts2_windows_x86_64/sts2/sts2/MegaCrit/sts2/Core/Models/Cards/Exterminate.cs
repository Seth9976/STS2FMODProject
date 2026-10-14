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
	// Token: 0x020009B5 RID: 2485
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Exterminate : CardModel
	{
		// Token: 0x06007165 RID: 29029 RVA: 0x0028B25F File Offset: 0x0028945F
		public Exterminate()
			: base(1, CardType.Attack, CardRarity.Event, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001DE0 RID: 7648
		// (get) Token: 0x06007166 RID: 29030 RVA: 0x0028B26C File Offset: 0x0028946C
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

		// Token: 0x06007167 RID: 29031 RVA: 0x0028B294 File Offset: 0x00289494
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue).FromCard(this, cardPlay)
				.TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06007168 RID: 29032 RVA: 0x0028B2E7 File Offset: 0x002894E7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(1m);
		}
	}
}
