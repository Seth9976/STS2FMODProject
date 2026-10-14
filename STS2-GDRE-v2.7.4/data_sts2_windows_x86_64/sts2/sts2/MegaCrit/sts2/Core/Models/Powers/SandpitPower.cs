using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006E2 RID: 1762
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SandpitPower : PowerModel
	{
		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x06005AC4 RID: 23236 RVA: 0x00252FEB File Offset: 0x002511EB
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001380 RID: 4992
		// (get) Token: 0x06005AC5 RID: 23237 RVA: 0x00252FEE File Offset: 0x002511EE
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001381 RID: 4993
		// (get) Token: 0x06005AC6 RID: 23238 RVA: 0x00252FF1 File Offset: 0x002511F1
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x06005AC7 RID: 23239 RVA: 0x00252FF4 File Offset: 0x002511F4
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			if (TestMode.IsOn)
			{
				return Task.CompletedTask;
			}
			NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(base.Target);
			this._initialAmount = base.Amount;
			this._initialTargetPosition = creatureNode.GlobalPosition.X;
			return Task.CompletedTask;
		}

		// Token: 0x06005AC8 RID: 23240 RVA: 0x00253044 File Offset: 0x00251244
		public override async Task AfterSideTurnStartLate(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (side == CombatSide.Enemy)
			{
				await PowerCmd.Decrement(this);
			}
		}

		// Token: 0x06005AC9 RID: 23241 RVA: 0x00253090 File Offset: 0x00251290
		public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal _, [Nullable(2)] Creature __, [Nullable(2)] CardModel cardSource)
		{
			if (!TestMode.IsOn)
			{
				if (power == this)
				{
					await this.UpdateCreaturePositions();
					if (LocalContext.IsMe(base.Target))
					{
						int num = Mathf.Clamp(6 - base.Amount, 0, 5);
						NRunMusicController instance = NRunMusicController.Instance;
						if (instance != null)
						{
							instance.UpdateMusicParameter(TheInsatiable.TheInsatiableTrackName, (float)num);
						}
					}
				}
			}
		}

		// Token: 0x06005ACA RID: 23242 RVA: 0x002530DC File Offset: 0x002512DC
		public override async Task AfterRemoved(Creature oldOwner)
		{
			if (!oldOwner.IsDead)
			{
				if (!base.Target.IsDead)
				{
					if (TestMode.IsOff)
					{
						NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(base.Owner);
						Tween tween = NCombatRoom.Instance.CreateTween();
						float num = creatureNode.GlobalPosition.X - 450f;
						foreach (Creature creature in this.AllAffectedCreatures)
						{
							NCreature creatureNode2 = NCombatRoom.Instance.GetCreatureNode(creature);
							tween.Parallel().TweenProperty(creatureNode2, "global_position:x", num, 0.699999988079071).SetEase(2L)
								.SetTrans(1L);
						}
					}
					SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_finisher", 1f);
					await CreatureCmd.TriggerAnim(base.Owner, "EatPlayerTrigger", 0f);
					await Cmd.Wait(0.5f, false);
					foreach (Creature creature2 in this.AllAffectedCreatures)
					{
						if (TestMode.IsOff)
						{
							NCombatRoom.Instance.GetCreatureNode(creature2).Visuals.Visible = false;
						}
						if (creature2.IsPlayer || creature2.Monster is Osty)
						{
							await CreatureCmd.Kill(creature2, true);
						}
					}
					IEnumerator<Creature> enumerator2 = null;
				}
			}
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x00253128 File Offset: 0x00251328
		public override async Task AfterCreatureAddedToCombat(Creature creature)
		{
			if (creature.Side != base.Owner.Side)
			{
				await this.UpdateCreaturePositions();
			}
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x00253174 File Offset: 0x00251374
		public override async Task AfterOstyRevived(Creature osty)
		{
			await this.UpdateCreaturePositions();
		}

		// Token: 0x06005ACD RID: 23245 RVA: 0x002531B8 File Offset: 0x002513B8
		public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Enemy)
			{
				await this.UpdateCreaturePositions();
			}
		}

		// Token: 0x06005ACE RID: 23246 RVA: 0x00253204 File Offset: 0x00251404
		private async Task UpdateCreaturePositions()
		{
			if (!TestMode.IsOn)
			{
				NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(base.Owner);
				NCreature creatureNode2 = NCombatRoom.Instance.GetCreatureNode(base.Target);
				float num = creatureNode.GlobalPosition.X - 450f;
				float num2 = this._initialTargetPosition + 50f;
				float num3 = (num2 - num) / (float)this._initialAmount;
				int num4 = Mathf.Min(base.Amount, this._initialAmount);
				int num5 = Mathf.Max(base.Amount - this._initialAmount, 0);
				NCreature ncreature = creatureNode2;
				float num6 = 0f;
				Creature target = base.Target;
				Player player = ((target != null) ? target.Player : null);
				if (player != null && player.IsOstyAlive && LocalContext.IsMe(base.Target))
				{
					NCreature creatureNode3 = NCombatRoom.Instance.GetCreatureNode(base.Target.Player.Osty);
					ncreature = creatureNode3;
					float x = NCreature.GetOstyOffsetFromPlayer(creatureNode3.Entity).X;
					num2 += x;
					num3 = (num2 - num) / (float)this._initialAmount;
					float num7 = 100f * (1f - (float)num4 / (float)this._initialAmount);
					num6 = x - num7;
				}
				float num8 = creatureNode.GlobalPosition.X - 400f + num3 * (float)num4 + num3 * ((float)num5 / ((float)num5 + 2f));
				Tween tween = null;
				foreach (Creature creature in this.AllAffectedCreatures)
				{
					float num9 = num8 - ncreature.GlobalPosition.X;
					if (creature != ncreature.Entity)
					{
						num9 = num8 - num6 - creatureNode2.GlobalPosition.X;
					}
					if (Math.Abs(num9) > 5f && !creature.IsDead)
					{
						NCreature creatureNode4 = NCombatRoom.Instance.GetCreatureNode(creature);
						if (creatureNode4 != null)
						{
							if (tween == null)
							{
								tween = NCombatRoom.Instance.CreateTween().SetParallel(true).SetEase(1L)
									.SetTrans(7L);
							}
							tween.TweenProperty(creatureNode4, "global_position:x", creatureNode4.GlobalPosition.X + num9, 0.25);
						}
					}
				}
				if (tween != null)
				{
					bool flag = await tween.AwaitFinished(NCombatRoom.Instance);
				}
			}
		}

		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x06005ACF RID: 23247 RVA: 0x00253248 File Offset: 0x00251448
		private IReadOnlyList<Creature> AllAffectedCreatures
		{
			get
			{
				Creature creature = base.Target.Player.Creature;
				IReadOnlyList<Creature> pets = base.Target.Pets;
				int num = 0;
				Creature[] array = new Creature[1 + pets.Count];
				array[num] = creature;
				num++;
				foreach (Creature creature2 in pets)
				{
					array[num] = creature2;
					num++;
				}
				return new <>z__ReadOnlyArray<Creature>(array);
			}
		}

		// Token: 0x04002510 RID: 9488
		private const float _paddingDistanceFromMonster = 450f;

		// Token: 0x04002511 RID: 9489
		private const float _paddingDistanceFromOriginal = 50f;

		// Token: 0x04002512 RID: 9490
		private const float _tweenTime = 0.25f;

		// Token: 0x04002513 RID: 9491
		private int _initialAmount;

		// Token: 0x04002514 RID: 9492
		private float _initialTargetPosition;
	}
}
