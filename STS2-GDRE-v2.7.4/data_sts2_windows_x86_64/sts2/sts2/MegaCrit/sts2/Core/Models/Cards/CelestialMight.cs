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
	// Token: 0x02000949 RID: 2377
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CelestialMight : CardModel
	{
		// Token: 0x06006F21 RID: 28449 RVA: 0x00286CAA File Offset: 0x00284EAA
		public CelestialMight()
			: base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001CDA RID: 7386
		// (get) Token: 0x06006F22 RID: 28450 RVA: 0x00286CB7 File Offset: 0x00284EB7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(6m, ValueProp.Move),
					new RepeatVar(3)
				});
			}
		}

		// Token: 0x06006F23 RID: 28451 RVA: 0x00286CDC File Offset: 0x00284EDC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_starry_impact", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x06006F24 RID: 28452 RVA: 0x00286D2F File Offset: 0x00284F2F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Repeat.UpgradeValueBy(1m);
		}
	}
}
