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
	// Token: 0x020009F8 RID: 2552
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HeavenlyDrill : CardModel
	{
		// Token: 0x060072D8 RID: 29400 RVA: 0x0028E273 File Offset: 0x0028C473
		public HeavenlyDrill()
			: base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E7F RID: 7807
		// (get) Token: 0x060072D9 RID: 29401 RVA: 0x0028E280 File Offset: 0x0028C480
		protected override bool HasEnergyCostX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E80 RID: 7808
		// (get) Token: 0x060072DA RID: 29402 RVA: 0x0028E283 File Offset: 0x0028C483
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return base.Owner.PlayerCombatState.Energy >= base.DynamicVars.Energy.IntValue;
			}
		}

		// Token: 0x17001E81 RID: 7809
		// (get) Token: 0x060072DB RID: 29403 RVA: 0x0028E2AA File Offset: 0x0028C4AA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(8m, ValueProp.Move),
					new EnergyVar(4)
				});
			}
		}

		// Token: 0x060072DC RID: 29404 RVA: 0x0028E2D0 File Offset: 0x0028C4D0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			int num = base.ResolveEnergyXValue();
			if (num >= base.DynamicVars.Energy.IntValue)
			{
				num *= 2;
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(num).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_giant_horizontal_slash", null, "slash_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x060072DD RID: 29405 RVA: 0x0028E323 File Offset: 0x0028C523
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}
	}
}
