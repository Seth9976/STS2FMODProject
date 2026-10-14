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
	// Token: 0x02000ABB RID: 2747
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SevenStars : CardModel
	{
		// Token: 0x060076EB RID: 30443 RVA: 0x00296A88 File Offset: 0x00294C88
		public SevenStars()
			: base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x1700202C RID: 8236
		// (get) Token: 0x060076EC RID: 30444 RVA: 0x00296A95 File Offset: 0x00294C95
		public override int CanonicalStarCost
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x1700202D RID: 8237
		// (get) Token: 0x060076ED RID: 30445 RVA: 0x00296A98 File Offset: 0x00294C98
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(7m, ValueProp.Move),
					new RepeatVar(7)
				});
			}
		}

		// Token: 0x060076EE RID: 30446 RVA: 0x00296AC0 File Offset: 0x00294CC0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue).FromCard(this, cardPlay)
				.TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_starry_impact", null, "slash_attack.mp3")
				.SpawningHitVfxOnEachCreature()
				.Execute(choiceContext);
		}

		// Token: 0x060076EF RID: 30447 RVA: 0x00296B13 File Offset: 0x00294D13
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
