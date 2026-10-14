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
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000696 RID: 1686
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HighVoltagePower : PowerModel
	{
		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x060058F7 RID: 22775 RVA: 0x0024F9DB File Offset: 0x0024DBDB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x060058F8 RID: 22776 RVA: 0x0024F9DE File Offset: 0x0024DBDE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x060058F9 RID: 22777 RVA: 0x0024F9E4 File Offset: 0x0024DBE4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x060058FA RID: 22778 RVA: 0x0024FA04 File Offset: 0x0024DC04
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, base.Amount, base.Owner, null, false);
			}
		}
	}
}
