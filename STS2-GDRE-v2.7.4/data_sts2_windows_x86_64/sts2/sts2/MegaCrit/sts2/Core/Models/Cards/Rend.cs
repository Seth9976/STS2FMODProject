using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A9F RID: 2719
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Rend : CardModel
	{
		// Token: 0x06007656 RID: 30294 RVA: 0x002957AD File Offset: 0x002939AD
		public Rend()
			: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001FEF RID: 8175
		// (get) Token: 0x06007657 RID: 30295 RVA: 0x002957BC File Offset: 0x002939BC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(15m);
				array[1] = new ExtraDamageVar(5m);
				array[2] = new CalculatedDamageVar(ValueProp.Move).WithMultiplier(delegate(CardModel card, [Nullable(2)] Creature target)
				{
					int num;
					if (target == null)
					{
						num = 0;
					}
					else
					{
						IEnumerable<PowerModel> powers = target.Powers;
						Func<PowerModel, bool> func;
						if ((func = Rend.<>O.<0>__ShouldCountPower) == null)
						{
							func = (Rend.<>O.<0>__ShouldCountPower = new Func<PowerModel, bool>(Rend.ShouldCountPower));
						}
						num = powers.Count<PowerModel>(func);
					}
					return num;
				});
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x06007658 RID: 30296 RVA: 0x00295820 File Offset: 0x00293A20
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x06007659 RID: 30297 RVA: 0x00295873 File Offset: 0x00293A73
		protected override void OnUpgrade()
		{
			base.DynamicVars.ExtraDamage.UpgradeValueBy(3m);
			base.DynamicVars.CalculationBase.UpgradeValueBy(3m);
		}

		// Token: 0x0600765A RID: 30298 RVA: 0x002958A1 File Offset: 0x00293AA1
		private static bool ShouldCountPower(PowerModel power)
		{
			return power.TypeForCurrentAmount == PowerType.Debuff && !(power is ITemporaryPower);
		}

		// Token: 0x020021CE RID: 8654
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04008D63 RID: 36195
			[Nullable(0)]
			public static Func<PowerModel, bool> <0>__ShouldCountPower;
		}
	}
}
