using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006D7 RID: 1751
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReattachPower : PowerModel
	{
		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x06005A79 RID: 23161 RVA: 0x002525B5 File Offset: 0x002507B5
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x06005A7A RID: 23162 RVA: 0x002525B8 File Offset: 0x002507B8
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x06005A7B RID: 23163 RVA: 0x002525BB File Offset: 0x002507BB
		public override bool ShouldScaleInMultiplayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005A7C RID: 23164 RVA: 0x002525BE File Offset: 0x002507BE
		protected override object InitInternalData()
		{
			return new ReattachPower.Data();
		}

		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x06005A7D RID: 23165 RVA: 0x002525C5 File Offset: 0x002507C5
		private bool IsReviving
		{
			get
			{
				return base.GetInternalData<ReattachPower.Data>().isReviving;
			}
		}

		/// <summary>
		/// Executed when Decimillipede Segment is reattaching at the end of its turn.
		/// </summary>
		// Token: 0x06005A7E RID: 23166 RVA: 0x002525D4 File Offset: 0x002507D4
		public async Task DoReattach()
		{
			if (!this.AreAllOtherSegmentsDead())
			{
				NCombatRoom instance = NCombatRoom.Instance;
				NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(base.Owner) : null);
				if (ncreature != null)
				{
					NDecimillipedeSegmentVfx specialNode = ncreature.GetSpecialNode<NDecimillipedeSegmentVfx>("%NDecimillipedeSegmentVfx");
					if (specialNode != null)
					{
						specialNode.Regenerate();
					}
				}
				base.GetInternalData<ReattachPower.Data>().isReviving = false;
				NCombatRoom instance2 = NCombatRoom.Instance;
				if (instance2 != null)
				{
					instance2.SetCreatureIsInteractable(base.Owner, true);
				}
				await CreatureCmd.Heal(base.Owner, base.Amount, true);
			}
		}

		// Token: 0x06005A7F RID: 23167 RVA: 0x00252618 File Offset: 0x00250818
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (base.Owner == creature)
				{
					if (!this.AreAllOtherSegmentsDead() || !base.Owner.IsDead)
					{
						base.GetInternalData<ReattachPower.Data>().isReviving = true;
						DecimillipedeSegment decimillipedeSegment = creature.Monster as DecimillipedeSegment;
						if (decimillipedeSegment != null)
						{
							base.Owner.Monster.SetMoveImmediate(decimillipedeSegment.DeadState, false);
						}
						NCombatRoom instance = NCombatRoom.Instance;
						if (instance != null)
						{
							instance.SetCreatureIsInteractable(base.Owner, false);
						}
					}
					else
					{
						await Cmd.Wait(0.25f, true);
						this.DoFadeOutOnAllSegments();
					}
				}
			}
		}

		/// <summary>
		/// This is so the Decimillipede Segment doesn't receive powers while it is reviving
		/// </summary>
		// Token: 0x06005A80 RID: 23168 RVA: 0x0025266B File Offset: 0x0025086B
		public override bool ShouldAllowHitting(Creature creature)
		{
			return creature != base.Owner || !this.IsReviving;
		}

		// Token: 0x06005A81 RID: 23169 RVA: 0x00252683 File Offset: 0x00250883
		public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
		{
			return creature != base.Owner;
		}

		// Token: 0x06005A82 RID: 23170 RVA: 0x00252691 File Offset: 0x00250891
		public override bool ShouldPowerBeRemovedAfterOwnerDeath()
		{
			return false;
		}

		/// <summary>
		/// Killing Decimillipede Segment shouldn't trigger fatal unless all other segments are dead too.
		/// </summary>
		// Token: 0x06005A83 RID: 23171 RVA: 0x00252694 File Offset: 0x00250894
		public override bool ShouldOwnerDeathTriggerFatal()
		{
			return this.AreAllOtherSegmentsDead();
		}

		// Token: 0x06005A84 RID: 23172 RVA: 0x0025269C File Offset: 0x0025089C
		private void DoFadeOutOnAllSegments()
		{
			List<NCreature> list = new List<NCreature>();
			foreach (Creature creature in base.CombatState.Enemies)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
				if (ncreature != null)
				{
					ncreature.AnimHideIntent(0.0);
					list.Add(ncreature);
				}
			}
			NMonsterDeathVfx nmonsterDeathVfx = NMonsterDeathVfx.Create(list);
			if (nmonsterDeathVfx != null && list.Count > 0)
			{
				Node parent = list[0].GetParent();
				parent.AddChildSafely(nmonsterDeathVfx);
				parent.MoveChildSafely(nmonsterDeathVfx, list[0].GetIndex(false));
				Task task = TaskHelper.RunSafely(this.PlayVfxAndThenRemoveNodes(nmonsterDeathVfx, list));
				foreach (NCreature ncreature2 in list)
				{
					ncreature2.DeathAnimationTask = task;
					NCombatRoom instance2 = NCombatRoom.Instance;
					if (instance2 != null)
					{
						instance2.RemoveCreatureNode(ncreature2);
					}
				}
			}
		}

		// Token: 0x06005A85 RID: 23173 RVA: 0x002527C4 File Offset: 0x002509C4
		private async Task PlayVfxAndThenRemoveNodes(NMonsterDeathVfx vfx, List<NCreature> creatures)
		{
			await Cmd.Wait(0.25f, true);
			await vfx.PlayVfx();
			foreach (NCreature ncreature in creatures)
			{
				ncreature.QueueFreeSafely();
			}
		}

		// Token: 0x06005A86 RID: 23174 RVA: 0x00252810 File Offset: 0x00250A10
		private IEnumerable<Creature> GetOtherSegments()
		{
			return from c in base.Owner.CombatState.GetTeammatesOf(base.Owner).Except<Creature>(new <>z__ReadOnlySingleElementList<Creature>(base.Owner))
				where c.HasPower<ReattachPower>()
				select c;
		}

		// Token: 0x06005A87 RID: 23175 RVA: 0x00252867 File Offset: 0x00250A67
		private bool AreAllOtherSegmentsDead()
		{
			return this.GetOtherSegments().All<Creature>((Creature s) => s.IsDead);
		}

		// Token: 0x02001C3B RID: 7227
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x040070FD RID: 28925
			public bool isReviving;
		}
	}
}
