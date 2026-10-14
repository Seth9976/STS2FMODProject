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
	// Token: 0x020009FE RID: 2558
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Hemokinesis : CardModel
	{
		// Token: 0x060072F5 RID: 29429 RVA: 0x0028E6AD File Offset: 0x0028C8AD
		public Hemokinesis()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E89 RID: 7817
		// (get) Token: 0x060072F6 RID: 29430 RVA: 0x0028E6BA File Offset: 0x0028C8BA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new HpLossVar(2m),
					new DamageVar(15m, ValueProp.Move)
				});
			}
		}

		// Token: 0x060072F7 RID: 29431 RVA: 0x0028E6E8 File Offset: 0x0028C8E8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_bloody_impact", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x060072F8 RID: 29432 RVA: 0x0028E73B File Offset: 0x0028C93B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(5m);
		}
	}
}
