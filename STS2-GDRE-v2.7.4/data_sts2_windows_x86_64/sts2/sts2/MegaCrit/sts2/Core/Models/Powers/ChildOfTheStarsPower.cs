using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000643 RID: 1603
	public sealed class ChildOfTheStarsPower : PowerModel
	{
		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x06005731 RID: 22321 RVA: 0x0024C6DA File Offset: 0x0024A8DA
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x06005732 RID: 22322 RVA: 0x0024C6DD File Offset: 0x0024A8DD
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005733 RID: 22323 RVA: 0x0024C6E0 File Offset: 0x0024A8E0
		[NullableContext(1)]
		public override async Task AfterStarsSpent(int amount, Player spender)
		{
			if (amount > 0)
			{
				if (spender == base.Owner.Player)
				{
					base.Flash();
					await CreatureCmd.GainBlock(base.Owner, base.Amount * amount, ValueProp.Unpowered, null, false);
				}
			}
		}
	}
}
