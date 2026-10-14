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
	// Token: 0x02000A20 RID: 2592
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Knockdown : CardModel
	{
		// Token: 0x060073A8 RID: 29608 RVA: 0x0028FD4B File Offset: 0x0028DF4B
		public Knockdown()
			: base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001ED3 RID: 7891
		// (get) Token: 0x060073A9 RID: 29609 RVA: 0x0028FD58 File Offset: 0x0028DF58
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001ED4 RID: 7892
		// (get) Token: 0x060073AA RID: 29610 RVA: 0x0028FD5B File Offset: 0x0028DF5B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(10m, ValueProp.Move),
					new PowerVar<KnockdownPower>(2m)
				});
			}
		}

		// Token: 0x060073AB RID: 29611 RVA: 0x0028FD88 File Offset: 0x0028DF88
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await PowerCmd.Apply<KnockdownPower>(choiceContext, cardPlay.Target, base.DynamicVars["KnockdownPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060073AC RID: 29612 RVA: 0x0028FDDB File Offset: 0x0028DFDB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
			base.DynamicVars["KnockdownPower"].UpgradeValueBy(1m);
		}
	}
}
