using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Random
{
	/// <summary>
	/// Xoshiro256** (xor, shift, rotate) pseudo-random number generator (PRNG).
	/// The original algorithm's license is as follows.
	///
	/// ====================================================================
	/// Written in 2018 by David Blackman and Sebastiano Vigna (vigna@acm.org)
	///
	/// To the extent possible under law, the author has dedicated all copyright
	/// and related and neighboring rights to this software to the public domain
	/// worldwide. This software is distributed without any warranty.
	/// See <see href="http://creativecommons.org/publicdomain/zero/1.0/" />. */
	/// =====================================================================
	///
	/// This particular implementation is copied from the Redzen library with heavy modifications:
	/// https://github.com/colgreen/Redzen/blob/main/Redzen/Random/Xoshiro256StarStarRandom.cs
	/// The original license of the Redzen library is as follows.
	///
	/// =====================================================================
	/// Redzen code library.
	///
	/// Copyright 2015-2023 Colin D. Green (colin.green1@gmail.com)
	///
	/// This software is issued under the MIT License.
	///
	/// Permission is hereby granted, free of charge, to any person obtaining a copy
	/// of this software and associated documentation files (the "Software"), to deal
	/// in the Software without restriction, including without limitation the rights
	/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	/// copies of the Software, and to permit persons to whom the Software is
	/// furnished to do so, subject to the following conditions:
	///
	/// The above copyright notice and this permission notice shall be included in
	/// all copies or substantial portions of the Software.
	///
	/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
	/// THE SOFTWARE.
	/// </summary>
	// Token: 0x0200014F RID: 335
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MegaRandom
	{
		/// <summary>
		/// Initializes a new instance with the provided seed.
		/// </summary>
		/// <param name="seed">Seed value.</param>
		// Token: 0x06001029 RID: 4137 RVA: 0x000615DE File Offset: 0x0005F7DE
		public MegaRandom(ulong seed)
		{
			this.Reinitialise(seed);
		}

		/// <summary>
		/// Initializes a new instance from a serialized instance.
		/// </summary>
		// Token: 0x0600102A RID: 4138 RVA: 0x000615ED File Offset: 0x0005F7ED
		public MegaRandom(SerializableRng serializable)
		{
			this.Reinitialise(serializable);
		}

		/// <summary>
		/// Splitmix64 PRNG.
		/// </summary>
		/// <param name="x">PRNG state. This can take any value, including zero.</param>
		/// <returns>A new random UInt64.</returns>
		// Token: 0x0600102B RID: 4139 RVA: 0x000615FC File Offset: 0x0005F7FC
		public static ulong Splitmix64(ref ulong x)
		{
			ulong num = (x += 11400714819323198485UL);
			num = (num ^ (num >> 30)) * 13787848793156543929UL;
			num = (num ^ (num >> 27)) * 10723151780598845931UL;
			return num ^ (num >> 31);
		}

		/// <summary>
		/// Re-initialises the random number generator state using the provided seed value.
		/// </summary>
		/// <param name="seed">Seed value.</param>
		// Token: 0x0600102C RID: 4140 RVA: 0x00061643 File Offset: 0x0005F843
		public void Reinitialise(ulong seed)
		{
			this._s0 = MegaRandom.Splitmix64(ref seed);
			this._s1 = MegaRandom.Splitmix64(ref seed);
			this._s2 = MegaRandom.Splitmix64(ref seed);
			this._s3 = MegaRandom.Splitmix64(ref seed);
		}

		/// <summary>
		/// Initializes the RNG state from a serializable object round-tripped from ToSerializable.
		/// </summary>
		// Token: 0x0600102D RID: 4141 RVA: 0x00061679 File Offset: 0x0005F879
		public void Reinitialise(SerializableRng serializable)
		{
			this._s0 = serializable.state0;
			this._s1 = serializable.state1;
			this._s2 = serializable.state2;
			this._s3 = serializable.state3;
		}

		/// <summary>
		/// Fills the provided span with random byte values, sampled from the uniform distribution with interval [0, 255].
		/// </summary>
		/// <param name="span">The byte span to fill with random samples.</param>
		// Token: 0x0600102E RID: 4142 RVA: 0x000616AC File Offset: 0x0005F8AC
		[NullableContext(0)]
		public unsafe void NextBytes(Span<byte> span)
		{
			ulong num = this._s0;
			ulong num2 = this._s1;
			ulong num3 = this._s2;
			ulong num4 = this._s3;
			while (span.Length >= 8)
			{
				Unsafe.WriteUnaligned<ulong>(MemoryMarshal.GetReference<byte>(span), BitOperations.RotateLeft(num2 * 5UL, 7) * 9UL);
				ulong num5 = num2 << 17;
				num3 ^= num;
				num4 ^= num2;
				num2 ^= num3;
				num ^= num4;
				num3 ^= num5;
				num4 = BitOperations.RotateLeft(num4, 45);
				span = span.Slice(8);
			}
			if (!span.IsEmpty)
			{
				ulong num6 = BitOperations.RotateLeft(num2 * 5UL, 7) * 9UL;
				byte* ptr = (byte*)(&num6);
				for (int i = 0; i < span.Length; i++)
				{
					*span[i] = ptr[i];
				}
				ulong num7 = num2 << 17;
				num3 ^= num;
				num4 ^= num2;
				num2 ^= num3;
				num ^= num4;
				num3 ^= num7;
				num4 = BitOperations.RotateLeft(num4, 45);
			}
			this._s0 = num;
			this._s1 = num2;
			this._s2 = num3;
			this._s3 = num4;
		}

		/// <summary>
		/// Get the next 64 random bits from the underlying PRNG. This method forms the foundation for most of the methods of each
		/// Next* implementation, which take these 64 bits and manipulate them to provide random values of various
		/// data types, such as integers, byte arrays, floating point values, etc.
		/// </summary>
		/// <returns>A <see cref="T:System.UInt64" /> containing random bits from the underlying PRNG algorithm.</returns>
		// Token: 0x0600102F RID: 4143 RVA: 0x000617AC File Offset: 0x0005F9AC
		private ulong NextULongInner()
		{
			ulong num = this._s0;
			ulong num2 = this._s1;
			ulong num3 = this._s2;
			ulong num4 = this._s3;
			ulong num5 = BitOperations.RotateLeft(num2 * 5UL, 7) * 9UL;
			ulong num6 = num2 << 17;
			num3 ^= num;
			num4 ^= num2;
			num2 ^= num3;
			num ^= num4;
			num3 ^= num6;
			num4 = BitOperations.RotateLeft(num4, 45);
			this._s0 = num;
			this._s1 = num2;
			this._s2 = num3;
			this._s3 = num4;
			return num5;
		}

		/// <summary>
		/// Returns a random integer sampled from the uniform distribution with interval [0, maxValue),
		/// i.e., exclusive of <paramref name="maxValue" />.
		/// </summary>
		/// <param name="maxValue">The maximum value to be sampled (exclusive).</param>
		/// <returns>A new random sample.</returns>
		// Token: 0x06001030 RID: 4144 RVA: 0x00061827 File Offset: 0x0005FA27
		public int Next(int maxValue)
		{
			if (maxValue < 1)
			{
				throw new ArgumentOutOfRangeException("maxValue", maxValue, "maxValue must be > 0");
			}
			return this.NextInner(maxValue);
		}

		/// <summary>
		/// Returns a random integer sampled from the uniform distribution with interval [minValue, maxValue),
		/// i.e., inclusive of <paramref name="minValue" /> and exclusive of <paramref name="maxValue" />.
		/// </summary>
		/// <param name="minValue">The minimum value to be sampled (inclusive).</param>
		/// <param name="maxValue">The maximum value to be sampled (exclusive).</param>
		/// <returns>A new random sample.</returns>
		/// <remarks>
		/// maxValue must be greater than minValue. minValue may be negative.
		/// </remarks>
		// Token: 0x06001031 RID: 4145 RVA: 0x0006184C File Offset: 0x0005FA4C
		public int Next(int minValue, int maxValue)
		{
			if (minValue >= maxValue)
			{
				throw new ArgumentOutOfRangeException("maxValue", maxValue, "maxValue must be > minValue");
			}
			long num = (long)maxValue - (long)minValue;
			if (num <= 2147483647L)
			{
				return this.NextInner((int)num) + minValue;
			}
			return (int)(this.NextInner(num) + (long)minValue);
		}

		/// <summary>
		/// Returns a random <see cref="T:System.Double" /> sampled from the uniform distribution with interval [0, 1),
		/// i.e., inclusive of 0.0 and exclusive of 1.0.
		/// </summary>
		/// <returns>A new random sample, of type <see cref="T:System.Double" />.</returns>
		// Token: 0x06001032 RID: 4146 RVA: 0x00061898 File Offset: 0x0005FA98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double NextDouble()
		{
			return (this.NextULongInner() >> 11) * 1.1102230246251565E-16;
		}

		/// <summary>
		/// Returns a random integer sampled from the uniform distribution with interval [0, int.MaxValue],
		/// i.e., <b>inclusive</b> of <see cref="F:System.Int32.MaxValue" />.
		/// </summary>
		/// <returns>A new random sample.</returns>
		/// <remarks>
		/// This method differs from <see cref="M:System.Random.Next" />, in the following way; the uniform distribution that
		/// is sampled from includes the value <see cref="F:System.Int32.MaxValue" />.
		/// </remarks>
		// Token: 0x06001033 RID: 4147 RVA: 0x000618AF File Offset: 0x0005FAAF
		public int NextInt()
		{
			return (int)(this.NextULongInner() >> 33);
		}

		/// <summary>
		/// Returns a random <see cref="T:System.UInt32" /> sampled from the uniform distribution with interval [0, uint.MaxValue],
		/// i.e., over the full range of possible uint values.
		/// </summary>
		/// <returns>A new random sample.</returns>
		// Token: 0x06001034 RID: 4148 RVA: 0x000618BB File Offset: 0x0005FABB
		public uint NextUInt()
		{
			return (uint)this.NextULongInner();
		}

		/// <summary>
		/// Returns a random <see cref="T:System.UInt64" /> sampled from the uniform distribution with interval [0, ulong.MaxValue],
		/// i.e., over the full range of possible ulong values.
		/// </summary>
		/// <returns>A new random sample.</returns>
		// Token: 0x06001035 RID: 4149 RVA: 0x000618C4 File Offset: 0x0005FAC4
		public ulong NextULong()
		{
			return this.NextULongInner();
		}

		/// <summary>
		/// Returns a random boolean sampled from the uniform discrete distribution {false, true}, i.e., a fair coin flip.
		/// </summary>
		/// <returns>A new random sample.</returns>
		/// <remarks>
		/// Returns a sample the Bernoulli distribution with p = 0.5; also known as a a fair coin flip.
		/// </remarks>
		// Token: 0x06001036 RID: 4150 RVA: 0x000618CC File Offset: 0x0005FACC
		public bool NextBool()
		{
			return (this.NextULongInner() & 9223372036854775808UL) > 0UL;
		}

		/// <summary>
		/// Returns a random <see cref="T:System.Single" /> sampled from the uniform distribution with interval [0, 1),
		/// i.e., inclusive of 0.0 and exclusive of 1.0.
		/// </summary>
		/// <returns>A new random sample, of type <see cref="T:System.Single" />.</returns>
		// Token: 0x06001037 RID: 4151 RVA: 0x000618E2 File Offset: 0x0005FAE2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float NextFloat()
		{
			return (this.NextULongInner() >> 40) * 5.9604645E-08f;
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x000618F5 File Offset: 0x0005FAF5
		private int NextInner(int maxValue)
		{
			return (int)(this.NextDouble() * (double)maxValue);
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00061901 File Offset: 0x0005FB01
		private long NextInner(long maxValue)
		{
			return (long)(this.NextDouble() * (double)maxValue);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0006190D File Offset: 0x0005FB0D
		public void FillSerializableState(SerializableRng rng)
		{
			rng.state0 = this._s0;
			rng.state1 = this._s1;
			rng.state2 = this._s2;
			rng.state3 = this._s3;
		}

		// Token: 0x040005B8 RID: 1464
		private const double _incrDouble = 1.1102230246251565E-16;

		// Token: 0x040005B9 RID: 1465
		private const float _incrFloat = 5.9604645E-08f;

		// Token: 0x040005BA RID: 1466
		private ulong _s0;

		// Token: 0x040005BB RID: 1467
		private ulong _s1;

		// Token: 0x040005BC RID: 1468
		private ulong _s2;

		// Token: 0x040005BD RID: 1469
		private ulong _s3;
	}
}
