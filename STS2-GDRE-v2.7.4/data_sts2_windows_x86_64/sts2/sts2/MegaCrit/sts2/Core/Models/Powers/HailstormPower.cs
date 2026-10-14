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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000689 RID: 1673
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class HailstormPower : PowerModel
	{
		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x060058AE RID: 22702 RVA: 0x0024F1DB File Offset: 0x0024D3DB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x060058AF RID: 22703 RVA: 0x0024F1DE File Offset: 0x0024D3DE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x060058B0 RID: 22704 RVA: 0x0024F1E1 File Offset: 0x0024D3E1
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromOrb<FrostOrb>());
			}
		}

		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x060058B1 RID: 22705 RVA: 0x0024F1ED File Offset: 0x0024D3ED
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("FrostOrbs", 1m));
			}
		}

		// Token: 0x060058B2 RID: 22706 RVA: 0x0024F204 File Offset: 0x0024D404
		public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				int num = base.Owner.Player.PlayerCombatState.OrbQueue.Orbs.Count<OrbModel>((OrbModel o) => o is FrostOrb);
				if (num >= base.DynamicVars["FrostOrbs"].IntValue)
				{
					base.Flash();
					await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies, base.Amount, ValueProp.Unpowered, base.Owner);
				}
			}
		}

		// Token: 0x040024FB RID: 9467
		public const string frostOrbKey = "FrostOrbs";

		// Token: 0x040024FC RID: 9468
		public const int frostOrbCount = 1;
	}
}
