using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000994 RID: 2452
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Dismantle : CardModel
	{
		// Token: 0x060070B0 RID: 28848 RVA: 0x00289D43 File Offset: 0x00287F43
		public Dismantle()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D8D RID: 7565
		// (get) Token: 0x060070B1 RID: 28849 RVA: 0x00289D50 File Offset: 0x00287F50
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				ICombatState combatState = base.CombatState;
				if (combatState == null)
				{
					return false;
				}
				return combatState.HittableEnemies.Any<Creature>((Creature e) => e.HasPower<VulnerablePower>());
			}
		}

		// Token: 0x17001D8E RID: 7566
		// (get) Token: 0x060070B2 RID: 28850 RVA: 0x00289D87 File Offset: 0x00287F87
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(8m, ValueProp.Move));
			}
		}

		// Token: 0x17001D8F RID: 7567
		// (get) Token: 0x060070B3 RID: 28851 RVA: 0x00289D9C File Offset: 0x00287F9C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x060070B4 RID: 28852 RVA: 0x00289DBC File Offset: 0x00287FBC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			int num = (cardPlay.Target.HasPower<VulnerablePower>() ? 2 : 1);
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(num).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x060070B5 RID: 28853 RVA: 0x00289E0F File Offset: 0x0028800F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(2m);
		}
	}
}
