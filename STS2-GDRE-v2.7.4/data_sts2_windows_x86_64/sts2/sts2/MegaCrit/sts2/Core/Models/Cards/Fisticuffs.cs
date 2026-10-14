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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009C5 RID: 2501
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Fisticuffs : CardModel
	{
		// Token: 0x060071C0 RID: 29120 RVA: 0x0028BEEF File Offset: 0x0028A0EF
		public Fisticuffs()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E09 RID: 7689
		// (get) Token: 0x060071C1 RID: 29121 RVA: 0x0028BEFC File Offset: 0x0028A0FC
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E0A RID: 7690
		// (get) Token: 0x060071C2 RID: 29122 RVA: 0x0028BEFF File Offset: 0x0028A0FF
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(7m, ValueProp.Move));
			}
		}

		// Token: 0x060071C3 RID: 29123 RVA: 0x0028BF14 File Offset: 0x0028A114
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			AttackCommand attackCommand2 = attackCommand;
			await CreatureCmd.GainBlock(base.Owner.Creature, attackCommand2.Results.SelectMany<List<DamageResult>, DamageResult>((List<DamageResult> r) => r).Sum<DamageResult>((DamageResult r) => r.TotalDamage + r.OverkillDamage), ValueProp.Move, cardPlay, false);
		}

		// Token: 0x060071C4 RID: 29124 RVA: 0x0028BF67 File Offset: 0x0028A167
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}
	}
}
