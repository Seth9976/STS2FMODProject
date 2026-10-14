using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000628 RID: 1576
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AdaptablePower : PowerModel
	{
		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x06005692 RID: 22162 RVA: 0x0024B72D File Offset: 0x0024992D
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x06005693 RID: 22163 RVA: 0x0024B730 File Offset: 0x00249930
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x06005694 RID: 22164 RVA: 0x0024B733 File Offset: 0x00249933
		protected override object InitInternalData()
		{
			return new AdaptablePower.Data();
		}

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06005695 RID: 22165 RVA: 0x0024B73A File Offset: 0x0024993A
		private bool IsReviving
		{
			get
			{
				return base.GetInternalData<AdaptablePower.Data>().isReviving;
			}
		}

		// Token: 0x06005696 RID: 22166 RVA: 0x0024B747 File Offset: 0x00249947
		public void DoRevive()
		{
			base.GetInternalData<AdaptablePower.Data>().isReviving = false;
		}

		// Token: 0x06005697 RID: 22167 RVA: 0x0024B758 File Offset: 0x00249958
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (creature == base.Owner)
				{
					TestSubject testSubject = creature.Monster as TestSubject;
					if (testSubject != null)
					{
						base.GetInternalData<AdaptablePower.Data>().isReviving = true;
						await testSubject.TriggerDeadState();
					}
				}
			}
		}

		/// <summary>
		/// This is so the Test Subject doesn't receive powers while it is reviving
		/// </summary>
		// Token: 0x06005698 RID: 22168 RVA: 0x0024B7AB File Offset: 0x002499AB
		public override bool ShouldAllowHitting(Creature creature)
		{
			return creature != base.Owner || !this.IsReviving;
		}

		// Token: 0x06005699 RID: 22169 RVA: 0x0024B7C1 File Offset: 0x002499C1
		public override bool ShouldStopCombatFromEnding()
		{
			return true;
		}

		// Token: 0x0600569A RID: 22170 RVA: 0x0024B7C4 File Offset: 0x002499C4
		public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
		{
			return creature != base.Owner;
		}

		// Token: 0x0600569B RID: 22171 RVA: 0x0024B7D2 File Offset: 0x002499D2
		public override bool ShouldPowerBeRemovedAfterOwnerDeath()
		{
			return false;
		}

		// Token: 0x02001B6F RID: 7023
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006D01 RID: 27905
			public bool isReviving;
		}
	}
}
