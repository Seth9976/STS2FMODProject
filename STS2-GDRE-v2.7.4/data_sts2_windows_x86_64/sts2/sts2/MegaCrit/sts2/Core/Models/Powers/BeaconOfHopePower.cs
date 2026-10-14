using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000635 RID: 1589
	public sealed class BeaconOfHopePower : PowerModel
	{
		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x060056DB RID: 22235 RVA: 0x0024BD4F File Offset: 0x00249F4F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x060056DC RID: 22236 RVA: 0x0024BD52 File Offset: 0x00249F52
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		/// <summary>
		/// Prevents Beacon of Hope from triggering if it already has triggered for this instance of block.
		/// This is to prevent Beacon of Hope infinitely triggering against another player Beacon of Hope.
		/// </summary>
		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x060056DD RID: 22237 RVA: 0x0024BD55 File Offset: 0x00249F55
		// (set) Token: 0x060056DE RID: 22238 RVA: 0x0024BD5D File Offset: 0x00249F5D
		private bool HasAlreadyBeenGivenBlock
		{
			get
			{
				return this._hasAlreadyBeenGivenBlock;
			}
			set
			{
				base.AssertMutable();
				this._hasAlreadyBeenGivenBlock = value;
			}
		}

		// Token: 0x060056DF RID: 22239 RVA: 0x0024BD6C File Offset: 0x00249F6C
		[NullableContext(1)]
		public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, [Nullable(2)] CardModel cardSource)
		{
			if (!(amount < 1m))
			{
				if (creature == base.Owner)
				{
					if (base.CombatState.CurrentSide == base.Owner.Side)
					{
						if (!this.HasAlreadyBeenGivenBlock)
						{
							decimal amountToGive = amount * 0.5m;
							if (!(amountToGive < 1m))
							{
								IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Owner)
									where c != null && c.IsAlive && c.IsPlayer && c.Player.Creature != base.Owner
									select c;
								this.HasAlreadyBeenGivenBlock = true;
								foreach (Creature creature2 in enumerable)
								{
									await CreatureCmd.GainBlock(creature2, amountToGive, ValueProp.Unpowered, null, false);
								}
								IEnumerator<Creature> enumerator = null;
								this.HasAlreadyBeenGivenBlock = false;
							}
						}
					}
				}
			}
		}

		// Token: 0x040024F2 RID: 9458
		private bool _hasAlreadyBeenGivenBlock;
	}
}
