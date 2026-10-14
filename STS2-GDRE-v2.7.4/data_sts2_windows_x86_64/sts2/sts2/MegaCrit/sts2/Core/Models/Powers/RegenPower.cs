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
	// Token: 0x020006DA RID: 1754
	public sealed class RegenPower : PowerModel
	{
		// Token: 0x1700136A RID: 4970
		// (get) Token: 0x06005A94 RID: 23188 RVA: 0x00252A43 File Offset: 0x00250C43
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700136B RID: 4971
		// (get) Token: 0x06005A95 RID: 23189 RVA: 0x00252A46 File Offset: 0x00250C46
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x06005A96 RID: 23190 RVA: 0x00252A49 File Offset: 0x00250C49
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005A97 RID: 23191 RVA: 0x00252A4C File Offset: 0x00250C4C
		[NullableContext(1)]
		public override async Task BeforeSideTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				if (!base.Owner.IsDead)
				{
					base.Flash();
					await CreatureCmd.Heal(base.Owner, base.Amount, true);
					await PowerCmd.Decrement(this);
				}
			}
		}
	}
}
