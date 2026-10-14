using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Random
{
	/// <summary>
	/// A custom random class which allows predictable results when utilizing seeds.
	/// </summary>
	// Token: 0x02000151 RID: 337
	[NullableContext(1)]
	[Nullable(0)]
	public class Rng
	{
		/// <summary>
		/// A non-deterministic RNG instance. This will not produce the same results after saving and loading.
		/// Good for when we need to randomize things that don't impact gameplay.
		/// We use this instead of the GD.Rand functions because they're extremely slow when called from C#.
		/// </summary>
		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x00061B7A File Offset: 0x0005FD7A
		public static Rng Chaotic { get; } = new Rng((ulong)DateTimeOffset.Now.ToUnixTimeSeconds());

		// Token: 0x06001046 RID: 4166 RVA: 0x00061B81 File Offset: 0x0005FD81
		public Rng(ulong seed = 0UL)
		{
			this._random = new MegaRandom(seed);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00061B95 File Offset: 0x0005FD95
		public Rng(SerializableRng serializable)
		{
			this._counter = serializable.counter;
			this._random = new MegaRandom(serializable);
		}

		/// <summary>
		/// An ergonomic constructor for creating an RNG for a specific piece of content during a run.
		/// This RNG will be unique among:
		///  - The run (using player.RunState.Rng)
		///  - The player SLOT
		///  - The ID of the content passed
		///
		/// This uses the player _slot index_, rather than the NetId, to ensure consistent results when playing a daily run.
		/// Because it only uses the three things above, if the player sees the same piece of content again this run, the
		/// RNG will return the same results. If that is a possibility, pass in a <paramref name="mixin" />.
		/// </summary>
		// Token: 0x06001048 RID: 4168 RVA: 0x00061BB5 File Offset: 0x0005FDB5
		public Rng(Player player, ModelId id, ulong mixin = 0UL)
			: this(player.RunState.Rng.Seed + (ulong)((long)player.RunState.GetPlayerSlotIndex(player)) + StringHelper.GetDeterministicHashCode(id.Entry) + mixin)
		{
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00061BE9 File Offset: 0x0005FDE9
		public Rng(ulong seed, string name)
			: this(seed + StringHelper.GetDeterministicHashCode(name))
		{
		}

		/// <summary>
		/// Load RNG state from serializable.
		/// Use this when you need to keep the old instance's references intact.
		/// </summary>
		// Token: 0x0600104A RID: 4170 RVA: 0x00061BF9 File Offset: 0x0005FDF9
		public void LoadFromSerializable(SerializableRng serializable)
		{
			this._counter = serializable.counter;
			this._random.Reinitialise(serializable);
		}

		/// <summary>
		/// Returns true or false. 50/50
		/// </summary>
		// Token: 0x0600104B RID: 4171 RVA: 0x00061C13 File Offset: 0x0005FE13
		public bool NextBool()
		{
			this._counter++;
			return this._random.Next(2) == 0;
		}

		/// <summary>
		/// Get a random integer between 0 (inclusive) and maxExclusive.
		/// </summary>
		/// <param name="maxExclusive">1 higher than the highest allowed int.</param>
		/// <returns>Random integer.</returns>
		// Token: 0x0600104C RID: 4172 RVA: 0x00061C32 File Offset: 0x0005FE32
		public int NextInt(int maxExclusive = 2147483647)
		{
			this._counter++;
			return this._random.Next(maxExclusive);
		}

		/// <summary>
		/// Get a random integer between minInclusive and maxExclusive.
		/// </summary>
		/// <param name="minInclusive">Lowest allowed number.</param>
		/// <param name="maxExclusive">1 higher than the maximum allowed number.</param>
		/// <returns>Random integer.</returns>
		// Token: 0x0600104D RID: 4173 RVA: 0x00061C4E File Offset: 0x0005FE4E
		public int NextInt(int minInclusive, int maxExclusive)
		{
			if (minInclusive >= maxExclusive)
			{
				throw new ArgumentOutOfRangeException("minInclusive", "Minimum must be lower than maximum.");
			}
			this._counter++;
			return this._random.Next(minInclusive, maxExclusive);
		}

		/// <summary>
		/// Get a random integer between 0 (inclusive) and maxExclusive.
		/// </summary>
		/// <param name="maxExclusive">1 higher than the highest allowed int.</param>
		/// <returns>Random integer.</returns>
		// Token: 0x0600104E RID: 4174 RVA: 0x00061C7F File Offset: 0x0005FE7F
		public uint NextUnsignedInt(uint maxExclusive = 4294967295U)
		{
			return this.NextUnsignedInt(0U, maxExclusive);
		}

		/// <summary>
		/// Get a random integer between minInclusive and maxInclusive.
		/// </summary>
		/// <param name="minInclusive">Lowest allowed number.</param>
		/// <param name="maxExclusive">1 higher than the maximum allowed number.</param>
		/// <returns>Random integer.</returns>
		// Token: 0x0600104F RID: 4175 RVA: 0x00061C8C File Offset: 0x0005FE8C
		public uint NextUnsignedInt(uint minInclusive, uint maxExclusive)
		{
			if (minInclusive >= maxExclusive)
			{
				throw new ArgumentOutOfRangeException("minInclusive", "Minimum must be lower than maximum.");
			}
			this._counter++;
			double num = this._random.NextDouble();
			double num2 = maxExclusive - minInclusive;
			uint num3 = (uint)(num * num2);
			return minInclusive + num3;
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00061CD5 File Offset: 0x0005FED5
		public ulong NextUnsignedLong()
		{
			this._counter++;
			return this._random.NextULong();
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00061CF0 File Offset: 0x0005FEF0
		public ulong NextUnsignedLong(ulong maxExclusive = 18446744073709551615UL)
		{
			if (maxExclusive == 18446744073709551615UL)
			{
				return this.NextUnsignedLong();
			}
			return this.NextUnsignedLong(0UL, maxExclusive);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00061D08 File Offset: 0x0005FF08
		public ulong NextUnsignedLong(ulong minInclusive, ulong maxExclusive)
		{
			if (minInclusive >= maxExclusive)
			{
				throw new ArgumentOutOfRangeException("minInclusive", "Minimum must be lower than maximum.");
			}
			this._counter++;
			double num = this._random.NextDouble();
			double num2 = maxExclusive - minInclusive;
			ulong num3 = (ulong)(num * num2);
			return minInclusive + num3;
		}

		/// <summary>
		/// Get a random floating-point between 0 and max.
		/// </summary>
		/// <param name="max">Highest allowed number.</param>
		/// <returns>Random float.</returns>
		// Token: 0x06001053 RID: 4179 RVA: 0x00061D51 File Offset: 0x0005FF51
		public float NextFloat(float max = 1f)
		{
			return this.NextFloat(0f, max);
		}

		/// <summary>
		/// Get a random floating-point number between min and max.
		/// </summary>
		/// <param name="min">Lowest allowed number.</param>
		/// <param name="max">Highest allowed number.</param>
		/// <returns>Random float.</returns>
		// Token: 0x06001054 RID: 4180 RVA: 0x00061D5F File Offset: 0x0005FF5F
		public float NextFloat(float min, float max)
		{
			if (min > max)
			{
				throw new ArgumentOutOfRangeException("min", "Minimum must not be higher than maximum.");
			}
			this._counter++;
			return (float)(this._random.NextDouble() * (double)(max - min) + (double)min);
		}

		/// <summary>
		/// Get a random double-precision (double) between 0.0 (inclusive) and 1.0 (exclusive).
		/// </summary>
		/// <returns>Random double.</returns>
		// Token: 0x06001055 RID: 4181 RVA: 0x00061D97 File Offset: 0x0005FF97
		public double NextDouble()
		{
			this._counter++;
			return this._random.NextDouble();
		}

		/// <summary>
		/// Get a random double-precision floating-point number between 0 and 1.
		/// </summary>
		/// <returns>Random double.</returns>
		// Token: 0x06001056 RID: 4182 RVA: 0x00061DB2 File Offset: 0x0005FFB2
		public double NextDouble(double min, double max)
		{
			if (min > max)
			{
				throw new ArgumentOutOfRangeException("min", "Minimum must not be higher than maximum.");
			}
			this._counter++;
			return this._random.NextDouble() * (max - min) + min;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00061DE7 File Offset: 0x0005FFE7
		public float NextGaussianFloat(float mean = 0f, float stdDev = 1f, float min = 0f, float max = 1f)
		{
			return (float)this.NextGaussianDouble((double)mean, (double)stdDev, (double)min, (double)max);
		}

		/// <summary>
		/// Generate a random floating-point number with a Gaussian distribution between min and max with the specified mean
		/// and standard deviation.
		/// See https://en.wikipedia.org/wiki/Normal_distribution for more on Gaussian distributions.
		/// </summary>
		/// <param name="mean">Mean for the Gaussian distribution.</param>
		/// <param name="stdDev">Standard deviation for the Gaussian distribution.</param>
		/// <param name="min">Lowest allowed number.</param>
		/// <param name="max">Highest allowed number.</param>
		/// <returns>Random double with a Gaussian distribution.</returns>
		// Token: 0x06001058 RID: 4184 RVA: 0x00061DFC File Offset: 0x0005FFFC
		public double NextGaussianDouble(double mean = 0.0, double stdDev = 1.0, double min = 0.0, double max = 1.0)
		{
			if (min > max)
			{
				throw new ArgumentOutOfRangeException("min", "Minimum must not be higher than maximum.");
			}
			bool flag = mean < 0.0 || mean > 1.0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("mean", mean, "Mean must be within [0, 1].");
			}
			double num6;
			do
			{
				double num = 1.0 - this.NextDouble();
				double num2 = 1.0 - this.NextDouble();
				double num3 = Math.Sqrt(-2.0 * Math.Log(num));
				double num4 = 6.283185307179586 * num2;
				double num5 = num3 * Math.Cos(num4);
				num6 = mean + num5 * stdDev;
			}
			while (num6 < 0.0 || num6 > 1.0);
			return num6 * (max - min) + min;
		}

		/// <summary>
		/// Returns a random number within a normal distribution.
		/// This is used as part of the algorithm for location assignment.
		/// <param name="mean">Mean for the Gaussian distribution.</param>
		/// <param name="stdDev">Standard deviation for the Gaussian distribution.</param>
		/// <param name="min">Lowest allowed number.</param>
		/// <param name="max">Highest allowed number.</param>
		/// <returns></returns>
		/// </summary>
		// Token: 0x06001059 RID: 4185 RVA: 0x00061ED8 File Offset: 0x000600D8
		public int NextGaussianInt(int mean, int stdDev, int min, int max)
		{
			if (min > max)
			{
				throw new ArgumentOutOfRangeException("min", "Minimum must not be higher than maximum.");
			}
			if (mean < min || mean > max)
			{
				throw new ArgumentOutOfRangeException("mean", mean, "Mean must be within [min, max].");
			}
			int num5;
			do
			{
				double num = 1.0 - this.NextDouble();
				double num2 = 1.0 - this.NextDouble();
				double num3 = Math.Sqrt(-2.0 * Math.Log(num)) * Math.Sin(6.283185307179586 * num2);
				double num4 = (double)mean + (double)stdDev * num3;
				num5 = (int)Math.Round(num4);
			}
			while (num5 < min || num5 > max);
			return num5;
		}

		/// <summary>
		/// Get a random item from the specified set of items.
		/// </summary>
		/// <param name="items">Set of items to pull from.</param>
		/// <typeparam name="T">Type of items contained in the set.</typeparam>
		/// <returns>Random item.</returns>
		// Token: 0x0600105A RID: 4186 RVA: 0x00061F80 File Offset: 0x00060180
		[NullableContext(2)]
		public T NextItem<T>([Nullable(1)] IEnumerable<T> items)
		{
			IEnumerable<T> enumerable = (items as T[]) ?? items.ToArray<T>();
			int num = enumerable.Count<T>();
			if (num == 0)
			{
				return default(T);
			}
			int num2 = this.NextInt(0, num);
			return enumerable.ElementAt<T>(num2);
		}

		/// <summary>
		/// Get a random item from the specified set of items, respecting a specified weighting function.
		/// </summary>
		/// <param name="items">Set of items to pull from.</param>
		/// <param name="weightFetcher">Function to determine which items should be returned more/less often.</param>
		/// <typeparam name="T">Type of items contained in the set.</typeparam>
		/// <returns>Random item.</returns>
		// Token: 0x0600105B RID: 4187 RVA: 0x00061FC4 File Offset: 0x000601C4
		[NullableContext(2)]
		public T WeightedNextItem<T>([Nullable(1)] IEnumerable<T> items, [Nullable(new byte[] { 1, 2 })] Func<T, float> weightFetcher)
		{
			return Rng.WeightedNextItem<T>(this.NextFloat(1f), items, weightFetcher, default(T));
		}

		/// <summary>
		/// Get a random item from the specified set of items, respecting a specified weighting function.
		/// </summary>
		/// <param name="randInput">Floating-point number between 0 and 1 to use for randomness.</param>
		/// <param name="items">Set of items to pull from.</param>
		/// <param name="weightFetcher">Function to determine which items should be returned more/less often.</param>
		/// <param name="fallback">Element to return if the weighting function fails to grab an item.</param>
		/// <typeparam name="T">Type of items contained in the set.</typeparam>
		/// <returns>Random item.</returns>
		// Token: 0x0600105C RID: 4188 RVA: 0x00061FEC File Offset: 0x000601EC
		public static T WeightedNextItem<[Nullable(2)] T>(float randInput, IEnumerable<T> items, Func<T, float> weightFetcher, T fallback)
		{
			float num = items.Sum<T>(weightFetcher);
			float num2 = randInput * num;
			foreach (T t in items)
			{
				num2 -= weightFetcher(t);
				if (num2 <= 0f)
				{
					return t;
				}
			}
			return fallback;
		}

		/// <summary>
		/// Shuffles a list in place using the Fisher-Yates algorithm.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		/// <typeparam name="T">Type of items in the list.</typeparam>
		// Token: 0x0600105D RID: 4189 RVA: 0x00062054 File Offset: 0x00060254
		public void Shuffle<[Nullable(2)] T>(IList<T> list)
		{
			for (int i = list.Count - 1; i > 0; i--)
			{
				int num = this.NextInt(i + 1);
				int num2 = i;
				int num3 = num;
				T t = list[num];
				T t2 = list[i];
				list[num2] = t;
				list[num3] = t2;
			}
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x000620AC File Offset: 0x000602AC
		public SerializableRng ToSerializable()
		{
			SerializableRng serializableRng = new SerializableRng
			{
				counter = this._counter
			};
			this._random.FillSerializableState(serializableRng);
			return serializableRng;
		}

		// Token: 0x040005C1 RID: 1473
		private int _counter;

		// Token: 0x040005C2 RID: 1474
		private readonly MegaRandom _random;
	}
}
