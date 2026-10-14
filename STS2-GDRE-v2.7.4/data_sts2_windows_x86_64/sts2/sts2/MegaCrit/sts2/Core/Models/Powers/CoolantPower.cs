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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200064B RID: 1611
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CoolantPower : PowerModel
	{
		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x0600575D RID: 22365 RVA: 0x0024CC0F File Offset: 0x0024AE0F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x0600575E RID: 22366 RVA: 0x0024CC12 File Offset: 0x0024AE12
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x0600575F RID: 22367 RVA: 0x0024CC15 File Offset: 0x0024AE15
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x0024CC28 File Offset: 0x0024AE28
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				int num = (from orb in base.Owner.Player.PlayerCombatState.OrbQueue.Orbs
					group orb by orb.Id).Count<IGrouping<ModelId, OrbModel>>();
				await CreatureCmd.GainBlock(base.Owner, num * base.Amount, ValueProp.Unpowered, null, false);
			}
		}
	}
}
