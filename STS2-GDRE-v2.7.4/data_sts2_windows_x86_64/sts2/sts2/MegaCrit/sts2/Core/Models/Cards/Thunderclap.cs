using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B17 RID: 2839
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Thunderclap : CardModel
	{
		// Token: 0x060078EE RID: 30958 RVA: 0x0029AD78 File Offset: 0x00298F78
		public Thunderclap()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x170020FA RID: 8442
		// (get) Token: 0x060078EF RID: 30959 RVA: 0x0029AD85 File Offset: 0x00298F85
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(4m, ValueProp.Move),
					new PowerVar<VulnerablePower>(1m)
				});
			}
		}

		// Token: 0x170020FB RID: 8443
		// (get) Token: 0x060078F0 RID: 30960 RVA: 0x0029ADB0 File Offset: 0x00298FB0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x060078F1 RID: 30961 RVA: 0x0029ADD0 File Offset: 0x00298FD0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			ICombatState combatState = base.CombatState;
			await PowerCmd.Apply<VulnerablePower>(choiceContext, (combatState != null) ? combatState.HittableEnemies : null, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060078F2 RID: 30962 RVA: 0x0029AE23 File Offset: 0x00299023
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
