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
	// Token: 0x02000718 RID: 1816
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TerritorialPower : PowerModel
	{
		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x06005C26 RID: 23590 RVA: 0x00255D37 File Offset: 0x00253F37
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700142E RID: 5166
		// (get) Token: 0x06005C27 RID: 23591 RVA: 0x00255D3A File Offset: 0x00253F3A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700142F RID: 5167
		// (get) Token: 0x06005C28 RID: 23592 RVA: 0x00255D40 File Offset: 0x00253F40
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x06005C29 RID: 23593 RVA: 0x00255D60 File Offset: 0x00253F60
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
