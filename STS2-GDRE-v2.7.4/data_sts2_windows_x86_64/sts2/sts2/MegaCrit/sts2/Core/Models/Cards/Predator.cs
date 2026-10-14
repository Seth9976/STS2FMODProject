using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A7D RID: 2685
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Predator : CardModel
	{
		// Token: 0x060075A5 RID: 30117 RVA: 0x002940AB File Offset: 0x002922AB
		public Predator()
			: base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001FA6 RID: 8102
		// (get) Token: 0x060075A6 RID: 30118 RVA: 0x002940B8 File Offset: 0x002922B8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(15m, ValueProp.Move));
			}
		}

		// Token: 0x060075A7 RID: 30119 RVA: 0x002940CC File Offset: 0x002922CC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, base.Owner.Creature, 2m, base.Owner.Creature, this, false);
		}

		// Token: 0x060075A8 RID: 30120 RVA: 0x0029411F File Offset: 0x0029231F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(5m);
		}
	}
}
