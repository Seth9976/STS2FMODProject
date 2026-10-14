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
	// Token: 0x0200065F RID: 1631
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DemonFormPower : PowerModel
	{
		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x060057CD RID: 22477 RVA: 0x0024D7EB File Offset: 0x0024B9EB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x060057CE RID: 22478 RVA: 0x0024D7EE File Offset: 0x0024B9EE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x060057CF RID: 22479 RVA: 0x0024D7F4 File Offset: 0x0024B9F4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x060057D0 RID: 22480 RVA: 0x0024D814 File Offset: 0x0024BA14
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner, base.Amount, base.Owner, null, false);
			}
		}
	}
}
