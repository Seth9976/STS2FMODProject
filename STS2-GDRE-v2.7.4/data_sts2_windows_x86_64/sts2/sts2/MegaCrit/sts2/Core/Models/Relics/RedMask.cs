using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005C9 RID: 1481
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RedMask : RelicModel
	{
		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x06005447 RID: 21575 RVA: 0x0024642F File Offset: 0x0024462F
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Common;
			}
		}

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x06005448 RID: 21576 RVA: 0x00246432 File Offset: 0x00244632
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<WeakPower>(1m));
			}
		}

		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x06005449 RID: 21577 RVA: 0x00246444 File Offset: 0x00244644
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>(null));
			}
		}

		// Token: 0x0600544A RID: 21578 RVA: 0x00246464 File Offset: 0x00244664
		public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				if (base.Owner.PlayerCombatState.TurnNumber <= 1)
				{
					base.Flash();
					await PowerCmd.Apply<WeakPower>(choiceContext, combatState.HittableEnemies, base.DynamicVars["WeakPower"].BaseValue, base.Owner.Creature, null, false);
				}
			}
		}
	}
}
