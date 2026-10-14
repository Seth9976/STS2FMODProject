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
	// Token: 0x02000636 RID: 1590
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BiasedCognitionPower : PowerModel
	{
		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x060056E2 RID: 22242 RVA: 0x0024BDF4 File Offset: 0x00249FF4
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x060056E3 RID: 22243 RVA: 0x0024BDF7 File Offset: 0x00249FF7
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x060056E4 RID: 22244 RVA: 0x0024BDFC File Offset: 0x00249FFC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>(null));
			}
		}

		// Token: 0x060056E5 RID: 22245 RVA: 0x0024BE1C File Offset: 0x0024A01C
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await PowerCmd.Apply<FocusPower>(new ThrowingPlayerChoiceContext(), base.Owner, -base.Amount, base.Owner, null, false);
			}
		}
	}
}
