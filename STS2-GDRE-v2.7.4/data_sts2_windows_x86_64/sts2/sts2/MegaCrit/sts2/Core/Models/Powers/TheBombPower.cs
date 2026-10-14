using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000719 RID: 1817
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TheBombPower : PowerModel
	{
		// Token: 0x17001430 RID: 5168
		// (get) Token: 0x06005C2B RID: 23595 RVA: 0x00255DBB File Offset: 0x00253FBB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001431 RID: 5169
		// (get) Token: 0x06005C2C RID: 23596 RVA: 0x00255DBE File Offset: 0x00253FBE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x06005C2D RID: 23597 RVA: 0x00255DC1 File Offset: 0x00253FC1
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x17001433 RID: 5171
		// (get) Token: 0x06005C2E RID: 23598 RVA: 0x00255DC4 File Offset: 0x00253FC4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(40m, ValueProp.Unpowered));
			}
		}

		/// <summary>
		/// Set the amount of damage that this instance of The Bomb will deal.
		/// This is necessary because Amount is used to track the number of turns left until exploding.
		/// </summary>
		/// <param name="damage">Amount of damage to deal</param>
		// Token: 0x06005C2F RID: 23599 RVA: 0x00255DD8 File Offset: 0x00253FD8
		public void SetDamage(decimal damage)
		{
			base.AssertMutable();
			base.DynamicVars.Damage.BaseValue = damage;
		}

		// Token: 0x06005C30 RID: 23600 RVA: 0x00255DF4 File Offset: 0x00253FF4
		public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				if (base.Amount > 1)
				{
					await PowerCmd.Decrement(this);
				}
				else
				{
					base.Flash();
					await Cmd.CustomScaledWait(0.2f, 0.4f, false, default(CancellationToken));
					foreach (Creature creature in base.CombatState.HittableEnemies)
					{
						NCombatRoom instance = NCombatRoom.Instance;
						if (instance != null)
						{
							instance.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(creature));
						}
					}
					await Cmd.CustomScaledWait(0.2f, 0.4f, false, default(CancellationToken));
					await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies, base.DynamicVars.Damage, base.Owner);
					await PowerCmd.Remove(this);
				}
			}
		}
	}
}
