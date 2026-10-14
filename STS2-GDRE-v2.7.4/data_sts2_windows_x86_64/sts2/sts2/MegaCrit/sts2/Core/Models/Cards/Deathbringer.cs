using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000979 RID: 2425
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Deathbringer : CardModel
	{
		// Token: 0x06007018 RID: 28696 RVA: 0x00288BFF File Offset: 0x00286DFF
		public Deathbringer()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies, true)
		{
		}

		// Token: 0x17001D43 RID: 7491
		// (get) Token: 0x06007019 RID: 28697 RVA: 0x00288C0C File Offset: 0x00286E0C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<DoomPower>(21m),
					new PowerVar<WeakPower>(1m)
				});
			}
		}

		// Token: 0x17001D44 RID: 7492
		// (get) Token: 0x0600701A RID: 28698 RVA: 0x00288C38 File Offset: 0x00286E38
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<DoomPower>(null),
					HoverTipFactory.FromPower<WeakPower>(null)
				});
			}
		}

		// Token: 0x0600701B RID: 28699 RVA: 0x00288C74 File Offset: 0x00286E74
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			ICombatState combatState = base.CombatState;
			await PowerCmd.Apply<DoomPower>(choiceContext, (combatState != null) ? combatState.HittableEnemies : null, base.DynamicVars.Doom.BaseValue, base.Owner.Creature, this, false);
			ICombatState combatState2 = base.CombatState;
			await PowerCmd.Apply<WeakPower>(choiceContext, (combatState2 != null) ? combatState2.HittableEnemies : null, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600701C RID: 28700 RVA: 0x00288CBF File Offset: 0x00286EBF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Doom.UpgradeValueBy(5m);
		}
	}
}
