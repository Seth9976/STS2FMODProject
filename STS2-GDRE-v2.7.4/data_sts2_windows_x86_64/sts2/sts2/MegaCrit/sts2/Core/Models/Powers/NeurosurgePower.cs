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
	// Token: 0x020006B5 RID: 1717
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NeurosurgePower : PowerModel
	{
		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x060059A7 RID: 22951 RVA: 0x00250D47 File Offset: 0x0024EF47
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x060059A8 RID: 22952 RVA: 0x00250D4A File Offset: 0x0024EF4A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x060059A9 RID: 22953 RVA: 0x00250D50 File Offset: 0x0024EF50
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>(null));
			}
		}

		// Token: 0x060059AA RID: 22954 RVA: 0x00250D70 File Offset: 0x0024EF70
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Apply<DoomPower>(new ThrowingPlayerChoiceContext(), base.Owner, base.Amount, base.Owner, null, false);
			}
		}
	}
}
