using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000907 RID: 2311
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AstralPulse : CardModel
	{
		// Token: 0x06006DD5 RID: 28117 RVA: 0x00284188 File Offset: 0x00282388
		public AstralPulse()
			: base(0, CardType.Attack, CardRarity.Common, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001C54 RID: 7252
		// (get) Token: 0x06006DD6 RID: 28118 RVA: 0x00284195 File Offset: 0x00282395
		public override int CanonicalStarCost
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001C55 RID: 7253
		// (get) Token: 0x06006DD7 RID: 28119 RVA: 0x00284198 File Offset: 0x00282398
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(6m, ValueProp.Move));
			}
		}

		// Token: 0x06006DD8 RID: 28120 RVA: 0x002841AC File Offset: 0x002823AC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(base.CombatState)
				.WithHitCount(2)
				.WithHitFx("vfx/vfx_starry_impact", null, null)
				.SpawningHitVfxOnEachCreature()
				.Execute(choiceContext);
		}

		// Token: 0x06006DD9 RID: 28121 RVA: 0x002841FF File Offset: 0x002823FF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}
	}
}
