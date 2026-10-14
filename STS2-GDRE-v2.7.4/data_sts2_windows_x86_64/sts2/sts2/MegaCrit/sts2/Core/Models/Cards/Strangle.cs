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
	// Token: 0x02000AEF RID: 2799
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Strangle : CardModel
	{
		// Token: 0x06007818 RID: 30744 RVA: 0x00299163 File Offset: 0x00297363
		public Strangle()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x170020A9 RID: 8361
		// (get) Token: 0x06007819 RID: 30745 RVA: 0x00299170 File Offset: 0x00297370
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(8m, ValueProp.Move),
					new PowerVar<StranglePower>(2m)
				});
			}
		}

		// Token: 0x0600781A RID: 30746 RVA: 0x0029919C File Offset: 0x0029739C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await PowerCmd.Apply<StranglePower>(choiceContext, cardPlay.Target, base.DynamicVars["StranglePower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600781B RID: 30747 RVA: 0x002991EF File Offset: 0x002973EF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars["StranglePower"].UpgradeValueBy(1m);
		}
	}
}
