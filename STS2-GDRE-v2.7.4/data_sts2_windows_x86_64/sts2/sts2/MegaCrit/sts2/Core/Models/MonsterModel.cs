using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004E7 RID: 1255
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class MonsterModel : AbstractModel
	{
		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06004D4E RID: 19790 RVA: 0x0022E1E0 File Offset: 0x0022C3E0
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06004D4F RID: 19791 RVA: 0x0022E1E3 File Offset: 0x0022C3E3
		public virtual LocString Title
		{
			get
			{
				return MonsterModel.L10NMonsterLookup(base.Id.Entry + ".name");
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06004D50 RID: 19792
		public abstract int MinInitialHp { get; }

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06004D51 RID: 19793
		public abstract int MaxInitialHp { get; }

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06004D52 RID: 19794 RVA: 0x0022E1FF File Offset: 0x0022C3FF
		public virtual bool IsHealthBarVisible
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// At time of death, a SubViewport is created with the bounds of the creature's spine body in its current pose.
		/// If the creature needs additional padding (usually because the death animation changes its bounds), this property
		/// can be adjusted so that the spawned SubViewport is larger.
		/// </summary>
		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06004D53 RID: 19795 RVA: 0x0022E202 File Offset: 0x0022C402
		public virtual Vector2 ExtraDeathVfxPadding
		{
			get
			{
				return MonsterModel.defaultDeathVfxPadding;
			}
		}

		/// <summary>
		/// For specific creatures that are very close, this value can be made some positive value so that their health bars
		/// do not overlap each other.
		/// </summary>
		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06004D54 RID: 19796 RVA: 0x0022E209 File Offset: 0x0022C409
		public virtual float HpBarSizeReduction
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06004D55 RID: 19797 RVA: 0x0022E210 File Offset: 0x0022C410
		protected virtual string VisualsPath
		{
			get
			{
				return SceneHelper.GetScenePath("creature_visuals/" + base.Id.Entry.ToLowerInvariant());
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06004D56 RID: 19798 RVA: 0x0022E234 File Offset: 0x0022C434
		public unsafe virtual IEnumerable<string> AssetPaths
		{
			get
			{
				int num = 1;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount<string>(list, num);
				Span<string> span = CollectionsMarshal.AsSpan<string>(list);
				int num2 = 0;
				*span[num2] = this.VisualsPath;
				List<string> list2 = list;
				foreach (AbstractIntent abstractIntent in this.GetIntents())
				{
					list2.AddRange(abstractIntent.AssetPaths);
				}
				return list2;
			}
		}

		/// <summary>
		/// This RNG should be used for things that are nice to have synced between players. It should be deterministic as
		/// long as you only use it in deterministic contexts.
		/// It should ONLY be set in <see cref="M:MegaCrit.Sts2.Core.Combat.CombatState.CreateCreature(MegaCrit.Sts2.Core.Models.MonsterModel,MegaCrit.Sts2.Core.Combat.CombatSide,System.String)" />.
		/// </summary>
		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06004D57 RID: 19799 RVA: 0x0022E2B8 File Offset: 0x0022C4B8
		// (set) Token: 0x06004D58 RID: 19800 RVA: 0x0022E2CE File Offset: 0x0022C4CE
		public Rng Rng
		{
			get
			{
				if (!base.IsMutable)
				{
					return Rng.Chaotic;
				}
				return this._rng;
			}
			set
			{
				base.AssertMutable();
				this._rng = value;
			}
		}

		/// <summary>
		/// Run-scoped RNG set for the run that this monster is in.
		/// </summary>
		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06004D59 RID: 19801 RVA: 0x0022E2DD File Offset: 0x0022C4DD
		// (set) Token: 0x06004D5A RID: 19802 RVA: 0x0022E2E5 File Offset: 0x0022C4E5
		public RunRngSet RunRng
		{
			get
			{
				return this._runRng;
			}
			set
			{
				base.AssertMutable();
				if (this._runRng != null)
				{
					throw new InvalidOperationException("RunRng has already been set!");
				}
				this._runRng = value;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06004D5B RID: 19803 RVA: 0x0022E307 File Offset: 0x0022C507
		// (set) Token: 0x06004D5C RID: 19804 RVA: 0x0022E30F File Offset: 0x0022C50F
		public bool IsPerformingMove
		{
			get
			{
				return this._isPerformingMove;
			}
			private set
			{
				base.AssertMutable();
				this._isPerformingMove = value;
			}
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x0022E320 File Offset: 0x0022C520
		public NCreatureVisuals CreateVisuals()
		{
			NCreatureVisuals ncreatureVisuals;
			try
			{
				ncreatureVisuals = PreloadManager.Cache.GetScene(this.VisualsPath).Instantiate<NCreatureVisuals>(0L);
			}
			catch (Exception ex)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(97, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Encountered exception loading the creature visuals for ");
				Creature creature = this._creature;
				defaultInterpolatedStringHandler.AppendFormatted((creature != null) ? creature.Name : null);
				defaultInterpolatedStringHandler.AppendLiteral(". Falling back to error scene. Exception: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(ex);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				SentryService.CaptureException(ex);
				ncreatureVisuals = this.CreateFallbackVisuals();
			}
			return ncreatureVisuals;
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x0022E3B8 File Offset: 0x0022C5B8
		private NCreatureVisuals CreateFallbackVisuals()
		{
			return PreloadManager.Cache.GetScene(MonsterModel._fallbackVisualsPath).Instantiate<NCreatureVisuals>(0L);
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06004D5F RID: 19807 RVA: 0x0022E3D0 File Offset: 0x0022C5D0
		protected virtual string AttackSfx
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
				defaultInterpolatedStringHandler.AppendLiteral("event:/sfx/enemy/enemy_attacks/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("_attack");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06004D60 RID: 19808 RVA: 0x0022E440 File Offset: 0x0022C640
		protected virtual string CastSfx
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("event:/sfx/enemy/enemy_attacks/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("_cast");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06004D61 RID: 19809 RVA: 0x0022E4B0 File Offset: 0x0022C6B0
		public virtual string DeathSfx
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
				defaultInterpolatedStringHandler.AppendLiteral("event:/sfx/enemy/enemy_attacks/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("_die");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06004D62 RID: 19810 RVA: 0x0022E520 File Offset: 0x0022C720
		public virtual bool HasDeathSfx
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06004D63 RID: 19811 RVA: 0x0022E523 File Offset: 0x0022C723
		[Nullable(2)]
		public virtual string HurtSfx
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06004D64 RID: 19812 RVA: 0x0022E526 File Offset: 0x0022C726
		public virtual bool HasHurtSfx
		{
			get
			{
				return this.HurtSfx != null;
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06004D65 RID: 19813 RVA: 0x0022E531 File Offset: 0x0022C731
		protected virtual bool HasPhobiaSpineSkin
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Should the default fade animation be played after this monster dies?
		/// Usually true, but false for monsters with certain special death animations (Decimillipede, etc.) or that we want
		/// to keep around after death (Osty, etc.).
		/// </summary>
		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06004D66 RID: 19814 RVA: 0x0022E534 File Offset: 0x0022C734
		public virtual bool ShouldFadeAfterDeath
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06004D67 RID: 19815 RVA: 0x0022E537 File Offset: 0x0022C737
		public virtual bool ShouldDisappearFromDoom
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06004D68 RID: 19816 RVA: 0x0022E53A File Offset: 0x0022C73A
		public virtual float HurtAnimationTrackOffsetForDoom
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06004D69 RID: 19817 RVA: 0x0022E541 File Offset: 0x0022C741
		public virtual bool ShouldShowInCompendium
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Used for creatures where we can't rely on the length of their death animations to delay the rewards screen.
		/// For example, <see cref="T:MegaCrit.Sts2.Core.Models.Monsters.Crusher" />'s and <see cref="T:MegaCrit.Sts2.Core.Models.Monsters.Rocket" />'s visuals are part of the combat background.
		/// </summary>
		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06004D6A RID: 19818 RVA: 0x0022E544 File Offset: 0x0022C744
		public virtual float DeathAnimLengthOverride
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06004D6B RID: 19819 RVA: 0x0022E54B File Offset: 0x0022C74B
		public bool HasDeathAnimLengthOverride
		{
			get
			{
				return this.DeathAnimLengthOverride > 0f;
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06004D6C RID: 19820 RVA: 0x0022E55A File Offset: 0x0022C75A
		public virtual bool CanChangeScale
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x06004D6D RID: 19821 RVA: 0x0022E55D File Offset: 0x0022C75D
		public virtual DamageSfxType TakeDamageSfxType
		{
			get
			{
				return DamageSfxType.Armor;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x06004D6E RID: 19822 RVA: 0x0022E560 File Offset: 0x0022C760
		public virtual string TakeDamageSfx
		{
			get
			{
				return "event:/sfx/enemy/enemy_impact_enemy_size/enemy_impact_" + StringHelper.Slugify(this.TakeDamageSfxType.ToString()).ToLowerInvariant();
			}
		}

		/// <summary>
		/// The creature that represents this monster in combat.
		/// Will technically be null on a canonical monster model, but we should never be checking that, so we leave this as
		/// non-nullable for convenience.
		/// </summary>
		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06004D6F RID: 19823 RVA: 0x0022E595 File Offset: 0x0022C795
		// (set) Token: 0x06004D70 RID: 19824 RVA: 0x0022E5AC File Offset: 0x0022C7AC
		public Creature Creature
		{
			get
			{
				Creature creature = this._creature;
				if (creature == null)
				{
					throw new InvalidOperationException("Creature was accessed before it was set.");
				}
				return creature;
			}
			set
			{
				base.AssertMutable();
				if (this._creature != null)
				{
					throw new InvalidOperationException("Monster " + base.Id.Entry + " already has a creature.");
				}
				this._creature = value;
			}
		}

		/// <summary>
		/// The CombatState that this monster's creature exists in.
		/// </summary>
		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06004D71 RID: 19825 RVA: 0x0022E5E3 File Offset: 0x0022C7E3
		public ICombatState CombatState
		{
			get
			{
				return this.Creature.CombatState ?? NullCombatState.Instance;
			}
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x0022E5FC File Offset: 0x0022C7FC
		private List<AbstractIntent> GetIntents()
		{
			List<AbstractIntent> list = new List<AbstractIntent>();
			MonsterMoveStateMachine monsterMoveStateMachine = this.GenerateMoveStateMachine();
			foreach (MonsterState monsterState in monsterMoveStateMachine.States.Values)
			{
				if (monsterState.IsMove)
				{
					MoveState moveState = monsterState as MoveState;
					if (moveState != null)
					{
						list.AddRange(moveState.Intents);
					}
				}
			}
			return list;
		}

		/// <summary>
		/// Get this monster's move state machine.
		/// Null for canonical monster models.
		/// </summary>
		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06004D73 RID: 19827 RVA: 0x0022E67C File Offset: 0x0022C87C
		// (set) Token: 0x06004D74 RID: 19828 RVA: 0x0022E684 File Offset: 0x0022C884
		[Nullable(2)]
		public MonsterMoveStateMachine MoveStateMachine
		{
			[NullableContext(2)]
			get
			{
				return this._moveStateMachine;
			}
			[NullableContext(2)]
			private set
			{
				base.AssertMutable();
				if (this.MoveStateMachine != null)
				{
					throw new InvalidOperationException(base.Id.Entry + "'s move state machine has already been set");
				}
				this._moveStateMachine = value;
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06004D75 RID: 19829 RVA: 0x0022E6B6 File Offset: 0x0022C8B6
		// (set) Token: 0x06004D76 RID: 19830 RVA: 0x0022E6BE File Offset: 0x0022C8BE
		public MoveState NextMove { get; private set; } = new MoveState();

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06004D77 RID: 19831 RVA: 0x0022E6C7 File Offset: 0x0022C8C7
		public bool IntendsToAttack
		{
			get
			{
				return this.NextMove.Intents.Any<AbstractIntent>(delegate(AbstractIntent intent)
				{
					IntentType intentType = intent.IntentType;
					return intentType == IntentType.Attack || intentType == IntentType.DeathBlow;
				});
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06004D78 RID: 19832 RVA: 0x0022E6F8 File Offset: 0x0022C8F8
		// (set) Token: 0x06004D79 RID: 19833 RVA: 0x0022E700 File Offset: 0x0022C900
		public bool SpawnedThisTurn
		{
			get
			{
				return this._spawnedThisTurn;
			}
			private set
			{
				base.AssertMutable();
				this._spawnedThisTurn = value;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x06004D7A RID: 19834 RVA: 0x0022E70F File Offset: 0x0022C90F
		// (set) Token: 0x06004D7B RID: 19835 RVA: 0x0022E721 File Offset: 0x0022C921
		public MonsterModel CanonicalInstance
		{
			get
			{
				if (!base.IsMutable)
				{
					return this;
				}
				return this._canonicalInstance;
			}
			private set
			{
				base.AssertMutable();
				this._canonicalInstance = value;
			}
		}

		/// <summary>
		/// Logic to run after the monster is first added to the combat room. No-op by default.
		/// </summary>
		// Token: 0x06004D7C RID: 19836 RVA: 0x0022E730 File Offset: 0x0022C930
		public virtual Task AfterAddedToRoom()
		{
			return Task.CompletedTask;
		}

		/// <summary>
		/// Logic to run after the monster is removed from the combat room. No-op by default.
		/// </summary>
		// Token: 0x06004D7D RID: 19837 RVA: 0x0022E737 File Offset: 0x0022C937
		public virtual void BeforeRemovedFromRoom()
		{
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x0022E73C File Offset: 0x0022C93C
		public virtual List<BestiaryMonsterMove> GenerateBestiaryMoveList([Nullable(2)] NCreatureVisuals creatureVisuals)
		{
			List<BestiaryMonsterMove> list = new List<BestiaryMonsterMove>();
			foreach (string text in this.GetAllMoves(this.MoveStateMachine))
			{
				if (this.ShouldShowMoveInBestiary(text))
				{
					string text2 = text;
					if (text2.EndsWith("_MOVE"))
					{
						string text3 = text2;
						text2 = text3.Substring(0, text3.Length - 5);
					}
					LocString bestiaryMoveName = this.GetBestiaryMoveName(text2);
					if (!bestiaryMoveName.Exists())
					{
						if (!text2.EndsWith('2') && !text2.EndsWith('3') && !text2.EndsWith('4'))
						{
							Log.Warn("No loc for move " + text + " in monster " + this.Title.GetFormattedText(), 2);
							list.Add(BestiaryMonsterMove.FromState(text));
						}
					}
					else
					{
						list.Add(BestiaryMonsterMove.FromState(bestiaryMoveName, text));
					}
				}
			}
			MegaSkeletonDataResource megaSkeletonDataResource;
			if (creatureVisuals == null)
			{
				megaSkeletonDataResource = null;
			}
			else
			{
				MegaSprite spineBody = creatureVisuals.SpineBody;
				if (spineBody == null)
				{
					megaSkeletonDataResource = null;
				}
				else
				{
					MegaSkeleton skeleton = spineBody.GetSkeleton();
					megaSkeletonDataResource = ((skeleton != null) ? skeleton.GetData() : null);
				}
			}
			MegaSkeletonDataResource megaSkeletonDataResource2 = megaSkeletonDataResource;
			if (megaSkeletonDataResource2 != null && megaSkeletonDataResource2.HasAnimation("revive"))
			{
				list.Add(BestiaryMonsterMove.FromAnim("revive", null));
			}
			if (megaSkeletonDataResource2 != null && megaSkeletonDataResource2.HasAnimation("hurt"))
			{
				list.Add(BestiaryMonsterMove.FromAnim("hurt", this.TakeDamageSfx).StopOtherSfx());
			}
			if (megaSkeletonDataResource2 != null && megaSkeletonDataResource2.HasAnimation("die"))
			{
				list.Add(BestiaryMonsterMove.FromAnim("die", this.DeathSfx).StopOtherSfx());
			}
			return list;
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x0022E8E0 File Offset: 0x0022CAE0
		protected virtual bool ShouldShowMoveInBestiary(string moveStateId)
		{
			return true;
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x0022E8E3 File Offset: 0x0022CAE3
		private IEnumerable<string> GetAllMoves(MonsterMoveStateMachine machine)
		{
			MonsterModel.<GetAllMoves>d__97 <GetAllMoves>d__ = new MonsterModel.<GetAllMoves>d__97(-2);
			<GetAllMoves>d__.<>3__machine = machine;
			return <GetAllMoves>d__;
		}

		/// <summary>
		/// Destroys the monster's move state machine.
		/// </summary>
		// Token: 0x06004D81 RID: 19841 RVA: 0x0022E8F3 File Offset: 0x0022CAF3
		public void ResetStateMachine()
		{
			this._moveStateMachine = null;
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x0022E8FC File Offset: 0x0022CAFC
		public static LocString L10NMonsterLookup(string entryName)
		{
			return new LocString("monsters", entryName);
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x0022E90C File Offset: 0x0022CB0C
		public MonsterModel ToMutable()
		{
			base.AssertCanonical();
			MonsterModel monsterModel = (MonsterModel)base.MutableClone();
			monsterModel.CanonicalInstance = this;
			return monsterModel;
		}

		// Token: 0x06004D84 RID: 19844
		protected abstract MonsterMoveStateMachine GenerateMoveStateMachine();

		// Token: 0x06004D85 RID: 19845 RVA: 0x0022E933 File Offset: 0x0022CB33
		public void SetUpForCombat()
		{
			this.MoveStateMachine = this.GenerateMoveStateMachine();
			this.SpawnedThisTurn = true;
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x0022E948 File Offset: 0x0022CB48
		public void RollMove(IEnumerable<Creature> targets)
		{
			this.NextMove = this.MoveStateMachine.RollMove(targets, this.Creature, this.RunRng.MonsterAi);
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x0022E970 File Offset: 0x0022CB70
		public void SetMoveImmediate(MoveState state, bool forceTransition = false)
		{
			if (!this.NextMove.CanTransitionAway && !forceTransition)
			{
				return;
			}
			this.NextMove = state;
			this.MoveStateMachine.ForceCurrentState(state);
			NCreature creatureNode = this.Creature.GetCreatureNode();
			if (creatureNode != null && this.CombatState.IsLiveCombat())
			{
				TaskHelper.RunSafely(creatureNode.RefreshIntents());
			}
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x0022E9CC File Offset: 0x0022CBCC
		public async Task PerformMove()
		{
			if (this.CombatState != null)
			{
				ICombatState combatState = this.CombatState;
				await Cmd.CustomScaledWait(0.1f, 0.2f, false, default(CancellationToken));
				this.IsPerformingMove = true;
				MoveState move = this.NextMove;
				IReadOnlyList<Creature> targets = combatState.PlayerCreatures;
				if (TestMode.IsOff)
				{
					Log.Info("Monster " + base.Id.Entry + " performing move " + move.Id, 2);
				}
				await move.PerformMove(targets);
				MonsterMoveStateMachine moveStateMachine = this.MoveStateMachine;
				if (moveStateMachine != null)
				{
					moveStateMachine.OnMovePerformed(move);
				}
				CombatManager.Instance.History.MonsterPerformedMove(combatState, this, move, targets);
				this.IsPerformingMove = false;
				if (this.Creature.IsDead && Hook.ShouldCreatureBeRemovedFromCombatAfterDeath(combatState, this.Creature))
				{
					combatState.RemoveCreature(this.Creature, true);
				}
				await Cmd.CustomScaledWait(0.1f, 0.4f, false, default(CancellationToken));
			}
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x0022EA0F File Offset: 0x0022CC0F
		public virtual void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
		{
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x0022EA14 File Offset: 0x0022CC14
		public virtual CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("idle_loop", true);
			AnimState animState2 = new AnimState("cast", false);
			AnimState animState3 = new AnimState("attack", false);
			AnimState animState4 = new AnimState("hurt", false);
			AnimState animState5 = new AnimState("die", false);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState, null);
			creatureAnimator.AddAnyState("Cast", animState2, null);
			creatureAnimator.AddAnyState("Attack", animState3, null);
			creatureAnimator.AddAnyState("Dead", animState5, null);
			creatureAnimator.AddAnyState("Hit", animState4, null);
			return creatureAnimator;
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x0022EAC5 File Offset: 0x0022CCC5
		public void OnSideSwitch()
		{
			base.AssertMutable();
			this.SpawnedThisTurn = false;
		}

		/// <summary>
		/// Called when this monster is about to die to Doom.
		/// Primarily used set up the creature visuals for the Doom vfx (ie hiding nodes that have additive materials)
		/// </summary>
		// Token: 0x06004D8C RID: 19852 RVA: 0x0022EAD4 File Offset: 0x0022CCD4
		public virtual void OnDieToDoom()
		{
		}

		/// <summary>
		/// Helper to get the move name label for the Bestiary entry.
		/// </summary>
		// Token: 0x06004D8D RID: 19853 RVA: 0x0022EAD6 File Offset: 0x0022CCD6
		protected LocString GetBestiaryMoveName(string moveId)
		{
			return new LocString("monsters", base.Id.Entry + ".moves." + moveId + ".title");
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x0022EB00 File Offset: 0x0022CD00
		public void OnPhobiaModeToggled(bool isOn, MegaSprite spine, MegaSkeleton skeleton)
		{
			if (!this.HasPhobiaSpineSkin)
			{
				return;
			}
			MegaSkin megaSkin = spine.NewSkin("custom-skin");
			MegaSkeletonDataResource data = skeleton.GetData();
			megaSkin.AddSkin(data.FindSkin(isOn ? "phobia" : "normal"));
			skeleton.SetSkin(megaSkin);
			skeleton.SetSlotsToSetupPose();
		}

		// Token: 0x04001D5E RID: 7518
		private static readonly string _fallbackVisualsPath = SceneHelper.GetScenePath("creature_visuals/fallback");

		// Token: 0x04001D5F RID: 7519
		public static readonly Vector2 defaultDeathVfxPadding = 1.2f * Vector2.One;

		// Token: 0x04001D60 RID: 7520
		public const string stunnedMoveId = "STUNNED";

		// Token: 0x04001D61 RID: 7521
		protected const string _locTableName = "monsters";

		// Token: 0x04001D62 RID: 7522
		[Nullable(2)]
		private Rng _rng;

		// Token: 0x04001D63 RID: 7523
		[Nullable(2)]
		private RunRngSet _runRng;

		// Token: 0x04001D64 RID: 7524
		private bool _isPerformingMove;

		// Token: 0x04001D65 RID: 7525
		[Nullable(2)]
		private Creature _creature;

		// Token: 0x04001D66 RID: 7526
		[Nullable(2)]
		private MonsterMoveStateMachine _moveStateMachine;

		// Token: 0x04001D68 RID: 7528
		private bool _spawnedThisTurn;

		// Token: 0x04001D69 RID: 7529
		private MonsterModel _canonicalInstance;
	}
}
