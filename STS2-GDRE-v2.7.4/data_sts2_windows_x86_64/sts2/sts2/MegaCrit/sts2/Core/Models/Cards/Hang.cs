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
	// Token: 0x020009F3 RID: 2547
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Hang : CardModel
	{
		// Token: 0x060072C0 RID: 29376 RVA: 0x0028DEFB File Offset: 0x0028C0FB
		public Hang()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E77 RID: 7799
		// (get) Token: 0x060072C1 RID: 29377 RVA: 0x0028DF08 File Offset: 0x0028C108
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(10m, ValueProp.Move));
			}
		}

		// Token: 0x060072C2 RID: 29378 RVA: 0x0028DF1C File Offset: 0x0028C11C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			int powerAmount = cardPlay.Target.GetPowerAmount<HangPower>();
			int num = Math.Max(2, powerAmount);
			if (powerAmount + num > 999999999)
			{
				num = Math.Max(0, 999999999 - powerAmount);
			}
			await PowerCmd.Apply<HangPower>(choiceContext, cardPlay.Target, num, base.Owner.Creature, this, false);
		}

		// Token: 0x060072C3 RID: 29379 RVA: 0x0028DF6F File Offset: 0x0028C16F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
