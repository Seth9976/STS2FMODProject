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
	// Token: 0x02000A77 RID: 2679
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PoisonedStab : CardModel
	{
		// Token: 0x06007589 RID: 30089 RVA: 0x00293CD2 File Offset: 0x00291ED2
		public PoisonedStab()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F9A RID: 8090
		// (get) Token: 0x0600758A RID: 30090 RVA: 0x00293CDF File Offset: 0x00291EDF
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(6m, ValueProp.Move),
					new PowerVar<PoisonPower>(3m)
				});
			}
		}

		// Token: 0x17001F9B RID: 8091
		// (get) Token: 0x0600758B RID: 30091 RVA: 0x00293D0C File Offset: 0x00291F0C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x0600758C RID: 30092 RVA: 0x00293D2C File Offset: 0x00291F2C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_dramatic_stab", null, null)
				.Execute(choiceContext);
			await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, base.DynamicVars.Poison.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600758D RID: 30093 RVA: 0x00293D7F File Offset: 0x00291F7F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
			base.DynamicVars.Poison.UpgradeValueBy(1m);
		}
	}
}
