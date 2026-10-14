using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Entities.Creatures
{
	// Token: 0x02000CA1 RID: 3233
	[NullableContext(1)]
	[Nullable(0)]
	public class Creature
	{
		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x060083FA RID: 33786 RVA: 0x002C8880 File Offset: 0x002C6A80
		// (remove) Token: 0x060083FB RID: 33787 RVA: 0x002C88B8 File Offset: 0x002C6AB8
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<int, int> BlockChanged;

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x060083FC RID: 33788 RVA: 0x002C88F0 File Offset: 0x002C6AF0
		// (remove) Token: 0x060083FD RID: 33789 RVA: 0x002C8928 File Offset: 0x002C6B28
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<int, int> CurrentHpChanged;

		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x060083FE RID: 33790 RVA: 0x002C8960 File Offset: 0x002C6B60
		// (remove) Token: 0x060083FF RID: 33791 RVA: 0x002C8998 File Offset: 0x002C6B98
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<int, int> MaxHpChanged;

		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06008400 RID: 33792 RVA: 0x002C89D0 File Offset: 0x002C6BD0
		// (remove) Token: 0x06008401 RID: 33793 RVA: 0x002C8A08 File Offset: 0x002C6C08
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<PowerModel> PowerApplied;

		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x06008402 RID: 33794 RVA: 0x002C8A40 File Offset: 0x002C6C40
		// (remove) Token: 0x06008403 RID: 33795 RVA: 0x002C8A78 File Offset: 0x002C6C78
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<PowerModel, int, bool> PowerIncreased;

		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x06008404 RID: 33796 RVA: 0x002C8AB0 File Offset: 0x002C6CB0
		// (remove) Token: 0x06008405 RID: 33797 RVA: 0x002C8AE8 File Offset: 0x002C6CE8
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<PowerModel, bool> PowerDecreased;

		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x06008406 RID: 33798 RVA: 0x002C8B20 File Offset: 0x002C6D20
		// (remove) Token: 0x06008407 RID: 33799 RVA: 0x002C8B58 File Offset: 0x002C6D58
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<PowerModel> PowerRemoved;

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x06008408 RID: 33800 RVA: 0x002C8B90 File Offset: 0x002C6D90
		// (remove) Token: 0x06008409 RID: 33801 RVA: 0x002C8BC8 File Offset: 0x002C6DC8
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<Creature> Died;

		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x0600840A RID: 33802 RVA: 0x002C8C00 File Offset: 0x002C6E00
		// (remove) Token: 0x0600840B RID: 33803 RVA: 0x002C8C38 File Offset: 0x002C6E38
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<Creature> Revived;

		// Token: 0x17002408 RID: 9224
		// (get) Token: 0x0600840C RID: 33804 RVA: 0x002C8C6D File Offset: 0x002C6E6D
		// (set) Token: 0x0600840D RID: 33805 RVA: 0x002C8C78 File Offset: 0x002C6E78
		public int Block
		{
			get
			{
				return this._block;
			}
			private set
			{
				if (value < 0)
				{
					throw new ArgumentException("Block must be positive", "value");
				}
				if (this._block == value)
				{
					return;
				}
				int block = this._block;
				this._block = value;
				Action<int, int> blockChanged = this.BlockChanged;
				if (blockChanged == null)
				{
					return;
				}
				blockChanged(block, this._block);
			}
		}

		// Token: 0x17002409 RID: 9225
		// (get) Token: 0x0600840E RID: 33806 RVA: 0x002C8CC8 File Offset: 0x002C6EC8
		// (set) Token: 0x0600840F RID: 33807 RVA: 0x002C8CD0 File Offset: 0x002C6ED0
		public int CurrentHp
		{
			get
			{
				return this._currentHp;
			}
			private set
			{
				if (value < 0)
				{
					throw new ArgumentException("Current HP must be positive", "value");
				}
				if (this._currentHp == value)
				{
					return;
				}
				int currentHp = this._currentHp;
				this._currentHp = value;
				Action<int, int> currentHpChanged = this.CurrentHpChanged;
				if (currentHpChanged == null)
				{
					return;
				}
				currentHpChanged(currentHp, this._currentHp);
			}
		}

		// Token: 0x1700240A RID: 9226
		// (get) Token: 0x06008410 RID: 33808 RVA: 0x002C8D20 File Offset: 0x002C6F20
		// (set) Token: 0x06008411 RID: 33809 RVA: 0x002C8D28 File Offset: 0x002C6F28
		public int MaxHp
		{
			get
			{
				return this._maxHp;
			}
			private set
			{
				if (this._maxHp == value)
				{
					return;
				}
				int maxHp = this._maxHp;
				this._maxHp = value;
				Action<int, int> maxHpChanged = this.MaxHpChanged;
				if (maxHpChanged == null)
				{
					return;
				}
				maxHpChanged(maxHp, this._maxHp);
			}
		}

		// Token: 0x1700240B RID: 9227
		// (get) Token: 0x06008412 RID: 33810 RVA: 0x002C8D64 File Offset: 0x002C6F64
		// (set) Token: 0x06008413 RID: 33811 RVA: 0x002C8D6C File Offset: 0x002C6F6C
		public int? MonsterMaxHpBeforeModification { get; private set; }

		// Token: 0x1700240C RID: 9228
		// (get) Token: 0x06008414 RID: 33812 RVA: 0x002C8D75 File Offset: 0x002C6F75
		// (set) Token: 0x06008415 RID: 33813 RVA: 0x002C8D7D File Offset: 0x002C6F7D
		public uint? CombatId { get; set; }

		// Token: 0x1700240D RID: 9229
		// (get) Token: 0x06008416 RID: 33814 RVA: 0x002C8D86 File Offset: 0x002C6F86
		[Nullable(2)]
		public MonsterModel Monster
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x1700240E RID: 9230
		// (get) Token: 0x06008417 RID: 33815 RVA: 0x002C8D8E File Offset: 0x002C6F8E
		[Nullable(2)]
		public Player Player
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x1700240F RID: 9231
		// (get) Token: 0x06008418 RID: 33816 RVA: 0x002C8D96 File Offset: 0x002C6F96
		public ModelId ModelId
		{
			get
			{
				if (!this.IsPlayer)
				{
					return this.Monster.Id;
				}
				return this.Player.Character.Id;
			}
		}

		// Token: 0x17002410 RID: 9232
		// (get) Token: 0x06008419 RID: 33817 RVA: 0x002C8DBC File Offset: 0x002C6FBC
		public CombatSide Side { get; }

		/// <summary>
		/// The CombatState that this creature exists in.
		/// Never null for monsters.
		/// Will be null for players when outside of combat.
		/// </summary>
		// Token: 0x17002411 RID: 9233
		// (get) Token: 0x0600841A RID: 33818 RVA: 0x002C8DC4 File Offset: 0x002C6FC4
		// (set) Token: 0x0600841B RID: 33819 RVA: 0x002C8DCC File Offset: 0x002C6FCC
		[Nullable(2)]
		public ICombatState CombatState
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		/// <summary>
		/// Use this when displaying the name to the player.
		/// </summary>
		// Token: 0x17002412 RID: 9234
		// (get) Token: 0x0600841C RID: 33820 RVA: 0x002C8DD8 File Offset: 0x002C6FD8
		public string Name
		{
			get
			{
				if (this.IsMonster)
				{
					return this.Monster.Title.GetFormattedText();
				}
				if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
				{
					return this.Player.Character.Title.GetFormattedText();
				}
				return PlatformUtil.GetPlayerNameRaw(RunManager.Instance.NetService.Platform, this.Player.NetId);
			}
		}

		/// <summary>
		/// Use this when logging the name of the creature.
		/// It avoids logging player names directly.
		/// </summary>
		// Token: 0x17002413 RID: 9235
		// (get) Token: 0x0600841D RID: 33821 RVA: 0x002C8E40 File Offset: 0x002C7040
		public string LogName
		{
			get
			{
				if (this.IsMonster)
				{
					return this.Monster.Title.GetFormattedText();
				}
				if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
				{
					return this.Player.Character.Title.GetFormattedText();
				}
				return "PlayerId " + this.Player.NetId.ToString();
			}
		}

		// Token: 0x17002414 RID: 9236
		// (get) Token: 0x0600841E RID: 33822 RVA: 0x002C8EA5 File Offset: 0x002C70A5
		public bool IsMonster
		{
			get
			{
				return this.Monster != null;
			}
		}

		// Token: 0x17002415 RID: 9237
		// (get) Token: 0x0600841F RID: 33823 RVA: 0x002C8EB0 File Offset: 0x002C70B0
		public bool IsPlayer
		{
			get
			{
				return this.Player != null;
			}
		}

		/// <summary>
		/// How should this creature's health bar be displayed?
		/// This is mostly for visuals, although a few gameplay effects (like <see cref="T:MegaCrit.Sts2.Core.Models.Powers.HellraiserPower" />) check it to see
		/// if the creature can currently take damage.
		/// </summary>
		// Token: 0x17002416 RID: 9238
		// (get) Token: 0x06008420 RID: 33824 RVA: 0x002C8EBB File Offset: 0x002C70BB
		// (set) Token: 0x06008421 RID: 33825 RVA: 0x002C8EC3 File Offset: 0x002C70C3
		public HpDisplay HpDisplay { get; set; }

		/// <summary>
		/// The player that owns this pet.
		/// Returns null if this creature is not a pet.
		/// </summary>
		// Token: 0x17002417 RID: 9239
		// (get) Token: 0x06008422 RID: 33826 RVA: 0x002C8ECC File Offset: 0x002C70CC
		// (set) Token: 0x06008423 RID: 33827 RVA: 0x002C8ED4 File Offset: 0x002C70D4
		[Nullable(2)]
		public Player PetOwner
		{
			[NullableContext(2)]
			get
			{
				return this._petOwner;
			}
			[NullableContext(2)]
			set
			{
				if (this._petOwner != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Pet ");
					defaultInterpolatedStringHandler.AppendFormatted<Creature>(this);
					defaultInterpolatedStringHandler.AppendLiteral(" already has an owner.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this._petOwner = value;
			}
		}

		/// <summary>
		/// Is this creature a pet?
		/// </summary>
		// Token: 0x17002418 RID: 9240
		// (get) Token: 0x06008424 RID: 33828 RVA: 0x002C8F27 File Offset: 0x002C7127
		public bool IsPet
		{
			get
			{
				return this.PetOwner != null;
			}
		}

		/// <summary>
		/// Get all the pets that belong to this creature.
		/// </summary>
		// Token: 0x17002419 RID: 9241
		// (get) Token: 0x06008425 RID: 33829 RVA: 0x002C8F32 File Offset: 0x002C7132
		public IReadOnlyList<Creature> Pets
		{
			get
			{
				Player player = this.Player;
				IReadOnlyList<Creature> readOnlyList;
				if (player == null)
				{
					readOnlyList = null;
				}
				else
				{
					PlayerCombatState playerCombatState = player.PlayerCombatState;
					readOnlyList = ((playerCombatState != null) ? playerCombatState.Pets : null);
				}
				return readOnlyList ?? Array.Empty<Creature>();
			}
		}

		// Token: 0x1700241A RID: 9242
		// (get) Token: 0x06008426 RID: 33830 RVA: 0x002C8F5B File Offset: 0x002C715B
		public bool IsAlive
		{
			get
			{
				return this.CurrentHp > 0;
			}
		}

		// Token: 0x1700241B RID: 9243
		// (get) Token: 0x06008427 RID: 33831 RVA: 0x002C8F66 File Offset: 0x002C7166
		public bool IsDead
		{
			get
			{
				return !this.IsAlive;
			}
		}

		// Token: 0x1700241C RID: 9244
		// (get) Token: 0x06008428 RID: 33832 RVA: 0x002C8F71 File Offset: 0x002C7171
		// (set) Token: 0x06008429 RID: 33833 RVA: 0x002C8F79 File Offset: 0x002C7179
		[Nullable(2)]
		public string SlotName
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x1700241D RID: 9245
		// (get) Token: 0x0600842A RID: 33834 RVA: 0x002C8F84 File Offset: 0x002C7184
		public IEnumerable<IHoverTip> HoverTips
		{
			get
			{
				if (!CombatManager.Instance.IsInProgress)
				{
					return Array.Empty<IHoverTip>();
				}
				List<IHoverTip> list = new List<IHoverTip>();
				if (this.IsMonster)
				{
					foreach (AbstractIntent abstractIntent in this.Monster.NextMove.Intents)
					{
						if (abstractIntent.HasIntentTip)
						{
							list.Add(abstractIntent.GetHoverTip(this.CombatState.Allies, this));
						}
					}
				}
				foreach (PowerModel powerModel in this._powers)
				{
					IEnumerable<IHoverTip> hoverTips = powerModel.HoverTips;
					foreach (IHoverTip hoverTip in hoverTips)
					{
						list.MegaTryAddingTip(hoverTip);
					}
				}
				return list;
			}
		}

		// Token: 0x1700241E RID: 9246
		// (get) Token: 0x0600842B RID: 33835 RVA: 0x002C90A0 File Offset: 0x002C72A0
		public bool IsEnemy
		{
			get
			{
				return this.Side == CombatSide.Enemy;
			}
		}

		/// <summary>
		/// Is this creature a <b>primary enemy</b>?
		/// A <b>primary enemy</b> can stay alive all alone.
		/// A <b>secondary enemy</b> will automatically die unless there's also a living primary enemy.
		/// Most enemies are primary enemies. Enemies with powers like <see cref="T:MegaCrit.Sts2.Core.Models.Powers.MinionPower" /> or <see cref="T:MegaCrit.Sts2.Core.Models.Powers.IllusionPower" /> are
		/// secondary.
		/// </summary>
		// Token: 0x1700241F RID: 9247
		// (get) Token: 0x0600842C RID: 33836 RVA: 0x002C90AB File Offset: 0x002C72AB
		public bool IsPrimaryEnemy
		{
			get
			{
				return this.Side == CombatSide.Enemy && !this.IsSecondaryEnemy;
			}
		}

		/// <summary>
		/// Is this creature a <b>secondary enemy</b>?
		/// See <see cref="P:MegaCrit.Sts2.Core.Entities.Creatures.Creature.IsPrimaryEnemy" /> for detailed definitions.
		/// </summary>
		// Token: 0x17002420 RID: 9248
		// (get) Token: 0x0600842D RID: 33837 RVA: 0x002C90C1 File Offset: 0x002C72C1
		public bool IsSecondaryEnemy
		{
			get
			{
				if (this.Side != CombatSide.Enemy)
				{
					return false;
				}
				return this.Powers.Any<PowerModel>((PowerModel p) => p.OwnerIsSecondaryEnemy);
			}
		}

		/// <summary>
		/// Is this creature <b>hittable</b>?
		/// A <b>hittable</b> creature is alive and can be "hit" by effects.
		/// This is different from <b>targetable</b>; an un-targetable creature can still be hit by AOE effects.
		/// </summary>
		// Token: 0x17002421 RID: 9249
		// (get) Token: 0x0600842E RID: 33838 RVA: 0x002C90F8 File Offset: 0x002C72F8
		public bool IsHittable
		{
			get
			{
				return !this.IsDead && Hook.ShouldAllowHitting(this.CombatState, this);
			}
		}

		/// <summary>
		/// Can this creature have powers applied to it?
		/// A creature can have powers applied to it if it's in combat and can be "hit" by effects.
		/// This is different from <b>targetable</b>; an un-targetable creature can have powers applied to it.
		/// It's also different from <b>hittable</b>; a creature is not hittable if it's dead, but dead creatures can still
		/// have powers applied to them.
		/// </summary>
		// Token: 0x17002422 RID: 9250
		// (get) Token: 0x0600842F RID: 33839 RVA: 0x002C9115 File Offset: 0x002C7315
		public bool CanReceivePowers
		{
			get
			{
				return this.CombatState != null && Hook.ShouldAllowHitting(this.CombatState, this);
			}
		}

		// Token: 0x17002423 RID: 9251
		// (get) Token: 0x06008430 RID: 33840 RVA: 0x002C9132 File Offset: 0x002C7332
		public bool IsStunned
		{
			get
			{
				MonsterModel monster = this.Monster;
				return ((monster != null) ? monster.NextMove.Id : null) == "STUNNED";
			}
		}

		// Token: 0x06008431 RID: 33841 RVA: 0x002C9158 File Offset: 0x002C7358
		public Creature(MonsterModel monster, CombatSide side, [Nullable(2)] string slotName)
		{
			monster.AssertMutable();
			int minInitialHp = monster.MinInitialHp;
			int maxInitialHp = monster.MaxInitialHp;
			if (minInitialHp > maxInitialHp)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 3);
				defaultInterpolatedStringHandler.AppendFormatted(monster.Id.Entry);
				defaultInterpolatedStringHandler.AppendLiteral(" has min HP ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(minInitialHp);
				defaultInterpolatedStringHandler.AppendLiteral(" greater than its max ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(maxInitialHp);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Monster = monster;
			this.Monster.Creature = this;
			this.SlotName = slotName;
			this._maxHp = maxInitialHp;
			this._currentHp = maxInitialHp;
			this.Side = side;
		}

		// Token: 0x06008432 RID: 33842 RVA: 0x002C921A File Offset: 0x002C741A
		public Creature(Player player, int currentHp, int maxHp)
		{
			this.Player = player;
			this._currentHp = currentHp;
			this._maxHp = maxHp;
			this.Side = CombatSide.Player;
		}

		// Token: 0x06008433 RID: 33843 RVA: 0x002C924C File Offset: 0x002C744C
		public void SetUniqueMonsterHpValue(IReadOnlyList<Creature> creaturesOnSide, Rng rng)
		{
			if (this.Monster == null)
			{
				throw new InvalidOperationException("Can't set unique monster HP value for a player.");
			}
			int minInitialHp = this.Monster.MinInitialHp;
			int num = this.Monster.MaxInitialHp + 1;
			HashSet<int> hashSet = Enumerable.Range(minInitialHp, num - minInitialHp).ToHashSet<int>();
			hashSet.ExceptWith(from e in creaturesOnSide.Except<Creature>(new <>z__ReadOnlySingleElementList<Creature>(this))
				select e.MaxHp);
			int num2;
			if (hashSet.Count > 0)
			{
				num2 = rng.NextItem<int>(hashSet);
			}
			else
			{
				num2 = rng.NextInt(minInitialHp, num);
			}
			this._maxHp = num2;
			this._currentHp = num2;
			this.MonsterMaxHpBeforeModification = new int?(num2);
		}

		// Token: 0x06008434 RID: 33844 RVA: 0x002C9301 File Offset: 0x002C7501
		[NullableContext(2)]
		public void ScaleMonsterHpForMultiplayer(EncounterModel encounter, int playerCount, int actIndex)
		{
			if (playerCount == 1)
			{
				return;
			}
			this.SetMaxHpInternal(Creature.ScaleHpForMultiplayer(this.MaxHp, encounter, playerCount, actIndex));
			this.SetCurrentHpInternal(this.MaxHp);
		}

		// Token: 0x06008435 RID: 33845 RVA: 0x002C9334 File Offset: 0x002C7534
		[NullableContext(2)]
		public NCreatureVisuals CreateVisuals()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (this.Player != null)
			{
				return this.Player.Character.CreateVisuals();
			}
			if (this.Monster != null)
			{
				return this.Monster.CreateVisuals();
			}
			throw new InvalidOperationException("Creature and Monster should never both be null.");
		}

		/// <summary>
		/// Called when the creature is first added to combat.
		/// </summary>
		// Token: 0x06008436 RID: 33846 RVA: 0x002C9384 File Offset: 0x002C7584
		public async Task AfterAddedToRoom()
		{
			if (this.Side == CombatSide.Enemy)
			{
				await this.Monster.AfterAddedToRoom();
			}
		}

		/// <summary>
		/// DO NOT CALL UNLESS YOU KNOW EXACTLY WHAT YOU'RE DOING.
		/// Hooks and everything are all done in CreatureCmd.Damage, so there needs to be a really good reason to want to
		/// avoid them.
		/// </summary>
		/// <param name="amount">Amount of damage to be dealt to this creature's block.</param>
		/// <param name="props">Damage props</param>
		/// <returns>How much damage was blocked.</returns>
		// Token: 0x06008437 RID: 33847 RVA: 0x002C93C8 File Offset: 0x002C75C8
		public decimal DamageBlockInternal(decimal amount, ValueProp props)
		{
			decimal num = (props.HasFlag(ValueProp.Unblockable) ? 0m : Math.Min(this.Block, amount));
			this.Block -= (int)num;
			return num;
		}

		/// <summary>
		/// DO NOT CALL UNLESS YOU KNOW EXACTLY WHAT YOU'RE DOING.
		/// Hooks and everything are all done in CreatureCmd.Damage, so there needs to be a really good reason to want to
		/// avoid them.
		/// </summary>
		/// <param name="amount">Amount of damage to be dealt to this creature's HP.</param>
		/// <param name="props">Value props for the damage.</param>
		/// <returns>Result of the damage.</returns>
		// Token: 0x06008438 RID: 33848 RVA: 0x002C9418 File Offset: 0x002C7618
		public DamageResult LoseHpInternal(decimal amount, ValueProp props)
		{
			bool flag = this.CurrentHp > 0 && amount >= this.CurrentHp;
			int currentHp = this.CurrentHp;
			int num = (int)Math.Clamp(amount, 0m, 999999999m);
			this.CurrentHp = Math.Max(this.CurrentHp - num, 0);
			return new DamageResult(this, props)
			{
				UnblockedDamage = currentHp - this.CurrentHp,
				WasTargetKilled = flag,
				OverkillDamage = (flag ? Math.Max(num - currentHp, 0) : 0)
			};
		}

		// Token: 0x06008439 RID: 33849 RVA: 0x002C94AC File Offset: 0x002C76AC
		public void GainBlockInternal(decimal amount)
		{
			if (amount < 0m)
			{
				throw new ArgumentException("amount must be positive. Use LoseBlock for block loss.");
			}
			this.Block = (int)Math.Min(this.Block + amount, 999999999m);
		}

		// Token: 0x0600843A RID: 33850 RVA: 0x002C94FC File Offset: 0x002C76FC
		public void LoseBlockInternal(decimal amount)
		{
			if (amount < 0m)
			{
				throw new ArgumentException("amount must be positive. Use GainBlock for block gain.");
			}
			this.Block = (int)Math.Max(this.Block - amount, 0m);
		}

		// Token: 0x0600843B RID: 33851 RVA: 0x002C953C File Offset: 0x002C773C
		public void HealInternal(decimal amount)
		{
			bool isDead = this.IsDead;
			this.SetCurrentHpInternal(this.CurrentHp + amount);
			if (isDead && !this.IsDead)
			{
				Player player = this.Player;
				if (player != null)
				{
					player.ActivateHooks();
				}
				Action<Creature> revived = this.Revived;
				if (revived == null)
				{
					return;
				}
				revived(this);
			}
		}

		// Token: 0x0600843C RID: 33852 RVA: 0x002C9594 File Offset: 0x002C7794
		public void SetCurrentHpInternal(decimal amount)
		{
			this.CurrentHp = (int)Math.Min(amount, this.MaxHp);
		}

		// Token: 0x0600843D RID: 33853 RVA: 0x002C95B4 File Offset: 0x002C77B4
		public void SetMaxHpInternal(decimal amount)
		{
			if (amount < 0m)
			{
				throw new ArgumentException("amount must be non-negative.");
			}
			this.MaxHp = Math.Min((int)amount, 999999999);
			this.CurrentHp = Math.Min(this.CurrentHp, this.MaxHp);
		}

		// Token: 0x0600843E RID: 33854 RVA: 0x002C9606 File Offset: 0x002C7806
		public void Reset()
		{
			this.RemoveAllPowersInternalExcept(null);
			this.Block = 0;
		}

		// Token: 0x0600843F RID: 33855 RVA: 0x002C9617 File Offset: 0x002C7817
		public void InvokeDiedEvent()
		{
			Action<Creature> died = this.Died;
			if (died == null)
			{
				return;
			}
			died(this);
		}

		/// <summary>
		/// Stun this creature.
		/// You should probably be calling
		/// <see cref="M:MegaCrit.Sts2.Core.Commands.CreatureCmd.Stun(MegaCrit.Sts2.Core.Entities.Creatures.Creature,System.Func{System.Collections.Generic.IReadOnlyList{MegaCrit.Sts2.Core.Entities.Creatures.Creature},System.Threading.Tasks.Task},System.String)" /> instead.
		/// </summary>
		/// <param name="stunMove">Logic for the move that the stunned creature will "perform".</param>
		/// <param name="nextMoveId">
		/// ID of the move that the creature should perform the turn after they perform stunMove.
		/// If null or empty, this will default to the last move they performed.
		/// </param>
		// Token: 0x06008440 RID: 33856 RVA: 0x002C962C File Offset: 0x002C782C
		public void StunInternal(Func<IReadOnlyList<Creature>, Task> stunMove, [Nullable(2)] string nextMoveId)
		{
			if (this.Monster == null)
			{
				throw new InvalidOperationException("Can't stun a player.");
			}
			if (this.CombatState == null)
			{
				return;
			}
			if (this.IsDead)
			{
				return;
			}
			if (string.IsNullOrEmpty(nextMoveId))
			{
				List<MonsterState> stateLog = this.Monster.MoveStateMachine.StateLog;
				nextMoveId = stateLog.Last<MonsterState>().Id;
			}
			MoveState moveState = new MoveState("STUNNED", stunMove, new AbstractIntent[]
			{
				new StunIntent()
			})
			{
				FollowUpStateId = nextMoveId,
				MustPerformOnceBeforeTransitioning = true
			};
			this.Monster.SetMoveImmediate(moveState, false);
		}

		// Token: 0x06008441 RID: 33857 RVA: 0x002C96BC File Offset: 0x002C78BC
		public void PrepareForNextTurn(IEnumerable<Creature> targets, bool rollNewMove = true)
		{
			Creature[] array = targets.ToArray<Creature>();
			if (rollNewMove && this.Monster.MoveStateMachine != null)
			{
				this.Monster.RollMove(array);
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(this) : null);
			if (ncreature != null)
			{
				TaskHelper.RunSafely(ncreature.RefreshIntents());
			}
		}

		// Token: 0x17002424 RID: 9252
		// (get) Token: 0x06008442 RID: 33858 RVA: 0x002C970E File Offset: 0x002C790E
		public IReadOnlyList<PowerModel> Powers
		{
			get
			{
				return this._powers;
			}
		}

		// Token: 0x06008443 RID: 33859 RVA: 0x002C9716 File Offset: 0x002C7916
		[NullableContext(0)]
		public bool HasPower<T>() where T : PowerModel
		{
			return this._powers.Any<PowerModel>((PowerModel p) => p is T);
		}

		// Token: 0x06008444 RID: 33860 RVA: 0x002C9744 File Offset: 0x002C7944
		public bool HasPower(ModelId id)
		{
			return this._powers.Any<PowerModel>((PowerModel p) => p.Id == id);
		}

		// Token: 0x06008445 RID: 33861 RVA: 0x002C9775 File Offset: 0x002C7975
		[NullableContext(0)]
		[return: Nullable(2)]
		public T GetPower<T>() where T : PowerModel
		{
			return this._powers.FirstOrDefault<PowerModel>((PowerModel p) => p is T) as T;
		}

		// Token: 0x06008446 RID: 33862 RVA: 0x002C97AC File Offset: 0x002C79AC
		[return: Nullable(2)]
		public PowerModel GetPower(ModelId id)
		{
			return this._powers.FirstOrDefault<PowerModel>((PowerModel p) => p.Id == id);
		}

		// Token: 0x06008447 RID: 33863 RVA: 0x002C97DD File Offset: 0x002C79DD
		public IEnumerable<T> GetPowerInstances<[Nullable(0)] T>() where T : PowerModel
		{
			return this._powers.OfType<T>();
		}

		// Token: 0x06008448 RID: 33864 RVA: 0x002C97EC File Offset: 0x002C79EC
		public IEnumerable<PowerModel> GetPowerInstances(ModelId id)
		{
			return this._powers.Where<PowerModel>((PowerModel p) => p.Id == id);
		}

		// Token: 0x06008449 RID: 33865 RVA: 0x002C9820 File Offset: 0x002C7A20
		[return: Nullable(2)]
		public PowerModel GetPowerById(ModelId id)
		{
			return this._powers.FirstOrDefault<PowerModel>((PowerModel p) => p.Id == id);
		}

		// Token: 0x0600844A RID: 33866 RVA: 0x002C9851 File Offset: 0x002C7A51
		[NullableContext(0)]
		public int GetPowerAmount<T>() where T : PowerModel
		{
			T t = this.GetPower<T>();
			if (t == null)
			{
				return 0;
			}
			return t.Amount;
		}

		/// <summary>
		/// NEVER CALL THIS!
		/// ONLY PowerModel.ApplyInternal should be calling this.
		/// </summary>
		/// <param name="power">Power to apply.</param>
		// Token: 0x0600844B RID: 33867 RVA: 0x002C986C File Offset: 0x002C7A6C
		public void ApplyPowerInternal(PowerModel power)
		{
			if (power.Owner != this)
			{
				throw new InvalidOperationException("ONLY CALL THIS FROM PowerModel.ApplyInternal!");
			}
			if (power.InstanceType == PowerInstanceType.None && this._powers.Any<PowerModel>((PowerModel p) => p.GetType() == power.GetType()))
			{
				throw new InvalidOperationException("Trying to add multiple instances of a non-instanced power to a creature.");
			}
			this._powers.Add(power);
			Action<PowerModel> powerApplied = this.PowerApplied;
			if (powerApplied == null)
			{
				return;
			}
			powerApplied(power);
		}

		/// <summary>
		/// NEVER CALL THIS!
		/// ONLY PowerModel.Amount should be calling this.
		/// </summary>
		/// <param name="power">Power to modify.</param>
		/// <param name="change">How much the power has changed.</param>
		/// <param name="silent">Whether or not VFX should be displayed for this power.</param>
		// Token: 0x0600844C RID: 33868 RVA: 0x002C98F8 File Offset: 0x002C7AF8
		public void InvokePowerModified(PowerModel power, int change, bool silent)
		{
			if (change > 0)
			{
				Action<PowerModel, int, bool> powerIncreased = this.PowerIncreased;
				if (powerIncreased == null)
				{
					return;
				}
				powerIncreased(power, change, silent);
				return;
			}
			else if (power.StackType.Equals(PowerStackType.Counter) && power.AllowNegative && change < 0)
			{
				Action<PowerModel, int, bool> powerIncreased2 = this.PowerIncreased;
				if (powerIncreased2 == null)
				{
					return;
				}
				powerIncreased2(power, change, silent);
				return;
			}
			else
			{
				Action<PowerModel, bool> powerDecreased = this.PowerDecreased;
				if (powerDecreased == null)
				{
					return;
				}
				powerDecreased(power, silent);
				return;
			}
		}

		/// <summary>
		/// NEVER CALL THIS!
		/// ONLY PowerModel.RemoveInternal should be calling this.
		/// </summary>
		/// <param name="power">Power to remove.</param>
		// Token: 0x0600844D RID: 33869 RVA: 0x002C996B File Offset: 0x002C7B6B
		public void RemovePowerInternal(PowerModel power)
		{
			if (power.Owner != this)
			{
				throw new InvalidOperationException("ONLY CALL THIS FROM PowerModel.RemoveInternal!");
			}
			this._powers.Remove(power);
			Action<PowerModel> powerRemoved = this.PowerRemoved;
			if (powerRemoved == null)
			{
				return;
			}
			powerRemoved(power);
		}

		/// <summary>
		/// NEVER CALL THIS UNLESS YOU KNOW WHAT YOU'RE DOING!
		/// This skips the AfterRemoved call for powers.
		/// </summary>
		/// <param name="except">Powers that should not be removed.</param>
		// Token: 0x0600844E RID: 33870 RVA: 0x002C99A0 File Offset: 0x002C7BA0
		public IEnumerable<PowerModel> RemoveAllPowersInternalExcept([Nullable(new byte[] { 2, 1 })] IEnumerable<PowerModel> except = null)
		{
			List<PowerModel> list = this._powers.Except<PowerModel>(except ?? Array.Empty<PowerModel>()).ToList<PowerModel>();
			foreach (PowerModel powerModel in list)
			{
				powerModel.RemoveInternal();
			}
			return list;
		}

		// Token: 0x0600844F RID: 33871 RVA: 0x002C9A0C File Offset: 0x002C7C0C
		public IEnumerable<PowerModel> RemoveAllPowersAfterDeath()
		{
			return this.RemoveAllPowersInternalExcept(this._powers.Where<PowerModel>((PowerModel p) => !p.ShouldPowerBeRemovedAfterOwnerDeath() || !Hook.ShouldPowerBeRemovedOnDeath(p)));
		}

		// Token: 0x06008450 RID: 33872 RVA: 0x002C9A40 File Offset: 0x002C7C40
		public void BeforeTurnStart(CombatSide side)
		{
			foreach (PowerModel powerModel in this._powers)
			{
				powerModel.AmountOnTurnStart = powerModel.Amount;
			}
		}

		// Token: 0x06008451 RID: 33873 RVA: 0x002C9A98 File Offset: 0x002C7C98
		public async Task AfterTurnStart(CombatSide side)
		{
			if (side == CombatSide.Player)
			{
				Player player = this.Player;
				bool flag;
				if (player == null)
				{
					flag = false;
				}
				else
				{
					PlayerCombatState playerCombatState = player.PlayerCombatState;
					flag = ((playerCombatState != null) ? new int?(playerCombatState.TurnNumber) : null).GetValueOrDefault() == 1;
				}
				if (flag)
				{
					return;
				}
			}
			await this.ClearBlock();
		}

		// Token: 0x06008452 RID: 33874 RVA: 0x002C9AE3 File Offset: 0x002C7CE3
		public void OnSideSwitch()
		{
			if (this.IsPlayer)
			{
				this.Player.OnSideSwitch();
				return;
			}
			this.Monster.OnSideSwitch();
		}

		// Token: 0x06008453 RID: 33875 RVA: 0x002C9B04 File Offset: 0x002C7D04
		public async Task TakeTurn()
		{
			if (!this.IsMonster || this.Side != CombatSide.Enemy)
			{
				throw new InvalidOperationException("Only enemy monsters can take automated turns.");
			}
			if (!this.Monster.SpawnedThisTurn)
			{
				await this.Monster.PerformMove();
			}
		}

		// Token: 0x06008454 RID: 33876 RVA: 0x002C9B48 File Offset: 0x002C7D48
		private async Task ClearBlock()
		{
			AbstractModel abstractModel;
			if (Hook.ShouldClearBlock(this.CombatState, this, out abstractModel))
			{
				this.Block = 0;
			}
			else
			{
				await Hook.AfterPreventingBlockClear(this.CombatState, abstractModel, this);
			}
		}

		// Token: 0x06008455 RID: 33877 RVA: 0x002C9B8B File Offset: 0x002C7D8B
		public override string ToString()
		{
			return "Creature " + this.LogName;
		}

		/// <summary>
		/// Helper function to get the percent hp of a creature (0 - 1).
		/// </summary>
		/// <returns></returns>
		// Token: 0x06008456 RID: 33878 RVA: 0x002C9B9D File Offset: 0x002C7D9D
		public double GetHpPercentRemaining()
		{
			return (double)this._currentHp / (double)this._maxHp;
		}

		// Token: 0x06008457 RID: 33879 RVA: 0x002C9BAE File Offset: 0x002C7DAE
		[NullableContext(2)]
		public static decimal ScaleHpForMultiplayer(decimal hp, EncounterModel encounter, int playerCount, int actIndex)
		{
			if (playerCount <= 1)
			{
				return hp;
			}
			return hp * playerCount * MultiplayerScalingModel.GetMultiplayerScaling(encounter, actIndex);
		}

		// Token: 0x06008458 RID: 33880 RVA: 0x002C9BCE File Offset: 0x002C7DCE
		[NullableContext(2)]
		public NCreature GetCreatureNode()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature;
			if ((ncreature = ((instance != null) ? instance.GetCreatureNode(this) : null)) == null)
			{
				NBestiary instance2 = NBestiary.Instance;
				if (instance2 == null)
				{
					return null;
				}
				ncreature = instance2.GetCreatureNode(this);
			}
			return ncreature;
		}

		/// <summary>
		/// Sets the visibility of this creature's node. No-op if the node doesn't exist (e.g. test mode, or combat ended
		/// and the node was freed).
		/// </summary>
		// Token: 0x06008459 RID: 33881 RVA: 0x002C9C00 File Offset: 0x002C7E00
		public void SetNodeVisible(bool visible)
		{
			NCreature creatureNode = this.GetCreatureNode();
			if (creatureNode == null)
			{
				return;
			}
			creatureNode.Visible = visible;
		}

		// Token: 0x0600845A RID: 33882 RVA: 0x002C9C20 File Offset: 0x002C7E20
		[NullableContext(2)]
		public Control GetVfxContainer()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			if (((instance != null) ? instance.GetCreatureNode(this) : null) != null)
			{
				return NCombatRoom.Instance.CombatVfxContainer;
			}
			NBestiary instance2 = NBestiary.Instance;
			if (((instance2 != null) ? instance2.GetCreatureNode(this) : null) != null)
			{
				return NBestiary.Instance.VfxContainer;
			}
			return null;
		}

		// Token: 0x0600845B RID: 33883 RVA: 0x002C9C78 File Offset: 0x002C7E78
		[NullableContext(2)]
		public Control GetBackVfxContainer()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			if (((instance != null) ? instance.GetCreatureNode(this) : null) != null)
			{
				return NCombatRoom.Instance.BackCombatVfxContainer;
			}
			NBestiary instance2 = NBestiary.Instance;
			if (((instance2 != null) ? instance2.GetCreatureNode(this) : null) != null)
			{
				return NBestiary.Instance.BackVfxContainer;
			}
			return null;
		}

		// Token: 0x04002D4D RID: 11597
		private int _block;

		// Token: 0x04002D4E RID: 11598
		private int _currentHp;

		// Token: 0x04002D4F RID: 11599
		private int _maxHp;

		// Token: 0x04002D50 RID: 11600
		private readonly List<PowerModel> _powers = new List<PowerModel>();

		// Token: 0x04002D58 RID: 11608
		[Nullable(2)]
		private Player _petOwner;
	}
}
