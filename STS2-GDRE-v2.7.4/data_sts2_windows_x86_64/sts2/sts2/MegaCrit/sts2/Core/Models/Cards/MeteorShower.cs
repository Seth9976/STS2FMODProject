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
	// Token: 0x02000A3C RID: 2620
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeteorShower : CardModel
	{
		// Token: 0x0600744D RID: 29773 RVA: 0x0029143B File Offset: 0x0028F63B
		public MeteorShower()
			: base(0, CardType.Attack, CardRarity.Ancient, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001F15 RID: 7957
		// (get) Token: 0x0600744E RID: 29774 RVA: 0x00291448 File Offset: 0x0028F648
		public override int CanonicalStarCost
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001F16 RID: 7958
		// (get) Token: 0x0600744F RID: 29775 RVA: 0x0029144B File Offset: 0x0028F64B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(14m, ValueProp.Move),
					new PowerVar<VulnerablePower>(2m),
					new PowerVar<WeakPower>(2m)
				});
			}
		}

		// Token: 0x17001F17 RID: 7959
		// (get) Token: 0x06007450 RID: 29776 RVA: 0x00291484 File Offset: 0x0028F684
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<WeakPower>(null),
					HoverTipFactory.FromPower<VulnerablePower>(null)
				});
			}
		}

		// Token: 0x06007451 RID: 29777 RVA: 0x002914C0 File Offset: 0x0028F6C0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			ICombatState combatState = base.CombatState;
			await PowerCmd.Apply<WeakPower>(choiceContext, (combatState != null) ? combatState.HittableEnemies : null, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this, false);
			ICombatState combatState2 = base.CombatState;
			await PowerCmd.Apply<VulnerablePower>(choiceContext, (combatState2 != null) ? combatState2.HittableEnemies : null, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007452 RID: 29778 RVA: 0x00291513 File Offset: 0x0028F713
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(7m);
		}
	}
}
