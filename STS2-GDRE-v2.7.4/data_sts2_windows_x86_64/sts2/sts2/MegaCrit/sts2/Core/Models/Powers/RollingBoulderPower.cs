using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006DF RID: 1759
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RollingBoulderPower : PowerModel
	{
		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x06005AB1 RID: 23217 RVA: 0x00252DC3 File Offset: 0x00250FC3
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x06005AB2 RID: 23218 RVA: 0x00252DC6 File Offset: 0x00250FC6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001378 RID: 4984
		// (get) Token: 0x06005AB3 RID: 23219 RVA: 0x00252DC9 File Offset: 0x00250FC9
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x06005AB4 RID: 23220 RVA: 0x00252DCC File Offset: 0x00250FCC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(5m, ValueProp.Unpowered));
			}
		}

		// Token: 0x06005AB5 RID: 23221 RVA: 0x00252DE0 File Offset: 0x00250FE0
		public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
		{
			if (player == base.Owner.Player)
			{
				base.Flash();
				if (TestMode.IsOn)
				{
					await this.DoDamage(choiceContext, base.CombatState.HittableEnemies);
				}
				else
				{
					List<Task> damageTasks = new List<Task>();
					NRollingBoulderVfx nrollingBoulderVfx = NRollingBoulderVfx.Create(base.CombatState.HittableEnemies, base.Amount, null);
					nrollingBoulderVfx.Connect(NRollingBoulderVfx.SignalName.HitCreature, Callable.From<NCreature>(delegate(NCreature c)
					{
						damageTasks.Add(this.DoDamage(choiceContext, new <>z__ReadOnlySingleElementList<Creature>(c.Entity)));
					}), 0U);
					SignalAwaiter signalAwaiter = nrollingBoulderVfx.ToSignal(nrollingBoulderVfx, NRollingBoulderVfx.SignalName.Finished);
					NCombatRoom instance = NCombatRoom.Instance;
					if (instance != null)
					{
						instance.CombatVfxContainer.CallDeferred(Node.MethodName.AddChild, new Variant[] { nrollingBoulderVfx });
					}
					await signalAwaiter;
					await Task.WhenAll(damageTasks);
				}
				base.SetAmount(base.Amount + base.DynamicVars.Damage.IntValue, false);
			}
		}

		// Token: 0x06005AB6 RID: 23222 RVA: 0x00252E33 File Offset: 0x00251033
		private Task<IEnumerable<DamageResult>> DoDamage(PlayerChoiceContext choiceContext, IEnumerable<Creature> targets)
		{
			return CreatureCmd.Damage(choiceContext, targets, base.Amount, ValueProp.Unpowered, base.Owner);
		}
	}
}
