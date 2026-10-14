using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AA4 RID: 2724
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RipAndTear : CardModel
	{
		// Token: 0x06007676 RID: 30326 RVA: 0x00295C03 File Offset: 0x00293E03
		public RipAndTear()
			: base(1, CardType.Attack, CardRarity.Event, TargetType.RandomEnemy, true)
		{
		}

		// Token: 0x17001FFE RID: 8190
		// (get) Token: 0x06007677 RID: 30327 RVA: 0x00295C10 File Offset: 0x00293E10
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(7m, ValueProp.Move));
			}
		}

		// Token: 0x17001FFF RID: 8191
		// (get) Token: 0x06007678 RID: 30328 RVA: 0x00295C23 File Offset: 0x00293E23
		public override CardPoolModel VisualCardPool
		{
			get
			{
				return ModelDb.CardPool<DefectCardPool>();
			}
		}

		// Token: 0x06007679 RID: 30329 RVA: 0x00295C2C File Offset: 0x00293E2C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard(this, cardPlay)
				.TargetingRandomOpponents(base.CombatState, true)
				.WithHitVfxNode((Creature t) => NScratchVfx.Create(t, true))
				.Execute(choiceContext);
		}

		// Token: 0x0600767A RID: 30330 RVA: 0x00295C7F File Offset: 0x00293E7F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}
	}
}
