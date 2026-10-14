using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Runs
{
	// Token: 0x02000111 RID: 273
	[NullableContext(1)]
	[Nullable(0)]
	public class RunRngSet
	{
		// Token: 0x06000CD6 RID: 3286 RVA: 0x00050ED5 File Offset: 0x0004F0D5
		public static RunRngSet GetMockInstance()
		{
			if (TestMode.IsOff)
			{
				throw new InvalidOperationException("Cannot get RunRng when not in a run outside of tests!");
			}
			return RunRngSet._mockInstance;
		}

		/// <summary>
		/// We generate a string for the seed that gets hashed to a uint for the actual thing passed to the RNGs.
		/// This is the original string that was input, for display purposes.
		/// </summary>
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x00050EEE File Offset: 0x0004F0EE
		public string StringSeed { get; }

		/// <summary>
		/// The seed that was hashed from the InputSeed.
		/// </summary>
		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00050EF6 File Offset: 0x0004F0F6
		public ulong Seed { get; }

		/// <summary>
		/// Determines everything that's generated upfront when a run first starts. This includes:
		/// * Which monsters you'll fight.
		/// * Which events you'll run into.
		/// * Which relics you'll be offered.
		/// </summary>
		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00050EFE File Offset: 0x0004F0FE
		public Rng UpFront
		{
			get
			{
				return this.GetRng(RunRngType.UpFront);
			}
		}

		/// <summary>
		/// Determines how your draw pile gets shuffled, both at the start of combat and when you
		/// run out of cards in it.
		/// </summary>
		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00050F07 File Offset: 0x0004F107
		public Rng Shuffle
		{
			get
			{
				return this.GetRng(RunRngType.Shuffle);
			}
		}

		/// <summary>
		/// Determines what types of room is rolled when visiting an unknown map point.
		/// </summary>
		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00050F10 File Offset: 0x0004F110
		public Rng UnknownMapPoint
		{
			get
			{
				return this.GetRng(RunRngType.UnknownMapPoint);
			}
		}

		/// <summary>
		/// Determines what cards are generated during combat by things like Attack Potion.
		/// Distinct from CardRewards because we don't want Attack Potion usage to impact card rewards.
		/// </summary>
		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x00050F19 File Offset: 0x0004F119
		public Rng CombatCardGeneration
		{
			get
			{
				return this.GetRng(RunRngType.CombatCardGeneration);
			}
		}

		/// <summary>
		/// Determines what potions are generated during combat by things like Alchemize.
		/// Distinct from PotionRewards because these can't generate healing potions.
		/// </summary>
		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x00050F22 File Offset: 0x0004F122
		public Rng CombatPotionGeneration
		{
			get
			{
				return this.GetRng(RunRngType.CombatPotionGeneration);
			}
		}

		/// <summary>
		/// Determines what cards are randomly chosen during combat by things like True Grit.
		/// </summary>
		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x00050F2B File Offset: 0x0004F12B
		public Rng CombatCardSelection
		{
			get
			{
				return this.GetRng(RunRngType.CombatCardSelection);
			}
		}

		/// <summary>
		/// Determines random in-combat energy costs for things like Confusion and Snecko Eye.
		/// </summary>
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x00050F34 File Offset: 0x0004F134
		public Rng CombatEnergyCosts
		{
			get
			{
				return this.GetRng(RunRngType.CombatEnergyCosts);
			}
		}

		/// <summary>
		/// Determines the results of random targeting during combat (Bouncing Flask, Sword Boomerang, etc.).
		/// </summary>
		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00050F3D File Offset: 0x0004F13D
		public Rng CombatTargets
		{
			get
			{
				return this.GetRng(RunRngType.CombatTargets);
			}
		}

		/// <summary>
		/// Determines what moves each monster makes whenever there's randomness involved.
		/// </summary>
		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x00050F46 File Offset: 0x0004F146
		public Rng MonsterAi
		{
			get
			{
				return this.GetRng(RunRngType.MonsterAi);
			}
		}

		/// <summary>
		/// Determines some niche one-off RNG stuff that we don't care about interacting, like the <see cref="T:MegaCrit.Sts2.Core.Models.Modifiers.CursedRun" />
		/// modifier.
		/// </summary>
		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x00050F4F File Offset: 0x0004F14F
		public Rng Niche
		{
			get
			{
				return this.GetRng(RunRngType.Niche);
			}
		}

		/// <summary>
		/// Determines what orbs are randomly chosen during combat by things like Chaos.
		/// </summary>
		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x00050F59 File Offset: 0x0004F159
		public Rng CombatOrbGeneration
		{
			get
			{
				return this.GetRng(RunRngType.CombatOrbs);
			}
		}

		/// <summary>
		/// Determines who gets treasure when multiple players pick the same relic at the treasure room.
		/// </summary>
		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x00050F63 File Offset: 0x0004F163
		public Rng TreasureRoomRelics
		{
			get
			{
				return this.GetRng(RunRngType.TreasureRoomRelics);
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00050F70 File Offset: 0x0004F170
		public RunRngSet(string seed)
		{
			this.StringSeed = seed;
			if (seed.StartsWith("old"))
			{
				string stringSeed = this.StringSeed;
				int length = "old".Length;
				this.Seed = (ulong)StringHelper.GetDeterministicHashCodeOld(stringSeed.Substring(length, stringSeed.Length - length));
			}
			else
			{
				this.Seed = StringHelper.GetDeterministicHashCode(seed);
			}
			foreach (RunRngType runRngType in Enum.GetValues<RunRngType>())
			{
				this._rngs[runRngType] = this.CreateRng(runRngType);
			}
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0005100C File Offset: 0x0004F20C
		private Rng CreateRng(RunRngType rngType)
		{
			string text = StringHelper.SnakeCase(rngType.ToString());
			return new Rng(this.Seed, text);
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00051038 File Offset: 0x0004F238
		public SerializableRunRngSet ToSerializable()
		{
			SerializableRunRngSet serializableRunRngSet = new SerializableRunRngSet
			{
				Seed = this.StringSeed
			};
			foreach (KeyValuePair<RunRngType, Rng> keyValuePair in this._rngs)
			{
				RunRngType runRngType;
				Rng rng;
				keyValuePair.Deconstruct(out runRngType, out rng);
				RunRngType runRngType2 = runRngType;
				Rng rng2 = rng;
				serializableRunRngSet.Rngs[runRngType2] = rng2.ToSerializable();
			}
			return serializableRunRngSet;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x000510BC File Offset: 0x0004F2BC
		public static RunRngSet FromSave(SerializableRunRngSet save)
		{
			RunRngSet runRngSet = new RunRngSet(save.Seed);
			foreach (KeyValuePair<RunRngType, SerializableRng> keyValuePair in save.Rngs)
			{
				RunRngType runRngType;
				SerializableRng serializableRng;
				keyValuePair.Deconstruct(out runRngType, out serializableRng);
				RunRngType runRngType2 = runRngType;
				SerializableRng serializableRng2 = serializableRng;
				runRngSet._rngs[runRngType2] = new Rng(serializableRng2);
			}
			return runRngSet;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0005113C File Offset: 0x0004F33C
		public void LoadFromSerializable(SerializableRunRngSet save)
		{
			if (this.StringSeed != save.Seed)
			{
				throw new NotImplementedException("RngSet seed should not change during the run!");
			}
			foreach (KeyValuePair<RunRngType, SerializableRng> keyValuePair in save.Rngs)
			{
				RunRngType runRngType;
				SerializableRng serializableRng;
				keyValuePair.Deconstruct(out runRngType, out serializableRng);
				RunRngType runRngType2 = runRngType;
				SerializableRng serializableRng2 = serializableRng;
				this._rngs[runRngType2].LoadFromSerializable(serializableRng2);
			}
		}

		/// <summary>
		/// ONLY USE THIS FOR TESTING!
		/// Mock out the specified RNG type to use the specified seed.
		/// </summary>
		// Token: 0x06000CEA RID: 3306 RVA: 0x000511CC File Offset: 0x0004F3CC
		public void MockRng(RunRngType rngType, ulong seed)
		{
			this._rngs[rngType] = new Rng(seed);
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x000511E0 File Offset: 0x0004F3E0
		public Rng GetRng(RunRngType rngType)
		{
			return this._rngs[rngType];
		}

		// Token: 0x0400046F RID: 1135
		private static readonly RunRngSet _mockInstance = new RunRngSet(string.Empty);

		// Token: 0x04000472 RID: 1138
		private readonly Dictionary<RunRngType, Rng> _rngs = new Dictionary<RunRngType, Rng>();
	}
}
