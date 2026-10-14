using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MegaCrit.Sts2.Core.Unlocks
{
	/// <summary>
	/// Represents the state of unlocked content for a given player.
	/// The local player can encapsulate this state in their progress save. The real place this class is useful is in
	/// multiplayer scenarios, where different players have different unlock states.
	///
	/// This is used in three places:
	///  - In the <see cref="T:MegaCrit.Sts2.Core.Entities.Players.Player" /> class, to query a specific player's unlock state (e.g. for rewards or Attack potion generation)
	///  - In the <see cref="T:MegaCrit.Sts2.Core.Runs.RunState" /> class, to query the superset of all players' unlock states (e.g. for ancient generation)
	///  - In the <see cref="T:MegaCrit.Sts2.Core.Saves.SaveManager" /> class, to query the local player's unlock state outside of a run (e.g. in the compendium)
	///
	/// For multiplayer, keep in mind which one is appropriate to use at a given time. Using the third when one of the first
	/// two should be used will result in a state divergence.
	///
	/// In singleplayer, the first two are equivalent, but you should still consider when using the third is appropriate.
	/// If you use the third during a run, epochs unlocked between a save and a load may be inappropriately included in the
	/// run.
	/// </summary>
	// Token: 0x02000035 RID: 53
	[NullableContext(1)]
	[Nullable(0)]
	public class UnlockState
	{
		/// <summary>
		/// Ths number of runs the player has done.
		/// If this is the shared unlock set, this is the **maximum** number of runs among all players.
		/// </summary>
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600021F RID: 543 RVA: 0x000114A7 File Offset: 0x0000F6A7
		public int NumberOfRuns { get; }

		/// <summary>
		/// Returns the all the characters that the player has unlocked.
		/// </summary>
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000220 RID: 544 RVA: 0x000114B0 File Offset: 0x0000F6B0
		public IEnumerable<CharacterModel> Characters
		{
			get
			{
				List<CharacterModel> list = ModelDb.AllCharacters.ToList<CharacterModel>();
				if (!this.IsEpochRevealed<Silent1Epoch>())
				{
					list.Remove(ModelDb.Character<Silent>());
				}
				if (!this.IsEpochRevealed<Regent1Epoch>())
				{
					list.Remove(ModelDb.Character<Regent>());
				}
				if (!this.IsEpochRevealed<Necrobinder1Epoch>())
				{
					list.Remove(ModelDb.Character<Necrobinder>());
				}
				if (!this.IsEpochRevealed<Defect1Epoch>())
				{
					list.Remove(ModelDb.Character<Defect>());
				}
				return list;
			}
		}

		/// <summary>
		/// Get all the ancients in the game that don't belong to a specific act and that the player has unlocked.
		/// </summary>
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0001151C File Offset: 0x0000F71C
		public IEnumerable<AncientEventModel> SharedAncients
		{
			get
			{
				List<AncientEventModel> list = ModelDb.AllSharedAncients.ToList<AncientEventModel>();
				if (!this.IsEpochRevealed<DarvEpoch>())
				{
					list.Remove(ModelDb.AncientEvent<Darv>());
				}
				return list;
			}
		}

		/// <summary>
		/// Get all the relics in the game that the player has unlocked.
		/// </summary>
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00011549 File Offset: 0x0000F749
		public IEnumerable<RelicModel> Relics
		{
			get
			{
				return ModelDb.AllRelicPools.Select<RelicPoolModel, IEnumerable<RelicModel>>((RelicPoolModel p) => p.GetUnlockedRelics(this)).SelectMany<IEnumerable<RelicModel>, RelicModel>((IEnumerable<RelicModel> r) => r);
			}
		}

		/// <summary>
		/// Get all the relics in the game that the player has unlocked.
		/// </summary>
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00011585 File Offset: 0x0000F785
		public IEnumerable<PotionModel> Potions
		{
			get
			{
				return ModelDb.AllPotionPools.Select<PotionPoolModel, IEnumerable<PotionModel>>((PotionPoolModel p) => p.GetUnlockedPotions(this)).SelectMany<IEnumerable<PotionModel>, PotionModel>((IEnumerable<PotionModel> r) => r);
			}
		}

		/// <summary>
		/// Returns true if the player has seen this encounter (won or lost).
		/// If this is the multiplayer unlock state, returns true if any player has seen the encounter.
		/// </summary>
		// Token: 0x06000224 RID: 548 RVA: 0x000115C1 File Offset: 0x0000F7C1
		public bool HasSeenEncounter(EncounterModel encounter)
		{
			return this._encountersSeen.Contains(encounter.Id);
		}

		/// <summary>
		/// Get all the card pools in the game that belong to specific characters and that the player has unlocked.
		/// </summary>
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000225 RID: 549 RVA: 0x000115D4 File Offset: 0x0000F7D4
		public IEnumerable<CardPoolModel> CharacterCardPools
		{
			get
			{
				return this.Characters.Select<CharacterModel, CardPoolModel>((CharacterModel c) => c.CardPool);
			}
		}

		/// <summary>
		/// Get all the cards in the game that the player has unlocked.
		/// Be careful using this, it includes cards that you shouldn't be able to randomly roll for rewards.
		/// </summary>
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00011600 File Offset: 0x0000F800
		public IEnumerable<CardModel> Cards
		{
			get
			{
				return this.CardPools.SelectMany<CardPoolModel, CardModel>((CardPoolModel p) => p.AllCards).Concat<CardModel>(this.Characters.SelectMany<CharacterModel, CardModel>((CharacterModel c) => c.StartingDeck).Distinct<CardModel>()).Distinct<CardModel>();
			}
		}

		/// <summary>
		/// Get all the card pools in the game that the player has unlocked.
		/// </summary>
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00011670 File Offset: 0x0000F870
		public IEnumerable<CardPoolModel> CardPools
		{
			get
			{
				return this.CharacterCardPools.Concat<CardPoolModel>(ModelDb.AllSharedCardPools).Distinct<CardPoolModel>();
			}
		}

		/// <summary>
		/// Injection constructor.
		/// </summary>
		// Token: 0x06000228 RID: 552 RVA: 0x00011688 File Offset: 0x0000F888
		public UnlockState(IEnumerable<string> unlockedEpochIds, IEnumerable<ModelId> encountersSeen, int numberOfRuns)
		{
			Func<string, bool> func;
			if ((func = UnlockState.<>O.<0>__IsValidEpoch) == null)
			{
				func = (UnlockState.<>O.<0>__IsValidEpoch = new Func<string, bool>(UnlockState.IsValidEpoch));
			}
			this._unlockedEpochIds = unlockedEpochIds.Where<string>(func).ToHashSet<string>();
			this._encountersSeen = encountersSeen.ToHashSet<ModelId>();
			this.NumberOfRuns = numberOfRuns;
		}

		/// <summary>
		/// Produces an unlock state from a progress state. Only use this to derive an unlock state for the local player.
		/// </summary>
		/// <param name="progress">The progress state to derive the unlock state from.</param>
		// Token: 0x06000229 RID: 553 RVA: 0x000116DC File Offset: 0x0000F8DC
		public UnlockState(ProgressState progress)
		{
			IEnumerable<string> enumerable = from e in progress.Epochs
				where e.State == EpochState.Revealed
				select e.Id;
			Func<string, bool> func;
			if ((func = UnlockState.<>O.<0>__IsValidEpoch) == null)
			{
				func = (UnlockState.<>O.<0>__IsValidEpoch = new Func<string, bool>(UnlockState.IsValidEpoch));
			}
			this._unlockedEpochIds = enumerable.Where<string>(func).ToHashSet<string>();
			this._encountersSeen = progress.EncounterStats.Keys.Where<ModelId>((ModelId id) => ModelDb.GetByIdOrNull<AbstractModel>(id) is EncounterModel).ToHashSet<ModelId>();
			this.NumberOfRuns = progress.NumberOfRuns;
		}

		/// <summary>
		/// Produces an unlock state that is the union of all passed unlock states.
		/// In essence: In a multiplayer game, if one person has something unlocked, it will be unlocked for everyone in the
		/// multiplayer session.
		/// </summary>
		/// <param name="unlockStatesEnumerable">Set of states to merge together.</param>
		// Token: 0x0600022A RID: 554 RVA: 0x000117B0 File Offset: 0x0000F9B0
		public UnlockState(IEnumerable<UnlockState> unlockStatesEnumerable)
		{
			UnlockState[] array = unlockStatesEnumerable.ToArray<UnlockState>();
			IEnumerable<string> enumerable = array.Select<UnlockState, HashSet<string>>((UnlockState s) => s._unlockedEpochIds).SelectMany<HashSet<string>, string>((HashSet<string> m) => m).Distinct<string>();
			Func<string, bool> func;
			if ((func = UnlockState.<>O.<0>__IsValidEpoch) == null)
			{
				func = (UnlockState.<>O.<0>__IsValidEpoch = new Func<string, bool>(UnlockState.IsValidEpoch));
			}
			this._unlockedEpochIds = enumerable.Where<string>(func).ToHashSet<string>();
			this._encountersSeen = array.Select<UnlockState, HashSet<ModelId>>((UnlockState s) => s._encountersSeen).SelectMany<HashSet<ModelId>, ModelId>((HashSet<ModelId> b) => b).Distinct<ModelId>()
				.ToHashSet<ModelId>();
			this.NumberOfRuns = array.Max<UnlockState>((UnlockState s) => s.NumberOfRuns);
		}

		/// <summary>
		/// Used to filter invalid epochs out from the unlock state.
		/// If invalid epochs are placed into _unlockedEpochs, when we try to serialize this object in multiplayer, we'll
		/// get an error because they do not exist in the integer map.
		/// Mostly useful for mods that may remove epochs, but also useful if we ever decide to deprecate epochs.
		/// I know we could pass EpochModel.IsValid directly, but the comment is useful context.
		/// </summary>
		// Token: 0x0600022B RID: 555 RVA: 0x000118C7 File Offset: 0x0000FAC7
		public static bool IsValidEpoch(string epochId)
		{
			return EpochModel.IsValid(epochId);
		}

		/// <summary>
		/// Checks if this player has revealed an epoch on the timeline.
		/// </summary>
		/// <typeparam name="T">The type of epoch.</typeparam>
		// Token: 0x0600022C RID: 556 RVA: 0x000118CF File Offset: 0x0000FACF
		[NullableContext(0)]
		public bool IsEpochRevealed<T>() where T : EpochModel
		{
			return this._unlockedEpochIds.Contains(EpochModel.GetId<T>());
		}

		/// <returns>The amount of epochs the player has unlocked.</returns>
		// Token: 0x0600022D RID: 557 RVA: 0x000118E1 File Offset: 0x0000FAE1
		public int EpochUnlockCount()
		{
			return this._unlockedEpochIds.Count;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000118EE File Offset: 0x0000FAEE
		public SerializableUnlockState ToSerializable()
		{
			return new SerializableUnlockState
			{
				UnlockedEpochs = this._unlockedEpochIds.ToList<string>(),
				EncountersSeen = this._encountersSeen.ToList<ModelId>(),
				NumberOfRuns = this.NumberOfRuns
			};
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00011924 File Offset: 0x0000FB24
		public static UnlockState FromSerializable(SerializableUnlockState unlockState)
		{
			if (unlockState == null)
			{
				return UnlockState.all;
			}
			IEnumerable<ModelId> encountersSeen = unlockState.EncountersSeen;
			Func<ModelId, EncounterModel> func;
			if ((func = UnlockState.<>O.<1>__EncounterOrDeprecated) == null)
			{
				func = (UnlockState.<>O.<1>__EncounterOrDeprecated = new Func<ModelId, EncounterModel>(SaveUtil.EncounterOrDeprecated));
			}
			List<ModelId> list = (from e in encountersSeen.Select<ModelId, EncounterModel>(func)
				select e.Id).ToHashSet<ModelId>().ToList<ModelId>();
			return new UnlockState(unlockState.UnlockedEpochs, list, unlockState.NumberOfRuns);
		}

		// Token: 0x040000E3 RID: 227
		public static readonly UnlockState none = new UnlockState(Array.Empty<string>(), Array.Empty<ModelId>(), 0);

		// Token: 0x040000E4 RID: 228
		public static readonly UnlockState all = new UnlockState(EpochModel.AllEpochIds, ModelDb.AllEncounters.Select<EncounterModel, ModelId>((EncounterModel e) => e.Id), 999999999);

		/// <summary>
		/// The total set of epochs revealed.
		/// If this is the shared unlock set, this is the superset of all epochs from all players.
		/// </summary>
		// Token: 0x040000E5 RID: 229
		private readonly HashSet<string> _unlockedEpochIds;

		/// <summary>
		/// The encounter IDs this player has seen (lost or won).
		/// If this is the shared unlock set, this is the superset of all encounters seen by all players.
		/// </summary>
		// Token: 0x040000E6 RID: 230
		private readonly HashSet<ModelId> _encountersSeen;

		// Token: 0x02000E04 RID: 3588
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040032E0 RID: 13024
			[Nullable(0)]
			public static Func<string, bool> <0>__IsValidEpoch;

			// Token: 0x040032E1 RID: 13025
			[Nullable(0)]
			public static Func<ModelId, EncounterModel> <1>__EncounterOrDeprecated;
		}
	}
}
