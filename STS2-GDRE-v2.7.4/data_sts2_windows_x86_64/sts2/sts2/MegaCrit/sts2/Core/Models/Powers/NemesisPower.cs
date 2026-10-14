using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006B4 RID: 1716
	public sealed class NemesisPower : PowerModel
	{
		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x060059A3 RID: 22947 RVA: 0x00250CE3 File Offset: 0x0024EEE3
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x060059A4 RID: 22948 RVA: 0x00250CE6 File Offset: 0x0024EEE6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x060059A5 RID: 22949 RVA: 0x00250CEC File Offset: 0x0024EEEC
		[NullableContext(1)]
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				this._shouldApplyIntangible = !this._shouldApplyIntangible;
				if (this._shouldApplyIntangible)
				{
					base.Flash();
					await PowerCmd.Apply<IntangiblePower>(choiceContext, base.Owner, 1m, base.Owner, null, false);
				}
				else if (base.Owner.HasPower<IntangiblePower>())
				{
					await PowerCmd.Remove(base.Owner.GetPower<IntangiblePower>());
				}
			}
		}

		// Token: 0x04002507 RID: 9479
		private bool _shouldApplyIntangible;
	}
}
