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
	// Token: 0x020009B7 RID: 2487
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FallingStar : CardModel
	{
		// Token: 0x06007170 RID: 29040 RVA: 0x0028B3B3 File Offset: 0x002895B3
		public FallingStar()
			: base(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001DE5 RID: 7653
		// (get) Token: 0x06007171 RID: 29041 RVA: 0x0028B3C0 File Offset: 0x002895C0
		public override int CanonicalStarCost
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001DE6 RID: 7654
		// (get) Token: 0x06007172 RID: 29042 RVA: 0x0028B3C3 File Offset: 0x002895C3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(8m, ValueProp.Move),
					new PowerVar<VulnerablePower>(1m),
					new PowerVar<WeakPower>(1m)
				});
			}
		}

		// Token: 0x17001DE7 RID: 7655
		// (get) Token: 0x06007173 RID: 29043 RVA: 0x0028B3FC File Offset: 0x002895FC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<WeakPower>(null),
					HoverTipFactory.FromPower<VulnerablePower>(null)
				});
			}
		}

		// Token: 0x06007174 RID: 29044 RVA: 0x0028B438 File Offset: 0x00289638
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_starry_impact", "blunt_attack.mp3", null)
				.Execute(choiceContext);
			await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007175 RID: 29045 RVA: 0x0028B48B File Offset: 0x0028968B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
