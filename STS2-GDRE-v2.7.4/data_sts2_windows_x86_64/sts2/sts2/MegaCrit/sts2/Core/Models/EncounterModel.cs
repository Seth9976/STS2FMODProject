using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models
{
	/// <summary>
	/// An encounter represents a single combat encounter. It can be a weak encounter, normal enemies, elites,
	/// boss fight, or enemies you fight during an event. Encounters may be made up of one or more monsters and
	/// the enemies the player fights may not always be the same due to randomization of that encounter.
	/// </summary>
	// Token: 0x020004E0 RID: 1248
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class EncounterModel : AbstractModel
	{
		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x06004C5B RID: 19547 RVA: 0x0022BADE File Offset: 0x00229CDE
		public override bool ShouldReceiveCombatHooks
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// A per-encounter RNG that we can use to do random rolls in the encounter (for things like monster HP)
		/// independently of the run's centralized RNG.
		/// This is safe to do in encounters because we don't need to keep track of a given encounter's RNG state once it's
		/// over.
		/// The private backing field for this can be null, but by the time this is being accessed by subclasses, it should
		/// always be set. If it's not, you're doing something wrong.
		/// </summary>
		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06004C5C RID: 19548 RVA: 0x0022BAE1 File Offset: 0x00229CE1
		protected Rng Rng
		{
			get
			{
				return this._rng;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06004C5D RID: 19549
		public abstract RoomType RoomType { get; }

		/// <summary>
		/// Is this one of the weak encounters that we start acts with?
		/// </summary>
		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06004C5E RID: 19550 RVA: 0x0022BAE9 File Offset: 0x00229CE9
		public virtual bool IsWeak
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Should this encounter give combat rewards when it's over?
		/// Usually true, but many event-specific encounters set this to false.
		/// </summary>
		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06004C5F RID: 19551 RVA: 0x0022BAEC File Offset: 0x00229CEC
		public virtual bool ShouldGiveRewards
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06004C60 RID: 19552 RVA: 0x0022BAF0 File Offset: 0x00229CF0
		public virtual int MinGoldReward
		{
			get
			{
				int num;
				switch (this.RoomType)
				{
				case RoomType.Monster:
					num = 10;
					break;
				case RoomType.Elite:
					num = 35;
					break;
				case RoomType.Boss:
					num = 100;
					break;
				default:
					num = 0;
					break;
				}
				double num2 = (double)num;
				if (AscensionHelper.HasAscension(AscensionLevel.Poverty))
				{
					num2 *= AscensionHelper.PovertyAscensionGoldMultiplier;
				}
				return (int)num2;
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06004C61 RID: 19553 RVA: 0x0022BB40 File Offset: 0x00229D40
		public virtual int MaxGoldReward
		{
			get
			{
				int num;
				switch (this.RoomType)
				{
				case RoomType.Monster:
					num = 20;
					break;
				case RoomType.Elite:
					num = 45;
					break;
				case RoomType.Boss:
					num = 100;
					break;
				default:
					num = 0;
					break;
				}
				double num2 = (double)num;
				if (AscensionHelper.HasAscension(AscensionLevel.Poverty))
				{
					num2 *= AscensionHelper.PovertyAscensionGoldMultiplier;
				}
				return (int)num2;
			}
		}

		/// <summary>
		/// The description for this encounter's custom reward.
		/// For example, <see cref="T:MegaCrit.Sts2.Core.Models.Monsters.ThievingHopper" /> uses a <see cref="T:MegaCrit.Sts2.Core.Rewards.SpecialCardReward" /> to give you your stolen card
		/// back, and we use this field to specify the "Take your stolen card back" text for the reward button.
		/// </summary>
		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06004C62 RID: 19554 RVA: 0x0022BB90 File Offset: 0x00229D90
		[Nullable(2)]
		public LocString CustomRewardDescription
		{
			[NullableContext(2)]
			get
			{
				return LocString.GetIfExists("encounters", base.Id.Entry + ".customRewardDescription");
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x06004C63 RID: 19555 RVA: 0x0022BBB1 File Offset: 0x00229DB1
		public virtual IEnumerable<EncounterTag> Tags
		{
			get
			{
				return Array.Empty<EncounterTag>();
			}
		}

		/// <summary>
		/// Have this encounter's monsters been generated yet?
		/// Used for delayed-start combats (like combat-style events that can transition to combats) to make sure we don't
		/// try to re-generated already generated monsters when the actual combat starts.
		/// </summary>
		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x06004C64 RID: 19556 RVA: 0x0022BBB8 File Offset: 0x00229DB8
		public bool HaveMonstersBeenGenerated
		{
			get
			{
				return this._monstersWithSlots != null;
			}
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x0022BBC3 File Offset: 0x00229DC3
		public virtual float GetCameraScaling()
		{
			return 1f;
		}

		// Token: 0x06004C66 RID: 19558 RVA: 0x0022BBCA File Offset: 0x00229DCA
		public virtual Vector2 GetCameraOffset()
		{
			return Vector2.Zero;
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x0022BBD4 File Offset: 0x00229DD4
		public string GetNextSlot(ICombatState combatState)
		{
			return this.Slots.FirstOrDefault((string s) => combatState.Enemies.All<Creature>((Creature c) => c.SlotName != s), string.Empty);
		}

		// Token: 0x06004C68 RID: 19560
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		protected abstract IReadOnlyList<ValueTuple<MonsterModel, string>> GenerateMonsters();

		/// <summary>
		/// Generate the monsters that the players should fight in this encounter.
		/// </summary>
		/// <param name="runState">
		/// The <see cref="T:MegaCrit.Sts2.Core.Runs.IRunState" /> that this encounter exists in.
		/// Used for things like generating a deterministic RNG, etc.
		/// </param>
		// Token: 0x06004C69 RID: 19561 RVA: 0x0022BC0C File Offset: 0x00229E0C
		public void GenerateMonstersWithSlots(IRunState runState)
		{
			base.AssertMutable();
			if (this._monstersWithSlots != null)
			{
				throw new InvalidOperationException("Monsters have already been generated for this encounter.");
			}
			if (this._rng == null)
			{
				ulong num = runState.Rng.Seed + (ulong)((long)runState.TotalFloor) + StringHelper.GetDeterministicHashCode(base.Id.Entry);
				this._rng = new Rng(num);
			}
			this._monstersWithSlots = this.GenerateMonsters();
			foreach (ValueTuple<MonsterModel, string> valueTuple in this._monstersWithSlots)
			{
				MonsterModel item = valueTuple.Item1;
				item.AssertMutable();
			}
		}

		/// <summary>
		/// Returns a list of (mutable) monsters in this encounter, along with the slot they should be placed in.
		/// This should only be used when generating a real encounter, not when generating the hypothetical monsters that could
		/// be in the encounter. For that, use AllPossibleMonsters.
		/// </summary>
		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x06004C6A RID: 19562 RVA: 0x0022BCBC File Offset: 0x00229EBC
		[Nullable(new byte[] { 1, 0, 1, 2 })]
		public IReadOnlyList<ValueTuple<MonsterModel, string>> MonstersWithSlots
		{
			[return: Nullable(new byte[] { 1, 0, 1, 2 })]
			get
			{
				base.AssertMutable();
				if (this._monstersWithSlots == null)
				{
					throw new InvalidOperationException("GenerateMonstersWithSlots must be called before using this property!");
				}
				return this._monstersWithSlots;
			}
		}

		/// <summary>
		/// A list of all the monsters that were present in this encounter.
		/// This counts monsters that were initially spawned in the encounter, as well as monsters that were summoned during
		/// it.
		/// </summary>
		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06004C6B RID: 19563 RVA: 0x0022BCDD File Offset: 0x00229EDD
		public IReadOnlyList<MonsterModel> SpawnedEnemies
		{
			get
			{
				base.AssertMutable();
				return this._spawnedEnemies ?? new List<MonsterModel>();
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06004C6C RID: 19564
		public abstract IEnumerable<MonsterModel> AllPossibleMonsters { get; }

		// Token: 0x06004C6D RID: 19565 RVA: 0x0022BCF4 File Offset: 0x00229EF4
		[NullableContext(2)]
		public bool SharesTagsWith(EncounterModel other)
		{
			return other != null && this.Tags.Intersect<EncounterTag>(other.Tags).Any<EncounterTag>();
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06004C6E RID: 19566 RVA: 0x0022BD11 File Offset: 0x00229F11
		// (set) Token: 0x06004C6F RID: 19567 RVA: 0x0022BD23 File Offset: 0x00229F23
		public EncounterModel CanonicalInstance
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

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06004C70 RID: 19568 RVA: 0x0022BD32 File Offset: 0x00229F32
		public virtual bool HasScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06004C71 RID: 19569 RVA: 0x0022BD35 File Offset: 0x00229F35
		public virtual IReadOnlyList<string> Slots
		{
			get
			{
				return Array.Empty<string>();
			}
		}

		/// <summary>
		/// Should the players be fully centered in the encounter?
		/// Usually this is false, because we want to leave padding in the center for card plays.
		/// However, in certain situations (like "surrounded" combats), we want to fully center the players and just live
		/// with them being temporarily covered by played cards.
		/// </summary>
		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06004C72 RID: 19570 RVA: 0x0022BD3C File Offset: 0x00229F3C
		public virtual bool FullyCenterPlayers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06004C73 RID: 19571 RVA: 0x0022BD3F File Offset: 0x00229F3F
		private string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("encounters/" + base.Id.Entry.ToLowerInvariant());
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06004C74 RID: 19572 RVA: 0x0022BD60 File Offset: 0x00229F60
		protected virtual bool HasCustomBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x0022BD63 File Offset: 0x00229F63
		public NCombatBackground CreateBackground(ActModel parentAct, Rng rng)
		{
			return NCombatBackground.Create(this.GetBackgroundAssets(parentAct, rng));
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x0022BD72 File Offset: 0x00229F72
		private BackgroundAssets GetBackgroundAssets(ActModel parentAct, Rng rng)
		{
			base.AssertMutable();
			if (this._backgroundAssets == null)
			{
				if (this.HasCustomBackground)
				{
					this._backgroundAssets = this.CreateBackgroundAssetsForCustom(rng);
				}
				else
				{
					this._backgroundAssets = parentAct.GenerateBackgroundAssets(rng);
				}
			}
			return this._backgroundAssets;
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x0022BDAC File Offset: 0x00229FAC
		private BackgroundAssets CreateBackgroundAssetsForCustom(Rng rng)
		{
			return new BackgroundAssets(base.Id.Entry.ToLowerInvariant(), rng);
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06004C78 RID: 19576 RVA: 0x0022BDC4 File Offset: 0x00229FC4
		public virtual string CustomBgm
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06004C79 RID: 19577 RVA: 0x0022BDCB File Offset: 0x00229FCB
		public bool HasBgm
		{
			get
			{
				return this.CustomBgm != "";
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06004C7A RID: 19578 RVA: 0x0022BDDD File Offset: 0x00229FDD
		public virtual string AmbientSfx
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06004C7B RID: 19579 RVA: 0x0022BDE4 File Offset: 0x00229FE4
		public bool HasAmbientSfx
		{
			get
			{
				return this.AmbientSfx != "";
			}
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x0022BDF6 File Offset: 0x00229FF6
		public Control CreateScene()
		{
			return PreloadManager.Cache.GetScene(this.ScenePath).Instantiate<Control>(0L);
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06004C7D RID: 19581 RVA: 0x0022BE10 File Offset: 0x0022A010
		public virtual string BossNodePath
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("res://animations/map/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(base.Id.Entry.ToLowerInvariant());
				defaultInterpolatedStringHandler.AppendLiteral("_node_skel_data.tres");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004C7E RID: 19582 RVA: 0x0022BE80 File Offset: 0x0022A080
		[Nullable(2)]
		public virtual MegaSkeletonDataResource BossNodeSpineResource
		{
			[NullableContext(2)]
			get
			{
				if (!ResourceLoader.Exists(this.BossNodePath, ""))
				{
					return null;
				}
				return new MegaSkeletonDataResource(PreloadManager.Cache.GetAsset<Resource>(this.BossNodePath));
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004C7F RID: 19583 RVA: 0x0022BEB0 File Offset: 0x0022A0B0
		public LocString Title
		{
			get
			{
				return EncounterModel.L10NLookup(base.Id.Entry + ".title");
			}
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x0022BECC File Offset: 0x0022A0CC
		public EncounterModel ToMutable()
		{
			base.AssertCanonical();
			EncounterModel encounterModel = (EncounterModel)base.MutableClone();
			encounterModel.CanonicalInstance = this;
			return encounterModel;
		}

		/// <summary>
		/// Used for pre-loading boss map assets
		/// </summary>
		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06004C81 RID: 19585 RVA: 0x0022BEF4 File Offset: 0x0022A0F4
		public IEnumerable<string> MapNodeAssetPaths
		{
			get
			{
				if (this.BossNodeSpineResource != null)
				{
					return new <>z__ReadOnlySingleElementList<string>(this.BossNodePath);
				}
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					this.BossNodePath + ".png",
					this.BossNodePath + "_outline.png"
				});
			}
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x0022BF48 File Offset: 0x0022A148
		public IEnumerable<string> GetAssetPaths(IRunState runState)
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.UnionWith(this.GetBackgroundAssets(runState.Act, NCombatRoom.GenerateBackgroundRngForCurrentPoint(runState)).AssetPaths);
			if (this.HasScene)
			{
				hashSet.Add(this.ScenePath);
			}
			hashSet.UnionWith(this.ExtraAssetPaths);
			foreach (ValueTuple<MonsterModel, string> valueTuple in this.MonstersWithSlots)
			{
				MonsterModel item = valueTuple.Item1;
				hashSet.UnionWith(item.AssetPaths);
			}
			return hashSet;
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06004C83 RID: 19587 RVA: 0x0022BFE4 File Offset: 0x0022A1E4
		public virtual IEnumerable<string> ExtraAssetPaths
		{
			get
			{
				return Array.Empty<string>();
			}
		}

		/// <summary>
		/// Randomize the per-encounter RNG.
		/// Should only be used for debugging (like in the `encounter` console command) to keep from
		/// rolling the same
		/// </summary>
		// Token: 0x06004C84 RID: 19588 RVA: 0x0022BFEC File Offset: 0x0022A1EC
		public void DebugRandomizeRng()
		{
			base.AssertMutable();
			this._rng = new Rng((ulong)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds);
		}

		/// <summary>
		/// Get the message to show when the specified character loses in this encounter.
		/// </summary>
		/// <param name="character">Character that lost.</param>
		// Token: 0x06004C85 RID: 19589 RVA: 0x0022C024 File Offset: 0x0022A224
		public LocString GetLossMessageFor(CharacterModel character)
		{
			LocString locString = EncounterModel.L10NLookup(base.Id.Entry + ".loss");
			character.AddDetailsTo(locString);
			locString.Add("encounter", this.Title);
			return locString;
		}

		/// <summary>
		/// The proportion (0-1) of gold rewards the players receive when this encounter's combat ends.
		/// Defaults to the fraction of spawned enemies that were defeated rather than escaped.
		/// Override for encounters with bespoke escape/reward rules (e.g. <see cref="T:MegaCrit.Sts2.Core.Models.Encounters.GremlinMercNormal" />).
		/// </summary>
		// Token: 0x06004C86 RID: 19590 RVA: 0x0022C065 File Offset: 0x0022A265
		public virtual float CalculateGoldProportion(CombatState combatState)
		{
			return 1f - (float)combatState.EscapedCreatures.Count / (float)this.SpawnedEnemies.Count;
		}

		/// <summary>
		/// Override to persist encounter-specific state when saving a pre-finished combat room.
		/// </summary>
		// Token: 0x06004C87 RID: 19591 RVA: 0x0022C086 File Offset: 0x0022A286
		public virtual Dictionary<string, string> SaveCustomState()
		{
			return new Dictionary<string, string>();
		}

		/// <summary>
		/// Override to restore encounter-specific state when loading a pre-finished combat room.
		/// </summary>
		// Token: 0x06004C88 RID: 19592 RVA: 0x0022C08D File Offset: 0x0022A28D
		public virtual void LoadCustomState(Dictionary<string, string> state)
		{
		}

		// Token: 0x06004C89 RID: 19593 RVA: 0x0022C08F File Offset: 0x0022A28F
		private static LocString L10NLookup(string key)
		{
			return new LocString("encounters", key);
		}

		/// <summary>
		/// Called when a creature is spawned during combat.
		/// If it's an enemy, it's added to the SpawnedEnemies list.
		/// </summary>
		// Token: 0x06004C8A RID: 19594 RVA: 0x0022C09C File Offset: 0x0022A29C
		public void OnCreatureSpawned(Creature creature)
		{
			base.AssertMutable();
			if (creature.Side != CombatSide.Enemy)
			{
				return;
			}
			MonsterModel monster = creature.Monster;
			MonsterModel monsterModel = ((monster != null) ? monster.CanonicalInstance : null);
			if (monsterModel == null)
			{
				return;
			}
			if (this._spawnedEnemies != null && this._spawnedEnemies.Contains(monsterModel))
			{
				return;
			}
			if (this._spawnedEnemies == null)
			{
				this._spawnedEnemies = new List<MonsterModel>();
			}
			this._spawnedEnemies.Add(monsterModel);
		}

		// Token: 0x04001D30 RID: 7472
		private const string _locTable = "encounters";

		// Token: 0x04001D31 RID: 7473
		[Nullable(2)]
		private Rng _rng;

		// Token: 0x04001D32 RID: 7474
		[Nullable(new byte[] { 2, 0, 1, 2 })]
		private IReadOnlyList<ValueTuple<MonsterModel, string>> _monstersWithSlots;

		// Token: 0x04001D33 RID: 7475
		[Nullable(new byte[] { 2, 1 })]
		private List<MonsterModel> _spawnedEnemies;

		// Token: 0x04001D34 RID: 7476
		private EncounterModel _canonicalInstance;

		// Token: 0x04001D35 RID: 7477
		[Nullable(2)]
		private BackgroundAssets _backgroundAssets;
	}
}
