using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000664 RID: 1636
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DoomPower : PowerModel
	{
		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x060057E9 RID: 22505 RVA: 0x0024DA2F File Offset: 0x0024BC2F
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x060057EA RID: 22506 RVA: 0x0024DA32 File Offset: 0x0024BC32
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x060057EB RID: 22507 RVA: 0x0024DA35 File Offset: 0x0024BC35
		public override Color AmountLabelColor
		{
			get
			{
				return PowerModel._normalAmountLabelColor;
			}
		}

		/// <summary>
		/// Kill the specified creatures with the <see cref="T:MegaCrit.Sts2.Core.Models.Powers.DoomPower" /> power.
		/// All creatures being killed in a given Doom trigger (side turn finish, <see cref="T:MegaCrit.Sts2.Core.Models.Cards.EndOfDays" />, etc) should be
		/// passed at the same time here, otherwise we can miss Fatal triggers for reviving powers like
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Powers.ReattachPower" />.
		///
		/// This does extra stuff in addition to a normal <see cref="M:MegaCrit.Sts2.Core.Commands.CreatureCmd.Kill(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Boolean)" /> call:
		/// * Plays special VFX.
		/// * Runs <see cref="M:MegaCrit.Sts2.Core.Hooks.Hook.AfterDiedToDoom(MegaCrit.Sts2.Core.Combat.ICombatState,System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Entities.Creatures.Creature})" />.
		///
		/// It's in a public method so effects like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.EndOfDays" /> can use it.
		/// </summary>
		// Token: 0x060057EC RID: 22508 RVA: 0x0024DA3C File Offset: 0x0024BC3C
		public static async Task DoomKill(IReadOnlyList<Creature> creatures)
		{
			if (creatures.Count != 0)
			{
				ICombatState combatState = creatures.First<Creature>().CombatState;
				foreach (Creature creature in creatures)
				{
					await DoomPower.PlayVfx(creature);
					await CreatureCmd.Kill(creature, false);
					creature = null;
				}
				IEnumerator<Creature> enumerator = null;
				await Hook.AfterDiedToDoom(combatState, creatures);
			}
		}

		// Token: 0x060057ED RID: 22509 RVA: 0x0024DA7F File Offset: 0x0024BC7F
		public static IReadOnlyList<Creature> GetDoomedCreatures(IReadOnlyList<Creature> creatures)
		{
			return creatures.Where<Creature>(delegate(Creature c)
			{
				DoomPower power = c.GetPower<DoomPower>();
				return power != null && power.IsOwnerDoomed();
			}).ToList<Creature>();
		}

		// Token: 0x060057EE RID: 22510 RVA: 0x0024DAAC File Offset: 0x0024BCAC
		public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side != CombatSide.Player)
			{
				if (this.ShouldDoomTrigger(participants))
				{
					await DoomPower.DoomKill(this.GetDoomedCreatures(side));
				}
			}
		}

		// Token: 0x060057EF RID: 22511 RVA: 0x0024DB00 File Offset: 0x0024BD00
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side != CombatSide.Enemy)
			{
				if (this.ShouldDoomTrigger(participants))
				{
					await DoomPower.DoomKill(this.GetDoomedCreatures(side));
				}
			}
		}

		// Token: 0x060057F0 RID: 22512 RVA: 0x0024DB54 File Offset: 0x0024BD54
		private bool ShouldDoomTrigger(IEnumerable<Creature> participants)
		{
			if (CombatManager.Instance.IsOverOrEnding)
			{
				return false;
			}
			if (!participants.Contains(base.Owner))
			{
				return false;
			}
			if (base.Owner.IsDead)
			{
				return false;
			}
			if (!this.IsOwnerDoomed())
			{
				return false;
			}
			IReadOnlyList<Creature> doomedCreatures = this.GetDoomedCreatures(base.Owner.Side);
			return doomedCreatures.First<Creature>() == base.Owner;
		}

		// Token: 0x060057F1 RID: 22513 RVA: 0x0024DBBC File Offset: 0x0024BDBC
		private static async Task PlayVfx(Creature creature)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				bool flag = false;
				if (creature.IsMonster)
				{
					Player player = creature.Player;
					AbstractModel abstractModel;
					flag = Hook.ShouldDie(((player != null) ? player.RunState : null) ?? creature.CombatState.RunState, creature.CombatState, creature, out abstractModel);
					flag = flag && creature.Monster.ShouldDisappearFromDoom;
				}
				DoomPower.StartDoomAnim(ncreature, flag);
				NDoomOverlayVfx orCreate = NDoomOverlayVfx.GetOrCreate();
				if (orCreate != null && !orCreate.IsInsideTree())
				{
					NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(orCreate);
				}
				List<Creature> list = (from c in creature.CombatState.GetTeammatesOf(creature)
					where c.IsAlive
					select c).ToList<Creature>();
				if (flag)
				{
					if (list.Count<Creature>() == 1 && list.First<Creature>() == creature)
					{
						await Cmd.Wait(1.5f, false);
					}
					else
					{
						await Cmd.Wait(0.25f, false);
					}
				}
			}
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x0024DBFF File Offset: 0x0024BDFF
		private IReadOnlyList<Creature> GetDoomedCreatures(CombatSide side)
		{
			return DoomPower.GetDoomedCreatures(base.CombatState.GetCreaturesOnSide(side));
		}

		/// <summary>
		/// Will the owner of this power die to Doom at the end of their turn?
		/// </summary>
		// Token: 0x060057F3 RID: 22515 RVA: 0x0024DC12 File Offset: 0x0024BE12
		private bool IsOwnerDoomed()
		{
			return base.Owner.CurrentHp <= base.Amount;
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x0024DC2C File Offset: 0x0024BE2C
		private unsafe static void StartDoomAnim(NCreature creature, bool shouldDie)
		{
			Task task = null;
			if (shouldDie)
			{
				MonsterModel monster = creature.Entity.Monster;
				if (monster != null)
				{
					monster.OnDieToDoom();
				}
				creature.DisableInteractionForDeath();
				Tween tween = creature.AnimDisableUi();
				tween.TweenCallback(Callable.From(new Action(creature.QueueFreeSafely)));
				task = DoomPower.WaitForTween(tween, creature);
				if (creature.SpineAnimation.IsValid)
				{
					creature.SetAnimationTrigger("Hit");
					using (MegaTrackEntry currentTrack = creature.SpineAnimation.GetCurrentTrack(0))
					{
						if (((currentTrack != null) ? currentTrack.GetAnimationName() : null) == "hurt")
						{
							MonsterModel monster2 = creature.Entity.Monster;
							float num = ((monster2 != null) ? monster2.HurtAnimationTrackOffsetForDoom : 0.1f);
							currentTrack.SetTrackTime(num);
							currentTrack.SetTimeScale(0f);
						}
					}
				}
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					instance.RemoveCreatureNode(creature);
				}
			}
			NDoomVfx ndoomVfx = NDoomVfx.Create(creature.Visuals, creature.Hitbox.GlobalPosition, creature.Hitbox.Size, shouldDie);
			NCombatRoom instance2 = NCombatRoom.Instance;
			if (instance2 != null)
			{
				instance2.CombatVfxContainer.AddChildSafely(ndoomVfx);
			}
			if (shouldDie)
			{
				<>y__InlineArray2<Task> <>y__InlineArray = default(<>y__InlineArray2<Task>);
				*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray2<Task>, Task>(ref <>y__InlineArray, 0) = task;
				*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray2<Task>, Task>(ref <>y__InlineArray, 1) = ndoomVfx.VfxTask;
				creature.DeathAnimationTask = Task.WhenAll(<PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<<>y__InlineArray2<Task>, Task>(in <>y__InlineArray, 2));
			}
		}

		// Token: 0x060057F5 RID: 22517 RVA: 0x0024DDA0 File Offset: 0x0024BFA0
		private static async Task WaitForTween(Tween t, Node owner)
		{
			bool flag = await t.AwaitFinished(owner);
		}
	}
}
