using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004E9 RID: 1257
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class PotionModel : AbstractModel
	{
		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06004DC5 RID: 19909 RVA: 0x0022F1A4 File Offset: 0x0022D3A4
		// (remove) Token: 0x06004DC6 RID: 19910 RVA: 0x0022F1DC File Offset: 0x0022D3DC
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action BeforeUse;

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06004DC7 RID: 19911 RVA: 0x0022F211 File Offset: 0x0022D411
		public LocString Title
		{
			get
			{
				return new LocString("potions", base.Id.Entry + ".title");
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06004DC8 RID: 19912 RVA: 0x0022F232 File Offset: 0x0022D432
		private LocString Description
		{
			get
			{
				return new LocString("potions", base.Id.Entry + ".description");
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06004DC9 RID: 19913 RVA: 0x0022F253 File Offset: 0x0022D453
		public LocString SelectionScreenPrompt
		{
			get
			{
				return new LocString("potions", base.Id.Entry + ".selectionScreenPrompt");
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06004DCA RID: 19914 RVA: 0x0022F274 File Offset: 0x0022D474
		public LocString DynamicDescription
		{
			get
			{
				LocString description = this.Description;
				this.DynamicVars.AddTo(description);
				string prefix = EnergyIconHelper.GetPrefix(this);
				description.Add("energyPrefix", EnergyIconHelper.GetPrefix(this));
				foreach (KeyValuePair<string, object> keyValuePair in description.Variables)
				{
					EnergyVar energyVar = keyValuePair.Value as EnergyVar;
					if (energyVar != null)
					{
						energyVar.ColorPrefix = prefix;
					}
				}
				return description;
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06004DCB RID: 19915 RVA: 0x0022F300 File Offset: 0x0022D500
		private string PackedImagePath
		{
			get
			{
				return ImageHelper.GetImagePath("atlases/potion_atlas.sprites/" + base.Id.Entry.ToLowerInvariant() + ".tres");
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06004DCC RID: 19916 RVA: 0x0022F326 File Offset: 0x0022D526
		private string PackedOutlinePath
		{
			get
			{
				return ImageHelper.GetImagePath("atlases/potion_outline_atlas.sprites/" + base.Id.Entry.ToLowerInvariant() + ".tres");
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06004DCD RID: 19917 RVA: 0x0022F34C File Offset: 0x0022D54C
		public string ImagePath
		{
			get
			{
				return this.PackedImagePath;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06004DCE RID: 19918 RVA: 0x0022F354 File Offset: 0x0022D554
		public Texture2D Image
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(this.PackedImagePath, null, 1L);
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06004DCF RID: 19919 RVA: 0x0022F364 File Offset: 0x0022D564
		[Nullable(2)]
		public string OutlinePath
		{
			[NullableContext(2)]
			get
			{
				if (!ResourceLoader.Exists(this.PackedOutlinePath, ""))
				{
					return null;
				}
				return this.PackedOutlinePath;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06004DD0 RID: 19920 RVA: 0x0022F380 File Offset: 0x0022D580
		[Nullable(2)]
		public Texture2D Outline
		{
			[NullableContext(2)]
			get
			{
				if (this.OutlinePath == null)
				{
					return null;
				}
				return ResourceLoader.Load<Texture2D>(this.OutlinePath, null, 1L);
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06004DD1 RID: 19921 RVA: 0x0022F39A File Offset: 0x0022D59A
		public string LargeImagePath
		{
			get
			{
				return ImageHelper.GetImagePath("potions/large/" + base.Id.Entry.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06004DD2 RID: 19922 RVA: 0x0022F3C0 File Offset: 0x0022D5C0
		public Texture2D LargeImage
		{
			get
			{
				return ResourceLoader.Load<Texture2D>(ResourceLoader.Exists(this.LargeImagePath, "") ? this.LargeImagePath : ImageHelper.GetImagePath("potions/missing_potion.png"), null, 1L);
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06004DD3 RID: 19923
		public abstract PotionRarity Rarity { get; }

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06004DD4 RID: 19924
		public abstract PotionUsage Usage { get; }

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06004DD5 RID: 19925
		public abstract TargetType TargetType { get; }

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06004DD6 RID: 19926 RVA: 0x0022F3EE File Offset: 0x0022D5EE
		public PotionPoolModel Pool
		{
			get
			{
				return ModelDb.AllPotionPools.First<PotionPoolModel>((PotionPoolModel p) => p.AllPotionIds.Contains(base.Id));
			}
		}

		/// <summary>
		/// Get the player that owns this relic.
		/// Will technically be null on a canonical relic model, but we should never be checking that, so we leave this as
		/// non-nullable for convenience.
		/// </summary>
		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06004DD7 RID: 19927 RVA: 0x0022F406 File Offset: 0x0022D606
		// (set) Token: 0x06004DD8 RID: 19928 RVA: 0x0022F414 File Offset: 0x0022D614
		public Player Owner
		{
			get
			{
				base.AssertMutable();
				return this._owner;
			}
			set
			{
				base.AssertMutable();
				if (this._owner != null && this._owner != value)
				{
					throw new InvalidOperationException("Cannot move potion " + base.Id.Entry + " from one owner to another");
				}
				this._owner = value;
			}
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06004DD9 RID: 19929 RVA: 0x0022F454 File Offset: 0x0022D654
		public DynamicVarSet DynamicVars
		{
			get
			{
				if (this._dynamicVars != null)
				{
					return this._dynamicVars;
				}
				this._dynamicVars = new DynamicVarSet(this.CanonicalVars);
				this._dynamicVars.InitializeWithOwner(this);
				return this._dynamicVars;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06004DDA RID: 19930 RVA: 0x0022F488 File Offset: 0x0022D688
		protected virtual IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return Array.Empty<DynamicVar>();
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06004DDB RID: 19931 RVA: 0x0022F48F File Offset: 0x0022D68F
		// (set) Token: 0x06004DDC RID: 19932 RVA: 0x0022F497 File Offset: 0x0022D697
		public bool IsQueued { get; private set; }

		/// <summary>
		/// Whether or not using this potion can heal the player or their pets (like Fruit Juice), or do other restricted
		/// actions (like Fairy in a Bottle's resurrection).
		///
		/// Used primarily to filter potions out of random in-combat generation effects, to avoid making annoying
		/// "optimal play" behavior.
		/// </summary>
		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06004DDD RID: 19933 RVA: 0x0022F4A0 File Offset: 0x0022D6A0
		public virtual bool CanBeGeneratedInCombat
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Whether or not this potion passes its custom usability check.
		/// By default, there are no custom checks, so this always returns true.
		/// Subclasses can override this to add custom checks.
		/// </summary>
		/// <example>
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Potions.FoulPotion" /> makes sure that we're either in combat, at the Merchant, or in the
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Events.FakeMerchant" /> event.
		/// </example>
		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06004DDE RID: 19934 RVA: 0x0022F4A3 File Offset: 0x0022D6A3
		public virtual bool PassesCustomUsabilityCheck
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06004DDF RID: 19935 RVA: 0x0022F4A8 File Offset: 0x0022D6A8
		public HoverTip HoverTip
		{
			get
			{
				HoverTip hoverTip = new HoverTip(this.Title, this.DynamicDescription, null);
				hoverTip.SetCanonicalModel(this.CanonicalInstance);
				return hoverTip;
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06004DE0 RID: 19936 RVA: 0x0022F4D7 File Offset: 0x0022D6D7
		public IEnumerable<IHoverTip> HoverTips
		{
			get
			{
				return new IHoverTip[] { this.HoverTip }.Concat<IHoverTip>(this.ExtraHoverTips);
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06004DE1 RID: 19937 RVA: 0x0022F4F8 File Offset: 0x0022D6F8
		public virtual IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return Array.Empty<IHoverTip>();
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06004DE2 RID: 19938 RVA: 0x0022F4FF File Offset: 0x0022D6FF
		// (set) Token: 0x06004DE3 RID: 19939 RVA: 0x0022F511 File Offset: 0x0022D711
		public PotionModel CanonicalInstance
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

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06004DE4 RID: 19940 RVA: 0x0022F520 File Offset: 0x0022D720
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Set to true when this potion is removed from a player's potion belt.
		/// </summary>
		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06004DE5 RID: 19941 RVA: 0x0022F523 File Offset: 0x0022D723
		// (set) Token: 0x06004DE6 RID: 19942 RVA: 0x0022F52B File Offset: 0x0022D72B
		public bool HasBeenRemovedFromState { get; private set; }

		// Token: 0x06004DE7 RID: 19943 RVA: 0x0022F534 File Offset: 0x0022D734
		public PotionModel ToMutable()
		{
			base.AssertCanonical();
			PotionModel potionModel = (PotionModel)base.MutableClone();
			potionModel.CanonicalInstance = this;
			return potionModel;
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x0022F55B File Offset: 0x0022D75B
		protected override void AfterCloned()
		{
			base.AfterCloned();
			this.HasBeenRemovedFromState = false;
			this.BeforeUse = null;
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x0022F571 File Offset: 0x0022D771
		public void Discard()
		{
			this.Owner.DiscardPotionInternal(this, false);
			this.HasBeenRemovedFromState = true;
		}

		/// <summary>
		/// Remove this potion as part of using it.
		/// </summary>
		// Token: 0x06004DEA RID: 19946 RVA: 0x0022F587 File Offset: 0x0022D787
		public void RemoveBeforeUse()
		{
			this.Owner.RemoveUsedPotionInternal(this);
			this.HasBeenRemovedFromState = true;
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x0022F59C File Offset: 0x0022D79C
		[NullableContext(2)]
		public void EnqueueManualUse(Creature target)
		{
			base.AssertMutable();
			Action beforeUse = this.BeforeUse;
			if (beforeUse != null)
			{
				beforeUse();
			}
			if (target == null && this.IsValidTarget(this.Owner.Creature))
			{
				target = this.Owner.Creature;
			}
			UsePotionAction usePotionAction = new UsePotionAction(this, target, CombatManager.Instance.IsInProgress);
			this.IsQueued = true;
			RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(usePotionAction);
		}

		/// <summary>
		/// Returns true if target is valid for this potion.
		/// NOTE: This operates differently than cards! Do not try to unify this with CardModel.IsValidTarget unless you
		/// change UI targeting; namely, CardModel's TargetType.Self does not pass a target, whereas potions do.
		/// </summary>
		// Token: 0x06004DEC RID: 19948 RVA: 0x0022F60C File Offset: 0x0022D80C
		[NullableContext(2)]
		public bool IsValidTarget(Creature target)
		{
			if (target == null)
			{
				return this.TargetType == TargetType.TargetedNoCreature || !this.TargetType.IsSingleTarget();
			}
			if (!target.IsAlive)
			{
				return false;
			}
			if (this.TargetType == TargetType.AnyEnemy)
			{
				return target.Side != this.Owner.Creature.Side;
			}
			if (this.TargetType == TargetType.AnyAlly)
			{
				return target.Side == this.Owner.Creature.Side && target != this.Owner.Creature;
			}
			if (this.TargetType == TargetType.AnyPlayer)
			{
				return target.IsPlayer;
			}
			return this.TargetType == TargetType.Self && target == this.Owner.Creature;
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x0022F6C4 File Offset: 0x0022D8C4
		public async Task OnUseWrapper(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			this.RemoveBeforeUse();
			ICombatState combatState = this.Owner.Creature.CombatState;
			choiceContext.PushModel(this);
			await CombatManager.Instance.WaitForUnpause();
			await Hook.BeforePotionUsed(this.Owner.RunState, combatState, this, target);
			if (TestMode.IsOff && combatState != null)
			{
				NCombatRoom instance = NCombatRoom.Instance;
				NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(this.Owner.Creature) : null);
				Vector2 vector = Vector2.Zero;
				if (this.TargetType.IsSingleTarget())
				{
					NCombatRoom instance2 = NCombatRoom.Instance;
					NCreature ncreature2 = ((instance2 != null) ? instance2.GetCreatureNode(target) : null);
					vector = ((ncreature2 != null) ? ncreature2.GetBottomOfHitbox() : Vector2.Zero);
				}
				else
				{
					IReadOnlyList<Creature> readOnlyList;
					if (this.TargetType == TargetType.AllEnemies)
					{
						readOnlyList = (from c in combatState.GetCreaturesOnSide(CombatSide.Enemy)
							where c.IsHittable
							select c).ToList<Creature>();
					}
					else
					{
						readOnlyList = (from c in combatState.GetCreaturesOnSide(CombatSide.Player)
							where c.IsHittable
							select c).ToList<Creature>();
					}
					foreach (Creature creature in readOnlyList)
					{
						NCombatRoom instance3 = NCombatRoom.Instance;
						NCreature ncreature3 = ((instance3 != null) ? instance3.GetCreatureNode(creature) : null);
						vector += ((ncreature3 != null) ? ncreature3.VfxSpawnPosition : Vector2.Zero);
					}
					vector /= (float)readOnlyList.Count;
				}
				NItemThrowVfx nitemThrowVfx = NItemThrowVfx.Create((ncreature != null) ? ncreature.VfxSpawnPosition : Vector2.Zero, vector, this.Image, null);
				NCombatRoom instance4 = NCombatRoom.Instance;
				if (instance4 != null)
				{
					instance4.CombatVfxContainer.AddChildSafely(nitemThrowVfx);
				}
				await Cmd.Wait(0.5f, false);
			}
			CombatManager.Instance.BeginCardOrPotionEffect(this.Owner);
			object obj = null;
			try
			{
				BranchingPlayerChoiceContext branchingPlayerChoiceContext = new BranchingPlayerChoiceContext(LocalContext.NetId.Value, GameActionType.Combat, choiceContext);
				branchingPlayerChoiceContext.PushModel(this);
				await branchingPlayerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(this.OnUse(branchingPlayerChoiceContext, target));
			}
			catch (object obj)
			{
			}
			await CombatManager.Instance.EndCardOrPotionEffect(this.Owner);
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			obj = null;
			if (!this.Owner.Creature.IsDead)
			{
				base.InvokeExecutionFinished();
				if (combatState != null && CombatManager.Instance.IsInProgress)
				{
					CombatManager.Instance.History.PotionUsed(combatState, this, target);
				}
				await Hook.AfterPotionUsed(this.Owner.RunState, combatState, this, target);
				MapPointHistoryEntry currentMapPointHistoryEntry = this.Owner.RunState.CurrentMapPointHistoryEntry;
				if (currentMapPointHistoryEntry != null)
				{
					currentMapPointHistoryEntry.GetEntry(this.Owner.NetId).PotionUsed.Add(base.Id);
				}
				await CombatManager.Instance.CheckForEmptyHand(choiceContext, this.Owner);
				choiceContext.PopModel(this);
			}
		}

		// Token: 0x06004DEE RID: 19950 RVA: 0x0022F717 File Offset: 0x0022D917
		public void AfterUsageCanceled()
		{
			this.IsQueued = false;
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x0022F720 File Offset: 0x0022D920
		protected virtual Task OnUse(PlayerChoiceContext choiceContext, [Nullable(2)] Creature target)
		{
			return Task.CompletedTask;
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x0022F727 File Offset: 0x0022D927
		public SerializablePotion ToSerializable(int slotIndex)
		{
			base.AssertMutable();
			return new SerializablePotion
			{
				Id = base.Id,
				SlotIndex = slotIndex
			};
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x0022F747 File Offset: 0x0022D947
		public static PotionModel FromSerializable(SerializablePotion save)
		{
			return SaveUtil.PotionOrDeprecated(save.Id).ToMutable();
		}

		/// <summary>
		/// Ensure that the target is valid (non-null) for a targeted potion.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">Thrown if the target is null.</exception>
		// Token: 0x06004DF2 RID: 19954 RVA: 0x0022F759 File Offset: 0x0022D959
		[NullableContext(2)]
		protected static void AssertValidForTargetedPotion([NotNull] Creature target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target", "Target must be present for targeted potions.");
			}
		}

		// Token: 0x06004DF3 RID: 19955 RVA: 0x0022F76E File Offset: 0x0022D96E
		public bool CanThrowAtAlly()
		{
			return this.TargetType == TargetType.AnyPlayer && this.Owner.RunState.Players.Count > 1 && CombatManager.Instance.IsInProgress;
		}

		// Token: 0x04001D71 RID: 7537
		public const string locTable = "potions";

		// Token: 0x04001D73 RID: 7539
		[Nullable(2)]
		private Player _owner;

		// Token: 0x04001D74 RID: 7540
		[Nullable(2)]
		private DynamicVarSet _dynamicVars;

		// Token: 0x04001D76 RID: 7542
		private PotionModel _canonicalInstance;
	}
}
