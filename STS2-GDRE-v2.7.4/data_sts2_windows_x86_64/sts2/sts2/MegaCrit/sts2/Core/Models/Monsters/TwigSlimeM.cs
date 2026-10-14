using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000805 RID: 2053
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TwigSlimeM : MonsterModel
	{
		// Token: 0x170018FF RID: 6399
		// (get) Token: 0x06006652 RID: 26194 RVA: 0x0026F8F3 File Offset: 0x0026DAF3
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 27, 26);
			}
		}

		// Token: 0x17001900 RID: 6400
		// (get) Token: 0x06006653 RID: 26195 RVA: 0x0026F8FF File Offset: 0x0026DAFF
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 29, 28);
			}
		}

		// Token: 0x17001901 RID: 6401
		// (get) Token: 0x06006654 RID: 26196 RVA: 0x0026F90B File Offset: 0x0026DB0B
		protected override bool HasPhobiaSpineSkin
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001902 RID: 6402
		// (get) Token: 0x06006655 RID: 26197 RVA: 0x0026F90E File Offset: 0x0026DB0E
		private int ClumpDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 11);
			}
		}

		// Token: 0x17001903 RID: 6403
		// (get) Token: 0x06006656 RID: 26198 RVA: 0x0026F91B File Offset: 0x0026DB1B
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x06006657 RID: 26199 RVA: 0x0026F920 File Offset: 0x0026DB20
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("POKEY_POUNCE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ClumpShotMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ClumpDamage)
			});
			MoveState moveState2 = new MoveState("STICKY_SHOT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StickyShotMove), new AbstractIntent[]
			{
				new StatusIntent(1)
			});
			RandomBranchState randomBranchState = new RandomBranchState("RAND");
			moveState.FollowUpState = randomBranchState;
			moveState2.FollowUpState = randomBranchState;
			randomBranchState.AddBranch(moveState, 2);
			randomBranchState.AddBranch(moveState2, MoveRepeatType.CannotRepeat);
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(randomBranchState);
			return new MonsterMoveStateMachine(list, moveState2);
		}

		// Token: 0x06006658 RID: 26200 RVA: 0x0026F9CC File Offset: 0x0026DBCC
		private async Task ClumpShotMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ClumpDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_slime_impact", null, null)
				.Execute(null);
		}

		// Token: 0x06006659 RID: 26201 RVA: 0x0026FA10 File Offset: 0x0026DC10
		private async Task StickyShotMove(IReadOnlyList<Creature> targets)
		{
			if (TestMode.IsOff)
			{
				NCreature ncreature = null;
				foreach (Creature creature in targets)
				{
					NCreature creatureNode = creature.GetCreatureNode();
					if (creatureNode != null && (ncreature == null || ncreature.GlobalPosition.X > creatureNode.GlobalPosition.X))
					{
						ncreature = creatureNode;
					}
				}
				NCreature creatureNode2 = base.Creature.GetCreatureNode();
				Node2D node2D = ((creatureNode2 != null) ? creatureNode2.GetSpecialNode<Node2D>("Visuals/SpitTarget") : null);
				if (creatureNode2 != null && node2D != null && ncreature != null)
				{
					node2D.GlobalPosition = new Vector2(ncreature.GlobalPosition.X, node2D.GlobalPosition.Y);
				}
			}
			SfxCmd.Play(this.CastSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.75f);
			VfxCmd.PlayOnCreatureCenters(targets, "vfx/vfx_slime_impact");
			await CardPileCmd.AddToCombatAndPreview<Slimed>(targets, PileType.Discard, 1, null, CardPilePosition.Bottom);
		}

		// Token: 0x0400270C RID: 9996
		private const int _stickyAmount = 1;
	}
}
