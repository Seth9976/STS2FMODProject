using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A84 RID: 2692
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Protector : CardModel
	{
		// Token: 0x060075C4 RID: 30148 RVA: 0x0029444B File Offset: 0x0029264B
		public Protector()
			: base(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001FB1 RID: 8113
		// (get) Token: 0x060075C5 RID: 30149 RVA: 0x00294458 File Offset: 0x00292658
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x17001FB2 RID: 8114
		// (get) Token: 0x060075C6 RID: 30150 RVA: 0x00294465 File Offset: 0x00292665
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x17001FB3 RID: 8115
		// (get) Token: 0x060075C7 RID: 30151 RVA: 0x00294474 File Offset: 0x00292674
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(10m);
				array[1] = new ExtraDamageVar(1m).FromOsty();
				array[2] = new CalculatedDamageVar(ValueProp.Move).FromOsty().WithMultiplier(delegate(CardModel card, [Nullable(2)] Creature _)
				{
					Creature osty = card.Owner.Osty;
					return (osty != null && osty.IsAlive) ? osty.MaxHp : 0;
				});
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x060075C8 RID: 30152 RVA: 0x002944E0 File Offset: 0x002926E0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromOsty(base.Owner.Osty, this, cardPlay).Targeting(cardPlay.Target)
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
			}
		}

		// Token: 0x060075C9 RID: 30153 RVA: 0x00294533 File Offset: 0x00292733
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
			base.DynamicVars.CalculationBase.UpgradeValueBy(5m);
		}
	}
}
