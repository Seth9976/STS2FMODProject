using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Models.Modifiers;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace MegaCrit.Sts2.Core.Models
{
	// Token: 0x020004E4 RID: 1252
	[NullableContext(1)]
	[Nullable(0)]
	public static class ModelDb
	{
		/// <summary>
		/// Returns every single class that subclasses AbstractModel.
		/// Prefer this over using AbstractModelSubtypes.All because it also returns models in mods.
		/// </summary>
		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06004CDC RID: 19676 RVA: 0x0022CC40 File Offset: 0x0022AE40
		public static Type[] AllAbstractModelSubtypes
		{
			get
			{
				if (ModelDb._allAbstractModelSubtypes != null)
				{
					return ModelDb._allAbstractModelSubtypes;
				}
				HashSet<Type> hashSet = new HashSet<Type>();
				foreach (Type type in AbstractModelSubtypes.All)
				{
					hashSet.Add(type);
				}
				foreach (Type type in ReflectionHelper.GetSubtypesInMods<AbstractModel>())
				{
					hashSet.Add(type);
				}
				ModelDb._allAbstractModelSubtypes = hashSet.ToArray<Type>();
				return ModelDb._allAbstractModelSubtypes;
			}
		}

		/// <summary>
		/// Returns every model registered to the database.
		/// This should be used very sparingly.
		/// </summary>
		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06004CDD RID: 19677 RVA: 0x0022CCEC File Offset: 0x0022AEEC
		public static IEnumerable<AbstractModel> All
		{
			get
			{
				return ModelDb._contentById.Values;
			}
		}

		/// <summary>
		/// Initializes the ModelDb.
		/// Note that the alternative is to initialize in a static initializer, but if we do this we don't control the time
		/// at which it gets initialized, and it usually happens at a bad time during gameplay.
		/// </summary>
		// Token: 0x06004CDE RID: 19678 RVA: 0x0022CCF8 File Offset: 0x0022AEF8
		public static void Init([Nullable(new byte[] { 2, 1 })] Type[] injectedModelTypes = null)
		{
			foreach (Type type in injectedModelTypes ?? ModelDb.AllAbstractModelSubtypes)
			{
				ModelId id = ModelDb.GetId(type);
				AbstractModel abstractModel = (AbstractModel)Activator.CreateInstance(type);
				ModelDb._contentById[id] = abstractModel;
			}
		}

		/// <summary>
		/// Injects a model into the ModelDb. Should only be used in tests and mods.
		/// </summary>
		/// <param name="type">The type to inject.</param>
		// Token: 0x06004CDF RID: 19679 RVA: 0x0022CD44 File Offset: 0x0022AF44
		public static void Inject([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type)
		{
			if (ModelDb.Contains(type))
			{
				return;
			}
			ModelId id = ModelDb.GetId(type);
			AbstractModel abstractModel = (AbstractModel)Activator.CreateInstance(type);
			ModelDb._contentById[id] = abstractModel;
		}

		/// <summary>
		/// Removes a model from the ModelDb. Should only be used in tests and mods.
		/// </summary>
		/// <param name="type">The type to remove.</param>
		// Token: 0x06004CE0 RID: 19680 RVA: 0x0022CD7C File Offset: 0x0022AF7C
		public static void Remove(Type type)
		{
			ModelId id = ModelDb.GetId(type);
			ModelDb._contentById.Remove(id);
		}

		/// <summary>
		/// Only called in tests.
		/// Remove all content from the DB.
		/// </summary>
		// Token: 0x06004CE1 RID: 19681 RVA: 0x0022CD9C File Offset: 0x0022AF9C
		public static void ResetForTest()
		{
			ModelDb._allAbstractModelSubtypes = null;
			ModelDb._contentById.Clear();
		}

		/// <summary>
		/// Assigns IDs to all canonical models in the model database.
		/// This must happen after Init (i.e. the AbstractModel constructors); otherwise, there is a circular dependency
		/// between the static constructor and the ModelIdSerializationCache.
		/// </summary>
		// Token: 0x06004CE2 RID: 19682 RVA: 0x0022CDB0 File Offset: 0x0022AFB0
		public static void InitIds()
		{
			foreach (KeyValuePair<ModelId, AbstractModel> keyValuePair in ModelDb._contentById)
			{
				keyValuePair.Value.InitId(keyValuePair.Key);
			}
		}

		/// <summary>
		/// Precomputes a bunch of model data to speed up operations later.
		/// </summary>
		// Token: 0x06004CE3 RID: 19683 RVA: 0x0022CE10 File Offset: 0x0022B010
		public static void Preload()
		{
			IEnumerable<CardModel> allCards = ModelDb.AllCards;
			IEnumerable<CardPoolModel> allCharacterCardPools = ModelDb.AllCharacterCardPools;
			IEnumerable<EventModel> allSharedEvents = ModelDb.AllSharedEvents;
			IEnumerable<EventModel> allEvents = ModelDb.AllEvents;
			IEnumerable<RelicModel> allRelics = ModelDb.AllRelics;
			IEnumerable<PotionModel> allPotions = ModelDb.AllPotions;
			IEnumerable<EncounterModel> allEncounters = ModelDb.AllEncounters;
			IReadOnlyList<AchievementModel> achievements = ModelDb.Achievements;
			foreach (CardModel cardModel in ModelDb.AllCards)
			{
				CardPoolModel pool = cardModel.Pool;
				IEnumerable<string> allPortraitPaths = cardModel.AllPortraitPaths;
			}
			foreach (RelicModel relicModel in ModelDb.AllRelics)
			{
				string iconPath = relicModel.IconPath;
			}
			foreach (PowerModel powerModel in ModelDb.AllPowers)
			{
				string iconPath2 = powerModel.IconPath;
				string resolvedBigIconPath = powerModel.ResolvedBigIconPath;
			}
		}

		// Token: 0x06004CE4 RID: 19684 RVA: 0x0022CF18 File Offset: 0x0022B118
		public static ModelId GetId<[Nullable(0)] T>() where T : AbstractModel
		{
			return ModelDb.GetId(typeof(T));
		}

		// Token: 0x06004CE5 RID: 19685 RVA: 0x0022CF29 File Offset: 0x0022B129
		public static ModelId GetId(Type type)
		{
			return new ModelId(ModelDb.GetCategory(type), ModelDb.GetEntry(type));
		}

		// Token: 0x06004CE6 RID: 19686 RVA: 0x0022CF3C File Offset: 0x0022B13C
		public static Type GetCategoryType(Type type)
		{
			Type type2 = type;
			while (type2.BaseType != typeof(AbstractModel))
			{
				type2 = type2.BaseType;
			}
			return type2;
		}

		// Token: 0x06004CE7 RID: 19687 RVA: 0x0022CF6C File Offset: 0x0022B16C
		public static string GetCategory(Type type)
		{
			return ModelId.SlugifyCategory(ModelDb.GetCategoryType(type).Name);
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x0022CF7E File Offset: 0x0022B17E
		public static string GetEntry(Type type)
		{
			return StringHelper.Slugify(type.Name);
		}

		// Token: 0x06004CE9 RID: 19689 RVA: 0x0022CF8C File Offset: 0x0022B18C
		[return: Nullable(2)]
		public static T GetByIdOrNull<[Nullable(0)] T>(ModelId id) where T : AbstractModel
		{
			AbstractModel abstractModel;
			if (ModelDb._contentById.TryGetValue(id, out abstractModel))
			{
				return (T)((object)abstractModel);
			}
			return default(T);
		}

		// Token: 0x06004CEA RID: 19690 RVA: 0x0022CFB8 File Offset: 0x0022B1B8
		public static T GetById<[Nullable(0)] T>(ModelId id) where T : AbstractModel
		{
			T byIdOrNull = ModelDb.GetByIdOrNull<T>(id);
			T t = byIdOrNull;
			if (t == null)
			{
				throw new ModelNotFoundException(id);
			}
			return t;
		}

		// Token: 0x06004CEB RID: 19691 RVA: 0x0022CFDD File Offset: 0x0022B1DD
		public static bool Contains(Type type)
		{
			return ModelDb._contentById.ContainsKey(ModelDb.GetId(type));
		}

		// Token: 0x06004CEC RID: 19692 RVA: 0x0022CFEF File Offset: 0x0022B1EF
		private static T Get<[Nullable(0)] T>() where T : AbstractModel
		{
			return (T)((object)ModelDb._contentById[ModelDb.GetId<T>()]);
		}

		// Token: 0x06004CED RID: 19693 RVA: 0x0022D008 File Offset: 0x0022B208
		private static AbstractModel Get(Type type)
		{
			if (!type.IsSubclassOf(typeof(AbstractModel)))
			{
				throw new InvalidOperationException();
			}
			ModelId id = ModelDb.GetId(type);
			AbstractModel abstractModel;
			if (ModelDb._contentById.TryGetValue(id, out abstractModel))
			{
				return abstractModel;
			}
			throw new ModelNotFoundException(id);
		}

		// Token: 0x06004CEE RID: 19694 RVA: 0x0022D04B File Offset: 0x0022B24B
		public static T Affliction<[Nullable(0)] T>() where T : AfflictionModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Every Affliction defined in the game code, including mock ones for testing.
		/// </summary>
		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06004CEF RID: 19695 RVA: 0x0022D054 File Offset: 0x0022B254
		public static IEnumerable<AfflictionModel> DebugAfflictions
		{
			get
			{
				return from t in ModelDb.AllAbstractModelSubtypes
					where t.IsSubclassOf(typeof(AfflictionModel))
					select (AfflictionModel)ModelDb.Get(t);
			}
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x0022D0AE File Offset: 0x0022B2AE
		public static T Enchantment<[Nullable(0)] T>() where T : EnchantmentModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Every Enchantment defined in the game code, including mock ones for testing.
		/// </summary>
		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06004CF1 RID: 19697 RVA: 0x0022D0B8 File Offset: 0x0022B2B8
		public static IEnumerable<EnchantmentModel> DebugEnchantments
		{
			get
			{
				return from t in ModelDb.AllAbstractModelSubtypes
					where t.IsSubclassOf(typeof(EnchantmentModel))
					select (EnchantmentModel)ModelDb.Get(t);
			}
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x0022D112 File Offset: 0x0022B312
		public static T Card<[Nullable(0)] T>() where T : CardModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Get all the cards in the game (ignores Unlocks/Epoch state).
		/// Be careful using this, it includes cards that you shouldn't be able to randomly roll for rewards.
		/// </summary>
		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06004CF3 RID: 19699 RVA: 0x0022D11C File Offset: 0x0022B31C
		public static IEnumerable<CardModel> AllCards
		{
			get
			{
				IEnumerable<CardModel> enumerable;
				if ((enumerable = ModelDb._allCards) == null)
				{
					IEnumerable<CardModel> enumerable2 = (ModelDb._allCards = ModelDb.AllCardPools.SelectMany<CardPoolModel, CardModel>((CardPoolModel p) => p.AllCards).Concat<CardModel>(ModelDb.AllCharacters.SelectMany<CharacterModel, CardModel>((CharacterModel c) => c.StartingDeck).Distinct<CardModel>()).Distinct<CardModel>());
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x0022D19B File Offset: 0x0022B39B
		public static T CardPool<[Nullable(0)] T>() where T : CardPoolModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Get all the card pools in the game.
		/// Ignores Unlocks/Epoch state.
		/// </summary>
		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06004CF5 RID: 19701 RVA: 0x0022D1A4 File Offset: 0x0022B3A4
		public static IEnumerable<CardPoolModel> AllCardPools
		{
			get
			{
				IEnumerable<CardPoolModel> enumerable;
				if ((enumerable = ModelDb._allCardPools) == null)
				{
					IEnumerable<CardPoolModel> enumerable2 = (ModelDb._allCardPools = ModelDb.AllCharacterCardPools.Concat<CardPoolModel>(ModelDb.AllSharedCardPools).Distinct<CardPoolModel>());
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		/// <summary>
		/// The card pools that are shared across all characters.
		/// WARNING: Do NOT add TestCardPool to this list, or test cards might accidentally appear in-game.
		/// </summary>
		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06004CF6 RID: 19702 RVA: 0x0022D1D8 File Offset: 0x0022B3D8
		public static IEnumerable<CardPoolModel> AllSharedCardPools
		{
			get
			{
				return new <>z__ReadOnlyArray<CardPoolModel>(new CardPoolModel[]
				{
					ModelDb.CardPool<ColorlessCardPool>(),
					ModelDb.CardPool<CurseCardPool>(),
					ModelDb.CardPool<DeprecatedCardPool>(),
					ModelDb.CardPool<EventCardPool>(),
					ModelDb.CardPool<QuestCardPool>(),
					ModelDb.CardPool<StatusCardPool>(),
					ModelDb.CardPool<TokenCardPool>()
				});
			}
		}

		/// <summary>
		/// Get all the card pools in the game that belong to specific characters.
		/// </summary>
		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06004CF7 RID: 19703 RVA: 0x0022D228 File Offset: 0x0022B428
		public static IEnumerable<CardPoolModel> AllCharacterCardPools
		{
			get
			{
				IEnumerable<CardPoolModel> enumerable;
				if ((enumerable = ModelDb._allCharacterCardPools) == null)
				{
					IEnumerable<CardPoolModel> enumerable2 = (ModelDb._allCharacterCardPools = ModelDb.AllCharacters.Select<CharacterModel, CardPoolModel>((CharacterModel c) => c.CardPool));
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x0022D26F File Offset: 0x0022B46F
		public static T Character<[Nullable(0)] T>() where T : CharacterModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Get all the characters in the game (ignores Unlocks/Epoch state).
		/// </summary>
		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06004CF9 RID: 19705 RVA: 0x0022D276 File Offset: 0x0022B476
		public static IEnumerable<CharacterModel> AllCharacters
		{
			get
			{
				return new <>z__ReadOnlyArray<CharacterModel>(new CharacterModel[]
				{
					ModelDb.Character<Ironclad>(),
					ModelDb.Character<Silent>(),
					ModelDb.Character<Regent>(),
					ModelDb.Character<Necrobinder>(),
					ModelDb.Character<Defect>()
				});
			}
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x0022D2AB File Offset: 0x0022B4AB
		public static T Event<[Nullable(0)] T>() where T : EventModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Get all the events in the game that don't belong to a specific act.
		/// </summary>
		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06004CFB RID: 19707 RVA: 0x0022D2B4 File Offset: 0x0022B4B4
		public static IEnumerable<EventModel> AllSharedEvents
		{
			get
			{
				IEnumerable<EventModel> enumerable;
				if ((enumerable = ModelDb._allSharedEvents) == null)
				{
					enumerable = (ModelDb._allSharedEvents = new <>z__ReadOnlyArray<EventModel>(new EventModel[]
					{
						ModelDb.Event<BrainLeech>(),
						ModelDb.Event<CrystalSphere>(),
						ModelDb.Event<DollRoom>(),
						ModelDb.Event<FakeMerchant>(),
						ModelDb.Event<PotionCourier>(),
						ModelDb.Event<RanwidTheElder>(),
						ModelDb.Event<RelicTrader>(),
						ModelDb.Event<RoomFullOfCheese>(),
						ModelDb.Event<SelfHelpBook>(),
						ModelDb.Event<SlipperyBridge>(),
						ModelDb.Event<StoneOfAllTime>(),
						ModelDb.Event<Symbiote>(),
						ModelDb.Event<TeaMaster>(),
						ModelDb.Event<TheFutureOfPotions>(),
						ModelDb.Event<TheLegendsWereTrue>(),
						ModelDb.Event<ThisOrThat>(),
						ModelDb.Event<WarHistorianRepy>(),
						ModelDb.Event<WelcomeToWongos>()
					}));
				}
				return enumerable;
			}
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x0022D378 File Offset: 0x0022B578
		public static T AncientEvent<[Nullable(0)] T>() where T : AncientEventModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Get all the ancients in the game.
		/// Ignores Unlocks/Epoch state.
		/// </summary>
		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06004CFD RID: 19709 RVA: 0x0022D37F File Offset: 0x0022B57F
		public static IEnumerable<AncientEventModel> AllAncients
		{
			get
			{
				return ModelDb.Acts.SelectMany<ActModel, AncientEventModel>((ActModel a) => a.AllAncients).Concat<AncientEventModel>(ModelDb.AllSharedAncients).Distinct<AncientEventModel>();
			}
		}

		/// <summary>
		/// Get all the ancients in the game that don't belong to a specific act.
		/// Notably, we only have 1 right now. That's Darv.
		/// </summary>
		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06004CFE RID: 19710 RVA: 0x0022D3B9 File Offset: 0x0022B5B9
		public static IEnumerable<AncientEventModel> AllSharedAncients
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<AncientEventModel>(ModelDb.AncientEvent<Darv>());
			}
		}

		/// <summary>
		/// Get all the events in the game (ignores Unlocks/Epoch state).
		/// </summary>
		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06004CFF RID: 19711 RVA: 0x0022D3C8 File Offset: 0x0022B5C8
		public static IEnumerable<EventModel> AllEvents
		{
			get
			{
				IEnumerable<EventModel> enumerable;
				if ((enumerable = ModelDb._allEvents) == null)
				{
					IEnumerable<EventModel> enumerable2 = (ModelDb._allEvents = ModelDb.Acts.SelectMany<ActModel, EventModel>((ActModel a) => a.AllEvents).Concat<EventModel>(ModelDb.AllSharedEvents).Distinct<EventModel>());
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x0022D41E File Offset: 0x0022B61E
		public static T Monster<[Nullable(0)] T>() where T : MonsterModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Returns a list of every possible Monster in the game.
		/// </summary>
		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06004D01 RID: 19713 RVA: 0x0022D428 File Offset: 0x0022B628
		public static IEnumerable<MonsterModel> Monsters
		{
			get
			{
				return ModelDb.Acts.SelectMany<ActModel, MonsterModel>((ActModel act) => act.AllMonsters).Concat<MonsterModel>(ModelDb.EventEncounters.SelectMany<EncounterModel, MonsterModel>((EncounterModel e) => e.AllPossibleMonsters)).Distinct<MonsterModel>();
			}
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x0022D491 File Offset: 0x0022B691
		public static T Encounter<[Nullable(0)] T>() where T : EncounterModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Get all the encounters in the game.
		/// </summary>
		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06004D03 RID: 19715 RVA: 0x0022D498 File Offset: 0x0022B698
		public static IEnumerable<EncounterModel> AllEncounters
		{
			get
			{
				IEnumerable<EncounterModel> enumerable;
				if ((enumerable = ModelDb._allEncounters) == null)
				{
					IEnumerable<EncounterModel> enumerable2 = (ModelDb._allEncounters = ModelDb.Acts.SelectMany<ActModel, EncounterModel>((ActModel a) => a.AllEncounters).Concat<EncounterModel>(ModelDb.EventEncounters).Distinct<EncounterModel>());
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		/// <summary>
		/// There's not a good place to source these encounters, so we store it here
		/// </summary>
		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06004D04 RID: 19716 RVA: 0x0022D4F0 File Offset: 0x0022B6F0
		public static IEnumerable<EncounterModel> EventEncounters
		{
			get
			{
				IEnumerable<EncounterModel> enumerable;
				if ((enumerable = ModelDb._eventEncounters) == null)
				{
					enumerable = (ModelDb._eventEncounters = new <>z__ReadOnlyArray<EncounterModel>(new EncounterModel[]
					{
						ModelDb.Encounter<FakeMerchantEventEncounter>(),
						ModelDb.Encounter<MysteriousKnightEventEncounter>(),
						ModelDb.Encounter<BattlewornDummyEventV1Encounter>(),
						ModelDb.Encounter<BattlewornDummyEventV2Encounter>(),
						ModelDb.Encounter<BattlewornDummyEventV3Encounter>()
					}));
				}
				return enumerable;
			}
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x0022D53F File Offset: 0x0022B73F
		public static T Potion<[Nullable(0)] T>() where T : PotionModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Get all the potions in the game.
		/// Be careful using this, it includes potions that you shouldn't be able to randomly roll for rewards.
		/// </summary>
		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06004D06 RID: 19718 RVA: 0x0022D548 File Offset: 0x0022B748
		public static IEnumerable<PotionModel> AllPotions
		{
			get
			{
				IEnumerable<PotionModel> enumerable;
				if ((enumerable = ModelDb._allPotions) == null)
				{
					enumerable = (ModelDb._allPotions = from p in ModelDb.AllPotionPools.SelectMany<PotionPoolModel, PotionModel>((PotionPoolModel p) => p.AllPotions).Distinct<PotionModel>()
						orderby p.Id.Entry
						select p);
				}
				return enumerable;
			}
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x0022D5B6 File Offset: 0x0022B7B6
		public static T PotionPool<[Nullable(0)] T>() where T : PotionPoolModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Get all the potion pools in the game.
		/// </summary>
		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06004D08 RID: 19720 RVA: 0x0022D5C0 File Offset: 0x0022B7C0
		public static IEnumerable<PotionPoolModel> AllPotionPools
		{
			get
			{
				IEnumerable<PotionPoolModel> enumerable;
				if ((enumerable = ModelDb._allPotionPools) == null)
				{
					IEnumerable<PotionPoolModel> enumerable2 = (ModelDb._allPotionPools = ModelDb.AllCharacterPotionPools.Concat<PotionPoolModel>(ModelDb.AllSharedPotionPools).Distinct<PotionPoolModel>());
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		/// <summary>
		/// Get all the potion pools in the game that belong to specific characters.
		/// </summary>
		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06004D09 RID: 19721 RVA: 0x0022D5F4 File Offset: 0x0022B7F4
		public static IEnumerable<PotionPoolModel> AllCharacterPotionPools
		{
			get
			{
				IEnumerable<PotionPoolModel> enumerable;
				if ((enumerable = ModelDb._allCharacterPotionPools) == null)
				{
					IEnumerable<PotionPoolModel> enumerable2 = (ModelDb._allCharacterPotionPools = ModelDb.AllCharacters.Select<CharacterModel, PotionPoolModel>((CharacterModel c) => c.PotionPool));
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		/// <summary>
		/// Get all the relic pools in the game that belong to specific characters.
		/// </summary>
		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06004D0A RID: 19722 RVA: 0x0022D63C File Offset: 0x0022B83C
		public static IEnumerable<RelicPoolModel> AllCharacterRelicPools
		{
			get
			{
				IEnumerable<RelicPoolModel> enumerable;
				if ((enumerable = ModelDb._allCharacterRelicPools) == null)
				{
					IEnumerable<RelicPoolModel> enumerable2 = (ModelDb._allCharacterRelicPools = ModelDb.AllCharacters.Select<CharacterModel, RelicPoolModel>((CharacterModel c) => c.RelicPool));
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		/// <summary>
		/// Get all the potion pools in the game that are shared between all characters.
		/// </summary>
		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06004D0B RID: 19723 RVA: 0x0022D684 File Offset: 0x0022B884
		private static IEnumerable<PotionPoolModel> AllSharedPotionPools
		{
			get
			{
				IEnumerable<PotionPoolModel> enumerable;
				if ((enumerable = ModelDb._allSharedPotionPools) == null)
				{
					enumerable = (ModelDb._allSharedPotionPools = new <>z__ReadOnlyArray<PotionPoolModel>(new PotionPoolModel[]
					{
						ModelDb.PotionPool<DeprecatedPotionPool>(),
						ModelDb.PotionPool<EventPotionPool>(),
						ModelDb.PotionPool<MockPotionPool>(),
						ModelDb.PotionPool<SharedPotionPool>(),
						ModelDb.PotionPool<TokenPotionPool>()
					}));
				}
				return enumerable;
			}
		}

		// Token: 0x06004D0C RID: 19724 RVA: 0x0022D6D3 File Offset: 0x0022B8D3
		public static T Power<[Nullable(0)] T>() where T : PowerModel
		{
			return ModelDb.Get<T>();
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06004D0D RID: 19725 RVA: 0x0022D6DC File Offset: 0x0022B8DC
		public static IEnumerable<PowerModel> AllPowers
		{
			get
			{
				IEnumerable<PowerModel> enumerable;
				if ((enumerable = ModelDb._allPowers) == null)
				{
					IEnumerable<PowerModel> enumerable2 = (ModelDb._allPowers = from t in ModelDb.AllAbstractModelSubtypes
						where t.IsSubclassOf(typeof(PowerModel))
						select (PowerModel)ModelDb.Get(t));
					enumerable = enumerable2;
				}
				return enumerable;
			}
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x0022D747 File Offset: 0x0022B947
		public static PowerModel DebugPower(Type type)
		{
			return (PowerModel)ModelDb.Get(type);
		}

		// Token: 0x06004D0F RID: 19727 RVA: 0x0022D754 File Offset: 0x0022B954
		public static T Relic<[Nullable(0)] T>() where T : RelicModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Get all the relics in the game (ignores Unlocks/Epoch state).
		/// Be careful using this, it includes relics that you shouldn't be able to randomly roll for rewards.
		/// </summary>
		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06004D10 RID: 19728 RVA: 0x0022D75C File Offset: 0x0022B95C
		public static IEnumerable<RelicModel> AllRelics
		{
			get
			{
				IEnumerable<RelicModel> enumerable;
				if ((enumerable = ModelDb._allRelics) == null)
				{
					enumerable = (ModelDb._allRelics = from r in ModelDb.AllRelicPools.SelectMany<RelicPoolModel, RelicModel>((RelicPoolModel p) => p.AllRelics).Concat<RelicModel>(ModelDb.AllCharacters.SelectMany<CharacterModel, RelicModel>((CharacterModel c) => c.StartingRelics)).Distinct<RelicModel>()
						orderby r.Id.Entry
						select r);
				}
				return enumerable;
			}
		}

		// Token: 0x06004D11 RID: 19729 RVA: 0x0022D7FB File Offset: 0x0022B9FB
		public static T RelicPool<[Nullable(0)] T>() where T : RelicPoolModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// Get all the relic pools in the game.
		/// </summary>
		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06004D12 RID: 19730 RVA: 0x0022D802 File Offset: 0x0022BA02
		public static IEnumerable<RelicPoolModel> AllRelicPools
		{
			get
			{
				return ModelDb.CharacterRelicPools.Concat<RelicPoolModel>(ModelDb.AllSharedRelicPools).Distinct<RelicPoolModel>();
			}
		}

		/// <summary>
		/// Get all the relic pools in the game that belong to specific characters.
		/// </summary>
		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06004D13 RID: 19731 RVA: 0x0022D818 File Offset: 0x0022BA18
		public static IEnumerable<RelicPoolModel> CharacterRelicPools
		{
			get
			{
				return ModelDb.AllCharacters.Select<CharacterModel, RelicPoolModel>((CharacterModel c) => c.RelicPool);
			}
		}

		/// <summary>
		/// Get all the relic pools in the game that are shared between all characters.
		/// </summary>
		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06004D14 RID: 19732 RVA: 0x0022D843 File Offset: 0x0022BA43
		private static IEnumerable<RelicPoolModel> AllSharedRelicPools
		{
			get
			{
				return new <>z__ReadOnlyArray<RelicPoolModel>(new RelicPoolModel[]
				{
					ModelDb.RelicPool<DeprecatedRelicPool>(),
					ModelDb.RelicPool<EventRelicPool>(),
					ModelDb.RelicPool<FallbackRelicPool>(),
					ModelDb.RelicPool<SharedRelicPool>()
				});
			}
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x0022D870 File Offset: 0x0022BA70
		public static T Orb<[Nullable(0)] T>() where T : OrbModel
		{
			return ModelDb.Get<T>();
		}

		// Token: 0x06004D16 RID: 19734 RVA: 0x0022D878 File Offset: 0x0022BA78
		[return: Nullable(2)]
		public static OrbModel DebugOrb(Type type)
		{
			OrbModel orbModel;
			try
			{
				orbModel = (OrbModel)ModelDb.Get(type);
			}
			catch
			{
				orbModel = null;
			}
			return orbModel;
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004D17 RID: 19735 RVA: 0x0022D8AC File Offset: 0x0022BAAC
		public static IEnumerable<OrbModel> Orbs
		{
			get
			{
				return new <>z__ReadOnlyArray<OrbModel>(new OrbModel[]
				{
					ModelDb.Orb<LightningOrb>(),
					ModelDb.Orb<FrostOrb>(),
					ModelDb.Orb<DarkOrb>(),
					ModelDb.Orb<PlasmaOrb>()
				});
			}
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x0022D8D9 File Offset: 0x0022BAD9
		public static T Act<[Nullable(0)] T>() where T : ActModel
		{
			return ModelDb.Get<T>();
		}

		/// <summary>
		/// All acts in the game.
		/// Note that callers depend on this list being sorted by act index, then by default/non-default.
		/// If you need a different ordering, feel free to change this or split into another list somewhere.
		/// </summary>
		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06004D19 RID: 19737 RVA: 0x0022D8E0 File Offset: 0x0022BAE0
		public unsafe static IEnumerable<ActModel> Acts
		{
			get
			{
				if (ModelDb._acts == null)
				{
					int num = 4;
					List<ActModel> list = new List<ActModel>(num);
					CollectionsMarshal.SetCount<ActModel>(list, num);
					Span<ActModel> span = CollectionsMarshal.AsSpan<ActModel>(list);
					int num2 = 0;
					*span[num2] = ModelDb.Act<Overgrowth>();
					num2++;
					*span[num2] = ModelDb.Act<Underdocks>();
					num2++;
					*span[num2] = ModelDb.Act<Hive>();
					num2++;
					*span[num2] = ModelDb.Act<Glory>();
					ModelDb._acts = list;
				}
				return ModelDb._acts;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06004D1A RID: 19738 RVA: 0x0022D95C File Offset: 0x0022BB5C
		public static IReadOnlyList<IReadOnlyList<ActModel>> ActsByIndex
		{
			get
			{
				if (ModelDb._actsByIndex != null)
				{
					return ModelDb._actsByIndex;
				}
				ModelDb._actsByIndex = new List<List<ActModel>>();
				foreach (ActModel actModel in ModelDb.Acts)
				{
					if (actModel.Index >= 0)
					{
						for (int i = ModelDb._actsByIndex.Count; i <= actModel.Index; i++)
						{
							ModelDb._actsByIndex.Add(new List<ActModel>());
						}
						ModelDb._actsByIndex[actModel.Index].Add(actModel);
					}
				}
				return ModelDb._actsByIndex;
			}
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x0022DA08 File Offset: 0x0022BC08
		public static T Singleton<[Nullable(0)] T>() where T : SingletonModel
		{
			return ModelDb.Get<T>();
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x0022DA0F File Offset: 0x0022BC0F
		public static T Badge<[Nullable(0)] T>() where T : BadgeModel
		{
			return ModelDb.Get<T>();
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004D1D RID: 19741 RVA: 0x0022DA18 File Offset: 0x0022BC18
		public static IReadOnlyList<BadgeModel> BadgeModels
		{
			get
			{
				if (ModelDb._badges == null)
				{
					ModelDb._badges = new List<BadgeModel>();
					foreach (Type type in ModelDb.AllAbstractModelSubtypes)
					{
						if (type.IsSubclassOf(typeof(BadgeModel)))
						{
							ModelDb._badges.Add((BadgeModel)ModelDb.Get(type));
						}
					}
				}
				return ModelDb._badges;
			}
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x0022DA7A File Offset: 0x0022BC7A
		public static T Achievement<[Nullable(0)] T>() where T : AchievementModel
		{
			return ModelDb.Get<T>();
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06004D1F RID: 19743 RVA: 0x0022DA84 File Offset: 0x0022BC84
		public static IReadOnlyList<AchievementModel> Achievements
		{
			get
			{
				if (ModelDb._achievements == null)
				{
					ModelDb._achievements = new List<AchievementModel>();
					foreach (Type type in ModelDb.AllAbstractModelSubtypes)
					{
						if (type.IsSubclassOf(typeof(AchievementModel)))
						{
							ModelDb._achievements.Add((AchievementModel)ModelDb.Get(type));
						}
					}
				}
				return ModelDb._achievements;
			}
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x0022DAE6 File Offset: 0x0022BCE6
		public static T Modifier<[Nullable(0)] T>() where T : ModifierModel
		{
			return ModelDb.Get<T>();
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004D21 RID: 19745 RVA: 0x0022DAF0 File Offset: 0x0022BCF0
		public static IReadOnlyList<ModifierModel> GoodModifiers
		{
			get
			{
				return new <>z__ReadOnlyArray<ModifierModel>(new ModifierModel[]
				{
					ModelDb.Modifier<Draft>(),
					ModelDb.Modifier<SealedDeck>(),
					ModelDb.Modifier<Hoarder>(),
					ModelDb.Modifier<Specialized>(),
					ModelDb.Modifier<Insanity>(),
					ModelDb.Modifier<AllStar>(),
					ModelDb.Modifier<Flight>(),
					ModelDb.Modifier<Vintage>(),
					ModelDb.Modifier<CharacterCards>()
				});
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004D22 RID: 19746 RVA: 0x0022DB54 File Offset: 0x0022BD54
		public static IReadOnlyList<ModifierModel> BadModifiers
		{
			get
			{
				return new <>z__ReadOnlyArray<ModifierModel>(new ModifierModel[]
				{
					ModelDb.Modifier<DeadlyEvents>(),
					ModelDb.Modifier<CursedRun>(),
					ModelDb.Modifier<BigGameHunter>(),
					ModelDb.Modifier<Midas>(),
					ModelDb.Modifier<Murderous>(),
					ModelDb.Modifier<NightTerrors>(),
					ModelDb.Modifier<Terminal>()
				});
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06004D23 RID: 19747 RVA: 0x0022DBA4 File Offset: 0x0022BDA4
		public static IReadOnlyList<IReadOnlySet<ModifierModel>> MutuallyExclusiveModifiers
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IReadOnlySet<ModifierModel>>(new HashSet<ModifierModel>
				{
					ModelDb.Modifier<SealedDeck>(),
					ModelDb.Modifier<Draft>(),
					ModelDb.Modifier<Insanity>()
				});
			}
		}

		// Token: 0x04001D43 RID: 7491
		private const int _initialCapacity = 4096;

		// Token: 0x04001D44 RID: 7492
		private static readonly Dictionary<ModelId, AbstractModel> _contentById = new Dictionary<ModelId, AbstractModel>(4096);

		// Token: 0x04001D45 RID: 7493
		[Nullable(new byte[] { 2, 1 })]
		private static Type[] _allAbstractModelSubtypes;

		// Token: 0x04001D46 RID: 7494
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<CardModel> _allCards;

		// Token: 0x04001D47 RID: 7495
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<CardPoolModel> _allCardPools;

		// Token: 0x04001D48 RID: 7496
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<CardPoolModel> _allCharacterCardPools;

		// Token: 0x04001D49 RID: 7497
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<EventModel> _allSharedEvents;

		// Token: 0x04001D4A RID: 7498
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<EventModel> _allEvents;

		// Token: 0x04001D4B RID: 7499
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<EncounterModel> _allEncounters;

		// Token: 0x04001D4C RID: 7500
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<EncounterModel> _eventEncounters;

		// Token: 0x04001D4D RID: 7501
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<PotionModel> _allPotions;

		// Token: 0x04001D4E RID: 7502
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<PotionPoolModel> _allPotionPools;

		// Token: 0x04001D4F RID: 7503
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<PotionPoolModel> _allCharacterPotionPools;

		// Token: 0x04001D50 RID: 7504
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<RelicPoolModel> _allCharacterRelicPools;

		// Token: 0x04001D51 RID: 7505
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<PotionPoolModel> _allSharedPotionPools;

		// Token: 0x04001D52 RID: 7506
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<PowerModel> _allPowers;

		// Token: 0x04001D53 RID: 7507
		[Nullable(new byte[] { 2, 1 })]
		private static IEnumerable<RelicModel> _allRelics;

		// Token: 0x04001D54 RID: 7508
		[Nullable(new byte[] { 2, 1 })]
		private static List<ActModel> _acts;

		// Token: 0x04001D55 RID: 7509
		[Nullable(new byte[] { 2, 1, 1 })]
		private static List<List<ActModel>> _actsByIndex;

		// Token: 0x04001D56 RID: 7510
		[Nullable(new byte[] { 2, 1 })]
		private static List<BadgeModel> _badges;

		// Token: 0x04001D57 RID: 7511
		[Nullable(new byte[] { 2, 1 })]
		private static List<AchievementModel> _achievements;
	}
}
