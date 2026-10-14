using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A92 RID: 2706
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Rattle : CardModel
	{
		// Token: 0x0600760E RID: 30222 RVA: 0x00294ECD File Offset: 0x002930CD
		public Rattle()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001FCF RID: 8143
		// (get) Token: 0x0600760F RID: 30223 RVA: 0x00294EDA File Offset: 0x002930DA
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x17001FD0 RID: 8144
		// (get) Token: 0x06007610 RID: 30224 RVA: 0x00294EE7 File Offset: 0x002930E7
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x17001FD1 RID: 8145
		// (get) Token: 0x06007611 RID: 30225 RVA: 0x00294EF8 File Offset: 0x002930F8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[4];
				array[0] = new OstyDamageVar(7m, ValueProp.Move);
				array[1] = new CalculationBaseVar(0m);
				array[2] = new CalculationExtraVar(1m);
				array[3] = new CalculatedVar("CalculatedHits").WithMultiplier((CardModel card, [Nullable(2)] Creature _) => 1 + CombatManager.Instance.History.Entries.OfType<CreatureAttackedEntry>().Count<CreatureAttackedEntry>((CreatureAttackedEntry e) => e.Actor == card.Owner.Osty && e.HappenedThisTurn(card.CombatState)));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x06007612 RID: 30226 RVA: 0x00294F6C File Offset: 0x0029316C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.OstyDamage.BaseValue).FromOsty(base.Owner.Osty, this, cardPlay).Targeting(cardPlay.Target)
					.WithHitCount((int)((CalculatedVar)base.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target))
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
			}
		}

		// Token: 0x06007613 RID: 30227 RVA: 0x00294FBF File Offset: 0x002931BF
		protected override void OnUpgrade()
		{
			base.DynamicVars.OstyDamage.UpgradeValueBy(2m);
		}

		// Token: 0x040028B6 RID: 10422
		private const string _calculatedHitsKey = "CalculatedHits";
	}
}
