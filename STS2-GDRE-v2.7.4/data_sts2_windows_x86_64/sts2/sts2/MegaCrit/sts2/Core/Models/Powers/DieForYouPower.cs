using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000662 RID: 1634
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DieForYouPower : PowerModel
	{
		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x060057DD RID: 22493 RVA: 0x0024D967 File Offset: 0x0024BB67
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x060057DE RID: 22494 RVA: 0x0024D96A File Offset: 0x0024BB6A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x060057DF RID: 22495 RVA: 0x0024D96D File Offset: 0x0024BB6D
		public override bool ShouldPlayVfx
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060057E0 RID: 22496 RVA: 0x0024D970 File Offset: 0x0024BB70
		public override Creature ModifyUnblockedDamageTarget(Creature target, decimal _, ValueProp props, [Nullable(2)] Creature __)
		{
			Player petOwner = base.Owner.PetOwner;
			if (target != ((petOwner != null) ? petOwner.Creature : null))
			{
				return target;
			}
			if (base.Owner.IsDead)
			{
				return target;
			}
			if (!props.IsPoweredAttack())
			{
				return target;
			}
			return base.Owner;
		}

		/// <summary>
		/// This is so Osty won't receive powers while it is dead
		/// </summary>
		// Token: 0x060057E1 RID: 22497 RVA: 0x0024D9AD File Offset: 0x0024BBAD
		public override bool ShouldAllowHitting(Creature creature)
		{
			return creature.IsAlive;
		}

		// Token: 0x060057E2 RID: 22498 RVA: 0x0024D9B5 File Offset: 0x0024BBB5
		public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
		{
			return creature != base.Owner;
		}

		// Token: 0x060057E3 RID: 22499 RVA: 0x0024D9C3 File Offset: 0x0024BBC3
		public override bool ShouldPowerBeRemovedAfterOwnerDeath()
		{
			return false;
		}
	}
}
