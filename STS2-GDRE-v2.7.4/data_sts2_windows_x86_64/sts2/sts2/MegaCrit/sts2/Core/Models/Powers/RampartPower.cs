using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006D4 RID: 1748
	public sealed class RampartPower : PowerModel
	{
		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x06005A69 RID: 23145 RVA: 0x002523D3 File Offset: 0x002505D3
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x06005A6A RID: 23146 RVA: 0x002523D6 File Offset: 0x002505D6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x06005A6B RID: 23147 RVA: 0x002523D9 File Offset: 0x002505D9
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005A6C RID: 23148 RVA: 0x002523DC File Offset: 0x002505DC
		[NullableContext(1)]
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (side == CombatSide.Player)
			{
				if (CombatManager.Instance.PlayersTakingExtraTurn.Count <= 0)
				{
					IEnumerable<Creature> enumerable = base.CombatState.Enemies.Where<Creature>((Creature c) => c.Monster is TurretOperator);
					foreach (Creature creature in enumerable)
					{
						await CreatureCmd.GainBlock(creature, base.Amount, ValueProp.Unpowered, null, false);
					}
					IEnumerator<Creature> enumerator = null;
				}
			}
		}
	}
}
