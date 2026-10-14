using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007C7 RID: 1991
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KinFollower : MonsterModel
	{
		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x06006213 RID: 25107 RVA: 0x00262C90 File Offset: 0x00260E90
		public unsafe override IEnumerable<string> AssetPaths
		{
			get
			{
				int num = 1;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount<string>(list, num);
				Span<string> span = CollectionsMarshal.AsSpan<string>(list);
				int num2 = 0;
				*span[num2] = SceneHelper.GetScenePath("vfx/vfx_kin_poof");
				List<string> list2 = list;
				return list2.Concat<string>(base.AssetPaths);
			}
		}

		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x06006214 RID: 25108 RVA: 0x00262CD5 File Offset: 0x00260ED5
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_die";
			}
		}

		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x06006215 RID: 25109 RVA: 0x00262CDC File Offset: 0x00260EDC
		public override string HurtSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/the_kin_priest/the_kin_priest_hurt";
			}
		}

		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x06006216 RID: 25110 RVA: 0x00262CE3 File Offset: 0x00260EE3
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 62, 58);
			}
		}

		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x06006217 RID: 25111 RVA: 0x00262CEF File Offset: 0x00260EEF
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 63, 59);
			}
		}

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x06006218 RID: 25112 RVA: 0x00262CFB File Offset: 0x00260EFB
		private int QuickSlashDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 5);
			}
		}

		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x06006219 RID: 25113 RVA: 0x00262D06 File Offset: 0x00260F06
		private int BoomerangDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 2, 2);
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x0600621A RID: 25114 RVA: 0x00262D11 File Offset: 0x00260F11
		private int DanceStrength
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
			}
		}

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x0600621B RID: 25115 RVA: 0x00262D1C File Offset: 0x00260F1C
		// (set) Token: 0x0600621C RID: 25116 RVA: 0x00262D24 File Offset: 0x00260F24
		public bool StartsWithDance
		{
			get
			{
				return this._startsWithDance;
			}
			set
			{
				base.AssertMutable();
				this._startsWithDance = value;
			}
		}

		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x0600621D RID: 25117 RVA: 0x00262D33 File Offset: 0x00260F33
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Fur;
			}
		}

		// Token: 0x0600621E RID: 25118 RVA: 0x00262D38 File Offset: 0x00260F38
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkin megaSkin = spine.NewSkin("custom-skin");
			MegaSkeletonDataResource data = skeleton.GetData();
			MegaSkin megaSkin2 = data.FindSkin(Rng.Chaotic.NextItem<string>(KinFollower._hairOptions));
			megaSkin.AddSkin(megaSkin2);
			skeleton.SetSkin(megaSkin);
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x00262D84 File Offset: 0x00260F84
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<MinionPower>(new ThrowingPlayerChoiceContext(), base.Creature, 1m, base.Creature, null, false);
		}

		// Token: 0x06006220 RID: 25120 RVA: 0x00262DC8 File Offset: 0x00260FC8
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("QUICK_SLASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.QuickSlashMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.QuickSlashDamage)
			});
			MoveState moveState2 = new MoveState("BOOMERANG_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BoomerangMove), new AbstractIntent[]
			{
				new MultiAttackIntent(this.BoomerangDamage, 2)
			});
			MoveState moveState3 = new MoveState("POWER_DANCE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PowerDanceMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			MoveState moveState4 = (this.StartsWithDance ? moveState3 : moveState);
			return new MonsterMoveStateMachine(list, moveState4);
		}

		// Token: 0x06006221 RID: 25121 RVA: 0x00262E98 File Offset: 0x00261098
		private async Task QuickSlashMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.QuickSlashDamage).FromMonster(this).WithAttackerAnim("SlashTrigger", 0.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_quick_slash", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x06006222 RID: 25122 RVA: 0x00262EDC File Offset: 0x002610DC
		private async Task PowerDanceMove(IReadOnlyList<Creature> targets)
		{
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_buff", 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Cast", 0.9f);
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, this.DanceStrength, base.Creature, null, false);
		}

		// Token: 0x06006223 RID: 25123 RVA: 0x00262F20 File Offset: 0x00261120
		private async Task BoomerangMove(IReadOnlyList<Creature> targets)
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
				Node2D node2D = ((creatureNode2 != null) ? creatureNode2.GetSpecialNode<Node2D>("Visuals/AttackDistanceControl") : null);
				if (creatureNode2 != null && node2D != null && ncreature != null)
				{
					float num = 400f * creatureNode2.Visuals.Scale.X;
					node2D.GlobalPosition = new Vector2(ncreature.GlobalPosition.X + num, node2D.GlobalPosition.Y);
				}
			}
			await DamageCmd.Attack(this.BoomerangDamage).WithHitCount(2).FromMonster(this)
				.WithAttackerAnim("BoomerangTrigger", 0.2f, null)
				.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_boomerang_slash", null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.OnlyPlayAnimOnce()
				.Execute(null);
		}

		// Token: 0x06006224 RID: 25124 RVA: 0x00262F6C File Offset: 0x0026116C
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("attack_slash", false);
			AnimState animState3 = new AnimState("attack_boomerang", false);
			AnimState animState4 = new AnimState("buff", false);
			AnimState animState5 = new AnimState("hurt", false);
			AnimState animState6 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("BoomerangTrigger", animState3, null);
			creatureAnimator.AddAnyState("SlashTrigger", animState2, null);
			creatureAnimator.AddAnyState("Cast", animState4, null);
			creatureAnimator.AddAnyState("Dead", animState6, null);
			creatureAnimator.AddAnyState("Hit", animState5, null);
			return creatureAnimator;
		}

		// Token: 0x040025DC RID: 9692
		private static readonly string[] _hairOptions = new string[] { "hair_1", "hair_2", "hair_3" };

		// Token: 0x040025DD RID: 9693
		private const string _kinPoofPath = "vfx/vfx_kin_poof";

		// Token: 0x040025DE RID: 9694
		private const string _slashTrigger = "SlashTrigger";

		// Token: 0x040025DF RID: 9695
		private const string _boomerangTrigger = "BoomerangTrigger";

		// Token: 0x040025E0 RID: 9696
		private const string _quickSlashSfx = "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_quick_slash";

		// Token: 0x040025E1 RID: 9697
		private const string _boomerangSfx = "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_boomerang_slash";

		// Token: 0x040025E2 RID: 9698
		private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/the_kin_minion/the_kin_minion_buff";

		// Token: 0x040025E3 RID: 9699
		private const int _boomerangRepeat = 2;

		// Token: 0x040025E4 RID: 9700
		private bool _startsWithDance;
	}
}
