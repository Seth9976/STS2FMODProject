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
	// Token: 0x0200098F RID: 2447
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Devastate : CardModel
	{
		// Token: 0x06007095 RID: 28821 RVA: 0x00289A5F File Offset: 0x00287C5F
		public Devastate()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D81 RID: 7553
		// (get) Token: 0x06007096 RID: 28822 RVA: 0x00289A6C File Offset: 0x00287C6C
		public override int CanonicalStarCost
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17001D82 RID: 7554
		// (get) Token: 0x06007097 RID: 28823 RVA: 0x00289A6F File Offset: 0x00287C6F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(35m, ValueProp.Move));
			}
		}

		// Token: 0x06007098 RID: 28824 RVA: 0x00289A84 File Offset: 0x00287C84
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x06007099 RID: 28825 RVA: 0x00289AD7 File Offset: 0x00287CD7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(10m);
		}
	}
}
