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
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A03 RID: 2563
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HighFive : CardModel
	{
		// Token: 0x0600730D RID: 29453 RVA: 0x0028E9D7 File Offset: 0x0028CBD7
		public HighFive()
			: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001E93 RID: 7827
		// (get) Token: 0x0600730E RID: 29454 RVA: 0x0028E9E4 File Offset: 0x0028CBE4
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x17001E94 RID: 7828
		// (get) Token: 0x0600730F RID: 29455 RVA: 0x0028E9F1 File Offset: 0x0028CBF1
		protected override bool IsPlayable
		{
			get
			{
				return !base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x17001E95 RID: 7829
		// (get) Token: 0x06007310 RID: 29456 RVA: 0x0028EA01 File Offset: 0x0028CC01
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new OstyDamageVar(11m, ValueProp.Move),
					new PowerVar<VulnerablePower>(2m)
				});
			}
		}

		// Token: 0x17001E96 RID: 7830
		// (get) Token: 0x06007311 RID: 29457 RVA: 0x0028EA2C File Offset: 0x0028CC2C
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x17001E97 RID: 7831
		// (get) Token: 0x06007312 RID: 29458 RVA: 0x0028EA3C File Offset: 0x0028CC3C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x06007313 RID: 29459 RVA: 0x0028EA5C File Offset: 0x0028CC5C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.OstyDamage.BaseValue).FromOsty(base.Owner.Osty, this, cardPlay).TargetingAllOpponents(base.CombatState)
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
				ICombatState combatState = base.CombatState;
				await PowerCmd.Apply<VulnerablePower>(choiceContext, (combatState != null) ? combatState.HittableEnemies : null, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this, false);
			}
		}

		// Token: 0x06007314 RID: 29460 RVA: 0x0028EAAF File Offset: 0x0028CCAF
		protected override void OnUpgrade()
		{
			base.DynamicVars.OstyDamage.UpgradeValueBy(2m);
			base.DynamicVars.Vulnerable.UpgradeValueBy(1m);
		}
	}
}
