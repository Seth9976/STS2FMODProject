using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009CA RID: 2506
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Flatten : CardModel
	{
		// Token: 0x060071D8 RID: 29144 RVA: 0x0028C27F File Offset: 0x0028A47F
		public Flatten()
			: base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E11 RID: 7697
		// (get) Token: 0x060071D9 RID: 29145 RVA: 0x0028C28C File Offset: 0x0028A48C
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return this.HasOstyAttackedThisTurn;
			}
		}

		// Token: 0x17001E12 RID: 7698
		// (get) Token: 0x060071DA RID: 29146 RVA: 0x0028C294 File Offset: 0x0028A494
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x17001E13 RID: 7699
		// (get) Token: 0x060071DB RID: 29147 RVA: 0x0028C2A1 File Offset: 0x0028A4A1
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x17001E14 RID: 7700
		// (get) Token: 0x060071DC RID: 29148 RVA: 0x0028C2B0 File Offset: 0x0028A4B0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new OstyDamageVar(12m, ValueProp.Move));
			}
		}

		// Token: 0x060071DD RID: 29149 RVA: 0x0028C2C4 File Offset: 0x0028A4C4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.OstyDamage.BaseValue).FromOsty(base.Owner.Osty, this, cardPlay).Targeting(cardPlay.Target)
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
			}
		}

		// Token: 0x060071DE RID: 29150 RVA: 0x0028C317 File Offset: 0x0028A517
		protected override void OnUpgrade()
		{
			base.DynamicVars.OstyDamage.UpgradeValueBy(4m);
		}

		// Token: 0x060071DF RID: 29151 RVA: 0x0028C32F File Offset: 0x0028A52F
		public override Task AfterCardEnteredCombat(CardModel card)
		{
			if (card != this)
			{
				return Task.CompletedTask;
			}
			if (!this.HasOstyAttackedThisTurn)
			{
				return Task.CompletedTask;
			}
			this.ReduceCost();
			return Task.CompletedTask;
		}

		// Token: 0x060071E0 RID: 29152 RVA: 0x0028C354 File Offset: 0x0028A554
		public override Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
		{
			if (command.Attacker == null)
			{
				return Task.CompletedTask;
			}
			if (command.Attacker != base.Owner.Osty)
			{
				return Task.CompletedTask;
			}
			this.ReduceCost();
			return Task.CompletedTask;
		}

		// Token: 0x060071E1 RID: 29153 RVA: 0x0028C388 File Offset: 0x0028A588
		private void ReduceCost()
		{
			base.EnergyCost.SetThisTurn(0, false);
		}

		// Token: 0x17001E15 RID: 7701
		// (get) Token: 0x060071E2 RID: 29154 RVA: 0x0028C397 File Offset: 0x0028A597
		private bool HasOstyAttackedThisTurn
		{
			get
			{
				return CombatManager.Instance.History.Entries.OfType<CreatureAttackedEntry>().Any<CreatureAttackedEntry>((CreatureAttackedEntry e) => e.Actor == base.Owner.Osty && e.HappenedThisTurn(base.CombatState));
			}
		}
	}
}
