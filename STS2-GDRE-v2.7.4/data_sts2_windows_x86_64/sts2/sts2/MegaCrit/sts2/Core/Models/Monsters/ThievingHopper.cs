using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Models.Monsters
{
	// Token: 0x020007FE RID: 2046
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ThievingHopper : MonsterModel
	{
		// Token: 0x170018C7 RID: 6343
		// (get) Token: 0x060065C1 RID: 26049 RVA: 0x0026DD32 File Offset: 0x0026BF32
		public override int MinInitialHp
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 84, 79);
			}
		}

		// Token: 0x170018C8 RID: 6344
		// (get) Token: 0x060065C2 RID: 26050 RVA: 0x0026DD3E File Offset: 0x0026BF3E
		public override int MaxInitialHp
		{
			get
			{
				return this.MinInitialHp;
			}
		}

		// Token: 0x170018C9 RID: 6345
		// (get) Token: 0x060065C3 RID: 26051 RVA: 0x0026DD46 File Offset: 0x0026BF46
		// (set) Token: 0x060065C4 RID: 26052 RVA: 0x0026DD4E File Offset: 0x0026BF4E
		public bool IsHovering
		{
			get
			{
				return this._isHovering;
			}
			set
			{
				base.AssertMutable();
				this._isHovering = value;
			}
		}

		// Token: 0x170018CA RID: 6346
		// (get) Token: 0x060065C5 RID: 26053 RVA: 0x0026DD5D File Offset: 0x0026BF5D
		protected override string AttackSfx
		{
			get
			{
				if (!this.IsHovering)
				{
					return "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_attack";
				}
				return "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_attack_hover";
			}
		}

		// Token: 0x170018CB RID: 6347
		// (get) Token: 0x060065C6 RID: 26054 RVA: 0x0026DD72 File Offset: 0x0026BF72
		private string FleeSfx
		{
			get
			{
				if (!this.IsHovering)
				{
					return "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_flee";
				}
				return "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_flee_hover";
			}
		}

		// Token: 0x170018CC RID: 6348
		// (get) Token: 0x060065C7 RID: 26055 RVA: 0x0026DD87 File Offset: 0x0026BF87
		public override string DeathSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_die";
			}
		}

		// Token: 0x170018CD RID: 6349
		// (get) Token: 0x060065C8 RID: 26056 RVA: 0x0026DD8E File Offset: 0x0026BF8E
		public override DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Insect;
			}
		}

		// Token: 0x170018CE RID: 6350
		// (get) Token: 0x060065C9 RID: 26057 RVA: 0x0026DD91 File Offset: 0x0026BF91
		public override string TakeDamageSfx
		{
			get
			{
				if (!this.IsHovering)
				{
					return base.TakeDamageSfx;
				}
				return "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hurt_hover";
			}
		}

		// Token: 0x060065CA RID: 26058 RVA: 0x0026DDA8 File Offset: 0x0026BFA8
		public override async Task AfterAddedToRoom()
		{
			await base.AfterAddedToRoom();
			await PowerCmd.Apply<EscapeArtistPower>(new ThrowingPlayerChoiceContext(), base.Creature, 5m, base.Creature, null, false);
		}

		// Token: 0x060065CB RID: 26059 RVA: 0x0026DDEB File Offset: 0x0026BFEB
		public override void BeforeRemovedFromRoom()
		{
			SfxCmd.StopLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hover_loop");
		}

		// Token: 0x170018CF RID: 6351
		// (get) Token: 0x060065CC RID: 26060 RVA: 0x0026DDFD File Offset: 0x0026BFFD
		private int TheftDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 17);
			}
		}

		// Token: 0x170018D0 RID: 6352
		// (get) Token: 0x060065CD RID: 26061 RVA: 0x0026DE0A File Offset: 0x0026C00A
		private int HatTrickDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 23, 21);
			}
		}

		// Token: 0x170018D1 RID: 6353
		// (get) Token: 0x060065CE RID: 26062 RVA: 0x0026DE17 File Offset: 0x0026C017
		private int NabDamage
		{
			get
			{
				return AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16, 14);
			}
		}

		// Token: 0x060065CF RID: 26063 RVA: 0x0026DE24 File Offset: 0x0026C024
		protected override MonsterMoveStateMachine GenerateMoveStateMachine()
		{
			List<MonsterState> list = new List<MonsterState>();
			MoveState moveState = new MoveState("THIEVERY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThieveryMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.TheftDamage),
				new CardDebuffIntent()
			});
			MoveState moveState2 = new MoveState("NAB_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.NabMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.NabDamage)
			});
			MoveState moveState3 = new MoveState("HAT_TRICK_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HatTrickMove), new AbstractIntent[]
			{
				new SingleAttackIntent(this.HatTrickDamage)
			});
			MoveState moveState4 = new MoveState("FLUTTER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FlutterMove), new AbstractIntent[]
			{
				new BuffIntent()
			});
			MoveState moveState5 = new MoveState("ESCAPE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EscapeMove), new AbstractIntent[]
			{
				new EscapeIntent()
			});
			moveState.FollowUpState = moveState4;
			moveState4.FollowUpState = moveState3;
			moveState3.FollowUpState = moveState2;
			moveState2.FollowUpState = moveState5;
			moveState5.FollowUpState = moveState5;
			list.Add(moveState);
			list.Add(moveState2);
			list.Add(moveState3);
			list.Add(moveState4);
			list.Add(moveState5);
			return new MonsterMoveStateMachine(list, moveState);
		}

		// Token: 0x060065D0 RID: 26064 RVA: 0x0026DF60 File Offset: 0x0026C160
		private async Task ThieveryMove(IReadOnlyList<Creature> targets)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature creatureNode = ((instance != null) ? instance.GetCreatureNode(base.Creature) : null);
			if (creatureNode != null)
			{
				Creature creature = LocalContext.GetMe(targets) ?? targets.First<Creature>();
				NCreature creatureNode2 = creature.GetCreatureNode();
				Node2D specialNode = creatureNode.GetSpecialNode<Node2D>("Visuals/SpineBoneNode");
				if (specialNode != null && creatureNode2 != null)
				{
					float num = 900f * creatureNode.Visuals.Scale.X;
					specialNode.GlobalPosition = new Vector2(creatureNode2.GlobalPosition.X + num, specialNode.GlobalPosition.Y);
				}
			}
			await CreatureCmd.TriggerAnim(base.Creature, "Steal", 0.25f);
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_steal", 1f);
			List<CardModel> cardsToSteal = new List<CardModel>();
			foreach (Creature creature2 in targets)
			{
				if (!creature2.IsDead)
				{
					IEnumerable<CardModel> enumerable;
					List<CardModel> list = (enumerable = (from c in CardPile.GetCards(creature2.Player ?? creature2.PetOwner, new PileType[]
						{
							PileType.Draw,
							PileType.Discard
						})
						where c.DeckVersion != null
						select c).ToList<CardModel>());
					Func<CardModel, bool>[] stealPriorities = ThievingHopper._stealPriorities;
					for (int i = 0; i < stealPriorities.Length; i++)
					{
						IEnumerable<CardModel> enumerable2 = list.Where<CardModel>(stealPriorities[i]);
						if (enumerable2.Any<CardModel>())
						{
							enumerable = enumerable2;
							break;
						}
					}
					if (enumerable.Any<CardModel>())
					{
						CardModel cardToSteal = base.RunRng.CombatCardGeneration.NextItem<CardModel>(enumerable);
						await CardPileCmd.RemoveFromCombat(cardToSteal, false);
						cardsToSteal.Add(cardToSteal);
						cardToSteal = null;
					}
				}
			}
			IEnumerator<Creature> enumerator = null;
			await Cmd.Wait(0.6f, false);
			foreach (CardModel cardModel in cardsToSteal)
			{
				if (creatureNode != null && LocalContext.IsMine(cardModel))
				{
					Marker2D specialNode2 = creatureNode.GetSpecialNode<Marker2D>("%StolenCardPos");
					if (specialNode2 != null)
					{
						NCard ncard = NCard.Create(cardModel, ModelVisibility.Visible);
						specialNode2.AddChildSafely(ncard);
						ncard.Position += ncard.Size * 0.5f;
						ncard.UpdateVisuals(PileType.Deck, CardPreviewMode.Normal);
					}
				}
				SwipePower swipe = (SwipePower)ModelDb.Power<SwipePower>().ToMutable(0);
				await swipe.Steal(cardModel);
				await PowerCmd.Apply(new ThrowingPlayerChoiceContext(), swipe, base.Creature, 1m, base.Creature, null, false);
				swipe = null;
			}
			List<CardModel>.Enumerator enumerator2 = default(List<CardModel>.Enumerator);
			await DamageCmd.Attack(this.TheftDamage).FromMonster(this).WithNoAttackerAnim()
				.WithHitFx("vfx/vfx_attack_blunt", null, null)
				.Execute(null);
		}

		// Token: 0x060065D1 RID: 26065 RVA: 0x0026DFAC File Offset: 0x0026C1AC
		private async Task NabMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.NabDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060065D2 RID: 26066 RVA: 0x0026DFF0 File Offset: 0x0026C1F0
		private async Task HatTrickMove(IReadOnlyList<Creature> targets)
		{
			await DamageCmd.Attack(this.HatTrickDamage).FromMonster(this).WithAttackerAnim("Attack", 0.3f, null)
				.WithAttackerFx(null, this.AttackSfx, null)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(null);
		}

		// Token: 0x060065D3 RID: 26067 RVA: 0x0026E034 File Offset: 0x0026C234
		private async Task FlutterMove(IReadOnlyList<Creature> targets)
		{
			this.IsHovering = true;
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_take_off", 1f);
			SfxCmd.PlayLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hover_loop");
			await CreatureCmd.TriggerAnim(base.Creature, "Hover", 0f);
			await Cmd.Wait(1.25f, false);
			await PowerCmd.Apply<FlutterPower>(new ThrowingPlayerChoiceContext(), base.Creature, 5m, base.Creature, null, false);
		}

		// Token: 0x060065D4 RID: 26068 RVA: 0x0026E078 File Offset: 0x0026C278
		private async Task EscapeMove(IReadOnlyList<Creature> targets)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				NCreature creatureNode = instance.GetCreatureNode(base.Creature);
				if (creatureNode != null)
				{
					creatureNode.ToggleIsInteractable(false);
				}
			}
			SfxCmd.Play(this.FleeSfx, 1f);
			await CreatureCmd.TriggerAnim(base.Creature, "Flee", 0.85f);
			if (this.IsHovering)
			{
				SfxCmd.StopLoop(base.Creature, "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hover_loop");
				this.IsHovering = false;
			}
			await Cmd.Wait(1.5f, false);
			await CreatureCmd.Escape(base.Creature, true);
		}

		// Token: 0x060065D5 RID: 26069 RVA: 0x0026E0BC File Offset: 0x0026C2BC
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true)
			{
				BoundsContainer = "GroundedBounds"
			};
			AnimState animState2 = new AnimState("flee", false);
			AnimState animState3 = new AnimState("flee_hover", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("hurt_hover", false);
			AnimState animState6 = new AnimState("attack", false);
			AnimState animState7 = new AnimState("attack_hover", false);
			AnimState animState8 = new AnimState("die", false);
			AnimState animState9 = new AnimState("take_off", false);
			AnimState animState10 = new AnimState("hover_loop", true)
			{
				BoundsContainer = "FlyingBounds"
			};
			AnimState animState11 = new AnimState("steal", false);
			animState9.NextState = animState10;
			animState11.NextState = animState;
			animState4.NextState = animState;
			animState5.NextState = animState10;
			animState6.NextState = animState;
			animState7.NextState = animState10;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("StunTrigger", animState, null);
			creatureAnimator.AddAnyState("Hover", animState9, null);
			creatureAnimator.AddAnyState("Steal", animState11, null);
			creatureAnimator.AddAnyState("Dead", animState8, null);
			creatureAnimator.AddAnyState("Hit", animState5, () => this.IsHovering);
			creatureAnimator.AddAnyState("Hit", animState4, () => !this.IsHovering);
			creatureAnimator.AddAnyState("Attack", animState7, () => this.IsHovering);
			creatureAnimator.AddAnyState("Attack", animState6, () => !this.IsHovering);
			creatureAnimator.AddAnyState("Flee", animState3, () => this.IsHovering);
			creatureAnimator.AddAnyState("Flee", animState2, () => !this.IsHovering);
			return creatureAnimator;
		}

		/// <summary>
		/// The priorities used to determine which card to steal from the player's deck.
		/// </summary>
		// Token: 0x040026E5 RID: 9957
		private static readonly Func<CardModel, bool>[] _stealPriorities = new Func<CardModel, bool>[]
		{
			(CardModel c) => !(c.Enchantment is Imbued) && c.Rarity == CardRarity.Uncommon,
			delegate(CardModel c)
			{
				bool flag = !(c.Enchantment is Imbued);
				bool flag2 = flag;
				if (flag2)
				{
					bool flag3;
					switch (c.Rarity)
					{
					case CardRarity.Common:
					case CardRarity.Rare:
					case CardRarity.Event:
						flag3 = true;
						goto IL_0042;
					}
					flag3 = false;
					IL_0042:
					flag2 = flag3;
				}
				return flag2;
			},
			delegate(CardModel c)
			{
				bool flag4 = !(c.Enchantment is Imbued);
				bool flag5 = flag4;
				if (flag5)
				{
					CardRarity rarity = c.Rarity;
					bool flag6 = rarity == CardRarity.Basic || rarity == CardRarity.Quest;
					flag5 = flag6;
				}
				return flag5;
			},
			(CardModel c) => c.Rarity == CardRarity.Ancient || c.Enchantment is Imbued
		};

		// Token: 0x040026E6 RID: 9958
		public const string stunTrigger = "StunTrigger";

		// Token: 0x040026E7 RID: 9959
		private bool _isHovering;

		// Token: 0x040026E8 RID: 9960
		private const string _fleeTrigger = "Flee";

		// Token: 0x040026E9 RID: 9961
		private const string _hoverTrigger = "Hover";

		// Token: 0x040026EA RID: 9962
		private const string _stealTrigger = "Steal";

		// Token: 0x040026EB RID: 9963
		private const string _escapeMoveId = "ESCAPE_MOVE";

		// Token: 0x040026EC RID: 9964
		private const string _stealSfx = "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_steal";

		// Token: 0x040026ED RID: 9965
		private const string _takeOffSfx = "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_take_off";

		// Token: 0x040026EE RID: 9966
		public const string hoverLoop = "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hover_loop";
	}
}
