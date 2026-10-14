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
	// Token: 0x02000731 RID: 1841
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WraithFormPower : PowerModel
	{
		// Token: 0x17001477 RID: 5239
		// (get) Token: 0x06005CB8 RID: 23736 RVA: 0x00256FE7 File Offset: 0x002551E7
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001478 RID: 5240
		// (get) Token: 0x06005CB9 RID: 23737 RVA: 0x00256FEA File Offset: 0x002551EA
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001479 RID: 5241
		// (get) Token: 0x06005CBA RID: 23738 RVA: 0x00256FF0 File Offset: 0x002551F0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>(null));
			}
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x00257010 File Offset: 0x00255210
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner))
			{
				base.Flash();
				await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), base.Owner, -base.Amount, base.Owner, null, false);
			}
		}
	}
}
