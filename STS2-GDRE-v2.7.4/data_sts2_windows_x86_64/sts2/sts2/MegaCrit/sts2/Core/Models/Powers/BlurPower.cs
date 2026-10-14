using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000639 RID: 1593
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BlurPower : PowerModel
	{
		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x060056F2 RID: 22258 RVA: 0x0024BFCF File Offset: 0x0024A1CF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x060056F3 RID: 22259 RVA: 0x0024BFD2 File Offset: 0x0024A1D2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x060056F4 RID: 22260 RVA: 0x0024BFD5 File Offset: 0x0024A1D5
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060056F5 RID: 22261 RVA: 0x0024BFE7 File Offset: 0x0024A1E7
		public override bool ShouldClearBlock(Creature creature)
		{
			return base.Owner != creature;
		}

		// Token: 0x060056F6 RID: 22262 RVA: 0x0024BFF5 File Offset: 0x0024A1F5
		public override Task AfterPreventingBlockClear(AbstractModel preventer, Creature creature)
		{
			if (this != preventer)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x060056F7 RID: 22263 RVA: 0x0024C00C File Offset: 0x0024A20C
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
