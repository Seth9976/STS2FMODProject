using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Orbs
{
	// Token: 0x02000C7F RID: 3199
	[NullableContext(1)]
	[Nullable(0)]
	public class OrbQueue
	{
		// Token: 0x170023E2 RID: 9186
		// (get) Token: 0x06008343 RID: 33603 RVA: 0x002C4437 File Offset: 0x002C2637
		public IReadOnlyList<OrbModel> Orbs
		{
			get
			{
				return this._orbs;
			}
		}

		// Token: 0x170023E3 RID: 9187
		// (get) Token: 0x06008344 RID: 33604 RVA: 0x002C443F File Offset: 0x002C263F
		// (set) Token: 0x06008345 RID: 33605 RVA: 0x002C4447 File Offset: 0x002C2647
		public int Capacity { get; private set; }

		// Token: 0x06008346 RID: 33606 RVA: 0x002C4450 File Offset: 0x002C2650
		public OrbQueue(Player owner)
		{
			this._owner = owner;
		}

		// Token: 0x06008347 RID: 33607 RVA: 0x002C446A File Offset: 0x002C266A
		public void Clear()
		{
			this._orbs.Clear();
			this.Capacity = 0;
		}

		// Token: 0x06008348 RID: 33608 RVA: 0x002C447E File Offset: 0x002C267E
		public void AddCapacity(int capacity)
		{
			this.Capacity += capacity;
		}

		// Token: 0x06008349 RID: 33609 RVA: 0x002C448E File Offset: 0x002C268E
		public void RemoveCapacity(int capacity)
		{
			this.Capacity = Math.Max(0, this.Capacity - capacity);
			while (this.Orbs.Count > this.Capacity)
			{
				this.Remove(this._orbs.Last<OrbModel>());
			}
		}

		// Token: 0x0600834A RID: 33610 RVA: 0x002C44CC File Offset: 0x002C26CC
		public async Task<bool> TryEnqueue(OrbModel orb)
		{
			bool flag;
			if (this.Capacity == 0)
			{
				flag = false;
			}
			else
			{
				orb.AssertMutable();
				if (this.Orbs.Count >= this.Capacity)
				{
					throw new InvalidOperationException("OrbQueue is full");
				}
				this._orbs.Add(orb);
				await this.SmallWait();
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600834B RID: 33611 RVA: 0x002C4517 File Offset: 0x002C2717
		public bool Remove(OrbModel orb)
		{
			return this._orbs.Remove(orb);
		}

		// Token: 0x0600834C RID: 33612 RVA: 0x002C4525 File Offset: 0x002C2725
		public void Insert(int idx, OrbModel orb)
		{
			if (idx >= this.Capacity)
			{
				throw new InvalidOperationException("idx cannot be greater than capacity");
			}
			this._orbs.Insert(idx, orb);
		}

		// Token: 0x0600834D RID: 33613 RVA: 0x002C4548 File Offset: 0x002C2748
		public async Task BeforeTurnEnd(PlayerChoiceContext choiceContext)
		{
			foreach (OrbModel orbModel in this.Orbs.ToList<OrbModel>())
			{
				if (this._owner.Creature.CombatState == null)
				{
					return;
				}
				await orbModel.BeforeTurnEndOrbTrigger(choiceContext);
			}
			List<OrbModel>.Enumerator enumerator = default(List<OrbModel>.Enumerator);
		}

		// Token: 0x0600834E RID: 33614 RVA: 0x002C4594 File Offset: 0x002C2794
		public async Task AfterTurnStart(PlayerChoiceContext choiceContext)
		{
			foreach (OrbModel orbModel in this.Orbs.ToList<OrbModel>())
			{
				if (this._owner.Creature.CombatState == null)
				{
					return;
				}
				await orbModel.AfterTurnStartOrbTrigger(choiceContext);
			}
			List<OrbModel>.Enumerator enumerator = default(List<OrbModel>.Enumerator);
		}

		// Token: 0x0600834F RID: 33615 RVA: 0x002C45E0 File Offset: 0x002C27E0
		private async Task SmallWait()
		{
			if (LocalContext.IsMe(this._owner))
			{
				await Cmd.CustomScaledWait(0.1f, 0.25f, false, default(CancellationToken));
			}
			else
			{
				await Cmd.Wait(0.05f, false);
			}
		}

		// Token: 0x04002C6B RID: 11371
		public const int maxCapacity = 10;

		// Token: 0x04002C6C RID: 11372
		private readonly Player _owner;

		// Token: 0x04002C6D RID: 11373
		private readonly List<OrbModel> _orbs = new List<OrbModel>();
	}
}
