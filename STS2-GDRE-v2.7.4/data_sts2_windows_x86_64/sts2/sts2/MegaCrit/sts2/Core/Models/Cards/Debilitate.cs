using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200097C RID: 2428
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Debilitate : CardModel
	{
		// Token: 0x0600702A RID: 28714 RVA: 0x00288EE3 File Offset: 0x002870E3
		public Debilitate()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D4B RID: 7499
		// (get) Token: 0x0600702B RID: 28715 RVA: 0x00288EF0 File Offset: 0x002870F0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<VulnerablePower>(null),
					HoverTipFactory.FromPower<WeakPower>(null)
				});
			}
		}

		// Token: 0x17001D4C RID: 7500
		// (get) Token: 0x0600702C RID: 28716 RVA: 0x00288F2A File Offset: 0x0028712A
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(10m, ValueProp.Move),
					new PowerVar<DebilitatePower>(2m)
				});
			}
		}

		// Token: 0x0600702D RID: 28717 RVA: 0x00288F58 File Offset: 0x00287158
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await PowerCmd.Apply<DebilitatePower>(choiceContext, cardPlay.Target, base.DynamicVars["DebilitatePower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600702E RID: 28718 RVA: 0x00288FAB File Offset: 0x002871AB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars["DebilitatePower"].UpgradeValueBy(1m);
		}
	}
}
