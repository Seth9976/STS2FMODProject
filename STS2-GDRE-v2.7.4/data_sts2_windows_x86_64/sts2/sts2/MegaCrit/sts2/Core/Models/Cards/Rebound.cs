using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A98 RID: 2712
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Rebound : CardModel
	{
		// Token: 0x0600762D RID: 30253 RVA: 0x002952D7 File Offset: 0x002934D7
		public Rebound()
			: base(1, CardType.Attack, CardRarity.Event, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001FDC RID: 8156
		// (get) Token: 0x0600762E RID: 30254 RVA: 0x002952E4 File Offset: 0x002934E4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(9m, ValueProp.Move));
			}
		}

		// Token: 0x17001FDD RID: 8157
		// (get) Token: 0x0600762F RID: 30255 RVA: 0x002952F8 File Offset: 0x002934F8
		public override CardPoolModel VisualCardPool
		{
			get
			{
				return ModelDb.CardPool<DefectCardPool>();
			}
		}

		// Token: 0x06007630 RID: 30256 RVA: 0x00295300 File Offset: 0x00293500
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await PowerCmd.Apply<ReboundPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x06007631 RID: 30257 RVA: 0x00295353 File Offset: 0x00293553
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
