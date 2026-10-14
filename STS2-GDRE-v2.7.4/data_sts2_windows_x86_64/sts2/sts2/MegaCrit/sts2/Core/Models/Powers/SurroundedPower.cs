using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200070C RID: 1804
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SurroundedPower : PowerModel
	{
		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x06005BB8 RID: 23480 RVA: 0x00254AF2 File Offset: 0x00252CF2
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x06005BB9 RID: 23481 RVA: 0x00254AF5 File Offset: 0x00252CF5
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x06005BBA RID: 23482 RVA: 0x00254AF8 File Offset: 0x00252CF8
		// (set) Token: 0x06005BBB RID: 23483 RVA: 0x00254B00 File Offset: 0x00252D00
		public SurroundedPower.Direction Facing
		{
			get
			{
				return this._facing;
			}
			private set
			{
				base.AssertMutable();
				this._facing = value;
			}
		}

		// Token: 0x06005BBC RID: 23484 RVA: 0x00254B10 File Offset: 0x00252D10
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (dealer == null)
			{
				return 1m;
			}
			if (target != base.Owner)
			{
				return 1m;
			}
			SurroundedPower.Direction facing = this.Facing;
			if (facing != SurroundedPower.Direction.Right)
			{
				if (facing == SurroundedPower.Direction.Left)
				{
					if (!dealer.HasPower<BackAttackRightPower>())
					{
						return 1m;
					}
				}
			}
			else if (!dealer.HasPower<BackAttackLeftPower>())
			{
				return 1m;
			}
			return 1.5m;
		}

		// Token: 0x06005BBD RID: 23485 RVA: 0x00254B78 File Offset: 0x00252D78
		public override async Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (cardPlay.Target != null)
			{
				if (cardPlay.Card.Owner == base.Owner.Player)
				{
					await this.UpdateDirection(cardPlay.Target);
				}
			}
		}

		// Token: 0x06005BBE RID: 23486 RVA: 0x00254BC4 File Offset: 0x00252DC4
		public override async Task BeforePotionUsed(PotionModel potion, [Nullable(2)] Creature target)
		{
			if (CombatManager.Instance.IsInProgress)
			{
				if (target != null)
				{
					if (potion.Owner == base.Owner.Player)
					{
						await this.UpdateDirection(target);
					}
				}
			}
		}

		// Token: 0x06005BBF RID: 23487 RVA: 0x00254C18 File Offset: 0x00252E18
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (creature.Side != base.Owner.Side)
				{
					IReadOnlyList<Creature> hittableEnemies = base.Owner.CombatState.HittableEnemies;
					if (hittableEnemies.Count != 0)
					{
						if (!hittableEnemies.All<Creature>((Creature e) => e.HasPower<BackAttackLeftPower>()))
						{
							if (!hittableEnemies.All<Creature>((Creature e) => e.HasPower<BackAttackRightPower>()))
							{
								goto IL_0109;
							}
						}
						await this.UpdateDirection(hittableEnemies[0]);
						IL_0109:;
					}
				}
			}
		}

		// Token: 0x06005BC0 RID: 23488 RVA: 0x00254C6C File Offset: 0x00252E6C
		private async Task UpdateDirection(Creature target)
		{
			SurroundedPower.Direction facing = this.Facing;
			if (facing != SurroundedPower.Direction.Right)
			{
				if (facing == SurroundedPower.Direction.Left)
				{
					if (target.HasPower<BackAttackRightPower>())
					{
						await this.FaceDirection(SurroundedPower.Direction.Right);
					}
				}
			}
			else if (target.HasPower<BackAttackLeftPower>())
			{
				await this.FaceDirection(SurroundedPower.Direction.Left);
			}
		}

		// Token: 0x06005BC1 RID: 23489 RVA: 0x00254CB8 File Offset: 0x00252EB8
		private async Task FaceDirection(SurroundedPower.Direction direction)
		{
			this.Facing = direction;
			Creature owner = base.Owner;
			IReadOnlyList<Creature> pets = base.Owner.Pets;
			int num = 0;
			Creature[] array = new Creature[1 + pets.Count];
			array[num] = owner;
			num++;
			foreach (Creature creature in pets)
			{
				array[num] = creature;
				num++;
			}
			IEnumerable<Creature> enumerable = new <>z__ReadOnlyArray<Creature>(array);
			IEnumerable<Node2D> enumerable2 = enumerable.Select<Creature, Node2D>(delegate(Creature c)
			{
				NCombatRoom instance2 = NCombatRoom.Instance;
				if (instance2 == null)
				{
					return null;
				}
				NCreature creatureNode = instance2.GetCreatureNode(c);
				if (creatureNode == null)
				{
					return null;
				}
				return creatureNode.Body;
			});
			foreach (Node2D node2D in enumerable2)
			{
				await this.FlipScale(node2D);
			}
			IEnumerator<Node2D> enumerator2 = null;
			if (LocalContext.GetMe(base.Owner.CombatState) == base.Owner.Player)
			{
				NRunMusicController instance = NRunMusicController.Instance;
				if (instance != null)
				{
					instance.UpdateMusicParameter("kaiser_crab_direction", (float)((this.Facing == SurroundedPower.Direction.Left) ? 1 : 2));
				}
			}
		}

		// Token: 0x06005BC2 RID: 23490 RVA: 0x00254D04 File Offset: 0x00252F04
		private Task FlipScale([Nullable(2)] Node2D body)
		{
			if (body == null)
			{
				return Task.CompletedTask;
			}
			float x = body.Scale.X;
			if ((this.Facing == SurroundedPower.Direction.Right && x < 0f) || (this.Facing == SurroundedPower.Direction.Left && x > 0f))
			{
				body.Scale *= new Vector2(-1f, 1f);
			}
			return Task.CompletedTask;
		}

		// Token: 0x0400251E RID: 9502
		private const string _kaiserCrabDirectionCustomTrackName = "kaiser_crab_direction";

		// Token: 0x0400251F RID: 9503
		private SurroundedPower.Direction _facing;

		// Token: 0x02001C87 RID: 7303
		[NullableContext(0)]
		public enum Direction
		{
			// Token: 0x04007289 RID: 29321
			Right,
			// Token: 0x0400728A RID: 29322
			Left
		}
	}
}
