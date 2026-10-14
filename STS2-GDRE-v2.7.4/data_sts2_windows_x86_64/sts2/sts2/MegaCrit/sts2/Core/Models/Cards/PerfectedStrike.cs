using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A6F RID: 2671
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PerfectedStrike : CardModel
	{
		// Token: 0x0600755E RID: 30046 RVA: 0x002936BA File Offset: 0x002918BA
		public PerfectedStrike()
			: base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F8B RID: 8075
		// (get) Token: 0x0600755F RID: 30047 RVA: 0x002936C7 File Offset: 0x002918C7
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x17001F8C RID: 8076
		// (get) Token: 0x06007560 RID: 30048 RVA: 0x002936D8 File Offset: 0x002918D8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(6m);
				array[1] = new ExtraDamageVar(2m);
				array[2] = new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, [Nullable(2)] Creature _) => card.Owner.PlayerCombatState.AllCards.Count<CardModel>((CardModel c) => c.Tags.Contains(CardTag.Strike)));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x06007561 RID: 30049 RVA: 0x0029373C File Offset: 0x0029193C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			AttackCommand attackCommand = DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx(null, null, "heavy_attack.mp3")
				.WithHitVfxNode((Creature t) => NBigSlashVfx.Create(t))
				.WithHitVfxNode((Creature t) => NBigSlashImpactVfx.Create(t));
			if (base.DynamicVars.CalculatedDamage.Calculate(cardPlay.Target) > 12m)
			{
				attackCommand.WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(base.Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(base.Owner.Character), null);
			}
			await attackCommand.Execute(choiceContext);
		}

		// Token: 0x06007562 RID: 30050 RVA: 0x0029378F File Offset: 0x0029198F
		protected override void OnUpgrade()
		{
			base.DynamicVars.ExtraDamage.UpgradeValueBy(1m);
		}
	}
}
