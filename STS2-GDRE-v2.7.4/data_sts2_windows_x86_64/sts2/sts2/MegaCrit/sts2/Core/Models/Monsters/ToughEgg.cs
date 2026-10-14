using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x02000801 RID: 2049
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ToughEgg : MonsterModel
	{
		// Token: 0x170018E1 RID: 6369
		// (get) Token: 0x0600660B RID: 26123 RVA: 0x0026EC4F File Offset: 0x0026CE4F
		public override LocString Title
		{
			get
			{
				if (!this._hatched)
				{
					return MonsterModel.L10NMonsterLookup(base.Id.Entry + ".name");
				}
				return MonsterModel.L10NMonsterLookup("HATCHLING.name");
			}
		}

		// Token: 0x170018E2 RID: 6370
		// (get) Token: 0x0600660C RID: 26124 RVA: 0x0026EC7E File Offset: 0x0026CE7E
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 15, 14);
			}
		}

		// Token: 0x170018E3 RID: 6371
		// (get) Token: 0x0600660D RID: 26125 RVA: 0x0026EC8A File Offset: 0x0026CE8A
		public override int MaxInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 18);
			}
		}

		// Token: 0x170018E4 RID: 6372
		// (get) Token: 0x0600660E RID: 26126 RVA: 0x0026EC96 File Offset: 0x0026CE96
		public int HatchlingMinHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 20, 19);
			}
		}

		// Token: 0x170018E5 RID: 6373
		// (get) Token: 0x0600660F RID: 26127 RVA: 0x0026ECA2 File Offset: 0x0026CEA2
		public int HatchlingMaxHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 23, 22);
			}
		}

		// Token: 0x170018E6 RID: 6374
		// (get) Token: 0x06006610 RID: 26128 RVA: 0x0026ECAE File Offset: 0x0026CEAE
		private static int NibbleDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
			}
		}

		// Token: 0x170018E7 RID: 6375
		// (get) Token: 0x06006611 RID: 26129 RVA: 0x0026ECB9 File Offset: 0x0026CEB9
		public override string DeathSfx
		{
			get
			{
				if (!this._hatched)
				{
					return "event:/sfx/enemy/enemy_attacks/tough_egg/tough_egg_die";
				}
				return "event:/sfx/enemy/enemy_attacks/tough_egg/hatchling_die";
			}
		}

		// Token: 0x170018E8 RID: 6376
		// (get) Token: 0x06006612 RID: 26130 RVA: 0x0026ECCE File Offset: 0x0026CECE
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Slime;
			}
		}

		// Token: 0x170018E9 RID: 6377
		// (get) Token: 0x06006613 RID: 26131 RVA: 0x0026ECD1 File Offset: 0x0026CED1
		// (set) Token: 0x06006614 RID: 26132 RVA: 0x0026ECD9 File Offset: 0x0026CED9
		[Nullable(2)]
		public MonsterState AfterHatchedState
		{
			[NullableContext(2)]
			get
			{
				return this._afterHatchedState;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._afterHatchedState = value;
			}
		}

		// Token: 0x170018EA RID: 6378
		// (get) Token: 0x06006615 RID: 26133 RVA: 0x0026ECE8 File Offset: 0x0026CEE8
		// (set) Token: 0x06006616 RID: 26134 RVA: 0x0026ECF0 File Offset: 0x0026CEF0
		public bool IsHatched
		{
			get
			{
				return this._isHatched;
			}
			set
			{
				base.AssertMutable();
				this._isHatched = value;
			}
		}

		// Token: 0x170018EB RID: 6379
		// (get) Token: 0x06006617 RID: 26135 RVA: 0x0026ECFF File Offset: 0x0026CEFF
		// (set) Token: 0x06006618 RID: 26136 RVA: 0x0026ED07 File Offset: 0x0026CF07
		public Vector2? HatchPos
		{
			get
			{
				return this._hatchPos;
			}
			set
			{
				base.AssertMutable();
				this._hatchPos = value;
			}
		}

		// Token: 0x06006619 RID: 26137 RVA: 0x0026ED18 File Offset: 0x0026CF18
		public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
			MegaSkin megaSkin = spine.NewSkin("custom-skin");
			megaSkin.AddSkin(skeleton.GetData().FindSkin(base.Rng.NextItem<string>(ToughEgg._eggOptions)));
			skeleton.SetSkin(megaSkin);
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x0600661A RID: 26138 RVA: 0x0026ED60 File Offset: 0x0026CF60
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			if (TestMode.IsOff && this.HatchPos != null)
			{
				NCombatRoom.Instance.GetCreatureNode(base.Creature).GlobalPosition = this.HatchPos.Value;
			}
			if (!this.IsHatched)
			{
				int num = ((base.CombatState.CurrentSide == CombatSide.Enemy) ? 2 : 1);
				await PowerCmd.Apply<HatchPower>(new ThrowingPlayerChoiceContext(), base.Creature, num, base.Creature, null, false);
			}
			else
			{
				await this.Hatch();
				MonsterMoveStateMachine moveStateMachine = base.MoveStateMachine;
				if (moveStateMachine != null)
				{
					moveStateMachine.ForceCurrentState(this.AfterHatchedState);
				}
			}
		}

		// Token: 0x0600661B RID: 26139 RVA: 0x0026EDA4 File Offset: 0x0026CFA4
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("HATCH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HatchMove), new AbstractIntent[]
			{
				new SummonIntent()
			});
			MoveState moveState2 = new MoveState("NIBBLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.NibbleMove), new AbstractIntent[]
			{
				new SingleAttackIntent(ToughEgg.NibbleDamage)
			});
			moveState.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState2;
			list.Add(moveState);
			list.Add(moveState2);
			this.AfterHatchedState = moveState2;
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x0600661C RID: 26140 RVA: 0x0026EE30 File Offset: 0x0026D030
		private async Task HatchMove(IReadOnlyList<Creature> targets)
		{
			this.IsHatched = true;
			await PowerCmd.Remove<HatchPower>(base.Creature);
			this._hatched = true;
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/tough_egg/tough_egg_hatch", 1f);
			List<PowerModel> list = base.Creature.Powers.Where<PowerModel>((PowerModel p) => !(p is MinionPower)).ToList<PowerModel>();
			foreach (PowerModel powerModel in list)
			{
				await PowerCmd.Remove(powerModel);
			}
			List<PowerModel>.Enumerator enumerator = default(List<PowerModel>.Enumerator);
			await this.Hatch();
		}

		// Token: 0x0600661D RID: 26141 RVA: 0x0026EE74 File Offset: 0x0026D074
		private async Task Hatch()
		{
			await CreatureCmd.TriggerAnim(base.Creature, "Hatch", 0.5f);
			decimal num = Creature.ScaleHpForMultiplayer(base.RunRng.Niche.NextInt(this.HatchlingMinHp, this.HatchlingMaxHp), base.CombatState.Encounter, base.CombatState.Players.Count, base.CombatState.RunState.CurrentActIndex);
			await CreatureCmd.SetMaxAndCurrentHp(base.Creature, num);
		}

		// Token: 0x0600661E RID: 26142 RVA: 0x0026EEB8 File Offset: 0x0026D0B8
		private async Task NibbleMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(ToughEgg.NibbleDamage).FromMonster(this).WithAttackerAnim("Attack", 0.2f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_bite", null, null)
				.Execute(null);
		}

		// Token: 0x0600661F RID: 26143 RVA: 0x0026EEFC File Offset: 0x0026D0FC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("hurt", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("die", false);
			AnimState animState5 = new AnimState("egg_spawn", false);
			AnimState animState6 = new AnimState("egg_idle_loop", true);
			AnimState animState7 = new AnimState("egg_hurt", false);
			AnimState animState8 = new AnimState("egg_die", false);
			AnimState animState9 = new AnimState("egg_hatch", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState5.NextState = animState6;
			animState9.NextState = animState;
			animState7.NextState = animState6;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState5, controller);
			creatureAnimator.AddAnyState("Hatch", animState9, null);
			creatureAnimator.AddAnyState("Hit", animState7, () => !this.IsHatched);
			creatureAnimator.AddAnyState("Dead", animState8, () => !this.IsHatched);
			creatureAnimator.AddAnyState("Hit", animState2, () => this.IsHatched);
			creatureAnimator.AddAnyState("Dead", animState4, () => this.IsHatched);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			return creatureAnimator;
		}

		// Token: 0x040026F8 RID: 9976
		private bool _hatched;

		// Token: 0x040026F9 RID: 9977
		private const string _hatchTrigger = "Hatch";

		// Token: 0x040026FA RID: 9978
		private const string _hatchSfx = "event:/sfx/enemy/enemy_attacks/tough_egg/tough_egg_hatch";

		// Token: 0x040026FB RID: 9979
		private static readonly string[] _eggOptions = new string[] { "egg1", "egg2" };

		// Token: 0x040026FC RID: 9980
		[Nullable(2)]
		private MonsterState _afterHatchedState;

		// Token: 0x040026FD RID: 9981
		private bool _isHatched;

		// Token: 0x040026FE RID: 9982
		private Vector2? _hatchPos;
	}
}
