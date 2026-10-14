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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007CB RID: 1995
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LeafSlimeM : MonsterModel
	{
		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x0600627E RID: 25214 RVA: 0x0026427B File Offset: 0x0026247B
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 33, 32);
			}
		}

		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x0600627F RID: 25215 RVA: 0x00264287 File Offset: 0x00262487
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 36, 35);
			}
		}

		// Token: 0x17001767 RID: 5991
		// (get) Token: 0x06006280 RID: 25216 RVA: 0x00264293 File Offset: 0x00262493
		private int ClumpDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
			}
		}

		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x06006281 RID: 25217 RVA: 0x0026429F File Offset: 0x0026249F
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x06006282 RID: 25218 RVA: 0x002642A4 File Offset: 0x002624A4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("CLUMP_SHOT", new Func<IReadOnlyList<Creature>, Task>(this.ClumpShotMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.ClumpDamage)
			});
			MoveState moveState2 = new MoveState("STICKY_SHOT", new Func<IReadOnlyList<Creature>, Task>(this.StickyShotMove), new AbstractIntent[]
			{
				new StatusIntent(2)
			});
			moveState2.FollowUpState = moveState;
			moveState.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			return new MonsterMoveStateMachine(list, moveState2);
		}

		// Token: 0x06006283 RID: 25219 RVA: 0x0026432C File Offset: 0x0026252C
		private async Task ClumpShotMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.ClumpDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_slime_impact", null, null)
				.Execute(null);
		}

		// Token: 0x06006284 RID: 25220 RVA: 0x00264370 File Offset: 0x00262570
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
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 1f);
			VfxCmd.PlayOnCreatureCenters(targets, "vfx/vfx_slime_impact");
			await CardPileCmd.AddToCombatAndPreview<Slimed>(targets, PileType.Discard, 2, null, CardPilePosition.Bottom);
		}

		// Token: 0x0400260F RID: 9743
		private const int _stickyAmount = 2;
	}
}
