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
	// Token: 0x02000AE9 RID: 2793
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Stardust : CardModel
	{
		// Token: 0x060077FC RID: 30716 RVA: 0x00298D45 File Offset: 0x00296F45
		public Stardust()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy, true)
		{
		}

		// Token: 0x170020A1 RID: 8353
		// (get) Token: 0x060077FD RID: 30717 RVA: 0x00298D52 File Offset: 0x00296F52
		public override bool HasStarCostX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170020A2 RID: 8354
		// (get) Token: 0x060077FE RID: 30718 RVA: 0x00298D55 File Offset: 0x00296F55
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(5m, ValueProp.Move));
			}
		}

		// Token: 0x060077FF RID: 30719 RVA: 0x00298D68 File Offset: 0x00296F68
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.ResolveStarXValue()).FromCard(this, cardPlay)
				.TargetingRandomOpponents(base.CombatState, true)
				.WithHitFx("vfx/vfx_starry_impact", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06007800 RID: 30720 RVA: 0x00298DBB File Offset: 0x00296FBB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}
	}
}
