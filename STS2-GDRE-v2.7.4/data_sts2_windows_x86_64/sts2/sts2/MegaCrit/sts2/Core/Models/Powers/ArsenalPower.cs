using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200062D RID: 1581
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ArsenalPower : PowerModel
	{
		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x060056B0 RID: 22192 RVA: 0x0024B97E File Offset: 0x00249B7E
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x060056B1 RID: 22193 RVA: 0x0024B981 File Offset: 0x00249B81
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x060056B2 RID: 22194 RVA: 0x0024B984 File Offset: 0x00249B84
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>(null));
			}
		}

		// Token: 0x060056B3 RID: 22195 RVA: 0x0024B9A4 File Offset: 0x00249BA4
		public override async Task AfterCardGeneratedForCombat(CardModel card, [Nullable(2)] Player creator)
		{
			if (creator != null && creator.Creature == base.Owner)
			{
				base.Flash();
				await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner, base.Amount, base.Owner, null, false);
			}
		}
	}
}
