using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000ADF RID: 2783
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Spite : CardModel
	{
		// Token: 0x060077C0 RID: 30656 RVA: 0x0029849F File Offset: 0x0029669F
		public Spite()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002089 RID: 8329
		// (get) Token: 0x060077C1 RID: 30657 RVA: 0x002984AC File Offset: 0x002966AC
		protected override bool ShouldGlowGoldInternal
		{
			get
			{
				return Spite.LostHpThisTurn(base.Owner.Creature);
			}
		}

		// Token: 0x1700208A RID: 8330
		// (get) Token: 0x060077C2 RID: 30658 RVA: 0x002984BE File Offset: 0x002966BE
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(5m, ValueProp.Move),
					new RepeatVar(2)
				});
			}
		}

		// Token: 0x060077C3 RID: 30659 RVA: 0x002984E4 File Offset: 0x002966E4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			int num = (Spite.LostHpThisTurn(base.Owner.Creature) ? base.DynamicVars.Repeat.IntValue : 1);
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(num).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x060077C4 RID: 30660 RVA: 0x00298537 File Offset: 0x00296737
		protected override void OnUpgrade()
		{
			base.DynamicVars.Repeat.UpgradeValueBy(1m);
		}

		// Token: 0x060077C5 RID: 30661 RVA: 0x00298550 File Offset: 0x00296750
		private static bool LostHpThisTurn(Creature creature)
		{
			return CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Any<DamageReceivedEntry>((DamageReceivedEntry e) => e.HappenedThisTurn(creature.CombatState) && e.Receiver == creature && e.Result.UnblockedDamage > 0);
		}
	}
}
