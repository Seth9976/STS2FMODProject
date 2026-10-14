using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace MegaCrit.Sts2.Core.Models.Powers.Mocks
{
	// Token: 0x0200073B RID: 1851
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockPreventDeathPower : PowerModel
	{
		// Token: 0x1700149B RID: 5275
		// (get) Token: 0x06005CFD RID: 23805 RVA: 0x002573C3 File Offset: 0x002555C3
		public override bool IsMock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700149C RID: 5276
		// (get) Token: 0x06005CFE RID: 23806 RVA: 0x002573C6 File Offset: 0x002555C6
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x1700149D RID: 5277
		// (get) Token: 0x06005CFF RID: 23807 RVA: 0x002573C9 File Offset: 0x002555C9
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x06005D00 RID: 23808 RVA: 0x002573CC File Offset: 0x002555CC
		public override bool ShouldDie(Creature creature)
		{
			return creature != base.Owner;
		}

		// Token: 0x06005D01 RID: 23809 RVA: 0x002573DC File Offset: 0x002555DC
		public override async Task AfterPreventingDeath(Creature creature)
		{
			await CreatureCmd.Heal(creature, base.Amount, true);
			await PowerCmd.Remove(this);
		}
	}
}
