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
	/// <summary>
	/// The only difference between the starting Strike cards are portrait, attack vfx, and color.
	/// </summary>
	// Token: 0x020009F9 RID: 2553
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Hegemony : CardModel
	{
		/// <summary>
		/// The only difference between the starting Strike cards are portrait, attack vfx, and color.
		/// </summary>
		// Token: 0x060072DE RID: 29406 RVA: 0x0028E33B File Offset: 0x0028C53B
		public Hegemony()
			: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E82 RID: 7810
		// (get) Token: 0x060072DF RID: 29407 RVA: 0x0028E348 File Offset: 0x0028C548
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(15m, ValueProp.Move),
					new EnergyVar(2)
				});
			}
		}

		// Token: 0x17001E83 RID: 7811
		// (get) Token: 0x060072E0 RID: 29408 RVA: 0x0028E36E File Offset: 0x0028C56E
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x060072E1 RID: 29409 RVA: 0x0028E37C File Offset: 0x0028C57C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
				.Execute(choiceContext);
			await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060072E2 RID: 29410 RVA: 0x0028E3CF File Offset: 0x0028C5CF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
